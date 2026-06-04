using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public enum VehicleStatus
    {
        Available,
        Rented,
        InMaintenance,
        Immobilized,
        Reserved
    }

    public class Vehicle
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Matricule { get; set; } = string.Empty; // Plate number (unique)

        [Required]
        [MaxLength(100)]
        public string Brand { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Model { get; set; } = string.Empty;

        public int Year { get; set; }

        [Required]
        [MaxLength(50)]
        public string Type { get; set; } = "Car"; // e.g., Car, SUV, Van, Truck, Motorcycle

        [Required]
        [MaxLength(50)]
        public string FuelType { get; set; } = "Gasoline"; // Gasoline, Diesel, Electric, Hybrid, LPG

        [Required]
        [MaxLength(50)]
        public string Transmission { get; set; } = "Manual"; // Manual, Automatic

        [Required]
        [MaxLength(100)]
        public string VIN { get; set; } = string.Empty; // Chassis number (unique)

        [MaxLength(100)]
        public string EngineNumber { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Color { get; set; } = string.Empty;

        public int SeatsCount { get; set; }

        public decimal DailyRate { get; set; } // Tarif journalier

        public VehicleStatus Status { get; set; } = VehicleStatus.Available;

        public DateTime PurchaseDate { get; set; }

        public decimal PurchasePrice { get; set; }

        public int InitialKm { get; set; }

        public int CurrentKm { get; set; }

        public string PhotoPath { get; set; } = string.Empty; // Path or URL to photo(s)

        public string Notes { get; set; } = string.Empty;
    }
}
