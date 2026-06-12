import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  public readonly apiHost = window.location.port === '4200' ? 'http://localhost:5222' : '';
  public readonly baseUrl = `${this.apiHost}/api`;

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
  deleteKmEntry(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/km/${id}`);
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

  // ================= Users =================
  getUsers(search?: string, page = 1, pageSize = 10): Observable<any> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());
    if (search) params = params.set('search', search);

    return this.http.get(`${this.baseUrl}/user`, { params });
  }
  getUserById(id: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/user/${id}`);
  }
  createUser(user: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/user`, user);
  }
  updateUser(id: number, user: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/user/${id}`, user);
  }
  toggleUserLock(id: number): Observable<any> {
    return this.http.put(`${this.baseUrl}/user/${id}/lock`, {});
  }
  deleteUser(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/user/${id}`);
  }
}
