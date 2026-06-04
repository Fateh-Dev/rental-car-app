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
    public class InsuranceController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _env;

        public InsuranceController(DataContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet("vehicle/{vehicleId}")]
        public async Task<IActionResult> GetPoliciesByVehicle(int vehicleId)
        {
            var policies = await _context.InsurancePolicies
                .Where(ip => ip.VehicleId == vehicleId)
                .OrderByDescending(ip => ip.ExpiryDate)
                .ToListAsync();

            var current = policies.FirstOrDefault();
            var settings = await _context.GlobalSettings.FirstOrDefaultAsync() ?? new GlobalSettings();
            var thresholds = settings.InsuranceExpiryDaysThresholds.Split(',').Select(int.Parse).ToList();

            var report = policies.Select(p => {
                var daysRemaining = (p.ExpiryDate.Date - DateTime.UtcNow.Date).Days;
                string status = "Valid";
                if (daysRemaining < 0)
                {
                    status = "Expired";
                }
                else if (thresholds.Any(t => daysRemaining <= t))
                {
                    status = "ExpiringSoon";
                }

                return new
                {
                    Policy = p,
                    DaysRemaining = daysRemaining,
                    Status = status
                };
            }).ToList();

            return Ok(new { current = report.FirstOrDefault(), history = report.Skip(1).ToList(), all = report });
        }

        [HttpPost]
        public async Task<IActionResult> AddPolicy([FromBody] InsurancePolicy policy)
        {
            var vehicle = await _context.Vehicles.FindAsync(policy.VehicleId);
            if (vehicle == null) return BadRequest(new { message = "Vehicle not found" });

            _context.InsurancePolicies.Add(policy);
            await _context.SaveChangesAsync();
            return Ok(policy);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePolicy(int id, [FromBody] InsurancePolicy updated)
        {
            var policy = await _context.InsurancePolicies.FindAsync(id);
            if (policy == null) return NotFound(new { message = "Insurance policy not found" });

            policy.InsurerName = updated.InsurerName;
            policy.PolicyNumber = updated.PolicyNumber;
            policy.CoverageType = updated.CoverageType;
            policy.StartDate = updated.StartDate;
            policy.ExpiryDate = updated.ExpiryDate;
            policy.PremiumAmount = updated.PremiumAmount;
            policy.InsuredValue = updated.InsuredValue;
            policy.AgentContact = updated.AgentContact;

            if (!string.IsNullOrWhiteSpace(updated.DocumentPath))
            {
                policy.DocumentPath = updated.DocumentPath;
            }

            await _context.SaveChangesAsync();
            return Ok(policy);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePolicy(int id)
        {
            var policy = await _context.InsurancePolicies.FindAsync(id);
            if (policy == null) return NotFound(new { message = "Policy not found" });

            _context.InsurancePolicies.Remove(policy);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Insurance policy deleted" });
        }

        [HttpPost("upload-policy")]
        public async Task<IActionResult> UploadPolicy(IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest(new { message = "No file uploaded" });

            var uploadsFolder = Path.Combine(_env.ContentRootPath, "wwwroot", "uploads", "policies");
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

            var relativePath = $"/uploads/policies/{fileName}";
            return Ok(new { documentPath = relativePath });
        }
    }
}
