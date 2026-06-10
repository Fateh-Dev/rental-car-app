import { Component, OnInit, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api.service';
import { TableModule } from 'primeng/table';
import { DialogModule } from 'primeng/dialog';
import { DatePickerModule } from 'primeng/datepicker';
import { ChartModule } from 'primeng/chart';
import { I18nService } from '../../services/i18n.service';
import { TranslatePipe } from '../../pipes/translate.pipe';
import { AppCurrencyPipe } from '../../pipes/app-currency.pipe';

@Component({
  selector: 'app-fuel',
  standalone: true,
  imports: [CommonModule, FormsModule, TableModule, DialogModule, DatePickerModule, ChartModule, TranslatePipe, AppCurrencyPipe],
  templateUrl: './fuel.component.html',
  styleUrls: ['./fuel.component.css']
})
export class FuelComponent implements OnInit {
  vehiclesList: any[] = [];
  inactiveVehicles: any[] = [];
  inactivityThresholdDays = 15;

  // Selected vehicle state
  selectedVehicle: any = null;
  fuelLogs: any[] = [];
  kmHistory: any[] = [];

  // Chart data
  chartData: any = null;
  chartOptions: any = null;

  // Dialog states & forms
  showAddKmDialog = false;
  kmForm = this.getEmptyKmForm();

  showAddFuelDialog = false;
  fuelForm = this.getEmptyFuelForm();

  constructor(private api: ApiService, public i18n: I18nService) {
    effect(() => {
      this.i18n.currentLang();
      if (this.fuelLogs && this.fuelLogs.length >= 2) {
        this.buildChart();
      }
    });
  }

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
    this.loadVehicleLogs(vehicle.id);
    this.loadVehicleKmHistory(vehicle.id);
  }

  loadVehicleLogs(vehicleId: number): void {
    this.api.getFuelLogs(vehicleId).subscribe({
      next: (res) => {
        this.fuelLogs = res;
        this.buildChart();
      },
      error: (err) => console.error('Failed to load fuel logs', err)
    });
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

  getEmptyFuelForm(): any {
    return {
      vehicleId: null,
      date: new Date(),
      kmValue: 0,
      liters: 0,
      costPerLiter: 0,
      stationName: '',
      fuelType: ''
    };
  }

  openAddKm(vehicle: any, event: Event): void {
    event.stopPropagation(); // Avoid selecting the vehicle row on button click
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
      error: (err) => alert(err.error?.message || this.i18n.t('common.errorOccurred'))
    });
  }

  openAddFuel(): void {
    if (!this.selectedVehicle) return;
    this.fuelForm = this.getEmptyFuelForm();
    this.fuelForm.vehicleId = this.selectedVehicle.id;
    this.fuelForm.kmValue = this.selectedVehicle.currentKm;
    this.fuelForm.fuelType = this.selectedVehicle.fuelType || 'Gasoline';
    this.showAddFuelDialog = true;
  }

  submitFuel(): void {
    const payload = {
      ...this.fuelForm,
      date: this.fuelForm.date.toISOString()
    };

    this.api.addFuelLog(payload).subscribe({
      next: () => {
        this.showAddFuelDialog = false;
        this.loadVehicles();
        this.loadInactivityReport();
        this.loadVehicleLogs(payload.vehicleId);
        this.loadVehicleKmHistory(payload.vehicleId);
      },
      error: (err) => alert(err.error?.message || this.i18n.t('common.errorOccurred'))
    });
  }

  deleteFuelLog(id: number): void {
    if (confirm(this.i18n.t('common.deleteConfirm'))) {
      this.api.deleteFuelLog(id).subscribe({
        next: () => {
          if (this.selectedVehicle) {
            this.loadVehicleLogs(this.selectedVehicle.id);
          }
        },
        error: (err) => alert(err.error?.message || this.i18n.t('common.errorOccurred'))
      });
    }
  }

  exportAllKmCsv(): void {
    window.open(this.api.getKmExportCsvUrl());
  }

  exportVehicleKmCsv(): void {
    if (this.selectedVehicle) {
      window.open(this.api.getKmExportCsvUrl(this.selectedVehicle.id));
    }
  }

  exportAllFuelCsv(): void {
    window.open(this.api.getFuelExportCsvUrl());
  }

  exportVehicleFuelCsv(): void {
    if (this.selectedVehicle) {
      window.open(this.api.getFuelExportCsvUrl(this.selectedVehicle.id));
    }
  }

  buildChart(): void {
    // Need at least 2 logs to show a line chart trend
    if (this.fuelLogs.length < 2) {
      this.chartData = null;
      return;
    }

    // Chronological order for line chart
    const sorted = [...this.fuelLogs]
      .filter(l => l.kmDrivenSinceLastFill > 0)
      .reverse();

    if (sorted.length === 0) {
      this.chartData = null;
      return;
    }

    const labels = sorted.map(l => new Date(l.log.date).toLocaleDateString(this.i18n.currentLang() === 'ar' ? 'ar-SA' : this.i18n.currentLang() === 'fr' ? 'fr-FR' : 'en-US'));
    const data = sorted.map(l => l.consumptionL100);

    this.chartData = {
      labels: labels,
      datasets: [
        {
          label: this.i18n.t('fuel.consumptionTrend'),
          data: data,
          fill: true,
          borderColor: '#4f46e5', // indigo-600
          tension: 0.4,
          backgroundColor: 'rgba(79, 70, 229, 0.1)'
        }
      ]
    };

    this.chartOptions = {
      plugins: {
        legend: {
          display: false
        }
      },
      scales: {
        y: {
          beginAtZero: true,
          title: {
            display: true,
            text: 'L/100km',
            color: '#475569'
          }
        }
      }
    };
  }
}
