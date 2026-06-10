import { Component, OnInit, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { I18nService } from '../../services/i18n.service';
import { TranslatePipe } from '../../pipes/translate.pipe';
import { AppCurrencyPipe } from '../../pipes/app-currency.pipe';
import { ChartModule } from 'primeng/chart';
import { TableModule } from 'primeng/table';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink, ChartModule, TableModule, TranslatePipe, AppCurrencyPipe],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  today = new Date();

  stats: any = {
    total: 0,
    available: 0,
    rented: 0,
    inMaintenance: 0,
    reserved: 0,
    immobilized: 0
  };

  revenue: any = {
    totalRevenue: 0,
    paidRevenue: 0,
    unpaidRevenue: 0
  };

  unpaidContracts: any[] = [];
  alerts: any[] = [];
  criticalAlertsCount = 0;

  // Chart Data
  chartData: any;
  chartOptions: any;

  constructor(private api: ApiService, public i18n: I18nService) {
    effect(() => {
      // Re-initialize chart when language changes
      this.i18n.currentLang();
      if (this.stats && this.stats.total > 0) {
        this.initChart(this.stats);
      }
    });
  }

  ngOnInit(): void {
    this.loadFleetStatus();
    this.loadRevenue();
    this.loadUnpaidContracts();
    this.loadAlerts();
  }

  loadFleetStatus(): void {
    this.api.getFleetStatus().subscribe({
      next: (res) => {
        this.stats = res;
        this.initChart(res);
      },
      error: (err) => console.error('Failed to load fleet status', err)
    });
  }

  loadRevenue(): void {
    this.api.getRevenueReport().subscribe({
      next: (res) => {
        this.revenue = res;
      },
      error: (err) => console.error('Failed to load revenue', err)
    });
  }

  loadUnpaidContracts(): void {
    this.api.getUnpaidContracts().subscribe({
      next: (res) => {
        this.unpaidContracts = res.slice(0, 5);
      },
      error: (err) => console.error('Failed to load unpaid contracts', err)
    });
  }

  loadAlerts(): void {
    this.api.getAlerts().subscribe({
      next: (res) => {
        this.alerts = res.alerts.slice(0, 5);
        this.criticalAlertsCount = res.criticalCount;
      },
      error: (err) => console.error('Failed to load alerts', err)
    });
  }

  initChart(stats: any): void {
    const textColor = '#334155';

    this.chartData = {
      labels: [
        this.i18n.t('statuses.available'),
        this.i18n.t('statuses.rented'),
        this.i18n.t('statuses.inMaintenance'),
        this.i18n.t('statuses.reserved'),
        this.i18n.t('statuses.immobilized')
      ],
      datasets: [
        {
          data: [stats.available, stats.rented, stats.inMaintenance, stats.reserved, stats.immobilized],
          backgroundColor: [
            '#10b981', // green-500
            '#6366f1', // indigo-500
            '#f59e0b', // amber-500
            '#3b82f6', // blue-500
            '#ef4444'  // red-500
          ],
          hoverBackgroundColor: [
            '#059669',
            '#4f46e5',
            '#d97706',
            '#2563eb',
            '#dc2626'
          ]
        }
      ]
    };

    this.chartOptions = {
      cutout: '60%',
      plugins: {
        legend: {
          labels: {
            color: textColor,
            font: {
              weight: '600'
            }
          },
          position: 'bottom'
        }
      }
    };
  }
}
