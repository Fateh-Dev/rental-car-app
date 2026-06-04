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
