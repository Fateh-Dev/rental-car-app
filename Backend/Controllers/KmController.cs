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
    public class KmController : ControllerBase
    {
        private readonly DataContext _context;

        public KmController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("vehicle/{vehicleId}")]
        public async Task<IActionResult> GetVehicleHistory(int vehicleId)
        {
            var history = await _context.KmEntries
                .Where(k => k.VehicleId == vehicleId)
                .OrderByDescending(k => k.Date)
                .ToListAsync();

            return Ok(history);
        }

        [HttpPost]
        public async Task<IActionResult> AddManualEntry([FromBody] KmEntry entry)
        {
            var vehicle = await _context.Vehicles.FindAsync(entry.VehicleId);
            if (vehicle == null) return BadRequest(new { message = "Vehicle not found" });

            if (entry.KmValue < vehicle.CurrentKm)
            {
                return BadRequest(new { message = $"Odometer reading ({entry.KmValue} km) cannot be less than current vehicle odometer ({vehicle.CurrentKm} km)." });
            }

            entry.Source = "Manual";
            if (entry.Date == default)
            {
                entry.Date = DateTime.UtcNow;
            }

            _context.KmEntries.Add(entry);

            // Update vehicle current odometer
            vehicle.CurrentKm = entry.KmValue;

            await _context.SaveChangesAsync();
            return Ok(entry);
        }

        [HttpGet("inactivity-report")]
        public async Task<IActionResult> GetInactivityReport()
        {
            var settings = await _context.GlobalSettings.FirstOrDefaultAsync() ?? new GlobalSettings();
            var thresholdDays = settings.KmInactivityDaysThreshold;
            var targetDate = DateTime.UtcNow.AddDays(-thresholdDays);

            var inactiveVehicles = await _context.Vehicles
                .Select(v => new
                {
                    v.Id,
                    v.Matricule,
                    v.Brand,
                    v.Model,
                    v.Status,
                    v.CurrentKm,
                    LastActivityDate = _context.KmEntries
                        .Where(ke => ke.VehicleId == v.Id)
                        .Select(ke => ke.Date)
                        .OrderByDescending(d => d)
                        .FirstOrDefault()
                })
                .ToListAsync();

            var result = inactiveVehicles
                .Select(v => new
                {
                    v.Id,
                    v.Matricule,
                    v.Brand,
                    v.Model,
                    v.Status,
                    v.CurrentKm,
                    v.LastActivityDate,
                    DaysInactive = v.LastActivityDate == default ? (DateTime.UtcNow - DateTime.UtcNow).Days : (DateTime.UtcNow - v.LastActivityDate).Days
                })
                .Where(v => v.LastActivityDate == default || v.DaysInactive > thresholdDays)
                .OrderByDescending(v => v.DaysInactive)
                .ToList();

            return Ok(new
            {
                thresholdDays,
                vehicles = result
            });
        }

        [HttpGet("export-csv")]
        public async Task<IActionResult> ExportCsv([FromQuery] int? vehicleId)
        {
            var query = _context.KmEntries.Include(k => k.Vehicle).AsQueryable();
            if (vehicleId.HasValue)
            {
                query = query.Where(k => k.VehicleId == vehicleId.Value);
            }

            var entries = await query.OrderByDescending(k => k.Date).ToListAsync();

            var builder = new StringBuilder();
            builder.AppendLine("ID,Vehicle,Matricule,Date,Odometer (Km),Source,Notes");

            foreach (var item in entries)
            {
                var vehicleName = item.Vehicle != null ? $"{item.Vehicle.Brand} {item.Vehicle.Model}" : "";
                var matricule = item.Vehicle != null ? item.Vehicle.Matricule : "";
                builder.AppendLine($"{item.Id},\"{vehicleName}\",\"{matricule}\",\"{item.Date:yyyy-MM-dd HH:mm}\",{item.KmValue},\"{item.Source}\",\"{item.Notes.Replace("\"", "\"\"")}\"");
            }

            var csvBytes = Encoding.UTF8.GetBytes(builder.ToString());
            return File(csvBytes, "text/csv", $"Suivi_Kilometrage_{(vehicleId.HasValue ? $"Vehicule_{vehicleId}" : "Total")}_{DateTime.UtcNow:yyyyMMdd}.csv");
        }
    }
}
