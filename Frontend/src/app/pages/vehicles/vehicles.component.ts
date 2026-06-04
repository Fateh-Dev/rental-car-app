import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api.service';
import { TableModule } from 'primeng/table';
import { DialogModule } from 'primeng/dialog';
import { SelectModule } from 'primeng/select';
import { DatePickerModule } from 'primeng/datepicker';

@Component({
  selector: 'app-vehicles',
  standalone: true,
  imports: [CommonModule, FormsModule, TableModule, DialogModule, SelectModule, DatePickerModule],
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
    { label: 'Loué', value: 'Rented' },
    { label: 'En Maintenance', value: 'InMaintenance' },
    { label: 'Réservé', value: 'Reserved' },
    { label: 'Immobilisé', value: 'Immobilized' }
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

  constructor(private api: ApiService) {}

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
    if (confirm('Êtes-vous sûr de vouloir supprimer ce véhicule de la flotte active ?')) {
      this.api.deleteVehicle(id).subscribe({
        next: () => {
          this.loadVehicles();
        },
        error: (err) => alert(err.error?.message || 'Erreur lors de la suppression du véhicule')
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
        error: (err) => alert(err.error?.message || 'Erreur lors de la mise à jour')
      });
    } else {
      this.api.createVehicle(payload).subscribe({
        next: () => {
          this.showCrudDialog = false;
          this.loadVehicles();
        },
        error: (err) => alert(err.error?.message || 'Erreur lors de l\'enregistrement')
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
