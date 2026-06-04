using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class InsurancePolicy
    {
        public int Id { get; set; }

        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

        [Required]
        [MaxLength(150)]
        public string InsurerName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string PolicyNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string CoverageType { get; set; } = "Third-Party"; // Third-Party, Comprehensive, Fleet

        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        public decimal PremiumAmount { get; set; }
        public decimal InsuredValue { get; set; }

        [MaxLength(150)]
        public string AgentContact { get; set; } = string.Empty;

        public string DocumentPath { get; set; } = string.Empty; // PDF path
    }
}
