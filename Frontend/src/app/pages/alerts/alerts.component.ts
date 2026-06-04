import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { I18nService } from '../../services/i18n.service';
import { TranslatePipe } from '../../pipes/translate.pipe';

@Component({
  selector: 'app-alerts',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, TranslatePipe],
  templateUrl: './alerts.component.html',
  styleUrls: ['./alerts.component.css']
})
export class AlertsComponent implements OnInit {
  alerts: any[] = [];
  filteredAlerts: any[] = [];
  
  // Count stats
  criticalCount = 0;
  warningCount = 0;
  infoCount = 0;

  // Filter criteria
  selectedSeverity = 'ALL';
  selectedType = 'ALL';
  searchTerm = '';

  // Pagination
  page = 1;
  pageSize = 10;
  pages: number[] = [];
  displayedAlerts: any[] = [];

  constructor(private api: ApiService, public i18n: I18nService) {}

  ngOnInit(): void {
    this.loadAlerts();
  }

  loadAlerts(): void {
    this.api.getAlerts().subscribe({
      next: (res) => {
        this.alerts = res.alerts;
        this.criticalCount = res.criticalCount;
        this.warningCount = res.warningCount;
        this.infoCount = res.infoCount;
        this.applyFilters();
      },
      error: (err) => console.error('Failed to load system alerts', err)
    });
  }

  get totalPages(): number {
    return Math.max(1, Math.ceil(this.filteredAlerts.length / this.pageSize));
  }

  applyFilters(): void {
    this.filteredAlerts = this.alerts.filter(alert => {
      const matchesSeverity = this.selectedSeverity === 'ALL' || alert.severity.toUpperCase() === this.selectedSeverity;
      const matchesType = this.selectedType === 'ALL' || alert.type.toUpperCase() === this.selectedType.toUpperCase();
      const search = this.searchTerm.toLowerCase();
      const matchesSearch = !search ||
        alert.target.toLowerCase().includes(search) ||
        alert.message.toLowerCase().includes(search) ||
        alert.type.toLowerCase().includes(search);

      return matchesSeverity && matchesType && matchesSearch;
    });
    this.page = 1;
    this.updatePagination();
  }

  onPageChange(page: number): void {
    if (page < 1 || page > this.totalPages) return;
    this.page = page;
    this.updateDisplayedAlerts();
  }

  private updatePagination(): void {
    const totalPages = this.totalPages;
    this.pages = [];
    for (let i = 1; i <= totalPages; i++) {
      this.pages.push(i);
    }
    this.updateDisplayedAlerts();
  }

  private updateDisplayedAlerts(): void {
    const start = (this.page - 1) * this.pageSize;
    this.displayedAlerts = this.filteredAlerts.slice(start, start + this.pageSize);
  }

  getAlertIcon(type: string): string {
    switch (type.toLowerCase()) {
      case 'insurance':
        return 'pi-shield';
      case 'inspection':
        return 'pi-check-circle';
      case 'maintenance':
        return 'pi-cog';
      case 'odometerinactivity':
        return 'pi-history';
      case 'driverlicense':
        return 'pi-id-card';
      case 'consumable':
        return 'pi-wrench';
      default:
        return 'pi-bell';
    }
  }

  getAlertBgClass(type: string): string {
    switch (type.toLowerCase()) {
      case 'insurance': return 'bg-blue-100 text-blue-800 dark:bg-blue-950/40 dark:text-blue-400 border-blue-200 dark:border-blue-900';
      case 'inspection': return 'bg-emerald-100 text-emerald-800 dark:bg-emerald-950/40 dark:text-emerald-400 border-emerald-200 dark:border-emerald-900';
      case 'maintenance': return 'bg-orange-100 text-orange-800 dark:bg-orange-950/40 dark:text-orange-400 border-orange-200 dark:border-orange-900';
      case 'odometerinactivity': return 'bg-slate-100 text-slate-800 dark:bg-slate-800 dark:text-slate-300 border-slate-200 dark:border-slate-700';
      case 'driverlicense': return 'bg-purple-100 text-purple-800 dark:bg-purple-950/40 dark:text-purple-400 border-purple-200 dark:border-purple-900';
      case 'consumable': return 'bg-cyan-100 text-cyan-800 dark:bg-cyan-950/40 dark:text-cyan-400 border-cyan-200 dark:border-cyan-900';
      default: return 'bg-slate-100 text-slate-800 border-slate-200';
    }
  }

  getSeverityBadge(severity: string): string {
    switch (severity.toLowerCase()) {
      case 'critical':
        return 'bg-red-100 text-red-800 dark:bg-red-950/40 dark:text-red-400 border-red-200 dark:border-red-900';
      case 'warning':
        return 'bg-amber-100 text-amber-800 dark:bg-amber-950/40 dark:text-amber-400 border-amber-200 dark:border-amber-900';
      default:
        return 'bg-blue-100 text-blue-800 dark:bg-blue-950/40 dark:text-blue-400 border-blue-200 dark:border-blue-900';
    }
  }
}
