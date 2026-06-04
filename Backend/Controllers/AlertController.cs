using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AlertController : ControllerBase
    {
        private readonly DataContext _context;

        public AlertController(DataContext context)
        {
            _context = context;
        }

        public class AlertItem
        {
            public string Type { get; set; } = string.Empty; // Maintenance, Consumable, Insurance, Inspection, OdometerInactivity, DriverLicense
            public string Severity { get; set; } = "Info"; // Info, Warning, Critical
            public string Target { get; set; } = string.Empty; // Vehicle plate or Client name
            public string Message { get; set; } = string.Empty;
            public int? VehicleId { get; set; }
            public int? ClientId { get; set; }
            public string DaysOrKmLeftText { get; set; } = string.Empty;
            public DateTime DateConcerned { get; set; }
        }

        [HttpGet]
        public async Task<IActionResult> GetAlerts()
        {
            var alerts = new List<AlertItem>();
            var settings = await _context.GlobalSettings.FirstOrDefaultAsync() ?? new GlobalSettings();
            var vehicles = await _context.Vehicles.ToListAsync();
            var clients = await _context.Clients.ToListAsync();

            var today = DateTime.UtcNow.Date;

            // 1. Insurance Expiry
            var insuranceThresholds = settings.InsuranceExpiryDaysThresholds.Split(',').Select(int.Parse).ToList();
            var maxInsuranceThreshold = insuranceThresholds.Max();

            var policies = await _context.InsurancePolicies
                .GroupBy(p => p.VehicleId)
                .Select(g => g.OrderByDescending(p => p.ExpiryDate).FirstOrDefault())
                .ToListAsync();

            foreach (var policy in policies)
            {
                if (policy == null) continue;
                var vehicle = vehicles.FirstOrDefault(v => v.Id == policy.VehicleId);
                var daysRemaining = (policy.ExpiryDate.Date - today).Days;

                if (daysRemaining < 0)
                {
                    alerts.Add(new AlertItem
                    {
                        Type = "Insurance",
                        Severity = "Critical",
                        Target = vehicle?.Matricule ?? $"Vehicle #{policy.VehicleId}",
                        Message = $"Insurance expired on {policy.ExpiryDate:yyyy-MM-dd}.",
                        VehicleId = policy.VehicleId,
                        DaysOrKmLeftText = $"{Math.Abs(daysRemaining)} days overdue",
                        DateConcerned = policy.ExpiryDate
                    });
                }
                else if (daysRemaining <= maxInsuranceThreshold)
                {
                    string severity = "Info";
                    if (daysRemaining <= 7) severity = "Critical";
                    else if (daysRemaining <= 15) severity = "Warning";

                    alerts.Add(new AlertItem
                    {
                        Type = "Insurance",
                        Severity = severity,
                        Target = vehicle?.Matricule ?? $"Vehicle #{policy.VehicleId}",
                        Message = $"Insurance expiring soon in {daysRemaining} days.",
                        VehicleId = policy.VehicleId,
                        DaysOrKmLeftText = $"{daysRemaining} days remaining",
                        DateConcerned = policy.ExpiryDate
                    });
                }
            }

            // 2. Technical Inspection Expiry
            var inspectionThresholds = settings.InspectionExpiryDaysThresholds.Split(',').Select(int.Parse).ToList();
            var maxInspectionThreshold = inspectionThresholds.Max();

            var inspections = await _context.TechnicalInspections
                .GroupBy(i => i.VehicleId)
                .Select(g => g.OrderByDescending(i => i.ExpiryDate).FirstOrDefault())
                .ToListAsync();

            foreach (var inspection in inspections)
            {
                if (inspection == null) continue;
                var vehicle = vehicles.FirstOrDefault(v => v.Id == inspection.VehicleId);
                var daysRemaining = (inspection.ExpiryDate.Date - today).Days;

                if (inspection.Result.ToLower() == "fail")
                {
                    alerts.Add(new AlertItem
                    {
                        Type = "Inspection",
                        Severity = "Critical",
                        Target = vehicle?.Matricule ?? $"Vehicle #{inspection.VehicleId}",
                        Message = "Technical inspection failed! Vehicle requires repairs.",
                        VehicleId = inspection.VehicleId,
                        DaysOrKmLeftText = "FAILED",
                        DateConcerned = inspection.InspectionDate
                    });
                }
                else if (daysRemaining < 0)
                {
                    alerts.Add(new AlertItem
                    {
                        Type = "Inspection",
                        Severity = "Critical",
                        Target = vehicle?.Matricule ?? $"Vehicle #{inspection.VehicleId}",
                        Message = $"Technical inspection expired on {inspection.ExpiryDate:yyyy-MM-dd}.",
                        VehicleId = inspection.VehicleId,
                        DaysOrKmLeftText = $"{Math.Abs(daysRemaining)} days overdue",
                        DateConcerned = inspection.ExpiryDate
                    });
                }
                else if (daysRemaining <= maxInspectionThreshold)
                {
                    string severity = "Info";
                    if (daysRemaining <= 7) severity = "Critical";
                    else if (daysRemaining <= 15) severity = "Warning";

                    alerts.Add(new AlertItem
                    {
                        Type = "Inspection",
                        Severity = severity,
                        Target = vehicle?.Matricule ?? $"Vehicle #{inspection.VehicleId}",
                        Message = $"Technical inspection expiring in {daysRemaining} days.",
                        VehicleId = inspection.VehicleId,
                        DaysOrKmLeftText = $"{daysRemaining} days remaining",
                        DateConcerned = inspection.ExpiryDate
                    });
                }
            }

            // 3. Maintenance Due
            var scheduledMaintenances = await _context.Maintenances
                .Where(m => m.Status == MaintenanceStatus.Scheduled)
                .ToListAsync();

            foreach (var maint in scheduledMaintenances)
            {
                var vehicle = vehicles.FirstOrDefault(v => v.Id == maint.VehicleId);
                var daysRemaining = (maint.DatePerformed.Date - today).Days;

                if (daysRemaining < 0)
                {
                    alerts.Add(new AlertItem
                    {
                        Type = "Maintenance",
                        Severity = "Critical",
                        Target = vehicle?.Matricule ?? $"Vehicle #{maint.VehicleId}",
                        Message = $"Scheduled {maint.MaintenanceType} is overdue by {Math.Abs(daysRemaining)} days.",
                        VehicleId = maint.VehicleId,
                        DaysOrKmLeftText = $"{Math.Abs(daysRemaining)} days overdue",
                        DateConcerned = maint.DatePerformed
                    });
                }
                else if (daysRemaining <= settings.MaintenanceNotifyDaysBefore)
                {
                    alerts.Add(new AlertItem
                    {
                        Type = "Maintenance",
                        Severity = "Warning",
                        Target = vehicle?.Matricule ?? $"Vehicle #{maint.VehicleId}",
                        Message = $"Upcoming scheduled {maint.MaintenanceType} in {daysRemaining} days.",
                        VehicleId = maint.VehicleId,
                        DaysOrKmLeftText = $"{daysRemaining} days left",
                        DateConcerned = maint.DatePerformed
                    });
                }
            }

            // 4. Odometer Inactivity
            var allKmLogs = await _context.KmEntries.ToListAsync();
            foreach (var vehicle in vehicles)
            {
                var lastLog = allKmLogs
                    .Where(ke => ke.VehicleId == vehicle.Id)
                    .OrderByDescending(ke => ke.Date)
                    .FirstOrDefault();

                var lastDate = lastLog?.Date ?? vehicle.PurchaseDate;
                var daysInactive = (DateTime.UtcNow - lastDate).Days;

                if (daysInactive > settings.KmInactivityDaysThreshold)
                {
                    alerts.Add(new AlertItem
                    {
                        Type = "OdometerInactivity",
                        Severity = "Warning",
                        Target = vehicle.Matricule,
                        Message = $"No mileage activity registered for {daysInactive} days.",
                        VehicleId = vehicle.Id,
                        DaysOrKmLeftText = $"{daysInactive} days inactive",
                        DateConcerned = lastDate
                    });
                }
            }

            // 5. Driver's License Expiry
            foreach (var client in clients)
            {
                var daysRemaining = (client.LicenseExpiryDate.Date - today).Days;
                if (daysRemaining < 0)
                {
                    alerts.Add(new AlertItem
                    {
                        Type = "DriverLicense",
                        Severity = "Critical",
                        Target = client.FullName,
                        Message = $"Driver's license expired on {client.LicenseExpiryDate:yyyy-MM-dd}.",
                        ClientId = client.Id,
                        DaysOrKmLeftText = $"{Math.Abs(daysRemaining)} days overdue",
                        DateConcerned = client.LicenseExpiryDate
                    });
                }
                else if (daysRemaining <= 30) // license alert fixed warning window
                {
                    alerts.Add(new AlertItem
                    {
                        Type = "DriverLicense",
                        Severity = "Warning",
                        Target = client.FullName,
                        Message = $"Driver's license expiring in {daysRemaining} days.",
                        ClientId = client.Id,
                        DaysOrKmLeftText = $"{daysRemaining} days left",
                        DateConcerned = client.LicenseExpiryDate
                    });
                }
            }

            // 6. Consumables Expiry
            var configs = await _context.ConsumableConfigs.ToListAsync();
            var consumableLogs = await _context.ConsumableLogs.ToListAsync();

            var standardTypes = new List<string>
            {
                "OilChange", "AirFilter", "OilFilter", "FuelFilter", "CabinFilter",
                "FrontBrakes", "RearBrakes", "FrontTires", "RearTires", "Battery"
            };
            var allTypes = configs.Select(c => c.ConsumableType).Union(standardTypes).Distinct();

            foreach (var vehicle in vehicles)
            {
                var vehicleLogs = consumableLogs.Where(l => l.VehicleId == vehicle.Id).ToList();

                foreach (var type in allTypes)
                {
                    var config = configs.FirstOrDefault(c => c.ConsumableType.ToLower() == type.ToLower());
                    var typeLogs = vehicleLogs.Where(l => l.ConsumableType.ToLower() == type.ToLower()).OrderByDescending(l => l.ReplacementDate).ToList();
                    var lastLog = typeLogs.FirstOrDefault();

                    int intervalKm = config?.IntervalKm ?? GetDefaultIntervalKm(type);
                    int intervalMonths = config?.IntervalMonths ?? GetDefaultIntervalMonths(type);

                    DateTime lastDate = lastLog?.ReplacementDate ?? vehicle.PurchaseDate;
                    int lastKm = lastLog?.ReplacementKm ?? vehicle.InitialKm;

                    int kmSince = vehicle.CurrentKm - lastKm;
                    int monthsSince = ((DateTime.UtcNow.Year - lastDate.Year) * 12) + DateTime.UtcNow.Month - lastDate.Month;

                    bool isDueKm = intervalKm > 0 && kmSince >= intervalKm;
                    bool isDueMonths = intervalMonths > 0 && monthsSince >= intervalMonths;

                    bool isWarningKm = intervalKm > 0 && !isDueKm && (intervalKm - kmSince) <= settings.ConsumableNotifyKmBefore;
                    bool isWarningMonths = intervalMonths > 0 && !isDueMonths && ((lastDate.AddMonths(intervalMonths) - DateTime.UtcNow).Days <= settings.ConsumableNotifyDaysBefore);

                    if (isDueKm || isDueMonths)
                    {
                        alerts.Add(new AlertItem
                        {
                            Type = "Consumable",
                            Severity = "Critical",
                            Target = vehicle.Matricule,
                            Message = $"{type} replacement is overdue. Interval: {intervalKm}km / {intervalMonths}m. Current: {kmSince}km / {monthsSince}m.",
                            VehicleId = vehicle.Id,
                            DaysOrKmLeftText = isDueKm ? $"{kmSince - intervalKm} km overdue" : $"{monthsSince - intervalMonths} months overdue",
                            DateConcerned = lastDate
                        });
                    }
                    else if (isWarningKm || isWarningMonths)
                    {
                        alerts.Add(new AlertItem
                        {
                            Type = "Consumable",
                            Severity = "Warning",
                            Target = vehicle.Matricule,
                            Message = $"{type} replacement approaching due threshold. Current: {kmSince}km / {monthsSince}m.",
                            VehicleId = vehicle.Id,
                            DaysOrKmLeftText = isWarningKm ? $"{intervalKm - kmSince} km left" : $"{(lastDate.AddMonths(intervalMonths) - DateTime.UtcNow).Days} days left",
                            DateConcerned = lastDate
                        });
                    }
                }
            }

            return Ok(new
            {
                count = alerts.Count,
                criticalCount = alerts.Count(a => a.Severity == "Critical"),
                warningCount = alerts.Count(a => a.Severity == "Warning"),
                infoCount = alerts.Count(a => a.Severity == "Info"),
                alerts = alerts.OrderByDescending(a => a.Severity == "Critical").ThenByDescending(a => a.Severity == "Warning").ToList()
            });
        }

        private static int GetDefaultIntervalKm(string type)
        {
            return type.ToLower() switch
            {
                "oilchange" => 10000,
                "oilfilter" => 10000,
                "airfilter" => 20000,
                "fuelfilter" => 30000,
                "cabinfilter" => 20000,
                "frontbrakes" => 40000,
                "rearbrakes" => 60000,
                "fronttires" => 50000,
                "reartires" => 50000,
                _ => 0
            };
        }

        private static int GetDefaultIntervalMonths(string type)
        {
            return type.ToLower() switch
            {
                "oilchange" => 12,
                "battery" => 36,
                "fronttires" => 48,
                "reartires" => 48,
                _ => 0
            };
        }
    }
}
