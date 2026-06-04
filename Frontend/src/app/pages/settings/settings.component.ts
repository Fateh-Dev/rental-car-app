import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [CommonModule, FormsModule],
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
  extras: any[] = [];

  // Local additions variables
  newVehicleType = '';
  newFuelType = '';
  newMaintenanceType = '';
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

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.loadSettings();
    this.loadProfile();
  }

  loadSettings(): void {
    this.api.getSettings().subscribe({
      next: (res) => {
        this.settings = res;
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
    this.settings.extrasJson = JSON.stringify(this.extras);

    this.api.updateSettings(this.settings).subscribe({
      next: (res) => {
        this.settings = res;
        this.parseReferenceData();
        this.showFeedback('Paramètres sauvegardés avec succès !', 'success');
      },
      error: (err) => this.showFeedback(err.error?.message || 'Erreur lors de la sauvegarde des paramètres', 'error')
    });
  }

  updateProfile(): void {
    this.api.updateProfile({ fullName: this.profile.fullName }).subscribe({
      next: (res) => {
        this.profile = res.user;
        this.showFeedback('Profil mis à jour avec succès !', 'success');
      },
      error: (err) => this.showFeedback(err.error?.message || 'Erreur lors de la mise à jour du profil', 'error')
    });
  }

  updatePassword(): void {
    if (this.passwordForm.newPassword !== this.passwordForm.confirmPassword) {
      this.showFeedback('Les nouveaux mots de passe ne correspondent pas.', 'error');
      return;
    }

    const payload = {
      currentPassword: this.passwordForm.currentPassword,
      newPassword: this.passwordForm.newPassword
    };

    this.api.changePassword(payload).subscribe({
      next: () => {
        this.showFeedback('Mot de passe mis à jour avec succès !', 'success');
        this.passwordForm = { currentPassword: '', newPassword: '', confirmPassword: '' };
      },
      error: (err) => this.showFeedback(err.error?.message || 'Erreur lors de la mise à jour du mot de passe', 'error')
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
}
