using System;

namespace Backend.Models
{
    public class KmEntry
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }
        public DateTime Date { get; set; }
        public int KmValue { get; set; }
        public string Source { get; set; } = "Manual"; // Manual, ContractStart, ContractReturn
        public string Notes { get; set; } = string.Empty;
    }
}
