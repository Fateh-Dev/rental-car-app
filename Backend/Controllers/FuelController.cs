using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FuelController : ControllerBase
    {
        private readonly DataContext _context;

        public FuelController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("vehicle/{vehicleId}")]
        public async Task<IActionResult> GetVehicleLogs(int vehicleId)
        {
            var logs = await _context.FuelLogs
                .Where(f => f.VehicleId == vehicleId)
                .OrderBy(f => f.KmValue)
                .ToListAsync();

            // Calculate L/100km for each log
            var result = logs.Select((log, index) =>
            {
                decimal consumption = 0;
                int kmDiff = 0;

                if (index > 0)
                {
                    var prevLog = logs[index - 1];
                    kmDiff = log.KmValue - prevLog.KmValue;
                    if (kmDiff > 0)
                    {
                        consumption = (log.Liters / kmDiff) * 100;
                    }
                }

                return new
                {
                    Log = log,
                    KmDrivenSinceLastFill = kmDiff,
                    ConsumptionL100 = Math.Round(consumption, 2)
                };
            }).OrderByDescending(r => r.Log.Date).ToList();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddLog([FromBody] FuelLog log)
        {
            var vehicle = await _context.Vehicles.FindAsync(log.VehicleId);
            if (vehicle == null) return BadRequest(new { message = "Vehicle not found" });

            if (log.KmValue < vehicle.CurrentKm)
            {
                return BadRequest(new { message = $"Odometer value ({log.KmValue} km) cannot be less than current vehicle odometer ({vehicle.CurrentKm} km)." });
            }

            log.TotalCost = log.Liters * log.CostPerLiter;

            // Get historical logs to detect anomalies
            var existingLogs = await _context.FuelLogs
                .Where(f => f.VehicleId == log.VehicleId)
                .OrderBy(f => f.KmValue)
                .ToListAsync();

            if (existingLogs.Count > 0)
            {
                // Calculate new log consumption
                var lastLog = existingLogs.Last();
                int kmDiff = log.KmValue - lastLog.KmValue;
                if (kmDiff > 0)
                {
                    decimal currentConsumption = (log.Liters / kmDiff) * 100;

                    // Calculate average consumption from past logs
                    var consumptions = new List<decimal>();
                    for (int i = 1; i < existingLogs.Count; i++)
                    {
                        var diff = existingLogs[i].KmValue - existingLogs[i - 1].KmValue;
                        if (diff > 0)
                        {
                            consumptions.Add((existingLogs[i].Liters / diff) * 100);
                        }
                    }

                    if (consumptions.Count > 0)
                    {
                        var average = consumptions.Average();
                        // Deviaiton > 30% is flagged as anomaly
                        if (currentConsumption > average * 1.3m || currentConsumption < average * 0.7m)
                        {
                            log.IsAnomaly = true;
                        }
                    }
                }
            }

            _context.FuelLogs.Add(log);

            // Update vehicle current odometer if higher
            if (log.KmValue > vehicle.CurrentKm)
            {
                vehicle.CurrentKm = log.KmValue;
                var kmEntry = new KmEntry
                {
                    VehicleId = vehicle.Id,
                    Date = log.Date,
                    KmValue = log.KmValue,
                    Source = "Manual",
                    Notes = $"Odometer updated via fuel fill-up log."
                };
                _context.KmEntries.Add(kmEntry);
            }

            await _context.SaveChangesAsync();
            return Ok(log);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLog(int id)
        {
            var log = await _context.FuelLogs.FindAsync(id);
            if (log == null) return NotFound(new { message = "Fuel log not found" });

            _context.FuelLogs.Remove(log);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Fuel log deleted" });
        }

        [HttpGet("export-csv")]
        public async Task<IActionResult> ExportCsv([FromQuery] int? vehicleId)
        {
            var query = _context.FuelLogs.Include(f => f.Vehicle).AsQueryable();
            if (vehicleId.HasValue)
            {
                query = query.Where(f => f.VehicleId == vehicleId.Value);
            }

            var logs = await query.OrderBy(f => f.KmValue).ToListAsync();

            var builder = new StringBuilder();
            builder.AppendLine("ID,Vehicle,Matricule,Date,Odometer (Km),Liters,Cost/L,Total Cost,Station,Fuel Type,Anomaly");

            for (int i = 0; i < logs.Count; i++)
            {
                var item = logs[i];
                var vehicleName = item.Vehicle != null ? $"{item.Vehicle.Brand} {item.Vehicle.Model}" : "";
                var matricule = item.Vehicle != null ? item.Vehicle.Matricule : "";
                builder.AppendLine($"{item.Id},\"{vehicleName}\",\"{matricule}\",\"{item.Date:yyyy-MM-dd HH:mm}\",{item.KmValue},{item.Liters},{item.CostPerLiter},{item.TotalCost},\"{item.StationName}\",\"{item.FuelType}\",{item.IsAnomaly}");
            }

            var csvBytes = Encoding.UTF8.GetBytes(builder.ToString());
            return File(csvBytes, "text/csv", $"Rapport_Carburant_{(vehicleId.HasValue ? $"Vehicule_{vehicleId}" : "Total")}_{DateTime.UtcNow:yyyyMMdd}.csv");
        }
    }
}
