import { Component, OnInit, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api.service';
import { TableModule } from 'primeng/table';
import { ChartModule } from 'primeng/chart';
import { DatePickerModule } from 'primeng/datepicker';
import { I18nService } from '../../services/i18n.service';
import { TranslatePipe } from '../../pipes/translate.pipe';

@Component({
  selector: 'app-reports',
  standalone: true,
  imports: [CommonModule, FormsModule, TableModule, ChartModule, DatePickerModule, TranslatePipe],
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.css']
})
export class ReportsComponent implements OnInit {
  // Fleet status summary
  fleetStatus: any = { Total: 0, Available: 0, Rented: 0, InMaintenance: 0, Immobilized: 0, Reserved: 0 };
  fleetStatusChart: any = null;
  fleetStatusOptions: any = null;

  // Financial Revenue
  revenue: any = { totalRevenue: 0, paidRevenue: 0, unpaidRevenue: 0, byVehicle: [] };
  revenueChart: any = null;
  revenueOptions: any = null;

  // Profitability and ownership report
  profitabilityReport: any[] = [];
  averageUtilization = 0;

  // Top clients and unpaid contracts
  topClients: any[] = [];
  unpaidContracts: any[] = [];

  // Date range filters for revenue
  startDate: Date | null = null;
  endDate: Date | null = null;

  constructor(private api: ApiService, public i18n: I18nService) {
    effect(() => {
      this.i18n.currentLang();
      if (this.fleetStatus && this.fleetStatus.available > 0) {
        this.buildFleetStatusChart();
      }
      if (this.revenue && this.revenue.totalRevenue > 0) {
        this.buildRevenueChart();
      }
    });
  }

  ngOnInit(): void {
    this.loadFleetStatus();
    this.loadRevenue();
    this.loadProfitability();
    this.loadTopClients();
    this.loadUnpaidContracts();
  }

  loadFleetStatus(): void {
    this.api.getFleetStatus().subscribe({
      next: (res) => {
        this.fleetStatus = res;
        this.buildFleetStatusChart();
      },
      error: (err) => console.error('Failed to load fleet status', err)
    });
  }

  loadRevenue(): void {
    const startStr = this.startDate ? this.startDate.toISOString() : undefined;
    const endStr = this.endDate ? this.endDate.toISOString() : undefined;

    this.api.getRevenueReport(startStr, endStr).subscribe({
      next: (res) => {
        this.revenue = res;
        this.buildRevenueChart();
      },
      error: (err) => console.error('Failed to load revenue report', err)
    });
  }

  loadProfitability(): void {
    this.api.getProfitabilityReport().subscribe({
      next: (res) => {
        this.profitabilityReport = res;
        this.calculateAverageUtilization();
      },
      error: (err) => console.error('Failed to load profitability report', err)
    });
  }

  calculateAverageUtilization(): void {
    if (this.profitabilityReport.length === 0) {
      this.averageUtilization = 0;
      return;
    }
    const sum = this.profitabilityReport.reduce((acc, curr) => acc + (curr.utilizationRate || 0), 0);
    this.averageUtilization = sum / this.profitabilityReport.length;
  }

  loadTopClients(): void {
    this.api.getTopClients().subscribe({
      next: (res) => {
        this.topClients = res;
      },
      error: (err) => console.error('Failed to load top clients', err)
    });
  }

  loadUnpaidContracts(): void {
    this.api.getUnpaidContracts().subscribe({
      next: (res) => {
        this.unpaidContracts = res;
      },
      error: (err) => console.error('Failed to load unpaid contracts', err)
    });
  }

  exportProfitabilityCsv(): void {
    window.open(this.api.getProfitabilityCsvUrl());
  }

  buildFleetStatusChart(): void {
    this.fleetStatusChart = {
      labels: [
        this.i18n.t('statuses.available'),
        this.i18n.t('statuses.rented'),
        this.i18n.t('statuses.inMaintenance'),
        this.i18n.t('statuses.reserved'),
        this.i18n.t('statuses.immobilized')
      ],
      datasets: [
        {
          data: [
            this.fleetStatus.available,
            this.fleetStatus.rented,
            this.fleetStatus.inMaintenance,
            this.fleetStatus.reserved,
            this.fleetStatus.immobilized
          ],
          backgroundColor: ['#10b981', '#6366f1', '#f59e0b', '#3b82f6', '#ef4444'],
          hoverBackgroundColor: ['#059669', '#4f46e5', '#d97706', '#2563eb', '#dc2626']
        }
      ]
    };

    this.fleetStatusOptions = {
      cutout: '70%',
      plugins: {
        legend: {
          position: 'bottom',
          labels: {
            usePointStyle: true,
            boxWidth: 8
          }
        }
      }
    };
  }

  buildRevenueChart(): void {
    this.revenueChart = {
      labels: [this.i18n.t('reports.paidLabel'), this.i18n.t('reports.dueLabel')],
      datasets: [
        {
          data: [this.revenue.paidRevenue, this.revenue.unpaidRevenue],
          backgroundColor: ['#10b981', '#f43f5e'],
          hoverBackgroundColor: ['#059669', '#e11d48']
        }
      ]
    };

    this.revenueOptions = {
      cutout: '70%',
      plugins: {
        legend: {
          position: 'bottom',
          labels: {
            usePointStyle: true,
            boxWidth: 8
          }
        }
      }
    };
  }
}
