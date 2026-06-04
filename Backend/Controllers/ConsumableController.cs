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
    public class ConsumableController : ControllerBase
    {
        private readonly DataContext _context;

        public ConsumableController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("configs")]
        public async Task<IActionResult> GetConfigs()
        {
            var configs = await _context.ConsumableConfigs.ToListAsync();
            return Ok(configs);
        }

        [HttpPost("configs")]
        public async Task<IActionResult> SaveConfig([FromBody] ConsumableConfig config)
        {
            var existing = await _context.ConsumableConfigs
                .FirstOrDefaultAsync(c => c.ConsumableType.ToLower() == config.ConsumableType.ToLower());

            if (existing != null)
            {
                existing.IntervalKm = config.IntervalKm;
                existing.IntervalMonths = config.IntervalMonths;
            }
            else
            {
                _context.ConsumableConfigs.Add(config);
            }

            await _context.SaveChangesAsync();
            return Ok(config);
        }

        [HttpGet("vehicle/{vehicleId}")]
        public async Task<IActionResult> GetVehicleStatus(int vehicleId)
        {
            var vehicle = await _context.Vehicles.FindAsync(vehicleId);
            if (vehicle == null) return NotFound(new { message = "Vehicle not found" });

            var configs = await _context.ConsumableConfigs.ToListAsync();
            var logs = await _context.ConsumableLogs
                .Where(l => l.VehicleId == vehicleId)
                .OrderByDescending(l => l.ReplacementDate)
                .ToListAsync();

            var globalSettings = await _context.GlobalSettings.FirstOrDefaultAsync() ?? new GlobalSettings();
            var kmAlertThreshold = globalSettings.ConsumableNotifyKmBefore;
            var daysAlertThreshold = globalSettings.ConsumableNotifyDaysBefore;

            // Predefined standard types if not in DB configs yet, let's create a list
            var standardTypes = new List<string>
            {
                "OilChange",
                "AirFilter", "OilFilter", "FuelFilter", "CabinFilter",
                "FrontBrakes", "RearBrakes",
                "FrontTires", "RearTires",
                "Battery"
            };

            var allTypes = configs.Select(c => c.ConsumableType).Union(standardTypes).Distinct();

            var statusReport = new List<object>();

            foreach (var type in allTypes)
            {
                var config = configs.FirstOrDefault(c => c.ConsumableType.ToLower() == type.ToLower());
                var typeLogs = logs.Where(l => l.ConsumableType.ToLower() == type.ToLower()).ToList();
                var lastLog = typeLogs.FirstOrDefault();

                // Get interval limits
                int intervalKm = config?.IntervalKm ?? GetDefaultIntervalKm(type);
                int intervalMonths = config?.IntervalMonths ?? GetDefaultIntervalMonths(type);

                // Last replacement details
                DateTime lastDate = lastLog?.ReplacementDate ?? vehicle.PurchaseDate;
                int lastKm = lastLog?.ReplacementKm ?? vehicle.InitialKm;

                // Calculations
                int kmSince = vehicle.CurrentKm - lastKm;
                int monthsSince = ((DateTime.UtcNow.Year - lastDate.Year) * 12) + DateTime.UtcNow.Month - lastDate.Month;

                bool isDueKm = false;
                bool isDueMonths = false;
                bool isWarningKm = false;
                bool isWarningMonths = false;

                if (intervalKm > 0)
                {
                    isDueKm = kmSince >= intervalKm;
                    isWarningKm = !isDueKm && (intervalKm - kmSince) <= kmAlertThreshold;
                }

                if (intervalMonths > 0)
                {
                    isDueMonths = monthsSince >= intervalMonths;
                    var remainingDays = (lastDate.AddMonths(intervalMonths) - DateTime.UtcNow).Days;
                    isWarningMonths = !isDueMonths && remainingDays <= daysAlertThreshold;
                }

                string status = "OK";
                if (isDueKm || isDueMonths)
                {
                    status = "Due";
                }
                else if (isWarningKm || isWarningMonths)
                {
                    status = "Warning";
                }

                statusReport.Add(new
                {
                    ConsumableType = type,
                    LastReplacementDate = lastLog?.ReplacementDate,
                    LastReplacementKm = lastLog?.ReplacementKm,
                    KmSinceReplacement = kmSince,
                    MonthsSinceReplacement = monthsSince,
                    IntervalKm = intervalKm,
                    IntervalMonths = intervalMonths,
                    Status = status,
                    LogsCount = typeLogs.Count,
                    Details = lastLog
                });
            }

            return Ok(new
            {
                vehicleId,
                currentKm = vehicle.CurrentKm,
                statusReport,
                logs
            });
        }

        [HttpPost("log")]
        public async Task<IActionResult> AddLog([FromBody] ConsumableLog log)
        {
            var vehicle = await _context.Vehicles.FindAsync(log.VehicleId);
            if (vehicle == null) return BadRequest(new { message = "Vehicle not found" });

            if (log.ReplacementKm < vehicle.InitialKm)
            {
                return BadRequest(new { message = $"Odometer replacement value cannot be less than vehicle's initial mileage ({vehicle.InitialKm} km)." });
            }

            // Update vehicle current odometer if replacement km is higher
            if (log.ReplacementKm > vehicle.CurrentKm)
            {
                vehicle.CurrentKm = log.ReplacementKm;
                var kmEntry = new KmEntry
                {
                    VehicleId = vehicle.Id,
                    Date = log.ReplacementDate,
                    KmValue = log.ReplacementKm,
                    Source = "Manual",
                    Notes = $"Odometer updated via consumable replacement log ({log.ConsumableType})."
                };
                _context.KmEntries.Add(kmEntry);
            }

            _context.ConsumableLogs.Add(log);
            await _context.SaveChangesAsync();
            return Ok(log);
        }

        [HttpDelete("log/{id}")]
        public async Task<IActionResult> DeleteLog(int id)
        {
            var log = await _context.ConsumableLogs.FindAsync(id);
            if (log == null) return NotFound(new { message = "Consumable replacement log not found" });

            _context.ConsumableLogs.Remove(log);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Replacement log deleted" });
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
                _ => 0 // Custom / Battery doesn't use km
            };
        }

        private static int GetDefaultIntervalMonths(string type)
        {
            return type.ToLower() switch
            {
                "oilchange" => 12,
                "battery" => 36, // 3 years
                "fronttires" => 48,
                "reartires" => 48,
                _ => 0
            };
        }
    }
}
