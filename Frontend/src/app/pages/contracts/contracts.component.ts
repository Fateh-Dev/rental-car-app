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
    { label: 'Non Payé', value: 'Unpaid' },
    { label: 'Partiellement Payé', value: 'PartiallyPaid' },
    { label: 'Payé', value: 'Paid' }
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
}
