using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public enum ContractStatus
    {
        Draft,
        Active,
        Completed,
        Cancelled
    }

    public enum PaymentStatus
    {
        Unpaid,
        PartiallyPaid,
        Paid
    }

    public class RentalContract
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string ContractNumber { get; set; } = string.Empty; // unique, e.g. CTR-20260603-001

        public int ClientId { get; set; }
        public Client? Client { get; set; }

        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

        [Required]
        [MaxLength(50)]
        public string ContractType { get; set; } = "Daily"; // Daily, Weekly, Monthly, Long-term

        public DateTime StartDate { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
        public DateTime? ActualReturnDate { get; set; }

        public int KmDeparture { get; set; }
        public int? KmReturn { get; set; }
        public int KmDriven { get; set; } // Auto-calculated (KmReturn - KmDeparture)

        public decimal DailyRate { get; set; } // overridable from vehicle rate
        public int RentalDays { get; set; } // auto-calculated, adjustable
        public decimal TotalAmount { get; set; } // Days * DailyRate

        // Additional fees
        public decimal FuelPenalty { get; set; }
        public decimal DamageFees { get; set; }
        public decimal LateReturnFee { get; set; }
        public decimal ExtrasCharges { get; set; } // GPS, child seat, etc.
        public decimal AdditionalCharges { get; set; } // General additional charges

        public decimal DiscountAmount { get; set; } // Flat discount amount
        public decimal FinalAmountDue { get; set; } // TotalAmount + Additional - Discount

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Unpaid;
        public decimal AmountPaid { get; set; } = 0;

        [MaxLength(50)]
        public string PaymentMethod { get; set; } = "Cash"; // Cash, Card, BankTransfer, Cheque

        public decimal DepositAmount { get; set; }
        [MaxLength(50)]
        public string DepositStatus { get; set; } = "Collected"; // Collected, Returned

        public ContractStatus ContractStatus { get; set; } = ContractStatus.Draft;

        public string Notes { get; set; } = string.Empty;
    }
}
