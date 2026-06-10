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
    pricePerLiter: 'Ø³Ø¹Ø± Ø§Ù„Ù„ØªØ± (DZD/Ù„ØªØ±)',
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
    optionRate: 'Ø§Ù„ØªØ¹Ø±ÙØ© (DZD/ÙŠÙˆÙ…)',
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
    premiumValue: 'Premium Amount / Value',
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
    premium: 'Premium Amount',
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
    pricePerLiter: 'Price per Liter (DZD/L)',
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
    optionRate: 'Rate (DZD/day)',
    coverageTypes: 'Insurance Coverage Types',
    coverageTypesDesc: 'Manage coverage types selectable for insurance policies (e.g. Comprehensive, Third Party).',
    addCoverage: 'Add a coverage...',
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
    policyNo: 'NÂ° Contrat d\'assurance',
    coverage: 'Couverture',
    validity: 'ValiditÃ©',
    premiumValue: 'Cotisation / Valeur',
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
    premium: 'Montant de la cotisation',
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
    pricePerLiter: 'Prix par Litre (DZD/L)',
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
    optionRate: 'Tarif (DZD/j)',
    coverageTypes: "Types de Couverture d'Assurance",
    coverageTypesDesc: "GÃ©rez les types de couverture sÃ©lectionnables pour vos contrats d'assurance (ex: Tous Risques, ResponsabilitÃ© Civile).",
    addCoverage: 'Ajouter une couverture...',
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
import { AppCurrencyPipe } from '../../pipes/app-currency.pipe';

@Component({
  selector: 'app-reports',
  standalone: true,
  imports: [CommonModule, FormsModule, TableModule, ChartModule, DatePickerModule, TranslatePipe, AppCurrencyPipe],
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
  coverageTypes: string[] = [];
  extras: any[] = [];

  // Local additions variables
  newVehicleType = '';
  newFuelType = '';
  newMaintenanceType = '';
  newCoverageType = '';
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
      this.coverageTypes = JSON.parse(this.settings.coverageTypesJson || '[]');
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
    this.settings.coverageTypesJson = JSON.stringify(this.coverageTypes);
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

  addCoverageType(): void {
    const val = this.newCoverageType.trim();
    if (val && !this.coverageTypes.includes(val)) {
      this.coverageTypes.push(val);
      this.newCoverageType = '';
    }
  }
  removeCoverageType(index: number): void {
    this.coverageTypes.splice(index, 1);
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



# Parc Auto - Frontend (Angular) - Part 2

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\layout\app-layout\app-layout.component.html
```html
<div class="app-shell">
  <div
    class="sidebar-backdrop"
    [class.is-visible]="sidebarOpen"
    (click)="closeSidebar()"
    aria-hidden="true"
  ></div>

  <!-- Sidebar -->
  <aside class="sidebar" [class.sidebar-open]="sidebarOpen" [class.sidebar-closed]="!sidebarOpen" [class.collapsed]="sidebarCollapsed">
    <div class="sidebar-header">
      <div class="sidebar-brand">
        <span class="sidebar-logo">
          <i class="pi pi-car"></i>
        </span>
        <span class="sidebar-brand-text" *ngIf="!sidebarCollapsed">{{ 'sidebar.brand' | t }}</span>
      </div>
      <button (click)="toggleSidebar()" class="sidebar-close-btn md-hidden" aria-label="Close sidebar">
        <i class="pi pi-times"></i>
      </button>
    </div>

    <nav class="sidebar-nav">
      <a 
        *ngFor="let item of menuItems; trackBy: trackByRoute" 
        [attr.href]="item.route"
        (click)="navigateTo(item.route, $event)"
        [class.active]="isLinkActive(item.route)"
        class="nav-item"
        [title]="sidebarCollapsed ? (item.labelKey | t) : ''"
      >
        <i [class]="item.icon"></i>
        <span class="nav-label" *ngIf="!sidebarCollapsed">{{ item.labelKey | t }}</span>
      </a>
    </nav>

    <div class="sidebar-footer">
      <!-- Collapse toggle (desktop only) -->
      <button (click)="toggleCollapse()" class="collapse-btn" [title]="sidebarCollapsed ? 'Expand' : 'Collapse'">
        <i class="pi" [class.pi-angle-double-right]="sidebarCollapsed" [class.pi-angle-double-left]="!sidebarCollapsed"></i>
      </button>
      <div class="sidebar-user" *ngIf="!sidebarCollapsed">
        <div class="user-avatar">{{ adminName.charAt(0).toUpperCase() }}</div>
        <span class="user-name">{{ adminName }}</span>
      </div>
      <button (click)="logout()" class="btn-icon sidebar-logout" [title]="'common.logout' | t">
        <i class="pi pi-sign-out"></i>
      </button>
    </div>
  </aside>

  <!-- Main -->
  <div class="main-wrapper">
    <!-- Topbar -->
    <header class="topbar">
      <div class="topbar-left">
        <button (click)="toggleSidebar()" class="btn-icon topbar-menu-btn" aria-label="Toggle menu">
          <i class="pi pi-bars"></i>
        </button>
        <h1 class="topbar-title">{{ 'topbar.title' | t }}</h1>
      </div>

      <div class="topbar-right">
        <!-- Language Switcher -->
        <div class="lang-wrapper">
          <button (click)="toggleLangDropdown()" class="btn-icon lang-btn" [title]="'topbar.language' | t">
            <span class="lang-flag">{{ i18n.currentLang() === 'fr' ? 'ðŸ‡«ðŸ‡·' : i18n.currentLang() === 'en' ? 'ðŸ‡¬ðŸ‡§' : 'ðŸ‡¸ðŸ‡¦' }}</span>
            <span class="lang-code">{{ i18n.currentLang() | uppercase }}</span>
          </button>

          <div *ngIf="showLangDropdown" class="lang-dropdown card">
            <button 
              *ngFor="let lang of languages"
              (click)="switchLang(lang.code)"
              class="lang-option"
              [class.active]="i18n.currentLang() === lang.code"
            >
              <span class="lang-option-flag">{{ lang.flag }}</span>
              <span class="lang-option-label">{{ lang.label }}</span>
              <i *ngIf="i18n.currentLang() === lang.code" class="pi pi-check lang-option-check"></i>
            </button>
          </div>
        </div>

        <!-- Alerts Bell -->
        <div class="alerts-wrapper">
          <button (click)="toggleAlertsDropdown()" class="btn-icon alerts-btn">
            <i class="pi pi-bell"></i>
            <span 
              *ngIf="alertsCount > 0"
              class="alerts-dot"
              [class.critical]="criticalAlertsCount > 0"
            ></span>
          </button>

          <div *ngIf="showAlertsDropdown" class="alerts-dropdown card">
            <div class="alerts-dropdown-header">
              <span class="alerts-dropdown-title">{{ 'topbar.activeAlerts' | t }}</span>
              <a routerLink="/alerts" (click)="showAlertsDropdown = false" class="alerts-view-all">{{ 'common.viewAll' | t }}</a>
            </div>
            <div class="alerts-dropdown-body">
              <div *ngIf="recentAlerts.length === 0" class="alerts-empty">
                {{ 'topbar.noActiveAlerts' | t }}
              </div>
              <a 
                *ngFor="let alert of recentAlerts"
                [routerLink]="alert.type === 'DriverLicense' ? '/clients' : '/alerts'"
                (click)="showAlertsDropdown = false"
                class="alert-item"
              >
                <i class="pi pi-exclamation-triangle alert-icon"
                  [class.text-danger]="alert.severity === 'Critical'"
                  [class.text-warning]="alert.severity === 'Warning'"
                  [class.text-accent]="alert.severity === 'Info'"
                ></i>
                <div class="alert-content">
                  <div class="alert-target">{{ alert.target }}</div>
                  <div class="alert-message">{{ alert.message }}</div>
                </div>
              </a>
            </div>
          </div>
        </div>

        <div class="topbar-divider"></div>

        <!-- User -->
        <div class="topbar-user">
          <div class="topbar-user-info">
            <span class="topbar-user-name">{{ adminName }}</span>
            <span class="topbar-user-role">{{ 'common.admin' | t }}</span>
          </div>
          <a routerLink="/settings" class="user-avatar topbar-avatar">
            {{ adminName.charAt(0).toUpperCase() }}
          </a>
        </div>
      </div>
    </header>

    <!-- Page Content -->
    <main class="page-content">
      <router-outlet></router-outlet>
    </main>
  </div>
</div>

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\alerts\alerts.component.html
```html
<div class="page">
  <!-- Page Header -->
  <div class="page-header page-header-dark">
    <div>
      <h2 class="page-title">{{ 'alerts.pageTitle' | t }}</h2>
      <p class="page-desc">{{ 'alerts.pageDesc' | t }}</p>
    </div>
    <button (click)="loadAlerts()" class="btn-primary">
      <i class="pi pi-refresh"></i> {{ 'common.refresh' | t }}
    </button>
  </div>

  <!-- KPI Badges Row -->
  <div class="kpi-grid">
    <div (click)="selectedSeverity = 'ALL'; applyFilters()" 
         [class.active]="selectedSeverity === 'ALL'"
         class="card kpi-card"
    >
      <span class="kpi-label">{{ 'alerts.totalAlerts' | t }}</span>
      <div class="kpi-number-row">
        <span class="kpi-number">{{ alerts.length }}</span>
      </div>
    </div>
    
    <div (click)="selectedSeverity = 'CRITICAL'; applyFilters()" 
         [class.active]="selectedSeverity === 'CRITICAL'"
         class="card kpi-card critical"
    >
      <span class="kpi-label kpi-label-danger">{{ 'alerts.critical' | t }}</span>
      <div class="kpi-number-row">
        <span class="kpi-number kpi-number-danger">{{ criticalCount }}</span>
        <span *ngIf="criticalCount > 0" class="kpi-pulse kpi-pulse-danger"></span>
      </div>
    </div>

    <div (click)="selectedSeverity = 'WARNING'; applyFilters()" 
         [class.active]="selectedSeverity === 'WARNING'"
         class="card kpi-card warning"
    >
      <span class="kpi-label kpi-label-warning">{{ 'alerts.warning' | t }}</span>
      <div class="kpi-number-row">
        <span class="kpi-number kpi-number-warning">{{ warningCount }}</span>
      </div>
    </div>

    <div (click)="selectedSeverity = 'INFO'; applyFilters()" 
         [class.active]="selectedSeverity === 'INFO'"
         class="card kpi-card info"
    >
      <span class="kpi-label kpi-label-info">{{ 'alerts.info' | t }}</span>
      <div class="kpi-number-row">
        <span class="kpi-number kpi-number-info">{{ infoCount }}</span>
      </div>
    </div>
  </div>

  <!-- Filters Panel -->
  <div class="card filter-bar">
    <div class="filter-left">
      <!-- Search Input -->
      <div class="filter-search">
        <i class="pi pi-search filter-search-icon"></i>
        <input type="text" [(ngModel)]="searchTerm" (ngModelChange)="applyFilters()" 
               [placeholder]="'alerts.searchPlaceholder' | t" 
               class="form-input" />
      </div>

      <!-- Type Dropdown Filter -->
      <div class="filter-select-group">
        <span class="filter-module-label">{{ 'alerts.module' | t }}</span>
        <select [(ngModel)]="selectedType" (ngModelChange)="applyFilters()" class="form-input filter-select">
          <option value="ALL">{{ 'alerts.allModules' | t }}</option>
          <option value="INSURANCE">{{ 'alerts.moduleInsurance' | t }}</option>
          <option value="INSPECTION">{{ 'alerts.moduleInspection' | t }}</option>
          <option value="MAINTENANCE">{{ 'alerts.moduleMaintenance' | t }}</option>
          <option value="ODOMETERINACTIVITY">{{ 'alerts.moduleOdometer' | t }}</option>
          <option value="DRIVERLICENSE">{{ 'alerts.moduleLicense' | t }}</option>
          <option value="CONSUMABLE">{{ 'alerts.moduleConsumable' | t }}</option>
        </select>
      </div>
    </div>

    <!-- Count indicator -->
    <div class="filter-count">
      <strong>{{ filteredAlerts.length }}</strong> {{ 'alerts.alertsShown' | t }}
    </div>
  </div>

  <!-- Alerts list -->
  <div class="card table-card">
    <div class="table-scroll" *ngIf="displayedAlerts.length > 0">
      <table class="modern-table">
        <thead>
          <tr>
            <th class="col-center">{{ 'alerts.severity' | t }}</th>
            <th>{{ 'alerts.typeLabel' | t }}</th>
            <th>{{ 'alerts.concerns' | t }}</th>
            <th>{{ 'common.description' | t }}</th>
            <th>{{ 'alerts.deadline' | t }}</th>
            <th>{{ 'common.date' | t }}</th>
          </tr>
        </thead>
        <tbody>
          <tr *ngFor="let alert of displayedAlerts">
            <td class="col-center">
              <span
                class="badge"
                [class.badge-danger]="alert.severity === 'Critical'"
                [class.badge-warning]="alert.severity === 'Warning'"
                [class.badge-info]="alert.severity === 'Info'"
              >
                {{ alert.severity === 'Critical' ? ('alerts.severityCritical' | t) : alert.severity === 'Warning' ? ('alerts.severityWarning' | t) : ('alerts.severityInfo' | t) }}
              </span>
            </td>
            <td>
              <div
                class="alert-type-badge"
                [class.bg-type-red]="alert.type === 'Insurance' || alert.type === 'TechnicalInspection'"
                [class.bg-type-blue]="alert.type === 'OdometerInactivity'"
                [class.bg-type-amber]="alert.type === 'Consumable' || alert.type === 'Maintenance'"
                [class.bg-type-emerald]="alert.type === 'DriverLicense'"
              >
                <i class="pi"
                  [class.pi-shield]="alert.type === 'Insurance'"
                  [class.pi-file-pdf]="alert.type === 'TechnicalInspection'"
                  [class.pi-history]="alert.type === 'OdometerInactivity'"
                  [class.pi-cog]="alert.type === 'Consumable' || alert.type === 'Maintenance'"
                  [class.pi-user]="alert.type === 'DriverLicense'"
                ></i>
                <span>
                  {{ alert.type === 'OdometerInactivity' ? ('alerts.typeInactivityKm' | t) :
                     alert.type === 'DriverLicense' ? ('alerts.typeLicense' | t) :
                     alert.type === 'TechnicalInspection' ? ('alerts.typeInspection' | t) :
                     alert.type === 'Insurance' ? ('alerts.typeInsurance' | t) :
                     alert.type === 'Consumable' ? ('alerts.typeConsumable' | t) : alert.type }}
                </span>
              </div>
            </td>
            <td>
              <a *ngIf="alert.vehicleId" [routerLink]="['/vehicles']" class="table-link">
                <i class="pi pi-car"></i>{{ alert.target }}
              </a>
              <a *ngIf="alert.clientId" [routerLink]="['/clients']" class="table-link">
                <i class="pi pi-user"></i>{{ alert.target }}
              </a>
              <span *ngIf="!alert.vehicleId && !alert.clientId" class="cell-primary">{{ alert.target }}</span>
            </td>
            <td><span class="cell-muted">{{ i18n.translateAlertMessage(alert.message) }}</span></td>
            <td><span class="cell-primary">{{ i18n.translateAlertTime(alert.daysOrKmLeftText) }}</span></td>
            <td><span class="cell-muted">{{ alert.dateConcerned | date:'dd/MM/yyyy' }}</span></td>
          </tr>
        </tbody>
      </table>
    </div>

    <div *ngIf="filteredAlerts.length === 0" class="table-empty table-empty-success">
      <i class="pi pi-check-circle"></i>
      <span class="table-empty-title">{{ 'alerts.noAlerts' | t }}</span>
      <p class="table-empty-desc">{{ 'alerts.noAlertsDesc' | t }}</p>
    </div>

    <div *ngIf="filteredAlerts.length > 0" class="table-footer">
      <span class="table-footer-info">
        <strong>{{ filteredAlerts.length }}</strong> {{ 'alerts.alertsCount' | t }}
        <span *ngIf="filteredAlerts.length > pageSize"> â€” {{ 'common.page' | t }} {{ page }} / {{ totalPages }}</span>
      </span>
      <div *ngIf="filteredAlerts.length > pageSize" class="pagination">
        <button [disabled]="page === 1" (click)="onPageChange(page - 1)" class="page-btn"><i class="pi pi-chevron-left"></i></button>
        <button *ngFor="let p of pages" (click)="onPageChange(p)" [class.active]="p === page" class="page-btn">{{ p }}</button>
        <button [disabled]="page >= totalPages" (click)="onPageChange(page + 1)" class="page-btn"><i class="pi pi-chevron-right"></i></button>
      </div>
    </div>
  </div>
</div>

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\clients\clients.component.html
```html
<div class="page">
  <!-- Page Header -->
  <div class="page-header">
    <div>
      <h2 class="page-title">{{ 'clients.pageTitle' | t }}</h2>
      <p class="page-desc">{{ 'clients.pageDesc' | t }}</p>
    </div>
    <button (click)="openAddDialog()" class="btn-primary">
      <i class="pi pi-plus"></i>
      <span>{{ 'clients.addClient' | t }}</span>
    </button>
  </div>

  <!-- Search Card -->
  <div class="card filter-bar">
    <div class="filter-search">
      <i class="pi pi-search filter-search-icon"></i>
      <input 
        type="text" 
        [(ngModel)]="searchQuery" 
        (ngModelChange)="onSearchChange()"
        [placeholder]="'clients.searchPlaceholder' | t"
        class="form-input"
      />
    </div>
  </div>

  <!-- Client list -->
  <div class="card table-card">
    <div class="table-scroll" *ngIf="clients.length > 0">
      <table class="modern-table">
        <thead>
          <tr>
            <th>{{ 'common.client' | t }}</th>
            <th>{{ 'clients.identity' | t }}</th>
            <th>{{ 'clients.contact' | t }}</th>
            <th class="col-center">{{ 'clients.license' | t }}</th>
            <th>{{ 'clients.licenseValidity' | t }}</th>
            <th class="col-center">{{ 'common.actions' | t }}</th>
          </tr>
        </thead>
        <tbody>
          <tr *ngFor="let client of clients">
            <td>
              <div class="cell-stack">
                <span class="cell-stack-title">{{ client.fullName }}</span>
                <span class="cell-stack-sub" *ngIf="client.email">{{ client.email }}</span>
              </div>
            </td>
            <td><span class="cell-mono cell-muted">{{ client.nationalId }}</span></td>
            <td><span class="cell-muted">{{ client.phone }}</span></td>
            <td class="col-center"><span class="badge badge-accent">{{ client.licenseCategory }}</span></td>
            <td>
              <span [class.text-danger]="isLicenseExpired(client.licenseExpiryDate)" class="cell-primary">
                {{ client.licenseExpiryDate | date:'dd/MM/yyyy' }}
              </span>
              <span *ngIf="isLicenseExpired(client.licenseExpiryDate)" class="badge badge-danger" style="margin-left: 6px;">{{ 'clients.expired' | t }}</span>
            </td>
            <td>
              <div class="cell-actions">
                <button (click)="openDetails(client)" [title]="'clients.history' | t" class="btn-icon"><i class="pi pi-history"></i></button>
                <button (click)="openEditDialog(client)" [title]="'common.edit' | t" class="btn-icon"><i class="pi pi-pencil"></i></button>
                <button (click)="deleteClient(client.id)" [title]="'common.delete' | t" class="btn-icon danger"><i class="pi pi-trash"></i></button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <div *ngIf="clients.length === 0" class="table-empty">
      <i class="pi pi-users"></i>
      <span class="table-empty-title">{{ 'clients.noClientFound' | t }}</span>
      <p class="table-empty-desc">{{ 'clients.noClientDesc' | t }}</p>
    </div>

    <div *ngIf="clients.length > 0" class="table-footer">
      <span class="table-footer-info"><strong>{{ totalCount }}</strong> {{ 'clients.totalClients' | t }}</span>
      <div *ngIf="totalCount > pageSize" class="pagination">
        <button [disabled]="page === 1" (click)="onPageChange(page - 1)" class="page-btn"><i class="pi pi-chevron-left"></i></button>
        <button *ngFor="let p of pages" (click)="onPageChange(p)" [class.active]="p === page" class="page-btn">{{ p }}</button>
        <button [disabled]="page * pageSize >= totalCount" (click)="onPageChange(page + 1)" class="page-btn"><i class="pi pi-chevron-right"></i></button>
      </div>
    </div>
  </div>

  <!-- ADD / EDIT CLIENT DIALOG -->
  <p-dialog 
    [(visible)]="showCrudDialog" 
    [style]="{width: '500px'}" 
    [modal]="true" 
    [header]="isEditMode ? ('clients.editClient' | t) : ('clients.newClient' | t)"
    [draggable]="false"
    [resizable]="false"
  >
    <form (ngSubmit)="onSubmitClient()" class="form-grid">
      <div class="form-full">
        <label class="form-label required">{{ 'clients.fullName' | t }}</label>
        <input type="text" [(ngModel)]="clientForm.fullName" name="fullName" required class="form-input" placeholder="Jean Dupont"/>
      </div>
      <div>
        <label class="form-label required">{{ 'clients.nationalId' | t }}</label>
        <input type="text" [(ngModel)]="clientForm.nationalId" name="nationalId" required class="form-input" placeholder="NÂ° identitÃ© unique"/>
      </div>
      <div>
        <label class="form-label required">{{ 'clients.dateOfBirth' | t }}</label>
        <p-datepicker [(ngModel)]="clientForm.dateOfBirth" name="dateOfBirth" dateFormat="yy-mm-dd" [showIcon]="true" styleClass="w-full"></p-datepicker>
      </div>
      <div>
        <label class="form-label required">{{ 'clients.licenseNumber' | t }}</label>
        <input type="text" [(ngModel)]="clientForm.licenseNumber" name="licenseNumber" required class="form-input" placeholder="Permis NÂ°"/>
      </div>
      <div>
        <label class="form-label required">{{ 'clients.licenseCategory' | t }}</label>
        <select [(ngModel)]="clientForm.licenseCategory" name="licenseCategory" required class="form-input">
          <option *ngFor="let cat of licenseCategories" [value]="cat">{{ 'clients.category' | t }} {{ cat }}</option>
        </select>
      </div>
      <div>
        <label class="form-label required">{{ 'clients.licenseIssueDate' | t }}</label>
        <p-datepicker [(ngModel)]="clientForm.licenseIssueDate" name="licenseIssueDate" dateFormat="yy-mm-dd" [showIcon]="true" styleClass="w-full"></p-datepicker>
      </div>
      <div>
        <label class="form-label required">{{ 'clients.licenseExpiryDate' | t }}</label>
        <p-datepicker [(ngModel)]="clientForm.licenseExpiryDate" name="licenseExpiryDate" dateFormat="yy-mm-dd" [showIcon]="true" styleClass="w-full"></p-datepicker>
      </div>
      <div>
        <label class="form-label required">{{ 'clients.phone' | t }}</label>
        <input type="text" [(ngModel)]="clientForm.phone" name="phone" required class="form-input" placeholder="+33 6 12 34 56 78"/>
      </div>
      <div>
        <label class="form-label">{{ 'clients.email' | t }}</label>
        <input type="email" [(ngModel)]="clientForm.email" name="email" class="form-input" placeholder="jean.dupont@mail.com"/>
      </div>
      <div class="form-full">
        <label class="form-label">{{ 'clients.address' | t }}</label>
        <input type="text" [(ngModel)]="clientForm.address" name="address" class="form-input" placeholder="Adresse de domicile"/>
      </div>
      <div class="form-full">
        <label class="form-label">{{ 'clients.notesRemarks' | t }}</label>
        <textarea [(ngModel)]="clientForm.notes" name="notes" rows="3" class="form-input" placeholder="Remarques Ã©ventuelles sur la conduite, cautions, etc."></textarea>
      </div>

      <div class="form-full form-actions">
        <button type="button" (click)="showCrudDialog = false" class="btn-secondary">{{ 'common.cancel' | t }}</button>
        <button type="submit" class="btn-primary">{{ 'common.save' | t }}</button>
      </div>
    </form>
  </p-dialog>

  <!-- CLIENT HISTORY & DETAILS DIALOG -->
  <p-dialog 
    [(visible)]="showDetailsDialog" 
    [style]="{width: '650px'}" 
    [modal]="true" 
    [header]="selectedClient ? ('clients.clientFile' | t) + ': ' + selectedClient.fullName : ''"
    [draggable]="false"
    [resizable]="false"
  >
    <div *ngIf="selectedClient">
      <!-- General Specs -->
      <div class="specs-grid">
        <div class="spec-item">
          <span class="spec-label">{{ 'clients.nationalIdLabel' | t }}</span>
          <span class="spec-value">{{ selectedClient.nationalId }}</span>
        </div>
        <div class="spec-item">
          <span class="spec-label">{{ 'clients.phoneLabel' | t }}</span>
          <span class="spec-value">{{ selectedClient.phone }}</span>
        </div>
        <div class="spec-item">
          <span class="spec-label">{{ 'clients.licenseLabel' | t }}</span>
          <span class="spec-value">{{ 'clients.category' | t }} {{ selectedClient.licenseCategory }} - NÂ° {{ selectedClient.licenseNumber }}</span>
        </div>
        <div class="spec-item">
          <span class="spec-label">{{ 'clients.expirationLabel' | t }}</span>
          <span class="spec-value">
            <span [class.text-danger]="isLicenseExpired(selectedClient.licenseExpiryDate)">
              {{ selectedClient.licenseExpiryDate | date:'yyyy-MM-dd' }}
            </span>
            <span *ngIf="isLicenseExpired(selectedClient.licenseExpiryDate)" class="badge badge-danger" style="margin-left: 4px;">{{ 'clients.expired' | t }}</span>
          </span>
        </div>
      </div>

      <!-- Notes -->
      <div *ngIf="selectedClient.notes" class="notes-box">
        <div class="notes-box-title">{{ 'clients.adminNotes' | t }}</div>
        <div>{{ selectedClient.notes }}</div>
      </div>

      <!-- Rental History -->
      <div>
        <h4 class="section-title">{{ 'clients.rentalHistory' | t }}</h4>
        <div class="table-scroll">
          <table class="modern-table">
            <thead>
              <tr>
                <th>{{ 'clients.contractNo' | t }}</th>
                <th>{{ 'clients.vehicleLabel' | t }}</th>
                <th>{{ 'clients.registration' | t }}</th>
                <th>{{ 'clients.period' | t }}</th>
                <th>{{ 'clients.amountLabel' | t }}</th>
                <th>{{ 'common.status' | t }}</th>
              </tr>
            </thead>
            <tbody>
              <tr *ngFor="let item of rentalHistory">
                <td style="font-weight: 600; color: var(--color-accent);">{{ item.contractNumber }}</td>
                <td style="font-weight: 600;">{{ item.vehicleBrand }} {{ item.vehicleModel }}</td>
                <td style="color: var(--color-text-secondary);">{{ item.vehicleMatricule }}</td>
                <td style="color: var(--color-text-secondary); font-size: 12px;">{{ item.startDate | date:'yyyy-MM-dd' }} {{ 'common.to' | t | lowercase }} {{ (item.actualReturnDate || item.expectedReturnDate) | date:'yyyy-MM-dd' }}</td>
                <td style="font-weight: 600;">{{ item.finalAmountDue | appCurrency }}</td>
                <td>
                  <span 
                    [class.badge-success]="item.contractStatus === 'Completed'"
                    [class.badge-accent]="item.contractStatus === 'Active'"
                    [class.badge-info]="item.contractStatus === 'Draft'"
                    [class.badge-danger]="item.contractStatus === 'Cancelled'"
                    class="badge"
                  >
                    {{ 'statuses.' + item.contractStatus.toLowerCase() | t }}
                  </span>
                </td>
              </tr>
              <tr *ngIf="rentalHistory.length === 0">
                <td colspan="6" style="text-align: center; padding: 20px; color: var(--color-text-muted);">
                  {{ 'clients.noRentalHistory' | t }}
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </p-dialog>
</div>

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\consumables\consumables.component.html
```html
<div class="page">
  <!-- Page Header -->
  <div class="page-header">
    <div>
      <h2 class="page-title">{{ 'consumables.pageTitle' | t }}</h2>
      <p class="page-desc">{{ 'consumables.pageDesc' | t }}</p>
    </div>
    <div class="header-actions">
      <button (click)="showConfigDialog = true" class="btn-secondary">
        <i class="pi pi-sliders-h"></i>
        <span>{{ 'consumables.configureIntervals' | t }}</span>
      </button>
      <button (click)="openLogDialog()" class="btn-primary">
        <i class="pi pi-plus"></i>
        <span>{{ 'consumables.registerReplacement' | t }}</span>
      </button>
    </div>
  </div>

  <!-- Vehicle selector -->
  <div class="card selector-bar">
    <div class="selector-left">
      <label class="form-label" style="margin: 0; text-transform: none; font-size: 13px; font-weight: 600;">{{ 'consumables.selectVehicle' | t }}</label>
      <select 
        [(ngModel)]="selectedVehicleId" 
        (change)="onVehicleSelect()"
        class="form-input"
        style="width: 250px;"
      >
        <option [ngValue]="null" disabled>{{ 'consumables.chooseVehicle' | t }}</option>
        <option *ngFor="let v of vehicles" [value]="v.id">{{ v.brand }} {{ v.model }} ({{ v.matricule }})</option>
      </select>
    </div>

    <div *ngIf="selectedVehicleId" class="odometer-badge">
      {{ 'consumables.currentOdometer' | t }} <span class="odometer-value">{{ currentVehicleOdometer }} km</span>
    </div>
  </div>

  <!-- Consumable Status Grid Cards -->
  <div *ngIf="consumableStatusReport.length === 0" class="card empty-state">
    <i class="pi pi-cog empty-icon" style="font-size: 32px; display: block; margin-bottom: 8px;"></i>
    <p>{{ 'consumables.selectVehiclePrompt' | t }}</p>
  </div>

  <div *ngIf="consumableStatusReport.length > 0" class="consumables-grid">
    <div 
      *ngFor="let item of consumableStatusReport" 
      class="card consumable-card"
      [class.due]="item.status === 'Due'"
      [class.warning]="item.status === 'Warning'"
    >
      <div class="card-details">
        <!-- Card Header -->
        <div class="card-header">
          <div>
            <h4 class="card-title">{{ i18n.translateConsumableType(item.consumableType) }}</h4>
            <span class="card-subtitle">
              {{ 'consumables.intervalLabel' | t }}: {{ item.intervalKm > 0 ? item.intervalKm + ' km' : '' }} 
              {{ item.intervalMonths > 0 ? (item.intervalKm > 0 ? ' / ' : '') + item.intervalMonths + ' m' : '' }}
            </span>
          </div>

          <span 
            [class.badge-success]="item.status === 'OK'"
            [class.badge-warning]="item.status === 'Warning'"
            [class.badge-danger]="item.status === 'Due'"
            class="badge"
          >
            {{ item.status === 'OK' ? ('consumables.statusOk' | t) : item.status === 'Warning' ? ('consumables.statusWarning' | t) : ('consumables.statusDue' | t) }}
          </span>
        </div>

        <!-- Detail Usage Bars -->
        <div class="info-row">
          <span>{{ 'consumables.lastReplacement' | t }}</span>
          <span style="font-weight: 600; color: var(--color-text);">
            {{ item.lastReplacementDate ? (item.lastReplacementDate | date:'yyyy-MM-dd') : ('consumables.atOrder' | t) }} 
            ({{ item.lastReplacementKm || 0 }} km)
          </span>
        </div>

        <!-- Usage Meter -->
        <div *ngIf="item.intervalKm > 0" class="progress-group">
          <div class="info-row" style="font-size: 11px; margin-bottom: 2px;">
            <span>{{ 'consumables.kmTraveled' | t }}</span>
            <span style="font-weight: 600;">{{ item.kmSinceReplacement }} / {{ item.intervalKm }} km</span>
          </div>
          <!-- Progress Bar -->
          <div class="progress-container">
            <div 
              [style.width.%]="(item.kmSinceReplacement / item.intervalKm) * 100"
              [class.progress-success]="item.status === 'OK'"
              [class.progress-warning]="item.status === 'Warning'"
              [class.progress-danger]="item.status === 'Due'"
              class="progress-bar"
            ></div>
          </div>
        </div>

        <!-- Months usage if applicable -->
        <div *ngIf="item.intervalMonths > 0" class="info-row" style="margin-top: 4px;">
          <span>{{ 'consumables.timeElapsed' | t }}</span>
          <span style="font-weight: 600;">{{ item.monthsSinceReplacement }} m / {{ item.intervalMonths }} m</span>
        </div>

        <!-- Log Info Details if any -->
        <div *ngIf="item.details" class="extra-specs-box">
          <div *ngIf="item.details.viscosity" class="spec-line">
            <span>{{ 'consumables.viscosityOil' | t }}</span>
            <span style="font-weight: 700;">{{ item.details.oilType }} {{ item.details.viscosity }}</span>
          </div>
          <div *ngIf="item.details.brand" class="spec-line">
            <span>{{ 'consumables.brandSize' | t }}</span>
            <span style="font-weight: 700;">{{ item.details.brand }} {{ item.details.size }}</span>
          </div>
          <div *ngIf="item.details.axle" class="spec-line">
            <span>{{ 'consumables.axle' | t }}</span>
            <span style="font-weight: 700;">{{ item.details.axle }}</span>
          </div>
          <div *ngIf="item.details.notes" class="spec-line-notes">
            "{{ item.details.notes }}"
          </div>
        </div>
      </div>

      <!-- Quick Logger action -->
      <div class="card-actions">
        <button 
          (click)="openLogDialog(item.consumableType)" 
          class="btn-link"
        >
          <i class="pi pi-check"></i> {{ 'consumables.registerReplacement' | t }}
        </button>
      </div>
    </div>
  </div>

  <!-- REPLACEMENT LOG MODAL DIALOG -->
  <p-dialog 
    [(visible)]="showLogDialog" 
    [style]="{width: '450px'}" 
    [modal]="true" 
    [header]="'consumables.logReplacement' | t"
    [draggable]="false"
    [resizable]="false"
  >
    <form (ngSubmit)="onSubmitLog()" class="form-grid">
      <div class="form-full">
        <label class="form-label required">{{ 'consumables.consumableType' | t }}</label>
        <select [(ngModel)]="logForm.consumableType" name="consumableType" required class="form-input">
          <option *ngFor="let type of consumableTypes" [value]="type">{{ i18n.translateConsumableType(type) }}</option>
        </select>
      </div>

      <div>
        <label class="form-label required">{{ 'consumables.replacementDate' | t }}</label>
        <p-datepicker [(ngModel)]="logForm.replacementDate" name="replacementDate" dateFormat="yy-mm-dd" [showIcon]="true" styleClass="w-full"></p-datepicker>
      </div>
      <div>
        <label class="form-label required">{{ 'consumables.replacementKm' | t }}</label>
        <input type="number" [(ngModel)]="logForm.replacementKm" name="replacementKm" required class="form-input"/>
      </div>

      <!-- Conditional fields for Oil change -->
      <div *ngIf="logForm.consumableType === 'OilChange'" class="conditional-box">
        <div>
          <label class="form-label">{{ 'consumables.oilType' | t }}</label>
          <input type="text" [(ngModel)]="logForm.oilType" name="oilType" class="form-input" placeholder="e.g. SynthÃ©tique"/>
        </div>
        <div>
          <label class="form-label">{{ 'consumables.viscosity' | t }}</label>
          <input type="text" [(ngModel)]="logForm.viscosity" name="viscosity" class="form-input" placeholder="e.g. 5W-40"/>
        </div>
      </div>

      <!-- Conditional fields for Brakes or Tires -->
      <div *ngIf="logForm.consumableType.includes('Brakes') || logForm.consumableType.includes('Tires')" class="conditional-box">
        <div>
          <label class="form-label">{{ 'consumables.axlePosition' | t }}</label>
          <select [(ngModel)]="logForm.axle" name="axle" class="form-input">
            <option value="Front">{{ 'consumables.frontAxle' | t }}</option>
            <option value="Rear">{{ 'consumables.rearAxle' | t }}</option>
          </select>
        </div>
        <div>
          <label class="form-label">{{ 'consumables.brandLabel' | t }}</label>
          <input type="text" [(ngModel)]="logForm.brand" name="brand" class="form-input" placeholder="e.g. Michelin / Bosch"/>
        </div>
        <div *ngIf="logForm.consumableType.includes('Tires')" class="form-full" style="display: grid; grid-template-columns: 1fr 1fr; gap: 12px; margin-top: 4px;">
          <div>
            <label class="form-label">{{ 'consumables.dimensions' | t }}</label>
            <input type="text" [(ngModel)]="logForm.size" name="size" class="form-input" placeholder="e.g. 205/55 R16"/>
          </div>
          <div>
            <label class="form-label">{{ 'consumables.typeDetail' | t }}</label>
            <input type="text" [(ngModel)]="logForm.typeDetail" name="typeDetail" class="form-input" placeholder="Ã‰tÃ© / Hiver / 4S"/>
          </div>
        </div>
      </div>

      <!-- Conditional fields for Battery -->
      <div *ngIf="logForm.consumableType === 'Battery'" class="conditional-box">
        <div>
          <label class="form-label">{{ 'consumables.batteryBrand' | t }}</label>
          <input type="text" [(ngModel)]="logForm.brand" name="brand" class="form-input" placeholder="e.g. Varta / Bosch"/>
        </div>
        <div>
          <label class="form-label">{{ 'consumables.batteryCapacity' | t }}</label>
          <input type="text" [(ngModel)]="logForm.typeDetail" name="typeDetail" class="form-input" placeholder="e.g. 70Ah / 12V"/>
        </div>
      </div>

      <!-- General details for user custom types -->
      <div *ngIf="!['OilChange', 'AirFilter', 'OilFilter', 'FuelFilter', 'CabinFilter', 'FrontBrakes', 'RearBrakes', 'FrontTires', 'RearTires', 'Battery'].includes(logForm.consumableType)" class="conditional-box">
        <div>
          <label class="form-label">{{ 'consumables.manufacturer' | t }}</label>
          <input type="text" [(ngModel)]="logForm.brand" name="brand" class="form-input"/>
        </div>
        <div>
          <label class="form-label">{{ 'consumables.technicalDetails' | t }}</label>
          <input type="text" [(ngModel)]="logForm.typeDetail" name="typeDetail" class="form-input"/>
        </div>
      </div>

      <div class="form-full">
        <label class="form-label">{{ 'consumables.notesRemarks' | t }}</label>
        <textarea [(ngModel)]="logForm.notes" name="notes" rows="2" class="form-input" placeholder="Notez d'Ã©ventuels dÃ©tails sur le remplacement..."></textarea>
      </div>

      <div class="form-full form-actions">
        <button type="button" (click)="showLogDialog = false" class="btn-secondary">{{ 'common.cancel' | t }}</button>
        <button type="submit" class="btn-primary">{{ 'consumables.registerReplacement' | t }}</button>
      </div>
    </form>
  </p-dialog>

  <!-- CONFIGURATIONS LIST MODAL DIALOG -->
  <p-dialog 
    [(visible)]="showConfigDialog" 
    [style]="{width: '500px'}" 
    [modal]="true" 
    [header]="'consumables.intervalsConfig' | t"
    [draggable]="false"
    [resizable]="false"
  >
    <div style="display: flex; flex-direction: column; gap: 12px;">
      <div style="display: flex; justify-content: space-between; align-items: center; border-bottom: 1px solid var(--color-border-light); padding-bottom: 8px;">
        <span style="color: var(--color-text-secondary); font-size: 12px;">{{ 'consumables.intervalsDesc' | t }}</span>
        <button (click)="openConfigDialog()" class="btn-primary" style="padding: 4px 10px; font-size: 11px;">
          <i class="pi pi-plus"></i> {{ 'consumables.createRule' | t }}
        </button>
      </div>

      <div class="config-table-container">
        <table class="data-table">
          <thead>
            <tr>
              <th>{{ 'vehicles.consumable' | t }}</th>
              <th>{{ 'consumables.limitKm' | t }}</th>
              <th>{{ 'consumables.limitMonths' | t }}</th>
              <th style="text-align: right; width: 60px;">{{ 'common.actions' | t }}</th>
            </tr>
          </thead>
          <tbody>
            <tr *ngFor="let conf of configs">
              <td style="font-weight: 600;">{{ i18n.translateConsumableType(conf.consumableType) }}</td>
              <td style="font-weight: 500;">{{ conf.intervalKm > 0 ? conf.intervalKm + ' km' : ('common.none' | t) }}</td>
              <td style="font-weight: 500;">{{ conf.intervalMonths > 0 ? conf.intervalMonths + ' ' + ('common.months' | t) : ('common.none' | t) }}</td>
              <td style="text-align: right;">
                <button (click)="openConfigDialog(conf)" class="btn-icon" style="width: 24px; height: 24px;">
                  <i class="pi pi-pencil" style="font-size: 10px;"></i>
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Config dialog editing form (inline dialog box nested or standalone) -->
      <div *ngIf="selectedConfig" class="inline-config-form">
        <h5 class="inline-config-title">{{ 'consumables.configureRule' | t }} {{ selectedConfig.consumableType ? i18n.translateConsumableType(selectedConfig.consumableType) : ('consumables.newRule' | t) }}</h5>
        <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 12px;">
          <div *ngIf="!selectedConfig.id">
            <label class="form-label">{{ 'consumables.consumableType' | t }}</label>
            <input type="text" [(ngModel)]="selectedConfig.consumableType" class="form-input"/>
          </div>
          <div>
            <label class="form-label">{{ 'consumables.intervalKm' | t }}</label>
            <input type="number" [(ngModel)]="selectedConfig.intervalKm" class="form-input" placeholder="e.g. 10000"/>
          </div>
          <div>
            <label class="form-label">{{ 'consumables.intervalMonths' | t }}</label>
            <input type="number" [(ngModel)]="selectedConfig.intervalMonths" class="form-input" placeholder="e.g. 12"/>
          </div>
        </div>
        <div style="display: flex; justify-content: flex-end; gap: 8px; margin-top: 12px; border-top: 1px solid var(--color-border-light); padding-top: 10px;">
          <button (click)="selectedConfig = null" class="btn-secondary" style="padding: 4px 10px; font-size: 11px;">{{ 'common.cancel' | t }}</button>
          <button (click)="onSubmitConfig()" class="btn-primary" style="padding: 4px 10px; font-size: 11px;">{{ 'consumables.validateRule' | t }}</button>
        </div>
      </div>
    </div>
    
    <div style="display: flex; justify-content: flex-end; margin-top: 16px; border-top: 1px solid var(--color-border-light); padding-top: 12px;">
      <button (click)="showConfigDialog = false" class="btn-secondary">{{ 'common.close' | t }}</button>
    </div>
  </p-dialog>
</div>

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\contracts\contracts.component.html
```html
<div class="page">
  <!-- Page Header -->
  <div class="page-header">
    <div>
      <h2 class="page-title">{{ 'contracts.pageTitle' | t }}</h2>
      <p class="page-desc">{{ 'contracts.pageDesc' | t }}</p>
    </div>
    <button (click)="openAddDialog()" class="btn-primary">
      <i class="pi pi-plus"></i>
      <span>{{ 'contracts.newContract' | t }}</span>
    </button>
  </div>

  <!-- Filters -->
  <div class="card filter-bar">
    <div class="filter-search">
      <i class="pi pi-search filter-search-icon"></i>
      <input 
        type="text" 
        [(ngModel)]="searchQuery" 
        (ngModelChange)="onFilterChange()"
        [placeholder]="'contracts.searchPlaceholder' | t"
        class="form-input"
      />
    </div>

    <!-- Status Filter -->
    <select 
      [(ngModel)]="filterStatus" 
      (change)="onFilterChange()"
      class="form-input filter-select"
    >
      <option value="">{{ 'contracts.allStatuses' | t }}</option>
      <option value="Draft">{{ 'contracts.statusDraft' | t }}</option>
      <option value="Active">{{ 'contracts.statusActive' | t }}</option>
      <option value="Completed">{{ 'contracts.statusCompleted' | t }}</option>
      <option value="Cancelled">{{ 'contracts.statusCancelled' | t }}</option>
    </select>

    <!-- Payment Filter -->
    <select 
      [(ngModel)]="filterPaymentStatus" 
      (change)="onFilterChange()"
      class="form-input filter-select"
    >
      <option value="">{{ 'contracts.allInvoices' | t }}</option>
      <option value="Unpaid">{{ 'contracts.unpaid' | t }}</option>
      <option value="PartiallyPaid">{{ 'contracts.partial' | t }}</option>
      <option value="Paid">{{ 'contracts.paid' | t }}</option>
    </select>
  </div>

  <!-- Contracts list -->
  <div class="card table-card">
    <div class="table-scroll" *ngIf="contracts.length > 0">
      <table class="modern-table">
        <thead>
          <tr>
            <th>{{ 'contracts.contract' | t }}</th>
            <th>{{ 'common.client' | t }}</th>
            <th>{{ 'common.vehicle' | t }}</th>
            <th>{{ 'contracts.period' | t }}</th>
            <th class="col-right">{{ 'common.amount' | t }}</th>
            <th>{{ 'contracts.statuses' | t }}</th>
            <th class="col-center">{{ 'common.actions' | t }}</th>
          </tr>
        </thead>
        <tbody>
          <tr *ngFor="let contract of contracts">
            <td><span class="cell-accent">{{ contract.contractNumber }}</span></td>
            <td><span class="cell-primary">{{ contract.client?.fullName }}</span></td>
            <td>
              <div class="cell-stack">
                <span class="cell-stack-title">{{ contract.vehicle?.brand }} {{ contract.vehicle?.model }}</span>
                <span class="cell-stack-sub cell-mono">{{ contract.vehicle?.matricule }}</span>
              </div>
            </td>
            <td>
              <span class="cell-muted">{{ contract.startDate | date:'dd/MM/yyyy' }}</span>
              <span class="cell-muted"> â†’ {{ (contract.actualReturnDate || contract.expectedReturnDate) | date:'dd/MM/yyyy' }}</span>
            </td>
            <td class="col-right"><span class="cell-primary">{{ contract.finalAmountDue | appCurrency }}</span></td>
            <td>
              <div class="cell-stack" style="gap: 6px;">
                <span
                  [class.badge-info]="contract.contractStatus === 'Draft'"
                  [class.badge-warning]="contract.contractStatus === 'Active'"
                  [class.badge-success]="contract.contractStatus === 'Completed'"
                  [class.badge-danger]="contract.contractStatus === 'Cancelled'"
                  class="badge"
                >
                  {{ 'statuses.' + contract.contractStatus.toLowerCase() | t }}
                </span>
                <span
                  [class.badge-danger]="contract.paymentStatus === 'Unpaid'"
                  [class.badge-warning]="contract.paymentStatus === 'PartiallyPaid'"
                  [class.badge-success]="contract.paymentStatus === 'Paid'"
                  class="badge"
                >
                  {{ 'statuses.' + (contract.paymentStatus === 'Unpaid' ? 'unpaid' : contract.paymentStatus === 'PartiallyPaid' ? 'partiallyPaid' : 'paid') | t }}
                </span>
              </div>
            </td>
            <td>
              <div class="cell-actions">
                <button
                  *ngIf="contract.contractStatus === 'Active'"
                  (click)="openReturnDialog(contract)"
                  [title]="'contracts.returnVehicle' | t"
                  class="btn-primary btn-sm-success"
                >
                  <i class="pi pi-key"></i> {{ 'contracts.returnBtn' | t }}
                </button>
                <button (click)="openInvoice(contract)" [title]="'common.print' | t" class="btn-icon"><i class="pi pi-print"></i></button>
                <button (click)="openEditDialog(contract)" [title]="'common.edit' | t" class="btn-icon"><i class="pi pi-pencil"></i></button>
                <button (click)="deleteContract(contract)" [title]="'common.delete' | t" class="btn-icon text-danger" style="color: var(--color-danger);"><i class="pi pi-trash"></i></button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <div *ngIf="contracts.length === 0" class="table-empty">
      <i class="pi pi-file"></i>
      <span class="table-empty-title">{{ 'contracts.noContractFound' | t }}</span>
      <p class="table-empty-desc">{{ 'contracts.noContractDesc' | t }}</p>
    </div>

    <div *ngIf="contracts.length > 0" class="table-footer">
      <span class="table-footer-info"><strong>{{ totalCount }}</strong> {{ 'contracts.totalContracts' | t }}</span>
      <div *ngIf="totalCount > pageSize" class="pagination">
        <button [disabled]="page === 1" (click)="onPageChange(page - 1)" class="page-btn"><i class="pi pi-chevron-left"></i></button>
        <button *ngFor="let p of pages" (click)="onPageChange(p)" [class.active]="p === page" class="page-btn">{{ p }}</button>
        <button [disabled]="page * pageSize >= totalCount" (click)="onPageChange(page + 1)" class="page-btn"><i class="pi pi-chevron-right"></i></button>
      </div>
    </div>
  </div>

  <!-- ADD / EDIT CONTRACT DIALOG -->
  <p-dialog 
    [(visible)]="showCrudDialog" 
    [style]="{width: '900px'}" 
    [modal]="true" 
    [header]="isEditMode ? ('contracts.editContract' | t) : ('contracts.newContractDialog' | t)"
    [draggable]="false"
    [resizable]="false"
  >
    <!-- License Expiry Alert -->
    <div *ngIf="licenseWarning" class="alert-warning">
      <i class="pi pi-exclamation-circle" style="font-size: 14px;"></i>
      <span>{{ licenseWarning }}</span>
    </div>

    <form (ngSubmit)="onSubmitContract()" style="display: grid; grid-template-columns: 1.2fr 1fr; gap: 24px;">
      <!-- LEFT COLUMN -->
      <div style="display: flex; flex-direction: column; gap: 16px;">
        
        <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 12px;">
          <!-- Client Selector -->
          <div>
            <label class="form-label required">{{ 'contracts.selectClient' | t }}</label>
            <select *ngIf="!isEditMode" [(ngModel)]="contractForm.clientId" name="clientId" (change)="onClientSelect()" required class="form-input">
              <option [ngValue]="null">{{ 'contracts.chooseClient' | t }}</option>
              <option *ngFor="let c of activeClients" [ngValue]="c.id">{{ c.fullName }} ({{ c.nationalId }})</option>
            </select>
            <input *ngIf="isEditMode" type="text" disabled [value]="contractForm.client?.fullName + ' (' + contractForm.client?.nationalId + ')'" class="form-input disabled-input"/>
          </div>

          <!-- Vehicle Selector -->
          <div>
            <label class="form-label required">{{ 'contracts.selectVehicle' | t }}</label>
            <select *ngIf="!isEditMode" [(ngModel)]="contractForm.vehicleId" name="vehicleId" (change)="onVehicleSelect()" required class="form-input">
              <option [ngValue]="null">{{ 'contracts.chooseVehicle' | t }}</option>
              <option *ngFor="let v of availableVehicles" [ngValue]="v.id">{{ v.brand }} {{ v.model }} - Plaque: {{ v.matricule }}</option>
            </select>
            <input *ngIf="isEditMode" type="text" disabled [value]="contractForm.vehicle?.brand + ' ' + contractForm.vehicle?.model + ' - ' + contractForm.vehicle?.matricule" class="form-input disabled-input"/>
          </div>
        </div>

        <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 12px;">
          <div>
            <label class="form-label required">{{ 'contracts.startDateTime' | t }}</label>
            <p-datepicker [(ngModel)]="contractForm.startDate" name="startDate" dateFormat="yy-mm-dd" [showTime]="true" (onSelect)="calculateTotals()" styleClass="w-full"></p-datepicker>
          </div>
          <div>
            <label class="form-label required">{{ 'contracts.expectedReturn' | t }}</label>
            <p-datepicker [(ngModel)]="contractForm.expectedReturnDate" name="expectedReturnDate" dateFormat="yy-mm-dd" [showTime]="true" (onSelect)="calculateTotals()" styleClass="w-full"></p-datepicker>
          </div>
        </div>

        <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 12px;">
          <div>
            <label class="form-label required">{{ 'contracts.contractType' | t }}</label>
            <select [(ngModel)]="contractForm.contractType" name="contractType" required class="form-input">
              <option *ngFor="let ct of contractTypes" [value]="ct">{{ ct }}</option>
            </select>
          </div>
          <div>
            <label class="form-label required">{{ 'contracts.dailyRate' | t }}</label>
            <input type="number" [(ngModel)]="contractForm.dailyRate" name="dailyRate" (change)="calculateTotals()" required class="form-input"/>
          </div>
        </div>

        <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 12px;">
          <!-- Status Selection -->
          <div>
            <label class="form-label">{{ 'contracts.initialStatus' | t }}</label>
            <select *ngIf="!isEditMode || contractForm.contractStatus === 'Draft'" [(ngModel)]="contractForm.contractStatus" name="contractStatus" class="form-input">
              <option value="Draft">{{ 'contracts.draftReservation' | t }}</option>
              <option value="Active">{{ 'contracts.activateNow' | t }}</option>
            </select>
            <input *ngIf="isEditMode && contractForm.contractStatus !== 'Draft'" type="text" disabled [value]="'statuses.' + contractForm.contractStatus.toLowerCase() | t" class="form-input disabled-input"/>
          </div>
          <div>
            <label class="form-label">{{ 'contracts.initialBilling' | t }}</label>
            <select [(ngModel)]="contractForm.paymentStatus" (change)="onPaymentStatusChange()" name="paymentStatus" class="form-input">
              <option value="Unpaid">{{ 'contracts.unpaidPending' | t }}</option>
              <option value="PartiallyPaid">{{ 'contracts.depositPaid' | t }}</option>
              <option value="Paid">{{ 'contracts.fullyPaid' | t }}</option>
            </select>
          </div>
        </div>

        <div style="margin-top: 8px;">
          <label class="form-label">{{ 'contracts.adminNotes' | t }}</label>
          <input type="text" [(ngModel)]="contractForm.notes" name="notes" class="form-input" placeholder="Notes additionnelles..."/>
        </div>
      </div>

      <!-- RIGHT COLUMN -->
      <div style="display: flex; flex-direction: column; gap: 16px; background: #f8fafc; padding: 20px; border-radius: 12px; border: 1px solid var(--color-border-light);">
        
        <!-- Summary Grid (2x2) -->
        <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 16px;">
          <div class="summary-item">
            <span class="summary-label">{{ 'contracts.calculatedDays' | t }}</span>
            <span class="summary-value">{{ contractForm.rentalDays }} {{ 'common.days' | t }}</span>
          </div>
          <div class="summary-item">
            <span class="summary-label">{{ 'contracts.rentalAmount' | t }}</span>
            <span class="summary-value">{{ contractForm.totalAmount | appCurrency }}</span>
          </div>
          <div class="summary-item">
            <span class="summary-label">{{ 'contracts.extraFees' | t }}</span>
            <input type="number" [(ngModel)]="contractForm.extrasCharges" name="extrasCharges" (change)="calculateTotals()" class="form-input" style="padding: 4px 6px; font-size: 12px;"/>
          </div>
          <div class="summary-item">
            <span class="summary-label">{{ 'contracts.discount' | t }}</span>
            <input type="number" [(ngModel)]="contractForm.discountAmount" name="discountAmount" (change)="calculateTotals()" class="form-input" style="padding: 4px 6px; font-size: 12px;"/>
          </div>
        </div>

        <!-- Payment Row -->
        <div style="border-top: 1px solid var(--color-border-light); padding-top: 16px; margin-top: 8px;">
          <div style="display: flex; flex-direction: column; gap: 12px;">
            <!-- Payment Method -->
            <div>
              <label class="form-label">{{ 'contracts.paymentMethod' | t }}</label>
              <select [(ngModel)]="contractForm.paymentMethod" name="paymentMethod" class="form-input">
                <option *ngFor="let m of paymentMethods" [value]="m">{{ m }}</option>
              </select>
            </div>
            <!-- Amount Paid & Deposit -->
            <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 12px;">
              <div>
                <label class="form-label">{{ 'contracts.amountPaid' | t }}</label>
                <input type="number" [(ngModel)]="contractForm.amountPaid" name="amountPaid" class="form-input" placeholder="0"/>
              </div>
              <div>
                <label class="form-label">{{ 'contracts.deposit' | t }}</label>
                <input type="number" [(ngModel)]="contractForm.depositAmount" name="depositAmount" class="form-input"/>
              </div>
            </div>
          </div>
        </div>
      </div>
      
      <!-- FORM ACTIONS & TOTALS -->
      <div style="grid-column: 1 / -1; margin-top: 16px; padding-top: 16px; border-top: 1px solid var(--color-border-light); display: flex; justify-content: space-between; align-items: flex-end;">
        <div style="display: flex; gap: 32px; align-items: baseline;">
          <div class="amount-due-block">
            <span class="amount-due-label">{{ 'contracts.totalNetDue' | t }}</span>
            <span class="amount-due-value">{{ contractForm.finalAmountDue | appCurrency }}</span>
          </div>
          
          <div *ngIf="contractForm.paymentStatus !== 'Paid'" class="amount-due-block">
            <span class="amount-due-label" style="color: var(--color-danger);">{{ 'contracts.remainingDue' | t }}</span>
            <span class="amount-due-value" style="font-size: 18px; color: var(--color-danger);">{{ (contractForm.finalAmountDue - contractForm.amountPaid) | appCurrency }}</span>
          </div>
        </div>
        <div style="display: flex; gap: 8px;">
          <button type="button" (click)="showCrudDialog = false" class="btn-secondary">{{ 'common.cancel' | t }}</button>
          <button type="submit" class="btn-primary" style="height: 42px; padding: 0 24px; font-size: 14px;">{{ 'contracts.saveContract' | t }}</button>
        </div>
      </div>
    </form>
  </p-dialog>

  <!-- RETURN VEHICLE DIALOG -->
  <p-dialog 
    [(visible)]="showReturnDialog" 
    [style]="{width: '450px'}" 
    [modal]="true" 
    [header]="selectedContractForReturn ? ('contracts.closeContract' | t) + ': ' + selectedContractForReturn.contractNumber : ''"
    [draggable]="false"
    [resizable]="false"
  >
    <form (ngSubmit)="submitReturn()" class="form-grid">
      <div class="form-full return-info-banner">
        <div class="return-info-row">
          <span>{{ 'contracts.departureOdometer' | t }}</span>
          <span>{{ selectedContractForReturn?.kmDeparture }} km</span>
        </div>
        <div class="return-info-row">
          <span>{{ 'contracts.expectedReturnDate' | t }}</span>
          <span>{{ selectedContractForReturn?.expectedReturnDate | date:'yyyy-MM-dd HH:mm' }}</span>
        </div>
      </div>

      <div>
        <label class="form-label required">{{ 'contracts.returnIndex' | t }}</label>
        <input type="number" [(ngModel)]="returnForm.kmReturn" name="kmReturn" required class="form-input"/>
      </div>
      <div>
        <label class="form-label required">{{ 'contracts.actualReturnDate' | t }}</label>
        <p-datepicker [(ngModel)]="returnForm.returnDate" name="returnDate" dateFormat="yy-mm-dd" [showTime]="true" styleClass="w-full"></p-datepicker>
      </div>

      <div>
        <label class="form-label">{{ 'contracts.fuelPenalty' | t }}</label>
        <input type="number" [(ngModel)]="returnForm.fuelPenalty" name="fuelPenalty" class="form-input" placeholder="0"/>
      </div>
      <div>
        <label class="form-label">{{ 'contracts.damageFees' | t }}</label>
        <input type="number" [(ngModel)]="returnForm.damageFees" name="damageFees" class="form-input" placeholder="0"/>
      </div>

      <div class="form-full maintenance-check-box">
        <input type="checkbox" [(ngModel)]="returnForm.setInMaintenance" name="setInMaintenance" id="setInMaintenance" style="cursor: pointer; width: 16px; height: 16px;"/>
        <label for="setInMaintenance" class="form-label" style="margin: 0; text-transform: none; font-weight: 600; cursor: pointer; color: var(--color-text);">{{ 'contracts.sendToMaintenance' | t }}</label>
      </div>

      <div class="form-full">
        <label class="form-label">{{ 'contracts.returnNotes' | t }}</label>
        <textarea [(ngModel)]="returnForm.returnNotes" name="returnNotes" rows="2" class="form-input" [placeholder]="'contracts.returnNotesPlaceholder' | t"></textarea>
      </div>

      <div class="form-full form-actions">
        <button type="button" (click)="showReturnDialog = false" class="btn-secondary">{{ 'common.cancel' | t }}</button>
        <button type="submit" class="btn-primary" style="background: var(--color-success);">{{ 'contracts.validateReturn' | t }}</button>
      </div>
    </form>
  </p-dialog>

  <!-- PRINT INVOICE & CONTRACT PREVIEW MODAL -->
  <p-dialog 
    [(visible)]="showPrintDialog" 
    [style]="{width: '550px'}" 
    [modal]="true" 
    [header]="'contracts.printPreview' | t"
    [draggable]="false"
    [resizable]="false"
  >
    <div id="invoice-print-area" *ngIf="printContract" class="invoice-print">
      <!-- Header -->
      <div class="invoice-header">
        <div>
          <div class="invoice-brand">
            <i class="pi pi-car text-accent"></i> {{ 'contracts.invoiceBrand' | t }}
          </div>
          <p style="font-size: 10px; color: var(--color-text-secondary); margin: 4px 0 0;">{{ 'contracts.invoiceSubtitle' | t }}</p>
        </div>
        <div class="invoice-meta">
          <h4 class="invoice-meta-title">{{ 'contracts.contractInvoice' | t }}</h4>
          <span class="invoice-meta-number">NÂ° {{ printContract.contractNumber }}</span>
          <p style="font-size: 10px; color: var(--color-text-secondary); margin: 2px 0 0;">Date: {{ printContract.startDate | date:'yyyy-MM-dd' }}</p>
        </div>
      </div>

      <!-- Address Grid -->
      <div class="invoice-addresses">
        <div>
          <div class="address-block-title">{{ 'contracts.tenant' | t }}</div>
          <div class="address-block-name">{{ printContract.client?.fullName }}</div>
          <div style="color: var(--color-text-secondary); margin-top: 4px;">{{ 'clients.licenseLabel' | t }}: {{ 'clients.category' | t }} {{ printContract.client?.licenseCategory }} - NÂ° {{ printContract.client?.licenseNumber }}</div>
          <div style="color: var(--color-text-secondary); margin-top: 2px;">TÃ©l: {{ printContract.client?.phone }}</div>
        </div>
        <div>
          <div class="address-block-title">{{ 'contracts.vehicleLabel' | t }}</div>
          <div class="address-block-name">{{ printContract.vehicle?.brand }} {{ printContract.vehicle?.model }}</div>
          <div style="color: var(--color-text-secondary); margin-top: 4px;">{{ 'contracts.registrationLabel' | t }}: {{ printContract.vehicle?.matricule }}</div>
          <div style="color: var(--color-text-secondary); margin-top: 2px;">{{ 'contracts.departureCounter' | t }}: {{ printContract.kmDeparture }} km</div>
          <div *ngIf="printContract.kmReturn" style="color: var(--color-text-secondary); margin-top: 2px;">{{ 'contracts.returnCounter' | t }}: {{ printContract.kmReturn }} km</div>
        </div>
      </div>

      <!-- Core Specs Table -->
      <table class="invoice-table">
        <thead>
          <tr>
            <th>{{ 'common.description' | t }}</th>
            <th style="text-align: right;">{{ 'contracts.dailyRateLabel' | t }}</th>
            <th style="text-align: right;">{{ 'contracts.daysLabel' | t }}</th>
            <th style="text-align: right;">{{ 'contracts.totalHT' | t }}</th>
          </tr>
        </thead>
        <tbody>
          <tr>
            <td style="font-weight: 600;">{{ 'contracts.vehicleRental' | t }} ({{ printContract.vehicle?.brand }} {{ printContract.vehicle?.model }})</td>
            <td style="text-align: right;">{{ printContract.dailyRate | appCurrency }}</td>
            <td style="text-align: right;">{{ printContract.rentalDays }} {{ 'common.days' | t }}</td>
            <td style="text-align: right; font-weight: 600;">{{ printContract.totalAmount | appCurrency }}</td>
          </tr>
          <!-- Extras -->
          <tr *ngIf="printContract.extrasCharges > 0">
            <td style="color: var(--color-text-secondary);">{{ 'contracts.extrasAccessories' | t }}</td>
            <td style="text-align: right;">-</td>
            <td style="text-align: right;">-</td>
            <td style="text-align: right; font-weight: 600;">{{ printContract.extrasCharges | appCurrency }}</td>
          </tr>
          <!-- Penalties -->
          <tr *ngIf="printContract.lateReturnFee > 0 || printContract.damageFees > 0 || printContract.fuelPenalty > 0">
            <td style="color: var(--color-danger); font-weight: 600;">{{ 'contracts.penalties' | t }}</td>
            <td style="text-align: right;">-</td>
            <td style="text-align: right;">-</td>
            <td style="text-align: right; font-weight: 600; color: var(--color-danger);">
              {{ printContract.lateReturnFee + printContract.damageFees + printContract.fuelPenalty | appCurrency }}
            </td>
          </tr>
        </tbody>
      </table>

      <!-- Totals & Signatures -->
      <div class="invoice-totals-row">
        <div>
          <div class="address-block-title">{{ 'contracts.paymentLabel' | t }}</div>
          <div style="font-weight: 600; color: var(--color-text-secondary); margin-top: 4px;">{{ 'contracts.mode' | t }}: {{ printContract.paymentMethod }}</div>
          <div style="font-weight: 600; color: var(--color-text-secondary); margin-top: 2px;">{{ 'contracts.statusLabel' | t }}: {{ 'statuses.' + (printContract.paymentStatus === 'Unpaid' ? 'unpaid' : printContract.paymentStatus === 'PartiallyPaid' ? 'partiallyPaid' : 'paid') | t }}</div>
        </div>
        <div class="totals-details">
          <div class="totals-line">
            <span style="color: var(--color-text-secondary);">{{ 'contracts.subtotal' | t }}</span>
            <span style="font-weight: 600;">{{ printContract.totalAmount + printContract.extrasCharges | appCurrency }}</span>
          </div>
          <div *ngIf="printContract.discountAmount > 0" class="totals-line" style="color: var(--color-success); font-weight: 600;">
            <span>{{ 'contracts.reduction' | t }}</span>
            <span>-{{ printContract.discountAmount | appCurrency }}</span>
          </div>
          <div class="totals-line totals-line-grand">
            <span>{{ 'contracts.netAmountDue' | t }}</span>
            <span>{{ printContract.finalAmountDue | appCurrency }}</span>
          </div>
          <div *ngIf="printContract.paymentStatus === 'PartiallyPaid'" class="totals-line" style="color: var(--color-text-secondary);">
            <span>{{ 'contracts.amountPaid' | t }}</span>
            <span>-{{ printContract.amountPaid | appCurrency }}</span>
          </div>
          <div *ngIf="printContract.paymentStatus === 'PartiallyPaid'" class="totals-line" style="color: var(--color-danger); font-weight: 700; border-top: 1px dashed var(--color-border-light); padding-top: 4px; margin-top: 4px;">
            <span>{{ 'contracts.remainingDue' | t }}</span>
            <span>{{ (printContract.finalAmountDue - printContract.amountPaid) | appCurrency }}</span>
          </div>
        </div>
      </div>

      <!-- Signatures Footer -->
      <div class="invoice-signatures">
        <div class="signature-box">
          <span style="font-weight: 700; text-transform: uppercase;">{{ 'contracts.tenantSignature' | t }}</span>
          <div class="signature-line"></div>
        </div>
        <div class="signature-box">
          <span style="font-weight: 700; text-transform: uppercase;">{{ 'contracts.agencySignature' | t }}</span>
          <div class="signature-line"></div>
        </div>
      </div>
    </div>

    <!-- Print control panel -->
    <div class="flex justify-end gap-2 mt-6 no-print" style="border-top: 1px solid var(--color-border-light); padding-top: 16px;">
      <button (click)="showPrintDialog = false" class="btn-secondary">{{ 'common.close' | t }}</button>
      <button (click)="generatePdf()" class="btn-primary" style="background: var(--color-danger); border-color: var(--color-danger);">
        <i class="pi pi-file-pdf"></i>
        <span>GÃ©nÃ©rer PDF</span>
      </button>
    </div>
  </p-dialog>
</div>

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\dashboard\dashboard.component.html
```html
<div class="page">
  <!-- Page Header -->
  <div class="page-header">
    <div>
      <h2 class="page-title">{{ 'dashboard.pageTitle' | t }}</h2>
      <p class="page-desc">{{ 'dashboard.pageDesc' | t }}</p>
    </div>
    <span class="badge badge-neutral">{{ today | date:'dd/MM/yyyy' }}</span>
  </div>

  <!-- KPIs -->
  <div class="kpi-grid">
    <div class="card kpi-card">
      <div class="kpi-content">
        <span class="kpi-label">{{ 'dashboard.fleet' | t }}</span>
        <span class="kpi-value">{{ stats.total }}</span>
        <span class="kpi-sub">{{ 'dashboard.registeredVehicles' | t }}</span>
      </div>
      <div class="kpi-icon kpi-icon-accent"><i class="pi pi-car"></i></div>
    </div>

    <div class="card kpi-card">
      <div class="kpi-content">
        <span class="kpi-label">{{ 'dashboard.activeRentals' | t }}</span>
        <span class="kpi-value">{{ stats.rented }}</span>
        <span class="kpi-sub text-success">{{ stats.available }} {{ 'dashboard.available' | t }}</span>
      </div>
      <div class="kpi-icon kpi-icon-success"><i class="pi pi-key"></i></div>
    </div>

    <div class="card kpi-card">
      <div class="kpi-content">
        <span class="kpi-label">{{ 'dashboard.revenue' | t }}</span>
        <span class="kpi-value">{{ revenue.totalRevenue | appCurrency }}</span>
        <span class="kpi-sub">{{ revenue.paidRevenue | appCurrency }} {{ 'dashboard.paid' | t }}</span>
      </div>
      <div class="kpi-icon kpi-icon-info"><i class="pi pi-money-bill"></i></div>
    </div>

    <div class="card kpi-card">
      <div class="kpi-content">
        <span class="kpi-label">{{ 'dashboard.alerts' | t }}</span>
        <span class="kpi-value">{{ alerts.length }}</span>
        <span class="kpi-sub text-danger">{{ criticalAlertsCount }} {{ 'dashboard.critical' | t }}</span>
      </div>
      <div class="kpi-icon kpi-icon-danger"><i class="pi pi-exclamation-triangle"></i></div>
    </div>
  </div>

  <!-- Charts & Alerts Row -->
  <div class="dashboard-grid">
    <!-- Fleet Chart -->
    <div class="card dashboard-card">
      <div class="card-header">
        <div>
          <h4 class="card-title">{{ 'dashboard.fleetAvailability' | t }}</h4>
          <p class="card-desc">{{ 'dashboard.currentDistribution' | t }}</p>
        </div>
      </div>
      <div class="chart-wrapper">
        <p-chart *ngIf="chartData" type="doughnut" [data]="chartData" [options]="chartOptions" styleClass="w-full max-w-[220px]"></p-chart>
      </div>
    </div>

    <!-- Alerts Feed -->
    <div class="card dashboard-card dashboard-card-wide">
      <div class="card-header">
        <div>
          <h4 class="card-title">{{ 'dashboard.priorityAlerts' | t }}</h4>
          <p class="card-desc">{{ 'dashboard.priorityAlertsDesc' | t }}</p>
        </div>
        <a routerLink="/alerts" class="text-accent" style="font-size:12px;font-weight:600;text-decoration:none;">{{ 'common.viewAll' | t }}</a>
      </div>
      <div class="alerts-list">
        <div *ngIf="alerts.length === 0" class="empty-state-sm">
          {{ 'dashboard.noActiveAlert' | t }}
        </div>
        <div *ngFor="let alert of alerts" class="alert-row">
          <div class="alert-row-left">
            <span class="alert-row-icon"
              [class.badge-danger]="alert.severity === 'Critical'"
              [class.badge-warning]="alert.severity === 'Warning'"
              [class.badge-info]="alert.severity === 'Info'"
            >
              <i class="pi pi-exclamation-circle"></i>
            </span>
            <div>
              <span class="alert-row-target">{{ alert.target }}</span>
              <span class="alert-row-msg">{{ i18n.translateAlertMessage(alert.message) }}</span>
            </div>
          </div>
          <span class="badge"
            [class.badge-danger]="alert.severity === 'Critical'"
            [class.badge-warning]="alert.severity === 'Warning'"
            [class.badge-info]="alert.severity === 'Info'"
          >{{ i18n.translateAlertTime(alert.daysOrKmLeftText) }}</span>
        </div>
      </div>
    </div>
  </div>

  <!-- Unpaid Contracts -->
  <div class="card">
    <div class="card-header">
      <div>
        <h4 class="card-title">{{ 'dashboard.unpaid' | t }}</h4>
        <p class="card-desc">{{ 'dashboard.unpaidDesc' | t }}</p>
      </div>
      <a routerLink="/reports" class="text-accent" style="font-size:12px;font-weight:600;text-decoration:none;">{{ 'common.reports' | t }}</a>
    </div>
    <div class="table-wrapper">
      <p-table [value]="unpaidContracts" styleClass="p-datatable-sm">
        <ng-template pTemplate="header">
          <tr>
            <th>{{ 'dashboard.contractNo' | t }}</th>
            <th>{{ 'common.client' | t }}</th>
            <th>{{ 'common.vehicle' | t }}</th>
            <th>{{ 'dashboard.departure' | t }}</th>
            <th>{{ 'common.amount' | t }}</th>
            <th>{{ 'common.status' | t }}</th>
            <th>{{ 'common.actions' | t }}</th>
          </tr>
        </ng-template>
        <ng-template pTemplate="body" let-contract>
          <tr>
            <td class="font-semibold text-accent">{{ contract.contractNumber }}</td>
            <td>{{ contract.client?.fullName }}</td>
            <td>{{ contract.vehicle?.brand }} {{ contract.vehicle?.model }} ({{ contract.vehicle?.matricule }})</td>
            <td>{{ contract.startDate | date:'dd/MM/yyyy' }}</td>
            <td class="font-semibold">{{ contract.finalAmountDue | appCurrency }}</td>
            <td>
              <span class="badge"
                [class.badge-danger]="contract.paymentStatus === 'Unpaid'"
                [class.badge-warning]="contract.paymentStatus === 'PartiallyPaid'"
              >
                {{ contract.paymentStatus === 'Unpaid' ? ('dashboard.paymentUnpaid' | t) : ('dashboard.paymentPartial' | t) }}
              </span>
            </td>
            <td>
              <a 
                [routerLink]="['/contracts']" 
                [queryParams]="{search: contract.contractNumber}"
                class="text-accent"
                style="font-size:12px;font-weight:600;text-decoration:none;"
              >{{ 'common.manage' | t }}</a>
            </td>
          </tr>
        </ng-template>
        <ng-template pTemplate="emptymessage">
          <tr>
            <td colspan="7" class="empty-state-sm">{{ 'dashboard.noUnpaidContracts' | t }}</td>
          </tr>
        </ng-template>
      </p-table>
    </div>
  </div>
</div>

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\fuel\fuel.component.html
```html
<div class="page">
  <!-- Top Header Dashboard Controls -->
  <div class="page-header">
    <div>
      <h2 class="page-header-title">{{ 'fuel.pageTitle' | t }}</h2>
      <p class="page-header-desc">{{ 'fuel.pageDesc' | t }}</p>
    </div>
    <div class="header-actions">
      <button (click)="exportAllKmCsv()" class="btn-secondary" style="border-color: #475569; color: #fff; background: transparent;">
        <i class="pi pi-download"></i> {{ 'fuel.exportOdometers' | t }}
      </button>
      <button (click)="exportAllFuelCsv()" class="btn-primary">
        <i class="pi pi-download"></i> {{ 'fuel.exportFuel' | t }}
      </button>
    </div>
  </div>

  <!-- Main Split Layout -->
  <div class="split-layout">
    <!-- Left Column: Fleet List and Inactivity Alerts -->
    <div class="split-left">
      
      <!-- Inactivity Alerts Card -->
      <div *ngIf="inactiveVehicles.length > 0" class="inactivity-card">
        <div class="inactivity-header">
          <div class="inactivity-icon-box">
            <i class="pi pi-exclamation-triangle" style="font-size: 14px;"></i>
          </div>
          <div>
            <h3 class="inactivity-title">{{ 'fuel.inactivityDetected' | t }}</h3>
            <p class="inactivity-desc">
              {{ 'fuel.inactivityDesc' | t }} <strong>{{ inactivityThresholdDays }} {{ 'fuel.daysOfInactivity' | t }}</strong>.
            </p>
          </div>
        </div>
        <div class="inactivity-list">
          <div *ngFor="let v of inactiveVehicles" class="inactivity-item">
            <span style="font-weight: 600;">{{ v.brand }} {{ v.model }} <span style="color: var(--color-text-muted); font-size: 11px;">({{ v.matricule }})</span></span>
            <span class="badge badge-warning" style="text-transform: none;">
              {{ v.daysInactive }} {{ 'fuel.daysOfInactivity' | t }}
            </span>
          </div>
        </div>
      </div>

      <!-- Fleet Odometer Overview -->
      <div class="card" style="overflow: hidden;">
        <div class="vehicle-list-card-header">
          <h3 class="vehicle-list-title">
            <i class="pi pi-car text-accent"></i> {{ 'fuel.fleetOdometers' | t }}
          </h3>
          <span class="vehicle-list-count">
            {{ vehiclesList.length }} {{ 'fuel.vehiclesCount' | t }}
          </span>
        </div>
        
        <div class="vehicle-list">
          <div *ngFor="let vehicle of vehiclesList" 
               (click)="selectVehicle(vehicle)"
               [class.active]="selectedVehicle?.id === vehicle.id"
               class="vehicle-item"
           >
            <div class="vehicle-info">
              <h4 class="vehicle-name">{{ vehicle.brand }} {{ vehicle.model }}</h4>
              <span class="vehicle-plate">{{ 'fuel.plateLabel' | t }}: <span style="font-family: monospace; font-weight: 600; color: var(--color-text);">{{ vehicle.matricule }}</span></span>
              <div class="vehicle-badges">
                <span class="badge"
                      [class.badge-success]="vehicle.status === 'Available'"
                      [class.badge-accent]="vehicle.status === 'Rented'"
                      [class.badge-warning]="vehicle.status === 'InMaintenance'"
                      style="font-size: 9px;"
                >
                  {{ vehicle.status === 'Available' ? ('fuel.statusFree' | t) : vehicle.status === 'Rented' ? ('fuel.statusRented' | t) : ('fuel.statusGarage' | t) }}
                </span>
                <span class="badge badge-neutral" style="font-size: 9px;">
                  {{ vehicle.fuelType }}
                </span>
              </div>
            </div>
            
            <div class="vehicle-odometer-box">
              <div>
                <span class="odometer-number">{{ vehicle.currentKm | number }}</span>
                <span style="color: var(--color-text-muted); font-size: 11px; margin-left: 2px;">km</span>
              </div>
              <button (click)="openAddKm(vehicle, $event)" class="btn-primary" style="padding: 4px 8px; font-size: 11px; background: var(--color-bg); color: var(--color-text-secondary); border: 1px solid var(--color-border);">
                <i class="pi pi-plus" style="font-size: 9px;"></i> {{ 'fuel.reading' | t }}
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Right Column: Selected Vehicle Logs & Analytics -->
    <div class="split-right">
      
      <!-- Unselected State -->
      <div *ngIf="!selectedVehicle" class="card unselected-placeholder" style="border-style: dashed;">
        <i class="pi pi-chart-line unselected-icon"></i>
        <h3 style="font-weight: 700; font-size: 15px; color: var(--color-text); margin: 0;">{{ 'fuel.noVehicleSelected' | t }}</h3>
        <p style="font-size: 13px; color: var(--color-text-muted); max-w: 320px; margin: 4px 0 0;">
          {{ 'fuel.selectVehiclePrompt' | t }}
        </p>
      </div>

      <!-- Selected State Dashboard -->
      <div *ngIf="selectedVehicle" style="display: flex; flex-direction: column; gap: 20px;">
        
        <!-- Selected Vehicle Meta Info -->
        <div class="card meta-info-bar">
          <div class="meta-title-box">
            <div class="meta-title-row">
              <h3 class="meta-title">{{ selectedVehicle.brand }} {{ selectedVehicle.model }}</h3>
              <span class="meta-plate">{{ selectedVehicle.matricule }}</span>
            </div>
            <p class="meta-desc">
              {{ 'fuel.currentOdometer' | t }}: <strong style="color: var(--color-text);">{{ selectedVehicle.currentKm | number }} km</strong> | {{ 'vehicles.transmission' | t }}: {{ selectedVehicle.transmission === 'Manual' ? ('fuel.transmissionManual' | t) : ('fuel.transmissionAutomatic' | t) }}
            </p>
          </div>
          <div class="meta-actions">
            <button (click)="openAddFuel()" class="btn-primary">
              <i class="pi pi-plus"></i> {{ 'fuel.registerFillup' | t }}
            </button>
            <button (click)="exportVehicleFuelCsv()" class="btn-secondary">
              <i class="pi pi-download"></i> {{ 'fuel.csvFuel' | t }}
            </button>
            <button (click)="exportVehicleKmCsv()" class="btn-secondary">
              <i class="pi pi-download"></i> {{ 'fuel.csvKm' | t }}
            </button>
          </div>
        </div>

        <!-- Fuel Consumption Trend Chart -->
        <div class="card" style="padding: 16px;">
          <h3 class="card-title-inner" style="margin-bottom: 16px;">
            <i class="pi pi-chart-line text-accent"></i> {{ 'fuel.consumptionTrend' | t }}
          </h3>
          <div *ngIf="chartData" style="height: 240px; position: relative;">
            <p-chart type="line" [data]="chartData" [options]="chartOptions" height="240px"></p-chart>
          </div>
          <div *ngIf="!chartData" style="height: 120px; display: flex; flex-direction: column; align-items: center; justify-content: center; color: var(--color-text-muted); text-align: center;">
            <i class="pi pi-info-circle" style="font-size: 20px; margin-bottom: 4px;"></i>
            <span style="font-size: 13px; font-weight: 600;">{{ 'fuel.insufficientData' | t }}</span>
            <span style="font-size: 11px; margin-top: 2px;">{{ 'fuel.needTwoFillups' | t }}</span>
          </div>
        </div>

        <!-- Fuel Fill-up Log Table -->
        <div class="card" style="overflow: hidden;">
          <div class="card-header-inner">
            <h3 class="card-title-inner">
              <i class="pi pi-filter text-accent"></i> {{ 'fuel.fuelHistory' | t }}
            </h3>
          </div>
          
          <div style="padding: 8px;">
            <p-table [value]="fuelLogs" [rows]="5" [paginator]="true" responsiveLayout="scroll" styleClass="p-datatable-sm">
              <ng-template pTemplate="header">
                <tr>
                  <th>{{ 'common.date' | t }}</th>
                  <th>{{ 'fuel.odometer' | t }}</th>
                  <th>{{ 'fuel.liters' | t }}</th>
                  <th>{{ 'fuel.pricePerL' | t }}</th>
                  <th>{{ 'fuel.totalCost' | t }}</th>
                  <th>{{ 'fuel.average' | t }}</th>
                  <th>{{ 'vehicles.station' | t }}</th>
                  <th style="width: 50px;"></th>
                </tr>
              </ng-template>
              <ng-template pTemplate="body" let-entry>
                <tr>
                  <td>{{ entry.log.date | date:'dd/MM/yyyy' }}</td>
                  <td style="font-weight: 600;">{{ entry.log.kmValue | number }} km</td>
                  <td>{{ entry.log.liters }} L</td>
                  <td>{{ entry.log.costPerLiter | appCurrency }}/L</td>
                  <td style="font-weight: 600;">{{ entry.log.totalCost | appCurrency }}</td>
                  <td>
                    <div style="display: flex; align-items: center; gap: 4px;">
                      <span *ngIf="entry.kmDrivenSinceLastFill > 0" style="font-weight: 700; color: var(--color-accent);">
                        {{ entry.consumptionL100 }} <span style="font-size: 9px; color: var(--color-text-secondary); font-weight: 500;">L/100</span>
                      </span>
                      <span *ngIf="entry.kmDrivenSinceLastFill <= 0" style="color: var(--color-text-muted);">--</span>
                      <!-- Anomaly Flag Badge -->
                      <span *ngIf="entry.log.isAnomaly" class="badge badge-danger" style="font-size: 9px;" [title]="'fuel.anomalyLabel' | t">
                        {{ 'fuel.anomalyLabel' | t }}
                      </span>
                    </div>
                  </td>
                  <td>
                    <div style="font-weight: 600; color: var(--color-text-secondary);">{{ entry.log.stationName }}</div>
                    <div style="font-size: 10px; color: var(--color-text-muted);">{{ entry.log.fuelType }}</div>
                  </td>
                  <td style="text-align: center;">
                    <button (click)="deleteFuelLog(entry.log.id)" [title]="'common.delete' | t" class="btn-icon danger" style="width: 24px; height: 24px;">
                      <i class="pi pi-trash" style="font-size: 10px;"></i>
                    </button>
                  </td>
                </tr>
              </ng-template>
              <ng-template pTemplate="emptymessage">
                <tr>
                  <td colspan="8" style="text-align: center; padding: 20px; color: var(--color-text-muted);">{{ 'fuel.noFuelLog' | t }}</td>
                </tr>
              </ng-template>
            </p-table>
          </div>
        </div>

        <!-- Kilometer Odometer Timeline -->
        <div class="card" style="overflow: hidden;">
          <div class="card-header-inner">
            <h3 class="card-title-inner">
              <i class="pi pi-history text-accent"></i> {{ 'fuel.kmTimeline' | t }}
            </h3>
          </div>
          
          <div style="padding: 8px;">
            <p-table [value]="kmHistory" [rows]="5" [paginator]="true" responsiveLayout="scroll" styleClass="p-datatable-sm">
              <ng-template pTemplate="header">
                <tr>
                  <th>{{ 'fuel.dateTime' | t }}</th>
                  <th>{{ 'fuel.odometerValue' | t }}</th>
                  <th>{{ 'fuel.source' | t }}</th>
                  <th>{{ 'fuel.remarksEvent' | t }}</th>
                </tr>
              </ng-template>
              <ng-template pTemplate="body" let-entry>
                <tr>
                  <td style="font-size: 12px; color: var(--color-text-secondary);">{{ entry.date | date:'dd/MM/yyyy HH:mm' }}</td>
                  <td style="font-weight: 700; color: var(--color-text);">{{ entry.kmValue | number }} km</td>
                  <td>
                    <span class="badge"
                          [class.badge-accent]="entry.source === 'Manual'"
                          [class.badge-warning]="entry.source === 'Fuel'"
                          [class.badge-success]="entry.source === 'Contract'"
                    >
                      {{ entry.source === 'Manual' ? ('fuel.sourceManual' | t) : entry.source === 'Fuel' ? ('fuel.sourceFuel' | t) : ('fuel.sourceContract' | t) }}
                    </span>
                  </td>
                  <td style="font-size: 12px; color: var(--color-text-secondary);">{{ entry.notes }}</td>
                </tr>
              </ng-template>
              <ng-template pTemplate="emptymessage">
                <tr>
                  <td colspan="4" style="text-align: center; padding: 20px; color: var(--color-text-muted);">{{ 'fuel.noKmHistory' | t }}</td>
                </tr>
              </ng-template>
            </p-table>
          </div>
        </div>

      </div>

    </div>
  </div>
</div>

<!-- Dialog: Add Manual Kilometer Entry -->
<p-dialog [header]="'fuel.manualOdometerReading' | t" [(visible)]="showAddKmDialog" [modal]="true" [style]="{width: '400px'}" [draggable]="false" [resizable]="false">
  <div style="display: flex; flex-direction: column; gap: 12px; margin-top: 8px;">
    <div>
      <label class="form-label">{{ 'fuel.readingDate' | t }}</label>
      <p-datepicker [(ngModel)]="kmForm.date" dateFormat="dd/mm/yy" [showTime]="true" appendTo="body" class="w-full"></p-datepicker>
    </div>
    
    <div>
      <label class="form-label">{{ 'fuel.odometerIndex' | t }}</label>
      <input type="number" [(ngModel)]="kmForm.kmValue" class="form-input" required />
    </div>

    <div>
      <label class="form-label">{{ 'fuel.readingNotes' | t }}</label>
      <textarea [(ngModel)]="kmForm.notes" rows="2" class="form-input" [placeholder]="'fuel.readingPlaceholder' | t"></textarea>
    </div>
  </div>

  <div style="display: flex; justify-content: flex-end; gap: 8px; margin-top: 16px; border-top: 1px solid var(--color-border-light); padding-top: 12px;">
    <button (click)="showAddKmDialog = false" class="btn-secondary">{{ 'common.cancel' | t }}</button>
    <button (click)="submitKm()" class="btn-primary">{{ 'common.save' | t }}</button>
  </div>
</p-dialog>

<!-- Dialog: Add Fuel Fill-up Log -->
<p-dialog [header]="'fuel.registerFuelFillup' | t" [(visible)]="showAddFuelDialog" [modal]="true" [style]="{width: '450px'}" [draggable]="false" [resizable]="false">
  <div class="form-grid" style="margin-top: 8px;">
    <div>
      <label class="form-label">{{ 'common.date' | t }}</label>
      <p-datepicker [(ngModel)]="fuelForm.date" dateFormat="dd/mm/yy" [showTime]="true" appendTo="body" class="w-full"></p-datepicker>
    </div>
    <div>
      <label class="form-label">{{ 'fuel.kmIndex' | t }}</label>
      <input type="number" [(ngModel)]="fuelForm.kmValue" class="form-input" required />
    </div>

    <div>
      <label class="form-label">{{ 'fuel.volumeLiters' | t }}</label>
      <input type="number" step="0.01" [(ngModel)]="fuelForm.liters" class="form-input" required />
    </div>
    <div>
      <label class="form-label">{{ 'fuel.pricePerLiter' | t }}</label>
      <input type="number" step="0.001" [(ngModel)]="fuelForm.costPerLiter" class="form-input" required />
    </div>

    <div class="form-full">
      <label class="form-label">{{ 'fuel.stationName' | t }}</label>
      <input type="text" [(ngModel)]="fuelForm.stationName" placeholder="Ex: Shell, TotalEnergies, Q8..." class="form-input" required />
    </div>

    <div class="form-full">
      <label class="form-label">{{ 'fuel.fuelType' | t }}</label>
      <select [(ngModel)]="fuelForm.fuelType" class="form-input">
        <option value="Gasoline">{{ 'fuel.gasoline' | t }}</option>
        <option value="Diesel">{{ 'fuel.diesel' | t }}</option>
        <option value="Electric">{{ 'fuel.electric' | t }}</option>
        <option value="Hybrid">{{ 'fuel.hybrid' | t }}</option>
        <option value="LPG">{{ 'fuel.lpg' | t }}</option>
      </select>
    </div>

    <!-- Calculated Info Hint -->
    <div class="calc-hint-box">
      <span>{{ 'fuel.estimatedTotal' | t }}</span>
      <strong style="font-size: 13px;">{{ (fuelForm.liters * fuelForm.costPerLiter) | appCurrency }}</strong>
    </div>
  </div>

  <div style="display: flex; justify-content: flex-end; gap: 8px; margin-top: 16px; border-top: 1px solid var(--color-border-light); padding-top: 12px;">
    <button (click)="showAddFuelDialog = false" class="btn-secondary">{{ 'common.cancel' | t }}</button>
    <button (click)="submitFuel()" class="btn-primary">{{ 'common.save' | t }}</button>
  </div>
</p-dialog>

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\insurance-inspections\insurance-inspections.component.html
```html
<div class="page">
  <!-- Page Header -->
  <div class="page-header">
    <h2 class="page-title">{{ 'insurance.pageTitle' | t }}</h2>
    <p class="page-desc">{{ 'insurance.pageDesc' | t }}</p>
  </div>

  <!-- Grid layout for Insurances & Inspections Expirations -->
  <div class="grid-2col">
    <!-- Insurance policies expiries -->
    <div class="card" style="padding: 20px;">
      <div class="card-header-inner">
        <div>
          <h4 class="card-title-inner">{{ 'insurance.insuranceSchedule' | t }}</h4>
          <p class="card-desc-inner">{{ 'insurance.insuranceScheduleDesc' | t }}</p>
        </div>
        <span class="badge badge-accent">{{ 'insurance.auto' | t }}</span>
      </div>

      <div style="overflow-x: auto;">
        <p-table [value]="policies" styleClass="p-datatable-sm">
          <ng-template pTemplate="header">
            <tr>
              <th>{{ 'common.vehicle' | t }}</th>
              <th>{{ 'insurance.expiry' | t }}</th>
              <th>{{ 'insurance.timeRemaining' | t }}</th>
              <th style="width: 100px; text-align: center;">{{ 'insurance.severity' | t }}</th>
            </tr>
          </ng-template>
          <ng-template pTemplate="body" let-policy>
            <tr>
              <td style="font-weight: 600;">{{ policy.vehicle }}</td>
              <td>{{ policy.date | date:'yyyy-MM-dd' }}</td>
              <td style="font-weight: 600; color: var(--color-text-secondary);">{{ i18n.translateAlertTime(policy.daysOrKm) }}</td>
              <td style="text-align: center;">
                <span 
                  [class.badge-danger]="policy.severity === 'Critical'"
                  [class.badge-warning]="policy.severity === 'Warning'"
                  [class.badge-info]="policy.severity === 'Info'"
                  class="badge"
                >
                  {{ policy.severity === 'Critical' ? ('insurance.severityCritical' | t) : policy.severity === 'Warning' ? ('insurance.severityWarning' | t) : ('insurance.severityInfo' | t) }}
                </span>
              </td>
            </tr>
          </ng-template>
          <ng-template pTemplate="emptymessage">
            <tr>
              <td colspan="4" style="text-align: center; padding: 20px; color: var(--color-text-muted);">{{ 'insurance.noInsuranceAlert' | t }}</td>
            </tr>
          </ng-template>
        </p-table>
      </div>
    </div>

    <!-- Technical inspections expiries -->
    <div class="card" style="padding: 20px;">
      <div class="card-header-inner">
        <div>
          <h4 class="card-title-inner">{{ 'insurance.inspectionSchedule' | t }}</h4>
          <p class="card-desc-inner">{{ 'insurance.inspectionScheduleDesc' | t }}</p>
        </div>
        <span class="badge badge-success">{{ 'insurance.compliant' | t }}</span>
      </div>

      <div style="overflow-x: auto;">
        <p-table [value]="inspections" styleClass="p-datatable-sm">
          <ng-template pTemplate="header">
            <tr>
              <th>{{ 'common.vehicle' | t }}</th>
              <th>{{ 'insurance.expiry' | t }}</th>
              <th>{{ 'insurance.alertStatus' | t }}</th>
              <th style="width: 100px; text-align: center;">{{ 'insurance.severity' | t }}</th>
            </tr>
          </ng-template>
          <ng-template pTemplate="body" let-insp>
            <tr>
              <td style="font-weight: 600;">{{ insp.vehicle }}</td>
              <td>{{ insp.date | date:'yyyy-MM-dd' }}</td>
              <td style="font-weight: 600; color: var(--color-text-secondary);">{{ i18n.translateAlertTime(insp.daysOrKm) }}</td>
              <td style="text-align: center;">
                <span 
                  [class.badge-danger]="insp.severity === 'Critical'"
                  [class.badge-warning]="insp.severity === 'Warning'"
                  [class.badge-info]="insp.severity === 'Info'"
                  class="badge"
                >
                  {{ insp.severity === 'Critical' ? ('insurance.severityCritical' | t) : insp.severity === 'Warning' ? ('insurance.severityWarning' | t) : ('insurance.severityInfo' | t) }}
                </span>
              </td>
            </tr>
          </ng-template>
          <ng-template pTemplate="emptymessage">
            <tr>
              <td colspan="4" style="text-align: center; padding: 20px; color: var(--color-text-muted);">{{ 'insurance.noInspectionAlert' | t }}</td>
            </tr>
          </ng-template>
        </p-table>
      </div>
    </div>
  </div>
</div>

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\login\login.component.html
```html
<div class="login-page">
  <!-- Language Selector (top-right corner) -->
  <div class="login-lang-bar">
    <button 
      *ngFor="let lang of languages" 
      (click)="switchLang(lang.code)"
      class="login-lang-btn"
      [class.active]="i18n.currentLang() === lang.code"
    >
      <span>{{ lang.flag }}</span>
      <span>{{ lang.code | uppercase }}</span>
    </button>
  </div>

  <div class="login-container">
    <div class="login-card card">
      <!-- Header -->
      <div class="login-header">
        <div class="login-logo">
          <i class="pi pi-car"></i>
        </div>
        <h1 class="login-title">{{ 'login.title' | t }}</h1>
        <p class="login-subtitle">{{ 'login.subtitle' | t }}</p>
      </div>

      <!-- Error -->
      <div *ngIf="errorMessage" class="login-error">
        <i class="pi pi-info-circle"></i>
        <span>{{ errorMessage }}</span>
      </div>

      <!-- Form -->
      <form (ngSubmit)="onSubmit()" class="login-form">
        <div class="form-group">
          <label class="form-label required">{{ 'login.username' | t }}</label>
          <div class="input-wrapper">
            <i class="pi pi-user input-icon"></i>
            <input 
              type="text" 
              name="username"
              [(ngModel)]="username"
              required
              placeholder="admin"
              class="form-input has-icon"
            />
          </div>
        </div>

        <div class="form-group">
          <label class="form-label required">{{ 'login.password' | t }}</label>
          <div class="input-wrapper">
            <i class="pi pi-lock input-icon"></i>
            <input 
              type="password" 
              name="password"
              [(ngModel)]="password"
              required
              placeholder="â€¢â€¢â€¢â€¢â€¢â€¢â€¢â€¢"
              class="form-input has-icon"
            />
          </div>
        </div>

        <button 
          type="submit" 
          [disabled]="isLoading"
          class="btn-primary login-btn"
        >
          <span *ngIf="isLoading" class="spinner"></span>
          {{ isLoading ? ('login.loggingIn' | t) : ('login.loginBtn' | t) }}
        </button>
      </form>

      <div class="login-footer">
        {{ 'login.defaultCredentials' | t }} <code>admin</code> / <code>AdminPassword123!</code>
      </div>
    </div>
  </div>
</div>

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\maintenance\maintenance.component.html
```html
<div class="page">
  <!-- Page Header -->
  <div class="page-header">
    <div>
      <h2 class="page-title">{{ 'maintenance.pageTitle' | t }}</h2>
      <p class="page-desc">{{ 'maintenance.pageDesc' | t }}</p>
    </div>
    <div class="header-actions">
      <button (click)="openCalendar()" class="btn-secondary">
        <i class="pi pi-calendar"></i>
        <span>{{ 'maintenance.maintenanceCalendar' | t }}</span>
      </button>
      <button (click)="openAddDialog()" class="btn-primary">
        <i class="pi pi-plus"></i>
        <span>{{ 'maintenance.registerIntervention' | t }}</span>
      </button>
    </div>
  </div>

  <!-- Maintenance list -->
  <div class="card table-card">
    <div class="table-scroll" *ngIf="maintenances.length > 0">
      <table class="modern-table">
        <thead>
          <tr>
            <th>{{ 'common.vehicle' | t }}</th>
            <th>{{ 'maintenance.intervention' | t }}</th>
            <th>{{ 'maintenance.dates' | t }}</th>
            <th>Km</th>
            <th class="col-right">{{ 'common.cost' | t }}</th>
            <th>{{ 'maintenance.workshop' | t }}</th>
            <th>{{ 'maintenance.invoice' | t }}</th>
            <th class="col-center">{{ 'common.status' | t }}</th>
            <th class="col-center">{{ 'common.actions' | t }}</th>
          </tr>
        </thead>
        <tbody>
          <tr *ngFor="let maint of maintenances">
            <td>
              <div class="cell-stack">
                <span class="cell-stack-title">{{ maint.vehicle?.brand }} {{ maint.vehicle?.model }}</span>
                <span class="cell-stack-sub cell-mono">{{ maint.vehicle?.matricule }}</span>
              </div>
            </td>
            <td><span class="cell-primary">{{ i18n.translateMaintenanceType(maint.maintenanceType) }}</span></td>
            <td>
              <div class="cell-stack">
                <span class="cell-muted">{{ maint.datePerformed | date:'dd/MM/yyyy' }}</span>
                <span class="cell-stack-sub" *ngIf="maint.nextScheduledDate">{{ 'maintenance.nextScheduled' | t }}: {{ maint.nextScheduledDate | date:'dd/MM/yyyy' }}</span>
                <span class="cell-stack-sub" *ngIf="!maint.nextScheduledDate">â€”</span>
              </div>
            </td>
            <td><span class="cell-muted">{{ maint.kmAtMaintenance | number }} km</span></td>
            <td class="col-right"><span class="cell-primary">{{ maint.totalCost | appCurrency }}</span></td>
            <td><span class="cell-muted">{{ maint.workshopName }}</span></td>
            <td>
              <a *ngIf="maint.invoiceFilePath" [href]="'http://localhost:5222' + maint.invoiceFilePath" target="_blank" class="table-link">
                {{ maint.invoiceNumber || ('common.see' | t) }}
              </a>
              <span *ngIf="!maint.invoiceFilePath" class="cell-muted">â€”</span>
            </td>
            <td class="col-center">
              <span
                [class.badge-info]="maint.status === 'Scheduled'"
                [class.badge-warning]="maint.status === 'InProgress'"
                [class.badge-success]="maint.status === 'Completed'"
                class="badge"
              >
                {{ 'statuses.' + (maint.status === 'InProgress' ? 'inProgress' : maint.status.toLowerCase()) | t }}
              </span>
            </td>
            <td>
              <div class="cell-actions">
                <button (click)="openEditDialog(maint)" [title]="'common.edit' | t" class="btn-icon"><i class="pi pi-pencil"></i></button>
                <button (click)="deleteMaint(maint.id)" [title]="'common.delete' | t" class="btn-icon danger"><i class="pi pi-trash"></i></button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <div *ngIf="maintenances.length === 0" class="table-empty">
      <i class="pi pi-cog"></i>
      <span class="table-empty-title">{{ 'maintenance.noMaintenance' | t }}</span>
      <p class="table-empty-desc">{{ 'maintenance.noMaintenanceDesc' | t }}</p>
    </div>

    <div *ngIf="maintenances.length > 0" class="table-footer">
      <span class="table-footer-info"><strong>{{ maintenances.length }}</strong> {{ 'maintenance.interventions' | t }}</span>
    </div>
  </div>

  <!-- ADD / EDIT MAINTENANCE DIALOG -->
  <p-dialog 
    [(visible)]="showCrudDialog" 
    [style]="{width: '500px'}" 
    [modal]="true" 
    [header]="isEditMode ? ('maintenance.editMaintenance' | t) : ('maintenance.newIntervention' | t)"
    [draggable]="false"
    [resizable]="false"
  >
    <form (ngSubmit)="onSubmitMaint()" class="form-grid">
      <div>
        <label class="form-label required">{{ 'common.vehicle' | t }}</label>
        <select [(ngModel)]="maintenanceForm.vehicleId" name="vehicleId" required [disabled]="isEditMode" class="form-input">
          <option [ngValue]="null">{{ 'contracts.chooseVehicle' | t }}</option>
          <option *ngFor="let v of vehicles" [value]="v.id">{{ v.brand }} {{ v.model }} ({{ v.matricule }})</option>
        </select>
      </div>

      <div>
        <label class="form-label required">{{ 'maintenance.interventionType' | t }}</label>
        <select [(ngModel)]="maintenanceForm.maintenanceType" name="maintenanceType" required class="form-input">
          <option *ngFor="let t of maintenanceTypes" [value]="t">{{ i18n.translateMaintenanceType(t) }}</option>
        </select>
      </div>

      <div>
        <label class="form-label required">{{ 'maintenance.interventionDate' | t }}</label>
        <p-datepicker [(ngModel)]="maintenanceForm.datePerformed" name="datePerformed" dateFormat="yy-mm-dd" [showIcon]="true" styleClass="w-full"></p-datepicker>
      </div>

      <div>
        <label class="form-label">{{ 'maintenance.nextMaintenance' | t }}</label>
        <p-datepicker [(ngModel)]="maintenanceForm.nextScheduledDate" name="nextScheduledDate" dateFormat="yy-mm-dd" [showIcon]="true" styleClass="w-full"></p-datepicker>
      </div>

      <div>
        <label class="form-label required">{{ 'maintenance.counterKm' | t }}</label>
        <input type="number" [(ngModel)]="maintenanceForm.kmAtMaintenance" name="kmAtMaintenance" required class="form-input"/>
      </div>

      <div>
        <label class="form-label required">{{ 'maintenance.interventionStatus' | t }}</label>
        <select [(ngModel)]="maintenanceForm.status" name="status" required class="form-input">
          <option *ngFor="let s of statuses" [value]="s.value">{{ 'statuses.' + (s.value === 'InProgress' ? 'inProgress' : s.value.toLowerCase()) | t }}</option>
        </select>
      </div>

      <!-- Cost Split -->
      <div class="cost-box">
        <div>
          <label class="form-label" style="color: var(--color-text-secondary);">{{ 'maintenance.laborCost' | t }}</label>
          <input type="number" [(ngModel)]="maintenanceForm.laborCost" name="laborCost" class="form-input"/>
        </div>
        <div>
          <label class="form-label" style="color: var(--color-text-secondary);">{{ 'maintenance.partsCost' | t }}</label>
          <input type="number" [(ngModel)]="maintenanceForm.partsCost" name="partsCost" class="form-input"/>
        </div>
      </div>

      <div>
        <label class="form-label required">{{ 'maintenance.workshopName' | t }}</label>
        <input type="text" [(ngModel)]="maintenanceForm.workshopName" name="workshopName" required class="form-input" placeholder="Garage Central"/>
      </div>

      <div>
        <label class="form-label">{{ 'maintenance.workshopContact' | t }}</label>
        <input type="text" [(ngModel)]="maintenanceForm.workshopContact" name="workshopContact" class="form-input" placeholder="TÃ©l ou email"/>
      </div>

      <div class="form-full">
        <label class="form-label">{{ 'maintenance.workshopAddress' | t }}</label>
        <input type="text" [(ngModel)]="maintenanceForm.workshopAddress" name="workshopAddress" class="form-input" placeholder="Adresse du garage"/>
      </div>

      <div>
        <label class="form-label">{{ 'maintenance.invoiceNo' | t }}</label>
        <input type="text" [(ngModel)]="maintenanceForm.invoiceNumber" name="invoiceNumber" class="form-input" placeholder="NÂ° de facture"/>
      </div>

      <div>
        <label class="form-label">{{ 'maintenance.invoiceFile' | t }}</label>
        <div style="display: flex; align-items: center; gap: 8px;">
          <input type="file" (change)="onInvoiceUpload($event)" style="font-size: 11px; padding: 4px 0;"/>
          <span *ngIf="uploadingInvoice" style="font-size: 11px; color: var(--color-accent);" class="animate-pulse">{{ 'common.loading' | t }}</span>
        </div>
      </div>

      <div class="form-full">
        <label class="form-label">{{ 'maintenance.workDescription' | t }}</label>
        <textarea [(ngModel)]="maintenanceForm.description" name="description" rows="3" class="form-input" placeholder="Vidange, changement plaquettes de frein avant..."></textarea>
      </div>

      <div class="form-full form-actions">
        <button type="button" (click)="showCrudDialog = false" class="btn-secondary">{{ 'common.cancel' | t }}</button>
        <button type="submit" class="btn-primary">{{ 'common.save' | t }}</button>
      </div>
    </form>
  </p-dialog>

  <!-- CALENDAR DIALOG VIEW -->
  <p-dialog 
    [(visible)]="showCalendarDialog" 
    [style]="{width: '500px'}" 
    [modal]="true" 
    [header]="'maintenance.calendarTitle' | t"
    [draggable]="false"
    [resizable]="false"
  >
    <div style="max-height: 400px; overflow-y: auto; padding-right: 4px;">
      <div *ngIf="calendarEvents.length === 0" style="text-align: center; padding: 20px; color: var(--color-text-muted);">
        {{ 'maintenance.noScheduledMaintenance' | t }}
      </div>
      
      <div class="event-timeline" *ngIf="calendarEvents.length > 0">
        <div *ngFor="let item of calendarEvents" class="event-item">
          <span class="event-dot"></span>
          <div class="event-card">
            <div>
              <span class="event-date">{{ item.start | date:'yyyy-MM-dd' }}</span>
              <h5 class="event-title">{{ item.title }}</h5>
              <span class="event-desc">{{ 'maintenance.plateLabel' | t }}: {{ item.vehicleMatricule }}</span>
            </div>
            <span class="badge badge-info">{{ i18n.translateMaintenanceType(item.maintenanceType) }}</span>
          </div>
        </div>
      </div>
    </div>
    
    <div style="display: flex; justify-content: flex-end; margin-top: 16px; border-top: 1px solid var(--color-border-light); padding-top: 12px;">
      <button (click)="showCalendarDialog = false" class="btn-secondary">{{ 'common.close' | t }}</button>
    </div>
  </p-dialog>
</div>

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\reports\reports.component.html
```html
<div class="page">
  <!-- Page Header -->
  <div class="page-header">
    <div>
      <h2 class="page-header-title" style="color: white;">{{ 'reports.pageTitle' | t }}</h2>
      <p class="page-header-desc">{{ 'reports.pageDesc' | t }}</p>
    </div>
    <div class="header-actions">
      <button (click)="exportProfitabilityCsv()" class="btn-primary">
        <i class="pi pi-download"></i> {{ 'reports.exportProfitability' | t }}
      </button>
    </div>
  </div>

  <!-- Financial & Utilization Overview Cards -->
  <div class="kpi-grid">
    <!-- Total Revenue Card -->
    <div class="card kpi-card">
      <div class="kpi-left">
        <span class="kpi-label">{{ 'reports.grossRevenue' | t }}</span>
        <h2 class="kpi-number">{{ revenue.totalRevenue | appCurrency }}</h2>
        <div class="kpi-sub-row">
          <span style="color: var(--color-success); font-weight: 600;"><i class="pi pi-check" style="font-size: 9px;"></i> {{ 'reports.paidLabel' | t }}: {{ revenue.paidRevenue | appCurrency }}</span>
          <span style="color: var(--color-danger); font-weight: 600;"><i class="pi pi-exclamation-circle" style="font-size: 9px;"></i> {{ 'reports.dueLabel' | t }}: {{ revenue.unpaidRevenue | appCurrency }}</span>
        </div>
      </div>
      <div class="kpi-icon-box indigo">
        <i class="pi pi-wallet text-xl"></i>
      </div>
    </div>

    <!-- Average Fleet Utilization Rate Card -->
    <div class="card kpi-card">
      <div class="kpi-left">
        <span class="kpi-label">{{ 'reports.avgUtilization' | t }}</span>
        <h2 class="kpi-number">{{ averageUtilization | number:'1.0-2' }}%</h2>
        <span class="kpi-subtext">{{ 'reports.utilizationDesc' | t }}</span>
      </div>
      <div class="kpi-icon-box emerald">
        <i class="pi pi-chart-bar text-xl"></i>
      </div>
    </div>

    <!-- Fleet Status Quick Stats -->
    <div class="card kpi-card">
      <div class="kpi-left">
        <span class="kpi-label">{{ 'reports.fleetSize' | t }}</span>
        <h2 class="kpi-number">{{ fleetStatus.total }} {{ 'reports.vehiclesLabel' | t }}</h2>
        <div class="kpi-sub-row" style="margin-top: 6px;">
          <span class="badge badge-accent" style="font-size: 9px;">{{ fleetStatus.rented }} {{ 'reports.rented' | t }}</span>
          <span class="badge badge-warning" style="font-size: 9px;">{{ fleetStatus.inMaintenance }} {{ 'reports.garage' | t }}</span>
        </div>
      </div>
      <div class="kpi-icon-box amber">
        <i class="pi pi-car text-xl"></i>
      </div>
    </div>
  </div>

  <!-- Charts Block -->
  <div class="grid-2col">
    <!-- Fleet Availability Chart -->
    <div class="card chart-card">
      <div class="chart-header">
        <h3 class="chart-title"><i class="pi pi-chart-pie text-accent"></i> {{ 'reports.fleetAvailability' | t }}</h3>
      </div>
      <div class="chart-container">
        <p-chart *ngIf="fleetStatusChart" type="doughnut" [data]="fleetStatusChart" [options]="fleetStatusOptions" class="w-full max-w-[220px]"></p-chart>
      </div>
    </div>

    <!-- Revenue Paid vs Unpaid Chart -->
    <div class="card chart-card">
      <div class="chart-header">
        <h3 class="chart-title"><i class="pi pi-dollar text-accent"></i> {{ 'reports.financialRecovery' | t }}</h3>
        <!-- Date Filters -->
        <div style="display: flex; gap: 4px; align-items: center;">
          <p-datepicker [(ngModel)]="startDate" [placeholder]="'reports.startDate' | t" (ngModelChange)="loadRevenue()" dateFormat="dd/mm/y" appendTo="body" style="width: 100px; font-size: 11px;"></p-datepicker>
          <span style="font-size: 11px; color: var(--color-text-muted);">{{ 'common.to' | t | lowercase }}</span>
          <p-datepicker [(ngModel)]="endDate" [placeholder]="'reports.endDate' | t" (ngModelChange)="loadRevenue()" dateFormat="dd/mm/y" appendTo="body" style="width: 100px; font-size: 11px;"></p-datepicker>
        </div>
      </div>
      <div class="chart-container">
        <p-chart *ngIf="revenueChart" type="doughnut" [data]="revenueChart" [options]="revenueOptions" class="w-full max-w-[220px]"></p-chart>
      </div>
    </div>
  </div>

  <!-- Detailed Fleet Cost & Profitability Table -->
  <div class="card" style="overflow: hidden;">
    <div class="table-header-inner">
      <h3 class="table-title-inner">
        <i class="pi pi-calculator text-accent"></i> {{ 'reports.profitabilityAnalysis' | t }}
      </h3>
    </div>

    <div style="padding: 8px;">
      <p-table [value]="profitabilityReport" [rows]="10" [paginator]="true" responsiveLayout="scroll">
        <ng-template pTemplate="header">
          <tr>
            <th>{{ 'common.vehicle' | t }}</th>
            <th>{{ 'vehicles.matricule' | t }}</th>
            <th style="text-align: center;">{{ 'reports.utilizationRate' | t }}</th>
            <th>{{ 'reports.revenueLabel' | t }}</th>
            <th>{{ 'reports.maintenanceLabel' | t }}</th>
            <th>{{ 'reports.fuelLabel' | t }}</th>
            <th>{{ 'reports.insuranceLabel' | t }}</th>
            <th>{{ 'reports.tco' | t }}</th>
            <th>{{ 'reports.netProfit' | t }}</th>
          </tr>
        </ng-template>
        <ng-template pTemplate="body" let-item>
          <tr>
            <td style="font-weight: 600; color: var(--color-text);">{{ item.brand }} {{ item.model }}</td>
            <td style="font-family: monospace; font-size: 12px; color: var(--color-text-secondary);">{{ item.matricule }}</td>
            <td style="text-align: center; font-weight: 700; color: var(--color-text-secondary);">{{ item.utilizationRate }}%</td>
            <td style="color: var(--color-success); font-weight: 600;">{{ item.revenue | appCurrency }}</td>
            <td style="color: var(--color-text-secondary); font-size: 12px;">{{ item.maintenanceCost | appCurrency }}</td>
            <td style="color: var(--color-text-secondary); font-size: 12px;">{{ item.fuelCost | appCurrency }}</td>
            <td style="color: var(--color-text-secondary); font-size: 12px;">{{ item.insuranceCost | appCurrency }}</td>
            <td style="color: var(--color-warning); font-weight: 600;">{{ item.totalCost | appCurrency }}</td>
            <td style="font-weight: 800; font-size: 14px;" [class.text-success]="item.profitability >= 0" [class.text-danger]="item.profitability < 0">
              {{ item.profitability | appCurrency }}
            </td>
          </tr>
        </ng-template>
        <ng-template pTemplate="emptymessage">
          <tr>
            <td colspan="9" style="text-align: center; padding: 20px; color: var(--color-text-muted);">{{ 'reports.noProfitabilityData' | t }}</td>
          </tr>
        </ng-template>
      </p-table>
    </div>
  </div>

  <!-- Bottom Details Grid: Top Clients & Unpaid Invoices -->
  <div class="split-layout">
    
    <!-- Top Clients Table -->
    <div class="split-left card" style="overflow: hidden;">
      <div class="table-header-inner">
        <h3 class="table-title-inner">
          <i class="pi pi-users text-accent"></i> {{ 'reports.topClients' | t }}
        </h3>
      </div>
      
      <div style="padding: 8px;">
        <p-table [value]="topClients" [rows]="5" responsiveLayout="scroll" styleClass="p-datatable-sm">
          <ng-template pTemplate="header">
            <tr>
              <th>{{ 'reports.clientName' | t }}</th>
              <th style="text-align: center; width: 80px;">{{ 'reports.rentals' | t }}</th>
              <th style="text-align: right; width: 120px;">{{ 'reports.totalRented' | t }}</th>
            </tr>
          </ng-template>
          <ng-template pTemplate="body" let-client>
            <tr>
              <td style="font-weight: 600; color: var(--color-text);">{{ client.name }}</td>
              <td style="text-align: center; font-weight: 700; color: var(--color-text-secondary);">{{ client.rentalsCount }}</td>
              <td style="text-align: right; font-weight: 800; color: var(--color-accent);">{{ client.totalRevenue | appCurrency }}</td>
            </tr>
          </ng-template>
          <ng-template pTemplate="emptymessage">
            <tr>
              <td colspan="3" style="text-align: center; padding: 20px; color: var(--color-text-muted);">{{ 'reports.noClients' | t }}</td>
            </tr>
          </ng-template>
        </p-table>
      </div>
    </div>

    <!-- Unpaid / Partially Paid Contracts Table -->
    <div class="split-right card" style="overflow: hidden;">
      <div class="table-header-inner">
        <h3 class="table-title-inner">
          <i class="pi pi-clock text-accent"></i> {{ 'reports.latePayments' | t }}
        </h3>
      </div>
      
      <div style="padding: 8px;">
        <p-table [value]="unpaidContracts" [rows]="5" [paginator]="true" responsiveLayout="scroll" styleClass="p-datatable-sm">
          <ng-template pTemplate="header">
            <tr>
              <th>{{ 'reports.contractNo' | t }}</th>
              <th>{{ 'common.client' | t }}</th>
              <th>{{ 'common.vehicle' | t }}</th>
              <th style="text-align: right; width: 120px;">{{ 'reports.amountDue' | t }}</th>
              <th style="text-align: center; width: 120px;">{{ 'reports.paymentStatus' | t }}</th>
            </tr>
          </ng-template>
          <ng-template pTemplate="body" let-c>
            <tr>
              <td style="font-family: monospace; font-weight: 600; color: var(--color-text);">{{ c.contractNumber }}</td>
              <td style="font-size: 12px; color: var(--color-text-secondary);">{{ c.client?.fullName }}</td>
              <td style="font-size: 12px; color: var(--color-text-secondary);">{{ c.vehicle?.brand }} {{ c.vehicle?.model }}</td>
              <td style="text-align: right; font-weight: 700; color: var(--color-danger);">{{ c.finalAmountDue | appCurrency }}</td>
              <td style="text-align: center;">
                <span class="badge"
                      [class.badge-danger]="c.paymentStatus === 'Unpaid'"
                      [class.badge-warning]="c.paymentStatus === 'PartiallyPaid'"
                >
                  {{ 'statuses.' + (c.paymentStatus === 'Unpaid' ? 'unpaid' : 'partiallyPaid') | t }}
                </span>
              </td>
            </tr>
          </ng-template>
          <ng-template pTemplate="emptymessage">
            <tr>
              <td colspan="5" style="text-align: center; padding: 20px; color: var(--color-text-muted);">
                <i class="pi pi-check text-success" style="margin-right: 4px;"></i> {{ 'reports.noUnpaid' | t }}
              </td>
            </tr>
          </ng-template>
        </p-table>
      </div>
    </div>

  </div>
</div>

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\settings\settings.component.html
```html
<div class="page">
  <!-- Header -->
  <div class="page-header">
    <div>
      <h2 class="page-title">{{ 'settings.pageTitle' | t }}</h2>
      <p class="page-desc">{{ 'settings.pageDesc' | t }}</p>
    </div>
  </div>

  <!-- Notification Banner -->
  <div *ngIf="message" 
       [class.success]="messageType === 'success'"
       [class.error]="messageType === 'error'"
       class="notification-banner"
  >
    <i class="pi" [class.pi-check-circle]="messageType === 'success'" [class.pi-exclamation-circle]="messageType === 'error'"></i>
    <span>{{ message }}</span>
  </div>

  <!-- Content Grid -->
  <div class="settings-layout">
    <!-- Left Column -->
    <div class="settings-left">
      
      <!-- System Thresholds -->
      <div class="card settings-card">
        <div class="settings-card-header">
          <div class="settings-card-icon icon-accent">
            <i class="pi pi-sliders-h"></i>
          </div>
          <div>
            <h3 class="settings-card-title">{{ 'settings.thresholdsConfig' | t }}</h3>
            <p class="settings-card-desc">{{ 'settings.thresholdsDesc' | t }}</p>
          </div>
        </div>

        <div class="settings-card-body">
          <div class="settings-form-grid">
            <div class="form-group">
              <label class="form-label">{{ 'settings.currencySymbol' | t }}</label>
              <input type="text" [(ngModel)]="settings.currencySymbol" class="form-input" />
            </div>
            <div class="form-group">
              <label class="form-label">{{ 'settings.dateFormat' | t }}</label>
              <input type="text" [(ngModel)]="settings.dateFormat" class="form-input" />
            </div>
            <div class="form-group">
              <label class="form-label">{{ 'settings.kmInactivity' | t }}</label>
              <input type="number" [(ngModel)]="settings.kmInactivityDaysThreshold" class="form-input" />
            </div>
            <div class="form-group">
              <label class="form-label">{{ 'settings.maintenanceAlert' | t }}</label>
              <input type="number" [(ngModel)]="settings.maintenanceNotifyDaysBefore" class="form-input" />
            </div>
            <div class="form-group">
              <label class="form-label">{{ 'settings.insuranceThresholds' | t }}</label>
              <input type="text" [(ngModel)]="settings.insuranceExpiryDaysThresholds" placeholder="30,15,7" class="form-input" />
            </div>
            <div class="form-group">
              <label class="form-label">{{ 'settings.inspectionThresholds' | t }}</label>
              <input type="text" [(ngModel)]="settings.inspectionExpiryDaysThresholds" placeholder="30,15,7" class="form-input" />
            </div>
            <div class="form-group">
              <label class="form-label">{{ 'settings.consumableAlertKm' | t }}</label>
              <input type="number" [(ngModel)]="settings.consumableNotifyKmBefore" class="form-input" />
            </div>
            <div class="form-group">
              <label class="form-label">{{ 'settings.consumableAlertDays' | t }}</label>
              <input type="number" [(ngModel)]="settings.consumableNotifyDaysBefore" class="form-input" />
            </div>
          </div>

          <div class="settings-form-actions">
            <button (click)="saveSettings()" class="btn-primary">
              <i class="pi pi-check"></i>
              {{ 'settings.saveConfig' | t }}
            </button>
          </div>
        </div>
      </div>

      <!-- Reference Data Management â€” Accordion Style -->
      <div class="card settings-card">
        <div class="settings-card-header">
          <div class="settings-card-icon icon-teal">
            <i class="pi pi-list"></i>
          </div>
          <div>
            <h3 class="settings-card-title">{{ 'settings.referenceData' | t }}</h3>
            <p class="settings-card-desc">{{ 'settings.referenceDataDesc' | t }}</p>
          </div>
        </div>

        <div class="settings-card-body ref-body">
          <!-- Vehicle Categories -->
          <div class="ref-section" [class.ref-open]="openRefSection === 'vehicles'">
            <button class="ref-section-header" (click)="toggleRefSection('vehicles')">
              <div class="ref-section-left">
                <i class="pi pi-car ref-section-icon"></i>
                <div>
                  <span class="ref-section-title">{{ 'settings.vehicleCategories' | t }}</span>
                  <span class="ref-section-count">{{ vehicleTypes.length }} {{ 'settings.itemsConfigured' | t }}</span>
                </div>
              </div>
              <i class="pi pi-chevron-down ref-chevron"></i>
            </button>
            <div class="ref-section-body" *ngIf="openRefSection === 'vehicles'">
              <p class="ref-section-desc">{{ 'settings.vehicleCategoriesDesc' | t }}</p>
              <div class="tag-container">
                <span *ngFor="let type of vehicleTypes; let idx = index" class="catalog-tag">
                  {{ type }}
                  <button (click)="removeVehicleType(idx)" class="tag-remove-btn" title="Remove">
                    <i class="pi pi-times"></i>
                  </button>
                </span>
                <span *ngIf="vehicleTypes.length === 0" class="ref-empty">{{ 'settings.noItems' | t }}</span>
              </div>
              <div class="tag-add-row">
                <input type="text" [(ngModel)]="newVehicleType" [placeholder]="'settings.addCategory' | t" class="form-input" (keyup.enter)="addVehicleType()" />
                <button (click)="addVehicleType()" class="btn-primary btn-sm">
                  <i class="pi pi-plus"></i> {{ 'common.add' | t }}
                </button>
              </div>
            </div>
          </div>

          <!-- Fuel Types -->
          <div class="ref-section" [class.ref-open]="openRefSection === 'fuel'">
            <button class="ref-section-header" (click)="toggleRefSection('fuel')">
              <div class="ref-section-left">
                <i class="pi pi-bolt ref-section-icon"></i>
                <div>
                  <span class="ref-section-title">{{ 'settings.fuelTypes' | t }}</span>
                  <span class="ref-section-count">{{ fuelTypes.length }} {{ 'settings.itemsConfigured' | t }}</span>
                </div>
              </div>
              <i class="pi pi-chevron-down ref-chevron"></i>
            </button>
            <div class="ref-section-body" *ngIf="openRefSection === 'fuel'">
              <p class="ref-section-desc">{{ 'settings.fuelTypesDesc' | t }}</p>
              <div class="tag-container">
                <span *ngFor="let type of fuelTypes; let idx = index" class="catalog-tag">
                  {{ type }}
                  <button (click)="removeFuelType(idx)" class="tag-remove-btn" title="Remove">
                    <i class="pi pi-times"></i>
                  </button>
                </span>
                <span *ngIf="fuelTypes.length === 0" class="ref-empty">{{ 'settings.noItems' | t }}</span>
              </div>
              <div class="tag-add-row">
                <input type="text" [(ngModel)]="newFuelType" [placeholder]="'settings.addFuel' | t" class="form-input" (keyup.enter)="addFuelType()" />
                <button (click)="addFuelType()" class="btn-primary btn-sm">
                  <i class="pi pi-plus"></i> {{ 'common.add' | t }}
                </button>
              </div>
            </div>
          </div>

          <!-- Maintenance Types -->
          <div class="ref-section" [class.ref-open]="openRefSection === 'maintenance'">
            <button class="ref-section-header" (click)="toggleRefSection('maintenance')">
              <div class="ref-section-left">
                <i class="pi pi-wrench ref-section-icon"></i>
                <div>
                  <span class="ref-section-title">{{ 'settings.maintenanceTypes' | t }}</span>
                  <span class="ref-section-count">{{ maintenanceTypes.length }} {{ 'settings.itemsConfigured' | t }}</span>
                </div>
              </div>
              <i class="pi pi-chevron-down ref-chevron"></i>
            </button>
            <div class="ref-section-body" *ngIf="openRefSection === 'maintenance'">
              <p class="ref-section-desc">{{ 'settings.maintenanceTypesDesc' | t }}</p>
              <div class="tag-container">
                <span *ngFor="let type of maintenanceTypes; let idx = index" class="catalog-tag">
                  {{ i18n.translateMaintenanceType(type) }}
                  <button (click)="removeMaintenanceType(idx)" class="tag-remove-btn" title="Remove">
                    <i class="pi pi-times"></i>
                  </button>
                </span>
                <span *ngIf="maintenanceTypes.length === 0" class="ref-empty">{{ 'settings.noItems' | t }}</span>
              </div>
              <div class="tag-add-row">
                <input type="text" [(ngModel)]="newMaintenanceType" [placeholder]="'settings.addIntervention' | t" class="form-input" (keyup.enter)="addMaintenanceType()" />
                <button (click)="addMaintenanceType()" class="btn-primary btn-sm">
                  <i class="pi pi-plus"></i> {{ 'common.add' | t }}
                </button>
              </div>
            </div>
          </div>

          <!-- Extras Catalog -->
          <div class="ref-section" [class.ref-open]="openRefSection === 'extras'">
            <button class="ref-section-header" (click)="toggleRefSection('extras')">
              <div class="ref-section-left">
                <i class="pi pi-box ref-section-icon"></i>
                <div>
                  <span class="ref-section-title">{{ 'settings.extrasCatalog' | t }}</span>
                  <span class="ref-section-count">{{ extras.length }} {{ 'settings.itemsConfigured' | t }}</span>
                </div>
              </div>
              <i class="pi pi-chevron-down ref-chevron"></i>
            </button>
            <div class="ref-section-body" *ngIf="openRefSection === 'extras'">
              <p class="ref-section-desc">{{ 'settings.extrasCatalogDesc' | t }}</p>
              <div class="extras-grid">
                <div *ngFor="let item of extras; let idx = index" class="extra-item">
                  <span class="extra-name">{{ item.Name }}</span>
                  <div class="extra-price-box">
                    <span class="extra-price">{{ item.Price }} â‚¬{{ 'common.perDay' | t }}</span>
                    <button (click)="removeExtra(idx)" class="tag-remove-btn" title="Remove">
                      <i class="pi pi-trash"></i>
                    </button>
                  </div>
                </div>
                <span *ngIf="extras.length === 0" class="ref-empty">{{ 'settings.noItems' | t }}</span>
              </div>
              <div class="extra-add-row">
                <input type="text" [(ngModel)]="newExtra.name" [placeholder]="'settings.optionName' | t" class="form-input" />
                <input type="number" [(ngModel)]="newExtra.price" [placeholder]="'settings.optionRate' | t" class="form-input" style="max-width: 120px;" />
                <button (click)="addExtra()" class="btn-primary btn-sm">
                  <i class="pi pi-plus"></i> {{ 'common.add' | t }}
                </button>
              </div>
            </div>
          </div>

          <div class="ref-save-row">
            <button (click)="saveSettings()" class="btn-primary">
              <i class="pi pi-check"></i>
              {{ 'settings.saveRefData' | t }}
            </button>
          </div>
        </div>
      </div>

    </div>

    <!-- Right Column: Admin Profile and Password updates -->
    <div class="settings-right">
      
      <!-- Profile Information -->
      <div class="card settings-card">
        <div class="settings-card-header">
          <div class="settings-card-icon icon-emerald">
            <i class="pi pi-user"></i>
          </div>
          <div>
            <h3 class="settings-card-title">{{ 'settings.adminProfile' | t }}</h3>
            <p class="settings-card-desc">{{ 'settings.adminProfileDesc' | t }}</p>
          </div>
        </div>
        
        <div class="settings-card-body">
          <div class="profile-stack">
            <div class="form-group">
              <label class="form-label">{{ 'settings.usernameReadOnly' | t }}</label>
              <input type="text" [value]="profile.username" disabled class="form-input disabled-input" />
            </div>
            <div class="form-group">
              <label class="form-label">{{ 'settings.fullName' | t }}</label>
              <input type="text" [(ngModel)]="profile.fullName" class="form-input" />
            </div>
            <button (click)="updateProfile()" class="btn-primary full-width">
              <i class="pi pi-check"></i>
              {{ 'settings.updateProfile' | t }}
            </button>
          </div>
        </div>
      </div>

      <!-- Security / Change Password -->
      <div class="card settings-card">
        <div class="settings-card-header">
          <div class="settings-card-icon icon-amber">
            <i class="pi pi-key"></i>
          </div>
          <div>
            <h3 class="settings-card-title">{{ 'settings.security' | t }}</h3>
            <p class="settings-card-desc">{{ 'settings.securityDesc' | t }}</p>
          </div>
        </div>
        
        <div class="settings-card-body">
          <div class="profile-stack">
            <div class="form-group">
              <label class="form-label">{{ 'settings.currentPassword' | t }}</label>
              <input type="password" [(ngModel)]="passwordForm.currentPassword" class="form-input" />
            </div>
            <div class="form-group">
              <label class="form-label">{{ 'settings.newPassword' | t }}</label>
              <input type="password" [(ngModel)]="passwordForm.newPassword" class="form-input" />
            </div>
            <div class="form-group">
              <label class="form-label">{{ 'settings.confirmPassword' | t }}</label>
              <input type="password" [(ngModel)]="passwordForm.confirmPassword" class="form-input" />
            </div>
            <button (click)="updatePassword()" class="btn-secondary full-width">
              <i class="pi pi-lock"></i>
              {{ 'settings.changePassword' | t }}
            </button>
          </div>
        </div>
      </div>

    </div>
  </div>
</div>

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\vehicles\vehicles.component.html
```html
<div class="page">
  <!-- Page Header -->
  <div class="page-header">
    <div>
      <h2 class="page-title">{{ 'vehicles.pageTitle' | t }}</h2>
      <p class="page-desc">{{ 'vehicles.pageDesc' | t }}</p>
    </div>
    <button (click)="openAddDialog()" class="btn-primary">
      <i class="pi pi-plus"></i>
      {{ 'vehicles.addVehicle' | t }}
    </button>
  </div>

  <!-- Filters -->
  <div class="card filter-bar">
    <div class="filter-search">
      <i class="pi pi-search filter-search-icon"></i>
      <input 
        type="text" 
        [(ngModel)]="searchQuery" 
        (ngModelChange)="onFilterChange()"
        [placeholder]="'vehicles.searchPlaceholder' | t"
        class="form-input has-icon"
      />
    </div>
    <select [(ngModel)]="filterType" (change)="onFilterChange()" class="form-input filter-select">
      <option value="">{{ 'vehicles.allTypes' | t }}</option>
      <option *ngFor="let t of vehicleTypes" [value]="t">{{ t }}</option>
    </select>
    <select [(ngModel)]="filterFuelType" (change)="onFilterChange()" class="form-input filter-select">
      <option value="">{{ 'vehicles.allFuels' | t }}</option>
      <option *ngFor="let f of fuelTypes" [value]="f">{{ f }}</option>
    </select>
    <select [(ngModel)]="filterStatus" (change)="onFilterChange()" class="form-input filter-select">
      <option value="">{{ 'vehicles.allStatuses' | t }}</option>
      <option *ngFor="let s of statuses" [value]="s.value">{{ 'statuses.' + (s.value === 'InMaintenance' ? 'inMaintenance' : s.value.toLowerCase()) | t }}</option>
    </select>
  </div>

  <!-- Empty State -->
  <div *ngIf="vehicles.length === 0" class="card empty-state">
    <i class="pi pi-car empty-icon"></i>
    <p>{{ 'vehicles.noVehiclesFound' | t }}</p>
  </div>

  <!-- Vehicle Grid -->
  <div *ngIf="vehicles.length > 0" class="vehicle-grid">
    <div *ngFor="let vehicle of vehicles" class="card vehicle-card">
      <!-- Image -->
      <div class="vehicle-img">
        <img *ngIf="vehicle.photoPath" [src]="'http://localhost:5222' + vehicle.photoPath" [alt]="vehicle.brand" />
        <div *ngIf="!vehicle.photoPath" class="vehicle-img-placeholder">
          <i class="pi pi-image"></i>
        </div>
        <span class="vehicle-status-badge"
          [class.badge-success]="vehicle.status === 'Available'"
          [class.badge-accent]="vehicle.status === 'Rented'"
          [class.badge-warning]="vehicle.status === 'InMaintenance'"
          [class.badge-info]="vehicle.status === 'Reserved'"
          [class.badge-danger]="vehicle.status === 'Immobilized'"
        >
          {{ vehicle.status === 'Available' ? ('vehicles.statusAvailable' | t) : 
             vehicle.status === 'Rented' ? ('vehicles.statusRented' | t) : 
             vehicle.status === 'InMaintenance' ? ('vehicles.statusMaintenance' | t) : 
             vehicle.status === 'Reserved' ? ('vehicles.statusReserved' | t) : ('vehicles.statusImmobilized' | t) }}
        </span>
        <span class="vehicle-rate">{{ vehicle.dailyRate | appCurrency }}{{ 'common.perDay' | t }}</span>
      </div>

      <!-- Details -->
      <div class="vehicle-body">
        <div class="vehicle-top">
          <h4 class="vehicle-name">{{ vehicle.brand }} {{ vehicle.model }}</h4>
          <span class="badge badge-neutral">{{ vehicle.year }}</span>
        </div>
        <p class="vehicle-plate">{{ vehicle.matricule }}</p>

        <div class="vehicle-specs">
          <div class="spec-item">
            <span class="spec-label">{{ 'vehicles.engine' | t }}</span>
            <span class="spec-value">{{ vehicle.fuelType }}</span>
          </div>
          <div class="spec-item">
            <span class="spec-label">{{ 'vehicles.gearbox' | t }}</span>
            <span class="spec-value">{{ vehicle.transmission === 'Manual' ? ('vehicles.manual' | t) : ('vehicles.auto' | t) }}</span>
          </div>
          <div class="spec-item">
            <span class="spec-label">Km</span>
            <span class="spec-value">{{ vehicle.currentKm }}</span>
          </div>
        </div>

        <div class="vehicle-actions">
          <button (click)="openDetails(vehicle)" class="btn-link">
            <i class="pi pi-folder-open"></i> {{ 'vehicles.folder' | t }}
          </button>
          <div class="vehicle-actions-right">
            <button (click)="openEditDialog(vehicle)" class="btn-icon"><i class="pi pi-pencil"></i></button>
            <button (click)="deleteVehicle(vehicle.id)" class="btn-icon danger"><i class="pi pi-trash"></i></button>
          </div>
        </div>
      </div>
    </div>
  </div>

  <!-- Pagination -->
  <div *ngIf="totalCount > pageSize" class="pagination">
    <button [disabled]="page === 1" (click)="onPageChange(page - 1)" class="btn-icon">
      <i class="pi pi-chevron-left"></i>
    </button>
    <button 
      *ngFor="let p of pages" 
      (click)="onPageChange(p)"
      class="page-btn"
      [class.active]="p === page"
    >{{ p }}</button>
    <button [disabled]="page * pageSize >= totalCount" (click)="onPageChange(page + 1)" class="btn-icon">
      <i class="pi pi-chevron-right"></i>
    </button>
  </div>

  <!-- DETAILS MODAL -->
  <p-dialog 
    [(visible)]="showDetailsDialog" 
    [style]="{width: '72vw'}" 
    [modal]="true" 
    [header]="selectedVehicle ? ('vehicles.tracking' | t) + ': ' + selectedVehicle.brand + ' ' + selectedVehicle.model + ' (' + selectedVehicle.matricule + ')' : ''"
    styleClass="p-fluid"
    [draggable]="false"
    [resizable]="false"
  >
    <div class="details-layout">
      <!-- Tab selector -->
      <div class="details-tabs">
        <button (click)="switchDetailsTab('consumables')" [class.active]="detailsTab === 'consumables'" class="details-tab">
          <i class="pi pi-wrench"></i> {{ 'vehicles.consumablesTab' | t }}
        </button>
        <button (click)="switchDetailsTab('insurance')" [class.active]="detailsTab === 'insurance'" class="details-tab">
          <i class="pi pi-shield"></i> {{ 'vehicles.insuranceTab' | t }}
        </button>
        <button (click)="switchDetailsTab('inspections')" [class.active]="detailsTab === 'inspections'" class="details-tab">
          <i class="pi pi-exclamation-triangle"></i> {{ 'vehicles.inspectionsTab' | t }}
        </button>
        <button (click)="switchDetailsTab('fuel')" [class.active]="detailsTab === 'fuel'" class="details-tab">
          <i class="pi pi-filter"></i> {{ 'vehicles.fuelTab' | t }}
        </button>
        <button (click)="switchDetailsTab('km_history')" [class.active]="detailsTab === 'km_history'" class="details-tab">
          <i class="pi pi-map-marker"></i> {{ 'vehicles.kmHistoryTab' | t }}
        </button>
      </div>

      <!-- Tab Content -->
      <div class="details-content">
        <!-- Consumables -->
        <div *ngIf="detailsTab === 'consumables'">
          <div class="section-header">
            <h5 class="section-title">{{ 'vehicles.consumablesState' | t }}</h5>
          </div>
          <table class="data-table">
            <thead>
              <tr>
                <th>{{ 'vehicles.consumable' | t }}</th>
                <th>{{ 'vehicles.lastReplacement' | t }}</th>
                <th>{{ 'vehicles.traveled' | t }}</th>
                <th>{{ 'vehicles.interval' | t }}</th>
                <th>{{ 'common.status' | t }}</th>
              </tr>
            </thead>
            <tbody>
              <tr *ngFor="let item of consumablesStatus">
                <td class="font-semibold">{{ i18n.translateAlertMessage(item.consumableType) }}</td>
                <td>
                  <span *ngIf="item.lastReplacementDate">{{ item.lastReplacementDate | date:'dd/MM/yyyy' }} ({{ item.lastReplacementKm }} km)</span>
                  <span *ngIf="!item.lastReplacementDate" class="text-muted" style="font-style:italic">{{ 'vehicles.noLog' | t }}</span>
                </td>
                <td>{{ item.kmSinceReplacement }} km / {{ item.monthsSinceReplacement }} {{ 'common.months' | t }}</td>
                <td class="text-secondary">{{ item.intervalKm > 0 ? item.intervalKm + ' km' : '' }}{{ item.intervalMonths > 0 ? ' / ' + item.intervalMonths + ' m' : '' }}</td>
                <td>
                  <span class="badge"
                    [class.badge-success]="item.status === 'OK'"
                    [class.badge-warning]="item.status === 'Warning'"
                    [class.badge-danger]="item.status === 'Due'"
                  >{{ 'statuses.' + (item.status === 'Warning' ? 'warningStatus' : item.status.toLowerCase()) | t }}</span>
                </td>
              </tr>
            </tbody>
          </table>
        </div>

        <!-- Insurance -->
        <div *ngIf="detailsTab === 'insurance'">
          <div class="section-header">
            <h5 class="section-title">{{ 'vehicles.insuranceHistory' | t }}</h5>
            <button (click)="openAddPolicy()" class="btn-primary btn-sm"><i class="pi pi-plus"></i> {{ 'common.add' | t }}</button>
          </div>
          <table class="data-table">
            <thead>
              <tr>
                <th>{{ 'vehicles.insurer' | t }}</th>
                <th>{{ 'vehicles.policyNo' | t }}</th>
                <th>{{ 'vehicles.coverage' | t }}</th>
                <th>{{ 'vehicles.validity' | t }}</th>
                <th>{{ 'vehicles.premiumValue' | t }}</th>
                <th>{{ 'common.status' | t }}</th>
              </tr>
            </thead>
            <tbody>
              <tr *ngFor="let item of insurancePolicies">
                <td class="font-semibold">{{ item.policy.insurerName }}</td>
                <td class="text-secondary">{{ item.policy.policyNumber }}</td>
                <td>{{ 'vehicles.coverage' + item.policy.coverageType.replace('-', '') | t }}</td>
                <td class="text-secondary">{{ item.policy.startDate | date:'dd/MM/yy' }} â†’ {{ item.policy.expiryDate | date:'dd/MM/yy' }}</td>
                <td>{{ item.policy.premiumAmount | appCurrency }} / {{ item.policy.insuredValue | appCurrency }}</td>
                <td>
                  <span class="badge"
                    [class.badge-success]="item.status === 'Valid'"
                    [class.badge-warning]="item.status === 'ExpiringSoon'"
                    [class.badge-danger]="item.status === 'Expired'"
                  >{{ 'statuses.' + (item.status === 'ExpiringSoon' ? 'expiringSoon' : item.status.toLowerCase()) | t }}</span>
                </td>
              </tr>
            </tbody>
          </table>
        </div>

        <!-- Inspections -->
        <div *ngIf="detailsTab === 'inspections'">
          <div class="section-header">
            <h5 class="section-title">{{ 'vehicles.technicalInspections' | t }}</h5>
            <button (click)="openAddInspection()" class="btn-primary btn-sm"><i class="pi pi-plus"></i> {{ 'common.add' | t }}</button>
          </div>
          <table class="data-table">
            <thead>
              <tr>
                <th>{{ 'common.date' | t }}</th>
                <th>{{ 'vehicles.expiration' | t }}</th>
                <th>{{ 'vehicles.center' | t }}</th>
                <th>{{ 'vehicles.result' | t }}</th>
                <th>{{ 'common.cost' | t }}</th>
                <th>{{ 'common.status' | t }}</th>
              </tr>
            </thead>
            <tbody>
              <tr *ngFor="let item of inspectionsList">
                <td class="font-semibold">{{ item.inspection.inspectionDate | date:'dd/MM/yyyy' }}</td>
                <td class="text-secondary">{{ item.inspection.expiryDate | date:'dd/MM/yyyy' }}</td>
                <td>{{ item.inspection.centerName }}</td>
                <td>
                  <span class="font-semibold"
                    [class.text-success]="item.inspection.result === 'Pass'"
                    [class.text-danger]="item.inspection.result === 'Fail'"
                  >{{ 'vehicles.result' + item.inspection.result | t }}</span>
                </td>
                <td>{{ item.inspection.cost | appCurrency }}</td>
                <td>
                  <span class="badge"
                    [class.badge-success]="item.status === 'Valid'"
                    [class.badge-warning]="item.status === 'ExpiringSoon'"
                    [class.badge-danger]="item.status === 'Expired' || item.status === 'Failed'"
                  >{{ 'statuses.' + (item.status === 'ExpiringSoon' ? 'expiringSoon' : item.status.toLowerCase()) | t }}</span>
                </td>
              </tr>
            </tbody>
          </table>
        </div>

        <!-- Fuel -->
        <div *ngIf="detailsTab === 'fuel'">
          <div class="section-header">
            <h5 class="section-title">{{ 'vehicles.fuelTracking' | t }}</h5>
            <div class="section-actions">
              <button (click)="exportFuelCsv()" class="btn-secondary btn-sm"><i class="pi pi-download"></i> CSV</button>
              <button (click)="openAddFuel()" class="btn-primary btn-sm"><i class="pi pi-plus"></i> {{ 'vehicles.fillup' | t }}</button>
            </div>
          </div>
          <table class="data-table">
            <thead>
              <tr>
                <th>{{ 'common.date' | t }}</th>
                <th>{{ 'vehicles.counter' | t }}</th>
                <th>{{ 'vehicles.volume' | t }}</th>
                <th>{{ 'common.cost' | t }}</th>
                <th>{{ 'vehicles.consumption' | t }}</th>
                <th>{{ 'vehicles.anomaly' | t }}</th>
              </tr>
            </thead>
            <tbody>
              <tr *ngFor="let log of fuelLogsList">
                <td class="font-semibold">{{ log.log.date | date:'dd/MM/yyyy' }}</td>
                <td class="text-secondary">{{ log.log.kmValue }} km</td>
                <td>{{ log.log.liters }} L ({{ log.log.costPerLiter }} {{ i18n.currentLang() === 'ar' ? 'Ø¯.Ø¬' : 'DZD' }}/L)</td>
                <td class="font-semibold">{{ log.log.totalCost | appCurrency }}</td>
                <td class="text-accent">
                  <span *ngIf="log.consumptionL100 > 0">{{ log.consumptionL100 }} L/100km</span>
                  <span *ngIf="log.consumptionL100 === 0" class="text-muted" style="font-style:italic">â€”</span>
                </td>
                <td>
                  <span *ngIf="log.log.isAnomaly" class="badge badge-danger">{{ 'vehicles.overConsumption' | t }}</span>
                  <span *ngIf="!log.log.isAnomaly" class="text-muted">â€”</span>
                </td>
              </tr>
            </tbody>
          </table>
        </div>

        <!-- Km History -->
        <div *ngIf="detailsTab === 'km_history'">
          <div class="section-header">
            <h5 class="section-title">{{ 'vehicles.kmHistory' | t }}</h5>
            <div class="section-actions">
              <button (click)="exportKmCsv()" class="btn-secondary btn-sm"><i class="pi pi-download"></i> CSV</button>
              <button (click)="openAddKm()" class="btn-primary btn-sm"><i class="pi pi-plus"></i> {{ 'vehicles.reading' | t }}</button>
            </div>
          </div>
          <div class="km-timeline">
            <div *ngFor="let item of kmHistoryList" class="km-entry">
              <span class="km-dot"
                [class.dot-success]="item.source === 'Manual'"
                [class.dot-accent]="item.source === 'ContractStart'"
                [class.dot-info]="item.source === 'ContractReturn'"
              ></span>
              <div class="km-entry-content">
                <span class="km-entry-date">{{ item.date | date:'dd/MM/yyyy HH:mm' }}</span>
                <span class="km-entry-value">{{ item.kmValue }} km</span>
                <span class="km-entry-notes">{{ item.notes }}</span>
                <span class="km-entry-source badge badge-neutral">
                  {{ item.source === 'Manual' ? ('fuel.sourceManual' | t) : item.source === 'FuelFillup' ? ('fuel.sourceFuel' | t) : ('fuel.sourceContract' | t) }}
                </span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </p-dialog>

  <!-- ADD/EDIT VEHICLE DIALOG -->
  <p-dialog 
    [(visible)]="showCrudDialog" 
    [style]="{width: '900px'}" 
    [modal]="true" 
    [header]="isEditMode ? ('vehicles.editVehicle' | t) : ('vehicles.newVehicle' | t)"
    styleClass="p-fluid"
    [draggable]="false"
    [resizable]="false"
  >
    <form (ngSubmit)="onSubmitVehicle()" style="display: grid; grid-template-columns: 1fr 1fr; gap: 24px;">
      
      <!-- LEFT COLUMN -->
      <div style="display: flex; flex-direction: column; gap: 16px;">
        <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 12px;">
          <div class="form-group">
            <label class="form-label required">{{ 'vehicles.brand' | t }}</label>
            <input type="text" [(ngModel)]="vehicleForm.brand" name="brand" required class="form-input" placeholder="Renault"/>
          </div>
          <div class="form-group">
            <label class="form-label required">{{ 'vehicles.model' | t }}</label>
            <input type="text" [(ngModel)]="vehicleForm.model" name="model" required class="form-input" placeholder="Clio 5"/>
          </div>
        </div>
        
        <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 12px;">
          <div class="form-group">
            <label class="form-label required">{{ 'vehicles.matricule' | t }}</label>
            <input type="text" [(ngModel)]="vehicleForm.matricule" name="matricule" required class="form-input" placeholder="12345-120-16"/>
          </div>
          <div class="form-group">
            <label class="form-label required">{{ 'vehicles.vin' | t }}</label>
            <input type="text" [(ngModel)]="vehicleForm.vin" name="vin" required class="form-input" placeholder="VF123456..."/>
          </div>
        </div>
        
        <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 12px;">
          <div class="form-group">
            <label class="form-label required">{{ 'vehicles.year' | t }}</label>
            <input type="number" [(ngModel)]="vehicleForm.year" name="year" required class="form-input"/>
          </div>
          <div class="form-group">
            <label class="form-label">{{ 'vehicles.color' | t }}</label>
            <input type="text" [(ngModel)]="vehicleForm.color" name="color" class="form-input"/>
          </div>
        </div>
        
        <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 12px;">
          <div class="form-group">
            <label class="form-label required">{{ 'common.type' | t }}</label>
            <select [(ngModel)]="vehicleForm.type" name="type" required class="form-input">
              <option *ngFor="let t of vehicleTypes" [value]="t">{{ t }}</option>
            </select>
          </div>
          <div class="form-group">
            <label class="form-label required">{{ 'vehicles.seats' | t }}</label>
            <input type="number" [(ngModel)]="vehicleForm.seatsCount" name="seatsCount" required class="form-input"/>
          </div>
        </div>
        
        <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 12px;">
          <div class="form-group">
            <label class="form-label required">{{ 'vehicles.fuelType' | t }}</label>
            <select [(ngModel)]="vehicleForm.fuelType" name="fuelType" required class="form-input">
              <option *ngFor="let f of fuelTypes" [value]="f">{{ f }}</option>
            </select>
          </div>
          <div class="form-group">
            <label class="form-label required">{{ 'vehicles.transmission' | t }}</label>
            <select [(ngModel)]="vehicleForm.transmission" name="transmission" required class="form-input">
              <option *ngFor="let t of transmissions" [value]="t">{{ t }}</option>
            </select>
          </div>
        </div>

      </div>

      <!-- RIGHT COLUMN -->
      <div style="display: flex; flex-direction: column; gap: 16px; background: #f8fafc; padding: 20px; border-radius: 12px; border: 1px solid var(--color-border-light);">
        
        <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 12px;">
          <div class="form-group">
            <label class="form-label required">{{ 'vehicles.dailyRate' | t }}</label>
            <input type="number" [(ngModel)]="vehicleForm.dailyRate" name="dailyRate" required class="form-input"/>
          </div>
          <div class="form-group">
            <label class="form-label required">{{ 'vehicles.purchasePrice' | t }}</label>
            <input type="number" [(ngModel)]="vehicleForm.purchasePrice" name="purchasePrice" required class="form-input"/>
          </div>
        </div>

        <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 12px;">
          <div class="form-group">
            <label class="form-label">{{ 'vehicles.engineNumber' | t }}</label>
            <input type="text" [(ngModel)]="vehicleForm.engineNumber" name="engineNumber" class="form-input"/>
          </div>
          <div class="form-group">
            <label class="form-label required">{{ 'vehicles.initialKm' | t }}</label>
            <input type="number" [(ngModel)]="vehicleForm.initialKm" name="initialKm" [disabled]="isEditMode" required class="form-input"/>
          </div>
        </div>

        <div class="form-group photo-upload-group">
          <label class="form-label">{{ 'common.photo' | t }}</label>
          <label class="photo-dropzone" [class.has-image]="!!vehicleForm.photoPath">
            <input type="file" (change)="onPhotoUpload($event)" class="photo-input" accept="image/*"/>
            
            <div *ngIf="vehicleForm.photoPath && !uploadingPhoto" class="photo-preview-container">
              <img [src]="'http://localhost:5222' + vehicleForm.photoPath" alt="Preview" class="photo-preview-img" />
              <div class="photo-preview-overlay">
                <i class="pi pi-camera" style="font-size: 24px;"></i>
                <span>{{ 'common.edit' | t }}</span>
              </div>
            </div>

            <div *ngIf="!vehicleForm.photoPath && !uploadingPhoto" class="photo-empty-state">
              <i class="pi pi-cloud-upload upload-icon"></i>
              <span class="upload-text">Cliquez pour ajouter</span>
              <span class="upload-hint">PNG, JPG (Max 5MB)</span>
            </div>

            <div *ngIf="uploadingPhoto" class="photo-uploading-state">
              <i class="pi pi-spin pi-spinner spinner-icon"></i>
              <span>{{ 'common.uploadInProgress' | t }}</span>
            </div>
          </label>
        </div>

      </div>

      <!-- REMARKS (FULL WIDTH) -->
      <div class="form-group" style="grid-column: 1 / -1;">
        <label class="form-label">{{ 'vehicles.remarks' | t }}</label>
        <textarea [(ngModel)]="vehicleForm.notes" name="notes" class="form-input" style="min-height: 80px; resize: vertical;"></textarea>
      </div>

      <!-- FORM ACTIONS -->
      <div style="grid-column: 1 / -1; margin-top: 8px; border-top: 1px solid var(--color-border-light); padding-top: 16px; display: flex; justify-content: flex-end; gap: 8px;">
        <button type="button" (click)="showCrudDialog = false" class="btn-secondary">{{ 'common.cancel' | t }}</button>
        <button type="submit" class="btn-primary" style="height: 42px; padding: 0 24px; font-size: 14px;">{{ 'common.save' | t }}</button>
      </div>

    </form>
  </p-dialog>

  <!-- ADD INSURANCE DIALOG -->
  <p-dialog [(visible)]="showAddPolicyDialog" [style]="{width: '40vw'}" [modal]="true" [header]="'vehicles.newInsurancePolicy' | t" styleClass="p-fluid" [draggable]="false" [resizable]="false">
    <form (ngSubmit)="submitPolicy()" class="form-stack">
      <div class="form-group">
        <label class="form-label required">{{ 'vehicles.insurer' | t }}</label>
        <input type="text" [(ngModel)]="policyForm.insurerName" name="insurerName" required class="form-input"/>
      </div>
      <div class="form-group">
        <label class="form-label required">{{ 'vehicles.policyNo' | t }}</label>
        <input type="text" [(ngModel)]="policyForm.policyNumber" name="policyNumber" required class="form-input"/>
      </div>
      <div class="form-row">
        <div class="form-group">
          <label class="form-label required">{{ 'vehicles.coverage' | t }}</label>
          <select [(ngModel)]="policyForm.coverageType" name="coverageType" required class="form-input">
            <option value="Third-Party">{{ 'vehicles.coverageThirdParty' | t }}</option>
            <option value="Comprehensive">{{ 'vehicles.coverageComprehensive' | t }}</option>
            <option value="Fleet">{{ 'vehicles.coverageFleet' | t }}</option>
          </select>
        </div>
        <div class="form-group">
          <label class="form-label required">{{ 'vehicles.premium' | t }}</label>
          <input type="number" [(ngModel)]="policyForm.premiumAmount" name="premiumAmount" required class="form-input"/>
        </div>
      </div>
      <div class="form-row">
        <div class="form-group">
          <label class="form-label required">{{ 'vehicles.effectiveDate' | t }}</label>
          <p-datepicker [(ngModel)]="policyForm.startDate" name="startDate" dateFormat="yy-mm-dd" [showIcon]="true" styleClass="w-full"></p-datepicker>
        </div>
        <div class="form-group">
          <label class="form-label required">{{ 'vehicles.expiration' | t }}</label>
          <p-datepicker [(ngModel)]="policyForm.expiryDate" name="expiryDate" dateFormat="yy-mm-dd" [showIcon]="true" styleClass="w-full"></p-datepicker>
        </div>
      </div>
      <div class="form-group">
        <label class="form-label">{{ 'vehicles.documentPdf' | t }}</label>
        <input type="file" (change)="onPolicyFileUpload($event)" class="form-input file-input"/>
        <span *ngIf="uploadingPolicy" class="upload-status">{{ 'common.loading' | t }}</span>
      </div>
      <div class="form-actions">
        <button type="button" (click)="showAddPolicyDialog = false" class="btn-secondary">{{ 'common.cancel' | t }}</button>
        <button type="submit" class="btn-primary">{{ 'common.save' | t }}</button>
      </div>
    </form>
  </p-dialog>

  <!-- ADD INSPECTION DIALOG -->
  <p-dialog [(visible)]="showAddInspectionDialog" [style]="{width: '40vw'}" [modal]="true" [header]="'vehicles.newInspection' | t" styleClass="p-fluid" [draggable]="false" [resizable]="false">
    <form (ngSubmit)="submitInspection()" class="form-stack">
      <div class="form-row">
        <div class="form-group">
          <label class="form-label required">{{ 'vehicles.inspectionDate' | t }}</label>
          <p-datepicker [(ngModel)]="inspectionForm.inspectionDate" name="inspectionDate" dateFormat="yy-mm-dd" [showIcon]="true" styleClass="w-full"></p-datepicker>
        </div>
        <div class="form-group">
          <label class="form-label required">{{ 'vehicles.expiration' | t }}</label>
          <p-datepicker [(ngModel)]="inspectionForm.expiryDate" name="expiryDate" dateFormat="yy-mm-dd" [showIcon]="true" styleClass="w-full"></p-datepicker>
        </div>
      </div>
      <div class="form-row">
        <div class="form-group">
          <label class="form-label required">{{ 'vehicles.result' | t }}</label>
          <select [(ngModel)]="inspectionForm.result" name="result" required class="form-input">
            <option value="Pass">{{ 'vehicles.resultFavorable' | t }}</option>
            <option value="Conditional">{{ 'vehicles.resultCounterVisit' | t }}</option>
            <option value="Fail">{{ 'vehicles.resultUnfavorable' | t }}</option>
          </select>
        </div>
        <div class="form-group">
          <label class="form-label required">{{ 'common.cost' | t }} (DZD)</label>
          <input type="number" [(ngModel)]="inspectionForm.cost" name="cost" required class="form-input"/>
        </div>
      </div>
      <div class="form-group">
        <label class="form-label required">{{ 'vehicles.center' | t }}</label>
        <input type="text" [(ngModel)]="inspectionForm.centerName" name="centerName" required class="form-input"/>
      </div>
      <div class="form-group">
        <label class="form-label">{{ 'vehicles.centerAddress' | t }}</label>
        <input type="text" [(ngModel)]="inspectionForm.centerAddress" name="centerAddress" class="form-input"/>
      </div>
      <div class="form-group">
        <label class="form-label">{{ 'vehicles.remarks' | t }}</label>
        <textarea [(ngModel)]="inspectionForm.remarks" name="remarks" class="form-input"></textarea>
      </div>
      <div class="form-group">
        <label class="form-label">{{ 'vehicles.documentPdf' | t }}</label>
        <input type="file" (change)="onInspectionFileUpload($event)" class="form-input file-input"/>
        <span *ngIf="uploadingInspection" class="upload-status">{{ 'common.loading' | t }}</span>
      </div>
      <div class="form-actions">
        <button type="button" (click)="showAddInspectionDialog = false" class="btn-secondary">{{ 'common.cancel' | t }}</button>
        <button type="submit" class="btn-primary">{{ 'common.save' | t }}</button>
      </div>
    </form>
  </p-dialog>

  <!-- ADD FUEL DIALOG -->
  <p-dialog [(visible)]="showAddFuelDialog" [style]="{width: '36vw'}" [modal]="true" [header]="'vehicles.newFuelFillup' | t" styleClass="p-fluid" [draggable]="false" [resizable]="false">
    <form (ngSubmit)="submitFuel()" class="form-stack">
      <div class="form-row">
        <div class="form-group">
          <label class="form-label required">{{ 'common.date' | t }}</label>
          <p-datepicker [(ngModel)]="fuelForm.date" name="date" dateFormat="yy-mm-dd" [showIcon]="true" styleClass="w-full"></p-datepicker>
        </div>
        <div class="form-group">
          <label class="form-label required">{{ 'vehicles.counterKm' | t }}</label>
          <input type="number" [(ngModel)]="fuelForm.kmValue" name="kmValue" required class="form-input"/>
        </div>
      </div>
      <div class="form-row">
        <div class="form-group">
          <label class="form-label required">{{ 'vehicles.volumeL' | t }}</label>
          <input type="number" [(ngModel)]="fuelForm.liters" name="liters" required class="form-input"/>
        </div>
        <div class="form-group">
          <label class="form-label required">{{ 'vehicles.pricePerL' | t }}</label>
          <input type="number" step="0.001" [(ngModel)]="fuelForm.costPerLiter" name="costPerLiter" required class="form-input"/>
        </div>
      </div>
      <div class="form-group">
        <label class="form-label">{{ 'vehicles.station' | t }}</label>
        <input type="text" [(ngModel)]="fuelForm.stationName" name="stationName" class="form-input"/>
      </div>
      <div class="form-actions">
        <button type="button" (click)="showAddFuelDialog = false" class="btn-secondary">{{ 'common.cancel' | t }}</button>
        <button type="submit" class="btn-primary">{{ 'common.save' | t }}</button>
      </div>
    </form>
  </p-dialog>

  <!-- ADD KM DIALOG -->
  <p-dialog [(visible)]="showAddKmDialog" [style]="{width: '36vw'}" [modal]="true" [header]="'vehicles.kmReading' | t" styleClass="p-fluid" [draggable]="false" [resizable]="false">
    <form (ngSubmit)="submitKm()" class="form-stack">
      <div class="form-group">
        <label class="form-label required">{{ 'common.date' | t }}</label>
        <p-datepicker [(ngModel)]="kmForm.date" name="date" dateFormat="yy-mm-dd" [showIcon]="true" styleClass="w-full"></p-datepicker>
      </div>
      <div class="form-group">
        <label class="form-label required">{{ 'vehicles.counterKm' | t }}</label>
        <input type="number" [(ngModel)]="kmForm.kmValue" name="kmValue" required class="form-input"/>
      </div>
      <div class="form-group">
        <label class="form-label">{{ 'common.notes' | t }}</label>
        <input type="text" [(ngModel)]="kmForm.notes" name="notes" class="form-input"/>
      </div>
      <div class="form-actions">
        <button type="button" (click)="showAddKmDialog = false" class="btn-secondary">{{ 'common.cancel' | t }}</button>
        <button type="submit" class="btn-primary">{{ 'common.validate' | t }}</button>
      </div>
    </form>
  </p-dialog>
</div>

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\styles.css
```css
@import 'tailwindcss';
@import 'primeicons/primeicons.css';

/* â”€â”€ Design Tokens â”€â”€ */
:root {
  --font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
  --color-bg: #FAFAF9;
  --color-surface: #ffffff;
  --color-border: #E7E5E4;
  --color-border-light: #F5F5F4;
  --color-text: #1C1917;
  --color-text-secondary: #78716C;
  --color-text-muted: #A8A29E;
  --color-accent: #0EA5E9;
  --color-accent-hover: #0284C7;
  --color-accent-light: #F0F9FF;
  --color-accent-muted: rgba(14, 165, 233, 0.08);
  --color-danger: #EF4444;
  --color-danger-light: #FEF2F2;
  --color-warning: #F59E0B;
  --color-warning-light: #FFFBEB;
  --color-success: #10B981;
  --color-success-light: #ECFDF5;
  --radius-sm: 6px;
  --radius-md: 10px;
  --radius-lg: 12px;
  --radius-xl: 16px;
  --shadow-xs: 0 1px 2px rgba(0, 0, 0, 0.03);
  --shadow-sm: 0 1px 3px rgba(0, 0, 0, 0.04), 0 1px 2px rgba(0, 0, 0, 0.03);
  --shadow-md: 0 4px 12px rgba(0, 0, 0, 0.05), 0 1px 3px rgba(0, 0, 0, 0.03);
  --shadow-lg: 0 8px 24px rgba(0, 0, 0, 0.06), 0 2px 6px rgba(0, 0, 0, 0.03);
}

/* â”€â”€ Reset & Base â”€â”€ */
*, *::before, *::after {
  box-sizing: border-box;
}

body {
  font-family: var(--font-family);
  background: var(--color-bg);
  color: var(--color-text);
  margin: 0;
  min-height: 100vh;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  font-size: 14px;
  line-height: 1.5;
}

/* â”€â”€ Scrollbar â”€â”€ */
::-webkit-scrollbar {
  width: 6px;
  height: 6px;
}
::-webkit-scrollbar-track {
  background: transparent;
}
::-webkit-scrollbar-thumb {
  background: #D6D3D1;
  border-radius: 3px;
}
::-webkit-scrollbar-thumb:hover {
  background: #A8A29E;
}

/* â”€â”€ Card (shadow-based, no border) â”€â”€ */
.card {
  background: var(--color-surface);
  border-radius: var(--radius-lg);
  box-shadow: var(--shadow-sm);
  border: none;
}

/* â”€â”€ Buttons â”€â”€ */
.btn-primary {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  padding: 8px 18px;
  background: var(--color-accent);
  color: #fff;
  font-size: 13px;
  font-weight: 600;
  border: none;
  border-radius: var(--radius-md);
  cursor: pointer;
  touch-action: manipulation;
  transition: background 0.15s ease, box-shadow 0.15s ease;
}
@media (hover: hover) {
  .btn-primary:hover {
    background: var(--color-accent-hover);
    box-shadow: 0 2px 8px rgba(14, 165, 233, 0.25);
  }
}
.btn-primary:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.btn-secondary {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  padding: 8px 18px;
  background: transparent;
  color: var(--color-text);
  font-size: 13px;
  font-weight: 500;
  border: 1px solid var(--color-border);
  border-radius: var(--radius-md);
  cursor: pointer;
  touch-action: manipulation;
  transition: all 0.15s ease;
}
@media (hover: hover) {
  .btn-secondary:hover {
    background: var(--color-bg);
    border-color: #D6D3D1;
  }
}

.btn-icon {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 34px;
  height: 34px;
  border: none;
  border-radius: var(--radius-md);
  background: transparent;
  color: var(--color-text-secondary);
  cursor: pointer;
  touch-action: manipulation;
  transition: all 0.15s ease;
}
@media (hover: hover) {
  .btn-icon:hover {
    background: var(--color-border-light);
    color: var(--color-text);
  }
  .btn-icon.danger:hover {
    background: var(--color-danger-light);
    color: var(--color-danger);
  }
}

/* â”€â”€ Form Controls (filled pill style) â”€â”€ */
.form-input {
  width: 100%;
  padding: 9px 14px;
  font-size: 13px;
  color: var(--color-text);
  background: #F5F5F4;
  border: 1.5px solid transparent;
  border-radius: var(--radius-md);
  outline: none;
  transition: border-color 0.15s ease, background 0.15s ease, box-shadow 0.15s ease;
}
.form-input:focus {
  background: var(--color-surface);
  border-color: var(--color-accent);
  box-shadow: 0 0 0 3px rgba(14, 165, 233, 0.1);
}
.form-input::placeholder {
  color: var(--color-text-muted);
}

.form-label {
  display: block;
  font-size: 12px;
  font-weight: 600;
  color: var(--color-text-secondary);
  margin-bottom: 5px;
  letter-spacing: 0.01em;
}
.form-label.required::after {
  content: " *";
  color: var(--color-danger);
  font-weight: bold;
}

select.form-input {
  cursor: pointer;
  appearance: auto;
}

textarea.form-input {
  resize: vertical;
  min-height: 72px;
}

/* â”€â”€ Badge / Tag (softer, lowercase-friendly) â”€â”€ */
.badge {
  display: inline-flex;
  align-items: center;
  padding: 3px 10px;
  font-size: 11px;
  font-weight: 600;
  border-radius: 20px;
  letter-spacing: 0.01em;
  line-height: 1.5;
}
.badge-success { background: #ECFDF5; color: #059669; }
.badge-warning { background: #FFFBEB; color: #D97706; }
.badge-danger  { background: #FEF2F2; color: #DC2626; }
.badge-info    { background: #F0F9FF; color: #0EA5E9; }
.badge-neutral { background: #F5F5F4; color: #57534E; }
.badge-accent  { background: var(--color-accent-light); color: var(--color-accent); }

/* â”€â”€ Table (legacy / dialogs) â”€â”€ */
.data-table {
  width: 100%;
  border-collapse: collapse;
  font-size: 13px;
}
.data-table thead th {
  text-align: left;
  padding: 10px 12px;
  font-size: 11px;
  font-weight: 600;
  color: var(--color-text-muted);
  text-transform: uppercase;
  letter-spacing: 0.04em;
  border-bottom: 1px solid var(--color-border);
  background: var(--color-bg);
}
.data-table tbody td {
  padding: 10px 12px;
  border-bottom: 1px solid var(--color-border-light);
  color: var(--color-text);
}
.data-table tbody tr:hover {
  background: #FAFAF9;
}
.data-table tbody tr:last-child td {
  border-bottom: none;
}

/* â”€â”€ Modern list table â”€â”€ */
.table-card {
  overflow: hidden;
  padding: 0 !important;
}

.table-scroll {
  overflow-x: auto;
}

.modern-table {
  width: 100%;
  border-collapse: collapse;
  font-size: 13px;
}

.modern-table thead {
  background: var(--color-bg);
  border-bottom: 1px solid var(--color-border);
}

.modern-table th {
  padding: 12px 16px;
  text-align: left;
  font-size: 11px;
  font-weight: 600;
  color: var(--color-text-muted);
  text-transform: uppercase;
  letter-spacing: 0.05em;
  white-space: nowrap;
}

.modern-table th.col-center,
.modern-table td.col-center {
  text-align: center;
}

.modern-table th.col-right,
.modern-table td.col-right {
  text-align: right;
}

.modern-table tbody tr {
  border-bottom: 1px solid var(--color-border-light);
  transition: background 0.12s ease;
}

.modern-table tbody tr:last-child {
  border-bottom: none;
}

@media (hover: hover) {
  .modern-table tbody tr:hover {
    background: #FAFAF9;
  }
}

.modern-table td {
  padding: 14px 16px;
  color: var(--color-text);
  vertical-align: middle;
}

.modern-table .cell-primary {
  font-weight: 600;
  color: var(--color-text);
}

.modern-table .cell-accent {
  font-weight: 600;
  color: var(--color-accent);
}

.modern-table .cell-muted {
  color: var(--color-text-secondary);
  font-size: 12px;
}

.modern-table .cell-mono {
  font-family: 'JetBrains Mono', 'Fira Code', ui-monospace, monospace;
  font-size: 12px;
  font-weight: 500;
}

.modern-table .cell-actions {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 4px;
  flex-wrap: nowrap;
}

.modern-table .cell-stack {
  display: flex;
  flex-direction: column;
  gap: 2px;
}

.modern-table .cell-stack-title {
  font-weight: 600;
  color: var(--color-text);
}

.modern-table .cell-stack-sub {
  font-size: 12px;
  color: var(--color-text-muted);
}

.table-link {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  color: var(--color-accent);
  font-weight: 600;
  text-decoration: none;
  font-size: 13px;
}
@media (hover: hover) {
  .table-link:hover {
    text-decoration: underline;
  }
}

.table-empty {
  padding: 48px 24px;
  text-align: center;
  color: var(--color-text-muted);
}

.table-empty i {
  font-size: 36px;
  color: #E7E5E4;
  display: block;
  margin-bottom: 12px;
}

.table-empty-title {
  display: block;
  font-weight: 600;
  font-size: 14px;
  color: var(--color-text);
  margin-bottom: 4px;
}

.table-empty-desc {
  font-size: 12px;
  margin: 0;
}

.table-footer {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 12px 16px;
  border-top: 1px solid var(--color-border-light);
  background: var(--color-bg);
  flex-wrap: wrap;
  gap: 8px;
}

.table-footer-info {
  font-size: 12px;
  color: var(--color-text-muted);
}

.table-footer-info strong {
  color: var(--color-text);
  font-weight: 600;
}

.pagination {
  display: flex;
  align-items: center;
  gap: 4px;
}

.page-btn {
  min-width: 32px;
  height: 32px;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 0 8px;
  border: none;
  border-radius: var(--radius-md);
  background: transparent;
  font-size: 12px;
  font-weight: 600;
  cursor: pointer;
  color: var(--color-text-secondary);
  touch-action: manipulation;
  transition: all 0.12s ease;
}
@media (hover: hover) {
  .page-btn:hover:not(:disabled) {
    background: var(--color-border-light);
    color: var(--color-text);
  }
}
.page-btn.active {
  background: var(--color-accent);
  color: #fff;
}
.page-btn:disabled {
  opacity: 0.4;
  cursor: not-allowed;
}

/* Page layout helpers */
.page {
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.page-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  flex-wrap: wrap;
  gap: 12px;
}

.page-title {
  font-size: 20px;
  font-weight: 700;
  color: var(--color-text);
  margin: 0;
  letter-spacing: -0.01em;
}

.page-desc {
  font-size: 13px;
  color: var(--color-text-muted);
  margin: 4px 0 0;
}

.filter-bar {
  padding: 14px 18px;
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
  align-items: center;
}

.filter-search {
  flex: 1;
  min-width: 200px;
  position: relative;
}

.filter-search-icon {
  position: absolute;
  left: 14px;
  top: 50%;
  transform: translateY(-50%);
  color: var(--color-text-muted);
  font-size: 13px;
  pointer-events: none;
}

.filter-search .form-input {
  padding-left: 38px;
}

.filter-select {
  width: 160px;
  min-width: 140px;
}

.header-actions {
  display: flex;
  gap: 8px;
  flex-wrap: wrap;
}

/* â”€â”€ PrimeNG Overrides â”€â”€ */
.p-datatable .p-datatable-thead > tr > th {
  background: var(--color-bg) !important;
  color: var(--color-text-muted) !important;
  font-weight: 600 !important;
  font-size: 11px !important;
  text-transform: uppercase !important;
  letter-spacing: 0.04em !important;
  border-bottom: 1px solid var(--color-border) !important;
  padding: 10px 12px !important;
}
.p-datatable .p-datatable-tbody > tr {
  background: var(--color-surface) !important;
  transition: background 0.1s ease;
}
.p-datatable .p-datatable-tbody > tr:hover {
  background: #FAFAF9 !important;
}
.p-datatable .p-datatable-tbody > tr > td {
  padding: 10px 12px !important;
  border-bottom: 1px solid var(--color-border-light) !important;
  font-size: 13px !important;
}

/* PrimeNG Dialog overrides */
.p-dialog-mask,
.p-component-overlay {
  background-color: rgba(28, 25, 23, 0.25) !important;
  backdrop-filter: blur(4px) !important;
}
.p-dialog {
  background: var(--color-surface) !important;
  border-radius: var(--radius-xl) !important;
  border: none !important;
  box-shadow: var(--shadow-lg), 0 0 0 1px rgba(0,0,0,0.03) !important;
}
.p-dialog .p-dialog-header {
  background: var(--color-surface) !important;
  padding: 18px 24px !important;
  border-bottom: 1px solid var(--color-border-light) !important;
  font-size: 15px !important;
}
.p-dialog .p-dialog-content {
  background: var(--color-surface) !important;
  padding: 24px !important;
}
.p-dialog .p-dialog-footer {
  background: var(--color-surface) !important;
  border-top: 1px solid var(--color-border-light) !important;
  padding: 14px 24px !important;
}

/* PrimeNG Datepicker overrides */
.p-datepicker,
.p-datepicker-panel,
.p-datepicker-header,
.p-datepicker-calendar-container,
.p-datepicker-calendar,
.p-datepicker table,
.p-datepicker-group-container,
.p-datepicker-group,
.p-datepicker-calendar-container table {
  background: var(--color-surface) !important;
  background-color: var(--color-surface) !important;
}
.p-datepicker-title,
.p-datepicker-calendar th,
.p-datepicker-calendar td,
.p-datepicker-calendar td > span {
  background: transparent !important;
}
.p-datepicker-calendar td > span:hover {
  background: var(--color-bg) !important;
}
.p-datepicker-calendar td.p-datepicker-today > span {
  background: var(--color-accent-light) !important;
  color: var(--color-accent) !important;
}
.p-datepicker-calendar td.p-datepicker-today > span.p-highlight {
  background: var(--color-accent) !important;
  color: #fff !important;
}
.p-datepicker-panel {
  border: none !important;
  box-shadow: var(--shadow-lg) !important;
  border-radius: var(--radius-lg) !important;
}

/* â”€â”€ Utility classes â”€â”€ */
.text-accent { color: var(--color-accent); }
.text-muted { color: var(--color-text-muted); }
.text-secondary { color: var(--color-text-secondary); }
.text-danger { color: var(--color-danger); }
.text-success { color: var(--color-success); }
.text-warning { color: var(--color-warning); }
.font-mono { font-family: 'JetBrains Mono', 'Fira Code', monospace; }
.font-semibold { font-weight: 600; }

/* â”€â”€ RTL (Arabic) Global Overrides â”€â”€ */
[dir="rtl"] body {
  text-align: right;
}

[dir="rtl"] .data-table thead th {
  text-align: right;
}

[dir="rtl"] .modern-table th {
  text-align: right;
}

[dir="rtl"] .filter-search-icon {
  left: auto;
  right: 14px;
}

[dir="rtl"] .filter-search .form-input {
  padding-left: 14px;
  padding-right: 38px;
}

[dir="rtl"] .page-header {
  flex-direction: row-reverse;
}

[dir="rtl"] .filter-bar {
  flex-direction: row-reverse;
}

[dir="rtl"] .form-label {
  text-align: right;
}

[dir="rtl"] .cell-actions {
  flex-direction: row-reverse;
}

[dir="rtl"] .pagination {
  flex-direction: row-reverse;
}

[dir="rtl"] .header-actions {
  flex-direction: row-reverse;
}

[dir="rtl"] .btn-primary i,
[dir="rtl"] .btn-secondary i {
  order: 1;
}

/* â”€â”€ Print â”€â”€ */
@media print {
  body {
    background: white;
    color: black;
  }
  .no-print { display: none !important; }
  .card { box-shadow: none !important; border: 1px solid #e5e5e5 !important; }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\app.css
```css

```



# Parc Auto - Frontend (Angular) - Part 3

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\layout\app-layout\app-layout.component.css
```css
/* â”€â”€ App Shell â”€â”€ */
.app-shell {
  display: flex;
  height: 100vh;
  overflow: hidden;
}

/* â”€â”€ Sidebar (White, Expandable) â”€â”€ */
.sidebar {
  width: 240px;
  background: var(--color-surface);
  color: var(--color-text-secondary);
  display: flex;
  flex-direction: column;
  flex-shrink: 0;
  border-right: 1px solid var(--color-border);
  transition: width 0.22s cubic-bezier(0.4, 0, 0.2, 1);
  z-index: 20;
  overflow: hidden;
  will-change: width;
}

.sidebar.collapsed {
  width: 68px;
}

.sidebar-header {
  height: 56px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 16px;
  border-bottom: 1px solid var(--color-border-light);
  flex-shrink: 0;
}

.sidebar-brand {
  display: flex;
  align-items: center;
  gap: 10px;
  overflow: hidden;
  white-space: nowrap;
}

.sidebar-logo {
  width: 34px;
  height: 34px;
  border-radius: var(--radius-md);
  background: var(--color-accent-light);
  color: var(--color-accent);
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 15px;
  flex-shrink: 0;
}

.sidebar-brand-text {
  font-weight: 700;
  font-size: 15px;
  color: var(--color-text);
  white-space: nowrap;
  overflow: hidden;
}

.sidebar-close-btn {
  display: none;
  background: none;
  border: none;
  color: var(--color-text-muted);
  cursor: pointer;
  font-size: 14px;
}

.sidebar-nav {
  flex: 1;
  overflow-y: auto;
  overflow-x: hidden;
  padding: 10px 8px;
}

.nav-item {
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 9px 12px;
  border-radius: var(--radius-md);
  font-size: 13px;
  font-weight: 500;
  color: var(--color-text-secondary);
  text-decoration: none;
  transition: all 0.12s ease;
  margin-bottom: 2px;
  touch-action: manipulation;
  cursor: pointer;
  user-select: none;
  white-space: nowrap;
  overflow: hidden;
}

@media (hover: hover) {
  .nav-item:hover {
    background: var(--color-border-light);
    color: var(--color-text);
  }
}

.nav-item.active {
  background: var(--color-accent-light);
  color: var(--color-accent);
  font-weight: 600;
}

.nav-item i {
  font-size: 15px;
  width: 20px;
  text-align: center;
  flex-shrink: 0;
}

.nav-label {
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

/* Collapsed state nav items â€” center icons */
.sidebar.collapsed .nav-item {
  justify-content: center;
  padding: 10px;
}

.sidebar.collapsed .sidebar-header {
  justify-content: center;
  padding: 0 8px;
}

.sidebar-footer {
  padding: 10px 8px;
  border-top: 1px solid var(--color-border-light);
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 6px;
  flex-shrink: 0;
}

.sidebar.collapsed .sidebar-footer {
  flex-direction: column;
  gap: 6px;
  padding: 8px 6px;
}

.collapse-btn {
  width: 32px;
  height: 32px;
  border-radius: var(--radius-md);
  display: flex;
  align-items: center;
  justify-content: center;
  background: none;
  border: none;
  color: var(--color-text-muted);
  cursor: pointer;
  transition: all 0.15s;
  flex-shrink: 0;
}
.collapse-btn:hover {
  background: var(--color-border-light);
  color: var(--color-text);
}

.sidebar-user {
  display: flex;
  align-items: center;
  gap: 8px;
  overflow: hidden;
  flex: 1;
  min-width: 0;
}

.user-avatar {
  width: 30px;
  height: 30px;
  border-radius: var(--radius-md);
  background: var(--color-accent);
  color: #fff;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 12px;
  font-weight: 700;
  flex-shrink: 0;
  text-decoration: none;
}

.user-name {
  font-size: 12px;
  font-weight: 600;
  color: var(--color-text);
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.sidebar-logout {
  color: var(--color-text-muted) !important;
  flex-shrink: 0;
}
.sidebar-logout:hover {
  background: var(--color-danger-light) !important;
  color: var(--color-danger) !important;
}

/* â”€â”€ Main Wrapper â”€â”€ */
.main-wrapper {
  flex: 1;
  display: flex;
  flex-direction: column;
  overflow: hidden;
  min-width: 0;
}

/* â”€â”€ Topbar â”€â”€ */
.topbar {
  height: 56px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 24px;
  background: var(--color-surface);
  border-bottom: 1px solid var(--color-border-light);
  flex-shrink: 0;
}

.topbar-left {
  display: flex;
  align-items: center;
  gap: 12px;
}

.topbar-menu-btn {
  display: none;
}

.topbar-title {
  font-size: 14px;
  font-weight: 600;
  color: var(--color-text);
  margin: 0;
}

.topbar-right {
  display: flex;
  align-items: center;
  gap: 8px;
}

.topbar-divider {
  width: 1px;
  height: 20px;
  background: var(--color-border);
  margin: 0 4px;
}

.topbar-user {
  display: flex;
  align-items: center;
  gap: 10px;
}

.topbar-user-info {
  text-align: right;
}

.topbar-user-name {
  display: block;
  font-size: 13px;
  font-weight: 600;
  color: var(--color-text);
  line-height: 1.2;
}

.topbar-user-role {
  display: block;
  font-size: 11px;
  color: var(--color-text-muted);
}

.topbar-avatar {
  width: 34px;
  height: 34px;
}

/* â”€â”€ Alerts â”€â”€ */
.alerts-wrapper {
  position: relative;
}

.alerts-btn {
  position: relative;
}

.alerts-dot {
  position: absolute;
  top: 6px;
  right: 6px;
  width: 7px;
  height: 7px;
  border-radius: 50%;
  background: var(--color-warning);
}

.alerts-dot.critical {
  background: var(--color-danger);
}

.alerts-dropdown {
  position: absolute;
  right: 0;
  top: 42px;
  width: 320px;
  z-index: 30;
}

.alerts-dropdown-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 12px 16px;
  border-bottom: 1px solid var(--color-border-light);
}

.alerts-dropdown-title {
  font-size: 13px;
  font-weight: 600;
  color: var(--color-text);
}

.alerts-view-all {
  font-size: 12px;
  font-weight: 600;
  color: var(--color-accent);
  text-decoration: none;
}
.alerts-view-all:hover {
  text-decoration: underline;
}

.alerts-dropdown-body {
  max-height: 260px;
  overflow-y: auto;
}

.alerts-empty {
  padding: 24px;
  text-align: center;
  font-size: 12px;
  color: var(--color-text-muted);
}

.alert-item {
  display: flex;
  gap: 10px;
  padding: 10px 16px;
  text-decoration: none;
  border-bottom: 1px solid var(--color-border-light);
  transition: background 0.1s;
}
.alert-item:hover {
  background: var(--color-bg);
}
.alert-item:last-child {
  border-bottom: none;
}

.alert-icon {
  margin-top: 2px;
  font-size: 13px;
}

.alert-content {
  flex: 1;
  min-width: 0;
}

.alert-target {
  font-size: 12px;
  font-weight: 600;
  color: var(--color-text);
}

.alert-message {
  font-size: 11px;
  color: var(--color-text-muted);
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

/* â”€â”€ Page Content â”€â”€ */
.page-content {
  flex: 1;
  overflow-y: auto;
  padding: 28px;
  background: var(--color-bg);
}

.sidebar-backdrop {
  display: none;
}

/* â”€â”€ Language Switcher â”€â”€ */
.lang-wrapper {
  position: relative;
}

.lang-btn {
  display: flex;
  align-items: center;
  gap: 4px;
  padding: 4px 8px !important;
  border-radius: var(--radius-md);
  font-size: 12px;
  font-weight: 600;
  cursor: pointer;
  transition: background 0.15s;
}

.lang-flag {
  font-size: 16px;
  line-height: 1;
}

.lang-code {
  font-size: 11px;
  font-weight: 700;
  color: var(--color-text-secondary);
  letter-spacing: 0.5px;
}

.lang-dropdown {
  position: absolute;
  right: 0;
  top: 42px;
  width: 180px;
  z-index: 30;
  padding: 4px;
  border-radius: var(--radius-lg);
}

.lang-option {
  display: flex;
  align-items: center;
  gap: 10px;
  width: 100%;
  padding: 8px 12px;
  border: none;
  background: none;
  border-radius: var(--radius-md);
  cursor: pointer;
  font-size: 13px;
  font-weight: 500;
  color: var(--color-text);
  transition: background 0.12s;
}

.lang-option:hover {
  background: var(--color-bg);
}

.lang-option.active {
  background: var(--color-accent-muted);
  color: var(--color-accent);
  font-weight: 600;
}

.lang-option-flag {
  font-size: 18px;
  line-height: 1;
}

.lang-option-label {
  flex: 1;
  text-align: start;
}

.lang-option-check {
  font-size: 11px;
  color: var(--color-accent);
}

/* â”€â”€ RTL Overrides â”€â”€ */
:host-context([dir="rtl"]) .topbar-user-info {
  text-align: left;
}

:host-context([dir="rtl"]) .alerts-dropdown {
  right: auto;
  left: 0;
}

:host-context([dir="rtl"]) .lang-dropdown {
  right: auto;
  left: 0;
}

:host-context([dir="rtl"]) .alerts-dot {
  right: auto;
  left: 6px;
}

:host-context([dir="rtl"]) .sidebar {
  border-right: none;
  border-left: 1px solid var(--color-border);
}

:host-context([dir="rtl"]) .collapse-btn i.pi-angle-double-left {
  transform: scaleX(-1);
}

:host-context([dir="rtl"]) .collapse-btn i.pi-angle-double-right {
  transform: scaleX(-1);
}

/* â”€â”€ Responsive â”€â”€ */
@media (max-width: 768px) {
  .sidebar-backdrop.is-visible {
    display: block;
    position: fixed;
    inset: 0;
    background: rgba(28, 25, 23, 0.3);
    z-index: 19;
    backdrop-filter: blur(2px);
  }

  .sidebar {
    position: fixed;
    height: 100vh;
    left: 0;
    top: 0;
    transform: translateX(-100%);
    z-index: 21;
    width: 260px !important;
    box-shadow: var(--shadow-lg);
  }
  .sidebar.sidebar-open {
    transform: translateX(0);
  }
  .sidebar-close-btn {
    display: block;
  }
  .collapse-btn {
    display: none;
  }
  .topbar-menu-btn {
    display: flex;
  }
  .topbar-title {
    display: none;
  }
  .topbar-user-info {
    display: none;
  }

  :host-context([dir="rtl"]) .sidebar {
    left: auto;
    right: 0;
    transform: translateX(100%);
  }
  :host-context([dir="rtl"]) .sidebar.sidebar-open {
    transform: translateX(0);
  }
}

/* Hide collapse button on mobile */
@media (max-width: 768px) {
  .sidebar.collapsed {
    width: 260px !important;
  }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\alerts\alerts.component.css
```css
.kpi-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 16px;
}

.kpi-card {
  padding: 16px;
  cursor: pointer;
  touch-action: manipulation;
  transition: border-color 0.15s ease, background 0.15s ease;
}
@media (hover: hover) {
  .kpi-card:hover {
    border-color: #d1d5db;
  }
}

.kpi-label {
  font-size: 10px;
  font-weight: 600;
  color: var(--color-text-secondary);
  text-transform: uppercase;
  letter-spacing: 0.04em;
}

.kpi-number-row {
  display: flex;
  align-items: center;
  gap: 8px;
  margin-top: 4px;
}

.kpi-number {
  font-size: 24px;
  font-weight: 800;
  color: var(--color-text);
}

.kpi-label-danger { color: var(--color-danger); }
.kpi-label-warning { color: var(--color-warning); }
.kpi-label-info { color: #2563eb; }
.kpi-number-danger { color: var(--color-danger); }
.kpi-number-warning { color: var(--color-warning); }
.kpi-number-info { color: #2563eb; }

.kpi-pulse {
  display: inline-block;
  width: 8px;
  height: 8px;
  border-radius: 50%;
}
.kpi-pulse-danger {
  background: var(--color-danger);
}

.filter-module-label {
  font-size: 13px;
  font-weight: 600;
  color: var(--color-text-secondary);
}

.kpi-card.active {
  border-color: var(--color-accent);
  background: var(--color-accent-light) !important;
}

.kpi-card.active.critical {
  border-color: var(--color-danger);
  background: #fef2f2 !important;
}

.kpi-card.active.warning {
  border-color: var(--color-warning);
  background: #fffbeb !important;
}

.kpi-card.active.info {
  border-color: #93c5fd;
  background: #eff6ff !important;
}

.filter-left {
  display: flex;
  align-items: center;
  gap: 12px;
  flex: 1;
  flex-wrap: wrap;
}

.filter-select-group {
  display: flex;
  align-items: center;
  gap: 8px;
}

.filter-count {
  font-size: 12px;
  color: var(--color-text-muted);
}

.alert-type-badge {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  padding: 4px 10px;
  border-radius: 999px;
  font-size: 11px;
  font-weight: 600;
}

.bg-type-red { background: #fef2f2; color: #dc2626; }
.bg-type-blue { background: #eff6ff; color: #2563eb; }
.bg-type-amber { background: #fffbeb; color: #d97706; }
.bg-type-emerald { background: #ecfdf5; color: #059669; }

.table-empty-success i {
  color: var(--color-success) !important;
}

@media (max-width: 768px) {
  .kpi-grid {
    grid-template-columns: repeat(2, 1fr);
  }
}

@media (max-width: 480px) {
  .kpi-grid {
    grid-template-columns: 1fr;
  }
  .filter-bar {
    flex-direction: column;
    align-items: stretch;
  }
  .filter-select {
    width: 100%;
  }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\clients\clients.component.css
```css
/* Dialog & form specifics */
.specs-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 12px;
  background: var(--color-bg);
  border: 1px solid var(--color-border-light);
  border-radius: var(--radius-md);
  padding: 12px;
  margin-bottom: 20px;
}

.spec-item {
  display: flex;
  flex-direction: column;
  gap: 2px;
}

.spec-label {
  font-size: 10px;
  font-weight: 600;
  color: var(--color-text-muted);
  text-transform: uppercase;
  letter-spacing: 0.03em;
}

.spec-value {
  font-size: 12px;
  font-weight: 600;
  color: var(--color-text);
}

.notes-box {
  padding: 12px;
  background: #fffdf5;
  border: 1px solid #fef3c7;
  border-radius: var(--radius-md);
  color: #92400e;
  font-size: 12px;
  margin-bottom: 20px;
}

.notes-box-title {
  font-weight: 700;
  margin-bottom: 4px;
}

.section-title {
  font-size: 14px;
  font-weight: 600;
  color: var(--color-text);
  margin: 0 0 12px 0;
  padding-bottom: 8px;
  border-bottom: 1px solid var(--color-border-light);
}

.form-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 14px;
}

.form-full {
  grid-column: 1 / -1;
}

.form-actions {
  display: flex;
  justify-content: flex-end;
  gap: 8px;
  margin-top: 20px;
}

@media (max-width: 768px) {
  .specs-grid {
    grid-template-columns: repeat(2, 1fr);
  }
}

@media (max-width: 480px) {
  .specs-grid {
    grid-template-columns: 1fr;
  }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\consumables\consumables.component.css
```css
/* â”€â”€ Page â”€â”€ */
.page {
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.page-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  flex-wrap: wrap;
  gap: 12px;
}

.page-title {
  font-size: 18px;
  font-weight: 700;
  color: var(--color-text);
  margin: 0;
}

.page-desc {
  font-size: 13px;
  color: var(--color-text-muted);
  margin: 2px 0 0;
}

.header-actions {
  display: flex;
  gap: 8px;
}

/* â”€â”€ Vehicle Selector â”€â”€ */
.selector-bar {
  padding: 16px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  flex-wrap: wrap;
  gap: 12px;
}

.selector-left {
  display: flex;
  align-items: center;
  gap: 12px;
  flex: 1;
}

.odometer-badge {
  font-size: 12px;
  font-weight: 600;
  color: var(--color-text-secondary);
  background: var(--color-bg);
  border: 1px solid var(--color-border-light);
  padding: 8px 12px;
  border-radius: var(--radius-md);
}

.odometer-value {
  color: var(--color-accent);
  font-weight: 700;
}

/* â”€â”€ Empty State â”€â”€ */
.empty-state {
  padding: 48px 16px;
  text-align: center;
  color: var(--color-text-muted);
}

/* â”€â”€ Consumables Grid â”€â”€ */
.consumables-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 16px;
}

.consumable-card {
  padding: 20px;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  transition: all 0.15s ease;
}
.consumable-card:hover {
  border-color: #d1d5db;
}

.consumable-card.due {
  border-color: #fecaca;
}
.consumable-card.warning {
  border-color: #fef3c7;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 16px;
}

.card-title {
  font-size: 14px;
  font-weight: 700;
  color: var(--color-text);
  margin: 0;
}

.card-subtitle {
  font-size: 11px;
  color: var(--color-text-muted);
  margin-top: 2px;
}

.card-details {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.info-row {
  display: flex;
  justify-content: space-between;
  font-size: 12px;
  color: var(--color-text-secondary);
}

.progress-group {
  margin-top: 4px;
}

.progress-container {
  width: 100%;
  background: var(--color-bg);
  height: 6px;
  border-radius: var(--radius-sm);
  overflow: hidden;
  margin-top: 4px;
}

.progress-bar {
  height: 100%;
  transition: width 0.3s ease;
}
.progress-success { background: var(--color-success); }
.progress-warning { background: var(--color-warning); }
.progress-danger  { background: var(--color-danger); }

.extra-specs-box {
  padding: 10px;
  background: var(--color-bg);
  border: 1px solid var(--color-border-light);
  border-radius: var(--radius-md);
  margin-top: 8px;
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.spec-line {
  display: flex;
  justify-content: space-between;
  font-size: 11px;
  color: var(--color-text-secondary);
}

.spec-line-notes {
  font-size: 11px;
  color: var(--color-text-muted);
  font-style: italic;
  margin-top: 2px;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.card-actions {
  margin-top: 16px;
  padding-top: 12px;
  border-top: 1px solid var(--color-border-light);
  display: flex;
  justify-content: flex-end;
}

/* â”€â”€ Forms / Dialog â”€â”€ */
.form-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 14px;
}

.form-full {
  grid-column: 1 / -1;
}

.form-actions {
  display: flex;
  justify-content: flex-end;
  gap: 8px;
  margin-top: 20px;
}

.conditional-box {
  grid-column: 1 / -1;
  background: var(--color-bg);
  border: 1px solid var(--color-border-light);
  border-radius: var(--radius-md);
  padding: 12px;
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 12px;
}

/* â”€â”€ Config dialog styles â”€â”€ */
.config-header-desc {
  color: var(--color-text-secondary);
  font-size: 12px;
  margin-bottom: 12px;
}

.config-table-container {
  max-height: 240px;
  overflow-y: auto;
  margin-bottom: 16px;
  border: 1px solid var(--color-border-light);
  border-radius: var(--radius-md);
}

.inline-config-form {
  background: var(--color-bg);
  border: 1px solid var(--color-border-light);
  border-radius: var(--radius-md);
  padding: 16px;
  margin-top: 16px;
}

.inline-config-title {
  font-size: 13px;
  font-weight: 700;
  color: var(--color-text);
  margin: 0 0 12px 0;
}

/* â”€â”€ Responsive â”€â”€ */
@media (max-width: 1024px) {
  .consumables-grid { grid-template-columns: repeat(2, 1fr); }
}

@media (max-width: 768px) {
  .consumables-grid { grid-template-columns: 1fr; }
  .selector-bar { flex-direction: column; align-items: stretch; }
  .odometer-badge { text-align: center; }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\contracts\contracts.component.css
```css
.btn-sm-success {
  padding: 6px 10px;
  font-size: 11px;
  font-weight: 600;
  background: var(--color-success);
  gap: 4px;
}

/* â”€â”€ Wizard Forms â”€â”€ */
.form-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 14px;
}

.form-full {
  grid-column: 1 / -1;
}

.form-actions {
  display: flex;
  justify-content: flex-end;
  gap: 8px;
  margin-top: 20px;
}

/* Alert system */
.alert-warning {
  padding: 12px;
  background: #fef2f2;
  border: 1px solid #fecaca;
  color: var(--color-danger);
  border-radius: var(--radius-md);
  font-size: 12px;
  display: flex;
  align-items: flex-start;
  gap: 8px;
  margin-bottom: 16px;
  font-weight: 500;
}

/* Calculation summary section in forms */
.summary-grid {
  grid-column: 1 / -1;
  background: var(--color-bg);
  border: 1px solid var(--color-border-light);
  border-radius: var(--radius-md);
  padding: 16px;
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 16px;
}

.summary-item {
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.summary-label {
  font-size: 10px;
  font-weight: 600;
  color: var(--color-text-muted);
  text-transform: uppercase;
  letter-spacing: 0.03em;
}

.summary-value {
  font-size: 14px;
  font-weight: 700;
  color: var(--color-text);
  margin-top: 4px;
}

/* Totals box */
.totals-row {
  grid-column: 1 / -1;
  border-top: 1px solid var(--color-border-light);
  padding-top: 16px;
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.amount-due-block {
  display: flex;
  flex-direction: column;
}

.amount-due-label {
  font-size: 10px;
  font-weight: 600;
  color: var(--color-text-muted);
  text-transform: uppercase;
  letter-spacing: 0.03em;
}

.amount-due-value {
  font-size: 20px;
  font-weight: 800;
  color: var(--color-accent);
}

.payment-details {
  display: flex;
  align-items: center;
  gap: 16px;
}

.payment-control {
  width: 140px;
}

/* Return summary banner */
.return-info-banner {
  background: var(--color-accent-light);
  border: 1px solid #dbeafe;
  padding: 12px 16px;
  border-radius: var(--radius-md);
  color: var(--color-text);
  font-size: 12px;
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.return-info-row {
  display: flex;
  justify-content: space-between;
  font-weight: 600;
}

/* Maintenance Checkbox wrapper */
.maintenance-check-box {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 12px;
  background: var(--color-bg);
  border: 1px solid var(--color-border-light);
  border-radius: var(--radius-md);
  margin-top: 8px;
}

/* â”€â”€ Print area styling â”€â”€ */
.invoice-print {
  padding: 24px;
  background: #fff;
  color: #1f2937;
  font-size: 12px;
}

.invoice-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  border-bottom: 1px solid #e5e7eb;
  padding-bottom: 16px;
  margin-bottom: 20px;
}

.invoice-brand {
  font-size: 16px;
  font-weight: 800;
  letter-spacing: 0.05em;
  color: #111827;
  display: flex;
  align-items: center;
  gap: 6px;
}

.invoice-meta {
  text-align: right;
}

.invoice-meta-title {
  font-size: 13px;
  font-weight: 700;
  color: #111827;
  margin: 0;
}

.invoice-meta-number {
  font-size: 12px;
  font-weight: 700;
  color: var(--color-accent);
}

.invoice-addresses {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 32px;
  margin-bottom: 24px;
}

.address-block-title {
  font-size: 10px;
  font-weight: 700;
  text-transform: uppercase;
  color: #9ca3af;
  margin-bottom: 6px;
  letter-spacing: 0.04em;
}

.address-block-name {
  font-size: 12px;
  font-weight: 700;
  color: #111827;
}

.invoice-table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 16px;
  margin-bottom: 24px;
}

.invoice-table th {
  text-align: left;
  padding: 8px 0;
  border-bottom: 1px solid #e5e7eb;
  color: #9ca3af;
  font-weight: 700;
  font-size: 10px;
  text-transform: uppercase;
  letter-spacing: 0.04em;
}

.invoice-table td {
  padding: 12px 0;
  border-bottom: 1px solid #f3f4f6;
}

.invoice-totals-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 32px;
  border-top: 1px solid #e5e7eb;
  padding-top: 16px;
  margin-bottom: 32px;
}

.totals-details {
  text-align: right;
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.totals-line {
  display: flex;
  justify-content: space-between;
}

.totals-line-grand {
  font-size: 14px;
  font-weight: 800;
  border-top: 1px solid #e5e7eb;
  padding-top: 8px;
  color: #111827;
}

.invoice-signatures {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 32px;
  text-align: center;
  color: #9ca3af;
  font-size: 10px;
  margin-top: 48px;
}

.signature-box {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.signature-line {
  margin-top: 40px;
  width: 160px;
  height: 1px;
  background: #d1d5db;
}

@media print {
  body {
    background: white;
  }
  .no-print {
    display: none !important;
  }
  .invoice-print {
    padding: 0;
  }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\dashboard\dashboard.component.css
```css
/* â”€â”€ KPIs â”€â”€ */
.kpi-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 16px;
}

.kpi-card {
  padding: 20px 24px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  position: relative;
  overflow: hidden;
}

/* Left accent border per KPI */
.kpi-card:nth-child(1) { border-left: 4px solid var(--color-accent); }
.kpi-card:nth-child(2) { border-left: 4px solid var(--color-success); }
.kpi-card:nth-child(3) { border-left: 4px solid #3B82F6; }
.kpi-card:nth-child(4) { border-left: 4px solid var(--color-danger); }

.kpi-content {
  display: flex;
  flex-direction: column;
}

.kpi-label {
  font-size: 12px;
  font-weight: 600;
  color: var(--color-text-muted);
  text-transform: uppercase;
  letter-spacing: 0.03em;
}

.kpi-value {
  font-size: 26px;
  font-weight: 800;
  color: var(--color-text);
  line-height: 1.3;
  margin: 2px 0;
  letter-spacing: -0.02em;
}

.kpi-sub {
  font-size: 12px;
  color: var(--color-text-muted);
}

.kpi-icon {
  width: 42px;
  height: 42px;
  border-radius: var(--radius-md);
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 16px;
  flex-shrink: 0;
}

.kpi-icon-accent { background: var(--color-accent-light); color: var(--color-accent); }
.kpi-icon-success { background: #ECFDF5; color: #059669; }
.kpi-icon-info { background: #EFF6FF; color: #3B82F6; }
.kpi-icon-danger { background: #FEF2F2; color: #DC2626; }

/* â”€â”€ Dashboard Grid â”€â”€ */
.dashboard-grid {
  display: grid;
  grid-template-columns: 1fr 2fr;
  gap: 16px;
}

.dashboard-card {
  display: flex;
  flex-direction: column;
}

.card-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 18px 24px;
  border-bottom: 1px solid var(--color-border-light);
}

.card-title {
  font-size: 14px;
  font-weight: 600;
  color: var(--color-text);
  margin: 0;
}

.card-desc {
  font-size: 11px;
  color: var(--color-text-muted);
  margin: 2px 0 0;
}

.chart-wrapper {
  flex: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 24px;
}

/* â”€â”€ Alert Rows â”€â”€ */
.alerts-list {
  flex: 1;
  overflow-y: auto;
  max-height: 280px;
}

.alert-row {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 12px 24px;
  border-bottom: 1px solid var(--color-border-light);
  transition: background 0.1s;
}
.alert-row:hover {
  background: var(--color-bg);
}
.alert-row:last-child {
  border-bottom: none;
}

.alert-row-left {
  display: flex;
  align-items: center;
  gap: 12px;
}

.alert-row-icon {
  width: 30px;
  height: 30px;
  border-radius: var(--radius-md);
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 12px;
  flex-shrink: 0;
}

.alert-row-target {
  display: block;
  font-size: 13px;
  font-weight: 600;
  color: var(--color-text);
}

.alert-row-msg {
  display: block;
  font-size: 11px;
  color: var(--color-text-muted);
}

/* â”€â”€ Table â”€â”€ */
.table-wrapper {
  overflow-x: auto;
}

/* â”€â”€ Empty State â”€â”€ */
.empty-state-sm {
  text-align: center;
  padding: 32px 16px;
  color: var(--color-text-muted);
  font-size: 13px;
}

/* â”€â”€ Responsive â”€â”€ */
@media (max-width: 1024px) {
  .kpi-grid {
    grid-template-columns: repeat(2, 1fr);
  }
  .dashboard-grid {
    grid-template-columns: 1fr;
  }
}

@media (max-width: 640px) {
  .kpi-grid {
    grid-template-columns: 1fr;
  }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\fuel\fuel.component.css
```css
/* â”€â”€ Page â”€â”€ */
.page {
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.page-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  flex-wrap: wrap;
  gap: 12px;
  background: #111827;
  color: #fff;
  padding: 20px;
  border-radius: var(--radius-lg);
}

.page-header-title {
  font-size: 20px;
  font-weight: 700;
  margin: 0;
  letter-spacing: -0.02em;
}

.page-header-desc {
  font-size: 13px;
  color: #9ca3af;
  margin: 4px 0 0;
}

.header-actions {
  display: flex;
  gap: 8px;
}

/* â”€â”€ Split Layout â”€â”€ */
.split-layout {
  display: grid;
  grid-template-columns: repeat(12, 1fr);
  gap: 20px;
}

.split-left {
  grid-column: span 5;
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.split-right {
  grid-column: span 7;
  display: flex;
  flex-direction: column;
  gap: 20px;
}

/* â”€â”€ Inactivity Alerts Card â”€â”€ */
.inactivity-card {
  background: #fffdf5;
  border: 1px solid #fef3c7;
  color: #92400e;
  padding: 16px;
  border-radius: var(--radius-md);
  font-size: 12px;
}

.inactivity-header {
  display: flex;
  align-items: flex-start;
  gap: 10px;
  margin-bottom: 12px;
}

.inactivity-icon-box {
  background: #f59e0b;
  color: #fff;
  padding: 6px;
  border-radius: var(--radius-sm);
  display: flex;
  align-items: center;
  justify-content: center;
}

.inactivity-title {
  font-weight: 700;
  font-size: 13px;
  margin: 0;
}

.inactivity-desc {
  color: #b45309;
  margin-top: 2px;
}

.inactivity-list {
  max-height: 120px;
  overflow-y: auto;
  border-top: 1px solid #fde68a;
  padding-top: 8px;
}

.inactivity-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 6px 0;
}

/* â”€â”€ Vehicle List OdomÃ¨tres â”€â”€ */
.vehicle-list-card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 16px;
  border-bottom: 1px solid var(--color-border-light);
}

.vehicle-list-title {
  font-size: 14px;
  font-weight: 600;
  margin: 0;
  display: flex;
  align-items: center;
  gap: 6px;
}

.vehicle-list-count {
  font-size: 11px;
  font-weight: 600;
  background: var(--color-bg);
  border: 1px solid var(--color-border-light);
  padding: 3px 8px;
  border-radius: var(--radius-sm);
}

.vehicle-list {
  max-height: 520px;
  overflow-y: auto;
  padding: 12px;
}

.vehicle-item {
  padding: 12px;
  border: 1px solid var(--color-border-light);
  border-radius: var(--radius-md);
  margin-bottom: 8px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  transition: all 0.15s ease;
  cursor: pointer;
  touch-action: manipulation;
}
@media (hover: hover) {
  .vehicle-item:hover {
    border-color: #d1d5db;
  }
}
.vehicle-item.active {
  background: var(--color-accent-light);
  border-color: var(--color-accent);
}

.vehicle-info {
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.vehicle-name {
  font-size: 13px;
  font-weight: 700;
  color: var(--color-text);
  margin: 0;
}

.vehicle-plate {
  font-size: 11px;
  color: var(--color-text-muted);
}

.vehicle-badges {
  display: flex;
  gap: 4px;
  margin-top: 2px;
}

.vehicle-odometer-box {
  text-align: right;
  display: flex;
  flex-direction: column;
  align-items: flex-end;
  gap: 6px;
}

.odometer-number {
  font-size: 16px;
  font-weight: 800;
  color: var(--color-text);
}

/* â”€â”€ Unselected State placeholder â”€â”€ */
.unselected-placeholder {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 48px;
  text-align: center;
  color: var(--color-text-muted);
  min-height: 450px;
}

.unselected-icon {
  font-size: 40px;
  color: #d1d5db;
  margin-bottom: 12px;
}

/* â”€â”€ Selected State â”€â”€ */
.meta-info-bar {
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap;
  gap: 12px;
  padding: 16px;
}

.meta-title-box {
  display: flex;
  flex-direction: column;
}

.meta-title-row {
  display: flex;
  align-items: center;
  gap: 8px;
}

.meta-title {
  font-size: 16px;
  font-weight: 700;
  margin: 0;
}

.meta-plate {
  font-size: 11px;
  font-weight: 600;
  background: var(--color-bg);
  border: 1px solid var(--color-border-light);
  padding: 2px 6px;
  border-radius: var(--radius-sm);
}

.meta-desc {
  font-size: 11px;
  color: var(--color-text-muted);
  margin-top: 4px;
}

.meta-actions {
  display: flex;
  gap: 6px;
}

/* Charts & Logs */
.card-header-inner {
  padding: 16px 20px;
  border-bottom: 1px solid var(--color-border-light);
}

.card-title-inner {
  font-size: 14px;
  font-weight: 600;
  margin: 0;
  display: flex;
  align-items: center;
  gap: 6px;
}

/* â”€â”€ Forms / Dialog â”€â”€ */
.form-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 14px;
}

.form-full {
  grid-column: 1 / -1;
}

.form-actions {
  display: flex;
  justify-content: flex-end;
  gap: 8px;
  margin-top: 20px;
}

.calc-hint-box {
  grid-column: 1 / -1;
  background: var(--color-accent-light);
  border: 1px solid #dbeafe;
  padding: 10px 12px;
  border-radius: var(--radius-md);
  display: flex;
  justify-content: space-between;
  font-size: 11px;
  color: var(--color-text-secondary);
}

/* â”€â”€ Responsive â”€â”€ */
@media (max-width: 1024px) {
  .split-left, .split-right {
    grid-column: span 12;
  }
}

@media (max-width: 640px) {
  .meta-info-bar { flex-direction: column; align-items: stretch; }
  .meta-actions { justify-content: flex-start; }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\insurance-inspections\insurance-inspections.component.css
```css
/* â”€â”€ Page â”€â”€ */
.page {
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.page-header {
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.page-title {
  font-size: 18px;
  font-weight: 700;
  color: var(--color-text);
  margin: 0;
}

.page-desc {
  font-size: 13px;
  color: var(--color-text-muted);
  margin: 2px 0 0;
}

/* â”€â”€ Split Layout â”€â”€ */
.grid-2col {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 20px;
}

.card-header-inner {
  display: flex;
  justify-content: space-between;
  align-items: center;
  border-bottom: 1px solid var(--color-border-light);
  padding-bottom: 12px;
  margin-bottom: 12px;
}

.card-title-inner {
  font-size: 14px;
  font-weight: 600;
  margin: 0;
}

.card-desc-inner {
  font-size: 11px;
  color: var(--color-text-muted);
  margin-top: 2px;
}

@media (max-width: 992px) {
  .grid-2col {
    grid-template-columns: 1fr;
  }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\login\login.component.css
```css
.login-page {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: var(--color-bg);
  position: relative;
}

.login-lang-bar {
  position: absolute;
  top: 16px;
  right: 20px;
  display: flex;
  gap: 4px;
  z-index: 10;
}

.login-lang-btn {
  display: flex;
  align-items: center;
  gap: 4px;
  padding: 5px 10px;
  border: none;
  border-radius: var(--radius-md);
  background: var(--color-surface);
  box-shadow: var(--shadow-sm);
  font-size: 12px;
  font-weight: 600;
  color: var(--color-text-secondary);
  cursor: pointer;
  transition: all 0.15s;
}

.login-lang-btn:hover {
  color: var(--color-accent);
  box-shadow: var(--shadow-md);
}

.login-lang-btn.active {
  background: var(--color-accent);
  color: #fff;
}

:host-context([dir="rtl"]) .login-lang-bar {
  right: auto;
  left: 20px;
}

:host-context([dir="rtl"]) .input-icon {
  left: auto;
  right: 14px;
}

:host-context([dir="rtl"]) .form-input.has-icon {
  padding-left: 14px;
  padding-right: 38px;
}

.login-container {
  width: 100%;
  max-width: 400px;
  padding: 16px;
}

.login-card {
  padding: 36px;
  box-shadow: var(--shadow-md);
  border-radius: var(--radius-xl);
}

.login-header {
  text-align: center;
  margin-bottom: 28px;
}

.login-logo {
  width: 44px;
  height: 44px;
  border-radius: var(--radius-lg);
  background: var(--color-accent-light);
  color: var(--color-accent);
  display: inline-flex;
  align-items: center;
  justify-content: center;
  font-size: 20px;
  margin-bottom: 14px;
}

.login-title {
  font-size: 20px;
  font-weight: 700;
  color: var(--color-text);
  margin: 0;
}

.login-subtitle {
  font-size: 13px;
  color: var(--color-text-muted);
  margin: 4px 0 0;
}

.login-error {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 10px 14px;
  background: #FEF2F2;
  border-radius: var(--radius-md);
  color: #DC2626;
  font-size: 13px;
  margin-bottom: 20px;
}

.login-form {
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.form-group {
  display: flex;
  flex-direction: column;
}

.input-wrapper {
  position: relative;
}

.input-icon {
  position: absolute;
  left: 14px;
  top: 50%;
  transform: translateY(-50%);
  color: var(--color-text-muted);
  font-size: 14px;
}

.form-input.has-icon {
  padding-left: 38px;
}

.login-btn {
  width: 100%;
  justify-content: center;
  padding: 11px 16px;
  font-size: 14px;
  margin-top: 4px;
  border-radius: var(--radius-md);
}

.spinner {
  width: 14px;
  height: 14px;
  border: 2px solid rgba(255,255,255,0.3);
  border-top-color: #fff;
  border-radius: 50%;
  animation: spin 0.6s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

.login-footer {
  margin-top: 20px;
  padding-top: 16px;
  border-top: 1px solid var(--color-border-light);
  text-align: center;
  font-size: 12px;
  color: var(--color-text-muted);
}

.login-footer code {
  background: #F5F5F4;
  padding: 2px 6px;
  border-radius: 4px;
  font-size: 12px;
  color: var(--color-text-secondary);
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\maintenance\maintenance.component.css
```css
.form-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 14px;
}

.form-full {
  grid-column: 1 / -1;
}

.form-actions {
  display: flex;
  justify-content: flex-end;
  gap: 8px;
  margin-top: 20px;
}

.cost-box {
  grid-column: 1 / -1;
  background: var(--color-bg);
  border: 1px solid var(--color-border-light);
  border-radius: var(--radius-md);
  padding: 12px;
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 12px;
}

.event-timeline {
  position: relative;
  padding-left: 20px;
  border-left: 2px solid var(--color-border-light);
  margin-top: 12px;
}

.event-item {
  position: relative;
  margin-bottom: 16px;
}

.event-item:last-child {
  margin-bottom: 0;
}

.event-dot {
  position: absolute;
  left: -26px;
  top: 6px;
  width: 10px;
  height: 10px;
  border-radius: 50%;
  border: 2px solid #fff;
  background: var(--color-accent);
}

.event-card {
  padding: 12px 16px;
  background: var(--color-surface);
  border: 1px solid var(--color-border-light);
  border-radius: var(--radius-md);
  display: flex;
  justify-content: space-between;
  align-items: center;
  transition: border-color 0.15s ease;
}
@media (hover: hover) {
  .event-card:hover {
    border-color: #d1d5db;
  }
}

.event-date {
  font-size: 11px;
  color: var(--color-text-muted);
  font-weight: 600;
}

.event-title {
  font-size: 13px;
  font-weight: 700;
  color: var(--color-text);
  margin: 2px 0 0;
}

.event-desc {
  font-size: 11px;
  color: var(--color-text-secondary);
  margin-top: 2px;
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\reports\reports.component.css
```css
/* â”€â”€ Page â”€â”€ */
.page {
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.page-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  flex-wrap: wrap;
  gap: 12px;
  background: #111827;
  color: #fff;
  padding: 20px;
  border-radius: var(--radius-lg);
}

.page-header-title {
  font-size: 20px;
  font-weight: 700;
  margin: 0;
  letter-spacing: -0.02em;
}

.page-header-desc {
  font-size: 13px;
  color: #9ca3af;
  margin: 4px 0 0;
}

.header-actions {
  display: flex;
  gap: 8px;
}

/* â”€â”€ Financial & Utilization Overview Cards â”€â”€ */
.kpi-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 16px;
}

.kpi-card {
  padding: 20px;
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.kpi-left {
  display: flex;
  flex-direction: column;
}

.kpi-label {
  font-size: 10px;
  font-weight: 600;
  color: var(--color-text-secondary);
  text-transform: uppercase;
  letter-spacing: 0.04em;
}

.kpi-number {
  font-size: 24px;
  font-weight: 800;
  color: var(--color-text);
  margin-top: 4px;
}

.kpi-subtext {
  font-size: 11px;
  color: var(--color-text-muted);
  margin-top: 4px;
}

.kpi-sub-row {
  display: flex;
  gap: 12px;
  margin-top: 6px;
  font-size: 11px;
}

.kpi-icon-box {
  padding: 12px;
  background: var(--color-bg);
  border: 1px solid var(--color-border-light);
  color: var(--color-text-secondary);
  border-radius: var(--radius-md);
  display: flex;
  align-items: center;
  justify-content: center;
}

.kpi-icon-box.indigo { background: var(--color-accent-light); color: var(--color-accent); border-color: #dbeafe; }
.kpi-icon-box.emerald { background: #ecfdf5; color: #059669; border-color: #a7f3d0; }
.kpi-icon-box.amber { background: #fffbeb; color: #d97706; border-color: #fde68a; }

/* â”€â”€ Charts block â”€â”€ */
.grid-2col {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 20px;
}

.chart-card {
  padding: 20px;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.chart-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  border-bottom: 1px solid var(--color-border-light);
  padding-bottom: 12px;
  margin-bottom: 16px;
}

.chart-title {
  font-size: 14px;
  font-weight: 600;
  margin: 0;
  display: flex;
  align-items: center;
  gap: 6px;
}

.chart-container {
  height: 240px;
  display: flex;
  align-items: center;
  justify-content: center;
}

/* â”€â”€ Split Layout â”€â”€ */
.split-layout {
  display: grid;
  grid-template-columns: repeat(12, 1fr);
  gap: 20px;
}

.split-left {
  grid-column: span 5;
}

.split-right {
  grid-column: span 7;
}

.table-header-inner {
  padding: 16px 20px;
  border-bottom: 1px solid var(--color-border-light);
}

.table-title-inner {
  font-size: 14px;
  font-weight: 600;
  margin: 0;
  display: flex;
  align-items: center;
  gap: 6px;
}

@media (max-width: 1024px) {
  .kpi-grid { grid-template-columns: repeat(2, 1fr); }
  .grid-2col { grid-template-columns: 1fr; }
  .split-left, .split-right { grid-column: span 12; }
}

@media (max-width: 640px) {
  .kpi-grid { grid-template-columns: 1fr; }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\settings\settings.component.css
```css
/* â”€â”€ Settings Card â”€â”€ */
.settings-card {
  overflow: hidden;
}

.settings-card-header {
  display: flex;
  align-items: flex-start;
  gap: 14px;
  padding: 20px 24px;
  border-bottom: 1px solid var(--color-border-light);
}

.settings-card-icon {
  width: 40px;
  height: 40px;
  border-radius: var(--radius-md);
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 16px;
  flex-shrink: 0;
}

.icon-accent {
  background: var(--color-accent-light);
  color: var(--color-accent);
}
.icon-teal {
  background: #F0FDFA;
  color: #14B8A6;
}
.icon-emerald {
  background: #ECFDF5;
  color: #10B981;
}
.icon-amber {
  background: #FFFBEB;
  color: #F59E0B;
}

.settings-card-title {
  font-size: 15px;
  font-weight: 600;
  color: var(--color-text);
  margin: 0;
  line-height: 1.3;
}

.settings-card-desc {
  font-size: 12px;
  color: var(--color-text-muted);
  margin: 3px 0 0;
  line-height: 1.4;
}

.settings-card-body {
  padding: 20px 24px;
}

/* â”€â”€ Notification Banner â”€â”€ */
.notification-banner {
  padding: 14px 18px;
  border-radius: var(--radius-md);
  display: flex;
  align-items: center;
  gap: 10px;
  font-size: 13px;
  font-weight: 600;
  transition: all 0.3s ease;
}

.notification-banner.success {
  background: #ECFDF5;
  color: #065F46;
}

.notification-banner.error {
  background: #FEF2F2;
  color: #991B1B;
}

/* â”€â”€ Layout â”€â”€ */
.settings-layout {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 20px;
}

.settings-left {
  grid-column: span 2;
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.settings-right {
  grid-column: span 1;
  display: flex;
  flex-direction: column;
  gap: 20px;
}

/* â”€â”€ Threshold Form Grid â”€â”€ */
.settings-form-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 14px;
}

.settings-form-actions {
  display: flex;
  justify-content: flex-end;
  margin-top: 16px;
  padding-top: 16px;
  border-top: 1px solid var(--color-border-light);
}

/* â”€â”€ Reference Data â€” Accordion â”€â”€ */
.ref-body {
  padding: 0 !important;
}

.ref-section {
  border-bottom: 1px solid var(--color-border-light);
}

.ref-section:last-of-type {
  border-bottom: none;
}

.ref-section-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  width: 100%;
  padding: 16px 24px;
  border: none;
  background: none;
  cursor: pointer;
  transition: background 0.12s;
  text-align: left;
}

.ref-section-header:hover {
  background: var(--color-bg);
}

.ref-section-left {
  display: flex;
  align-items: center;
  gap: 12px;
}

.ref-section-icon {
  font-size: 15px;
  color: var(--color-text-muted);
  width: 20px;
  text-align: center;
}

.ref-section-title {
  display: block;
  font-size: 13px;
  font-weight: 600;
  color: var(--color-text);
}

.ref-section-count {
  display: block;
  font-size: 11px;
  color: var(--color-text-muted);
  margin-top: 1px;
}

.ref-chevron {
  font-size: 12px;
  color: var(--color-text-muted);
  transition: transform 0.2s ease;
}

.ref-open .ref-chevron {
  transform: rotate(180deg);
}

.ref-open .ref-section-icon {
  color: var(--color-accent);
}

.ref-section-body {
  padding: 0 24px 20px;
}

.ref-section-desc {
  font-size: 12px;
  color: var(--color-text-muted);
  margin: 0 0 12px;
  line-height: 1.5;
}

.ref-empty {
  font-size: 12px;
  color: var(--color-text-muted);
  font-style: italic;
}

.ref-save-row {
  display: flex;
  justify-content: flex-end;
  padding: 16px 24px;
  border-top: 1px solid var(--color-border-light);
}

/* â”€â”€ Tag / Catalog Items â”€â”€ */
.tag-container {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
  margin-bottom: 12px;
}

.catalog-tag {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  background: var(--color-bg);
  color: var(--color-text);
  font-size: 12px;
  font-weight: 500;
  padding: 5px 10px;
  border-radius: 20px;
}

.tag-remove-btn {
  background: none;
  border: none;
  color: var(--color-text-muted);
  cursor: pointer;
  padding: 0;
  display: flex;
  align-items: center;
  font-size: 9px;
  transition: color 0.12s;
}

.tag-remove-btn:hover {
  color: var(--color-danger);
}

.tag-add-row {
  display: flex;
  gap: 8px;
  max-width: 380px;
}

.btn-sm {
  padding: 6px 14px !important;
  font-size: 12px !important;
}

/* â”€â”€ Extras Grid â”€â”€ */
.extras-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 8px;
  margin-bottom: 12px;
}

.extra-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  background: var(--color-bg);
  padding: 10px 14px;
  border-radius: var(--radius-md);
  font-size: 12px;
}

.extra-name {
  font-weight: 600;
  color: var(--color-text);
}

.extra-price-box {
  display: flex;
  align-items: center;
  gap: 12px;
}

.extra-price {
  font-weight: 700;
  color: var(--color-accent);
  font-size: 12px;
}

.extra-add-row {
  display: flex;
  gap: 8px;
  max-width: 480px;
}

/* â”€â”€ Profile Stack â”€â”€ */
.profile-stack {
  display: flex;
  flex-direction: column;
  gap: 14px;
}

.form-group {
  display: flex;
  flex-direction: column;
}

.disabled-input {
  background: var(--color-bg) !important;
  color: var(--color-text-muted) !important;
  cursor: not-allowed;
}

.full-width {
  width: 100%;
  justify-content: center;
}

/* â”€â”€ Responsive â”€â”€ */
@media (max-width: 1024px) {
  .settings-left, .settings-right {
    grid-column: span 3;
  }
}

@media (max-width: 640px) {
  .settings-form-grid {
    grid-template-columns: 1fr;
  }
  .extras-grid {
    grid-template-columns: 1fr;
  }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Frontend\src\app\pages\vehicles\vehicles.component.css
```css
/* â”€â”€ Vehicle Grid â”€â”€ */
.vehicle-grid {
  display: grid;
  grid-template-columns: repeat(5, 1fr);
  gap: 16px;
}

.vehicle-card {
  display: flex;
  flex-direction: column;
  overflow: hidden;
  transition: box-shadow 0.15s ease;
}

.vehicle-card:hover {
  box-shadow: var(--shadow-md);
}

.vehicle-img {
  height: 180px;
  background: #F5F5F4;
  display: flex;
  align-items: center;
  justify-content: center;
  position: relative;
  overflow: hidden;
}

.vehicle-img img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.vehicle-img-placeholder {
  color: #D6D3D1;
  font-size: 40px;
}

.vehicle-status-badge {
  position: absolute;
  top: 10px;
  left: 10px;
  display: inline-flex;
  padding: 3px 10px;
  font-size: 10px;
  font-weight: 600;
  border-radius: 20px;
  letter-spacing: 0.02em;
}

.vehicle-rate {
  position: absolute;
  bottom: 10px;
  right: 10px;
  background: rgba(28, 25, 23, 0.7);
  color: #fff;
  font-size: 12px;
  font-weight: 600;
  padding: 3px 10px;
  border-radius: 20px;
}

.vehicle-body {
  padding: 16px;
  flex: 1;
  display: flex;
  flex-direction: column;
}

.vehicle-top {
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.vehicle-name {
  font-size: 15px;
  font-weight: 700;
  color: var(--color-text);
  margin: 0;
}

.vehicle-plate {
  font-size: 12px;
  color: var(--color-text-muted);
  margin: 2px 0 0;
}

.vehicle-specs {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 8px;
  margin-top: 12px;
}

.spec-item {
  background: var(--color-bg);
  border-radius: var(--radius-md);
  padding: 8px;
  text-align: center;
}

.spec-label {
  display: block;
  font-size: 10px;
  font-weight: 600;
  color: var(--color-text-muted);
  text-transform: uppercase;
  letter-spacing: 0.03em;
}

.spec-value {
  display: block;
  font-size: 12px;
  font-weight: 600;
  color: var(--color-text);
  margin-top: 2px;
}

.vehicle-actions {
  margin-top: auto;
  padding-top: 12px;
  border-top: 1px solid var(--color-border-light);
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.vehicle-actions-right {
  display: flex;
  gap: 4px;
}

.btn-link {
  background: none;
  border: none;
  color: var(--color-accent);
  font-size: 12px;
  font-weight: 600;
  cursor: pointer;
  display: inline-flex;
  align-items: center;
  gap: 4px;
  padding: 0;
}
.btn-link:hover {
  text-decoration: underline;
}

/* â”€â”€ Pagination â”€â”€ */
.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 4px;
}

.page-btn {
  width: 32px;
  height: 32px;
  display: flex;
  align-items: center;
  justify-content: center;
  border: none;
  border-radius: var(--radius-md);
  background: transparent;
  font-size: 12px;
  font-weight: 600;
  cursor: pointer;
  color: var(--color-text-secondary);
}
.page-btn.active {
  background: var(--color-accent);
  color: #fff;
}

/* â”€â”€ Empty State â”€â”€ */
.empty-state {
  padding: 48px 16px;
  text-align: center;
  color: var(--color-text-muted);
  font-size: 13px;
}
.empty-icon {
  font-size: 36px;
  color: #D6D3D1;
  margin-bottom: 8px;
}

/* â”€â”€ Details Modal â”€â”€ */
.details-layout {
  display: flex;
  gap: 20px;
  margin-top: 12px;
  min-height: 400px;
}

.details-tabs {
  display: flex;
  flex-direction: column;
  gap: 2px;
  width: 160px;
  flex-shrink: 0;
  border-right: 1px solid var(--color-border-light);
  padding-right: 16px;
}

.details-tab {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 8px 10px;
  border: none;
  border-radius: var(--radius-md);
  background: transparent;
  color: var(--color-text-secondary);
  font-size: 12px;
  font-weight: 600;
  cursor: pointer;
  touch-action: manipulation;
  text-align: left;
  white-space: nowrap;
  transition: all 0.1s;
}
@media (hover: hover) {
  .details-tab:hover {
    background: var(--color-bg);
  }
}
.details-tab.active {
  background: var(--color-accent-light);
  color: var(--color-accent);
}
.details-tab i {
  font-size: 13px;
}

.details-content {
  flex: 1;
  min-width: 0;
  overflow-x: auto;
}

.section-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 16px;
  padding-bottom: 10px;
  border-bottom: 1px solid var(--color-border-light);
}

.section-title {
  font-size: 14px;
  font-weight: 600;
  color: var(--color-text);
  margin: 0;
}

.section-actions {
  display: flex;
  gap: 6px;
}

.btn-sm {
  padding: 5px 10px;
  font-size: 12px;
}

/* â”€â”€ Km Timeline â”€â”€ */
.km-timeline {
  position: relative;
  padding-left: 20px;
  border-left: 2px solid var(--color-border-light);
}

.km-entry {
  position: relative;
  padding: 0 0 20px 0;
}

.km-dot {
  position: absolute;
  left: -25px;
  top: 4px;
  width: 8px;
  height: 8px;
  border-radius: 50%;
  border: 2px solid #fff;
  background: var(--color-text-muted);
}
.dot-success { background: var(--color-success); }
.dot-accent  { background: var(--color-accent); }
.dot-info    { background: #3B82F6; }

.km-entry-content {
  display: flex;
  flex-direction: column;
  gap: 2px;
}

.km-entry-date {
  font-size: 11px;
  color: var(--color-text-muted);
}

.km-entry-value {
  font-size: 14px;
  font-weight: 700;
  color: var(--color-text);
}

.km-entry-notes {
  font-size: 12px;
  color: var(--color-text-secondary);
  font-style: italic;
}

.km-entry-source {
  width: fit-content;
  margin-top: 2px;
}

/* â”€â”€ Forms â”€â”€ */
.form-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 14px;
  margin-top: 12px;
}

.form-full {
  grid-column: 1 / -1;
}

.form-stack {
  display: flex;
  flex-direction: column;
  gap: 14px;
  margin-top: 12px;
}

.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 14px;
}

.form-actions {
  display: flex;
  justify-content: flex-end;
  gap: 8px;
  padding-top: 8px;
}

.file-input {
  font-size: 12px;
  padding: 6px;
}

.upload-status {
  font-size: 12px;
  color: var(--color-accent);
  margin-top: 4px;
}

/* â”€â”€ Photo Upload Area â”€â”€ */
.photo-dropzone {
  display: block;
  position: relative;
  border: 2px dashed var(--color-border);
  border-radius: var(--radius-md);
  background: var(--color-bg);
  min-height: 180px;
  cursor: pointer;
  overflow: hidden;
  transition: all 0.2s ease;
}

.photo-dropzone:hover {
  border-color: var(--color-primary);
  background: #f0fdf4;
}

.photo-dropzone.has-image {
  border-style: solid;
  border-color: var(--color-border-light);
  background: transparent;
}

.photo-dropzone.has-image:hover {
  border-color: var(--color-primary);
}

.photo-input {
  position: absolute;
  width: 0;
  height: 0;
  opacity: 0;
}

.photo-preview-container {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
}

.photo-preview-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.photo-preview-overlay {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5);
  color: white;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 8px;
  opacity: 0;
  transition: opacity 0.2s ease;
  font-weight: 500;
}

.photo-preview-container:hover .photo-preview-overlay {
  opacity: 1;
}

.photo-empty-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  height: 180px;
  color: var(--color-text-secondary);
  gap: 8px;
}

.upload-icon {
  font-size: 32px;
  color: var(--color-primary);
  margin-bottom: 4px;
}

.upload-text {
  font-weight: 600;
  font-size: 14px;
  color: var(--color-text);
}

.upload-hint {
  font-size: 11px;
  color: var(--color-text-muted);
}

.photo-uploading-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  height: 180px;
  color: var(--color-primary);
  gap: 12px;
  font-weight: 500;
}

.spinner-icon {
  font-size: 24px;
}

/* â”€â”€ Responsive â”€â”€ */
@media (max-width: 1024px) {
  .vehicle-grid { grid-template-columns: repeat(2, 1fr); }
}

@media (max-width: 640px) {
  .vehicle-grid { grid-template-columns: 1fr; }
  .filter-select { width: 100%; }
  .details-layout { flex-direction: column; }
  .details-tabs { flex-direction: row; width: 100%; overflow-x: auto; border-right: none; border-bottom: 1px solid var(--color-border-light); padding-right: 0; padding-bottom: 8px; }
}

```


