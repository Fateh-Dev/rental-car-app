import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api.service';
import { I18nService } from '../../services/i18n.service';
import { TranslatePipe } from '../../pipes/translate.pipe';

@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [CommonModule, FormsModule, TranslatePipe],
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {
  // Global configurations
  settings: any = {};
  
  // Local list variables parsed from JSON
  vehicleTypes: string[] = [];
  fuelTypes: string[] = [];
  maintenanceTypes: string[] = [];
  coverageTypes: string[] = [];
  extras: any[] = [];

  // Local additions variables
  newVehicleType = '';
  newFuelType = '';
  newMaintenanceType = '';
  newCoverageType = '';
  newExtra = { name: '', price: 0 };

  // Profile management
  profile: any = { username: '', fullName: '' };
  passwordForm = {
    currentPassword: '',
    newPassword: '',
    confirmPassword: ''
  };

  message = '';
  messageType = 'success';

  // Accordion state for reference data sections
  openRefSection: string | null = null;

  constructor(private api: ApiService, public i18n: I18nService) {}

  ngOnInit(): void {
    this.loadSettings();
    this.loadProfile();
  }

  toggleRefSection(section: string): void {
    this.openRefSection = this.openRefSection === section ? null : section;
  }

  loadSettings(): void {
    this.api.getSettings().subscribe({
      next: (res) => {
        this.settings = res;
        if (this.settings.currencySymbol) {
          localStorage.setItem('parc_auto_currency', this.settings.currencySymbol);
        }
        this.parseReferenceData();
      },
      error: (err) => console.error('Failed to load settings', err)
    });
  }

  loadProfile(): void {
    this.api.getProfile().subscribe({
      next: (res) => this.profile = res,
      error: (err) => console.error('Failed to load profile', err)
    });
  }

  parseReferenceData(): void {
    try {
      this.vehicleTypes = JSON.parse(this.settings.vehicleTypesJson || '[]');
      this.fuelTypes = JSON.parse(this.settings.fuelTypesJson || '[]');
      this.maintenanceTypes = JSON.parse(this.settings.maintenanceTypesJson || '[]');
      this.coverageTypes = JSON.parse(this.settings.coverageTypesJson || '[]');
      this.extras = JSON.parse(this.settings.extrasJson || '[]');
    } catch (e) {
      console.error('Failed to parse settings reference lists', e);
    }
  }

  saveSettings(): void {
    // Serialize local arrays back to JSON
    this.settings.vehicleTypesJson = JSON.stringify(this.vehicleTypes);
    this.settings.fuelTypesJson = JSON.stringify(this.fuelTypes);
    this.settings.maintenanceTypesJson = JSON.stringify(this.maintenanceTypes);
    this.settings.coverageTypesJson = JSON.stringify(this.coverageTypes);
    this.settings.extrasJson = JSON.stringify(this.extras);

    this.api.updateSettings(this.settings).subscribe({
      next: (res) => {
        this.settings = res;
        if (this.settings.currencySymbol) {
          localStorage.setItem('parc_auto_currency', this.settings.currencySymbol);
        }
        this.parseReferenceData();
        this.showFeedback(this.getFeedbackMsg('saveSuccess'), 'success');
      },
      error: (err) => this.showFeedback(this.api.getErrorMessage(err, this.getFeedbackMsg('saveError')), 'error')
    });
  }

  updateProfile(): void {
    this.api.updateProfile({ fullName: this.profile.fullName }).subscribe({
      next: (res) => {
        this.profile = res.user;
        this.showFeedback(this.getFeedbackMsg('profileSuccess'), 'success');
      },
      error: (err) => this.showFeedback(this.api.getErrorMessage(err, this.getFeedbackMsg('profileError')), 'error')
    });
  }

  updatePassword(): void {
    if (this.passwordForm.newPassword !== this.passwordForm.confirmPassword) {
      this.showFeedback(this.getFeedbackMsg('passwordMatchError'), 'error');
      return;
    }

    const payload = {
      currentPassword: this.passwordForm.currentPassword,
      newPassword: this.passwordForm.newPassword
    };

    this.api.changePassword(payload).subscribe({
      next: () => {
        this.showFeedback(this.getFeedbackMsg('passwordSuccess'), 'success');
        this.passwordForm = { currentPassword: '', newPassword: '', confirmPassword: '' };
      },
      error: (err) => this.showFeedback(this.api.getErrorMessage(err, this.getFeedbackMsg('passwordError')), 'error')
    });
  }

  // Helper additions & removals
  addVehicleType(): void {
    const val = this.newVehicleType.trim();
    if (val && !this.vehicleTypes.includes(val)) {
      this.vehicleTypes.push(val);
      this.newVehicleType = '';
    }
  }
  removeVehicleType(index: number): void {
    this.vehicleTypes.splice(index, 1);
  }

  addFuelType(): void {
    const val = this.newFuelType.trim();
    if (val && !this.fuelTypes.includes(val)) {
      this.fuelTypes.push(val);
      this.newFuelType = '';
    }
  }
  removeFuelType(index: number): void {
    this.fuelTypes.splice(index, 1);
  }

  addMaintenanceType(): void {
    const val = this.newMaintenanceType.trim();
    if (val && !this.maintenanceTypes.includes(val)) {
      this.maintenanceTypes.push(val);
      this.newMaintenanceType = '';
    }
  }
  removeMaintenanceType(index: number): void {
    this.maintenanceTypes.splice(index, 1);
  }

  addCoverageType(): void {
    const val = this.newCoverageType.trim();
    if (val && !this.coverageTypes.includes(val)) {
      this.coverageTypes.push(val);
      this.newCoverageType = '';
    }
  }
  removeCoverageType(index: number): void {
    this.coverageTypes.splice(index, 1);
  }

  addExtra(): void {
    const name = this.newExtra.name.trim();
    const price = this.newExtra.price;
    if (name && !this.extras.some(e => e.Name.toLowerCase() === name.toLowerCase())) {
      // API expects matching pascal case or camelcase properties
      this.extras.push({ Name: name, Price: price });
      this.newExtra = { name: '', price: 0 };
    }
  }
  removeExtra(index: number): void {
    this.extras.splice(index, 1);
  }

  showFeedback(text: string, type: 'success' | 'error'): void {
    this.message = text;
    this.messageType = type;
    setTimeout(() => {
      this.message = '';
    }, 4000);
  }

  getFeedbackMsg(key: string): string {
    const lang = this.i18n.currentLang();
    const msgs: Record<string, Record<string, string>> = {
      saveSuccess: {
        fr: 'Paramètres sauvegardés avec succès !',
        en: 'Settings saved successfully!',
        ar: 'تم حفظ الإعدادات بنجاح!'
      },
      saveError: {
        fr: 'Erreur lors de la sauvegarde des paramètres',
        en: 'Error saving settings',
        ar: 'خطأ أثناء حفظ الإعدادات'
      },
      profileSuccess: {
        fr: 'Profil mis à jour avec succès !',
        en: 'Profile updated successfully!',
        ar: 'تم تحديث الملف الشخصي بنجاح!'
      },
      profileError: {
        fr: 'Erreur lors de la mise à jour du profil',
        en: 'Error updating profile',
        ar: 'خطأ أثناء تحديث الملف الشخصي'
      },
      passwordMatchError: {
        fr: 'Les nouveaux mots de passe ne correspondent pas.',
        en: 'New passwords do not match.',
        ar: 'كلمات المرور الجديدة غير متطابقة.'
      },
      passwordSuccess: {
        fr: 'Mot de passe mis à jour avec succès !',
        en: 'Password updated successfully!',
        ar: 'تم تحديث كلمة المرور بنجاح!'
      },
      passwordError: {
        fr: 'Erreur lors de la mise à jour du mot de passe',
        en: 'Error updating password',
        ar: 'خطأ أثناء تحديث كلمة المرور'
      }
    };
    return msgs[key]?.[lang] || msgs[key]?.['fr'] || key;
  }
}
