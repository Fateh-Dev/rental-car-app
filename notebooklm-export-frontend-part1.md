# Parc Auto - Frontend (Angular) - Part 1

## Architecture: Angular 19 SPA with standalone components

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\main.ts
```typescript
import 'zone.js';
import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { App } from './app/app';

bootstrapApplication(App, appConfig)
  .catch((err) => console.error(err));

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\app.config.ts
```typescript
import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter, withPreloading, PreloadAllModules } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';

import { routes } from './app.routes';
import { authInterceptor } from './interceptors/auth.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes, withPreloading(PreloadAllModules)),
    provideHttpClient(withInterceptors([authInterceptor])),
    provideAnimationsAsync()
  ]
};

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\app.routes.ts
```typescript
import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { 
    path: 'login', 
    loadComponent: () => import('./pages/login/login.component').then(m => m.LoginComponent) 
  },
  {
    path: '',
    canActivate: [authGuard],
    loadComponent: () => import('./layout/app-layout/app-layout.component').then(m => m.AppLayoutComponent),
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { 
        path: 'dashboard', 
        loadComponent: () => import('./pages/dashboard/dashboard.component').then(m => m.DashboardComponent) 
      },
      { 
        path: 'vehicles', 
        loadComponent: () => import('./pages/vehicles/vehicles.component').then(m => m.VehiclesComponent) 
      },
      { 
        path: 'clients', 
        loadComponent: () => import('./pages/clients/clients.component').then(m => m.ClientsComponent) 
      },
      { 
        path: 'contracts', 
        loadComponent: () => import('./pages/contracts/contracts.component').then(m => m.ContractsComponent) 
      },
      { 
        path: 'maintenance', 
        loadComponent: () => import('./pages/maintenance/maintenance.component').then(m => m.MaintenanceComponent) 
      },
      { 
        path: 'consumables', 
        loadComponent: () => import('./pages/consumables/consumables.component').then(m => m.ConsumablesComponent) 
      },
      { 
        path: 'insurance-inspections', 
        loadComponent: () => import('./pages/insurance-inspections/insurance-inspections.component').then(m => m.InsuranceInspectionsComponent) 
      },
      { 
        path: 'fuel', 
        loadComponent: () => import('./pages/fuel/fuel.component').then(m => m.FuelComponent) 
      },
      { 
        path: 'alerts', 
        loadComponent: () => import('./pages/alerts/alerts.component').then(m => m.AlertsComponent) 
      },
      { 
        path: 'reports', 
        loadComponent: () => import('./pages/reports/reports.component').then(m => m.ReportsComponent) 
      },
      { 
        path: 'settings', 
        loadComponent: () => import('./pages/settings/settings.component').then(m => m.SettingsComponent) 
      }
    ]
  },
  { path: '**', redirectTo: 'dashboard' }
];

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\app.ts
```typescript
import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('Frontend');
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\guards\auth.guard.ts
```typescript
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const token = localStorage.getItem('parc_auto_token');

  if (token) {
    return true;
  }

  // Not authenticated, redirect to login
  router.navigate(['/login']);
  return false;
};

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\i18n\ar.ts
```typescript
export const AR: Record<string, any> = {
  // â”€â”€â”€ Common / Shared â”€â”€â”€
  common: {
    save: 'Ø­ÙØ¸',
    cancel: 'Ø¥Ù„ØºØ§Ø¡',
    delete: 'Ø­Ø°Ù',
    edit: 'ØªØ¹Ø¯ÙŠÙ„',
    add: 'Ø¥Ø¶Ø§ÙØ©',
    close: 'Ø¥ØºÙ„Ø§Ù‚',
    search: 'Ø¨Ø­Ø«',
    actions: 'Ø§Ù„Ø¥Ø¬Ø±Ø§Ø¡Ø§Øª',
    status: 'Ø§Ù„Ø­Ø§Ù„Ø©',
    date: 'Ø§Ù„ØªØ§Ø±ÙŠØ®',
    notes: 'Ù…Ù„Ø§Ø­Ø¸Ø§Øª',
    remarks: 'Ù…Ù„Ø§Ø­Ø¸Ø§Øª',
    yes: 'Ù†Ø¹Ù…',
    no: 'Ù„Ø§',
    confirm: 'ØªØ£ÙƒÙŠØ¯',
    loading: 'Ø¬Ø§Ø±ÙŠ Ø§Ù„ØªØ­Ù…ÙŠÙ„...',
    uploading: 'Ø¬Ø§Ø±ÙŠ Ø§Ù„Ø±ÙØ¹...',
    noData: 'Ù„Ø§ ØªÙˆØ¬Ø¯ Ø¨ÙŠØ§Ù†Ø§Øª',
    all: 'Ø§Ù„ÙƒÙ„',
    viewAll: 'Ø¹Ø±Ø¶ Ø§Ù„ÙƒÙ„',
    export: 'ØªØµØ¯ÙŠØ±',
    print: 'Ø·Ø¨Ø§Ø¹Ø©',
    refresh: 'ØªØ­Ø¯ÙŠØ«',
    back: 'Ø±Ø¬ÙˆØ¹',
    next: 'Ø§Ù„ØªØ§Ù„ÙŠ',
    previous: 'Ø§Ù„Ø³Ø§Ø¨Ù‚',
    page: 'ØµÙØ­Ø©',
    of: 'Ù…Ù†',
    total: 'Ø§Ù„Ù…Ø¬Ù…ÙˆØ¹',
    validate: 'ØªØ£ÙƒÙŠØ¯',
    perDay: '/ÙŠÙˆÙ…',
    days: 'Ø£ÙŠØ§Ù…',
    months: 'Ø£Ø´Ù‡Ø±',
    km: 'ÙƒÙ…',
    vehicle: 'Ù…Ø±ÙƒØ¨Ø©',
    client: 'Ø¹Ù…ÙŠÙ„',
    amount: 'Ø§Ù„Ù…Ø¨Ù„Øº',
    cost: 'Ø§Ù„ØªÙƒÙ„ÙØ©',
    type: 'Ø§Ù„Ù†ÙˆØ¹',
    description: 'Ø§Ù„ÙˆØµÙ',
    document: 'Ù…Ø³ØªÙ†Ø¯',
    photo: 'ØµÙˆØ±Ø©',
    none: 'Ù„Ø§ ÙŠÙˆØ¬Ø¯',
    see: 'Ø¹Ø±Ø¶',
    manage: 'Ø¥Ø¯Ø§Ø±Ø©',
    reports: 'Ø§Ù„ØªÙ‚Ø§Ø±ÙŠØ±',
    from: 'Ù…Ù†',
    to: 'Ø¥Ù„Ù‰',
    admin: 'Ù…Ø¯ÙŠØ±',
    logout: 'ØªØ³Ø¬ÙŠÙ„ Ø§Ù„Ø®Ø±ÙˆØ¬',
    uploadInProgress: 'Ø¬Ø§Ø±ÙŠ Ø§Ù„Ø±ÙØ¹...',
    deleteConfirm: 'Ù‡Ù„ Ø£Ù†Øª Ù…ØªØ£ÙƒØ¯ Ù…Ù† Ø§Ù„Ø­Ø°ÙØŸ',
    errorOccurred: 'Ø­Ø¯Ø« Ø®Ø·Ø£',
    savedSuccessfully: 'ØªÙ… Ø§Ù„Ø­ÙØ¸ Ø¨Ù†Ø¬Ø§Ø­',
  },

  // â”€â”€â”€ Sidebar / Navigation â”€â”€â”€
  sidebar: {
    brand: 'Ø¥Ø¯Ø§Ø±Ø© Ø§Ù„Ø£Ø³Ø·ÙˆÙ„',
    dashboard: 'Ù„ÙˆØ­Ø© Ø§Ù„ØªØ­ÙƒÙ…',
    vehicles: 'Ø§Ù„Ù…Ø±ÙƒØ¨Ø§Øª',
    clients: 'Ø§Ù„Ø¹Ù…Ù„Ø§Ø¡',
    contracts: 'Ø§Ù„Ø¹Ù‚ÙˆØ¯',
    kilometrage: 'Ø¹Ø¯Ø§Ø¯ Ø§Ù„ÙƒÙŠÙ„ÙˆÙ…ØªØ±Ø§Øª',
    maintenance: 'Ø§Ù„ØµÙŠØ§Ù†Ø©',
    consumables: 'Ø§Ù„Ù…ÙˆØ§Ø¯ Ø§Ù„Ø§Ø³ØªÙ‡Ù„Ø§ÙƒÙŠØ©',
    insuranceControl: 'Ø§Ù„ØªØ£Ù…ÙŠÙ† ÙˆØ§Ù„ÙØ­Øµ',
    alerts: 'Ø§Ù„ØªÙ†Ø¨ÙŠÙ‡Ø§Øª',
    reportsAnalytics: 'Ø§Ù„ØªÙ‚Ø§Ø±ÙŠØ± ÙˆØ§Ù„ØªØ­Ù„ÙŠÙ„Ø§Øª',
    settings: 'Ø§Ù„Ø¥Ø¹Ø¯Ø§Ø¯Ø§Øª',
  },

  // â”€â”€â”€ Topbar â”€â”€â”€
  topbar: {
    title: 'Ù†Ø¸Ø§Ù… Ø¥Ø¯Ø§Ø±Ø© Ø§Ù„Ø£Ø³Ø·ÙˆÙ„',
    activeAlerts: 'Ø§Ù„ØªÙ†Ø¨ÙŠÙ‡Ø§Øª Ø§Ù„Ù†Ø´Ø·Ø©',
    noActiveAlerts: 'Ù„Ø§ ØªÙˆØ¬Ø¯ ØªÙ†Ø¨ÙŠÙ‡Ø§Øª Ù†Ø´Ø·Ø©.',
    language: 'Ø§Ù„Ù„ØºØ©',
  },

  // â”€â”€â”€ Login â”€â”€â”€
  login: {
    title: 'Ø¥Ø¯Ø§Ø±Ø© Ø§Ù„Ø£Ø³Ø·ÙˆÙ„',
    subtitle: 'Ø¥Ø¯Ø§Ø±Ø© Ø£Ø³Ø·ÙˆÙ„ Ø§Ù„ØªØ£Ø¬ÙŠØ±',
    username: 'Ø§Ø³Ù… Ø§Ù„Ù…Ø³ØªØ®Ø¯Ù…',
    password: 'ÙƒÙ„Ù…Ø© Ø§Ù„Ù…Ø±ÙˆØ±',
    loginBtn: 'ØªØ³Ø¬ÙŠÙ„ Ø§Ù„Ø¯Ø®ÙˆÙ„',
    loggingIn: 'Ø¬Ø§Ø±ÙŠ Ø§Ù„Ø¯Ø®ÙˆÙ„...',
    defaultCredentials: 'Ø§Ù„Ø§ÙØªØ±Ø§Ø¶ÙŠ:',
    errorRequired: 'ÙŠØ±Ø¬Ù‰ Ø¥Ø¯Ø®Ø§Ù„ Ø§Ø³Ù… Ø§Ù„Ù…Ø³ØªØ®Ø¯Ù… ÙˆÙƒÙ„Ù…Ø© Ø§Ù„Ù…Ø±ÙˆØ±.',
    errorInvalid: 'Ø§Ø³Ù… Ø§Ù„Ù…Ø³ØªØ®Ø¯Ù… Ø£Ùˆ ÙƒÙ„Ù…Ø© Ø§Ù„Ù…Ø±ÙˆØ± ØºÙŠØ± ØµØ­ÙŠØ­Ø©.',
    errorServer: 'Ø­Ø¯Ø« Ø®Ø·Ø£ Ø£Ø«Ù†Ø§Ø¡ ØªØ³Ø¬ÙŠÙ„ Ø§Ù„Ø¯Ø®ÙˆÙ„. ÙŠØ±Ø¬Ù‰ Ø§Ù„Ù…Ø­Ø§ÙˆÙ„Ø© Ù…Ø±Ø© Ø£Ø®Ø±Ù‰.',
  },

  // â”€â”€â”€ Dashboard â”€â”€â”€
  dashboard: {
    pageTitle: 'Ù„ÙˆØ­Ø© Ø§Ù„ØªØ­ÙƒÙ…',
    pageDesc: 'Ù†Ø¸Ø±Ø© Ø¹Ø§Ù…Ø© Ø¹Ù„Ù‰ Ø§Ù„Ø£Ø³Ø·ÙˆÙ„ ÙˆØ§Ù„Ø¹Ù…Ù„ÙŠØ§Øª.',
    fleet: 'Ø§Ù„Ø£Ø³Ø·ÙˆÙ„',
    registeredVehicles: 'Ù…Ø±ÙƒØ¨Ø§Øª Ù…Ø³Ø¬Ù„Ø©',
    activeRentals: 'Ø§Ù„Ø¥ÙŠØ¬Ø§Ø±Ø§Øª Ø§Ù„Ù†Ø´Ø·Ø©',
    available: 'Ù…ØªØ§Ø­Ø©',
    revenue: 'Ø§Ù„Ø¥ÙŠØ±Ø§Ø¯Ø§Øª',
    paid: 'Ù…Ø¯ÙÙˆØ¹',
    alerts: 'Ø§Ù„ØªÙ†Ø¨ÙŠÙ‡Ø§Øª',
    critical: 'Ø­Ø±Ø¬Ø©',
    fleetAvailability: 'ØªÙˆÙØ± Ø§Ù„Ø£Ø³Ø·ÙˆÙ„',
    currentDistribution: 'Ø§Ù„ØªÙˆØ²ÙŠØ¹ Ø§Ù„Ø­Ø§Ù„ÙŠ',
    priorityAlerts: 'ØªÙ†Ø¨ÙŠÙ‡Ø§Øª Ø°Ø§Øª Ø£ÙˆÙ„ÙˆÙŠØ©',
    priorityAlertsDesc: 'Ø§Ù„ØµÙŠØ§Ù†Ø©ØŒ Ø§Ù„ØªØ£Ù…ÙŠÙ†ØŒ Ø§Ù„ÙØ­ÙˆØµØ§Øª',
    noActiveAlert: 'Ù„Ø§ ØªÙˆØ¬Ø¯ ØªÙ†Ø¨ÙŠÙ‡Ø§Øª Ù†Ø´Ø·Ø©.',
    unpaid: 'ØºÙŠØ± Ù…Ø¯ÙÙˆØ¹',
    unpaidDesc: 'Ø¹Ù‚ÙˆØ¯ ÙÙŠ Ø§Ù†ØªØ¸Ø§Ø± Ø§Ù„Ø¯ÙØ¹',
    contractNo: 'Ø±Ù‚Ù… Ø§Ù„Ø¹Ù‚Ø¯',
    departure: 'Ø§Ù„Ù…ØºØ§Ø¯Ø±Ø©',
    paymentUnpaid: 'ØºÙŠØ± Ù…Ø¯ÙÙˆØ¹',
    paymentPartial: 'Ø¬Ø²Ø¦ÙŠ',
    noUnpaidContracts: 'Ù„Ø§ ØªÙˆØ¬Ø¯ Ø¹Ù‚ÙˆØ¯ ØºÙŠØ± Ù…Ø¯ÙÙˆØ¹Ø©.',
  },

  // â”€â”€â”€ Vehicles â”€â”€â”€
  vehicles: {
    pageTitle: 'Ø¥Ø¯Ø§Ø±Ø© Ø§Ù„Ø£Ø³Ø·ÙˆÙ„',
    pageDesc: 'Ø³Ø¬Ù„ ÙˆØªØ§Ø¨Ø¹ Ù…Ø±ÙƒØ¨Ø§Øª Ø§Ù„Ø£Ø³Ø·ÙˆÙ„.',
    addVehicle: 'Ø¥Ø¶Ø§ÙØ© Ù…Ø±ÙƒØ¨Ø©',
    searchPlaceholder: 'Ø¨Ø­Ø« Ø¨Ø§Ù„Ø¹Ù„Ø§Ù…Ø© Ø§Ù„ØªØ¬Ø§Ø±ÙŠØ©ØŒ Ø§Ù„Ù…ÙˆØ¯ÙŠÙ„ØŒ Ø§Ù„Ù„ÙˆØ­Ø©ØŒ VIN...',
    allTypes: 'Ø¬Ù…ÙŠØ¹ Ø§Ù„Ø£Ù†ÙˆØ§Ø¹',
    allFuels: 'Ø¬Ù…ÙŠØ¹ Ø£Ù†ÙˆØ§Ø¹ Ø§Ù„ÙˆÙ‚ÙˆØ¯',
    allStatuses: 'Ø¬Ù…ÙŠØ¹ Ø§Ù„Ø­Ø§Ù„Ø§Øª',
    noVehiclesFound: 'Ù„Ø§ ØªÙˆØ¬Ø¯ Ù…Ø±ÙƒØ¨Ø§Øª Ù…Ø·Ø§Ø¨Ù‚Ø© Ù„Ù„Ù…Ø¹Ø§ÙŠÙŠØ±.',
    statusAvailable: 'Ù…ØªØ§Ø­Ø©',
    statusRented: 'Ù…Ø¤Ø¬Ø±Ø©',
    statusMaintenance: 'ÙÙŠ Ø§Ù„ØµÙŠØ§Ù†Ø©',
    statusReserved: 'Ù…Ø­Ø¬ÙˆØ²Ø©',
    statusImmobilized: 'Ù…ØªÙˆÙ‚ÙØ©',
    engine: 'Ø§Ù„Ù…Ø­Ø±Ùƒ',
    gearbox: 'Ù†Ø§Ù‚Ù„ Ø§Ù„Ø­Ø±ÙƒØ©',
    manual: 'ÙŠØ¯ÙˆÙŠ',
    auto: 'Ø£ÙˆØªÙˆÙ…Ø§ØªÙŠÙƒ',
    automatic: 'Ø£ÙˆØªÙˆÙ…Ø§ØªÙŠÙƒ',
    folder: 'Ø§Ù„Ù…Ù„Ù',
    editVehicle: 'ØªØ¹Ø¯ÙŠÙ„ Ø§Ù„Ù…Ø±ÙƒØ¨Ø©',
    newVehicle: 'Ù…Ø±ÙƒØ¨Ø© Ø¬Ø¯ÙŠØ¯Ø©',
    matricule: 'Ø±Ù‚Ù… Ø§Ù„Ù„ÙˆØ­Ø©',
    brand: 'Ø§Ù„Ø¹Ù„Ø§Ù…Ø© Ø§Ù„ØªØ¬Ø§Ø±ÙŠØ©',
    model: 'Ø§Ù„Ù…ÙˆØ¯ÙŠÙ„',
    year: 'Ø³Ù†Ø© Ø§Ù„ØµÙ†Ø¹',
    fuelType: 'Ù†ÙˆØ¹ Ø§Ù„ÙˆÙ‚ÙˆØ¯',
    transmission: 'Ù†Ø§Ù‚Ù„ Ø§Ù„Ø­Ø±ÙƒØ©',
    vin: 'Ø±Ù‚Ù… Ø§Ù„Ù‡ÙŠÙƒÙ„ (VIN)',
    engineNumber: 'Ø±Ù‚Ù… Ø§Ù„Ù…Ø­Ø±Ùƒ',
    color: 'Ø§Ù„Ù„ÙˆÙ†',
    seats: 'Ø§Ù„Ù…Ù‚Ø§Ø¹Ø¯',
    dailyRate: 'Ø§Ù„ØªØ¹Ø±ÙØ© Ø§Ù„ÙŠÙˆÙ…ÙŠØ©',
    purchasePrice: 'Ø³Ø¹Ø± Ø§Ù„Ø´Ø±Ø§Ø¡',
    initialKm: 'Ø§Ù„Ø¹Ø¯Ø§Ø¯ Ø§Ù„Ø§Ø¨ØªØ¯Ø§Ø¦ÙŠ (ÙƒÙ…)',
    remarks: 'Ù…Ù„Ø§Ø­Ø¸Ø§Øª',
    deleteConfirm: 'Ù‡Ù„ Ø£Ù†Øª Ù…ØªØ£ÙƒØ¯ Ù…Ù† Ø­Ø°Ù Ù‡Ø°Ù‡ Ø§Ù„Ù…Ø±ÙƒØ¨Ø© Ù…Ù† Ø§Ù„Ø£Ø³Ø·ÙˆÙ„ØŸ',
    errorDelete: 'Ø®Ø·Ø£ ÙÙŠ Ø­Ø°Ù Ø§Ù„Ù…Ø±ÙƒØ¨Ø©',
    errorUpdate: 'Ø®Ø·Ø£ ÙÙŠ ØªØ­Ø¯ÙŠØ« Ø§Ù„Ù…Ø±ÙƒØ¨Ø©',
    errorCreate: 'Ø®Ø·Ø£ ÙÙŠ Ø¥Ù†Ø´Ø§Ø¡ Ø§Ù„Ù…Ø±ÙƒØ¨Ø©',
    tracking: 'Ø§Ù„Ù…ØªØ§Ø¨Ø¹Ø©',
    consumablesTab: 'Ø§Ù„Ù…ÙˆØ§Ø¯ Ø§Ù„Ø§Ø³ØªÙ‡Ù„Ø§ÙƒÙŠØ©',
    insuranceTab: 'Ø§Ù„ØªØ£Ù…ÙŠÙ†',
    inspectionsTab: 'Ø§Ù„ÙØ­ÙˆØµØ§Øª',
    fuelTab: 'Ø§Ù„ÙˆÙ‚ÙˆØ¯',
    kmHistoryTab: 'Ø§Ù„ÙƒÙŠÙ„ÙˆÙ…ØªØ±Ø§Øª',
    consumablesState: 'Ø­Ø§Ù„Ø© Ø§Ù„Ù…ÙˆØ§Ø¯ Ø§Ù„Ø§Ø³ØªÙ‡Ù„Ø§ÙƒÙŠØ©',
    consumable: 'Ø§Ù„Ù…Ø§Ø¯Ø© Ø§Ù„Ø§Ø³ØªÙ‡Ù„Ø§ÙƒÙŠØ©',
    lastReplacement: 'Ø¢Ø®Ø± Ø§Ø³ØªØ¨Ø¯Ø§Ù„',
    traveled: 'Ù…Ø³Ø§ÙØ© Ù…Ù‚Ø·ÙˆØ¹Ø©',
    interval: 'Ø§Ù„ÙØªØ±Ø©',
    noLog: 'Ù„Ø§ ÙŠÙˆØ¬Ø¯ Ø³Ø¬Ù„',
    insuranceHistory: 'Ø³Ø¬Ù„ Ø§Ù„ØªØ£Ù…ÙŠÙ†',
    insurer: 'Ø´Ø±ÙƒØ© Ø§Ù„ØªØ£Ù…ÙŠÙ†',
    policyNo: 'Ø±Ù‚Ù… Ø§Ù„Ø¨ÙˆÙ„ÙŠØµØ©',
    coverage: 'Ø§Ù„ØªØºØ·ÙŠØ©',
    validity: 'Ø§Ù„ØµÙ„Ø§Ø­ÙŠØ©',
    premiumValue: 'Ø§Ù„Ù‚Ø³Ø· / Ø§Ù„Ù‚ÙŠÙ…Ø©',
    statusValid: 'Ø³Ø§Ø±ÙŠ',
    statusExpiringSoon: 'Ù‚Ø±ÙŠØ¨Ø§Ù‹',
    statusExpired: 'Ù…Ù†ØªÙ‡ÙŠ',
    technicalInspections: 'Ø§Ù„ÙØ­ÙˆØµØ§Øª Ø§Ù„ÙÙ†ÙŠØ©',
    expiration: 'Ø§Ù†ØªÙ‡Ø§Ø¡ Ø§Ù„ØµÙ„Ø§Ø­ÙŠØ©',
    center: 'Ø§Ù„Ù…Ø±ÙƒØ²',
    result: 'Ø§Ù„Ù†ØªÙŠØ¬Ø©',
    resultPass: 'Ù†Ø§Ø¬Ø­',
    resultConditional: 'Ø¥Ø¹Ø§Ø¯Ø© ÙØ­Øµ',
    resultFail: 'Ø±Ø§Ø³Ø¨',
    fuelTracking: 'ØªØªØ¨Ø¹ Ø§Ù„ÙˆÙ‚ÙˆØ¯',
    counter: 'Ø§Ù„Ø¹Ø¯Ø§Ø¯',
    volume: 'Ø§Ù„Ø­Ø¬Ù…',
    consumption: 'Ø§Ù„Ø§Ø³ØªÙ‡Ù„Ø§Ùƒ',
    anomaly: 'Ø´Ø°ÙˆØ°',
    overConsumption: 'Ø§Ø³ØªÙ‡Ù„Ø§Ùƒ Ù…ÙØ±Ø·',
    kmHistory: 'Ø³Ø¬Ù„ Ø§Ù„ÙƒÙŠÙ„ÙˆÙ…ØªØ±Ø§Øª',
    reading: 'Ù‚Ø±Ø§Ø¡Ø©',
    newInsurancePolicy: 'Ø¨ÙˆÙ„ÙŠØµØ© ØªØ£Ù…ÙŠÙ† Ø¬Ø¯ÙŠØ¯Ø©',
    coverageThirdParty: 'ØªØ£Ù…ÙŠÙ† Ø¶Ø¯ Ø§Ù„ØºÙŠØ±',
    coverageComprehensive: 'ØªØ£Ù…ÙŠÙ† Ø´Ø§Ù…Ù„',
    coverageFleet: 'ØªØ£Ù…ÙŠÙ† Ø£Ø³Ø·ÙˆÙ„',
    premium: 'Ø§Ù„Ù‚Ø³Ø·',
    effectiveDate: 'ØªØ§Ø±ÙŠØ® Ø§Ù„Ø³Ø±ÙŠØ§Ù†',
    documentPdf: 'Ø§Ù„Ù…Ø³ØªÙ†Ø¯ (PDF)',
    newInspection: 'ÙØ­Øµ ÙÙ†ÙŠ Ø¬Ø¯ÙŠØ¯',
    inspectionDate: 'ØªØ§Ø±ÙŠØ® Ø§Ù„ÙØ­Øµ',
    resultFavorable: 'Ù†Ø§Ø¬Ø­',
    resultCounterVisit: 'Ø¥Ø¹Ø§Ø¯Ø© ÙØ­Øµ',
    resultUnfavorable: 'Ø±Ø§Ø³Ø¨',
    centerAddress: 'Ø¹Ù†ÙˆØ§Ù† Ø§Ù„Ù…Ø±ÙƒØ²',
    newFuelFillup: 'ØªØ¹Ø¨Ø¦Ø© ÙˆÙ‚ÙˆØ¯ Ø¬Ø¯ÙŠØ¯Ø©',
    counterKm: 'Ø§Ù„Ø¹Ø¯Ø§Ø¯ (ÙƒÙ…)',
    volumeL: 'Ø§Ù„Ø­Ø¬Ù… (Ù„ØªØ±)',
    pricePerL: 'Ø§Ù„Ø³Ø¹Ø±/Ù„ØªØ±',
    station: 'Ø§Ù„Ù…Ø­Ø·Ø©',
    fillup: 'ØªØ¹Ø¨Ø¦Ø©',
    kmReading: 'Ù‚Ø±Ø§Ø¡Ø© Ø§Ù„ÙƒÙŠÙ„ÙˆÙ…ØªØ±Ø§Øª',
  },

  // â”€â”€â”€ Clients â”€â”€â”€
  clients: {
    pageTitle: 'Ù‚Ø§Ø¹Ø¯Ø© Ø¨ÙŠØ§Ù†Ø§Øª Ø§Ù„Ø¹Ù…Ù„Ø§Ø¡',
    pageDesc: 'Ø¥Ø¯Ø§Ø±Ø© Ø³Ø¬Ù„Ø§Øª Ø§Ù„Ø¹Ù…Ù„Ø§Ø¡ØŒ Ù…Ø¹Ù„ÙˆÙ…Ø§Øª Ø§Ù„Ø§ØªØµØ§Ù„ØŒ Ø±Ø®Øµ Ø§Ù„Ù‚ÙŠØ§Ø¯Ø© ÙˆØ³Ø¬Ù„ Ø§Ù„Ø¥ÙŠØ¬Ø§Ø±Ø§Øª.',
    addClient: 'Ø¥Ø¶Ø§ÙØ© Ø¹Ù…ÙŠÙ„',
    searchPlaceholder: 'Ø¨Ø­Ø« Ø¨Ø§Ù„Ø§Ø³Ù…ØŒ Ø§Ù„Ù‡Ø§ØªÙØŒ Ø§Ù„Ø¨Ø±ÙŠØ¯ØŒ Ø§Ù„Ù‡ÙˆÙŠØ©...',
    identity: 'Ø§Ù„Ù‡ÙˆÙŠØ©',
    contact: 'Ø§Ù„Ø§ØªØµØ§Ù„',
    license: 'Ø§Ù„Ø±Ø®ØµØ©',
    licenseValidity: 'Ø§Ù„ØµÙ„Ø§Ø­ÙŠØ©',
    expired: 'Ù…Ù†ØªÙ‡ÙŠØ©',
    history: 'Ø§Ù„Ø³Ø¬Ù„',
    noClientFound: 'Ù„Ù… ÙŠØªÙ… Ø§Ù„Ø¹Ø«ÙˆØ± Ø¹Ù„Ù‰ Ø¹Ù…ÙŠÙ„',
    noClientDesc: 'Ø£Ø¶Ù Ø¹Ù…ÙŠÙ„Ø§Ù‹ Ø£Ùˆ Ø¹Ø¯Ù‘Ù„ Ø§Ù„Ø¨Ø­Ø«.',
    totalClients: 'Ø¹Ù…ÙŠÙ„(Ø¹Ù…Ù„Ø§Ø¡) Ø¥Ø¬Ù…Ø§Ù„Ø§Ù‹',
    editClient: 'ØªØ¹Ø¯ÙŠÙ„ Ø¨ÙŠØ§Ù†Ø§Øª Ø§Ù„Ø¹Ù…ÙŠÙ„',
    newClient: 'Ø¥Ù†Ø´Ø§Ø¡ Ø¹Ù…ÙŠÙ„ Ø¬Ø¯ÙŠØ¯',
    fullName: 'Ø§Ù„Ø§Ø³Ù… Ø§Ù„ÙƒØ§Ù…Ù„',
    nationalId: 'Ø±Ù‚Ù… Ø§Ù„Ù‡ÙˆÙŠØ© / Ø¬ÙˆØ§Ø² Ø§Ù„Ø³ÙØ±',
    dateOfBirth: 'ØªØ§Ø±ÙŠØ® Ø§Ù„Ù…ÙŠÙ„Ø§Ø¯',
    licenseNumber: 'Ø±Ù‚Ù… Ø±Ø®ØµØ© Ø§Ù„Ù‚ÙŠØ§Ø¯Ø©',
    licenseCategory: 'ÙØ¦Ø© Ø§Ù„Ø±Ø®ØµØ©',
    category: 'Ø§Ù„ÙØ¦Ø©',
    licenseIssueDate: 'ØªØ§Ø±ÙŠØ® Ø¥ØµØ¯Ø§Ø± Ø§Ù„Ø±Ø®ØµØ©',
    licenseExpiryDate: 'ØªØ§Ø±ÙŠØ® Ø§Ù†ØªÙ‡Ø§Ø¡ Ø§Ù„Ø±Ø®ØµØ©',
    phone: 'Ø±Ù‚Ù… Ø§Ù„Ù‡Ø§ØªÙ',
    email: 'Ø§Ù„Ø¨Ø±ÙŠØ¯ Ø§Ù„Ø¥Ù„ÙƒØªØ±ÙˆÙ†ÙŠ',
    address: 'Ø§Ù„Ø¹Ù†ÙˆØ§Ù†',
    notesRemarks: 'Ù…Ù„Ø§Ø­Ø¸Ø§Øª / ØªØ¹Ù„ÙŠÙ‚Ø§Øª',
    clientFile: 'Ù…Ù„Ù Ø§Ù„Ø¹Ù…ÙŠÙ„',
    nationalIdLabel: 'Ø§Ù„Ù‡ÙˆÙŠØ© Ø§Ù„ÙˆØ·Ù†ÙŠØ©',
    phoneLabel: 'Ø§Ù„Ù‡Ø§ØªÙ',
    licenseLabel: 'Ø§Ù„Ø±Ø®ØµØ©',
    expirationLabel: 'Ø§Ù„Ø§Ù†ØªÙ‡Ø§Ø¡',
    adminNotes: 'Ù…Ù„Ø§Ø­Ø¸Ø§Øª Ø¥Ø¯Ø§Ø±ÙŠØ©:',
    rentalHistory: 'Ø³Ø¬Ù„ Ø§Ù„Ø¥ÙŠØ¬Ø§Ø±Ø§Øª',
    contractNo: 'Ø±Ù‚Ù… Ø§Ù„Ø¹Ù‚Ø¯',
    vehicleLabel: 'Ø§Ù„Ù…Ø±ÙƒØ¨Ø©',
    registration: 'Ø§Ù„ØªØ³Ø¬ÙŠÙ„',
    period: 'Ø§Ù„ÙØªØ±Ø©',
    amountLabel: 'Ø§Ù„Ù…Ø¨Ù„Øº',
    statusCompleted: 'Ù…ÙƒØªÙ…Ù„',
    statusActive: 'Ù†Ø´Ø·',
    statusDraft: 'Ù…Ø³ÙˆØ¯Ø©',
    statusCancelled: 'Ù…Ù„ØºÙŠ',
    noRentalHistory: 'Ù„Ø§ ØªÙˆØ¬Ø¯ Ø¹Ù‚ÙˆØ¯ Ø¥ÙŠØ¬Ø§Ø± Ù…Ø³Ø¬Ù„Ø© Ù„Ù‡Ø°Ø§ Ø§Ù„Ø¹Ù…ÙŠÙ„.',
  },

  // â”€â”€â”€ Contracts â”€â”€â”€
  contracts: {
    pageTitle: 'Ø¥Ø¯Ø§Ø±Ø© Ø¹Ù‚ÙˆØ¯ Ø§Ù„Ø¥ÙŠØ¬Ø§Ø±',
    pageDesc: 'Ø£Ù†Ø´Ø¦ Ø§Ù„Ø¹Ù‚ÙˆØ¯ØŒ ØªØ§Ø¨Ø¹ Ø§Ù„Ù…ØºØ§Ø¯Ø±Ø§ØªØŒ Ø³Ø¬Ù‘Ù„ Ø¥Ø±Ø¬Ø§Ø¹ Ø§Ù„Ù…Ø±ÙƒØ¨Ø§Øª ÙˆØ£Ø¯Ø± ÙÙˆØ§ØªÙŠØ± Ø§Ù„Ø¹Ù…Ù„Ø§Ø¡.',
    newContract: 'Ø¹Ù‚Ø¯ Ø¬Ø¯ÙŠØ¯',
    searchPlaceholder: 'Ø¨Ø­Ø« Ø¨Ø±Ù‚Ù… Ø§Ù„Ø¹Ù‚Ø¯ØŒ Ø§Ù„Ø¹Ù…ÙŠÙ„ØŒ Ø§Ù„Ù„ÙˆØ­Ø©...',
    allStatuses: 'Ø¬Ù…ÙŠØ¹ Ø§Ù„Ø­Ø§Ù„Ø§Øª',
    statusDraft: 'Ù…Ø³ÙˆØ¯Ø©',
    statusActive: 'Ø¬Ø§Ø±ÙŠ',
    statusCompleted: 'Ù…ÙƒØªÙ…Ù„',
    statusCancelled: 'Ù…Ù„ØºÙŠ',
    allInvoices: 'Ø¬Ù…ÙŠØ¹ Ø§Ù„ÙÙˆØ§ØªÙŠØ±',
    unpaid: 'ØºÙŠØ± Ù…Ø¯ÙÙˆØ¹',
    partial: 'Ø¬Ø²Ø¦ÙŠ',
    paid: 'Ù…Ø¯ÙÙˆØ¹',
    contract: 'Ø§Ù„Ø¹Ù‚Ø¯',
    period: 'Ø§Ù„ÙØªØ±Ø©',
    statuses: 'Ø§Ù„Ø­Ø§Ù„Ø§Øª',
    noContractFound: 'Ù„Ù… ÙŠØªÙ… Ø§Ù„Ø¹Ø«ÙˆØ± Ø¹Ù„Ù‰ Ø¹Ù‚ÙˆØ¯',
    noContractDesc: 'Ø£Ù†Ø´Ø¦ Ø¹Ù‚Ø¯Ø§Ù‹ Ø¬Ø¯ÙŠØ¯Ø§Ù‹ Ø£Ùˆ Ø¹Ø¯Ù‘Ù„ Ø§Ù„ÙÙ„Ø§ØªØ±.',
    totalContracts: 'Ø¹Ù‚Ø¯(Ø¹Ù‚ÙˆØ¯)',
    returnVehicle: 'Ø¥Ø±Ø¬Ø§Ø¹ Ø§Ù„Ù…Ø±ÙƒØ¨Ø©',
    returnBtn: 'Ø¥Ø±Ø¬Ø§Ø¹',
    editContract: 'ØªØ¹Ø¯ÙŠÙ„ Ø¹Ù‚Ø¯ Ø§Ù„Ø¥ÙŠØ¬Ø§Ø±',
    newContractDialog: 'Ø¹Ù‚Ø¯ Ø¥ÙŠØ¬Ø§Ø± Ø¬Ø¯ÙŠØ¯',
    selectClient: 'Ø§Ø®ØªØ± Ø§Ù„Ø¹Ù…ÙŠÙ„',
    chooseClient: 'Ø§Ø®ØªØ± Ø¹Ù…ÙŠÙ„Ø§Ù‹...',
    selectVehicle: 'Ø§Ø®ØªØ± Ø§Ù„Ù…Ø±ÙƒØ¨Ø©',
    chooseVehicle: 'Ø§Ø®ØªØ± Ù…Ø±ÙƒØ¨Ø©...',
    contractType: 'Ù†ÙˆØ¹ Ø§Ù„Ø¹Ù‚Ø¯',
    dailyRate: 'Ø§Ù„ØªØ¹Ø±ÙØ© Ø§Ù„ÙŠÙˆÙ…ÙŠØ©',
    startDateTime: 'ØªØ§Ø±ÙŠØ® ÙˆÙˆÙ‚Øª Ø§Ù„Ù…ØºØ§Ø¯Ø±Ø©',
    expectedReturn: 'ØªØ§Ø±ÙŠØ® ÙˆÙˆÙ‚Øª Ø§Ù„Ø¥Ø±Ø¬Ø§Ø¹ Ø§Ù„Ù…ØªÙˆÙ‚Ø¹',
    calculatedDays: 'Ø§Ù„Ø£ÙŠØ§Ù… Ø§Ù„Ù…Ø­Ø³ÙˆØ¨Ø©',
    rentalAmount: 'Ù…Ø¨Ù„Øº Ø§Ù„Ø¥ÙŠØ¬Ø§Ø±',
    extraFees: 'Ø±Ø³ÙˆÙ… Ø¥Ø¶Ø§ÙÙŠØ©',
    discount: 'Ø®ØµÙ…',
    totalNetDue: 'ØµØ§ÙÙŠ Ø§Ù„Ù…Ø¨Ù„Øº Ø§Ù„Ù…Ø³ØªØ­Ù‚',
    deposit: 'Ø§Ù„Ø¶Ù…Ø§Ù†',
    paymentMethod: 'Ø·Ø±ÙŠÙ‚Ø© Ø§Ù„Ø¯ÙØ¹',
    initialStatus: 'ØªØ­Ø¯ÙŠØ¯ Ø§Ù„Ø­Ø§Ù„Ø© Ø§Ù„Ø£ÙˆÙ„ÙŠØ©',
    draftReservation: 'Ù…Ø³ÙˆØ¯Ø© (Ø­Ø¬Ø² Ø¨Ø³ÙŠØ·)',
    activateNow: 'ØªÙØ¹ÙŠÙ„ Ø§Ù„Ø¢Ù† (Ù…ØºØ§Ø¯Ø±Ø© Ø§Ù„Ù…Ø±ÙƒØ¨Ø©)',
    initialBilling: 'Ø­Ø§Ù„Ø© Ø§Ù„ÙØ§ØªÙˆØ±Ø© Ø§Ù„Ø£ÙˆÙ„ÙŠØ©',
    unpaidPending: 'ØºÙŠØ± Ù…Ø¯ÙÙˆØ¹ (Ù…Ø¹Ù„Ù‚)',
    depositPaid: 'Ø¯ÙØ¹Ø© Ù…Ù‚Ø¯Ù…Ø© (Ø¬Ø²Ø¦ÙŠ)',
    fullyPaid: 'Ù…Ø¯ÙÙˆØ¹ (Ø§Ù„Ø¯ÙØ¹ Ø§Ù„ÙƒØ§Ù…Ù„)',
    adminNotes: 'Ù…Ù„Ø§Ø­Ø¸Ø§Øª Ø¥Ø¯Ø§Ø±ÙŠØ©',
    saveContract: 'Ø­ÙØ¸ Ø§Ù„Ø¹Ù‚Ø¯',
    errorUpdate: 'Ø®Ø·Ø£ ÙÙŠ ØªØ­Ø¯ÙŠØ« Ø§Ù„Ø¹Ù‚Ø¯',
    errorCreate: 'Ø®Ø·Ø£ ÙÙŠ Ø¥Ù†Ø´Ø§Ø¡ Ø§Ù„Ø¹Ù‚Ø¯',
    closeContract: 'Ø¥ØºÙ„Ø§Ù‚ Ø§Ù„Ø¹Ù‚Ø¯',
    departureOdometer: 'Ø¹Ø¯Ø§Ø¯ Ø§Ù„Ù…ØºØ§Ø¯Ø±Ø©:',
    expectedReturnDate: 'Ø§Ù„Ø¥Ø±Ø¬Ø§Ø¹ Ø§Ù„Ù…ØªÙˆÙ‚Ø¹:',
    returnIndex: 'Ø¹Ø¯Ø§Ø¯ Ø§Ù„Ø¥Ø±Ø¬Ø§Ø¹ (ÙƒÙ…)',
    actualReturnDate: 'ØªØ§Ø±ÙŠØ® Ø§Ù„Ø¥Ø±Ø¬Ø§Ø¹ Ø§Ù„ÙØ¹Ù„ÙŠ',
    fuelPenalty: 'ØºØ±Ø§Ù…Ø© Ø§Ù„ÙˆÙ‚ÙˆØ¯',
    damageFees: 'Ø±Ø³ÙˆÙ… Ø§Ù„Ø£Ø¶Ø±Ø§Ø±',
    sendToMaintenance: 'Ø¥Ø±Ø³Ø§Ù„ Ø§Ù„Ù…Ø±ÙƒØ¨Ø© Ù„Ù„ØµÙŠØ§Ù†Ø© (Ø£Ø¶Ø±Ø§Ø± Ù…ÙØ¨Ù„Ù‘ØºØ©)',
    returnNotes: 'Ù…Ù„Ø§Ø­Ø¸Ø§Øª Ø§Ù„Ø¥Ø±Ø¬Ø§Ø¹ / Ø§Ù„Ù…Ø¹Ø§ÙŠÙ†Ø©',
    returnNotesPlaceholder: 'Ø§Ù„Ù†Ø¸Ø§ÙØ©ØŒ Ù…Ø³ØªÙˆÙ‰ Ø§Ù„ÙˆÙ‚ÙˆØ¯ØŒ Ø§Ù„Ø®Ø¯ÙˆØ´...',
    validateReturn: 'ØªØ£ÙƒÙŠØ¯ Ø§Ù„Ø¥Ø±Ø¬Ø§Ø¹',
    errorReturn: 'Ø®Ø·Ø£ ÙÙŠ Ù…Ø¹Ø§Ù„Ø¬Ø© Ø¥Ø±Ø¬Ø§Ø¹ Ø§Ù„Ù…Ø±ÙƒØ¨Ø©',
    licenseWarning: 'ØªØ­Ø°ÙŠØ±: Ø±Ø®ØµØ© Ù‡Ø°Ø§ Ø§Ù„Ø¹Ù…ÙŠÙ„ Ù…Ù†ØªÙ‡ÙŠØ© Ù…Ù†Ø°',
    printPreview: 'Ù…Ø¹Ø§ÙŠÙ†Ø© Ø§Ù„Ø·Ø¨Ø§Ø¹Ø©',
    invoiceBrand: 'ØªØ£Ø¬ÙŠØ± Ø§Ù„Ø³ÙŠØ§Ø±Ø§Øª',
    invoiceSubtitle: 'Ø®Ø¯Ù…Ø© ØªØ£Ø¬ÙŠØ± Ø³ÙŠØ§Ø±Ø§Øª Ù…ØªÙ…ÙŠØ²Ø©',
    contractInvoice: 'Ø§Ù„Ø¹Ù‚Ø¯ ÙˆØ§Ù„ÙØ§ØªÙˆØ±Ø©',
    tenant: 'Ø§Ù„Ù…Ø³ØªØ£Ø¬Ø± / Ø§Ù„Ø¹Ù…ÙŠÙ„:',
    vehicleLabel: 'Ø§Ù„Ù…Ø±ÙƒØ¨Ø©:',
    registrationLabel: 'Ø§Ù„ØªØ³Ø¬ÙŠÙ„',
    departureCounter: 'Ø¹Ø¯Ø§Ø¯ Ø§Ù„Ù…ØºØ§Ø¯Ø±Ø©',
    returnCounter: 'Ø¹Ø¯Ø§Ø¯ Ø§Ù„Ø¥Ø±Ø¬Ø§Ø¹',
    dailyRateLabel: 'Ø§Ù„ØªØ¹Ø±ÙØ© Ø§Ù„ÙŠÙˆÙ…ÙŠØ©',
    daysLabel: 'Ø§Ù„Ø£ÙŠØ§Ù…',
    totalHT: 'Ø§Ù„Ù…Ø¬Ù…ÙˆØ¹',
    vehicleRental: 'ØªØ£Ø¬ÙŠØ± Ù…Ø±ÙƒØ¨Ø©',
    extrasAccessories: 'Ø®Ø¯Ù…Ø§Øª Ø¥Ø¶Ø§ÙÙŠØ© / Ù…Ù„Ø­Ù‚Ø§Øª',
    penalties: 'ØºØ±Ø§Ù…Ø§Øª (Ø§Ù„ØªØ£Ø®ÙŠØ±ØŒ Ø§Ù„Ø£Ø¶Ø±Ø§Ø±ØŒ Ø§Ù„ÙˆÙ‚ÙˆØ¯)',
    paymentLabel: 'Ø§Ù„Ø¯ÙØ¹:',
    mode: 'Ø§Ù„Ø·Ø±ÙŠÙ‚Ø©',
    statusLabel: 'Ø§Ù„Ø­Ø§Ù„Ø©',
    subtotal: 'Ø§Ù„Ù…Ø¬Ù…ÙˆØ¹ Ø§Ù„ÙØ±Ø¹ÙŠ:',
    reduction: 'Ø§Ù„Ø®ØµÙ…:',
    netAmountDue: 'ØµØ§ÙÙŠ Ø§Ù„Ù…Ø¨Ù„Øº Ø§Ù„Ù…Ø³ØªØ­Ù‚:',
    tenantSignature: 'ØªÙˆÙ‚ÙŠØ¹ Ø§Ù„Ù…Ø³ØªØ£Ø¬Ø±',
    agencySignature: 'ØªÙˆÙ‚ÙŠØ¹ Ø§Ù„ÙˆÙƒØ§Ù„Ø©',
    amountPaid: 'Ø§Ù„Ù…Ø¨Ù„Øº Ø§Ù„Ù…Ø¯ÙÙˆØ¹',
    remainingDue: 'Ø§Ù„Ù…Ø¨Ù„Øº Ø§Ù„Ù…ØªØ¨Ù‚ÙŠ',
  },

  // â”€â”€â”€ Maintenance â”€â”€â”€
  maintenance: {
    pageTitle: 'Ù…ØªØ§Ø¨Ø¹Ø© Ø§Ù„ØµÙŠØ§Ù†Ø©',
    pageDesc: 'Ø³Ø¬Ù„ Ø§Ù„ØªØ¯Ø®Ù„Ø§Øª Ø§Ù„Ù…ÙŠÙƒØ§Ù†ÙŠÙƒÙŠØ© (Ø§Ù„ÙˆÙ‚Ø§Ø¦ÙŠØ© Ø£Ùˆ Ø§Ù„ØªØµØ­ÙŠØ­ÙŠØ©) ÙˆØ¬Ø¯ÙˆÙ„Ø© Ø§Ù„ØµÙŠØ§Ù†Ø§Øª Ø§Ù„Ù‚Ø§Ø¯Ù…Ø©.',
    maintenanceCalendar: 'Ø¬Ø¯ÙˆÙ„ Ø§Ù„ØµÙŠØ§Ù†Ø©',
    registerIntervention: 'ØªØ³Ø¬ÙŠÙ„ ØªØ¯Ø®Ù„',
    intervention: 'Ø§Ù„ØªØ¯Ø®Ù„',
    dates: 'Ø§Ù„ØªÙˆØ§Ø±ÙŠØ®',
    workshop: 'Ø§Ù„ÙˆØ±Ø´Ø©',
    invoice: 'Ø§Ù„ÙØ§ØªÙˆØ±Ø©',
    nextScheduled: 'Ø§Ù„Ù‚Ø§Ø¯Ù…',
    statusScheduled: 'Ù…Ø¬Ø¯ÙˆÙ„',
    statusInProgress: 'Ù‚ÙŠØ¯ Ø§Ù„ØªÙ†ÙÙŠØ°',
    statusCompleted: 'Ù…ÙƒØªÙ…Ù„',
    noMaintenance: 'Ù„Ø§ ØªÙˆØ¬Ø¯ ØµÙŠØ§Ù†Ø© Ù…Ø³Ø¬Ù„Ø©',
    noMaintenanceDesc: 'Ø³Ø¬Ù„ ØªØ¯Ø®Ù„Ø§Ù‹ Ù„Ø¨Ø¯Ø¡ Ø§Ù„Ù…ØªØ§Ø¨Ø¹Ø©.',
    interventions: 'ØªØ¯Ø®Ù„(ØªØ¯Ø®Ù„Ø§Øª)',
    editMaintenance: 'ØªØ¹Ø¯ÙŠÙ„ Ø³Ø¬Ù„ Ø§Ù„ØµÙŠØ§Ù†Ø©',
    newIntervention: 'ØªØ³Ø¬ÙŠÙ„ ØªØ¯Ø®Ù„',
    interventionType: 'Ù†ÙˆØ¹ Ø§Ù„ØªØ¯Ø®Ù„',
    interventionDate: 'ØªØ§Ø±ÙŠØ® Ø§Ù„ØªØ¯Ø®Ù„',
    nextMaintenance: 'Ø§Ù„ØµÙŠØ§Ù†Ø© Ø§Ù„Ù‚Ø§Ø¯Ù…Ø©',
    counterKm: 'Ø§Ù„Ø¹Ø¯Ø§Ø¯ ÙƒÙ… (Ø§Ù„ØªØ¯Ø®Ù„)',
    interventionStatus: 'Ø­Ø§Ù„Ø© Ø§Ù„ØªØ¯Ø®Ù„',
    laborCost: 'ØªÙƒÙ„ÙØ© Ø§Ù„Ø¹Ù…Ø§Ù„Ø©',
    partsCost: 'ØªÙƒÙ„ÙØ© Ø§Ù„Ù‚Ø·Ø¹',
    workshopName: 'Ø§Ø³Ù… Ø§Ù„ÙˆØ±Ø´Ø© / Ø§Ù„Ù…Ø±Ø¢Ø¨',
    workshopContact: 'Ø§ØªØµØ§Ù„ Ø§Ù„ÙˆØ±Ø´Ø©',
    workshopAddress: 'Ø¹Ù†ÙˆØ§Ù† Ø§Ù„ÙˆØ±Ø´Ø©',
    invoiceNo: 'Ø±Ù‚Ù… Ø§Ù„ÙØ§ØªÙˆØ±Ø©',
    invoiceFile: 'Ù…Ù„Ù Ø§Ù„ÙØ§ØªÙˆØ±Ø© (PDF/ØµÙˆØ±Ø©)',
    workDescription: 'ÙˆØµÙ Ø§Ù„Ø£Ø¹Ù…Ø§Ù„',
    calendarTitle: 'Ø¬Ø¯ÙˆÙ„ Ø§Ù„ØµÙŠØ§Ù†Ø©',
    noScheduledMaintenance: 'Ù„Ø§ ØªÙˆØ¬Ø¯ ØµÙŠØ§Ù†Ø© Ù…Ø¬Ø¯ÙˆÙ„Ø©.',
    plateLabel: 'Ø§Ù„Ù„ÙˆØ­Ø©',
  },

  // â”€â”€â”€ Consumables â”€â”€â”€
  consumables: {
    pageTitle: 'Ø§Ù„Ù…ÙˆØ§Ø¯ Ø§Ù„Ø§Ø³ØªÙ‡Ù„Ø§ÙƒÙŠØ© ÙˆØ§Ù„ØµÙŠØ§Ù†Ø© Ø§Ù„Ø¯ÙˆØ±ÙŠØ©',
    pageDesc: 'Ø±Ø§Ù‚Ø¨ Ø­Ø§Ù„Ø© ØªØ¢ÙƒÙ„ Ø§Ù„ÙÙ„Ø§ØªØ± ÙˆØ§Ù„Ø¥Ø·Ø§Ø±Ø§Øª ÙˆØ§Ù„ÙØ±Ø§Ù…Ù„ ÙˆØ§Ù„Ø¨Ø·Ø§Ø±ÙŠØ§Øª ÙˆØ²ÙŠØª Ø§Ù„Ù…Ø­Ø±Ùƒ. Ø³Ø¬Ù„ Ø§Ù„Ø§Ø³ØªØ¨Ø¯Ø§Ù„Ø§Øª Ø§Ù„Ù…Ù†ØªØ¸Ù…Ø©.',
    configureIntervals: 'ØªÙƒÙˆÙŠÙ† Ø§Ù„ÙØªØ±Ø§Øª',
    registerReplacement: 'ØªØ³Ø¬ÙŠÙ„ Ø§Ø³ØªØ¨Ø¯Ø§Ù„',
    selectVehicle: 'Ø§Ø®ØªØ± Ù…Ø±ÙƒØ¨Ø©:',
    chooseVehicle: 'Ø§Ø®ØªØ± Ù…Ø±ÙƒØ¨Ø©...',
    currentOdometer: 'Ø§Ù„Ø¹Ø¯Ø§Ø¯ Ø§Ù„Ø­Ø§Ù„ÙŠ:',
    selectVehiclePrompt: 'ÙŠØ±Ø¬Ù‰ Ø§Ø®ØªÙŠØ§Ø± Ù…Ø±ÙƒØ¨Ø© Ù„Ø¹Ø±Ø¶ Ø­Ø§Ù„Ø© Ø§Ù„Ù…ÙˆØ§Ø¯ Ø§Ù„Ø§Ø³ØªÙ‡Ù„Ø§ÙƒÙŠØ©.',
    intervalLabel: 'Ø§Ù„ÙØªØ±Ø©',
    statusOk: 'Ù…Ø·Ø§Ø¨Ù‚',
    statusWarning: 'Ù‚Ø±ÙŠØ¨Ø§Ù‹',
    statusDue: 'ÙŠØ¬Ø¨ Ø§Ù„Ø§Ø³ØªØ¨Ø¯Ø§Ù„',
    lastReplacement: 'Ø¢Ø®Ø± Ø§Ø³ØªØ¨Ø¯Ø§Ù„:',
    atOrder: 'Ø¹Ù†Ø¯ Ø§Ù„Ø·Ù„Ø¨',
    kmTraveled: 'Ø§Ù„ÙƒÙŠÙ„ÙˆÙ…ØªØ±Ø§Øª Ø§Ù„Ù…Ù‚Ø·ÙˆØ¹Ø©:',
    timeElapsed: 'Ø§Ù„ÙˆÙ‚Øª Ø§Ù„Ù…Ù†Ù‚Ø¶ÙŠ:',
    viscosityOil: 'Ø§Ù„Ù„Ø²ÙˆØ¬Ø©/Ø§Ù„Ø²ÙŠØª:',
    brandSize: 'Ø§Ù„Ø¹Ù„Ø§Ù…Ø©/Ø§Ù„Ø­Ø¬Ù…:',
    axle: 'Ø§Ù„Ù…Ø­ÙˆØ±:',
    logReplacement: 'Ø³Ø¬Ù„ Ø§Ø³ØªØ¨Ø¯Ø§Ù„ Ø§Ù„Ù…ÙˆØ§Ø¯ Ø§Ù„Ø§Ø³ØªÙ‡Ù„Ø§ÙƒÙŠØ©',
    consumableType: 'Ù†ÙˆØ¹ Ø§Ù„Ù…Ø§Ø¯Ø© Ø§Ù„Ø§Ø³ØªÙ‡Ù„Ø§ÙƒÙŠØ©',
    replacementDate: 'ØªØ§Ø±ÙŠØ® Ø§Ù„Ø§Ø³ØªØ¨Ø¯Ø§Ù„',
    replacementKm: 'Ø§Ù„ÙƒÙŠÙ„ÙˆÙ…ØªØ±Ø§Øª (Ø§Ù„Ø¹Ø¯Ø§Ø¯)',
    oilType: 'Ù†ÙˆØ¹ Ø§Ù„Ø²ÙŠØª',
    viscosity: 'Ø§Ù„Ù„Ø²ÙˆØ¬Ø©',
    axlePosition: 'Ø§Ù„Ù…Ø­ÙˆØ± / Ø§Ù„Ù…ÙˆÙ‚Ø¹',
    frontAxle: 'Ø§Ù„Ù…Ø­ÙˆØ± Ø§Ù„Ø£Ù…Ø§Ù…ÙŠ',
    rearAxle: 'Ø§Ù„Ù…Ø­ÙˆØ± Ø§Ù„Ø®Ù„ÙÙŠ',
    brandLabel: 'Ø§Ù„Ø¹Ù„Ø§Ù…Ø©',
    dimensions: 'Ø§Ù„Ø£Ø¨Ø¹Ø§Ø¯',
    typeDetail: 'Ø§Ù„Ù†ÙˆØ¹',
    batteryBrand: 'Ø¹Ù„Ø§Ù…Ø© Ø§Ù„Ø¨Ø·Ø§Ø±ÙŠØ©',
    batteryCapacity: 'Ø§Ù„Ø³Ø¹Ø© / Ø§Ù„Ø£Ù…Ø¨ÙŠØ±',
    manufacturer: 'Ø§Ù„Ø¹Ù„Ø§Ù…Ø© / Ø§Ù„Ù…ØµÙ†Ø¹',
    technicalDetails: 'ØªÙØ§ØµÙŠÙ„ ÙÙ†ÙŠØ©',
    notesRemarks: 'Ù…Ù„Ø§Ø­Ø¸Ø§Øª / ØªØ¹Ù„ÙŠÙ‚Ø§Øª',
    intervalsConfig: 'ÙØªØ±Ø§Øª Ø§Ù„ØµÙŠØ§Ù†Ø© ÙˆØ§Ù„ØªÙ†Ø¨ÙŠÙ‡Ø§Øª',
    intervalsDesc: 'Ø­Ø¯Ø¯ Ø§Ù„Ø¹ØªØ¨Ø§Øª Ø§Ù„Ø­Ø±Ø¬Ø© Ù„Ù…ÙˆØ§Ø¯Ùƒ Ø§Ù„Ø§Ø³ØªÙ‡Ù„Ø§ÙƒÙŠØ©.',
    createRule: 'Ø¥Ù†Ø´Ø§Ø¡ Ù‚Ø§Ø¹Ø¯Ø©',
    limitKm: 'Ø§Ù„Ø­Ø¯ (ÙƒÙ…)',
    limitMonths: 'Ø§Ù„Ø­Ø¯ (Ø£Ø´Ù‡Ø±)',
    configureRule: 'ØªÙƒÙˆÙŠÙ† Ø§Ù„Ù‚Ø§Ø¹Ø¯Ø© Ù„Ù€',
    newRule: 'Ø¬Ø¯ÙŠØ¯',
    intervalKm: 'Ø§Ù„ÙØªØ±Ø© (ÙƒÙ…)',
    intervalMonths: 'Ø§Ù„ÙØªØ±Ø© (Ø£Ø´Ù‡Ø±)',
    validateRule: 'ØªØ£ÙƒÙŠØ¯ Ø§Ù„Ù‚Ø§Ø¹Ø¯Ø©',
  },

  // â”€â”€â”€ Insurance & Inspections â”€â”€â”€
  insurance: {
    pageTitle: 'Ø¥Ø¯Ø§Ø±Ø© Ø§Ù„ØªØ£Ù…ÙŠÙ† ÙˆØ§Ù„ÙØ­Øµ Ø§Ù„ÙÙ†ÙŠ',
    pageDesc: 'Ø±Ø§Ù‚Ø¨ Ø§Ù„Ø­Ø§Ù„Ø© Ø§Ù„Ø¥Ø¯Ø§Ø±ÙŠØ© Ù„Ø£Ø³Ø·ÙˆÙ„Ùƒ. Ø­Ø¯Ø¯ Ø¨Ù†Ø¸Ø±Ø© ÙˆØ§Ø­Ø¯Ø© Ø¨ÙˆØ§Ù„Øµ Ø§Ù„ØªØ£Ù…ÙŠÙ† ÙˆØ§Ù„ÙØ­ÙˆØµØ§Øª Ø§Ù„ÙÙ†ÙŠØ© Ø§Ù„Ù‚Ø±ÙŠØ¨Ø© Ù…Ù† Ø§Ù„Ø§Ù†ØªÙ‡Ø§Ø¡.',
    insuranceSchedule: 'Ø¬Ø¯ÙˆÙ„ Ø§Ù„ØªØ£Ù…ÙŠÙ†',
    insuranceScheduleDesc: 'Ø¨ÙˆØ§Ù„Øµ Ù‚Ø±ÙŠØ¨Ø© Ù…Ù† Ø§Ù„Ø§Ù†ØªÙ‡Ø§Ø¡ (Ø§Ù„Ø¹ØªØ¨Ø§Øª: 30ØŒ 15ØŒ 7 Ø£ÙŠØ§Ù…)',
    inspectionSchedule: 'Ø¬Ø¯ÙˆÙ„ Ø§Ù„ÙØ­ÙˆØµØ§Øª Ø§Ù„ÙÙ†ÙŠØ©',
    inspectionScheduleDesc: 'Ø§Ù„Ø²ÙŠØ§Ø±Ø§Øª Ø§Ù„ØªÙ†Ø¸ÙŠÙ…ÙŠØ© ÙˆØ¥Ø¹Ø§Ø¯Ø© Ø§Ù„ÙØ­Øµ Ø§Ù„Ù…Ø·Ù„ÙˆØ¨Ø©',
    expiry: 'Ø§Ù„Ø§Ù†ØªÙ‡Ø§Ø¡',
    timeRemaining: 'Ø§Ù„ÙˆÙ‚Øª Ø§Ù„Ù…ØªØ¨Ù‚ÙŠ',
    severity: 'Ø§Ù„Ø®Ø·ÙˆØ±Ø©',
    severityCritical: 'Ø­Ø±Ø¬',
    severityWarning: 'ØªØ­Ø°ÙŠØ±',
    severityInfo: 'Ù…Ø¹Ù„ÙˆÙ…Ø©',
    noInsuranceAlert: 'Ù„Ø§ ØªÙˆØ¬Ø¯ Ø¨ÙˆØ§Ù„Øµ ØªØ£Ù…ÙŠÙ† ÙÙŠ Ø­Ø§Ù„Ø© ØªÙ†Ø¨ÙŠÙ‡.',
    noInspectionAlert: 'Ù„Ø§ ØªÙˆØ¬Ø¯ ÙØ­ÙˆØµØ§Øª ÙÙ†ÙŠØ© ÙÙŠ Ø­Ø§Ù„Ø© ØªÙ†Ø¨ÙŠÙ‡.',
    compliant: 'Ù…Ø·Ø§Ø¨Ù‚',
    auto: 'ØªÙ„Ù‚Ø§Ø¦ÙŠ',
    alertStatus: 'Ø§Ù„ØªÙ†Ø¨ÙŠÙ‡ / Ø§Ù„Ø­Ø§Ù„Ø©',
  },

  // â”€â”€â”€ Fuel & Kilometrage â”€â”€â”€
  fuel: {
    pageTitle: 'Ù…ØªØ§Ø¨Ø¹Ø© Ø§Ù„ÙƒÙŠÙ„ÙˆÙ…ØªØ±Ø§Øª ÙˆØ§Ù„ÙˆÙ‚ÙˆØ¯',
    pageDesc: 'Ø£Ø¯Ø± Ù‚Ø±Ø§Ø¡Ø§Øª Ø§Ù„Ø¹Ø¯Ø§Ø¯ØŒ Ø§Ø³ØªÙ‡Ù„Ø§Ùƒ Ø§Ù„ÙˆÙ‚ÙˆØ¯ ÙˆØ§ÙƒØªØ´Ù Ø§Ù„Ø´Ø°ÙˆØ°.',
    exportOdometers: 'ØªØµØ¯ÙŠØ± Ø§Ù„Ø¹Ø¯Ø§Ø¯Ø§Øª',
    exportFuel: 'ØªØµØ¯ÙŠØ± Ø§Ù„ÙˆÙ‚ÙˆØ¯',
    inactivityDetected: 'ÙƒØ´Ù Ø¹Ø¯Ù… Ù†Ø´Ø§Ø· ÙƒÙŠÙ„ÙˆÙ…ØªØ±ÙŠ',
    inactivityDesc: 'Ù…Ø±ÙƒØ¨Ø§Øª Ø¨Ø¯ÙˆÙ† Ù†Ø´Ø§Ø· (Ø¹Ù‚Ø¯ Ø£Ùˆ Ù‚Ø±Ø§Ø¡Ø©) Ù„Ø£ÙƒØ«Ø± Ù…Ù†',
    daysOfInactivity: 'ÙŠÙˆÙ… Ø¹Ø¯Ù… Ù†Ø´Ø§Ø·',
    fleetOdometers: 'Ø§Ù„Ø£Ø³Ø·ÙˆÙ„ (Ø§Ù„Ø¹Ø¯Ø§Ø¯Ø§Øª)',
    vehiclesCount: 'Ù…Ø±ÙƒØ¨Ø§Øª',
    plateLabel: 'Ø§Ù„Ù„ÙˆØ­Ø©',
    statusFree: 'Ù…ØªØ§Ø­Ø©',
    statusRented: 'Ù…Ø¤Ø¬Ø±Ø©',
    statusGarage: 'ÙÙŠ Ø§Ù„Ù…Ø±Ø¢Ø¨',
    reading: 'Ù‚Ø±Ø§Ø¡Ø©',
    noVehicleSelected: 'Ù„Ù… ÙŠØªÙ… Ø§Ø®ØªÙŠØ§Ø± Ù…Ø±ÙƒØ¨Ø©',
    selectVehiclePrompt: 'Ø§Ø®ØªØ± Ù…Ø±ÙƒØ¨Ø© Ù…Ù† Ø§Ù„Ù‚Ø§Ø¦Ù…Ø© Ø§Ù„ÙŠÙ…Ù†Ù‰ Ù„Ø¹Ø±Ø¶ Ø³Ø¬Ù„ Ø§Ù„ÙƒÙŠÙ„ÙˆÙ…ØªØ±Ø§Øª Ø§Ù„ÙƒØ§Ù…Ù„ ÙˆØªØ¹Ø¨Ø¦Ø§Øª Ø§Ù„ÙˆÙ‚ÙˆØ¯ ÙˆØ§Ù„Ø§Ø³ØªÙ‡Ù„Ø§Ùƒ.',
    currentOdometer: 'Ø§Ù„Ø¹Ø¯Ø§Ø¯ Ø§Ù„Ø­Ø§Ù„ÙŠ',
    transmissionManual: 'ÙŠØ¯ÙˆÙŠ',
    transmissionAutomatic: 'Ø£ÙˆØªÙˆÙ…Ø§ØªÙŠÙƒ',
    registerFillup: 'ØªØ³Ø¬ÙŠÙ„ ØªØ¹Ø¨Ø¦Ø©',
    csvFuel: 'CSV Ø§Ù„ÙˆÙ‚ÙˆØ¯',
    csvKm: 'CSV ÙƒÙ…',
    consumptionTrend: 'ØªØ·ÙˆØ± Ø§Ù„Ø§Ø³ØªÙ‡Ù„Ø§Ùƒ (Ù„ØªØ±/100ÙƒÙ…)',
    insufficientData: 'Ø¨ÙŠØ§Ù†Ø§Øª ØºÙŠØ± ÙƒØ§ÙÙŠØ© Ù„Ø±Ø³Ù… Ø§Ù„Ù…Ù†Ø­Ù†Ù‰.',
    needTwoFillups: '(ÙŠØªØ·Ù„Ø¨ ØªØ¹Ø¨Ø¦ØªÙŠÙ† Ø¹Ù„Ù‰ Ø§Ù„Ø£Ù‚Ù„)',
    fuelHistory: 'Ø³Ø¬Ù„ ØªØ¹Ø¨Ø¦Ø§Øª Ø§Ù„ÙˆÙ‚ÙˆØ¯',
    odometer: 'Ø§Ù„Ø¹Ø¯Ø§Ø¯',
    liters: 'Ù„ØªØ±Ø§Øª',
    pricePerL: 'Ø§Ù„Ø³Ø¹Ø±/Ù„ØªØ±',
    totalCost: 'Ø§Ù„Ù…Ø¬Ù…ÙˆØ¹',
    average: 'Ø§Ù„Ù…Ø¹Ø¯Ù„',
    anomalyLabel: 'Ø´Ø°ÙˆØ°',
    noFuelLog: 'Ù„Ø§ ØªÙˆØ¬Ø¯ ØªØ¹Ø¨Ø¦Ø§Øª ÙˆÙ‚ÙˆØ¯ Ù…Ø³Ø¬Ù„Ø© Ù„Ù‡Ø°Ù‡ Ø§Ù„Ù…Ø±ÙƒØ¨Ø©.',
    kmTimeline: 'Ø³Ø¬Ù„ Ø§Ù„Ø¹Ø¯Ø§Ø¯ (Ù…ØªØ§Ø¨Ø¹Ø© Ø§Ù„ÙƒÙŠÙ„ÙˆÙ…ØªØ±Ø§Øª)',
    dateTime: 'Ø§Ù„ØªØ§Ø±ÙŠØ® / Ø§Ù„ÙˆÙ‚Øª',
    odometerValue: 'Ù‚ÙŠÙ…Ø© Ø§Ù„Ø¹Ø¯Ø§Ø¯',
    source: 'Ø§Ù„Ù…ØµØ¯Ø±',
    remarksEvent: 'Ù…Ù„Ø§Ø­Ø¸Ø§Øª / Ø§Ù„Ø­Ø¯Ø«',
    sourceManual: 'ÙŠØ¯ÙˆÙŠ',
    sourceFuel: 'ÙˆÙ‚ÙˆØ¯',
    sourceContract: 'Ø¹Ù‚Ø¯',
    noKmHistory: 'Ù„Ø§ ÙŠÙˆØ¬Ø¯ Ø³Ø¬Ù„ ÙƒÙŠÙ„ÙˆÙ…ØªØ±Ø§Øª.',
    manualOdometerReading: 'Ù‚Ø±Ø§Ø¡Ø© ÙŠØ¯ÙˆÙŠØ© Ù„Ù„Ø¹Ø¯Ø§Ø¯',
    readingDate: 'ØªØ§Ø±ÙŠØ® Ø§Ù„Ù‚Ø±Ø§Ø¡Ø©',
    odometerIndex: 'ÙÙ‡Ø±Ø³ Ø§Ù„Ø¹Ø¯Ø§Ø¯ (Ø§Ù„Ø¹Ø¯Ø§Ø¯ Ø¨Ø§Ù„ÙƒÙ…)',
    readingNotes: 'Ù…Ù„Ø§Ø­Ø¸Ø§Øª / Ø³Ø¨Ø¨ Ø§Ù„Ù‚Ø±Ø§Ø¡Ø©',
    readingPlaceholder: 'Ù‚Ø±Ø§Ø¡Ø© Ø´Ù‡Ø±ÙŠØ©ØŒ ØµÙŠØ§Ù†Ø©ØŒ Ø¥Ù„Ø®.',
    registerFuelFillup: 'ØªØ³Ø¬ÙŠÙ„ ØªØ¹Ø¨Ø¦Ø© ÙˆÙ‚ÙˆØ¯',
    kmIndex: 'ÙÙ‡Ø±Ø³ ÙƒÙ… (Ø§Ù„Ø¹Ø¯Ø§Ø¯)',
    volumeLiters: 'Ø§Ù„Ø­Ø¬Ù… (Ù„ØªØ±Ø§Øª)',
    pricePerLiter: 'Ø³Ø¹Ø± Ø§Ù„Ù„ØªØ± (â‚¬/Ù„ØªØ±)',
    stationName: 'Ø§Ø³Ù… Ù…Ø­Ø·Ø© Ø§Ù„ÙˆÙ‚ÙˆØ¯',
    fuelType: 'Ù†ÙˆØ¹ Ø§Ù„ÙˆÙ‚ÙˆØ¯',
    gasoline: 'Ø¨Ù†Ø²ÙŠÙ† (Ø®Ø§Ù„ÙŠ Ù…Ù† Ø§Ù„Ø±ØµØ§Øµ)',
    diesel: 'Ø¯ÙŠØ²Ù„',
    electric: 'ÙƒÙ‡Ø±Ø¨Ø§Ø¡',
    hybrid: 'Ù‡Ø¬ÙŠÙ†',
    lpg: 'ØºØ§Ø² Ù…Ø³Ø§Ù„',
    estimatedTotal: 'Ø§Ù„ØªÙƒÙ„ÙØ© Ø§Ù„Ø¥Ø¬Ù…Ø§Ù„ÙŠØ© Ø§Ù„Ù…Ù‚Ø¯Ø±Ø©:',
  },

  // â”€â”€â”€ Alerts â”€â”€â”€
  alerts: {
    pageTitle: 'Ù…Ø±ÙƒØ² Ø§Ù„ØªÙ†Ø¨ÙŠÙ‡Ø§Øª',
    pageDesc: 'Ø§Ù„ØªØ£Ù…ÙŠÙ†ØŒ Ø§Ù„ÙØ­ÙˆØµØ§Øª Ø§Ù„ÙÙ†ÙŠØ©ØŒ Ø§Ù„Ù…ÙˆØ§Ø¯ Ø§Ù„Ø§Ø³ØªÙ‡Ù„Ø§ÙƒÙŠØ© ÙˆØ±Ø®Øµ Ø§Ù„Ù‚ÙŠØ§Ø¯Ø©.',
    totalAlerts: 'Ø¥Ø¬Ù…Ø§Ù„ÙŠ Ø§Ù„ØªÙ†Ø¨ÙŠÙ‡Ø§Øª',
    critical: 'Ø­Ø±Ø¬',
    warning: 'ØªØ­Ø°ÙŠØ±',
    info: 'Ù…Ø¹Ù„ÙˆÙ…Ø©',
    searchPlaceholder: 'Ø¨Ø­Ø« ÙÙŠ Ø§Ù„Ù‡Ø¯ÙØŒ Ø§Ù„Ø±Ø³Ø§Ù„Ø©...',
    module: 'Ø§Ù„ÙˆØ­Ø¯Ø©',
    allModules: 'Ø¬Ù…ÙŠØ¹ Ø§Ù„ÙˆØ­Ø¯Ø§Øª',
    moduleInsurance: 'Ø§Ù„ØªØ£Ù…ÙŠÙ†',
    moduleInspection: 'Ø§Ù„ÙØ­Øµ Ø§Ù„ÙÙ†ÙŠ',
    moduleMaintenance: 'Ø§Ù„ØµÙŠØ§Ù†Ø© Ø§Ù„Ù…Ø¬Ø¯ÙˆÙ„Ø©',
    moduleOdometer: 'Ø¹Ø¯Ù… Ù†Ø´Ø§Ø· Ø§Ù„Ø¹Ø¯Ø§Ø¯',
    moduleLicense: 'Ø±Ø®ØµØ© Ø§Ù„Ø¹Ù…ÙŠÙ„',
    moduleConsumable: 'Ø§Ù„Ù…ÙˆØ§Ø¯ Ø§Ù„Ø§Ø³ØªÙ‡Ù„Ø§ÙƒÙŠØ©',
    alertsShown: 'ØªÙ†Ø¨ÙŠÙ‡(Ø§Øª) Ù…Ø¹Ø±ÙˆØ¶Ø©',
    severity: 'Ø§Ù„Ø®Ø·ÙˆØ±Ø©',
    typeLabel: 'Ø§Ù„Ù†ÙˆØ¹',
    concerns: 'ÙŠØ®Øµ',
    deadline: 'Ø§Ù„Ù…ÙˆØ¹Ø¯ Ø§Ù„Ù†Ù‡Ø§Ø¦ÙŠ',
    severityCritical: 'Ø­Ø±Ø¬',
    severityWarning: 'ØªØ­Ø°ÙŠØ±',
    severityInfo: 'Ù…Ø¹Ù„ÙˆÙ…Ø©',
    typeInactivityKm: 'Ø¹Ø¯Ù… Ù†Ø´Ø§Ø· ÙƒÙ…',
    typeLicense: 'Ø§Ù„Ø±Ø®ØµØ©',
    typeInspection: 'Ø§Ù„ÙØ­Øµ',
    typeInsurance: 'Ø§Ù„ØªØ£Ù…ÙŠÙ†',
    typeConsumable: 'Ø§Ø³ØªÙ‡Ù„Ø§ÙƒÙŠ',
    noAlerts: 'Ù„Ø§ ØªÙˆØ¬Ø¯ ØªÙ†Ø¨ÙŠÙ‡Ø§Øª Ù„Ù„Ø¥Ø¨Ù„Ø§Øº',
    noAlertsDesc: 'Ø¬Ù…ÙŠØ¹ Ø§Ù„Ù…ÙˆØ§Ø¹ÙŠØ¯ ÙˆØ§Ù„ÙØ­ÙˆØµØ§Øª Ù…Ø­Ø¯Ù‘Ø«Ø©.',
    alertsCount: 'ØªÙ†Ø¨ÙŠÙ‡(Ø§Øª)',
  },

  // â”€â”€â”€ Reports â”€â”€â”€
  reports: {
    pageTitle: 'Ø§Ù„ØªÙ‚Ø§Ø±ÙŠØ± ÙˆØ§Ù„ØªØ­Ù„ÙŠÙ„Ø§Øª Ø§Ù„ØªØ´ØºÙŠÙ„ÙŠØ©',
    pageDesc: 'Ù‚ÙŠÙ‘Ù… Ø§Ù„Ø±Ø¨Ø­ÙŠØ©ØŒ Ù…Ø¹Ø¯Ù„ Ø§Ø³ØªØ®Ø¯Ø§Ù… Ø§Ù„Ø£Ø³Ø·ÙˆÙ„ØŒ Ø§Ù„ØªÙƒÙ„ÙØ© Ø§Ù„Ø¥Ø¬Ù…Ø§Ù„ÙŠØ© Ù„Ù„Ù…Ù„ÙƒÙŠØ© (TCO) ÙˆØªØ§Ø¨Ø¹ Ø§Ù„ÙÙˆØ§ØªÙŠØ± ØºÙŠØ± Ø§Ù„Ù…Ø¯ÙÙˆØ¹Ø©.',
    exportProfitability: 'ØªØµØ¯ÙŠØ± Ø§Ù„Ø±Ø¨Ø­ÙŠØ© (CSV)',
    grossRevenue: 'Ø§Ù„Ø¥ÙŠØ±Ø§Ø¯Ø§Øª Ø§Ù„Ø¥Ø¬Ù…Ø§Ù„ÙŠØ©',
    paidLabel: 'Ù…Ø¯ÙÙˆØ¹',
    dueLabel: 'Ù…Ø³ØªØ­Ù‚',
    avgUtilization: 'Ù…Ø¹Ø¯Ù„ Ø§Ù„Ø§Ø³ØªØ®Ø¯Ø§Ù… Ø§Ù„Ù…ØªÙˆØ³Ø·',
    utilizationDesc: 'ÙˆÙ‚Øª Ø§Ù„ØªØ£Ø¬ÙŠØ± / Ø§Ù„ÙˆÙ‚Øª Ø§Ù„Ø¥Ø¬Ù…Ø§Ù„ÙŠ.',
    fleetSize: 'Ø­Ø¬Ù… Ø§Ù„Ø£Ø³Ø·ÙˆÙ„',
    vehiclesLabel: 'Ù…Ø±ÙƒØ¨Ø§Øª',
    rented: 'Ù…Ø¤Ø¬Ø±Ø©',
    garage: 'ÙÙŠ Ø§Ù„Ù…Ø±Ø¢Ø¨',
    fleetAvailability: 'Ø­Ø§Ù„Ø© ØªÙˆÙØ± Ø§Ù„Ø£Ø³Ø·ÙˆÙ„',
    financialRecovery: 'Ù…Ø¹Ø¯Ù„ Ø§Ù„ØªØ­ØµÙŠÙ„ Ø§Ù„Ù…Ø§Ù„ÙŠ',
    startDate: 'Ø§Ù„Ø¨Ø¯Ø§ÙŠØ©',
    endDate: 'Ø§Ù„Ù†Ù‡Ø§ÙŠØ©',
    profitabilityAnalysis: 'ØªØ­Ù„ÙŠÙ„ Ø§Ù„Ø±Ø¨Ø­ÙŠØ© Ø­Ø³Ø¨ Ø§Ù„Ù…Ø±ÙƒØ¨Ø© (TCO)',
    utilizationRate: 'Ù…Ø¹Ø¯Ù„ Ø§Ù„Ø§Ø³ØªØ®Ø¯Ø§Ù…',
    revenueLabel: 'Ø§Ù„Ø¥ÙŠØ±Ø§Ø¯Ø§Øª',
    maintenanceLabel: 'Ø§Ù„ØµÙŠØ§Ù†Ø©',
    fuelLabel: 'Ø§Ù„ÙˆÙ‚ÙˆØ¯',
    insuranceLabel: 'Ø§Ù„ØªØ£Ù…ÙŠÙ†',
    tco: 'TCO (Ø§Ù„ØªÙƒÙ„ÙØ© Ø§Ù„Ø¥Ø¬Ù…Ø§Ù„ÙŠØ©)',
    netProfit: 'ØµØ§ÙÙŠ Ø§Ù„Ø±Ø¨Ø­',
    noProfitabilityData: 'Ù„Ø§ ØªÙˆØ¬Ø¯ Ø¨ÙŠØ§Ù†Ø§Øª Ø±Ø¨Ø­ÙŠØ© Ù…ØªØ§Ø­Ø©.',
    topClients: 'Ø£ÙØ¶Ù„ Ø§Ù„Ø¹Ù…Ù„Ø§Ø¡ (Ø­Ø³Ø¨ Ø§Ù„Ø­Ø¬Ù… ÙˆØ§Ù„Ø¥ÙŠØ±Ø§Ø¯Ø§Øª)',
    clientName: 'Ø§Ø³Ù… Ø§Ù„Ø¹Ù…ÙŠÙ„',
    rentals: 'Ø§Ù„Ø¥ÙŠØ¬Ø§Ø±Ø§Øª',
    totalRented: 'Ø¥Ø¬Ù…Ø§Ù„ÙŠ Ø§Ù„Ù…Ø¤Ø¬Ø±',
    noClients: 'Ù„Ø§ ÙŠÙˆØ¬Ø¯ Ø¹Ù…Ù„Ø§Ø¡.',
    latePayments: 'Ø§Ù„ÙÙˆØ§ØªÙŠØ± ÙˆØ§Ù„Ø¹Ù‚ÙˆØ¯ Ø§Ù„Ù…ØªØ£Ø®Ø±Ø©',
    contractNo: 'Ø±Ù‚Ù… Ø§Ù„Ø¹Ù‚Ø¯',
    amountDue: 'Ø§Ù„Ù…Ø¨Ù„Øº Ø§Ù„Ù…Ø³ØªØ­Ù‚',
    paymentStatus: 'Ø­Ø§Ù„Ø© Ø§Ù„Ø¯ÙØ¹',
    paymentUnpaid: 'ØºÙŠØ± Ù…Ø¯ÙÙˆØ¹',
    paymentPartial: 'Ø¬Ø²Ø¦ÙŠ',
    noUnpaid: 'ØªÙ‡Ø§Ù†ÙŠÙ†Ø§ØŒ Ù„Ø§ ØªÙˆØ¬Ø¯ ÙÙˆØ§ØªÙŠØ± ØºÙŠØ± Ù…Ø¯ÙÙˆØ¹Ø©!',
  },

  // â”€â”€â”€ Settings â”€â”€â”€
  settings: {
    pageTitle: 'Ø§Ù„Ø¥Ø¹Ø¯Ø§Ø¯Ø§Øª Ø§Ù„Ø¹Ø§Ù…Ø©',
    pageDesc: 'Ø§Ø¶Ø¨Ø· Ù‚ÙˆØ§Ø¹Ø¯ Ø§Ù„ØªÙ†Ø¨ÙŠÙ‡Ø§ØªØŒ ÙƒØªØ§Ù„ÙˆØ¬ Ø§Ù„Ø®ÙŠØ§Ø±Ø§Øª Ø§Ù„Ø¥Ø¶Ø§ÙÙŠØ©ØŒ Ø§Ù„Ø¹Ù…Ù„Ø§ØªØŒ ÙˆØ¹Ø¯Ù‘Ù„ Ø¨ÙŠØ§Ù†Ø§Øª Ø§Ù„Ø§Ø¹ØªÙ…Ø§Ø¯.',
    thresholdsConfig: 'ØªÙƒÙˆÙŠÙ† Ø§Ù„Ø¹ØªØ¨Ø§Øª ÙˆØ§Ù„ØªÙ†Ø¨ÙŠÙ‡Ø§Øª',
    thresholdsDesc: 'Ø­Ø¯Ø¯ Ù…ØªÙ‰ ÙŠØªÙ… ØªØ´ØºÙŠÙ„ Ø§Ù„ØªÙ†Ø¨ÙŠÙ‡Ø§Øª Ù„Ù„ØµÙŠØ§Ù†Ø© ÙˆØ§Ù„ØªØ£Ù…ÙŠÙ† ÙˆØ§Ù„ÙØ­ÙˆØµØ§Øª ÙˆØ§Ù„Ù…ÙˆØ§Ø¯ Ø§Ù„Ø§Ø³ØªÙ‡Ù„Ø§ÙƒÙŠØ©.',
    currencySymbol: 'Ø±Ù…Ø² Ø§Ù„Ø¹Ù…Ù„Ø©',
    dateFormat: 'ØªÙ†Ø³ÙŠÙ‚ Ø§Ù„ØªØ§Ø±ÙŠØ®',
    kmInactivity: 'Ø¹Ø¯Ù… Ù†Ø´Ø§Ø· Ø§Ù„Ø¹Ø¯Ø§Ø¯ (Ø£ÙŠØ§Ù…)',
    maintenanceAlert: 'Ø§Ù„ØµÙŠØ§Ù†Ø© Ø§Ù„ÙˆÙ‚Ø§Ø¦ÙŠØ© (ØªÙ†Ø¨ÙŠÙ‡ Ù‚Ø¨Ù„ N ÙŠÙˆÙ…)',
    insuranceThresholds: 'Ø¹ØªØ¨Ø§Øª Ø§Ù„ØªØ£Ù…ÙŠÙ† (Ø§Ù„Ø£ÙŠØ§Ù… Ø§Ù„Ù…ØªØ¨Ù‚ÙŠØ© - CSV)',
    inspectionThresholds: 'Ø¹ØªØ¨Ø§Øª Ø§Ù„ÙØ­Øµ Ø§Ù„ÙÙ†ÙŠ (Ø£ÙŠØ§Ù… - CSV)',
    consumableAlertKm: 'Ø§Ù„Ù…ÙˆØ§Ø¯ Ø§Ù„Ø§Ø³ØªÙ‡Ù„Ø§ÙƒÙŠØ© (ØªÙ†Ø¨ÙŠÙ‡ Ù‚Ø¨Ù„ N ÙƒÙ…)',
    consumableAlertDays: 'Ø§Ù„Ù…ÙˆØ§Ø¯ Ø§Ù„Ø§Ø³ØªÙ‡Ù„Ø§ÙƒÙŠØ© (ØªÙ†Ø¨ÙŠÙ‡ Ù‚Ø¨Ù„ N ÙŠÙˆÙ…)',
    saveConfig: 'Ø­ÙØ¸ Ø§Ù„Ø¥Ø¹Ø¯Ø§Ø¯Ø§Øª',
    referenceData: 'Ø¥Ø¯Ø§Ø±Ø© Ø§Ù„Ø¨ÙŠØ§Ù†Ø§Øª Ø§Ù„Ù…Ø±Ø¬Ø¹ÙŠØ©',
    referenceDataDesc: 'Ø£Ø¯Ø± Ø§Ù„Ù‚ÙˆØ§Ø¦Ù… Ø§Ù„Ù…Ù†Ø³Ø¯Ù„Ø© ÙˆØ§Ù„ÙƒØªØ§Ù„ÙˆØ¬Ø§Øª Ø§Ù„Ù…Ø³ØªØ®Ø¯Ù…Ø© ÙÙŠ Ø¬Ù…ÙŠØ¹ Ø£Ù†Ø­Ø§Ø¡ Ø§Ù„ØªØ·Ø¨ÙŠÙ‚.',
    vehicleCategories: 'ÙØ¦Ø§Øª Ø§Ù„Ù…Ø±ÙƒØ¨Ø§Øª',
    vehicleCategoriesDesc: 'Ø­Ø¯Ø¯ Ø£Ù†ÙˆØ§Ø¹ Ø§Ù„Ù…Ø±ÙƒØ¨Ø§Øª Ø§Ù„Ù…ØªØ§Ø­Ø© Ø¹Ù†Ø¯ ØªØ³Ø¬ÙŠÙ„ Ù…Ø±ÙƒØ¨Ø© Ø¬Ø¯ÙŠØ¯Ø© (Ù…Ø«Ù„: Ø³ÙŠØ¯Ø§Ù†ØŒ SUVØŒ Ø´Ø§Ø­Ù†Ø©).',
    addCategory: 'Ø¥Ø¶Ø§ÙØ© ÙØ¦Ø©...',
    fuelTypes: 'Ø£Ù†ÙˆØ§Ø¹ Ø§Ù„ÙˆÙ‚ÙˆØ¯',
    fuelTypesDesc: 'Ù‚Ø§Ø¦Ù…Ø© Ø£Ù†ÙˆØ§Ø¹ Ø§Ù„ÙˆÙ‚ÙˆØ¯ Ø§Ù„Ù‚Ø§Ø¨Ù„Ø© Ù„Ù„Ø§Ø®ØªÙŠØ§Ø± ÙÙŠ Ù†Ù…Ø§Ø°Ø¬ Ø§Ù„Ù…Ø±ÙƒØ¨Ø§Øª (Ù…Ø«Ù„: Ø¨Ù†Ø²ÙŠÙ†ØŒ Ø¯ÙŠØ²Ù„ØŒ ÙƒÙ‡Ø±Ø¨Ø§Ø¡).',
    addFuel: 'Ø¥Ø¶Ø§ÙØ© Ù†ÙˆØ¹ ÙˆÙ‚ÙˆØ¯...',
    maintenanceTypes: 'Ø£Ù†ÙˆØ§Ø¹ ØªØ¯Ø®Ù„Ø§Øª Ø§Ù„ØµÙŠØ§Ù†Ø©',
    maintenanceTypesDesc: 'ÙØ¦Ø§Øª Ø£Ø¹Ù…Ø§Ù„ Ø§Ù„ØµÙŠØ§Ù†Ø© (Ù…Ø«Ù„: ÙˆÙ‚Ø§Ø¦ÙŠØŒ ØªØµØ­ÙŠØ­ÙŠØŒ Ø¥ØµÙ„Ø§Ø­ Ø­ÙˆØ§Ø¯Ø«).',
    addIntervention: 'Ø¥Ø¶Ø§ÙØ© ØªØ¯Ø®Ù„...',
    extrasCatalog: 'ÙƒØªØ§Ù„ÙˆØ¬ Ø§Ù„Ø®ÙŠØ§Ø±Ø§Øª ÙˆØ§Ù„Ù…Ù„Ø­Ù‚Ø§Øª',
    extrasCatalogDesc: 'Ø®ÙŠØ§Ø±Ø§Øª Ø¥Ø¶Ø§ÙÙŠØ© Ù…ØªØ§Ø­Ø© Ø¹Ù†Ø¯ Ø¥Ù†Ø´Ø§Ø¡ Ø¹Ù‚ÙˆØ¯ Ø§Ù„Ø¥ÙŠØ¬Ø§Ø± (Ù…Ø«Ù„: GPSØŒ Ù…Ù‚Ø¹Ø¯ Ø£Ø·ÙØ§Ù„).',
    optionName: 'Ø§Ø³Ù… Ø§Ù„Ø®ÙŠØ§Ø±',
    optionRate: 'Ø§Ù„ØªØ¹Ø±ÙØ© (â‚¬/ÙŠÙˆÙ…)',
    saveRefData: 'Ø­ÙØ¸ Ø§Ù„Ø¨ÙŠØ§Ù†Ø§Øª Ø§Ù„Ù…Ø±Ø¬Ø¹ÙŠØ©',
    itemsConfigured: 'Ø¹Ù†Ø§ØµØ± Ù…ÙƒÙˆÙ‘Ù†Ø©',
    noItems: 'Ù„Ù… ÙŠØªÙ… ØªÙƒÙˆÙŠÙ† Ø£ÙŠ Ø¹Ù†Ø§ØµØ± Ø¨Ø¹Ø¯.',
    adminProfile: 'Ø§Ù„Ù…Ù„Ù Ø§Ù„Ø´Ø®ØµÙŠ Ù„Ù„Ù…Ø¯ÙŠØ±',
    adminProfileDesc: 'Ø­Ø¯Ù‘Ø« Ø§Ø³Ù…Ùƒ Ø§Ù„Ù…Ø¹Ø±ÙˆØ¶ ÙˆÙ…Ø¹Ù„ÙˆÙ…Ø§Øª Ø­Ø³Ø§Ø¨Ùƒ.',
    usernameReadOnly: 'Ø§Ø³Ù… Ø§Ù„Ù…Ø³ØªØ®Ø¯Ù… (Ù„Ù„Ù‚Ø±Ø§Ø¡Ø© ÙÙ‚Ø·)',
    fullName: 'Ø§Ù„Ø§Ø³Ù… Ø§Ù„ÙƒØ§Ù…Ù„',
    updateProfile: 'ØªØ­Ø¯ÙŠØ« Ø§Ù„Ù…Ù„Ù Ø§Ù„Ø´Ø®ØµÙŠ',
    security: 'Ø§Ù„Ø£Ù…Ø§Ù† ÙˆØ§Ù„Ù…ØµØ§Ø¯Ù‚Ø©',
    securityDesc: 'ØºÙŠÙ‘Ø± ÙƒÙ„Ù…Ø© Ø§Ù„Ù…Ø±ÙˆØ± Ù„ØªØ£Ù…ÙŠÙ† Ø­Ø³Ø§Ø¨Ùƒ.',
    currentPassword: 'ÙƒÙ„Ù…Ø© Ø§Ù„Ù…Ø±ÙˆØ± Ø§Ù„Ø­Ø§Ù„ÙŠØ©',
    newPassword: 'ÙƒÙ„Ù…Ø© Ø§Ù„Ù…Ø±ÙˆØ± Ø§Ù„Ø¬Ø¯ÙŠØ¯Ø©',
    confirmPassword: 'ØªØ£ÙƒÙŠØ¯ ÙƒÙ„Ù…Ø© Ø§Ù„Ù…Ø±ÙˆØ± Ø§Ù„Ø¬Ø¯ÙŠØ¯Ø©',
    changePassword: 'ØªØºÙŠÙŠØ± ÙƒÙ„Ù…Ø© Ø§Ù„Ù…Ø±ÙˆØ±',
  },

  // â”€â”€â”€ Statuses (reusable) â”€â”€â”€
  statuses: {
    available: 'Ù…ØªØ§Ø­',
    rented: 'Ù…Ø¤Ø¬Ø±',
    inMaintenance: 'ÙÙŠ Ø§Ù„ØµÙŠØ§Ù†Ø©',
    reserved: 'Ù…Ø­Ø¬ÙˆØ²',
    immobilized: 'Ù…ØªÙˆÙ‚Ù',
    draft: 'Ù…Ø³ÙˆØ¯Ø©',
    active: 'Ù†Ø´Ø·',
    completed: 'Ù…ÙƒØªÙ…Ù„',
    cancelled: 'Ù…Ù„ØºÙŠ',
    scheduled: 'Ù…Ø¬Ø¯ÙˆÙ„',
    inProgress: 'Ù‚ÙŠØ¯ Ø§Ù„ØªÙ†ÙÙŠØ°',
    unpaid: 'ØºÙŠØ± Ù…Ø¯ÙÙˆØ¹',
    partiallyPaid: 'Ù…Ø¯ÙÙˆØ¹ Ø¬Ø²Ø¦ÙŠØ§Ù‹',
    paid: 'Ù…Ø¯ÙÙˆØ¹',
    valid: 'Ø³Ø§Ø±ÙŠ',
    expiringSoon: 'ÙŠÙ†ØªÙ‡ÙŠ Ù‚Ø±ÙŠØ¨Ø§Ù‹',
    expired: 'Ù…Ù†ØªÙ‡ÙŠ',
    ok: 'Ù…Ø·Ø§Ø¨Ù‚',
    warningStatus: 'Ù‚Ø±ÙŠØ¨Ø§Ù‹',
    due: 'ÙŠØ¬Ø¨ Ø§Ù„Ø§Ø³ØªØ¨Ø¯Ø§Ù„',
  },
};

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\i18n\en.ts
```typescript
export const EN: Record<string, any> = {
  // â”€â”€â”€ Common / Shared â”€â”€â”€
  common: {
    save: 'Save',
    cancel: 'Cancel',
    delete: 'Delete',
    edit: 'Edit',
    add: 'Add',
    close: 'Close',
    search: 'Search',
    actions: 'Actions',
    status: 'Status',
    date: 'Date',
    notes: 'Notes',
    remarks: 'Remarks',
    yes: 'Yes',
    no: 'No',
    confirm: 'Confirm',
    loading: 'Loading...',
    uploading: 'Uploading...',
    noData: 'No data',
    all: 'All',
    viewAll: 'View all',
    export: 'Export',
    print: 'Print',
    refresh: 'Refresh',
    back: 'Back',
    next: 'Next',
    previous: 'Previous',
    page: 'page',
    of: 'of',
    total: 'Total',
    validate: 'Validate',
    perDay: '/day',
    days: 'days',
    months: 'months',
    km: 'km',
    vehicle: 'Vehicle',
    client: 'Client',
    amount: 'Amount',
    cost: 'Cost',
    type: 'Type',
    description: 'Description',
    document: 'Document',
    photo: 'Photo',
    none: 'None',
    see: 'View',
    manage: 'Manage',
    reports: 'Reports',
    from: 'From',
    to: 'To',
    admin: 'Admin',
    logout: 'Log out',
    uploadInProgress: 'Upload in progress...',
    deleteConfirm: 'Are you sure you want to delete?',
    errorOccurred: 'An error occurred',
    savedSuccessfully: 'Saved successfully',
  },

  // â”€â”€â”€ Sidebar / Navigation â”€â”€â”€
  sidebar: {
    brand: 'Fleet Manager',
    dashboard: 'Dashboard',
    vehicles: 'Vehicles',
    clients: 'Clients',
    contracts: 'Contracts',
    kilometrage: 'Kilometrage',
    maintenance: 'Maintenance',
    consumables: 'Consumables',
    insuranceControl: 'Insurance & Control',
    alerts: 'Alerts',
    reportsAnalytics: 'Reports & Analytics',
    settings: 'Settings',
  },

  // â”€â”€â”€ Topbar â”€â”€â”€
  topbar: {
    title: 'Fleet Management System',
    activeAlerts: 'Active Alerts',
    noActiveAlerts: 'No active alerts.',
    language: 'Language',
  },

  // â”€â”€â”€ Login â”€â”€â”€
  login: {
    title: 'Fleet Manager',
    subtitle: 'Rental fleet management',
    username: 'Username',
    password: 'Password',
    loginBtn: 'Log in',
    loggingIn: 'Logging in...',
    defaultCredentials: 'Default:',
    errorRequired: 'Please enter your username and password.',
    errorInvalid: 'Invalid username or password.',
    errorServer: 'An error occurred during login. Please try again.',
  },

  // â”€â”€â”€ Dashboard â”€â”€â”€
  dashboard: {
    pageTitle: 'Dashboard',
    pageDesc: 'Fleet and operations overview.',
    fleet: 'Fleet',
    registeredVehicles: 'registered vehicles',
    activeRentals: 'Active Rentals',
    available: 'available',
    revenue: 'Revenue',
    paid: 'paid',
    alerts: 'Alerts',
    critical: 'critical',
    fleetAvailability: 'Fleet Availability',
    currentDistribution: 'Current distribution',
    priorityAlerts: 'Priority Alerts',
    priorityAlertsDesc: 'Maintenance, insurance, inspections',
    noActiveAlert: 'No active alerts.',
    unpaid: 'Unpaid',
    unpaidDesc: 'Contracts pending payment',
    contractNo: 'Contract #',
    departure: 'Departure',
    paymentUnpaid: 'Unpaid',
    paymentPartial: 'Partial',
    noUnpaidContracts: 'No unpaid contracts.',
  },

  // â”€â”€â”€ Vehicles â”€â”€â”€
  vehicles: {
    pageTitle: 'Fleet Management',
    pageDesc: 'Register and track fleet vehicles.',
    addVehicle: 'Add Vehicle',
    searchPlaceholder: 'Search by brand, model, plate, VIN...',
    allTypes: 'All Types',
    allFuels: 'All Fuels',
    allStatuses: 'All Statuses',
    noVehiclesFound: 'No vehicles match the criteria.',
    statusAvailable: 'Available',
    statusRented: 'Rented',
    statusMaintenance: 'In Service',
    statusReserved: 'Reserved',
    statusImmobilized: 'Immobilized',
    engine: 'Engine',
    gearbox: 'Gearbox',
    manual: 'Manual',
    auto: 'Auto',
    automatic: 'Automatic',
    folder: 'File',
    editVehicle: 'Edit Vehicle',
    newVehicle: 'New Vehicle',
    matricule: 'Plate Number',
    brand: 'Brand',
    model: 'Model',
    year: 'Year',
    fuelType: 'Fuel Type',
    transmission: 'Transmission',
    vin: 'Chassis (VIN)',
    engineNumber: 'Engine Number',
    color: 'Color',
    seats: 'Seats',
    dailyRate: 'Daily Rate',
    purchasePrice: 'Purchase Price',
    initialKm: 'Initial Km',
    remarks: 'Remarks',
    deleteConfirm: 'Are you sure you want to remove this vehicle from the active fleet?',
    errorDelete: 'Error deleting vehicle',
    errorUpdate: 'Error updating vehicle',
    errorCreate: 'Error creating vehicle',
    tracking: 'Tracking',
    consumablesTab: 'Consumables',
    insuranceTab: 'Insurance',
    inspectionsTab: 'Inspections',
    fuelTab: 'Fuel',
    kmHistoryTab: 'Kilometrage',
    consumablesState: 'Consumables Status',
    consumable: 'Consumable',
    lastReplacement: 'Last Replacement',
    traveled: 'Traveled',
    interval: 'Interval',
    noLog: 'No log',
    insuranceHistory: 'Insurance History',
    insurer: 'Insurer',
    policyNo: 'Policy #',
    coverage: 'Coverage',
    validity: 'Validity',
    premiumValue: 'Premium / Value',
    statusValid: 'Valid',
    statusExpiringSoon: 'Soon',
    statusExpired: 'Expired',
    technicalInspections: 'Technical Inspections',
    expiration: 'Expiration',
    center: 'Center',
    result: 'Result',
    resultPass: 'Passed',
    resultConditional: 'Conditional',
    resultFail: 'Failed',
    fuelTracking: 'Fuel Tracking',
    counter: 'Counter',
    volume: 'Volume',
    consumption: 'Consumption',
    anomaly: 'Anomaly',
    overConsumption: 'Over-consumption',
    kmHistory: 'Km History',
    reading: 'Reading',
    newInsurancePolicy: 'New Insurance Policy',
    coverageThirdParty: 'Third Party',
    coverageComprehensive: 'Comprehensive',
    coverageFleet: 'Fleet',
    premium: 'Premium',
    effectiveDate: 'Effective Date',
    documentPdf: 'Document (PDF)',
    newInspection: 'New Technical Inspection',
    inspectionDate: 'Inspection Date',
    resultFavorable: 'Passed',
    resultCounterVisit: 'Conditional',
    resultUnfavorable: 'Failed',
    centerAddress: 'Center Address',
    newFuelFillup: 'New Fuel Fill-up',
    counterKm: 'Counter (km)',
    volumeL: 'Volume (L)',
    pricePerL: 'Price/L',
    station: 'Station',
    fillup: 'Fill-up',
    kmReading: 'Km Reading',
  },

  // â”€â”€â”€ Clients â”€â”€â”€
  clients: {
    pageTitle: 'Client Database',
    pageDesc: 'Manage client records, contact details, driving licenses and rental history.',
    addClient: 'Add Client',
    searchPlaceholder: 'Search by name, phone, email, ID...',
    identity: 'Identity',
    contact: 'Contact',
    license: 'License',
    licenseValidity: 'Validity',
    expired: 'Expired',
    history: 'History',
    noClientFound: 'No client found',
    noClientDesc: 'Add a client or modify your search.',
    totalClients: 'client(s) total',
    editClient: 'Edit Client',
    newClient: 'Create New Client',
    fullName: 'Full Name',
    nationalId: 'National ID / Passport',
    dateOfBirth: 'Date of Birth',
    licenseNumber: 'License Number',
    licenseCategory: 'License Category',
    category: 'Category',
    licenseIssueDate: 'License Issue Date',
    licenseExpiryDate: 'License Expiry Date',
    phone: 'Phone Number',
    email: 'Email Address',
    address: 'Address',
    notesRemarks: 'Notes / Remarks',
    clientFile: 'Client File',
    nationalIdLabel: 'National ID',
    phoneLabel: 'Phone',
    licenseLabel: 'License',
    expirationLabel: 'Expiration',
    adminNotes: 'Administrative Notes:',
    rentalHistory: 'Rental History',
    contractNo: 'Contract #',
    vehicleLabel: 'Vehicle',
    registration: 'Registration',
    period: 'Period',
    amountLabel: 'Amount',
    statusCompleted: 'Completed',
    statusActive: 'Active',
    statusDraft: 'Draft',
    statusCancelled: 'Cancelled',
    noRentalHistory: 'No rental contracts on record for this client.',
  },

  // â”€â”€â”€ Contracts â”€â”€â”€
  contracts: {
    pageTitle: 'Rental Contract Management',
    pageDesc: 'Create contracts, track departures, record vehicle returns and manage client billing.',
    newContract: 'New Contract',
    searchPlaceholder: 'Search by contract #, client, plate...',
    allStatuses: 'All Statuses',
    statusDraft: 'Draft',
    statusActive: 'Active',
    statusCompleted: 'Completed',
    statusCancelled: 'Cancelled',
    allInvoices: 'All Invoices',
    unpaid: 'Unpaid',
    partial: 'Partial',
    paid: 'Paid',
    contract: 'Contract',
    period: 'Period',
    statuses: 'Statuses',
    noContractFound: 'No contracts found',
    noContractDesc: 'Create a new contract or adjust the filters.',
    totalContracts: 'contract(s)',
    returnVehicle: 'Return vehicle',
    returnBtn: 'Return',
    editContract: 'Edit Rental Contract',
    newContractDialog: 'New Rental Contract',
    selectClient: 'Select Client',
    chooseClient: 'Choose a client...',
    selectVehicle: 'Select Vehicle',
    chooseVehicle: 'Choose a vehicle...',
    contractType: 'Contract Type',
    dailyRate: 'Daily Rate',
    startDateTime: 'Start Date & Time',
    expectedReturn: 'Expected Return Date & Time',
    calculatedDays: 'Calculated Days',
    rentalAmount: 'Rental Amount',
    extraFees: 'Extra Fees',
    discount: 'Discount',
    totalNetDue: 'Total Net Due',
    deposit: 'Deposit',
    paymentMethod: 'Payment Method',
    initialStatus: 'Set Initial Status',
    draftReservation: 'Draft (Simple reservation)',
    activateNow: 'Activate now (Vehicle departure)',
    initialBilling: 'Initial Billing Status',
    unpaidPending: 'Unpaid (Pending)',
    depositPaid: 'Deposit paid (Partial)',
    fullyPaid: 'Paid (Full payment)',
    adminNotes: 'Administrative Notes',
    saveContract: 'Save Contract',
    errorUpdate: 'Error updating contract',
    errorCreate: 'Error creating contract',
    closeContract: 'Close Contract',
    departureOdometer: 'Departure odometer:',
    expectedReturnDate: 'Expected return:',
    returnIndex: 'Return odometer (Km)',
    actualReturnDate: 'Actual return date',
    fuelPenalty: 'Fuel Penalty',
    damageFees: 'Damage Fees',
    sendToMaintenance: 'Send vehicle to maintenance (damage reported)',
    returnNotes: 'Return notes / Findings',
    returnNotesPlaceholder: 'Cleanliness, fuel level, scratches...',
    validateReturn: 'Validate Return',
    errorReturn: 'Error processing vehicle return',
    licenseWarning: 'Warning: This client\'s license expired on',
    printPreview: 'Print Preview',
    invoiceBrand: 'FLEET AUTO RENTAL',
    invoiceSubtitle: 'Premium Car Rental Service',
    contractInvoice: 'CONTRACT & INVOICE',
    tenant: 'Tenant / Client:',
    vehicleLabel: 'Vehicle:',
    registrationLabel: 'Registration',
    departureCounter: 'Departure counter',
    returnCounter: 'Return counter',
    dailyRateLabel: 'Daily rate',
    daysLabel: 'Days',
    totalHT: 'Total',
    vehicleRental: 'Vehicle rental',
    extrasAccessories: 'Extras / Accessories',
    penalties: 'Penalties (Late, Damage, Fuel)',
    paymentLabel: 'Payment:',
    mode: 'Method',
    statusLabel: 'Status',
    subtotal: 'Subtotal:',
    reduction: 'Discount:',
    netAmountDue: 'Net Amount Due:',
    tenantSignature: 'Tenant Signature',
    agencySignature: 'Agency Signature',
    amountPaid: 'Amount Paid',
    remainingDue: 'Remaining Due',
  },

  // â”€â”€â”€ Maintenance â”€â”€â”€
  maintenance: {
    pageTitle: 'Maintenance Tracking',
    pageDesc: 'Record mechanical interventions (preventive or corrective) and schedule upcoming services.',
    maintenanceCalendar: 'Maintenance Calendar',
    registerIntervention: 'Register Intervention',
    intervention: 'Intervention',
    dates: 'Dates',
    workshop: 'Workshop',
    invoice: 'Invoice',
    nextScheduled: 'Next',
    statusScheduled: 'Scheduled',
    statusInProgress: 'In Progress',
    statusCompleted: 'Completed',
    noMaintenance: 'No maintenance recorded',
    noMaintenanceDesc: 'Record an intervention to start tracking.',
    interventions: 'intervention(s)',
    editMaintenance: 'Edit Maintenance Record',
    newIntervention: 'Record Intervention',
    interventionType: 'Intervention Type',
    interventionDate: 'Intervention Date',
    nextMaintenance: 'Next Maintenance',
    counterKm: 'Counter Km (Intervention)',
    interventionStatus: 'Intervention Status',
    laborCost: 'Labor Cost',
    partsCost: 'Parts Cost',
    workshopName: 'Workshop / Garage Name',
    workshopContact: 'Workshop Contact',
    workshopAddress: 'Workshop Address',
    invoiceNo: 'Invoice #',
    invoiceFile: 'Invoice File (PDF/Image)',
    workDescription: 'Work Description',
    calendarTitle: 'Maintenance Calendar',
    noScheduledMaintenance: 'No scheduled maintenance.',
    plateLabel: 'Plate',
  },

  // â”€â”€â”€ Consumables â”€â”€â”€
  consumables: {
    pageTitle: 'Consumables & Periodic Maintenance',
    pageDesc: 'Monitor wear status of filters, tires, brakes, batteries and engine oil. Record regular replacements.',
    configureIntervals: 'Configure Intervals',
    registerReplacement: 'Record Replacement',
    selectVehicle: 'Select Vehicle:',
    chooseVehicle: 'Choose a vehicle...',
    currentOdometer: 'Current Odometer:',
    selectVehiclePrompt: 'Please select a vehicle to view consumable status.',
    intervalLabel: 'Interval',
    statusOk: 'OK',
    statusWarning: 'Due Soon',
    statusDue: 'Replace Now',
    lastReplacement: 'Last replacement:',
    atOrder: 'On order',
    kmTraveled: 'Kilometers traveled:',
    timeElapsed: 'Time elapsed:',
    viscosityOil: 'Viscosity/Oil:',
    brandSize: 'Brand/Size:',
    axle: 'Axle:',
    logReplacement: 'Consumable Replacement Log',
    consumableType: 'Consumable Type',
    replacementDate: 'Replacement Date',
    replacementKm: 'Kilometrage (Counter)',
    oilType: 'Oil Type',
    viscosity: 'Viscosity',
    axlePosition: 'Axle / Position',
    frontAxle: 'Front Axle',
    rearAxle: 'Rear Axle',
    brandLabel: 'Brand',
    dimensions: 'Dimensions',
    typeDetail: 'Type',
    batteryBrand: 'Battery Brand',
    batteryCapacity: 'Capacity / Amperage',
    manufacturer: 'Brand / Manufacturer',
    technicalDetails: 'Technical Details',
    notesRemarks: 'Notes / Remarks',
    intervalsConfig: 'Maintenance Intervals & Alerts',
    intervalsDesc: 'Set critical thresholds for your consumables.',
    createRule: 'Create Rule',
    limitKm: 'Limit (km)',
    limitMonths: 'Limit (Months)',
    configureRule: 'Configure rule for',
    newRule: 'New',
    intervalKm: 'Interval (km)',
    intervalMonths: 'Interval (Months)',
    validateRule: 'Validate Rule',
  },

  // â”€â”€â”€ Insurance & Inspections â”€â”€â”€
  insurance: {
    pageTitle: 'Insurance & Technical Inspections',
    pageDesc: 'Monitor your fleet\'s administrative status. Identify insurance policies and inspections nearing expiry at a glance.',
    insuranceSchedule: 'Insurance Schedule',
    insuranceScheduleDesc: 'Policies nearing expiry (Thresholds: 30, 15, 7 days)',
    inspectionSchedule: 'Inspection Schedule',
    inspectionScheduleDesc: 'Regulatory visits and required re-inspections',
    expiry: 'Expiry',
    timeRemaining: 'Time Remaining',
    severity: 'Severity',
    severityCritical: 'Critical',
    severityWarning: 'Warning',
    severityInfo: 'Info',
    noInsuranceAlert: 'No insurance policies on alert.',
    noInspectionAlert: 'No technical inspections on alert.',
    compliant: 'Compliant',
    auto: 'Auto',
    alertStatus: 'Alert / Status',
  },

  // â”€â”€â”€ Fuel & Kilometrage â”€â”€â”€
  fuel: {
    pageTitle: 'Kilometrage & Fuel Tracking',
    pageDesc: 'Manage odometer readings, fuel consumption and detect anomalies.',
    exportOdometers: 'Export Odometers',
    exportFuel: 'Export Fuel',
    inactivityDetected: 'Kilometrage Inactivity Detected',
    inactivityDesc: 'Vehicles with no activity (contract or reading) for more than',
    daysOfInactivity: 'days of inactivity',
    fleetOdometers: 'Fleet (Odometers)',
    vehiclesCount: 'Vehicles',
    plateLabel: 'Plate',
    statusFree: 'Free',
    statusRented: 'Rented',
    statusGarage: 'Garage',
    reading: 'Reading',
    noVehicleSelected: 'No Vehicle Selected',
    selectVehiclePrompt: 'Select a vehicle from the left panel to view its complete km history, fuel fill-ups and consumption.',
    currentOdometer: 'Current odometer',
    transmissionManual: 'Manual',
    transmissionAutomatic: 'Automatic',
    registerFillup: 'Record Fill-up',
    csvFuel: 'CSV Fuel',
    csvKm: 'CSV Km',
    consumptionTrend: 'Consumption Trend (L/100km)',
    insufficientData: 'Insufficient data for trend chart.',
    needTwoFillups: '(Requires at least 2 fuel fill-ups)',
    fuelHistory: 'Fuel Fill-up History',
    odometer: 'Odometer',
    liters: 'Liters',
    pricePerL: 'Price/L',
    totalCost: 'Total',
    average: 'Average',
    anomalyLabel: 'Anomaly',
    noFuelLog: 'No fuel fill-ups recorded for this vehicle.',
    kmTimeline: 'Odometer History (Km Tracking)',
    dateTime: 'Date / Time',
    odometerValue: 'Odometer Value',
    source: 'Source',
    remarksEvent: 'Remarks / Event',
    sourceManual: 'Manual',
    sourceFuel: 'Fuel',
    sourceContract: 'Contract',
    noKmHistory: 'No km history.',
    manualOdometerReading: 'Manual Odometer Reading',
    readingDate: 'Reading Date',
    odometerIndex: 'Odometer Index (Counter in Km)',
    readingNotes: 'Notes / Reason for reading',
    readingPlaceholder: 'Monthly reading, maintenance, etc.',
    registerFuelFillup: 'Record Fuel Fill-up',
    kmIndex: 'Km Index (Odometer)',
    volumeLiters: 'Volume (Liters)',
    pricePerLiter: 'Price per Liter (â‚¬/L)',
    stationName: 'Station Name',
    fuelType: 'Fuel Type',
    gasoline: 'Gasoline (Unleaded)',
    diesel: 'Diesel',
    electric: 'Electric',
    hybrid: 'Hybrid',
    lpg: 'LPG',
    estimatedTotal: 'Estimated Total:',
  },

  // â”€â”€â”€ Alerts â”€â”€â”€
  alerts: {
    pageTitle: 'Alert Center',
    pageDesc: 'Insurance, technical inspections, consumables and driving licenses.',
    totalAlerts: 'Total Alerts',
    critical: 'Critical',
    warning: 'Warning',
    info: 'Information',
    searchPlaceholder: 'Search target, message...',
    module: 'Module',
    allModules: 'All modules',
    moduleInsurance: 'Insurance',
    moduleInspection: 'Technical Inspection',
    moduleMaintenance: 'Scheduled Maintenance',
    moduleOdometer: 'Odometer Inactivity',
    moduleLicense: 'Client License',
    moduleConsumable: 'Consumables',
    alertsShown: 'alert(s) shown',
    severity: 'Severity',
    typeLabel: 'Type',
    concerns: 'Concerns',
    deadline: 'Deadline',
    severityCritical: 'Critical',
    severityWarning: 'Warning',
    severityInfo: 'Info',
    typeInactivityKm: 'Km Inactivity',
    typeLicense: 'License',
    typeInspection: 'Inspection',
    typeInsurance: 'Insurance',
    typeConsumable: 'Consumable',
    noAlerts: 'No alerts to report',
    noAlertsDesc: 'All deadlines and inspections are up to date.',
    alertsCount: 'alert(s)',
  },

  // â”€â”€â”€ Reports â”€â”€â”€
  reports: {
    pageTitle: 'Reports & Operational Analytics',
    pageDesc: 'Evaluate profitability, fleet utilization rate, total cost of ownership (TCO) and track unpaid invoices.',
    exportProfitability: 'Export Profitability (CSV)',
    grossRevenue: 'Gross Revenue',
    paidLabel: 'Paid',
    dueLabel: 'Due',
    avgUtilization: 'Average Utilization Rate',
    utilizationDesc: 'Rented time / Total time.',
    fleetSize: 'Fleet Size',
    vehiclesLabel: 'Vehicles',
    rented: 'Rented',
    garage: 'Garage',
    fleetAvailability: 'Fleet Availability Status',
    financialRecovery: 'Financial Recovery Rate',
    startDate: 'Start',
    endDate: 'End',
    profitabilityAnalysis: 'Profitability Analysis by Vehicle (TCO)',
    utilizationRate: 'Utilization Rate',
    revenueLabel: 'Revenue',
    maintenanceLabel: 'Maintenance',
    fuelLabel: 'Fuel',
    insuranceLabel: 'Insurance',
    tco: 'TCO (Total Cost)',
    netProfit: 'Net Profit',
    noProfitabilityData: 'No profitability data available.',
    topClients: 'Top Clients (By Volume & Revenue)',
    clientName: 'Client Name',
    rentals: 'Rentals',
    totalRented: 'Total Rented',
    noClients: 'No clients available.',
    latePayments: 'Late Payment Invoices & Contracts',
    contractNo: 'Contract #',
    amountDue: 'Amount Due',
    paymentStatus: 'Payment Status',
    paymentUnpaid: 'Unpaid',
    paymentPartial: 'Partial',
    noUnpaid: 'Congratulations, no unpaid invoices!',
  },

  // â”€â”€â”€ Settings â”€â”€â”€
  settings: {
    pageTitle: 'Global Settings',
    pageDesc: 'Configure alert rules, extras catalog, currencies, and update your credentials.',
    thresholdsConfig: 'Thresholds & Alerts Configuration',
    thresholdsDesc: 'Define when alerts are triggered for maintenance, insurance, inspections and consumables.',
    currencySymbol: 'Currency Symbol',
    dateFormat: 'Date Format',
    kmInactivity: 'Odometer Inactivity (Days)',
    maintenanceAlert: 'Preventive Maintenance (Alert N days before)',
    insuranceThresholds: 'Insurance Thresholds (Days remaining - CSV)',
    inspectionThresholds: 'Inspection Thresholds (Days - CSV)',
    consumableAlertKm: 'Consumables (Alert N km before)',
    consumableAlertDays: 'Consumables (Alert N days before)',
    saveConfig: 'Save Configuration',
    referenceData: 'Reference Data Management',
    referenceDataDesc: 'Manage the dropdown options and catalogs used across the application.',
    vehicleCategories: 'Vehicle Categories',
    vehicleCategoriesDesc: 'Define the vehicle types available when registering a new vehicle (e.g. Sedan, SUV, Van).',
    addCategory: 'Add a category...',
    fuelTypes: 'Fuel Types',
    fuelTypesDesc: 'List of fuel types selectable in vehicle forms (e.g. Gasoline, Diesel, Electric).',
    addFuel: 'Add a fuel type...',
    maintenanceTypes: 'Maintenance Intervention Types',
    maintenanceTypesDesc: 'Categories of maintenance work (e.g. Preventive, Corrective, Accident Repair).',
    addIntervention: 'Add an intervention type...',
    extrasCatalog: 'Extras & Accessories Catalog',
    extrasCatalogDesc: 'Optional add-ons available when creating rental contracts (e.g. GPS, Child Seat).',
    optionName: 'Option name',
    optionRate: 'Rate (â‚¬/day)',
    saveRefData: 'Save Reference Data',
    itemsConfigured: 'items configured',
    noItems: 'No items configured yet.',
    adminProfile: 'Administrator Profile',
    adminProfileDesc: 'Update your display name and account information.',
    usernameReadOnly: 'Username (Read Only)',
    fullName: 'Full Name',
    updateProfile: 'Update Profile',
    security: 'Security & Authentication',
    securityDesc: 'Change your password to keep your account secure.',
    currentPassword: 'Current Password',
    newPassword: 'New Password',
    confirmPassword: 'Confirm New Password',
    changePassword: 'Change Password',
  },

  // â”€â”€â”€ Statuses (reusable) â”€â”€â”€
  statuses: {
    available: 'Available',
    rented: 'Rented',
    inMaintenance: 'In Maintenance',
    reserved: 'Reserved',
    immobilized: 'Immobilized',
    draft: 'Draft',
    active: 'Active',
    completed: 'Completed',
    cancelled: 'Cancelled',
    scheduled: 'Scheduled',
    inProgress: 'In Progress',
    unpaid: 'Unpaid',
    partiallyPaid: 'Partially Paid',
    paid: 'Paid',
    valid: 'Valid',
    expiringSoon: 'Expiring Soon',
    expired: 'Expired',
    ok: 'OK',
    warningStatus: 'Due Soon',
    due: 'Replace Now',
  },
};

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\i18n\fr.ts
```typescript
export const FR: Record<string, any> = {
  // â”€â”€â”€ Common / Shared â”€â”€â”€
  common: {
    save: 'Enregistrer',
    cancel: 'Annuler',
    delete: 'Supprimer',
    edit: 'Modifier',
    add: 'Ajouter',
    close: 'Fermer',
    search: 'Rechercher',
    actions: 'Actions',
    status: 'Statut',
    date: 'Date',
    notes: 'Notes',
    remarks: 'Remarques',
    yes: 'Oui',
    no: 'Non',
    confirm: 'Confirmer',
    loading: 'Chargement...',
    uploading: 'Chargement...',
    noData: 'Aucune donnÃ©e',
    all: 'Tous',
    viewAll: 'Voir tout',
    export: 'Exporter',
    print: 'Imprimer',
    refresh: 'Actualiser',
    back: 'Retour',
    next: 'Suivant',
    previous: 'PrÃ©cÃ©dent',
    page: 'page',
    of: 'de',
    total: 'Total',
    validate: 'Valider',
    perDay: '/j',
    days: 'jours',
    months: 'mois',
    km: 'km',
    vehicle: 'VÃ©hicule',
    client: 'Client',
    amount: 'Montant',
    cost: 'CoÃ»t',
    type: 'Type',
    description: 'Description',
    document: 'Document',
    photo: 'Photo',
    none: 'Aucun',
    see: 'Voir',
    manage: 'GÃ©rer',
    reports: 'Rapports',
    from: 'Du',
    to: 'Au',
    admin: 'Admin',
    logout: 'Se dÃ©connecter',
    uploadInProgress: 'Upload en cours...',
    deleteConfirm: 'ÃŠtes-vous sÃ»r de vouloir supprimer ?',
    errorOccurred: 'Une erreur est survenue',
    savedSuccessfully: 'EnregistrÃ© avec succÃ¨s',
  },

  // â”€â”€â”€ Sidebar / Navigation â”€â”€â”€
  sidebar: {
    brand: 'Parc Auto',
    dashboard: 'Dashboard',
    vehicles: 'VÃ©hicules',
    clients: 'Clients',
    contracts: 'Contrats',
    kilometrage: 'KilomÃ©trage',
    maintenance: 'Maintenance',
    consumables: 'Consommables',
    insuranceControl: 'Assurance & ContrÃ´le',
    alerts: 'Alertes',
    reportsAnalytics: 'Rapports & Analytics',
    settings: 'Configuration',
  },

  // â”€â”€â”€ Topbar â”€â”€â”€
  topbar: {
    title: 'SystÃ¨me de Gestion de Flotte',
    activeAlerts: 'Alertes actives',
    noActiveAlerts: 'Aucune alerte active.',
    language: 'Langue',
  },

  // â”€â”€â”€ Login â”€â”€â”€
  login: {
    title: 'Parc Auto',
    subtitle: 'Gestion de flotte de location',
    username: "Nom d'utilisateur",
    password: 'Mot de passe',
    loginBtn: 'Se connecter',
    loggingIn: 'Connexion...',
    defaultCredentials: 'Par dÃ©faut:',
    errorRequired: "Veuillez saisir votre nom d'utilisateur et votre mot de passe.",
    errorInvalid: "Nom d'utilisateur ou mot de passe incorrect.",
    errorServer: 'Une erreur est survenue lors de la connexion. Veuillez rÃ©essayer.',
  },

  // â”€â”€â”€ Dashboard â”€â”€â”€
  dashboard: {
    pageTitle: 'Tableau de bord',
    pageDesc: "Vue d'ensemble de la flotte et des opÃ©rations.",
    fleet: 'Flotte',
    registeredVehicles: 'vÃ©hicules enregistrÃ©s',
    activeRentals: 'Locations actives',
    available: 'disponibles',
    revenue: "Chiffre d'affaires",
    paid: 'payÃ©',
    alerts: 'Alertes',
    critical: 'critiques',
    fleetAvailability: 'DisponibilitÃ© du parc',
    currentDistribution: 'RÃ©partition actuelle',
    priorityAlerts: 'Alertes prioritaires',
    priorityAlertsDesc: 'Maintenance, assurance, inspections',
    noActiveAlert: 'Aucune alerte en cours.',
    unpaid: 'ImpayÃ©s',
    unpaidDesc: 'Contrats en attente de paiement',
    contractNo: 'NÂ° Contrat',
    departure: 'DÃ©part',
    paymentUnpaid: 'Non PayÃ©',
    paymentPartial: 'Partiel',
    noUnpaidContracts: 'Aucun contrat impayÃ©.',
  },

  // â”€â”€â”€ Vehicles â”€â”€â”€
  vehicles: {
    pageTitle: 'Gestion de la Flotte',
    pageDesc: 'Enregistrez et suivez les vÃ©hicules du parc.',
    addVehicle: 'Ajouter un vÃ©hicule',
    searchPlaceholder: 'Rechercher par marque, modÃ¨le, plaque, VIN...',
    allTypes: 'Tous les Types',
    allFuels: 'Tous Carburants',
    allStatuses: 'Tous Statuts',
    noVehiclesFound: 'Aucun vÃ©hicule ne correspond aux critÃ¨res.',
    statusAvailable: 'Libre',
    statusRented: 'LouÃ©',
    statusMaintenance: 'En service',
    statusReserved: 'RÃ©servÃ©',
    statusImmobilized: 'ImmobilisÃ©',
    engine: 'Moteur',
    gearbox: 'BoÃ®te',
    manual: 'Manuelle',
    auto: 'Auto',
    automatic: 'Automatique',
    folder: 'Dossier',
    editVehicle: 'Modifier le vÃ©hicule',
    newVehicle: 'Nouveau vÃ©hicule',
    matricule: 'Matricule',
    brand: 'Marque',
    model: 'ModÃ¨le',
    year: 'AnnÃ©e',
    fuelType: 'Carburant',
    transmission: 'Transmission',
    vin: 'ChÃ¢ssis (VIN)',
    engineNumber: 'NÂ° Moteur',
    color: 'Couleur',
    seats: 'SiÃ¨ges',
    dailyRate: 'Tarif/jour',
    purchasePrice: "Prix d'achat",
    initialKm: 'Compteur km initial',
    remarks: 'Remarques',
    deleteConfirm: 'ÃŠtes-vous sÃ»r de vouloir supprimer ce vÃ©hicule de la flotte active ?',
    errorDelete: 'Erreur lors de la suppression du vÃ©hicule',
    errorUpdate: 'Erreur lors de la mise Ã  jour',
    errorCreate: "Erreur lors de l'enregistrement",
    // Detail tabs
    tracking: 'Suivi',
    consumablesTab: 'Consommables',
    insuranceTab: 'Assurances',
    inspectionsTab: 'ContrÃ´les',
    fuelTab: 'Carburant',
    kmHistoryTab: 'KilomÃ©trage',
    consumablesState: 'Ã‰tat des consommables',
    consumable: 'Consommable',
    lastReplacement: 'Dernier remplacement',
    traveled: 'Parcouru',
    interval: 'Intervalle',
    noLog: 'Aucun log',
    insuranceHistory: 'Historique des assurances',
    insurer: 'Assureur',
    policyNo: 'NÂ° Police',
    coverage: 'Couverture',
    validity: 'ValiditÃ©',
    premiumValue: 'Prime / Valeur',
    statusValid: 'Valide',
    statusExpiringSoon: 'BientÃ´t',
    statusExpired: 'ExpirÃ©',
    technicalInspections: 'ContrÃ´les techniques',
    expiration: 'Expiration',
    center: 'Centre',
    result: 'RÃ©sultat',
    resultPass: 'AcceptÃ©',
    resultConditional: 'Contre-Visite',
    resultFail: 'RefusÃ©',
    fuelTracking: 'Suivi carburant',
    counter: 'Compteur',
    volume: 'Volume',
    consumption: 'Conso.',
    anomaly: 'Anomalie',
    overConsumption: 'Surconso',
    kmHistory: 'Historique kilomÃ©trique',
    reading: 'RelevÃ©',
    // Insurance add dialog
    newInsurancePolicy: "Nouveau contrat d'assurance",
    coverageThirdParty: 'ResponsabilitÃ© Civile',
    coverageComprehensive: 'Tous Risques',
    coverageFleet: 'Flotte',
    premium: 'Prime',
    effectiveDate: "Date d'effet",
    documentPdf: 'Document (PDF)',
    // Inspection add dialog
    newInspection: 'Nouveau contrÃ´le technique',
    inspectionDate: 'Date contrÃ´le',
    resultFavorable: 'Favorable',
    resultCounterVisit: 'Contre-visite',
    resultUnfavorable: 'DÃ©favorable',
    centerAddress: 'Adresse centre',
    // Fuel add dialog
    newFuelFillup: 'Nouveau plein carburant',
    counterKm: 'Compteur (km)',
    volumeL: 'Volume (L)',
    pricePerL: 'Prix/L',
    station: 'Station',
    fillup: 'Plein',
    // Km add dialog
    kmReading: 'RelevÃ© kilomÃ©trique',
  },

  // â”€â”€â”€ Clients â”€â”€â”€
  clients: {
    pageTitle: 'Base de DonnÃ©es Clients',
    pageDesc: 'GÃ©rez le registre des clients, leurs coordonnÃ©es de contact, permis de conduire et historique de locations.',
    addClient: 'Ajouter un Client',
    searchPlaceholder: 'Rechercher par nom, tÃ©lÃ©phone, email, ID...',
    identity: 'IdentitÃ©',
    contact: 'Contact',
    license: 'Permis',
    licenseValidity: 'ValiditÃ©',
    expired: 'ExpirÃ©',
    history: 'Historique',
    noClientFound: 'Aucun client trouvÃ©',
    noClientDesc: 'Ajoutez un client ou modifiez votre recherche.',
    totalClients: 'client(s) au total',
    editClient: 'Modifier la fiche client',
    newClient: 'CrÃ©er un nouveau client',
    fullName: 'Nom Complet',
    nationalId: 'NÂ° ID National / Passeport',
    dateOfBirth: 'Date de naissance',
    licenseNumber: 'NÂ° Permis de Conduire',
    licenseCategory: 'CatÃ©gorie du Permis',
    category: 'CatÃ©gorie',
    licenseIssueDate: "Date d'obtention Permis",
    licenseExpiryDate: "Date d'expiration Permis",
    phone: 'NÂ° de TÃ©lÃ©phone',
    email: 'Adresse E-mail',
    address: 'Adresse',
    notesRemarks: 'Remarques / Notes',
    clientFile: 'Dossier Client',
    nationalIdLabel: 'ID National',
    phoneLabel: 'TÃ©lÃ©phone',
    licenseLabel: 'Permis',
    expirationLabel: 'Expiration',
    adminNotes: 'Notes administratives :',
    rentalHistory: 'Historique des locations',
    contractNo: 'NÂ° Contrat',
    vehicleLabel: 'VÃ©hicule',
    registration: 'Immatriculation',
    period: 'PÃ©riode',
    amountLabel: 'Montant',
    statusCompleted: 'TerminÃ©',
    statusActive: 'Actif',
    statusDraft: 'Brouillon',
    statusCancelled: 'AnnulÃ©',
    noRentalHistory: 'Aucun contrat de location enregistrÃ© pour ce client.',
  },

  // â”€â”€â”€ Contracts â”€â”€â”€
  contracts: {
    pageTitle: 'Gestion des Contrats de Location',
    pageDesc: 'CrÃ©ez des contrats, suivez les dÃ©parts, enregistrez les retours de vÃ©hicules et gÃ©rez la facturation de vos clients.',
    newContract: 'Nouveau Contrat',
    searchPlaceholder: 'Rechercher par NÂ° contrat, client, plaque...',
    allStatuses: 'Tous les Statuts',
    statusDraft: 'Brouillon',
    statusActive: 'En Cours',
    statusCompleted: 'TerminÃ©',
    statusCancelled: 'AnnulÃ©',
    allInvoices: 'Toutes les Factures',
    unpaid: 'Non PayÃ©',
    partial: 'Partiel',
    paid: 'PayÃ©',
    contract: 'Contrat',
    period: 'PÃ©riode',
    statuses: 'Statuts',
    noContractFound: 'Aucun contrat trouvÃ©',
    noContractDesc: 'CrÃ©ez un nouveau contrat ou ajustez les filtres.',
    totalContracts: 'contrat(s)',
    returnVehicle: 'Retour vÃ©hicule',
    returnBtn: 'Retour',
    editContract: 'Modifier le contrat de location',
    newContractDialog: "Ã‰dition d'un nouveau contrat de location",
    selectClient: 'SÃ©lectionner un Client',
    chooseClient: 'Choisir un client...',
    selectVehicle: 'SÃ©lectionner un VÃ©hicule',
    chooseVehicle: 'Choisir un vÃ©hicule...',
    contractType: 'Type de Contrat',
    dailyRate: 'Tarif Journalier',
    startDateTime: 'Date & Heure de DÃ©part',
    expectedReturn: 'Date & Heure de Retour PrÃ©vue',
    calculatedDays: 'Jours CalculÃ©s',
    rentalAmount: 'Montant Location',
    extraFees: 'Frais Extras',
    discount: 'Remise / RÃ©duc',
    totalNetDue: 'Total Net DÃ»',
    deposit: 'Caution',
    paymentMethod: 'RÃ¨glement',
    initialStatus: "RÃ©gler l'Ã©tat initial",
    draftReservation: 'Brouillon (RÃ©servation simple)',
    activateNow: 'Activer maintenant (DÃ©part du vÃ©hicule)',
    initialBilling: 'Statut initial de facturation',
    unpaidPending: 'Non PayÃ© (En attente)',
    depositPaid: 'Acompte versÃ© (Partiel)',
    fullyPaid: 'PayÃ© (RÃ¨glement intÃ©gral)',
    adminNotes: 'Notes administratives',
    saveContract: 'Enregistrer le Contrat',
    errorUpdate: 'Erreur lors de la mise Ã  jour',
    errorCreate: "Erreur lors de l'enregistrement",
    // Return dialog
    closeContract: 'ClÃ´turer le contrat',
    departureOdometer: 'OdomÃ¨tre au dÃ©part :',
    expectedReturnDate: 'Retour prÃ©vu le :',
    returnIndex: 'Index retour (Km)',
    actualReturnDate: 'Date effective de retour',
    fuelPenalty: 'PÃ©nalitÃ© Essence',
    damageFees: 'Frais Dommages',
    sendToMaintenance: 'Envoyer le vÃ©hicule en maintenance (dÃ©gÃ¢ts signalÃ©s)',
    returnNotes: 'Notes de retour / Constats',
    returnNotesPlaceholder: "PropretÃ©, niveau d'essence, rayures...",
    validateReturn: 'Valider le retour',
    errorReturn: 'Erreur lors du retour du vÃ©hicule',
    // License warning
    licenseWarning: 'Attention: Le permis de ce client a expirÃ© le',
    // Print / Invoice
    printPreview: 'AperÃ§u avant Impression',
    invoiceBrand: 'PARC AUTO RENTAL',
    invoiceSubtitle: 'Service de Location Automobile Premium',
    contractInvoice: 'CONTRAT & FACTURE',
    tenant: 'Locataire / Client :',
    vehicleLabel: 'VÃ©hicule :',
    registrationLabel: 'Immatriculation',
    departureCounter: 'Compteur dÃ©part',
    returnCounter: 'Compteur retour',
    dailyRateLabel: 'Tarif journalier',
    daysLabel: 'Jours',
    totalHT: 'Total HT',
    vehicleRental: 'Location de vÃ©hicule',
    extrasAccessories: 'Services Extras / Accessoires',
    penalties: 'PÃ©nalitÃ©s / Retours (Retards, DÃ©gÃ¢ts, Carburant)',
    paymentLabel: 'RÃ¨glement :',
    mode: 'Mode',
    statusLabel: 'Statut',
    subtotal: 'Sous-total :',
    reduction: 'RÃ©duction :',
    netAmountDue: 'NET Ã€ PAYER :',
    tenantSignature: 'Signature du Locataire',
    agencySignature: "Signature de l'Agence",
    amountPaid: 'Montant PayÃ©',
    remainingDue: 'Reste Ã  payer',
  },

  // â”€â”€â”€ Maintenance â”€â”€â”€
  maintenance: {
    pageTitle: 'Suivi des Maintenances',
    pageDesc: 'Enregistrez les interventions mÃ©caniques (prÃ©ventives ou correctives) et planifiez les prochains entretiens.',
    maintenanceCalendar: "Calendrier d'Entretien",
    registerIntervention: 'Enregistrer Intervention',
    intervention: 'Intervention',
    dates: 'Dates',
    workshop: 'Atelier',
    invoice: 'Facture',
    nextScheduled: 'Prochain',
    statusScheduled: 'PlanifiÃ©',
    statusInProgress: 'En cours',
    statusCompleted: 'TerminÃ©',
    noMaintenance: 'Aucune maintenance enregistrÃ©e',
    noMaintenanceDesc: 'Enregistrez une intervention pour commencer le suivi.',
    interventions: 'intervention(s)',
    editMaintenance: 'Modifier la fiche de maintenance',
    newIntervention: 'Enregistrer une intervention',
    interventionType: "Type d'intervention",
    interventionDate: "Date de l'intervention",
    nextMaintenance: 'Prochain entretien',
    counterKm: 'Compteur km (Intervention)',
    interventionStatus: "Statut d'intervention",
    laborCost: "Main d'Å“uvre",
    partsCost: 'PiÃ¨ces dÃ©tachÃ©es',
    workshopName: "Nom de l'atelier / Garage",
    workshopContact: "Contact de l'atelier",
    workshopAddress: "Adresse de l'atelier",
    invoiceNo: 'Facture NÂ°',
    invoiceFile: 'Fichier Facture (PDF/Image)',
    workDescription: 'Description des travaux',
    calendarTitle: 'Calendrier de maintenance',
    noScheduledMaintenance: 'Aucun entretien programmÃ©.',
    plateLabel: 'Plaque',
  },

  // â”€â”€â”€ Consumables â”€â”€â”€
  consumables: {
    pageTitle: 'Suivi des Consommables & Entretien PÃ©riodique',
    pageDesc: "Supervisez l'Ã©tat d'usure des filtres, pneus, freins, batteries et huile moteur. Enregistrez les remplacements rÃ©guliers.",
    configureIntervals: 'Configurer Intervalles',
    registerReplacement: 'Enregistrer Remplacement',
    selectVehicle: 'SÃ©lectionner un VÃ©hicule :',
    chooseVehicle: 'Choisir un vÃ©hicule...',
    currentOdometer: 'Compteur Actuel :',
    selectVehiclePrompt: "Veuillez sÃ©lectionner un vÃ©hicule pour visualiser l'Ã©tat des consommables.",
    intervalLabel: 'Intervalle',
    statusOk: 'Conforme',
    statusWarning: 'BientÃ´t DÃ»',
    statusDue: 'Ã€ Remplacer',
    lastReplacement: 'Dernier remplacement :',
    atOrder: 'Ã€ la commande',
    kmTraveled: 'KilomÃ¨tres parcourus :',
    timeElapsed: 'Temps Ã©coulÃ© :',
    viscosityOil: 'ViscositÃ©/Huile :',
    brandSize: 'Marque/Taille :',
    axle: 'Essieu :',
    logReplacement: 'Log de remplacement de consommable',
    consumableType: 'Type de consommable',
    replacementDate: 'Date du remplacement',
    replacementKm: 'KilomÃ©trage (Compteur)',
    oilType: "Type d'huile",
    viscosity: 'ViscositÃ©',
    axlePosition: 'Essieu / Position',
    frontAxle: 'Essieu Avant',
    rearAxle: 'Essieu ArriÃ¨re',
    brandLabel: 'Marque',
    dimensions: 'Dimensions',
    typeDetail: 'Type',
    batteryBrand: 'Marque de batterie',
    batteryCapacity: 'CapacitÃ© / AmpÃ©rage',
    manufacturer: 'Marque / Fabricant',
    technicalDetails: 'DÃ©tails techniques',
    notesRemarks: 'Notes / Remarques',
    intervalsConfig: 'Intervalles de maintenance & alertes',
    intervalsDesc: 'DÃ©finissez les seuils critiques pour vos consommables.',
    createRule: 'CrÃ©er RÃ¨gle',
    limitKm: 'Limite (km)',
    limitMonths: 'Limite (Mois)',
    configureRule: 'Configurer la rÃ¨gle pour',
    newRule: 'Nouveau',
    intervalKm: 'Intervalle (km)',
    intervalMonths: 'Intervalle (Mois)',
    validateRule: 'Valider RÃ¨gle',
  },

  // â”€â”€â”€ Insurance & Inspections â”€â”€â”€
  insurance: {
    pageTitle: 'Supervision Assurances & ContrÃ´les Techniques',
    pageDesc: "Supervisez l'Ã©tat administratif de votre flotte. Identifiez d'un coup d'Å“il les contrats d'assurance et contrÃ´les techniques arrivant Ã  Ã©chÃ©ance.",
    insuranceSchedule: 'Ã‰chÃ©ancier Assurances',
    insuranceScheduleDesc: 'Polices arrivant Ã  Ã©chÃ©ance (Seuils: 30, 15, 7 jours)',
    inspectionSchedule: 'Ã‰chÃ©ancier ContrÃ´les Techniques',
    inspectionScheduleDesc: 'Visites rÃ©glementaires et contre-visites requises',
    expiry: 'Ã‰chÃ©ance',
    timeRemaining: 'Temps Restant',
    severity: 'GravitÃ©',
    severityCritical: 'Critique',
    severityWarning: 'Attention',
    severityInfo: 'Info',
    noInsuranceAlert: "Aucun contrat d'assurance en alerte.",
    noInspectionAlert: 'Aucun contrÃ´le technique en alerte.',
    compliant: 'Conforme',
    auto: 'Auto',
    alertStatus: 'Alerte / Statut',
  },

  // â”€â”€â”€ Fuel & Kilometrage â”€â”€â”€
  fuel: {
    pageTitle: 'Suivi KilomÃ©trage & Carburant',
    pageDesc: "GÃ©rez les relevÃ©s d'odomÃ¨tres, les consommations de carburant et dÃ©tectez les anomalies.",
    exportOdometers: 'Exporter OdomÃ¨tres',
    exportFuel: 'Exporter Carburant',
    inactivityDetected: 'InactivitÃ© KilomÃ©trique DÃ©tectÃ©e',
    inactivityDesc: 'VÃ©hicules sans activitÃ© (contrat ou relevÃ©) depuis plus de',
    daysOfInactivity: "jours d'inactivitÃ©",
    fleetOdometers: 'Parc Automobile (OdomÃ¨tres)',
    vehiclesCount: 'VÃ©hicules',
    plateLabel: 'Matricule',
    statusFree: 'Libre',
    statusRented: 'LouÃ©',
    statusGarage: 'Garage',
    reading: 'RelevÃ©',
    noVehicleSelected: 'Aucun VÃ©hicule SÃ©lectionnÃ©',
    selectVehiclePrompt: "SÃ©lectionnez un vÃ©hicule dans la liste de gauche pour consulter son historique kilomÃ©trique complet, ses pleins de carburant et sa consommation.",
    currentOdometer: 'OdomÃ¨tre actuel',
    transmissionManual: 'Manuelle',
    transmissionAutomatic: 'Automatique',
    registerFillup: 'Enregistrer Plein',
    csvFuel: 'CSV Carburant',
    csvKm: 'CSV Km',
    consumptionTrend: 'Ã‰volution de la Consommation (L/100km)',
    insufficientData: 'DonnÃ©es insuffisantes pour le graphique de tendance.',
    needTwoFillups: '(NÃ©cessite au moins 2 pleins de carburant)',
    fuelHistory: 'Historique des Pleins de Carburant',
    odometer: 'OdomÃ¨tre',
    liters: 'Litres',
    pricePerL: 'Prix/L',
    totalCost: 'Total',
    average: 'Moyenne',
    anomalyLabel: 'Anomalie',
    noFuelLog: 'Aucun plein enregistrÃ© pour ce vÃ©hicule.',
    kmTimeline: 'Historique OdomÃ©trique (Suivi KilomÃ©trage)',
    dateTime: 'Date / Heure',
    odometerValue: 'Valeur OdomÃ¨tre',
    source: 'Source',
    remarksEvent: 'Remarques / Ã‰vÃ©nement',
    sourceManual: 'Manuel',
    sourceFuel: 'Carburant',
    sourceContract: 'Contrat',
    noKmHistory: 'Aucun historique de kilomÃ©trage.',
    // Manual Km dialog
    manualOdometerReading: 'RelevÃ© OdomÃ¨tre Manuel',
    readingDate: 'Date du relevÃ©',
    odometerIndex: 'Index OdomÃ¨tre (Compteur en Km)',
    readingNotes: 'Notes / Raison du relevÃ©',
    readingPlaceholder: 'RelevÃ© mensuel, maintenance, etc.',
    // Fuel dialog
    registerFuelFillup: 'Enregistrer un Plein de Carburant',
    kmIndex: 'Index Km (OdomÃ¨tre)',
    volumeLiters: 'Volume (Litres)',
    pricePerLiter: 'Prix par Litre (â‚¬/L)',
    stationName: 'Nom de la Station Service',
    fuelType: 'Type de Carburant',
    gasoline: 'Essence (Sans Plomb)',
    diesel: 'Gazole / Diesel',
    electric: 'Ã‰lectricitÃ©',
    hybrid: 'Hybride',
    lpg: 'GPL',
    estimatedTotal: 'CoÃ»t Total EstimÃ©:',
  },

  // â”€â”€â”€ Alerts â”€â”€â”€
  alerts: {
    pageTitle: "Centre d'Alertes",
    pageDesc: 'Assurances, contrÃ´les techniques, consommables et permis de conduire.',
    totalAlerts: 'Total Alertes',
    critical: 'Critique',
    warning: 'Avertissement',
    info: 'Information',
    searchPlaceholder: 'Rechercher cible, message...',
    module: 'Module',
    allModules: 'Tous les modules',
    moduleInsurance: 'Assurance',
    moduleInspection: 'ContrÃ´le Technique',
    moduleMaintenance: 'Maintenance PlanifiÃ©e',
    moduleOdometer: 'InactivitÃ© OdomÃ¨tre',
    moduleLicense: 'Permis Client',
    moduleConsumable: 'Consommables',
    alertsShown: 'alerte(s) affichÃ©e(s)',
    severity: 'GravitÃ©',
    typeLabel: 'Type',
    concerns: 'Concerne',
    deadline: 'Ã‰chÃ©ance',
    severityCritical: 'Critique',
    severityWarning: 'Attention',
    severityInfo: 'Info',
    typeInactivityKm: 'InactivitÃ© Km',
    typeLicense: 'Permis',
    typeInspection: 'ContrÃ´le',
    typeInsurance: 'Assurance',
    typeConsumable: 'Consommable',
    noAlerts: 'Aucune alerte Ã  signaler',
    noAlertsDesc: 'Toutes les Ã©chÃ©ances et contrÃ´les sont Ã  jour.',
    alertsCount: 'alerte(s)',
  },

  // â”€â”€â”€ Reports â”€â”€â”€
  reports: {
    pageTitle: 'Rapports & Analyses OpÃ©rationnelles',
    pageDesc: "Ã‰valuez la rentabilitÃ©, le taux d'utilisation de la flotte, le coÃ»t total de possession (TCO) et suivez les impayÃ©s.",
    exportProfitability: 'Exporter RentabilitÃ© (CSV)',
    grossRevenue: "Chiffre d'Affaires Brut",
    paidLabel: 'PayÃ©',
    dueLabel: 'DÃ»',
    avgUtilization: "Taux d'Utilisation Moyen",
    utilizationDesc: 'Temps louÃ© / DurÃ©e totale.',
    fleetSize: 'Taille de la Flotte',
    vehiclesLabel: 'VÃ©hicules',
    rented: 'LouÃ©s',
    garage: 'Garage',
    fleetAvailability: 'Ã‰tat de DisponibilitÃ© Flotte',
    financialRecovery: 'Taux de Recouvrement Financier',
    startDate: 'DÃ©but',
    endDate: 'Fin',
    profitabilityAnalysis: 'Analyse de RentabilitÃ© par VÃ©hicule (TCO)',
    utilizationRate: 'Taux Utilisation',
    revenueLabel: 'Revenus',
    maintenanceLabel: 'Maintenance',
    fuelLabel: 'Carburant (Fuel)',
    insuranceLabel: 'Assurance',
    tco: 'TCO (CoÃ»t Total)',
    netProfit: 'BÃ©nÃ©fice Net',
    noProfitabilityData: 'Aucune donnÃ©e de rentabilitÃ© disponible.',
    topClients: 'Top Clients (Par Volume & Revenu)',
    clientName: 'Nom du Client',
    rentals: 'Locations',
    totalRented: 'Total LouÃ©',
    noClients: 'Aucun client disponible.',
    latePayments: 'Factures & Contrats en Retard de Paiement',
    contractNo: 'NÂ° Contrat',
    amountDue: 'Montant DÃ»',
    paymentStatus: 'Statut Pay.',
    paymentUnpaid: 'Non PayÃ©',
    paymentPartial: 'Partiel',
    noUnpaid: 'FÃ©licitations, aucun impayÃ© enregistrÃ© !',
  },

  // â”€â”€â”€ Settings â”€â”€â”€
  settings: {
    pageTitle: 'ParamÃ¨tres Globaux',
    pageDesc: "Configurez les rÃ¨gles d'alertes, le catalogue d'options supplÃ©mentaires, les devises, et modifiez vos identifiants.",
    thresholdsConfig: 'Configurations des Seuils & Alertes',
    thresholdsDesc: 'DÃ©finissez quand les alertes se dÃ©clenchent pour la maintenance, les assurances, les contrÃ´les et les consommables.',
    currencySymbol: 'Symbole Devise',
    dateFormat: 'Format de Date',
    kmInactivity: 'InactivitÃ© OdomÃ¨tre (Jours)',
    maintenanceAlert: 'Maintenance PrÃ©ventive (Alerter N jours avant)',
    insuranceThresholds: 'Seuils Assurances (Jours restants - CSV)',
    inspectionThresholds: 'Seuils ContrÃ´les Techniques (Jours - CSV)',
    consumableAlertKm: 'Consommables (Alerter N kilomÃ¨tres avant)',
    consumableAlertDays: 'Consommables (Alerter N jours avant)',
    saveConfig: 'Enregistrer les Configurations',
    referenceData: 'Ã‰dition des DonnÃ©es de RÃ©fÃ©rence',
    referenceDataDesc: 'GÃ©rez les listes dÃ©roulantes et catalogues utilisÃ©s dans toute l\'application.',
    vehicleCategories: 'CatÃ©gories de VÃ©hicules',
    vehicleCategoriesDesc: 'DÃ©finissez les types de vÃ©hicules disponibles lors de l\'enregistrement (ex: Berline, SUV, Utilitaire).',
    addCategory: 'Ajouter une catÃ©gorie...',
    fuelTypes: 'Types de Carburant',
    fuelTypesDesc: 'Liste des carburants sÃ©lectionnables dans les formulaires vÃ©hicule (ex: Essence, Diesel, Ã‰lectrique).',
    addFuel: 'Ajouter un carburant...',
    maintenanceTypes: "Types d'Interventions de Maintenance",
    maintenanceTypesDesc: 'CatÃ©gories de travaux de maintenance (ex: PrÃ©ventif, Correctif, RÃ©paration accident).',
    addIntervention: 'Ajouter une intervention...',
    extrasCatalog: "Catalogue d'Options & Accessoires",
    extrasCatalogDesc: 'Options supplÃ©mentaires disponibles lors de la crÃ©ation de contrats de location (ex: GPS, SiÃ¨ge enfant).',
    optionName: "Nom de l'option",
    optionRate: 'Tarif (â‚¬/j)',
    saveRefData: 'Enregistrer les DonnÃ©es RÃ©f',
    itemsConfigured: 'Ã©lÃ©ments configurÃ©s',
    noItems: 'Aucun Ã©lÃ©ment configurÃ©.',
    adminProfile: 'Profil Administrateur',
    adminProfileDesc: 'Mettez Ã  jour votre nom affichÃ© et vos informations de compte.',
    usernameReadOnly: "Nom d'utilisateur (Lecture Seule)",
    fullName: 'Nom Complet',
    updateProfile: 'Mettre Ã  jour le Profil',
    security: 'SÃ©curitÃ© & Authentification',
    securityDesc: 'Changez votre mot de passe pour sÃ©curiser votre compte.',
    currentPassword: 'Mot de Passe Actuel',
    newPassword: 'Nouveau Mot de Passe',
    confirmPassword: 'Confirmer le Nouveau Mot de Passe',
    changePassword: 'Changer le Mot de Passe',
  },

  // â”€â”€â”€ Statuses (reusable) â”€â”€â”€
  statuses: {
    available: 'Disponible',
    rented: 'LouÃ©',
    inMaintenance: 'En Maintenance',
    reserved: 'RÃ©servÃ©',
    immobilized: 'ImmobilisÃ©',
    draft: 'Brouillon',
    active: 'Actif',
    completed: 'TerminÃ©',
    cancelled: 'AnnulÃ©',
    scheduled: 'PlanifiÃ©',
    inProgress: 'En cours',
    unpaid: 'Non PayÃ©',
    partiallyPaid: 'Partiellement PayÃ©',
    paid: 'PayÃ©',
    valid: 'Valide',
    expiringSoon: 'BientÃ´t',
    expired: 'ExpirÃ©',
    ok: 'Conforme',
    warningStatus: 'BientÃ´t DÃ»',
    due: 'Ã€ Remplacer',
  },
};

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\interceptors\auth.interceptor.ts
```typescript
import { HttpInterceptorFn, HttpRequest, HttpHandlerFn, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, Observable, throwError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> => {
  const router = inject(Router);
  const token = localStorage.getItem('parc_auto_token');

  let authReq = req;
  if (token) {
    authReq = req.clone({
      headers: req.headers.set('Authorization', `Bearer ${token}`)
    });
  }

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        localStorage.removeItem('parc_auto_token');
        localStorage.removeItem('parc_auto_user');
        router.navigate(['/login']);
      }
      return throwError(() => error);
    })
  );
};

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\layout\app-layout\app-layout.component.ts
```typescript
import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavigationEnd, NavigationStart, Router, RouterLink, RouterOutlet } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { I18nService, Lang } from '../../services/i18n.service';
import { TranslatePipe } from '../../pipes/translate.pipe';
import { filter, Subscription } from 'rxjs';

@Component({
  selector: 'app-app-layout',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterOutlet, TranslatePipe],
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

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\alerts\alerts.component.ts
```typescript
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

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\clients\clients.component.ts
```typescript
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api.service';
import { DialogModule } from 'primeng/dialog';
import { DatePickerModule } from 'primeng/datepicker';
import { I18nService } from '../../services/i18n.service';
import { TranslatePipe } from '../../pipes/translate.pipe';
import { AppCurrencyPipe } from '../../pipes/app-currency.pipe';

@Component({
  selector: 'app-clients',
  standalone: true,
  imports: [CommonModule, FormsModule, DialogModule, DatePickerModule, TranslatePipe, AppCurrencyPipe],
  templateUrl: './clients.component.html',
  styleUrls: ['./clients.component.css']
})
export class ClientsComponent implements OnInit {
  clients: any[] = [];
  totalCount = 0;
  page = 1;
  pageSize = 10;
  pages: number[] = [];

  searchQuery = '';

  // Options
  licenseCategories: string[] = ['A', 'B', 'C', 'D', 'E'];

  // CRUD Dialog States
  showCrudDialog = false;
  isEditMode = false;
  clientForm: any = this.getEmptyForm();

  // Details Modal
  showDetailsDialog = false;
  selectedClient: any = null;
  rentalHistory: any[] = [];

  constructor(private api: ApiService, public i18n: I18nService) {}

  ngOnInit(): void {
    this.loadClients();
  }

  loadClients(): void {
    this.api.getClients(this.searchQuery, this.page, this.pageSize).subscribe({
      next: (res) => {
        this.clients = res.data;
        this.totalCount = res.totalCount;
        this.updatePagesArray();
      },
      error: (err) => console.error('Failed to load clients', err)
    });
  }

  onSearchChange(): void {
    this.page = 1;
    this.loadClients();
  }

  onPageChange(page: number): void {
    this.page = page;
    this.loadClients();
  }

  updatePagesArray(): void {
    const totalPages = Math.ceil(this.totalCount / this.pageSize);
    this.pages = [];
    for (let i = 1; i <= totalPages; i++) {
      this.pages.push(i);
    }
  }

  getEmptyForm(): any {
    return {
      fullName: '',
      nationalId: '',
      dateOfBirth: new Date(new Date().setFullYear(new Date().getFullYear() - 25)), // default ~25 years old
      licenseNumber: '',
      licenseCategory: 'B',
      licenseIssueDate: new Date(new Date().setFullYear(new Date().getFullYear() - 3)),
      licenseExpiryDate: new Date(new Date().setFullYear(new Date().getFullYear() + 5)),
      phone: '',
      email: '',
      address: '',
      notes: ''
    };
  }

  openAddDialog(): void {
    this.isEditMode = false;
    this.clientForm = this.getEmptyForm();
    this.showCrudDialog = true;
  }

  openEditDialog(client: any): void {
    this.isEditMode = true;
    this.clientForm = {
      ...client,
      dateOfBirth: new Date(client.dateOfBirth),
      licenseIssueDate: new Date(client.licenseIssueDate),
      licenseExpiryDate: new Date(client.licenseExpiryDate)
    };
    this.showCrudDialog = true;
  }

  openDetails(client: any): void {
    this.api.getClientById(client.id).subscribe({
      next: (res) => {
        this.selectedClient = res.client;
        this.rentalHistory = res.history;
        this.showDetailsDialog = true;
      },
      error: (err) => console.error('Failed to load client details', err)
    });
  }

  deleteClient(id: number): void {
    if (confirm(this.i18n.t('common.deleteConfirm'))) {
      this.api.deleteClient(id).subscribe({
        next: () => {
          this.loadClients();
        },
        error: (err) => alert(err.error?.message || this.i18n.t('common.errorOccurred'))
      });
    }
  }

  onSubmitClient(): void {
    const payload = {
      ...this.clientForm,
      dateOfBirth: this.clientForm.dateOfBirth.toISOString(),
      licenseIssueDate: this.clientForm.licenseIssueDate.toISOString(),
      licenseExpiryDate: this.clientForm.licenseExpiryDate.toISOString()
    };

    if (this.isEditMode) {
      this.api.updateClient(this.clientForm.id, payload).subscribe({
        next: () => {
          this.showCrudDialog = false;
          this.loadClients();
        },
        error: (err) => alert(err.error?.message || this.i18n.t('vehicles.errorUpdate'))
      });
    } else {
      this.api.createClient(payload).subscribe({
        next: () => {
          this.showCrudDialog = false;
          this.loadClients();
        },
        error: (err) => alert(err.error?.message || this.i18n.t('vehicles.errorCreate'))
      });
    }
  }

  isLicenseExpired(expiryDateStr: string): boolean {
    const expiry = new Date(expiryDateStr);
    return expiry < new Date();
  }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\consumables\consumables.component.ts
```typescript
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api.service';
import { TableModule } from 'primeng/table';
import { DialogModule } from 'primeng/dialog';
import { DatePickerModule } from 'primeng/datepicker';
import { I18nService } from '../../services/i18n.service';
import { TranslatePipe } from '../../pipes/translate.pipe';

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

  constructor(private api: ApiService, public i18n: I18nService) {}

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
      alert(this.i18n.t('consumables.selectVehiclePrompt'));
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
      error: (err) => alert(err.error?.message || this.i18n.t('vehicles.errorCreate'))
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
      alert('Veuillez spÃ©cifier le type de consommable.');
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

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\contracts\contracts.component.ts
```typescript
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { DialogModule } from 'primeng/dialog';
import { DatePickerModule } from 'primeng/datepicker';
import { I18nService } from '../../services/i18n.service';
import { TranslatePipe } from '../../pipes/translate.pipe';
import { AppCurrencyPipe } from '../../pipes/app-currency.pipe';

// @ts-ignore
import html2pdf from 'html2pdf.js';

@Component({
  selector: 'app-contracts',
  standalone: true,
  imports: [CommonModule, FormsModule, DialogModule, DatePickerModule, TranslatePipe, AppCurrencyPipe],
  templateUrl: './contracts.component.html',
  styleUrls: ['./contracts.component.css']
})
export class ContractsComponent implements OnInit {
  today = new Date();
  contracts: any[] = [];
  totalCount = 0;
  page = 1;
  pageSize = 10;
  pages: number[] = [];

  searchQuery = '';
  filterStatus = '';
  filterPaymentStatus = '';

  // Options
  contractTypes: string[] = ['Daily', 'Weekly', 'Monthly', 'Long-term'];
  paymentMethods: string[] = ['Cash', 'Card', 'BankTransfer', 'Cheque'];
  paymentStatuses = [
    { label: 'Non PayÃ©', value: 'Unpaid' },
    { label: 'Partiellement PayÃ©', value: 'PartiallyPaid' },
    { label: 'PayÃ©', value: 'Paid' }
  ];

  // Selections lists for the form
  availableVehicles: any[] = [];
  activeClients: any[] = [];

  // CRUD Dialog States
  showCrudDialog = false;
  isEditMode = false;
  contractForm: any = this.getEmptyForm();
  licenseWarning = '';

  // Return dialog states
  showReturnDialog = false;
  returnForm = this.getEmptyReturnForm();
  selectedContractForReturn: any = null;

  // Print invoice state
  showPrintDialog = false;
  printContract: any = null;

  constructor(private api: ApiService, private route: ActivatedRoute, public i18n: I18nService) {}

  ngOnInit(): void {
    // Read route query parameters for search if any
    this.route.queryParams.subscribe(params => {
      if (params['search']) {
        this.searchQuery = params['search'];
      }
      this.loadContracts();
    });
    this.loadFormSelections();
  }

  loadContracts(): void {
    this.api.getContracts(
      this.searchQuery,
      this.filterStatus,
      this.filterPaymentStatus,
      this.page,
      this.pageSize
    ).subscribe({
      next: (res) => {
        this.contracts = res.data;
        this.totalCount = res.totalCount;
        this.updatePagesArray();
      },
      error: (err) => console.error('Failed to load contracts', err)
    });
  }

  loadFormSelections(): void {
    // Load available/reserved vehicles
    this.api.getVehicles('', '', '', 'Available', 1, 100).subscribe(res => {
      this.availableVehicles = res.data;
    });
    // Load clients
    this.api.getClients('', 1, 100).subscribe(res => {
      this.activeClients = res.data;
    });
  }

  onFilterChange(): void {
    this.page = 1;
    this.loadContracts();
  }

  onPageChange(page: number): void {
    this.page = page;
    this.loadContracts();
  }

  updatePagesArray(): void {
    const totalPages = Math.ceil(this.totalCount / this.pageSize);
    this.pages = [];
    for (let i = 1; i <= totalPages; i++) {
      this.pages.push(i);
    }
  }

  getEmptyForm(): any {
    return {
      contractNumber: '',
      clientId: null,
      vehicleId: null,
      contractType: 'Daily',
      startDate: new Date(),
      expectedReturnDate: new Date(new Date().setDate(new Date().getDate() + 3)), // default 3 days
      dailyRate: 0,
      rentalDays: 3,
      totalAmount: 0,
      additionalCharges: 0,
      extrasCharges: 0,
      discountAmount: 0,
      finalAmountDue: 0,
      paymentStatus: 'Unpaid',
      amountPaid: 0,
      paymentMethod: 'Cash',
      depositAmount: 150,
      depositStatus: 'Collected',
      contractStatus: 'Draft',
      notes: ''
    };
  }

  getEmptyReturnForm(): any {
    return {
      kmReturn: 0,
      returnDate: new Date(),
      fuelPenalty: 0,
      damageFees: 0,
      extrasCharges: 0,
      setInMaintenance: false,
      returnNotes: ''
    };
  }

  openAddDialog(): void {
    this.isEditMode = false;
    this.contractForm = this.getEmptyForm();
    this.licenseWarning = '';
    this.showCrudDialog = true;
    this.loadFormSelections(); // Refresh lists
  }

  openEditDialog(contract: any): void {
    this.isEditMode = true;
    this.contractForm = {
      ...contract,
      startDate: new Date(contract.startDate),
      expectedReturnDate: new Date(contract.expectedReturnDate)
    };
    this.licenseWarning = '';
    this.showCrudDialog = true;
  }

  onClientSelect(): void {
    const client = this.activeClients.find(c => c.id === this.contractForm.clientId);
    if (client) {
      const expiry = new Date(client.licenseExpiryDate);
      if (expiry < new Date()) {
        this.licenseWarning = `${this.i18n.t('contracts.licenseWarning')} ${client.licenseExpiryDate.substring(0,10)}!`;
      } else {
        this.licenseWarning = '';
      }
    }
  }

  onVehicleSelect(): void {
    const vehicle = this.availableVehicles.find(v => v.id === this.contractForm.vehicleId);
    if (vehicle) {
      this.contractForm.dailyRate = vehicle.dailyRate;
      this.calculateTotals();
    }
  }

  calculateTotals(): void {
    if (this.contractForm.startDate && this.contractForm.expectedReturnDate) {
      const start = new Date(this.contractForm.startDate);
      const end = new Date(this.contractForm.expectedReturnDate);
      const diffTime = end.getTime() - start.getTime();
      const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
      
      this.contractForm.rentalDays = diffDays > 0 ? diffDays : 1;
      this.contractForm.totalAmount = this.contractForm.rentalDays * this.contractForm.dailyRate;
      this.contractForm.finalAmountDue = 
        this.contractForm.totalAmount + 
        Number(this.contractForm.additionalCharges) + 
        Number(this.contractForm.extrasCharges) - 
        Number(this.contractForm.discountAmount);
        
      if (this.contractForm.paymentStatus === 'Paid') {
        this.contractForm.amountPaid = this.contractForm.finalAmountDue;
      }
    }
  }

  onPaymentStatusChange(): void {
    if (this.contractForm.paymentStatus === 'Paid') {
      this.contractForm.amountPaid = this.contractForm.finalAmountDue;
    }
  }

  onSubmitContract(): void {
    const payload = {
      ...this.contractForm,
      startDate: this.contractForm.startDate.toISOString(),
      expectedReturnDate: this.contractForm.expectedReturnDate.toISOString()
    };

    if (this.isEditMode) {
      this.api.updateContract(this.contractForm.id, payload).subscribe({
        next: () => {
          this.showCrudDialog = false;
          this.loadContracts();
        },
        error: (err) => alert(err.error?.message || this.i18n.t('vehicles.errorUpdate'))
      });
    } else {
      this.api.createContract(payload).subscribe({
        next: () => {
          this.showCrudDialog = false;
          this.loadContracts();
        },
        error: (err) => alert(err.error?.message || this.i18n.t('vehicles.errorCreate'))
      });
    }
  }

  // ================= Return Vehicle Actions =================
  openReturnDialog(contract: any): void {
    this.selectedContractForReturn = contract;
    this.returnForm = this.getEmptyReturnForm();
    this.returnForm.kmReturn = contract.vehicle?.currentKm || contract.kmDeparture;
    this.showReturnDialog = true;
  }

  submitReturn(): void {
    const payload = {
      ...this.returnForm,
      returnDate: this.returnForm.returnDate.toISOString()
    };

    this.api.returnVehicle(this.selectedContractForReturn.id, payload).subscribe({
      next: () => {
        this.showReturnDialog = false;
        this.loadContracts();
      },
      error: (err) => alert(err.error?.message || this.i18n.t('contracts.errorReturn'))
    });
  }

  // ================= Invoice Print Layout =================
  deleteContract(contract: any): void {
    if (confirm(this.i18n.t('common.deleteConfirm'))) {
      this.api.deleteContract(contract.id).subscribe({
        next: () => this.loadContracts(),
        error: (err) => console.error('Failed to delete contract', err)
      });
    }
  }

  openInvoice(contract: any): void {
    this.printContract = contract;
    this.showPrintDialog = true;
  }

  printInvoice(): void {
    window.print();
  }

  generatePdf(): void {
    const element = document.getElementById('invoice-print-area');
    if (!element) return;
    
    const opt: any = {
      margin:       0.5,
      filename:     `Contrat_${this.printContract.contractNumber}.pdf`,
      image:        { type: 'jpeg', quality: 0.98 },
      html2canvas:  { scale: 2 },
      jsPDF:        { unit: 'in', format: 'a4', orientation: 'portrait' }
    };

    html2pdf().from(element).set(opt).save();
  }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\dashboard\dashboard.component.ts
```typescript
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

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\fuel\fuel.component.ts
```typescript
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

@Component({
  selector: 'app-fuel',
  standalone: true,
  imports: [CommonModule, FormsModule, TableModule, DialogModule, DatePickerModule, ChartModule, TranslatePipe],
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
      notes: 'Saisie manuelle odomÃ¨tre (VÃ©rification pÃ©riodique).'
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

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\insurance-inspections\insurance-inspections.component.ts
```typescript
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../services/api.service';
import { TableModule } from 'primeng/table';
import { I18nService } from '../../services/i18n.service';
import { TranslatePipe } from '../../pipes/translate.pipe';

@Component({
  selector: 'app-insurance-inspections',
  standalone: true,
  imports: [CommonModule, TableModule, TranslatePipe],
  templateUrl: './insurance-inspections.component.html',
  styleUrls: ['./insurance-inspections.component.css']
})
export class InsuranceInspectionsComponent implements OnInit {
  policies: any[] = [];
  inspections: any[] = [];

  constructor(private api: ApiService, public i18n: I18nService) {}

  ngOnInit(): void {
    this.loadInsuranceAndInspections();
  }

  loadInsuranceAndInspections(): void {
    this.api.getAlerts().subscribe({
      next: (res) => {
        // Filter alerts by type 'Insurance' and 'Inspection'
        this.policies = res.alerts
          .filter((a: any) => a.type === 'Insurance')
          .map((a: any) => ({
            vehicle: a.target,
            message: a.message,
            daysOrKm: a.daysOrKmLeftText,
            severity: a.severity,
            date: a.dateConcerned,
            vehicleId: a.vehicleId
          }));

        this.inspections = res.alerts
          .filter((a: any) => a.type === 'Inspection')
          .map((a: any) => ({
            vehicle: a.target,
            message: a.message,
            daysOrKm: a.daysOrKmLeftText,
            severity: a.severity,
            date: a.dateConcerned,
            vehicleId: a.vehicleId
          }));
      },
      error: (err) => console.error('Failed to load safety alerts', err)
    });
  }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\login\login.component.ts
```typescript
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { I18nService, Lang } from '../../services/i18n.service';
import { TranslatePipe } from '../../pipes/translate.pipe';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, TranslatePipe],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  username = '';
  password = '';
  errorMessage = '';
  isLoading = false;

  languages: { code: Lang; label: string; flag: string }[] = [];

  constructor(private api: ApiService, private router: Router, public i18n: I18nService) {
    this.languages = this.i18n.getLanguages();
    // Redirect if already logged in
    if (localStorage.getItem('parc_auto_token')) {
      this.router.navigate(['/dashboard']);
    }
  }

  switchLang(lang: Lang): void {
    this.i18n.setLang(lang);
  }

  onSubmit(): void {
    if (!this.username || !this.password) {
      this.errorMessage = this.i18n.t('login.errorRequired');
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.api.login({ username: this.username, password: this.password }).subscribe({
      next: (res) => {
        localStorage.setItem('parc_auto_token', res.token);
        localStorage.setItem('parc_auto_user', JSON.stringify(res.user));
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        this.isLoading = false;
        if (err.status === 401) {
          this.errorMessage = this.i18n.t('login.errorInvalid');
        } else {
          this.errorMessage = this.i18n.t('login.errorServer');
        }
        console.error('Login error', err);
      }
    });
  }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\maintenance\maintenance.component.ts
```typescript
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api.service';
import { DialogModule } from 'primeng/dialog';
import { DatePickerModule } from 'primeng/datepicker';
import { I18nService } from '../../services/i18n.service';
import { TranslatePipe } from '../../pipes/translate.pipe';
import { AppCurrencyPipe } from '../../pipes/app-currency.pipe';

@Component({
  selector: 'app-maintenance',
  standalone: true,
  imports: [CommonModule, FormsModule, DialogModule, DatePickerModule, TranslatePipe, AppCurrencyPipe],
  templateUrl: './maintenance.component.html',
  styleUrls: ['./maintenance.component.css']
})
export class MaintenanceComponent implements OnInit {
  maintenances: any[] = [];
  vehicles: any[] = [];

  // Options
  maintenanceTypes = ['Preventive', 'Corrective', 'AccidentRepair', 'Inspection'];
  statuses = [
    { label: 'PlanifiÃ©', value: 'Scheduled' },
    { label: 'En Cours', value: 'InProgress' },
    { label: 'TerminÃ©', value: 'Completed' }
  ];

  // Dialog CRUD
  showCrudDialog = false;
  isEditMode = false;
  maintenanceForm: any = this.getEmptyForm();
  uploadingInvoice = false;

  // Calendar View
  showCalendarDialog = false;
  calendarEvents: any[] = [];

  constructor(private api: ApiService, public i18n: I18nService) {}

  ngOnInit(): void {
    this.loadMaintenances();
    this.loadVehicles();
  }

  loadMaintenances(): void {
    this.api.getMaintenances().subscribe({
      next: (res) => this.maintenances = res,
      error: (err) => console.error('Failed to load maintenance records', err)
    });
  }

  loadVehicles(): void {
    this.api.getVehicles('', '', '', '', 1, 100).subscribe(res => {
      this.vehicles = res.data;
    });
  }

  getEmptyForm(): any {
    return {
      vehicleId: null,
      maintenanceType: 'Preventive',
      datePerformed: new Date(),
      nextScheduledDate: null,
      kmAtMaintenance: 0,
      workshopName: '',
      workshopAddress: '',
      workshopContact: '',
      description: '',
      laborCost: 0,
      partsCost: 0,
      totalCost: 0,
      invoiceNumber: '',
      invoiceFilePath: '',
      status: 'Scheduled'
    };
  }

  openAddDialog(): void {
    this.isEditMode = false;
    this.maintenanceForm = this.getEmptyForm();
    this.showCrudDialog = true;
    this.loadVehicles();
  }

  openEditDialog(maint: any): void {
    this.isEditMode = true;
    this.maintenanceForm = {
      ...maint,
      datePerformed: new Date(maint.datePerformed),
      nextScheduledDate: maint.nextScheduledDate ? new Date(maint.nextScheduledDate) : null
    };
    this.showCrudDialog = true;
  }

  deleteMaint(id: number): void {
    if (confirm(this.i18n.t('common.deleteConfirm'))) {
      this.api.deleteMaintenance(id).subscribe(() => {
        this.loadMaintenances();
      });
    }
  }

  onSubmitMaint(): void {
    const payload = {
      ...this.maintenanceForm,
      datePerformed: this.maintenanceForm.datePerformed.toISOString(),
      nextScheduledDate: this.maintenanceForm.nextScheduledDate ? this.maintenanceForm.nextScheduledDate.toISOString() : null
    };

    if (this.isEditMode) {
      this.api.updateMaintenance(this.maintenanceForm.id, payload).subscribe({
        next: () => {
          this.showCrudDialog = false;
          this.loadMaintenances();
        },
        error: (err) => alert(err.error?.message || this.i18n.t('vehicles.errorUpdate'))
      });
    } else {
      this.api.createMaintenance(payload).subscribe({
        next: () => {
          this.showCrudDialog = false;
          this.loadMaintenances();
        },
        error: (err) => alert(err.error?.message || this.i18n.t('vehicles.errorCreate'))
      });
    }
  }

  onInvoiceUpload(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.uploadingInvoice = true;
      this.api.uploadMaintenanceInvoice(file).subscribe({
        next: (res) => {
          this.maintenanceForm.invoiceFilePath = res.invoiceFilePath;
          this.uploadingInvoice = false;
        },
        error: () => { this.uploadingInvoice = false; }
      });
    }
  }

  openCalendar(): void {
    this.api.getMaintenanceCalendar().subscribe({
      next: (res: any) => {
        this.calendarEvents = res;
        this.showCalendarDialog = true;
      },
      error: (err: any) => console.error('Failed to load calendar events', err)
    });
  }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\reports\reports.component.ts
```typescript
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

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\settings\settings.component.ts
```typescript
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api.service';
import { I18nService } from '../../services/i18n.service';
import { TranslatePipe } from '../../pipes/translate.pipe';

@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [CommonModule, FormsModule, TranslatePipe],
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

  // Accordion state for reference data sections
  openRefSection: string | null = null;

  constructor(private api: ApiService, public i18n: I18nService) {}

  ngOnInit(): void {
    this.loadSettings();
    this.loadProfile();
  }

  toggleRefSection(section: string): void {
    this.openRefSection = this.openRefSection === section ? null : section;
  }

  loadSettings(): void {
    this.api.getSettings().subscribe({
      next: (res) => {
        this.settings = res;
        if (this.settings.currencySymbol) {
          localStorage.setItem('parc_auto_currency', this.settings.currencySymbol);
        }
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
        if (this.settings.currencySymbol) {
          localStorage.setItem('parc_auto_currency', this.settings.currencySymbol);
        }
        this.parseReferenceData();
        this.showFeedback(this.getFeedbackMsg('saveSuccess'), 'success');
      },
      error: (err) => this.showFeedback(err.error?.message || this.getFeedbackMsg('saveError'), 'error')
    });
  }

  updateProfile(): void {
    this.api.updateProfile({ fullName: this.profile.fullName }).subscribe({
      next: (res) => {
        this.profile = res.user;
        this.showFeedback(this.getFeedbackMsg('profileSuccess'), 'success');
      },
      error: (err) => this.showFeedback(err.error?.message || this.getFeedbackMsg('profileError'), 'error')
    });
  }

  updatePassword(): void {
    if (this.passwordForm.newPassword !== this.passwordForm.confirmPassword) {
      this.showFeedback(this.getFeedbackMsg('passwordMatchError'), 'error');
      return;
    }

    const payload = {
      currentPassword: this.passwordForm.currentPassword,
      newPassword: this.passwordForm.newPassword
    };

    this.api.changePassword(payload).subscribe({
      next: () => {
        this.showFeedback(this.getFeedbackMsg('passwordSuccess'), 'success');
        this.passwordForm = { currentPassword: '', newPassword: '', confirmPassword: '' };
      },
      error: (err) => this.showFeedback(err.error?.message || this.getFeedbackMsg('passwordError'), 'error')
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

  getFeedbackMsg(key: string): string {
    const lang = this.i18n.currentLang();
    const msgs: Record<string, Record<string, string>> = {
      saveSuccess: {
        fr: 'ParamÃ¨tres sauvegardÃ©s avec succÃ¨s !',
        en: 'Settings saved successfully!',
        ar: 'ØªÙ… Ø­ÙØ¸ Ø§Ù„Ø¥Ø¹Ø¯Ø§Ø¯Ø§Øª Ø¨Ù†Ø¬Ø§Ø­!'
      },
      saveError: {
        fr: 'Erreur lors de la sauvegarde des paramÃ¨tres',
        en: 'Error saving settings',
        ar: 'Ø®Ø·Ø£ Ø£Ø«Ù†Ø§Ø¡ Ø­ÙØ¸ Ø§Ù„Ø¥Ø¹Ø¯Ø§Ø¯Ø§Øª'
      },
      profileSuccess: {
        fr: 'Profil mis Ã  jour avec succÃ¨s !',
        en: 'Profile updated successfully!',
        ar: 'ØªÙ… ØªØ­Ø¯ÙŠØ« Ø§Ù„Ù…Ù„Ù Ø§Ù„Ø´Ø®ØµÙŠ Ø¨Ù†Ø¬Ø§Ø­!'
      },
      profileError: {
        fr: 'Erreur lors de la mise Ã  jour du profil',
        en: 'Error updating profile',
        ar: 'Ø®Ø·Ø£ Ø£Ø«Ù†Ø§Ø¡ ØªØ­Ø¯ÙŠØ« Ø§Ù„Ù…Ù„Ù Ø§Ù„Ø´Ø®ØµÙŠ'
      },
      passwordMatchError: {
        fr: 'Les nouveaux mots de passe ne correspondent pas.',
        en: 'New passwords do not match.',
        ar: 'ÙƒÙ„Ù…Ø§Øª Ø§Ù„Ù…Ø±ÙˆØ± Ø§Ù„Ø¬Ø¯ÙŠØ¯Ø© ØºÙŠØ± Ù…ØªØ·Ø§Ø¨Ù‚Ø©.'
      },
      passwordSuccess: {
        fr: 'Mot de passe mis Ã  jour avec succÃ¨s !',
        en: 'Password updated successfully!',
        ar: 'ØªÙ… ØªØ­Ø¯ÙŠØ« ÙƒÙ„Ù…Ø© Ø§Ù„Ù…Ø±ÙˆØ± Ø¨Ù†Ø¬Ø§Ø­!'
      },
      passwordError: {
        fr: 'Erreur lors de la mise Ã  jour du mot de passe',
        en: 'Error updating password',
        ar: 'Ø®Ø·Ø£ Ø£Ø«Ù†Ø§Ø¡ ØªØ­Ø¯ÙŠØ« ÙƒÙ„Ù…Ø© Ø§Ù„Ù…Ø±ÙˆØ±'
      }
    };
    return msgs[key]?.[lang] || msgs[key]?.['fr'] || key;
  }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\vehicles\vehicles.component.ts
```typescript
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api.service';
import { TableModule } from 'primeng/table';
import { DialogModule } from 'primeng/dialog';
import { SelectModule } from 'primeng/select';
import { DatePickerModule } from 'primeng/datepicker';
import { I18nService } from '../../services/i18n.service';
import { TranslatePipe } from '../../pipes/translate.pipe';
import { AppCurrencyPipe } from '../../pipes/app-currency.pipe';

@Component({
  selector: 'app-vehicles',
  standalone: true,
  imports: [CommonModule, FormsModule, TableModule, DialogModule, SelectModule, DatePickerModule, TranslatePipe, AppCurrencyPipe],
  templateUrl: './vehicles.component.html',
  styleUrls: ['./vehicles.component.css']
})
export class VehiclesComponent implements OnInit {
  vehicles: any[] = [];
  totalCount = 0;
  page = 1;
  pageSize = 9;
  pages: number[] = [];

  // Search & Filters
  searchQuery = '';
  filterType = '';
  filterFuelType = '';
  filterStatus = '';

  // Options
  vehicleTypes: string[] = ['Car', 'SUV', 'Van', 'Truck', 'Motorcycle'];
  fuelTypes: string[] = ['Gasoline', 'Diesel', 'Electric', 'Hybrid', 'LPG'];
  transmissions: string[] = ['Manual', 'Automatic'];
  statuses = [
    { label: 'Disponible', value: 'Available' },
    { label: 'LouÃ©', value: 'Rented' },
    { label: 'En Maintenance', value: 'InMaintenance' },
    { label: 'RÃ©servÃ©', value: 'Reserved' },
    { label: 'ImmobilisÃ©', value: 'Immobilized' }
  ];

  // Selected Vehicle for detail drawers/modals
  selectedVehicle: any = null;
  showDetailsDialog = false;
  detailsTab = 'consumables'; // consumables, insurance, inspections, fuel, km_history

  // Sub-detail data
  consumablesStatus: any[] = [];
  insurancePolicies: any[] = [];
  inspectionsList: any[] = [];
  fuelLogsList: any[] = [];
  kmHistoryList: any[] = [];

  // Dialog states for Add/Edit CRUD
  showCrudDialog = false;
  isEditMode = false;
  vehicleForm: any = this.getEmptyForm();
  uploadingPhoto = false;

  // Modals for sub-CRUDs
  showAddPolicyDialog = false;
  policyForm: any = this.getEmptyPolicyForm();
  uploadingPolicy = false;

  showAddInspectionDialog = false;
  inspectionForm: any = this.getEmptyInspectionForm();
  uploadingInspection = false;

  showAddFuelDialog = false;
  fuelForm: any = this.getEmptyFuelForm();

  showAddKmDialog = false;
  kmForm: any = this.getEmptyKmForm();

  constructor(private api: ApiService, public i18n: I18nService) {}

  ngOnInit(): void {
    this.loadSettings();
    this.loadVehicles();
  }

  loadSettings(): void {
    this.api.getSettings().subscribe({
      next: (res) => {
        if (res.vehicleTypesJson) {
          this.vehicleTypes = JSON.parse(res.vehicleTypesJson);
        }
        if (res.fuelTypesJson) {
          this.fuelTypes = JSON.parse(res.fuelTypesJson);
        }
      }
    });
  }

  loadVehicles(): void {
    this.api.getVehicles(
      this.searchQuery,
      this.filterType,
      this.filterFuelType,
      this.filterStatus,
      this.page,
      this.pageSize
    ).subscribe({
      next: (res) => {
        this.vehicles = res.data;
        this.totalCount = res.totalCount;
        this.updatePagesArray();
      },
      error: (err) => console.error('Failed to load vehicles', err)
    });
  }

  onFilterChange(): void {
    this.page = 1;
    this.loadVehicles();
  }

  onPageChange(page: number): void {
    this.page = page;
    this.loadVehicles();
  }

  updatePagesArray(): void {
    const totalPages = Math.ceil(this.totalCount / this.pageSize);
    this.pages = [];
    for (let i = 1; i <= totalPages; i++) {
      this.pages.push(i);
    }
  }

  getEmptyForm(): any {
    return {
      matricule: '',
      brand: '',
      model: '',
      year: new Date().getFullYear(),
      type: 'Car',
      fuelType: 'Gasoline',
      transmission: 'Manual',
      vin: '',
      engineNumber: '',
      color: '',
      seatsCount: 5,
      dailyRate: 50.0,
      status: 'Available',
      purchaseDate: new Date(),
      purchasePrice: 15000,
      initialKm: 0,
      photoPath: '',
      notes: ''
    };
  }

  getEmptyPolicyForm(): any {
    return {
      insurerName: '',
      policyNumber: '',
      coverageType: 'Third-Party',
      startDate: new Date(),
      expiryDate: new Date(new Date().setFullYear(new Date().getFullYear() + 1)),
      premiumAmount: 200,
      insuredValue: 10000,
      agentContact: '',
      documentPath: ''
    };
  }

  getEmptyInspectionForm(): any {
    return {
      inspectionDate: new Date(),
      expiryDate: new Date(new Date().setFullYear(new Date().getFullYear() + 1)),
      result: 'Pass',
      centerName: '',
      centerAddress: '',
      cost: 50,
      remarks: '',
      documentPath: ''
    };
  }

  getEmptyFuelForm(): any {
    return {
      date: new Date(),
      kmValue: 0,
      liters: 40,
      costPerLiter: 1.5,
      stationName: '',
      fuelType: 'Gasoline'
    };
  }

  getEmptyKmForm(): any {
    return {
      date: new Date(),
      kmValue: 0,
      notes: 'Odometer verification entry.'
    };
  }

  openAddDialog(): void {
    this.isEditMode = false;
    this.vehicleForm = this.getEmptyForm();
    this.showCrudDialog = true;
  }

  openEditDialog(vehicle: any): void {
    this.isEditMode = true;
    this.vehicleForm = {
      ...vehicle,
      purchaseDate: new Date(vehicle.purchaseDate)
    };
    this.showCrudDialog = true;
  }

  deleteVehicle(id: number): void {
    if (confirm(this.i18n.t('vehicles.deleteConfirm'))) {
      this.api.deleteVehicle(id).subscribe({
        next: () => {
          this.loadVehicles();
        },
        error: (err) => alert(err.error?.message || this.i18n.t('vehicles.errorDelete'))
      });
    }
  }

  onSubmitVehicle(): void {
    const payload = {
      ...this.vehicleForm,
      purchaseDate: this.vehicleForm.purchaseDate.toISOString()
    };

    if (this.isEditMode) {
      this.api.updateVehicle(this.vehicleForm.id, payload).subscribe({
        next: () => {
          this.showCrudDialog = false;
          this.loadVehicles();
        },
        error: (err) => alert(err.error?.message || this.i18n.t('vehicles.errorUpdate'))
      });
    } else {
      this.api.createVehicle(payload).subscribe({
        next: () => {
          this.showCrudDialog = false;
          this.loadVehicles();
        },
        error: (err) => alert(err.error?.message || this.i18n.t('vehicles.errorCreate'))
      });
    }
  }

  onPhotoUpload(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.uploadingPhoto = true;
      this.api.uploadVehiclePhoto(file).subscribe({
        next: (res) => {
          this.vehicleForm.photoPath = res.photoPath;
          this.uploadingPhoto = false;
        },
        error: (err) => {
          alert('Failed to upload image');
          this.uploadingPhoto = false;
        }
      });
    }
  }

  // ================= Vehicle Detail Drawer & Sub logs =================
  openDetails(vehicle: any): void {
    this.selectedVehicle = vehicle;
    this.showDetailsDialog = true;
    this.switchDetailsTab('consumables');
  }

  switchDetailsTab(tab: string): void {
    this.detailsTab = tab;
    if (!this.selectedVehicle) return;

    if (tab === 'consumables') {
      this.api.getVehicleConsumablesStatus(this.selectedVehicle.id).subscribe(res => this.consumablesStatus = res.statusReport);
    } else if (tab === 'insurance') {
      this.api.getInsurancePolicies(this.selectedVehicle.id).subscribe(res => this.insurancePolicies = res.all);
    } else if (tab === 'inspections') {
      this.api.getTechnicalInspections(this.selectedVehicle.id).subscribe(res => this.inspectionsList = res.all);
    } else if (tab === 'fuel') {
      this.api.getFuelLogs(this.selectedVehicle.id).subscribe(res => this.fuelLogsList = res);
    } else if (tab === 'km_history') {
      this.api.getKmHistory(this.selectedVehicle.id).subscribe(res => this.kmHistoryList = res);
    }
  }

  // Sub CRUD triggers
  openAddPolicy(): void {
    this.policyForm = this.getEmptyPolicyForm();
    this.showAddPolicyDialog = true;
  }

  submitPolicy(): void {
    const payload = {
      ...this.policyForm,
      vehicleId: this.selectedVehicle.id,
      startDate: this.policyForm.startDate.toISOString(),
      expiryDate: this.policyForm.expiryDate.toISOString()
    };
    this.api.addInsurancePolicy(payload).subscribe(() => {
      this.showAddPolicyDialog = false;
      this.switchDetailsTab('insurance');
    });
  }

  onPolicyFileUpload(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.uploadingPolicy = true;
      this.api.uploadInsurancePolicyFile(file).subscribe({
        next: (res) => {
          this.policyForm.documentPath = res.documentPath;
          this.uploadingPolicy = false;
        },
        error: () => { this.uploadingPolicy = false; }
      });
    }
  }

  openAddInspection(): void {
    this.inspectionForm = this.getEmptyInspectionForm();
    this.showAddInspectionDialog = true;
  }

  submitInspection(): void {
    const payload = {
      ...this.inspectionForm,
      vehicleId: this.selectedVehicle.id,
      inspectionDate: this.inspectionForm.inspectionDate.toISOString(),
      expiryDate: this.inspectionForm.expiryDate.toISOString()
    };
    this.api.addTechnicalInspection(payload).subscribe(() => {
      this.showAddInspectionDialog = false;
      this.switchDetailsTab('inspections');
    });
  }

  onInspectionFileUpload(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.uploadingInspection = true;
      this.api.uploadTechnicalInspectionFile(file).subscribe({
        next: (res) => {
          this.inspectionForm.documentPath = res.documentPath;
          this.uploadingInspection = false;
        },
        error: () => { this.uploadingInspection = false; }
      });
    }
  }

  openAddFuel(): void {
    this.fuelForm = this.getEmptyFuelForm();
    this.fuelForm.kmValue = this.selectedVehicle.currentKm;
    this.showAddFuelDialog = true;
  }

  submitFuel(): void {
    const payload = {
      ...this.fuelForm,
      vehicleId: this.selectedVehicle.id,
      date: this.fuelForm.date.toISOString(),
      fuelType: this.selectedVehicle.fuelType
    };
    this.api.addFuelLog(payload).subscribe({
      next: () => {
        this.showAddFuelDialog = false;
        this.selectedVehicle.currentKm = Math.max(this.selectedVehicle.currentKm, payload.kmValue);
        this.switchDetailsTab('fuel');
      },
      error: (err) => alert(err.error?.message || 'Failed to record fill-up')
    });
  }

  openAddKm(): void {
    this.kmForm = this.getEmptyKmForm();
    this.kmForm.kmValue = this.selectedVehicle.currentKm;
    this.showAddKmDialog = true;
  }

  submitKm(): void {
    const payload = {
      ...this.kmForm,
      vehicleId: this.selectedVehicle.id,
      date: this.kmForm.date.toISOString()
    };
    this.api.addKmManualEntry(payload).subscribe({
      next: () => {
        this.showAddKmDialog = false;
        this.selectedVehicle.currentKm = payload.kmValue;
        this.switchDetailsTab('km_history');
      },
      error: (err) => alert(err.error?.message || 'Failed to record odometer reading')
    });
  }

  exportKmCsv(): void {
    window.open(this.api.getKmExportCsvUrl(this.selectedVehicle.id));
  }

  exportFuelCsv(): void {
    window.open(this.api.getFuelExportCsvUrl(this.selectedVehicle.id));
  }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pipes\app-currency.pipe.ts
```typescript
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'appCurrency',
  standalone: true,
  pure: false
})
export class AppCurrencyPipe implements PipeTransform {
  transform(value: number | string | null | undefined): string {
    if (value == null) return '';
    const num = typeof value === 'string' ? parseFloat(value) : value;
    if (isNaN(num)) return '';
    
    // Get symbol from localStorage or default to DZD
    const symbol = localStorage.getItem('parc_auto_currency') || 'DZD';
    
    // Format number (e.g. 1 500,00)
    const formatted = new Intl.NumberFormat('fr-FR', {
      minimumFractionDigits: 0,
      maximumFractionDigits: 2
    }).format(num);

    return `${formatted} ${symbol}`;
  }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pipes\translate.pipe.ts
```typescript
import { Pipe, PipeTransform } from '@angular/core';
import { I18nService } from '../services/i18n.service';

@Pipe({
  name: 't',
  standalone: true,
  pure: false // Impure so it re-evaluates when language changes
})
export class TranslatePipe implements PipeTransform {
  constructor(private i18n: I18nService) {}

  transform(key: string): string {
    return this.i18n.t(key);
  }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\services\api.service.ts
```typescript
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  public readonly baseUrl = 'http://localhost:5222/api';

  constructor(private http: HttpClient) {}

  // ================= Auth =================
  login(dto: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/auth/login`, dto);
  }
  getProfile(): Observable<any> {
    return this.http.get(`${this.baseUrl}/auth/profile`);
  }
  updateProfile(dto: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/auth/profile`, dto);
  }
  changePassword(dto: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/auth/change-password`, dto);
  }

  // ================= Vehicles =================
  getVehicles(search?: string, type?: string, fuelType?: string, status?: string, page = 1, pageSize = 10): Observable<any> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());
    if (search) params = params.set('search', search);
    if (type) params = params.set('type', type);
    if (fuelType) params = params.set('fuelType', fuelType);
    if (status) params = params.set('status', status);

    return this.http.get(`${this.baseUrl}/vehicle`, { params });
  }
  getVehicleById(id: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/vehicle/${id}`);
  }
  createVehicle(vehicle: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/vehicle`, vehicle);
  }
  updateVehicle(id: number, vehicle: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/vehicle/${id}`, vehicle);
  }
  deleteVehicle(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/vehicle/${id}`);
  }
  uploadVehiclePhoto(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post(`${this.baseUrl}/vehicle/upload-photo`, formData);
  }

  // ================= Clients =================
  getClients(search?: string, page = 1, pageSize = 10): Observable<any> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());
    if (search) params = params.set('search', search);

    return this.http.get(`${this.baseUrl}/client`, { params });
  }
  getClientById(id: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/client/${id}`);
  }
  createClient(client: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/client`, client);
  }
  updateClient(id: number, client: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/client/${id}`, client);
  }
  deleteClient(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/client/${id}`);
  }

  // ================= Contracts =================
  getContracts(search?: string, status?: string, paymentStatus?: string, page = 1, pageSize = 10): Observable<any> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());
    if (search) params = params.set('search', search);
    if (status) params = params.set('status', status);
    if (paymentStatus) params = params.set('paymentStatus', paymentStatus);

    return this.http.get(`${this.baseUrl}/contract`, { params });
  }
  getContractById(id: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/contract/${id}`);
  }
  createContract(contract: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/contract`, contract);
  }
  updateContract(id: number, contract: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/contract/${id}`, contract);
  }
  returnVehicle(id: number, dto: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/contract/${id}/return`, dto);
  }

  deleteContract(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/contract/${id}`);
  }

  // ================= Km Suivi =================
  getKmHistory(vehicleId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/km/vehicle/${vehicleId}`);
  }
  addKmManualEntry(entry: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/km`, entry);
  }
  getKmInactivityReport(): Observable<any> {
    return this.http.get(`${this.baseUrl}/km/inactivity-report`);
  }
  getKmExportCsvUrl(vehicleId?: number): string {
    const token = localStorage.getItem('parc_auto_token');
    return `${this.baseUrl}/km/export-csv?access_token=${token}${vehicleId ? `&vehicleId=${vehicleId}` : ''}`;
  }

  // ================= Maintenance =================
  getMaintenances(status?: string, vehicleId?: number): Observable<any> {
    let params = new HttpParams();
    if (status) params = params.set('status', status);
    if (vehicleId) params = params.set('vehicleId', vehicleId.toString());
    return this.http.get(`${this.baseUrl}/maintenance`, { params });
  }
  getMaintenanceById(id: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/maintenance/${id}`);
  }
  createMaintenance(maint: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/maintenance`, maint);
  }
  updateMaintenance(id: number, maint: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/maintenance/${id}`, maint);
  }
  deleteMaintenance(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/maintenance/${id}`);
  }
  getMaintenanceCalendar(): Observable<any> {
    return this.http.get(`${this.baseUrl}/maintenance/calendar`);
  }
  uploadMaintenanceInvoice(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post(`${this.baseUrl}/maintenance/upload-invoice`, formData);
  }

  // ================= Consumables =================
  getConsumableConfigs(): Observable<any> {
    return this.http.get(`${this.baseUrl}/consumable/configs`);
  }
  saveConsumableConfig(config: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/consumable/configs`, config);
  }
  getVehicleConsumablesStatus(vehicleId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/consumable/vehicle/${vehicleId}`);
  }
  addConsumableLog(log: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/consumable/log`, log);
  }
  deleteConsumableLog(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/consumable/log/${id}`);
  }

  // ================= Insurance =================
  getInsurancePolicies(vehicleId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/insurance/vehicle/${vehicleId}`);
  }
  addInsurancePolicy(policy: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/insurance`, policy);
  }
  updateInsurancePolicy(id: number, policy: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/insurance/${id}`, policy);
  }
  deleteInsurancePolicy(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/insurance/${id}`);
  }
  uploadInsurancePolicyFile(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post(`${this.baseUrl}/insurance/upload-policy`, formData);
  }

  // ================= Technical Inspections =================
  getTechnicalInspections(vehicleId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/technicalinspection/vehicle/${vehicleId}`);
  }
  addTechnicalInspection(inspection: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/technicalinspection`, inspection);
  }
  updateTechnicalInspection(id: number, inspection: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/technicalinspection/${id}`, inspection);
  }
  deleteTechnicalInspection(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/technicalinspection/${id}`);
  }
  uploadTechnicalInspectionFile(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post(`${this.baseUrl}/technicalinspection/upload-inspection`, formData);
  }

  // ================= Fuel =================
  getFuelLogs(vehicleId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/fuel/vehicle/${vehicleId}`);
  }
  addFuelLog(log: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/fuel`, log);
  }
  deleteFuelLog(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/fuel/${id}`);
  }
  getFuelExportCsvUrl(vehicleId?: number): string {
    const token = localStorage.getItem('parc_auto_token');
    return `${this.baseUrl}/fuel/export-csv?access_token=${token}${vehicleId ? `&vehicleId=${vehicleId}` : ''}`;
  }

  // ================= Alerts =================
  getAlerts(): Observable<any> {
    return this.http.get(`${this.baseUrl}/alert`);
  }

  // ================= Reports =================
  getFleetStatus(): Observable<any> {
    return this.http.get(`${this.baseUrl}/report/fleet-status`);
  }
  getRevenueReport(start?: string, end?: string): Observable<any> {
    let params = new HttpParams();
    if (start) params = params.set('start', start);
    if (end) params = params.set('end', end);
    return this.http.get(`${this.baseUrl}/report/revenue`, { params });
  }
  getProfitabilityReport(): Observable<any> {
    return this.http.get(`${this.baseUrl}/report/profitability`);
  }
  getTopClients(): Observable<any> {
    return this.http.get(`${this.baseUrl}/report/top-clients`);
  }
  getUnpaidContracts(): Observable<any> {
    return this.http.get(`${this.baseUrl}/report/unpaid-contracts`);
  }
  getProfitabilityCsvUrl(): string {
    const token = localStorage.getItem('parc_auto_token');
    return `${this.baseUrl}/report/export-profitability-csv?access_token=${token}`;
  }

  // ================= Settings =================
  getSettings(): Observable<any> {
    return this.http.get(`${this.baseUrl}/settings`);
  }
  updateSettings(settings: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/settings`, settings);
  }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\services\i18n.service.ts
```typescript
import { Injectable, signal, computed } from '@angular/core';
import { FR } from '../i18n/fr';
import { EN } from '../i18n/en';
import { AR } from '../i18n/ar';

export type Lang = 'fr' | 'en' | 'ar';

const TRANSLATIONS: Record<Lang, Record<string, any>> = {
  fr: FR,
  en: EN,
  ar: AR
};

const STORAGE_KEY = 'parc_auto_lang';

@Injectable({ providedIn: 'root' })
export class I18nService {
  private _lang = signal<Lang>(this.getInitialLang());

  /** Reactive current language */
  readonly currentLang = this._lang.asReadonly();

  /** Whether the current language is RTL */
  readonly isRtl = computed(() => this._lang() === 'ar');

  constructor() {
    this.applyDirection(this._lang());
  }

  /** Switch the active language */
  setLang(lang: Lang): void {
    this._lang.set(lang);
    localStorage.setItem(STORAGE_KEY, lang);
    this.applyDirection(lang);
  }

  /** Translate a key using dot-notation (e.g. 'sidebar.dashboard') */
  t(key: string): string {
    const lang = this._lang();
    const parts = key.split('.');
    let value: any = TRANSLATIONS[lang];

    for (const part of parts) {
      if (value && typeof value === 'object' && part in value) {
        value = value[part];
      } else {
        // Fallback to French, then return the key itself
        let fallback: any = TRANSLATIONS['fr'];
        for (const p of parts) {
          if (fallback && typeof fallback === 'object' && p in fallback) {
            fallback = fallback[p];
          } else {
            return key;
          }
        }
        return typeof fallback === 'string' ? fallback : key;
      }
    }

    return typeof value === 'string' ? value : key;
  }

  /** Dynamically translate alert messages returned from the backend in English */
  translateAlertMessage(msg: string): string {
    const lang = this._lang();
    if (lang === 'en') return msg;

    try {
      // 1. Insurance expired
      if (msg.startsWith('Insurance expired on')) {
        const date = msg.replace('Insurance expired on ', '').replace('.', '').trim();
        if (lang === 'fr') return `Assurance expirÃ©e le ${date}.`;
        if (lang === 'ar') return `Ø§Ù†ØªÙ‡Øª ØµÙ„Ø§Ø­ÙŠØ© Ø§Ù„ØªØ£Ù…ÙŠÙ† Ø¨ØªØ§Ø±ÙŠØ® ${date}.`;
      }
      // "Insurance expiring soon in 5 days."
      if (msg.startsWith('Insurance expiring soon in')) {
        const match = msg.match(/\d+/);
        const days = match ? match[0] : '';
        if (lang === 'fr') return `L'assurance expire bientÃ´t dans ${days} jours.`;
        if (lang === 'ar') return `ÙŠÙ†ØªÙ‡ÙŠ Ø§Ù„ØªØ£Ù…ÙŠÙ† Ù‚Ø±ÙŠØ¨Ø§Ù‹ Ø®Ù„Ø§Ù„ ${days} Ø£ÙŠØ§Ù….`;
      }

      // 2. Technical inspection failed
      if (msg === 'Technical inspection failed! Vehicle requires repairs.') {
        if (lang === 'fr') return `Le contrÃ´le technique a Ã©chouÃ© ! Le vÃ©hicule nÃ©cessite des rÃ©parations.`;
        if (lang === 'ar') return `ÙØ´Ù„ Ø§Ù„ÙØ­Øµ Ø§Ù„ÙÙ†ÙŠ! Ø§Ù„Ù…Ø±ÙƒØ¨Ø© ØªØªØ·Ù„Ø¨ Ø¥ØµÙ„Ø§Ø­Ø§Øª.`;
      }
      // "Technical inspection expired on 2026-06-04."
      if (msg.startsWith('Technical inspection expired on')) {
        const date = msg.replace('Technical inspection expired on ', '').replace('.', '').trim();
        if (lang === 'fr') return `ContrÃ´le technique expirÃ© le ${date}.`;
        if (lang === 'ar') return `Ø§Ù†ØªÙ‡Øª ØµÙ„Ø§Ø­ÙŠØ© Ø§Ù„ÙØ­Øµ Ø§Ù„ÙÙ†ÙŠ Ø¨ØªØ§Ø±ÙŠØ® ${date}.`;
      }
      // "Technical inspection expiring in 5 days."
      if (msg.startsWith('Technical inspection expiring in')) {
        const match = msg.match(/\d+/);
        const days = match ? match[0] : '';
        if (lang === 'fr') return `Le contrÃ´le technique expire dans ${days} jours.`;
        if (lang === 'ar') return `ÙŠÙ†ØªÙ‡ÙŠ Ø§Ù„ÙØ­Øµ Ø§Ù„ÙÙ†ÙŠ Ø®Ù„Ø§Ù„ ${days} Ø£ÙŠØ§Ù….`;
      }

      // 3. Maintenance due
      if (msg.startsWith('Scheduled ') && msg.includes(' is overdue by ')) {
        const parts = msg.replace('Scheduled ', '').split(' is overdue by ');
        const type = parts[0];
        const daysMatch = parts[1] ? parts[1].match(/\d+/) : null;
        const days = daysMatch ? daysMatch[0] : '';
        const translatedType = this.translateMaintenanceType(type);
        if (lang === 'fr') return `La maintenance planifiÃ©e (${translatedType}) est en retard de ${days} jours.`;
        if (lang === 'ar') return `Ø§Ù„ØµÙŠØ§Ù†Ø© Ø§Ù„Ù…Ø¬Ø¯ÙˆÙ„Ø© (${translatedType}) Ù…ØªØ£Ø®Ø±Ø© Ø¨Ù€ ${days} Ø£ÙŠØ§Ù….`;
      }
      if (msg.startsWith('Upcoming scheduled ') && msg.includes(' in ')) {
        const parts = msg.replace('Upcoming scheduled ', '').split(' in ');
        const type = parts[0];
        const daysMatch = parts[1] ? parts[1].match(/\d+/) : null;
        const days = daysMatch ? daysMatch[0] : '';
        const translatedType = this.translateMaintenanceType(type);
        if (lang === 'fr') return `Maintenance planifiÃ©e (${translatedType}) Ã  venir dans ${days} jours.`;
        if (lang === 'ar') return `Ø§Ù„ØµÙŠØ§Ù†Ø© Ø§Ù„Ù…Ø¬Ø¯ÙˆÙ„Ø© Ø§Ù„Ù‚Ø§Ø¯Ù…Ø© (${translatedType}) Ø®Ù„Ø§Ù„ ${days} Ø£ÙŠØ§Ù….`;
      }

      // 4. Odometer inactivity
      if (msg.startsWith('No mileage activity registered for')) {
        const match = msg.match(/\d+/);
        const days = match ? match[0] : '';
        if (lang === 'fr') return `Aucune activitÃ© kilomÃ©trique enregistrÃ©e depuis ${days} jours.`;
        if (lang === 'ar') return `Ù„Ù… ÙŠØªÙ… ØªØ³Ø¬ÙŠÙ„ Ø£ÙŠ Ù†Ø´Ø§Ø· Ù„Ù„Ù…Ø³Ø§ÙØ© Ø§Ù„Ù…Ù‚Ø·ÙˆØ¹Ø© Ù…Ù†Ø° ${days} Ø£ÙŠØ§Ù….`;
      }

      // 5. Driver license
      if (msg.startsWith("Driver's license expired on")) {
        const date = msg.replace("Driver's license expired on ", '').replace('.', '').trim();
        if (lang === 'fr') return `Le permis de conduire a expirÃ© le ${date}.`;
        if (lang === 'ar') return `Ø§Ù†ØªÙ‡Øª ØµÙ„Ø§Ø­ÙŠØ© Ø±Ø®ØµØ© Ø§Ù„Ù‚ÙŠØ§Ø¯Ø© Ø¨ØªØ§Ø±ÙŠØ® ${date}.`;
      }
      if (msg.startsWith("Driver's license expiring in")) {
        const match = msg.match(/\d+/);
        const days = match ? match[0] : '';
        if (lang === 'fr') return `Le permis de conduire expire dans ${days} jours.`;
        if (lang === 'ar') return `ØªÙ†ØªÙ‡ÙŠ ØµÙ„Ø§Ø­ÙŠØ© Ø±Ø®ØµØ© Ø§Ù„Ù‚ÙŠØ§Ø¯Ø© Ø®Ù„Ø§Ù„ ${days} Ø£ÙŠØ§Ù….`;
      }

      // 6. Consumables
      if (msg.includes('replacement is overdue.')) {
        const matchType = msg.match(/^([a-zA-Z]+) replacement/);
        const type = matchType ? matchType[1] : 'Consumable';
        const parsedType = this.translateConsumableType(type);
        const matches = msg.match(/\d+/g);
        if (matches && matches.length >= 4) {
          const [intervalKm, intervalMonths, kmSince, monthsSince] = matches;
          if (lang === 'fr') return `Le remplacement de ${parsedType} est dÃ©passÃ©. Intervalle: ${intervalKm} km / ${intervalMonths} mois. Actuel: ${kmSince} km / ${monthsSince} mois.`;
          if (lang === 'ar') return `ØªØ¬Ø§ÙˆØ² Ù…ÙˆØ¹Ø¯ Ø§Ø³ØªØ¨Ø¯Ø§Ù„ ${parsedType}. Ø§Ù„ÙØ§ØµÙ„ Ø§Ù„Ø²Ù…Ù†ÙŠ: ${intervalKm} ÙƒÙ… / ${intervalMonths} Ø´Ù‡Ø±. Ø§Ù„Ø­Ø§Ù„ÙŠ: ${kmSince} ÙƒÙ… / ${monthsSince} Ø´Ù‡Ø±.`;
        }
      }
      if (msg.includes('replacement approaching due threshold.')) {
        const matchType = msg.match(/^([a-zA-Z]+) replacement/);
        const type = matchType ? matchType[1] : 'Consumable';
        const parsedType = this.translateConsumableType(type);
        const matches = msg.match(/\d+/g);
        if (matches && matches.length >= 2) {
          const [kmSince, monthsSince] = matches;
          if (lang === 'fr') return `Le remplacement de ${parsedType} approche de l'Ã©chÃ©ance. Actuel: ${kmSince} km / ${monthsSince} mois.`;
          if (lang === 'ar') return `ÙŠÙ‚ØªØ±Ø¨ Ù…ÙˆØ¹Ø¯ Ø§Ø³ØªØ¨Ø¯Ø§Ù„ ${parsedType}. Ø§Ù„Ø­Ø§Ù„ÙŠ: ${kmSince} ÙƒÙ… / ${monthsSince} Ø´Ù‡Ø±.`;
        }
      }
    } catch (e) {
      console.warn('Failed to parse alert message translation: ', msg, e);
    }

    return msg;
  }

  translateMaintenanceType(type: string): string {
    const lang = this._lang();
    const key = type.toLowerCase();
    if (lang === 'fr') {
      if (key === 'preventive') return 'PrÃ©ventif';
      if (key === 'corrective') return 'Correctif';
      if (key === 'inspection') return 'Inspection';
      if (key === 'accidentrepair') return 'RÃ©paration accident';
    }
    if (lang === 'ar') {
      if (key === 'preventive') return 'ÙˆÙ‚Ø§Ø¦ÙŠ';
      if (key === 'corrective') return 'ØªØµØ­ÙŠØ­ÙŠ';
      if (key === 'inspection') return 'ÙØ­Øµ';
      if (key === 'accidentrepair') return 'Ø¥sÙ„Ø§Ø­ Ø­Ø§Ø¯Ø«';
    }
    return type;
  }

  translateConsumableType(type: string): string {
    const lang = this._lang();
    const key = type.toLowerCase();
    if (lang === 'fr') {
      if (key === 'oilchange') return "Vidange d'huile";
      if (key === 'airfilter') return 'Filtre Ã  air';
      if (key === 'oilfilter') return 'Filtre Ã  huile';
      if (key === 'fuelfilter') return 'Filtre Ã  carburant';
      if (key === 'cabinfilter') return "Filtre d'habitacle";
      if (key === 'frontbrakes') return 'Freins avant';
      if (key === 'rearbrakes') return 'Freins arriÃ¨re';
      if (key === 'fronttires') return 'Pneus avant';
      if (key === 'reartires') return 'Pneus arriÃ¨re';
      if (key === 'battery') return 'Batterie';
    }
    if (lang === 'ar') {
      if (key === 'oilchange') return 'ØªØºÙŠÙŠØ± Ø§Ù„Ø²ÙŠØª';
      if (key === 'airfilter') return 'ÙÙ„ØªØ± Ø§Ù„Ù‡ÙˆØ§Ø¡';
      if (key === 'oilfilter') return 'ÙÙ„ØªØ± Ø§Ù„Ø²ÙŠØª';
      if (key === 'fuelfilter') return 'ÙÙ„ØªØ± Ø§Ù„ÙˆÙ‚ÙˆØ¯';
      if (key === 'cabinfilter') return 'ÙÙ„ØªØ± Ø§Ù„Ù…Ù‚ØµÙˆØ±Ø©';
      if (key === 'frontbrakes') return 'Ø§Ù„ÙØ±Ø§Ù…Ù„ Ø§Ù„Ø£Ù…Ø§Ù…ÙŠØ©';
      if (key === 'rearbrakes') return 'Ø§Ù„ÙØ±Ø§Ù…Ù„ Ø§Ù„Ø®Ù„ÙÙŠØ©';
      if (key === 'fronttires') return 'Ø§Ù„Ø¥Ø·Ø§Ø±Ø§Øª Ø§Ù„Ø£Ù…Ø§Ù…ÙŠØ©';
      if (key === 'reartires') return 'Ø§Ù„Ø¥Ø·Ø§Ø±Ø§Øª Ø§Ù„Ø®Ù„ÙÙŠØ©';
      if (key === 'battery') return 'Ø§Ù„Ø¨Ø·Ø§Ø±ÙŠØ©';
    }
    return type;
  }

  /** Dynamically translate alert time strings returned from the backend in English */
  translateAlertTime(timeStr: string): string {
    const lang = this._lang();
    if (lang === 'en') return timeStr;

    // "FAILED"
    if (timeStr === 'FAILED') {
      if (lang === 'fr') return 'Ã‰CHEC';
      if (lang === 'ar') return 'ÙØ´Ù„';
    }

    // "X days overdue"
    if (timeStr.includes('days overdue')) {
      const num = timeStr.match(/\d+/)?.[0] || '';
      if (lang === 'fr') return `${num} j de retard`;
      if (lang === 'ar') return `Ù…ØªØ£Ø®Ø± Ø¨Ù€ ${num} ÙŠÙˆÙ…`;
    }

    // "X days remaining"
    if (timeStr.includes('days remaining')) {
      const num = timeStr.match(/\d+/)?.[0] || '';
      if (lang === 'fr') return `${num} j restants`;
      if (lang === 'ar') return `Ù…ØªØ¨Ù‚ÙŠ ${num} ÙŠÙˆÙ…`;
    }

    // "X days left"
    if (timeStr.includes('days left')) {
      const num = timeStr.match(/\d+/)?.[0] || '';
      if (lang === 'fr') return `${num} j restants`;
      if (lang === 'ar') return `Ù…ØªØ¨Ù‚ÙŠ ${num} ÙŠÙˆÙ…`;
    }

    // "X days inactive"
    if (timeStr.includes('days inactive')) {
      const num = timeStr.match(/\d+/)?.[0] || '';
      if (lang === 'fr') return `${num} j inactif`;
      if (lang === 'ar') return `ØºÙŠØ± Ù†Ø´Ø· ${num} ÙŠÙˆÙ…`;
    }

    // "X km overdue"
    if (timeStr.includes('km overdue')) {
      const num = timeStr.match(/\d+/)?.[0] || '';
      if (lang === 'fr') return `${num} km de retard`;
      if (lang === 'ar') return `ØªØ¬Ø§ÙˆØ² Ø¨Ù€ ${num} ÙƒÙ…`;
    }

    // "X km left"
    if (timeStr.includes('km left')) {
      const num = timeStr.match(/\d+/)?.[0] || '';
      if (lang === 'fr') return `${num} km restants`;
      if (lang === 'ar') return `Ù…ØªØ¨Ù‚ÙŠ ${num} ÙƒÙ…`;
    }

    // "X months overdue"
    if (timeStr.includes('months overdue')) {
      const num = timeStr.match(/\d+/)?.[0] || '';
      if (lang === 'fr') return `${num} mois de retard`;
      if (lang === 'ar') return `Ù…ØªØ£Ø®Ø± Ø¨Ù€ ${num} Ø´Ù‡Ø±`;
    }

    return timeStr;
  }

  /** Get all available languages with labels */
  getLanguages(): { code: Lang; label: string; flag: string }[] {
    return [
      { code: 'fr', label: 'FranÃ§ais', flag: 'ðŸ‡«ðŸ‡·' },
      { code: 'en', label: 'English', flag: 'ðŸ‡¬ðŸ‡§' },
      { code: 'ar', label: 'Ø§Ù„Ø¹Ø±Ø¨ÙŠØ©', flag: 'ðŸ‡¸ðŸ‡¦' }
    ];
  }

  private getInitialLang(): Lang {
    const stored = localStorage.getItem(STORAGE_KEY) as Lang | null;
    if (stored && (stored === 'fr' || stored === 'en' || stored === 'ar')) {
      return stored;
    }
    return 'fr';
  }

  private applyDirection(lang: Lang): void {
    if (typeof document !== 'undefined') {
      const html = document.documentElement;
      if (lang === 'ar') {
        html.setAttribute('dir', 'rtl');
        html.setAttribute('lang', 'ar');
        document.body.classList.add('rtl');
      } else {
        html.setAttribute('dir', 'ltr');
        html.setAttribute('lang', lang);
        document.body.classList.remove('rtl');
      }
    }
  }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\index.html
```html
<!doctype html>
<html lang="en">
<head>
  <meta charset="utf-8">
  <title>Frontend</title>
  <base href="/">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="icon" type="image/x-icon" href="favicon.ico">
</head>
<body>
  <app-root></app-root>
</body>
</html>

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\app.html
```html
<router-outlet></router-outlet>

```


