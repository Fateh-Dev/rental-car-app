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
  selector: 'app-fuel',
  standalone: true,
  imports: [CommonModule, FormsModule, TableModule, DialogModule, DatePickerModule, TranslatePipe],
  templateUrl: './fuel.component.html',
  styleUrls: ['./fuel.component.css']
})
export class FuelComponent implements OnInit {
  vehiclesList: any[] = [];
  inactiveVehicles: any[] = [];
  inactivityThresholdDays = 15;

  // Selected vehicle state
  selectedVehicle: any = null;
  kmHistory: any[] = [];

  // Dialog states & forms
  showAddKmDialog = false;
  kmForm = this.getEmptyKmForm();

  constructor(private api: ApiService, public i18n: I18nService, private confirmService: ConfirmDialogService) {}

  ngOnInit(): void {
    this.loadVehicles();
    this.loadInactivityReport();
  }

  loadVehicles(): void {
    this.api.getVehicles('', '', '', '', 1, 100).subscribe({
      next: (res) => {
        this.vehiclesList = res.data;
        // If a vehicle was previously selected, refresh its details
        if (this.selectedVehicle) {
          const updated = this.vehiclesList.find(v => v.id === this.selectedVehicle.id);
          if (updated) {
            this.selectedVehicle = updated;
          }
        }
      },
      error: (err) => console.error('Failed to load fleet mileage', err)
    });
  }

  loadInactivityReport(): void {
    this.api.getKmInactivityReport().subscribe({
      next: (res) => {
        this.inactiveVehicles = res.vehicles;
        this.inactivityThresholdDays = res.thresholdDays;
      },
      error: (err) => console.error('Failed to load inactivity report', err)
    });
  }

  selectVehicle(vehicle: any): void {
    this.selectedVehicle = vehicle;
    this.loadVehicleKmHistory(vehicle.id);
  }

  loadVehicleKmHistory(vehicleId: number): void {
    this.api.getKmHistory(vehicleId).subscribe({
      next: (res) => {
        this.kmHistory = res;
      },
      error: (err) => console.error('Failed to load km history', err)
    });
  }

  getEmptyKmForm(): any {
    return {
      vehicleId: null,
      date: new Date(),
      kmValue: 0,
      notes: 'Saisie manuelle odomètre (Vérification périodique).'
    };
  }

  openAddKm(vehicle: any, event?: Event): void {
    if (event) {
      event.stopPropagation(); // Avoid selecting the vehicle row on button click
    }
    this.kmForm = this.getEmptyKmForm();
    this.kmForm.vehicleId = vehicle.id;
    this.kmForm.kmValue = vehicle.currentKm;
    this.showAddKmDialog = true;
  }

  submitKm(): void {
    const payload = {
      ...this.kmForm,
      date: this.kmForm.date.toISOString()
    };

    this.api.addKmManualEntry(payload).subscribe({
      next: () => {
        this.showAddKmDialog = false;
        this.loadVehicles();
        this.loadInactivityReport();
        if (this.selectedVehicle && this.selectedVehicle.id === payload.vehicleId) {
          this.loadVehicleKmHistory(payload.vehicleId);
        }
      },
      error: (err) => this.confirmService.alert({ title: 'Error', message: this.api.getErrorMessage(err, this.i18n.t('common.errorOccurred')), type: 'danger', icon: 'pi pi-times-circle' })
    });
  }

  deleteKmEntry(id: number): void {
    this.confirmService.confirm({
      title: this.i18n.t('common.delete'),
      message: this.i18n.t('common.deleteConfirm'),
      type: 'danger',
      icon: 'pi pi-trash'
    }).then(confirmed => {
      if (confirmed) {
        this.api.deleteKmEntry(id).subscribe({
          next: () => {
            this.loadVehicles();
            this.loadInactivityReport();
            if (this.selectedVehicle) {
              this.loadVehicleKmHistory(this.selectedVehicle.id);
            }
          },
          error: (err) => this.confirmService.alert({ title: 'Error', message: this.api.getErrorMessage(err, this.i18n.t('common.errorOccurred')), type: 'danger', icon: 'pi pi-times-circle' })
        });
      }
    });
  }

  exportAllKmCsv(): void {
    window.open(this.api.getKmExportCsvUrl());
  }

  exportVehicleKmCsv(): void {
    if (this.selectedVehicle) {
      window.open(this.api.getKmExportCsvUrl(this.selectedVehicle.id));
    }
  }
}
