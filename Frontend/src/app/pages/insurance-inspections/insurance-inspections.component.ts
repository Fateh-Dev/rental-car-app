import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../services/api.service';
import { TableModule } from 'primeng/table';

@Component({
  selector: 'app-insurance-inspections',
  standalone: true,
  imports: [CommonModule, TableModule],
  templateUrl: './insurance-inspections.component.html',
  styleUrls: ['./insurance-inspections.component.css']
})
export class InsuranceInspectionsComponent implements OnInit {
  policies: any[] = [];
  inspections: any[] = [];

  constructor(private api: ApiService) {}

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
