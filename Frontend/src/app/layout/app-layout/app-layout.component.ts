import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavigationEnd, NavigationStart, Router, RouterLink, RouterOutlet } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { I18nService, Lang } from '../../services/i18n.service';
import { TranslatePipe } from '../../pipes/translate.pipe';
import { filter, Subscription } from 'rxjs';
import { ConfirmDialogComponent } from '../../shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-app-layout',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterOutlet, TranslatePipe, ConfirmDialogComponent],
  templateUrl: './app-layout.component.html',
  styleUrls: ['./app-layout.component.css']
})
export class AppLayoutComponent implements OnInit, OnDestroy {
  adminName = 'Administrator';
  alertsCount = 0;
  criticalAlertsCount = 0;
  recentAlerts: any[] = [];
  showAlertsDropdown = false;
  showLangDropdown = false;
  sidebarOpen = typeof window !== 'undefined' ? window.innerWidth > 768 : true;
  sidebarCollapsed = false;
  private routerSub?: Subscription;
  private alertInterval?: ReturnType<typeof setInterval>;

  menuItems = [
    { labelKey: 'sidebar.dashboard', icon: 'pi pi-chart-bar', route: '/dashboard' },
    { labelKey: 'sidebar.vehicles', icon: 'pi pi-car', route: '/vehicles' },
    { labelKey: 'sidebar.clients', icon: 'pi pi-users', route: '/clients' },
    { labelKey: 'sidebar.contracts', icon: 'pi pi-file', route: '/contracts' },
    { labelKey: 'sidebar.kilometrage', icon: 'pi pi-map-marker', route: '/fuel' },
    { labelKey: 'sidebar.maintenance', icon: 'pi pi-cog', route: '/maintenance' },
    { labelKey: 'sidebar.consumables', icon: 'pi pi-wrench', route: '/consumables' },
    { labelKey: 'sidebar.insuranceControl', icon: 'pi pi-shield', route: '/insurance-inspections' },
    { labelKey: 'sidebar.alerts', icon: 'pi pi-bell', route: '/alerts' },
    { labelKey: 'sidebar.reportsAnalytics', icon: 'pi pi-print', route: '/reports' },
    { labelKey: 'sidebar.users', icon: 'pi pi-user', route: '/users' },
    { labelKey: 'sidebar.settings', icon: 'pi pi-sliders-h', route: '/settings' }
  ];

  languages: { code: Lang; label: string; flag: string }[] = [];

  constructor(private api: ApiService, private router: Router, public i18n: I18nService) {
    this.languages = this.i18n.getLanguages();
    // Restore collapsed state from localStorage
    if (typeof window !== 'undefined') {
      const saved = localStorage.getItem('parc_auto_sidebar_collapsed');
      if (saved === 'true') {
        this.sidebarCollapsed = true;
      }
    }
  }

  ngOnInit(): void {
    const userJson = localStorage.getItem('parc_auto_user');
    if (userJson) {
      const user = JSON.parse(userJson);
      this.adminName = user.fullName || user.username;
    }
    this.loadAlerts();
    // Poll alerts every 60 seconds
    this.alertInterval = setInterval(() => this.loadAlerts(), 60000);

    this.routerSub = this.router.events.subscribe((event) => {
      if (event instanceof NavigationStart) {
        this.cleanupBlockingOverlays();
      }
      if (event instanceof NavigationEnd) {
        if (window.innerWidth <= 768) {
          setTimeout(() => (this.sidebarOpen = false));
        }
      }
    });
  }

  ngOnDestroy(): void {
    this.routerSub?.unsubscribe();
    if (this.alertInterval) {
      clearInterval(this.alertInterval);
    }
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    const target = event.target as HTMLElement;
    if (!target.closest('.alerts-wrapper')) {
      this.showAlertsDropdown = false;
    }
    if (!target.closest('.lang-wrapper')) {
      this.showLangDropdown = false;
    }
  }

  trackByRoute(_index: number, item: { route: string }): string {
    return item.route;
  }

  navigateTo(route: string, event: Event): void {
    event.preventDefault();
    this.showAlertsDropdown = false;
    this.showLangDropdown = false;
    const currentPath = this.router.url.split('?')[0];
    if (currentPath === route) {
      if (window.innerWidth <= 768) {
        setTimeout(() => (this.sidebarOpen = false));
      }
      return;
    }
    void this.router.navigateByUrl(route);
  }

  private cleanupBlockingOverlays(): void {
    this.showAlertsDropdown = false;
    this.showLangDropdown = false;
    document.body.classList.remove('p-overflow-hidden');
    document.querySelectorAll('.p-dialog-mask, .p-overlay-mask, .p-component-overlay').forEach((el) => {
      el.remove();
    });
  }

  loadAlerts(): void {
    this.api.getAlerts().subscribe({
      next: (res) => {
        this.alertsCount = res.count;
        this.criticalAlertsCount = res.criticalCount;
        this.recentAlerts = res.alerts.slice(0, 5); // Keep top 5
      },
      error: (err) => console.error('Failed to load alerts', err)
    });
  }

  toggleAlertsDropdown(): void {
    this.showAlertsDropdown = !this.showAlertsDropdown;
    this.showLangDropdown = false;
  }

  toggleLangDropdown(): void {
    this.showLangDropdown = !this.showLangDropdown;
    this.showAlertsDropdown = false;
  }

  switchLang(lang: Lang): void {
    this.i18n.setLang(lang);
    this.showLangDropdown = false;
  }

  toggleSidebar(): void {
    setTimeout(() => {
      this.sidebarOpen = !this.sidebarOpen;
    });
  }

  toggleCollapse(): void {
    this.sidebarCollapsed = !this.sidebarCollapsed;
    localStorage.setItem('parc_auto_sidebar_collapsed', String(this.sidebarCollapsed));
  }

  closeSidebar(): void {
    setTimeout(() => {
      this.sidebarOpen = false;
    });
  }

  logout(): void {
    localStorage.removeItem('parc_auto_token');
    localStorage.removeItem('parc_auto_user');
    this.router.navigate(['/login']);
  }

  isLinkActive(route: string): boolean {
    if (route === '/dashboard') {
      return this.router.url === '/dashboard' || this.router.url === '/';
    }
    return this.router.url.startsWith(route);
  }
}
