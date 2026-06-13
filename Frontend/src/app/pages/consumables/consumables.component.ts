import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api.service';
import { TableModule } from 'primeng/table';
import { DialogModule } from 'primeng/dialog';
import { DatePickerModule } from 'primeng/datepicker';
import { I18nService } from '../../services/i18n.service';
import { TranslatePipe } from '../../pipes/translate.pipe';
import { ConfirmDialogService } from '../../services/confirm-dialog.service';

@Component({
  selector: 'app-consumables',
  standalone: true,
  imports: [CommonModule, FormsModule, TableModule, DialogModule, DatePickerModule, TranslatePipe],
  templateUrl: './consumables.component.html',
  styleUrls: ['./consumables.component.css']
})
export class ConsumablesComponent implements OnInit {
  vehicles: any[] = [];
  selectedVehicleId: number | null = null;
  consumableStatusReport: any[] = [];
  currentVehicleOdometer = 0;

  // Replacement dialog CRUD
  showLogDialog = false;
  logForm: any = this.getEmptyLogForm();

  // Consumables categories configs
  configs: any[] = [];
  showConfigDialog = false;
  selectedConfig: any = { consumableType: '', intervalKm: 10000, intervalMonths: 12 };

  consumableTypes: string[] = [
    'OilChange',
    'AirFilter',
    'OilFilter',
    'FuelFilter',
    'CabinFilter',
    'FrontBrakes',
    'RearBrakes',
    'FrontTires',
    'RearTires',
    'Battery'
  ];

  constructor(private api: ApiService, public i18n: I18nService, private confirmService: ConfirmDialogService) {}

  ngOnInit(): void {
    this.loadVehicles();
    this.loadConfigs();
  }

  loadVehicles(): void {
    this.api.getVehicles('', '', '', '', 1, 100).subscribe({
      next: (res) => {
        this.vehicles = res.data;
        if (this.vehicles.length > 0) {
          this.selectedVehicleId = this.vehicles[0].id;
          this.onVehicleSelect();
        }
      }
    });
  }

  loadConfigs(): void {
    this.api.getConsumableConfigs().subscribe(res => {
      this.configs = res;
      // Add custom user-defined config types to choice list if any
      res.forEach((c: any) => {
        if (!this.consumableTypes.includes(c.consumableType)) {
          this.consumableTypes.push(c.consumableType);
        }
      });
    });
  }

  onVehicleSelect(): void {
    if (!this.selectedVehicleId) return;

    const vehicle = this.vehicles.find(v => v.id === this.selectedVehicleId);
    if (vehicle) {
      this.currentVehicleOdometer = vehicle.currentKm;
    }

    this.api.getVehicleConsumablesStatus(this.selectedVehicleId).subscribe({
      next: (res) => {
        this.consumableStatusReport = res.statusReport;
      },
      error: (err) => console.error('Failed to load consumable status', err)
    });
  }

  getEmptyLogForm(): any {
    return {
      consumableType: 'OilChange',
      replacementDate: new Date(),
      replacementKm: 0,
      oilType: 'Synthetic',
      viscosity: '5W-40',
      brand: '',
      size: '',
      typeDetail: '',
      axle: 'Front',
      notes: ''
    };
  }

  openLogDialog(type?: string): void {
    if (!this.selectedVehicleId) {
      this.confirmService.alert({ title: 'Information', message: this.i18n.t('consumables.selectVehiclePrompt'), type: 'info', icon: 'pi pi-info-circle' });
      return;
    }

    this.logForm = this.getEmptyLogForm();
    this.logForm.replacementKm = this.currentVehicleOdometer;
    if (type) {
      this.logForm.consumableType = type;
    }
    this.showLogDialog = true;
  }

  onSubmitLog(): void {
    const payload = {
      ...this.logForm,
      vehicleId: this.selectedVehicleId,
      replacementDate: this.logForm.replacementDate.toISOString()
    };

    this.api.addConsumableLog(payload).subscribe({
      next: () => {
        this.showLogDialog = false;
        this.onVehicleSelect(); // Reload status
        // Update local vehicles list odometer if replacement km was higher
        const v = this.vehicles.find(x => x.id === this.selectedVehicleId);
        if (v && payload.replacementKm > v.currentKm) {
          v.currentKm = payload.replacementKm;
          this.currentVehicleOdometer = payload.replacementKm;
        }
      },
      error: (err) => this.confirmService.alert({ title: 'Error', message: this.api.getErrorMessage(err, this.i18n.t('vehicles.errorCreate')), type: 'danger', icon: 'pi pi-times-circle' })
    });
  }

  // Configurations CRUD
  openConfigDialog(config?: any): void {
    if (config) {
      this.selectedConfig = { ...config };
    } else {
      this.selectedConfig = { consumableType: '', intervalKm: 0, intervalMonths: 0 };
    }
    this.showConfigDialog = true;
  }

  onSubmitConfig(): void {
    if (!this.selectedConfig.consumableType) {
      this.confirmService.alert({ title: 'Erreur', message: 'Veuillez spécifier le type de consommable.', type: 'danger', icon: 'pi pi-times-circle' });
      return;
    }

    this.api.saveConsumableConfig(this.selectedConfig).subscribe({
      next: () => {
        this.showConfigDialog = false;
        this.loadConfigs();
        if (this.selectedVehicleId) this.onVehicleSelect();
      },
      error: (err) => console.error('Failed to save config', err)
    });
  }
}
