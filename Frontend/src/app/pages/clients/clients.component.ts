import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api.service';
import { DialogModule } from 'primeng/dialog';
import { DatePickerModule } from 'primeng/datepicker';
import { I18nService } from '../../services/i18n.service';
import { TranslatePipe } from '../../pipes/translate.pipe';
import { AppCurrencyPipe } from '../../pipes/app-currency.pipe';
import { ConfirmDialogService } from '../../services/confirm-dialog.service';

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

  constructor(private api: ApiService, public i18n: I18nService, private confirmService: ConfirmDialogService) {}

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
    this.confirmService.confirm({
      title: this.i18n.t('common.delete'),
      message: this.i18n.t('common.deleteConfirm'),
      type: 'danger',
      icon: 'pi pi-trash'
    }).then(confirmed => {
      if (confirmed) {
        this.api.deleteClient(id).subscribe({
          next: () => {
            this.loadClients();
          },
          error: (err) => alert(err.error?.message || this.i18n.t('common.errorOccurred'))
        });
      }
    });
  }

  onSubmitClient(): void {
    const payload = {
      ...this.clientForm,
      email: this.clientForm.email?.trim() || null,
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
