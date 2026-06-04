using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public enum MaintenanceStatus
    {
        Scheduled,
        InProgress,
        Completed
    }

    public class Maintenance
    {
        public int Id { get; set; }

        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

        [Required]
        [MaxLength(100)]
        public string MaintenanceType { get; set; } = "Preventive"; // Preventive, Corrective, AccidentRepair, Inspection

        public DateTime DatePerformed { get; set; }
        public DateTime? NextScheduledDate { get; set; }

        public int KmAtMaintenance { get; set; }

        [Required]
        [MaxLength(150)]
        public string WorkshopName { get; set; } = string.Empty;
        [MaxLength(250)]
        public string WorkshopAddress { get; set; } = string.Empty;
        [MaxLength(50)]
        public string WorkshopContact { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal LaborCost { get; set; }
        public decimal PartsCost { get; set; }
        public decimal TotalCost { get; set; } // labor + parts

        [MaxLength(50)]
        public string InvoiceNumber { get; set; } = string.Empty;
        public string InvoiceFilePath { get; set; } = string.Empty;

        public MaintenanceStatus Status { get; set; } = MaintenanceStatus.Scheduled;
    }
}
