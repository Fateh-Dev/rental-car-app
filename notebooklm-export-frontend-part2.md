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
        <label class="form-label">{{ 'clients.fullName' | t }}</label>
        <input type="text" [(ngModel)]="clientForm.fullName" name="fullName" required class="form-input" placeholder="Jean Dupont"/>
      </div>
      <div>
        <label class="form-label">{{ 'clients.nationalId' | t }}</label>
        <input type="text" [(ngModel)]="clientForm.nationalId" name="nationalId" required class="form-input" placeholder="NÂ° identitÃ© unique"/>
      </div>
      <div>
        <label class="form-label">{{ 'clients.dateOfBirth' | t }}</label>
        <p-datepicker [(ngModel)]="clientForm.dateOfBirth" name="dateOfBirth" dateFormat="yy-mm-dd" [showIcon]="true" styleClass="w-full"></p-datepicker>
      </div>
      <div>
        <label class="form-label">{{ 'clients.licenseNumber' | t }}</label>
        <input type="text" [(ngModel)]="clientForm.licenseNumber" name="licenseNumber" required class="form-input" placeholder="Permis NÂ°"/>
      </div>
      <div>
        <label class="form-label">{{ 'clients.licenseCategory' | t }}</label>
        <select [(ngModel)]="clientForm.licenseCategory" name="licenseCategory" required class="form-input">
          <option *ngFor="let cat of licenseCategories" [value]="cat">{{ 'clients.category' | t }} {{ cat }}</option>
        </select>
      </div>
      <div>
        <label class="form-label">{{ 'clients.licenseIssueDate' | t }}</label>
        <p-datepicker [(ngModel)]="clientForm.licenseIssueDate" name="licenseIssueDate" dateFormat="yy-mm-dd" [showIcon]="true" styleClass="w-full"></p-datepicker>
      </div>
      <div>
        <label class="form-label">{{ 'clients.licenseExpiryDate' | t }}</label>
        <p-datepicker [(ngModel)]="clientForm.licenseExpiryDate" name="licenseExpiryDate" dateFormat="yy-mm-dd" [showIcon]="true" styleClass="w-full"></p-datepicker>
      </div>
      <div>
        <label class="form-label">{{ 'clients.phone' | t }}</label>
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
        <label class="form-label">{{ 'consumables.consumableType' | t }}</label>
        <select [(ngModel)]="logForm.consumableType" name="consumableType" required class="form-input">
          <option *ngFor="let type of consumableTypes" [value]="type">{{ i18n.translateConsumableType(type) }}</option>
        </select>
      </div>

      <div>
        <label class="form-label">{{ 'consumables.replacementDate' | t }}</label>
        <p-datepicker [(ngModel)]="logForm.replacementDate" name="replacementDate" dateFormat="yy-mm-dd" [showIcon]="true" styleClass="w-full"></p-datepicker>
      </div>
      <div>
        <label class="form-label">{{ 'consumables.replacementKm' | t }}</label>
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
            <label class="form-label">{{ 'contracts.selectClient' | t }}</label>
            <select *ngIf="!isEditMode" [(ngModel)]="contractForm.clientId" name="clientId" (change)="onClientSelect()" required class="form-input">
              <option [ngValue]="null">{{ 'contracts.chooseClient' | t }}</option>
              <option *ngFor="let c of activeClients" [ngValue]="c.id">{{ c.fullName }} ({{ c.nationalId }})</option>
            </select>
            <input *ngIf="isEditMode" type="text" disabled [value]="contractForm.client?.fullName + ' (' + contractForm.client?.nationalId + ')'" class="form-input disabled-input"/>
          </div>

          <!-- Vehicle Selector -->
          <div>
            <label class="form-label">{{ 'contracts.selectVehicle' | t }}</label>
            <select *ngIf="!isEditMode" [(ngModel)]="contractForm.vehicleId" name="vehicleId" (change)="onVehicleSelect()" required class="form-input">
              <option [ngValue]="null">{{ 'contracts.chooseVehicle' | t }}</option>
              <option *ngFor="let v of availableVehicles" [ngValue]="v.id">{{ v.brand }} {{ v.model }} - Plaque: {{ v.matricule }}</option>
            </select>
            <input *ngIf="isEditMode" type="text" disabled [value]="contractForm.vehicle?.brand + ' ' + contractForm.vehicle?.model + ' - ' + contractForm.vehicle?.matricule" class="form-input disabled-input"/>
          </div>
        </div>

        <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 12px;">
          <div>
            <label class="form-label">{{ 'contracts.startDateTime' | t }}</label>
            <p-datepicker [(ngModel)]="contractForm.startDate" name="startDate" dateFormat="yy-mm-dd" [showTime]="true" (onSelect)="calculateTotals()" styleClass="w-full"></p-datepicker>
          </div>
          <div>
            <label class="form-label">{{ 'contracts.expectedReturn' | t }}</label>
            <p-datepicker [(ngModel)]="contractForm.expectedReturnDate" name="expectedReturnDate" dateFormat="yy-mm-dd" [showTime]="true" (onSelect)="calculateTotals()" styleClass="w-full"></p-datepicker>
          </div>
        </div>

        <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 12px;">
          <div>
            <label class="form-label">{{ 'contracts.contractType' | t }}</label>
            <select [(ngModel)]="contractForm.contractType" name="contractType" required class="form-input">
              <option *ngFor="let ct of contractTypes" [value]="ct">{{ ct }}</option>
            </select>
          </div>
          <div>
            <label class="form-label">{{ 'contracts.dailyRate' | t }}</label>
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
        <label class="form-label">{{ 'contracts.returnIndex' | t }}</label>
        <input type="number" [(ngModel)]="returnForm.kmReturn" name="kmReturn" required class="form-input"/>
      </div>
      <div>
        <label class="form-label">{{ 'contracts.actualReturnDate' | t }}</label>
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
                  <td>{{ entry.log.costPerLiter | number:'1.2-2' }} â‚¬</td>
                  <td style="font-weight: 600;">{{ entry.log.totalCost | number:'1.2-2' }} â‚¬</td>
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
      <strong style="font-size: 13px;">{{ (fuelForm.liters * fuelForm.costPerLiter) | number:'1.2-2' }} â‚¬</strong>
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
          <label class="form-label">{{ 'login.username' | t }}</label>
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
          <label class="form-label">{{ 'login.password' | t }}</label>
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
        <label class="form-label">{{ 'common.vehicle' | t }}</label>
        <select [(ngModel)]="maintenanceForm.vehicleId" name="vehicleId" required [disabled]="isEditMode" class="form-input">
          <option [ngValue]="null">{{ 'contracts.chooseVehicle' | t }}</option>
          <option *ngFor="let v of vehicles" [value]="v.id">{{ v.brand }} {{ v.model }} ({{ v.matricule }})</option>
        </select>
      </div>

      <div>
        <label class="form-label">{{ 'maintenance.interventionType' | t }}</label>
        <select [(ngModel)]="maintenanceForm.maintenanceType" name="maintenanceType" required class="form-input">
          <option *ngFor="let t of maintenanceTypes" [value]="t">{{ i18n.translateMaintenanceType(t) }}</option>
        </select>
      </div>

      <div>
        <label class="form-label">{{ 'maintenance.interventionDate' | t }}</label>
        <p-datepicker [(ngModel)]="maintenanceForm.datePerformed" name="datePerformed" dateFormat="yy-mm-dd" [showIcon]="true" styleClass="w-full"></p-datepicker>
      </div>

      <div>
        <label class="form-label">{{ 'maintenance.nextMaintenance' | t }}</label>
        <p-datepicker [(ngModel)]="maintenanceForm.nextScheduledDate" name="nextScheduledDate" dateFormat="yy-mm-dd" [showIcon]="true" styleClass="w-full"></p-datepicker>
      </div>

      <div>
        <label class="form-label">{{ 'maintenance.counterKm' | t }}</label>
        <input type="number" [(ngModel)]="maintenanceForm.kmAtMaintenance" name="kmAtMaintenance" required class="form-input"/>
      </div>

      <div>
        <label class="form-label">{{ 'maintenance.interventionStatus' | t }}</label>
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
        <label class="form-label">{{ 'maintenance.workshopName' | t }}</label>
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
        <h2 class="kpi-number">{{ revenue.totalRevenue | number:'1.2-2' }} â‚¬</h2>
        <div class="kpi-sub-row">
          <span style="color: var(--color-success); font-weight: 600;"><i class="pi pi-check" style="font-size: 9px;"></i> {{ 'reports.paidLabel' | t }}: {{ revenue.paidRevenue | number:'1.0-0' }} â‚¬</span>
          <span style="color: var(--color-danger); font-weight: 600;"><i class="pi pi-exclamation-circle" style="font-size: 9px;"></i> {{ 'reports.dueLabel' | t }}: {{ revenue.unpaidRevenue | number:'1.0-0' }} â‚¬</span>
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
            <td style="color: var(--color-success); font-weight: 600;">{{ item.revenue | number:'1.2-2' }} â‚¬</td>
            <td style="color: var(--color-text-secondary); font-size: 12px;">{{ item.maintenanceCost | number:'1.2-2' }} â‚¬</td>
            <td style="color: var(--color-text-secondary); font-size: 12px;">{{ item.fuelCost | number:'1.2-2' }} â‚¬</td>
            <td style="color: var(--color-text-secondary); font-size: 12px;">{{ item.insuranceCost | number:'1.2-2' }} â‚¬</td>
            <td style="color: var(--color-warning); font-weight: 600;">{{ item.totalCost | number:'1.2-2' }} â‚¬</td>
            <td style="font-weight: 800; font-size: 14px;" [class.text-success]="item.profitability >= 0" [class.text-danger]="item.profitability < 0">
              {{ item.profitability | number:'1.2-2' }} â‚¬
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
              <td style="text-align: right; font-weight: 800; color: var(--color-accent);">{{ client.totalRevenue | number:'1.2-2' }} â‚¬</td>
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
              <td style="text-align: right; font-weight: 700; color: var(--color-danger);">{{ c.finalAmountDue | number:'1.2-2' }} â‚¬</td>
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
                <td>{{ item.policy.premiumAmount }} â‚¬ / {{ item.policy.insuredValue }} â‚¬</td>
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
                <td>{{ item.inspection.cost }} â‚¬</td>
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
                <td>{{ log.log.liters }} L ({{ log.log.costPerLiter }} â‚¬/L)</td>
                <td class="font-semibold">{{ log.log.totalCost }} â‚¬</td>
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
            <label class="form-label">{{ 'vehicles.brand' | t }}</label>
            <input type="text" [(ngModel)]="vehicleForm.brand" name="brand" required class="form-input" placeholder="Renault"/>
          </div>
          <div class="form-group">
            <label class="form-label">{{ 'vehicles.model' | t }}</label>
            <input type="text" [(ngModel)]="vehicleForm.model" name="model" required class="form-input" placeholder="Clio 5"/>
          </div>
        </div>
        
        <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 12px;">
          <div class="form-group">
            <label class="form-label">{{ 'vehicles.matricule' | t }}</label>
            <input type="text" [(ngModel)]="vehicleForm.matricule" name="matricule" required class="form-input" placeholder="12345-120-16"/>
          </div>
          <div class="form-group">
            <label class="form-label">{{ 'vehicles.vin' | t }}</label>
            <input type="text" [(ngModel)]="vehicleForm.vin" name="vin" required class="form-input" placeholder="VF123456..."/>
          </div>
        </div>
        
        <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 12px;">
          <div class="form-group">
            <label class="form-label">{{ 'vehicles.year' | t }}</label>
            <input type="number" [(ngModel)]="vehicleForm.year" name="year" required class="form-input"/>
          </div>
          <div class="form-group">
            <label class="form-label">{{ 'vehicles.color' | t }}</label>
            <input type="text" [(ngModel)]="vehicleForm.color" name="color" class="form-input"/>
          </div>
        </div>
        
        <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 12px;">
          <div class="form-group">
            <label class="form-label">{{ 'common.type' | t }}</label>
            <select [(ngModel)]="vehicleForm.type" name="type" required class="form-input">
              <option *ngFor="let t of vehicleTypes" [value]="t">{{ t }}</option>
            </select>
          </div>
          <div class="form-group">
            <label class="form-label">{{ 'vehicles.seats' | t }}</label>
            <input type="number" [(ngModel)]="vehicleForm.seatsCount" name="seatsCount" required class="form-input"/>
          </div>
        </div>
        
        <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 12px;">
          <div class="form-group">
            <label class="form-label">{{ 'vehicles.fuelType' | t }}</label>
            <select [(ngModel)]="vehicleForm.fuelType" name="fuelType" required class="form-input">
              <option *ngFor="let f of fuelTypes" [value]="f">{{ f }}</option>
            </select>
          </div>
          <div class="form-group">
            <label class="form-label">{{ 'vehicles.transmission' | t }}</label>
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
            <label class="form-label">{{ 'vehicles.dailyRate' | t }}</label>
            <input type="number" [(ngModel)]="vehicleForm.dailyRate" name="dailyRate" required class="form-input"/>
          </div>
          <div class="form-group">
            <label class="form-label">{{ 'vehicles.purchasePrice' | t }}</label>
            <input type="number" [(ngModel)]="vehicleForm.purchasePrice" name="purchasePrice" required class="form-input"/>
          </div>
        </div>

        <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 12px;">
          <div class="form-group">
            <label class="form-label">{{ 'vehicles.engineNumber' | t }}</label>
            <input type="text" [(ngModel)]="vehicleForm.engineNumber" name="engineNumber" class="form-input"/>
          </div>
          <div class="form-group">
            <label class="form-label">{{ 'vehicles.initialKm' | t }}</label>
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
        <label class="form-label">{{ 'vehicles.insurer' | t }}</label>
        <input type="text" [(ngModel)]="policyForm.insurerName" name="insurerName" required class="form-input"/>
      </div>
      <div class="form-group">
        <label class="form-label">{{ 'vehicles.policyNo' | t }}</label>
        <input type="text" [(ngModel)]="policyForm.policyNumber" name="policyNumber" required class="form-input"/>
      </div>
      <div class="form-row">
        <div class="form-group">
          <label class="form-label">{{ 'vehicles.coverage' | t }}</label>
          <select [(ngModel)]="policyForm.coverageType" name="coverageType" required class="form-input">
            <option value="Third-Party">{{ 'vehicles.coverageThirdParty' | t }}</option>
            <option value="Comprehensive">{{ 'vehicles.coverageComprehensive' | t }}</option>
            <option value="Fleet">{{ 'vehicles.coverageFleet' | t }}</option>
          </select>
        </div>
        <div class="form-group">
          <label class="form-label">{{ 'vehicles.premium' | t }}</label>
          <input type="number" [(ngModel)]="policyForm.premiumAmount" name="premiumAmount" required class="form-input"/>
        </div>
      </div>
      <div class="form-row">
        <div class="form-group">
          <label class="form-label">{{ 'vehicles.effectiveDate' | t }}</label>
          <p-datepicker [(ngModel)]="policyForm.startDate" name="startDate" dateFormat="yy-mm-dd" [showIcon]="true" styleClass="w-full"></p-datepicker>
        </div>
        <div class="form-group">
          <label class="form-label">{{ 'vehicles.expiration' | t }}</label>
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
          <label class="form-label">{{ 'vehicles.inspectionDate' | t }}</label>
          <p-datepicker [(ngModel)]="inspectionForm.inspectionDate" name="inspectionDate" dateFormat="yy-mm-dd" [showIcon]="true" styleClass="w-full"></p-datepicker>
        </div>
        <div class="form-group">
          <label class="form-label">{{ 'vehicles.expiration' | t }}</label>
          <p-datepicker [(ngModel)]="inspectionForm.expiryDate" name="expiryDate" dateFormat="yy-mm-dd" [showIcon]="true" styleClass="w-full"></p-datepicker>
        </div>
      </div>
      <div class="form-row">
        <div class="form-group">
          <label class="form-label">{{ 'vehicles.result' | t }}</label>
          <select [(ngModel)]="inspectionForm.result" name="result" required class="form-input">
            <option value="Pass">{{ 'vehicles.resultFavorable' | t }}</option>
            <option value="Conditional">{{ 'vehicles.resultCounterVisit' | t }}</option>
            <option value="Fail">{{ 'vehicles.resultUnfavorable' | t }}</option>
          </select>
        </div>
        <div class="form-group">
          <label class="form-label">{{ 'common.cost' | t }} (â‚¬)</label>
          <input type="number" [(ngModel)]="inspectionForm.cost" name="cost" required class="form-input"/>
        </div>
      </div>
      <div class="form-group">
        <label class="form-label">{{ 'vehicles.center' | t }}</label>
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
          <label class="form-label">{{ 'common.date' | t }}</label>
          <p-datepicker [(ngModel)]="fuelForm.date" name="date" dateFormat="yy-mm-dd" [showIcon]="true" styleClass="w-full"></p-datepicker>
        </div>
        <div class="form-group">
          <label class="form-label">{{ 'vehicles.counterKm' | t }}</label>
          <input type="number" [(ngModel)]="fuelForm.kmValue" name="kmValue" required class="form-input"/>
        </div>
      </div>
      <div class="form-row">
        <div class="form-group">
          <label class="form-label">{{ 'vehicles.volumeL' | t }}</label>
          <input type="number" [(ngModel)]="fuelForm.liters" name="liters" required class="form-input"/>
        </div>
        <div class="form-group">
          <label class="form-label">{{ 'vehicles.pricePerL' | t }}</label>
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
        <label class="form-label">{{ 'common.date' | t }}</label>
        <p-datepicker [(ngModel)]="kmForm.date" name="date" dateFormat="yy-mm-dd" [showIcon]="true" styleClass="w-full"></p-datepicker>
      </div>
      <div class="form-group">
        <label class="form-label">{{ 'vehicles.counterKm' | t }}</label>
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


