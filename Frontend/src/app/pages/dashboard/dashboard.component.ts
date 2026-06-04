import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { ChartModule } from 'primeng/chart';
import { TableModule } from 'primeng/table';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink, ChartModule, TableModule],
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

  constructor(private api: ApiService) {}

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
        this.unpaidContracts = res.slice(0, 5); // show top 5 unpaid contracts
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
    const documentStyle = getComputedStyle(document.documentElement);
    const textColor = '#334155';

    this.chartData = {
      labels: ['Disponible', 'Loué', 'En Maintenance', 'Réservé', 'Immobilisé'],
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
