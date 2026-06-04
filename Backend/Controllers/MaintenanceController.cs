using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MaintenanceController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _env;

        public MaintenanceController(DataContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? status, [FromQuery] int? vehicleId)
        {
            var query = _context.Maintenances
                .Include(m => m.Vehicle)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
            {
                if (Enum.TryParse<MaintenanceStatus>(status, true, out var statusEnum))
                {
                    query = query.Where(m => m.Status == statusEnum);
                }
            }

            if (vehicleId.HasValue)
            {
                query = query.Where(m => m.VehicleId == vehicleId.Value);
            }

            var list = await query.OrderByDescending(m => m.DatePerformed).ToListAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var maintenance = await _context.Maintenances
                .Include(m => m.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (maintenance == null) return NotFound(new { message = "Maintenance not found" });
            return Ok(maintenance);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Maintenance maintenance)
        {
            var vehicle = await _context.Vehicles.FindAsync(maintenance.VehicleId);
            if (vehicle == null) return BadRequest(new { message = "Vehicle not found" });

            maintenance.TotalCost = maintenance.LaborCost + maintenance.PartsCost;

            if (maintenance.Status == MaintenanceStatus.InProgress)
            {
                vehicle.Status = VehicleStatus.InMaintenance;
            }

            _context.Maintenances.Add(maintenance);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = maintenance.Id }, maintenance);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Maintenance updated)
        {
            var maintenance = await _context.Maintenances.FindAsync(id);
            if (maintenance == null) return NotFound(new { message = "Maintenance record not found" });

            var vehicle = await _context.Vehicles.FindAsync(maintenance.VehicleId);
            if (vehicle == null) return BadRequest(new { message = "Vehicle not found" });

            // Handle transition states
            if (maintenance.Status != updated.Status)
            {
                if (updated.Status == MaintenanceStatus.InProgress)
                {
                    vehicle.Status = VehicleStatus.InMaintenance;
                }
                else if (updated.Status == MaintenanceStatus.Completed)
                {
                    vehicle.Status = VehicleStatus.Available;
                }
                else if (updated.Status == MaintenanceStatus.Scheduled && maintenance.Status == MaintenanceStatus.InProgress)
                {
                    vehicle.Status = VehicleStatus.Available;
                }
            }

            maintenance.MaintenanceType = updated.MaintenanceType;
            maintenance.DatePerformed = updated.DatePerformed;
            maintenance.NextScheduledDate = updated.NextScheduledDate;
            maintenance.KmAtMaintenance = updated.KmAtMaintenance;
            maintenance.WorkshopName = updated.WorkshopName;
            maintenance.WorkshopAddress = updated.WorkshopAddress;
            maintenance.WorkshopContact = updated.WorkshopContact;
            maintenance.Description = updated.Description;
            maintenance.LaborCost = updated.LaborCost;
            maintenance.PartsCost = updated.PartsCost;
            maintenance.TotalCost = updated.LaborCost + updated.PartsCost;
            maintenance.InvoiceNumber = updated.InvoiceNumber;
            maintenance.Status = updated.Status;

            if (!string.IsNullOrWhiteSpace(updated.InvoiceFilePath))
            {
                maintenance.InvoiceFilePath = updated.InvoiceFilePath;
            }

            await _context.SaveChangesAsync();
            return Ok(maintenance);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var maintenance = await _context.Maintenances.FindAsync(id);
            if (maintenance == null) return NotFound(new { message = "Maintenance record not found" });

            var vehicle = await _context.Vehicles.FindAsync(maintenance.VehicleId);
            if (vehicle != null && maintenance.Status == MaintenanceStatus.InProgress)
            {
                vehicle.Status = VehicleStatus.Available; // Return vehicle back if servicing cancelled
            }

            _context.Maintenances.Remove(maintenance);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Maintenance record deleted" });
        }

        [HttpGet("calendar")]
        public async Task<IActionResult> GetCalendar()
        {
            // Get all upcoming scheduled events
            var events = await _context.Maintenances
                .Include(m => m.Vehicle)
                .Where(m => m.Status == MaintenanceStatus.Scheduled || (m.NextScheduledDate != null && m.NextScheduledDate >= DateTime.UtcNow.AddMonths(-1)))
                .Select(m => new
                {
                    m.Id,
                    Title = $"{m.MaintenanceType} - {(m.Vehicle != null ? m.Vehicle.Brand + " " + m.Vehicle.Model : "Unknown Vehicle")}",
                    Start = m.Status == MaintenanceStatus.Scheduled ? m.DatePerformed : m.NextScheduledDate,
                    End = m.Status == MaintenanceStatus.Scheduled ? m.DatePerformed.AddHours(2) : m.NextScheduledDate.Value.AddHours(2),
                    m.Status,
                    m.MaintenanceType,
                    VehicleMatricule = m.Vehicle != null ? m.Vehicle.Matricule : ""
                })
                .ToListAsync();

            return Ok(events);
        }

        [HttpPost("upload-invoice")]
        public async Task<IActionResult> UploadInvoice(IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest(new { message = "No file uploaded" });

            var uploadsFolder = Path.Combine(_env.ContentRootPath, "wwwroot", "uploads", "invoices");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var relativePath = $"/uploads/invoices/{fileName}";
            return Ok(new { invoiceFilePath = relativePath });
        }
    }
}
