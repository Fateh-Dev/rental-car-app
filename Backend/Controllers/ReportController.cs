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
    public class ReportController : ControllerBase
    {
        private readonly DataContext _context;

        public ReportController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("fleet-status")]
        public async Task<IActionResult> GetFleetStatus()
        {
            var total = await _context.Vehicles.CountAsync();
            var available = await _context.Vehicles.CountAsync(v => v.Status == VehicleStatus.Available);
            var rented = await _context.Vehicles.CountAsync(v => v.Status == VehicleStatus.Rented);
            var inMaintenance = await _context.Vehicles.CountAsync(v => v.Status == VehicleStatus.InMaintenance);
            var immobilized = await _context.Vehicles.CountAsync(v => v.Status == VehicleStatus.Immobilized);
            var reserved = await _context.Vehicles.CountAsync(v => v.Status == VehicleStatus.Reserved);

            return Ok(new
            {
                Total = total,
                Available = available,
                Rented = rented,
                InMaintenance = inMaintenance,
                Immobilized = immobilized,
                Reserved = reserved
            });
        }

        [HttpGet("revenue")]
        public async Task<IActionResult> GetRevenueReport([FromQuery] DateTime? start, [FromQuery] DateTime? end)
        {
            var query = _context.RentalContracts.AsQueryable();

            if (start.HasValue)
            {
                query = query.Where(c => c.StartDate >= start.Value);
            }
            if (end.HasValue)
            {
                query = query.Where(c => c.StartDate <= end.Value);
            }

            var contracts = await query.ToListAsync();

            var totalRevenue = contracts.Sum(c => c.FinalAmountDue);
            var paidRevenue = contracts.Where(c => c.PaymentStatus == PaymentStatus.Paid).Sum(c => c.FinalAmountDue);
            var unpaidRevenue = totalRevenue - paidRevenue;

            var revenueByVehicle = contracts
                .GroupBy(c => c.VehicleId)
                .Select(g => new
                {
                    VehicleId = g.Key,
                    Amount = g.Sum(c => c.FinalAmountDue)
                }).ToList();

            var result = new List<object>();
            foreach (var item in revenueByVehicle)
            {
                var vehicle = await _context.Vehicles.FindAsync(item.VehicleId);
                result.Add(new
                {
                    VehicleId = item.VehicleId,
                    Brand = vehicle?.Brand ?? "Unknown",
                    Model = vehicle?.Model ?? "",
                    Matricule = vehicle?.Matricule ?? "",
                    Amount = item.Amount
                });
            }

            return Ok(new
            {
                TotalRevenue = totalRevenue,
                PaidRevenue = paidRevenue,
                UnpaidRevenue = unpaidRevenue,
                ByVehicle = result
            });
        }

        [HttpGet("profitability")]
        public async Task<IActionResult> GetProfitabilityReport()
        {
            var vehicles = await _context.Vehicles.ToListAsync();
            var contracts = await _context.RentalContracts.ToListAsync();
            var maintenances = await _context.Maintenances.ToListAsync();
            var fuelLogs = await _context.FuelLogs.ToListAsync();
            var insurances = await _context.InsurancePolicies.ToListAsync();

            var report = vehicles.Select(v =>
            {
                var revenue = contracts.Where(c => c.VehicleId == v.Id).Sum(c => c.FinalAmountDue);
                var maintCost = maintenances.Where(m => m.VehicleId == v.Id).Sum(m => m.TotalCost);
                var fuelCost = fuelLogs.Where(f => f.VehicleId == v.Id).Sum(f => f.TotalCost);
                var insCost = insurances.Where(i => i.VehicleId == v.Id).Sum(i => i.PremiumAmount);

                var totalCost = maintCost + fuelCost + insCost;
                var netProfit = revenue - totalCost;

                // Utilization rate calculation
                var vContracts = contracts.Where(c => c.VehicleId == v.Id && c.ContractStatus != ContractStatus.Cancelled).ToList();
                int rentalDays = vContracts.Sum(c => c.RentalDays);
                int daysSincePurchase = (DateTime.UtcNow.Date - v.PurchaseDate.Date).Days;
                if (daysSincePurchase <= 0) daysSincePurchase = 1;
                decimal utilization = ((decimal)rentalDays / daysSincePurchase) * 100;
                if (utilization > 100) utilization = 100;

                return new
                {
                    v.Id,
                    v.Brand,
                    v.Model,
                    v.Matricule,
                    Revenue = revenue,
                    MaintenanceCost = maintCost,
                    FuelCost = fuelCost,
                    InsuranceCost = insCost,
                    TotalCost = totalCost,
                    Profitability = netProfit,
                    UtilizationRate = Math.Round(utilization, 2)
                };
            }).OrderByDescending(r => r.Profitability).ToList();

            return Ok(report);
        }

        [HttpGet("top-clients")]
        public async Task<IActionResult> GetTopClients()
        {
            var contracts = await _context.RentalContracts
                .Include(c => c.Client)
                .ToListAsync();

            var topClients = contracts
                .Where(c => c.Client != null)
                .GroupBy(c => c.ClientId)
                .Select(g => new
                {
                    ClientId = g.Key,
                    Name = g.First().Client!.FullName,
                    Phone = g.First().Client!.Phone,
                    RentalsCount = g.Count(),
                    TotalRevenue = g.Sum(c => c.FinalAmountDue)
                })
                .OrderByDescending(c => c.TotalRevenue)
                .Take(10)
                .ToList();

            return Ok(topClients);
        }

        [HttpGet("unpaid-contracts")]
        public async Task<IActionResult> GetUnpaidContracts()
        {
            var list = await _context.RentalContracts
                .Include(c => c.Client)
                .Include(c => c.Vehicle)
                .Where(c => c.PaymentStatus != PaymentStatus.Paid && c.ContractStatus != ContractStatus.Cancelled)
                .ToListAsync();

            var sortedList = list.OrderByDescending(c => c.FinalAmountDue).ToList();

            return Ok(sortedList);
        }

        [HttpGet("export-profitability-csv")]
        public async Task<IActionResult> ExportProfitabilityCsv()
        {
            var vehicles = await _context.Vehicles.ToListAsync();
            var contracts = await _context.RentalContracts.ToListAsync();
            var maintenances = await _context.Maintenances.ToListAsync();
            var fuelLogs = await _context.FuelLogs.ToListAsync();
            var insurances = await _context.InsurancePolicies.ToListAsync();

            var builder = new StringBuilder();
            builder.AppendLine("Matricule,Vehicle,Utilization Rate %,Revenue,Maintenance Cost,Fuel Cost,Insurance Cost,Total Cost,Net Profit");

            foreach (var v in vehicles)
            {
                var revenue = contracts.Where(c => c.VehicleId == v.Id).Sum(c => c.FinalAmountDue);
                var maintCost = maintenances.Where(m => m.VehicleId == v.Id).Sum(m => m.TotalCost);
                var fuelCost = fuelLogs.Where(f => f.VehicleId == v.Id).Sum(f => f.TotalCost);
                var insCost = insurances.Where(i => i.VehicleId == v.Id).Sum(i => i.PremiumAmount);
                var totalCost = maintCost + fuelCost + insCost;
                var netProfit = revenue - totalCost;

                var vContracts = contracts.Where(c => c.VehicleId == v.Id && c.ContractStatus != ContractStatus.Cancelled).ToList();
                int rentalDays = vContracts.Sum(c => c.RentalDays);
                int daysSincePurchase = (DateTime.UtcNow.Date - v.PurchaseDate.Date).Days;
                if (daysSincePurchase <= 0) daysSincePurchase = 1;
                decimal utilization = ((decimal)rentalDays / daysSincePurchase) * 100;
                if (utilization > 100) utilization = 100;

                builder.AppendLine($"\"{v.Matricule}\",\"{v.Brand} {v.Model}\",{utilization:F2},{revenue:F2},{maintCost:F2},{fuelCost:F2},{insCost:F2},{totalCost:F2},{netProfit:F2}");
            }

            var csvBytes = Encoding.UTF8.GetBytes(builder.ToString());
            return File(csvBytes, "text/csv", $"Rapport_Rentabilite_Flotte_{DateTime.UtcNow:yyyyMMdd}.csv");
        }
    }
}
