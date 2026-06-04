using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    // Configuration thresholds for consumables
    public class ConsumableConfig
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string ConsumableType { get; set; } = string.Empty; // e.g. OilChange, AirFilter, FrontBrakes, etc.

        public int IntervalKm { get; set; } // km interval (0 if not km-based)
        public int IntervalMonths { get; set; } // month interval (0 if not time-based)
    }

    // Replacement records
    public class ConsumableLog
    {
        public int Id { get; set; }

        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

        [Required]
        [MaxLength(100)]
        public string ConsumableType { get; set; } = string.Empty; // Predefined or User-defined

        public DateTime ReplacementDate { get; set; }
        public int ReplacementKm { get; set; }

        // Specific details
        [MaxLength(100)]
        public string OilType { get; set; } = string.Empty; // Viscosity like 5W-40, for OilChange
        [MaxLength(50)]
        public string Viscosity { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Brand { get; set; } = string.Empty; // Tire brand, Battery brand, etc.
        [MaxLength(100)]
        public string Size { get; set; } = string.Empty; // Tire size
        [MaxLength(100)]
        public string TypeDetail { get; set; } = string.Empty; // Battery capacity, Tire type, filter type
        [MaxLength(50)]
        public string Axle { get; set; } = string.Empty; // Front / Rear axle for Brakes/Tires

        public string Notes { get; set; } = string.Empty;
    }
}
