using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class FuelLog
    {
        public int Id { get; set; }

        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

        public DateTime Date { get; set; }

        public int KmValue { get; set; } // Odometer km at fill-up
        public decimal Liters { get; set; }
        public decimal CostPerLiter { get; set; }
        public decimal TotalCost { get; set; } // Liters * CostPerLiter

        [MaxLength(150)]
        public string StationName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string FuelType { get; set; } = string.Empty;

        public bool IsAnomaly { get; set; } = false; // Flagged if deviates from average
    }
}
