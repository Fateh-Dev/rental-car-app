import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavigationEnd, NavigationStart, Router, RouterLink, RouterOutlet } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { filter, Subscription } from 'rxjs';

@Component({
  selector: 'app-app-layout',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterOutlet],
  templateUrl: './app-layout.component.html',
  styleUrls: ['./app-layout.component.css']
})
export class AppLayoutComponent implements OnInit, OnDestroy {
  adminName = 'Administrator';
  alertsCount = 0;
  criticalAlertsCount = 0;
  recentAlerts: any[] = [];
  showAlertsDropdown = false;
  sidebarOpen = typeof window !== 'undefined' ? window.innerWidth > 768 : true;
  private routerSub?: Subscription;

  menuItems = [
    { label: 'Dashboard', icon: 'pi pi-chart-bar', route: '/dashboard' },
    { label: 'Véhicules', icon: 'pi pi-car', route: '/vehicles' },
    { label: 'Clients', icon: 'pi pi-users', route: '/clients' },
    { label: 'Contrats', icon: 'pi pi-file', route: '/contracts' },
    { label: 'Kilométrage', icon: 'pi pi-map-marker', route: '/fuel' }, // grouped or split, let's link to fuel which has fuel logs, or km history
    { label: 'Maintenance', icon: 'pi pi-cog', route: '/maintenance' },
    { label: 'Consommables', icon: 'pi pi-wrench', route: '/consumables' },
    { label: 'Assurance & Contrôle', icon: 'pi pi-shield', route: '/insurance-inspections' },
    { label: 'Alertes', icon: 'pi pi-bell', route: '/alerts' },
    { label: 'Rapports & Analytics', icon: 'pi pi-print', route: '/reports' },
    { label: 'Configuration', icon: 'pi pi-sliders-h', route: '/settings' }
  ];

  constructor(private api: ApiService, private router: Router) {}

  ngOnInit(): void {
    const userJson = localStorage.getItem('parc_auto_user');
    if (userJson) {
      const user = JSON.parse(userJson);
      this.adminName = user.fullName || user.username;
    }
    this.loadAlerts();
    // Poll alerts every 60 seconds
    setInterval(() => this.loadAlerts(), 60000);

    this.routerSub = this.router.events.subscribe((event) => {
      if (event instanceof NavigationStart) {
        this.cleanupBlockingOverlays();
      }
      if (event instanceof NavigationEnd && window.innerWidth <= 768) {
        setTimeout(() => (this.sidebarOpen = false));
      }
    });
  }

  ngOnDestroy(): void {
    this.routerSub?.unsubscribe();
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    const target = event.target as HTMLElement;
    if (!target.closest('.alerts-wrapper')) {
      this.showAlertsDropdown = false;
    }
  }

  trackByRoute(_index: number, item: { route: string }): string {
    return item.route;
  }

  navigateTo(route: string, event: Event): void {
    event.preventDefault();
    this.showAlertsDropdown = false;
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
  }

  toggleSidebar(): void {
    setTimeout(() => {
      this.sidebarOpen = !this.sidebarOpen;
    });
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
