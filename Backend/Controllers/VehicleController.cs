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
    public class VehicleController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _env;

        public VehicleController(DataContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? search,
            [FromQuery] string? type,
            [FromQuery] string? fuelType,
            [FromQuery] string? status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _context.Vehicles.AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                var lowerSearch = search.ToLower();
                query = query.Where(v => v.Brand.ToLower().Contains(lowerSearch) ||
                                         v.Model.ToLower().Contains(lowerSearch) ||
                                         v.Matricule.ToLower().Contains(lowerSearch) ||
                                         v.VIN.ToLower().Contains(lowerSearch));
            }

            // Filters
            if (!string.IsNullOrWhiteSpace(type))
            {
                query = query.Where(v => v.Type.ToLower() == type.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(fuelType))
            {
                query = query.Where(v => v.FuelType.ToLower() == fuelType.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                if (Enum.TryParse<VehicleStatus>(status, true, out var statusEnum))
                {
                    query = query.Where(v => v.Status == statusEnum);
                }
            }

            var totalCount = await query.CountAsync();
            var vehicles = await query
                .OrderByDescending(v => v.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                totalCount,
                page,
                pageSize,
                data = vehicles
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null) return NotFound(new { message = "Vehicle not found" });
            return Ok(vehicle);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Vehicle vehicle)
        {
            // Verify matricule uniqueness
            if (await _context.Vehicles.AnyAsync(v => v.Matricule == vehicle.Matricule))
            {
                return BadRequest(new { message = $"Matricule '{vehicle.Matricule}' is already registered." });
            }

            // Verify VIN uniqueness
            if (await _context.Vehicles.AnyAsync(v => v.VIN == vehicle.VIN))
            {
                return BadRequest(new { message = $"VIN '{vehicle.VIN}' is already registered." });
            }

            // Set current km to initial km initially
            vehicle.CurrentKm = vehicle.InitialKm;

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            // Auto-log initial km entry
            var kmEntry = new KmEntry
            {
                VehicleId = vehicle.Id,
                Date = vehicle.PurchaseDate,
                KmValue = vehicle.InitialKm,
                Source = "Manual",
                Notes = "Initial kilométrage recorded at registration."
            };
            _context.KmEntries.Add(kmEntry);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, vehicle);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Vehicle updated)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null) return NotFound(new { message = "Vehicle not found" });

            // Verify matricule uniqueness
            if (updated.Matricule != vehicle.Matricule && await _context.Vehicles.AnyAsync(v => v.Matricule == updated.Matricule))
            {
                return BadRequest(new { message = $"Matricule '{updated.Matricule}' is already registered." });
            }

            // Verify VIN uniqueness
            if (updated.VIN != vehicle.VIN && await _context.Vehicles.AnyAsync(v => v.VIN == updated.VIN))
            {
                return BadRequest(new { message = $"VIN '{updated.VIN}' is already registered." });
            }

            vehicle.Brand = updated.Brand;
            vehicle.Model = updated.Model;
            vehicle.Matricule = updated.Matricule;
            vehicle.Year = updated.Year;
            vehicle.Type = updated.Type;
            vehicle.FuelType = updated.FuelType;
            vehicle.Transmission = updated.Transmission;
            vehicle.VIN = updated.VIN;
            vehicle.EngineNumber = updated.EngineNumber;
            vehicle.Color = updated.Color;
            vehicle.SeatsCount = updated.SeatsCount;
            vehicle.DailyRate = updated.DailyRate;
            vehicle.Status = updated.Status;
            vehicle.PurchaseDate = updated.PurchaseDate;
            vehicle.PurchasePrice = updated.PurchasePrice;
            vehicle.Notes = updated.Notes;

            if (!string.IsNullOrWhiteSpace(updated.PhotoPath))
            {
                vehicle.PhotoPath = updated.PhotoPath;
            }

            await _context.SaveChangesAsync();
            return Ok(vehicle);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null) return NotFound(new { message = "Vehicle not found" });

            // Check if rented or has active contracts
            var hasActiveContract = await _context.RentalContracts
                .AnyAsync(c => c.VehicleId == id && (c.ContractStatus == ContractStatus.Active || c.ContractStatus == ContractStatus.Draft));

            if (hasActiveContract)
            {
                return BadRequest(new { message = "Cannot delete a vehicle with active or draft contracts." });
            }

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Vehicle deleted successfully" });
        }

        [HttpPost("upload-photo")]
        public async Task<IActionResult> UploadPhoto(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "No file uploaded." });
            }

            var uploadsFolder = Path.Combine(_env.ContentRootPath, "wwwroot", "uploads");
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

            var relativePath = $"/uploads/{fileName}";
            return Ok(new { photoPath = relativePath });
        }
    }
}
