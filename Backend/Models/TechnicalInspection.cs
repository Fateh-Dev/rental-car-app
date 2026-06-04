using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class TechnicalInspection
    {
        public int Id { get; set; }

        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

        public DateTime InspectionDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string Result { get; set; } = "Pass"; // Pass, Conditional, Fail

        [Required]
        [MaxLength(150)]
        public string CenterName { get; set; } = string.Empty;
        [MaxLength(250)]
        public string CenterAddress { get; set; } = string.Empty;

        public decimal Cost { get; set; }
        public string Remarks { get; set; } = string.Empty;

        public string DocumentPath { get; set; } = string.Empty; // PDF path
    }
}
