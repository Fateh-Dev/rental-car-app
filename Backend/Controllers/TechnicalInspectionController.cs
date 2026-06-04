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
    public class TechnicalInspectionController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _env;

        public TechnicalInspectionController(DataContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet("vehicle/{vehicleId}")]
        public async Task<IActionResult> GetInspectionsByVehicle(int vehicleId)
        {
            var inspections = await _context.TechnicalInspections
                .Where(ti => ti.VehicleId == vehicleId)
                .OrderByDescending(ti => ti.ExpiryDate)
                .ToListAsync();

            var current = inspections.FirstOrDefault();
            var settings = await _context.GlobalSettings.FirstOrDefaultAsync() ?? new GlobalSettings();
            var thresholds = settings.InspectionExpiryDaysThresholds.Split(',').Select(int.Parse).ToList();

            var report = inspections.Select(i => {
                var daysRemaining = (i.ExpiryDate.Date - DateTime.UtcNow.Date).Days;
                string status = "Valid";
                if (i.Result.ToLower() == "fail")
                {
                    status = "Failed";
                }
                else if (daysRemaining < 0)
                {
                    status = "Expired";
                }
                else if (thresholds.Any(t => daysRemaining <= t))
                {
                    status = "ExpiringSoon";
                }

                return new
                {
                    Inspection = i,
                    DaysRemaining = daysRemaining,
                    Status = status
                };
            }).ToList();

            return Ok(new { current = report.FirstOrDefault(), history = report.Skip(1).ToList(), all = report });
        }

        [HttpPost]
        public async Task<IActionResult> AddInspection([FromBody] TechnicalInspection inspection)
        {
            var vehicle = await _context.Vehicles.FindAsync(inspection.VehicleId);
            if (vehicle == null) return BadRequest(new { message = "Vehicle not found" });

            _context.TechnicalInspections.Add(inspection);
            await _context.SaveChangesAsync();
            return Ok(inspection);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInspection(int id, [FromBody] TechnicalInspection updated)
        {
            var inspection = await _context.TechnicalInspections.FindAsync(id);
            if (inspection == null) return NotFound(new { message = "Technical inspection not found" });

            inspection.InspectionDate = updated.InspectionDate;
            inspection.ExpiryDate = updated.ExpiryDate;
            inspection.Result = updated.Result;
            inspection.CenterName = updated.CenterName;
            inspection.CenterAddress = updated.CenterAddress;
            inspection.Cost = updated.Cost;
            inspection.Remarks = updated.Remarks;

            if (!string.IsNullOrWhiteSpace(updated.DocumentPath))
            {
                inspection.DocumentPath = updated.DocumentPath;
            }

            await _context.SaveChangesAsync();
            return Ok(inspection);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInspection(int id)
        {
            var inspection = await _context.TechnicalInspections.FindAsync(id);
            if (inspection == null) return NotFound(new { message = "Inspection not found" });

            _context.TechnicalInspections.Remove(inspection);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Technical inspection record deleted" });
        }

        [HttpPost("upload-inspection")]
        public async Task<IActionResult> UploadInspection(IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest(new { message = "No file uploaded" });

            var uploadsFolder = Path.Combine(_env.ContentRootPath, "wwwroot", "uploads", "inspections");
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

            var relativePath = $"/uploads/inspections/{fileName}";
            return Ok(new { documentPath = relativePath });
        }
    }
}
