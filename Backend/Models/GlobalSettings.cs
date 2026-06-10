using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class GlobalSettings
    {
        public int Id { get; set; }

        [MaxLength(10)]
        public string CurrencySymbol { get; set; } = "DZD";

        [MaxLength(30)]
        public string DateFormat { get; set; } = "yyyy-MM-dd";

        public int KmInactivityDaysThreshold { get; set; } = 15;

        // Comma separated list of days, e.g. "30,15,7"
        [MaxLength(100)]
        public string InsuranceExpiryDaysThresholds { get; set; } = "30,15,7";

        [MaxLength(100)]
        public string InspectionExpiryDaysThresholds { get; set; } = "30,15,7";

        public int MaintenanceNotifyDaysBefore { get; set; } = 7;

        public int ConsumableNotifyKmBefore { get; set; } = 1000;
        public int ConsumableNotifyDaysBefore { get; set; } = 30;

        // Reference data stored as JSON strings
        public string VehicleTypesJson { get; set; } = "[\"Voiture\", \"SUV\", \"Fourgonnette\", \"Camion\", \"Moto\"]";
        public string FuelTypesJson { get; set; } = "[\"Essence\", \"Diesel\", \"Électrique\", \"Hybride\", \"GPL\"]";
        public string MaintenanceTypesJson { get; set; } = "[\"Préventif\", \"Correctif\", \"Réparation accident\", \"Inspection\"]";
        public string CoverageTypesJson { get; set; } = "[\"Responsabilité Civile\", \"Tous Risques\", \"Flotte\"]";
        public string ExtrasJson { get; set; } = "[{\"Name\":\"GPS\",\"Price\":5.0},{\"Name\":\"Siège bébé\",\"Price\":3.0},{\"Name\":\"Conducteur supplémentaire\",\"Price\":10.0}]";
    }
}
