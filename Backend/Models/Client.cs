using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string NationalId { get; set; } = string.Empty; // National ID or passport number (unique)

        public DateTime DateOfBirth { get; set; }

        [Required]
        [MaxLength(50)]
        public string LicenseNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string LicenseCategory { get; set; } = "B";

        public DateTime LicenseIssueDate { get; set; }

        public DateTime LicenseExpiryDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(250)]
        public string Address { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;
    }
}
