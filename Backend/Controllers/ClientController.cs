using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly DataContext _context;

        public ClientController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _context.Clients.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var lowerSearch = search.ToLower();
                query = query.Where(c => c.FullName.ToLower().Contains(lowerSearch) ||
                                         c.Phone.ToLower().Contains(lowerSearch) ||
                                         c.Email.ToLower().Contains(lowerSearch) ||
                                         c.NationalId.ToLower().Contains(lowerSearch) ||
                                         c.LicenseNumber.ToLower().Contains(lowerSearch));
            }

            var totalCount = await query.CountAsync();
            var clients = await query
                .OrderByDescending(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                totalCount,
                page,
                pageSize,
                data = clients
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound(new { message = "Client not found" });

            // Include history
            var history = await _context.RentalContracts
                .Where(c => c.ClientId == id)
                .Select(c => new
                {
                    c.Id,
                    c.ContractNumber,
                    c.VehicleId,
                    VehicleBrand = c.Vehicle != null ? c.Vehicle.Brand : "",
                    VehicleModel = c.Vehicle != null ? c.Vehicle.Model : "",
                    VehicleMatricule = c.Vehicle != null ? c.Vehicle.Matricule : "",
                    c.StartDate,
                    c.ExpectedReturnDate,
                    c.ActualReturnDate,
                    c.FinalAmountDue,
                    c.ContractStatus,
                    c.PaymentStatus
                })
                .OrderByDescending(c => c.StartDate)
                .ToListAsync();

            return Ok(new
            {
                client,
                history
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Client client)
        {
            // Verify national ID uniqueness
            if (await _context.Clients.AnyAsync(c => c.NationalId == client.NationalId))
            {
                return BadRequest(new { message = $"Client with National ID / Passport '{client.NationalId}' is already registered." });
            }

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = client.Id }, new { client });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Client updated)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound(new { message = "Client not found" });

            if (updated.NationalId != client.NationalId && await _context.Clients.AnyAsync(c => c.NationalId == updated.NationalId))
            {
                return BadRequest(new { message = $"Client with National ID / Passport '{updated.NationalId}' is already registered." });
            }

            client.FullName = updated.FullName;
            client.NationalId = updated.NationalId;
            client.DateOfBirth = updated.DateOfBirth;
            client.LicenseNumber = updated.LicenseNumber;
            client.LicenseCategory = updated.LicenseCategory;
            client.LicenseIssueDate = updated.LicenseIssueDate;
            client.LicenseExpiryDate = updated.LicenseExpiryDate;
            client.Phone = updated.Phone;
            client.Email = updated.Email;
            client.Address = updated.Address;
            client.Notes = updated.Notes;

            await _context.SaveChangesAsync();
            return Ok(client);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound(new { message = "Client not found" });

            // Check contracts
            var hasContracts = await _context.RentalContracts.AnyAsync(c => c.ClientId == id);
            if (hasContracts)
            {
                return BadRequest(new { message = "Cannot delete a client with rental history. Deactivate or log remarks instead." });
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Client deleted successfully" });
        }
    }
}
