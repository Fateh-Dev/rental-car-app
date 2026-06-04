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
    public class ContractController : ControllerBase
    {
        private readonly DataContext _context;

        public ContractController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? search,
            [FromQuery] string? status,
            [FromQuery] string? paymentStatus,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _context.RentalContracts
                .Include(c => c.Client)
                .Include(c => c.Vehicle)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var lowerSearch = search.ToLower();
                query = query.Where(c => c.ContractNumber.ToLower().Contains(lowerSearch) ||
                                         (c.Client != null && c.Client.FullName.ToLower().Contains(lowerSearch)) ||
                                         (c.Vehicle != null && (c.Vehicle.Brand.ToLower().Contains(lowerSearch) || c.Vehicle.Model.ToLower().Contains(lowerSearch) || c.Vehicle.Matricule.ToLower().Contains(lowerSearch))));
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                if (Enum.TryParse<ContractStatus>(status, true, out var statusEnum))
                {
                    query = query.Where(c => c.ContractStatus == statusEnum);
                }
            }

            if (!string.IsNullOrWhiteSpace(paymentStatus))
            {
                if (Enum.TryParse<PaymentStatus>(paymentStatus, true, out var payEnum))
                {
                    query = query.Where(c => c.PaymentStatus == payEnum);
                }
            }

            var totalCount = await query.CountAsync();
            var contracts = await query
                .OrderByDescending(c => c.StartDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                totalCount,
                page,
                pageSize,
                data = contracts
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var contract = await _context.RentalContracts
                .Include(c => c.Client)
                .Include(c => c.Vehicle)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contract == null) return NotFound(new { message = "Contract not found" });
            return Ok(contract);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RentalContract contract)
        {
            // Verify vehicle status
            var vehicle = await _context.Vehicles.FindAsync(contract.VehicleId);
            if (vehicle == null) return BadRequest(new { message = "Vehicle not found" });

            if (contract.ContractStatus == ContractStatus.Active || contract.ContractStatus == ContractStatus.Draft)
            {
                if (vehicle.Status != VehicleStatus.Available && vehicle.Status != VehicleStatus.Reserved)
                {
                    return BadRequest(new { message = $"Vehicle is not available for rental. Current status: {vehicle.Status}" });
                }
            }

            // Verify client
            var client = await _context.Clients.FindAsync(contract.ClientId);
            if (client == null) return BadRequest(new { message = "Client not found" });

            // Warning if client's driver's license is expired
            bool isLicenseExpired = client.LicenseExpiryDate < DateTime.UtcNow.Date;

            // Generate unique Contract number if empty
            if (string.IsNullOrWhiteSpace(contract.ContractNumber))
            {
                var count = await _context.RentalContracts.CountAsync();
                contract.ContractNumber = $"CTR-{DateTime.UtcNow:yyyyMMdd}-{count + 1:D3}";
            }

            // Pre-fill fields
            contract.KmDeparture = vehicle.CurrentKm;
            contract.DailyRate = contract.DailyRate > 0 ? contract.DailyRate : vehicle.DailyRate;

            // Compute days and amounts
            var days = (contract.ExpectedReturnDate.Date - contract.StartDate.Date).Days;
            contract.RentalDays = days > 0 ? days : 1;
            contract.TotalAmount = contract.RentalDays * contract.DailyRate;
            contract.FinalAmountDue = contract.TotalAmount + contract.AdditionalCharges + contract.ExtrasCharges - contract.DiscountAmount;

            if (contract.ContractStatus == ContractStatus.Active)
            {
                vehicle.Status = VehicleStatus.Rented;

                // Log automatic km entry
                var kmEntry = new KmEntry
                {
                    VehicleId = vehicle.Id,
                    Date = contract.StartDate,
                    KmValue = contract.KmDeparture,
                    Source = "ContractStart",
                    Notes = $"Odometer recorded at start of contract {contract.ContractNumber}."
                };
                _context.KmEntries.Add(kmEntry);
            }
            else if (contract.ContractStatus == ContractStatus.Draft)
            {
                vehicle.Status = VehicleStatus.Reserved;
            }

            _context.RentalContracts.Add(contract);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = contract.Id }, new { contract, warning = isLicenseExpired ? "Client's driver's license is expired!" : null });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RentalContract updated)
        {
            var contract = await _context.RentalContracts
                .Include(c => c.Vehicle)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contract == null) return NotFound(new { message = "Contract not found" });

            var vehicle = contract.Vehicle;
            if (vehicle == null) return BadRequest(new { message = "Associated vehicle not found" });

            // Handle transition states
            if (contract.ContractStatus != updated.ContractStatus)
            {
                if (updated.ContractStatus == ContractStatus.Active)
                {
                    // Transition Draft -> Active
                    vehicle.Status = VehicleStatus.Rented;
                    contract.KmDeparture = vehicle.CurrentKm;

                    var kmEntry = new KmEntry
                    {
                        VehicleId = vehicle.Id,
                        Date = DateTime.UtcNow,
                        KmValue = contract.KmDeparture,
                        Source = "ContractStart",
                        Notes = $"Odometer recorded at start of contract {contract.ContractNumber}."
                    };
                    _context.KmEntries.Add(kmEntry);
                }
                else if (updated.ContractStatus == ContractStatus.Cancelled)
                {
                    // Transition -> Cancelled
                    vehicle.Status = VehicleStatus.Available;
                }
            }

            contract.ContractType = updated.ContractType;
            contract.StartDate = updated.StartDate;
            contract.ExpectedReturnDate = updated.ExpectedReturnDate;
            contract.DailyRate = updated.DailyRate;
            contract.RentalDays = updated.RentalDays;
            contract.AdditionalCharges = updated.AdditionalCharges;
            contract.ExtrasCharges = updated.ExtrasCharges;
            contract.DiscountAmount = updated.DiscountAmount;
            contract.FinalAmountDue = (contract.RentalDays * contract.DailyRate) + contract.AdditionalCharges + contract.ExtrasCharges - contract.DiscountAmount;
            contract.PaymentStatus = updated.PaymentStatus;
            contract.PaymentMethod = updated.PaymentMethod;
            contract.DepositAmount = updated.DepositAmount;
            contract.DepositStatus = updated.DepositStatus;
            contract.ContractStatus = updated.ContractStatus;
            contract.Notes = updated.Notes;

            await _context.SaveChangesAsync();
            return Ok(contract);
        }

        public class ReturnVehicleDto
        {
            public int KmReturn { get; set; }
            public DateTime ReturnDate { get; set; }
            public decimal FuelPenalty { get; set; }
            public decimal DamageFees { get; set; }
            public decimal ExtrasCharges { get; set; }
            public bool SetInMaintenance { get; set; } // If damage noted, set vehicle status to Maintenance
            public string ReturnNotes { get; set; } = string.Empty;
        }

        [HttpPost("{id}/return")]
        public async Task<IActionResult> ReturnVehicle(int id, [FromBody] ReturnVehicleDto dto)
        {
            var contract = await _context.RentalContracts
                .Include(c => c.Vehicle)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contract == null) return NotFound(new { message = "Contract not found" });
            if (contract.ContractStatus != ContractStatus.Active)
            {
                return BadRequest(new { message = "Only active contracts can be completed." });
            }

            var vehicle = contract.Vehicle;
            if (vehicle == null) return BadRequest(new { message = "Vehicle not found" });

            if (dto.KmReturn < contract.KmDeparture)
            {
                return BadRequest(new { message = $"Return km ({dto.KmReturn}) cannot be less than departure km ({contract.KmDeparture})." });
            }

            // Update contract Return details
            contract.ActualReturnDate = dto.ReturnDate;
            contract.KmReturn = dto.KmReturn;
            contract.KmDriven = dto.KmReturn - contract.KmDeparture;

            // Calculate late fee
            decimal lateFee = 0;
            if (dto.ReturnDate > contract.ExpectedReturnDate)
            {
                var lateSpan = dto.ReturnDate - contract.ExpectedReturnDate;
                var lateDays = Math.Ceiling(lateSpan.TotalDays);
                lateFee = (decimal)lateDays * contract.DailyRate;
            }

            contract.LateReturnFee = lateFee;
            contract.FuelPenalty = dto.FuelPenalty;
            contract.DamageFees = dto.DamageFees;
            contract.ExtrasCharges += dto.ExtrasCharges;

            // Recalculate total due
            contract.FinalAmountDue = contract.TotalAmount + contract.AdditionalCharges + contract.ExtrasCharges + contract.LateReturnFee + contract.FuelPenalty + contract.DamageFees - contract.DiscountAmount;
            contract.ContractStatus = ContractStatus.Completed;
            if (contract.FinalAmountDue == 0 || contract.PaymentStatus == PaymentStatus.Paid)
            {
                contract.PaymentStatus = PaymentStatus.Paid;
            }
            else
            {
                contract.PaymentStatus = PaymentStatus.PartiallyPaid; // Auto tag as partially paid if return fees added
            }

            // Update Vehicle current odometer and status
            vehicle.CurrentKm = dto.KmReturn;
            vehicle.Status = dto.SetInMaintenance ? VehicleStatus.InMaintenance : VehicleStatus.Available;

            // Log km timeline auto entry
            var kmEntry = new KmEntry
            {
                VehicleId = vehicle.Id,
                Date = dto.ReturnDate,
                KmValue = dto.KmReturn,
                Source = "ContractReturn",
                Notes = $"Odometer recorded at vehicle return for contract {contract.ContractNumber}."
            };
            _context.KmEntries.Add(kmEntry);

            await _context.SaveChangesAsync();

            return Ok(new { message = "Vehicle returned successfully", contract });
        }
    }
}
