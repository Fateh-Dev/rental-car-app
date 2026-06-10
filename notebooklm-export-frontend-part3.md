# Parc Auto - Frontend (Angular) - Part 3

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


