import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api.service';
import { DialogModule } from 'primeng/dialog';
import { DatePickerModule } from 'primeng/datepicker';

@Component({
  selector: 'app-maintenance',
  standalone: true,
  imports: [CommonModule, FormsModule, DialogModule, DatePickerModule],
  templateUrl: './maintenance.component.html',
  styleUrls: ['./maintenance.component.css']
})
export class MaintenanceComponent implements OnInit {
  maintenances: any[] = [];
  vehicles: any[] = [];

  // Options
  maintenanceTypes = ['Preventive', 'Corrective', 'AccidentRepair', 'Inspection'];
  statuses = [
    { label: 'Planifié', value: 'Scheduled' },
    { label: 'En Cours', value: 'InProgress' },
    { label: 'Terminé', value: 'Completed' }
  ];

  // Dialog CRUD
  showCrudDialog = false;
  isEditMode = false;
  maintenanceForm: any = this.getEmptyForm();
  uploadingInvoice = false;

  // Calendar View
  showCalendarDialog = false;
  calendarEvents: any[] = [];

  constructor(private api: ApiService) {}

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
    if (confirm('Supprimer cette fiche de maintenance ?')) {
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
        error: (err) => alert(err.error?.message || 'Erreur lors de la mise à jour')
      });
    } else {
      this.api.createMaintenance(payload).subscribe({
        next: () => {
          this.showCrudDialog = false;
          this.loadMaintenances();
        },
        error: (err) => alert(err.error?.message || 'Erreur lors de l\'enregistrement')
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
      next: (res) => {
        this.calendarEvents = res;
        this.showCalendarDialog = true;
      },
      error: (err) => console.error('Failed to load calendar events', err)
    });
  }
}
