using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Helpers;
using Backend.Models;

namespace Backend.Data
{
    public static class DbSeeder
    {
        public static void SeedMockData(DataContext context)
        {
            // Clear existing mock data first to guarantee fresh seeding
            context.RentalContracts.RemoveRange(context.RentalContracts);
            context.KmEntries.RemoveRange(context.KmEntries);
            context.Maintenances.RemoveRange(context.Maintenances);
            context.ConsumableLogs.RemoveRange(context.ConsumableLogs);
            context.InsurancePolicies.RemoveRange(context.InsurancePolicies);
            context.TechnicalInspections.RemoveRange(context.TechnicalInspections);
            context.FuelLogs.RemoveRange(context.FuelLogs);
            context.Vehicles.RemoveRange(context.Vehicles);
            context.Clients.RemoveRange(context.Clients);
            context.SaveChanges();

            // Seed Vehicles
            if (!context.Vehicles.Any())
            {
                var vehicles = new List<Vehicle>
                {
                    // --- 4x LIVAN X3 PRO ---
                    new()
                    {
                        Matricule = "123-AB-45", // Used in mock relations
                        Brand = "Livan",
                        Model = "X3 Pro",
                        Year = 2023,
                        Type = "SUV",
                        FuelType = "Essence",
                        Transmission = "Automatic",
                        VIN = "VF31LIVAN11111111",
                        EngineNumber = "ENG-LIVAN-01",
                        Color = "White",
                        SeatsCount = 5,
                        DailyRate = 60.00m,
                        Status = VehicleStatus.Available,
                        PurchaseDate = DateTime.UtcNow.AddMonths(-10),
                        PurchasePrice = 22000.00m,
                        InitialKm = 0,
                        CurrentKm = 0,
                        Notes = "Véhicule n°1",
                    },
                    new()
                    {
                        Matricule = "456-EF-78", // Used in mock relations
                        Brand = "Livan",
                        Model = "X3 Pro",
                        Year = 2023,
                        Type = "SUV",
                        FuelType = "Essence",
                        Transmission = "Automatic",
                        VIN = "VF31LIVAN22222222",
                        EngineNumber = "ENG-LIVAN-02",
                        Color = "Grey",
                        SeatsCount = 5,
                        DailyRate = 60.00m,
                        Status = VehicleStatus.Available,
                        PurchaseDate = DateTime.UtcNow.AddMonths(-10),
                        PurchasePrice = 22000.00m,
                        InitialKm = 0,
                        CurrentKm = 0,
                        Notes = "Véhicule n°2",
                    },
                    new()
                    {
                        Matricule = "LIV-03-DZ",
                        Brand = "Livan",
                        Model = "X3 Pro",
                        Year = 2023,
                        Type = "SUV",
                        FuelType = "Essence",
                        Transmission = "Automatic",
                        VIN = "VF31LIVAN33333333",
                        EngineNumber = "ENG-LIVAN-03",
                        Color = "Black",
                        SeatsCount = 5,
                        DailyRate = 60.00m,
                        Status = VehicleStatus.Available,
                        PurchaseDate = DateTime.UtcNow.AddMonths(-8),
                        PurchasePrice = 22000.00m,
                        InitialKm = 0,
                        CurrentKm = 0,
                        Notes = "Véhicule n°3",
                    },
                    new()
                    {
                        Matricule = "LIV-04-DZ",
                        Brand = "Livan",
                        Model = "X3 Pro",
                        Year = 2023,
                        Type = "SUV",
                        FuelType = "Essence",
                        Transmission = "Automatic",
                        VIN = "VF31LIVAN44444444",
                        EngineNumber = "ENG-LIVAN-04",
                        Color = "White",
                        SeatsCount = 5,
                        DailyRate = 60.00m,
                        Status = VehicleStatus.Available,
                        PurchaseDate = DateTime.UtcNow.AddMonths(-8),
                        PurchasePrice = 22000.00m,
                        InitialKm = 0,
                        CurrentKm = 0,
                        Notes = "Véhicule n°4",
                    },
                    // --- 4x VOLKSWAGEN THARU ---
                    new()
                    {
                        Matricule = "987-CD-65", // Used in mock relations
                        Brand = "Volkswagen",
                        Model = "Tharu",
                        Year = 2022,
                        Type = "SUV",
                        FuelType = "Essence",
                        Transmission = "Automatic",
                        VIN = "VW1THARU11111111",
                        EngineNumber = "ENG-THARU-01",
                        Color = "Black",
                        SeatsCount = 5,
                        DailyRate = 85.00m,
                        Status = VehicleStatus.Available,
                        PurchaseDate = DateTime.UtcNow.AddYears(-2),
                        PurchasePrice = 35000.00m,
                        InitialKm = 0,
                        CurrentKm = 0,
                        Notes = "Véhicule n°1",
                    },
                    new()
                    {
                        Matricule = "789-IJ-01", // Used in mock relations
                        Brand = "Volkswagen",
                        Model = "Tharu",
                        Year = 2022,
                        Type = "SUV",
                        FuelType = "Essence",
                        Transmission = "Automatic",
                        VIN = "VW1THARU22222222",
                        EngineNumber = "ENG-THARU-02",
                        Color = "Grey",
                        SeatsCount = 5,
                        DailyRate = 85.00m,
                        Status = VehicleStatus.Available,
                        PurchaseDate = DateTime.UtcNow.AddYears(-2),
                        PurchasePrice = 35000.00m,
                        InitialKm = 0,
                        CurrentKm = 0,
                        Notes = "Véhicule n°2",
                    },
                    new()
                    {
                        Matricule = "THA-03-DZ",
                        Brand = "Volkswagen",
                        Model = "Tharu",
                        Year = 2023,
                        Type = "SUV",
                        FuelType = "Essence",
                        Transmission = "Automatic",
                        VIN = "VW1THARU33333333",
                        EngineNumber = "ENG-THARU-03",
                        Color = "White",
                        SeatsCount = 5,
                        DailyRate = 85.00m,
                        Status = VehicleStatus.Available,
                        PurchaseDate = DateTime.UtcNow.AddMonths(-6),
                        PurchasePrice = 36000.00m,
                        InitialKm = 0,
                        CurrentKm = 0,
                        Notes = "Véhicule n°3",
                    },
                    new()
                    {
                        Matricule = "THA-04-DZ",
                        Brand = "Volkswagen",
                        Model = "Tharu",
                        Year = 2023,
                        Type = "SUV",
                        FuelType = "Essence",
                        Transmission = "Automatic",
                        VIN = "VW1THARU44444444",
                        EngineNumber = "ENG-THARU-04",
                        Color = "Silver",
                        SeatsCount = 5,
                        DailyRate = 85.00m,
                        Status = VehicleStatus.Available,
                        PurchaseDate = DateTime.UtcNow.AddMonths(-6),
                        PurchasePrice = 36000.00m,
                        InitialKm = 0,
                        CurrentKm = 0,
                        Notes = "Véhicule n°4",
                    },
                    // --- 1x FIAT DOBLO VITRÉ ---
                    new()
                    {
                        Matricule = "654-GH-98", // Used in mock relations
                        Brand = "Fiat",
                        Model = "Doblo Vitré",
                        Year = 2021,
                        Type = "Fourgonnette",
                        FuelType = "Diesel",
                        Transmission = "Automatic",
                        VIN = "WDB6543210987JKL4",
                        EngineNumber = "ENG-DOBLO-01",
                        Color = "White",
                        SeatsCount = 5,
                        DailyRate = 70.00m,
                        Status = VehicleStatus.Available,
                        PurchaseDate = DateTime.UtcNow.AddYears(-3),
                        PurchasePrice = 25000.00m,
                        InitialKm = 0,
                        CurrentKm = 0,
                        Notes = "Véhicule utilitaire n°1",
                    },
                };

                context.Vehicles.AddRange(vehicles);
                context.SaveChanges();
            }

            // Seed Clients
            if (!context.Clients.Any())
            {
                var clients = new List<Client>
                {
                    new()
                    {
                        FullName = "Yacine Benmansour",
                        NationalId = "NID-892718",
                        DateOfBirth = new DateTime(1985, 5, 12),
                        LicenseNumber = "PERMIS-YB-1985",
                        LicenseCategory = "B",
                        LicenseIssueDate = new DateTime(2005, 6, 20),
                        LicenseExpiryDate = DateTime.UtcNow.AddYears(2),
                        Phone = "+213 555 12 34 56",
                        Email = "yacine.benmansour@mail.com",
                        Address = "12 Rue Didouche Mourad, Alger",
                        Notes = "Client régulier, aucun problème signalé.",
                    },
                    new()
                    {
                        FullName = "Amira Belkacem",
                        NationalId = "NID-198273",
                        DateOfBirth = new DateTime(1990, 8, 25),
                        LicenseNumber = "PERMIS-AB-1990",
                        LicenseCategory = "B",
                        LicenseIssueDate = new DateTime(2010, 9, 15),
                        LicenseExpiryDate = DateTime.UtcNow.AddDays(-5), // EXPIRED!
                        Phone = "+213 661 98 76 54",
                        Email = "amira.belkacem@mail.com",
                        Address = "45 Boulevard de la Soummam, Oran",
                        Notes = "Alerte : permis de conduire expiré !",
                    },
                    new()
                    {
                        FullName = "Fatima Zohra Khelifi",
                        NationalId = "NID-561928",
                        DateOfBirth = new DateTime(1992, 11, 3),
                        LicenseNumber = "PERMIS-FK-1992",
                        LicenseCategory = "B",
                        LicenseIssueDate = new DateTime(2012, 12, 10),
                        LicenseExpiryDate = DateTime.UtcNow.AddDays(20), // EXPIRING SOON!
                        Phone = "+213 770 11 12 22",
                        Email = "fatima.khelifi@mail.com",
                        Address = "8 Avenue de l'ALN, Constantine",
                        Notes = "Permis expire bientôt.",
                    },
                    new()
                    {
                        FullName = "Sofiane Madani",
                        NationalId = "NID-901827",
                        DateOfBirth = new DateTime(1978, 2, 18),
                        LicenseNumber = "PERMIS-SM-1978",
                        LicenseCategory = "B",
                        LicenseIssueDate = new DateTime(1998, 3, 22),
                        LicenseExpiryDate = DateTime.UtcNow.AddYears(5),
                        Phone = "+213 550 44 45 55",
                        Email = "sofiane.madani@mail.com",
                        Address = "23 Rue Larbi Ben M'hidi, Alger",
                        Notes = "Paiement toujours en règle.",
                    },
                };

                context.Clients.AddRange(clients);
                context.SaveChanges();
            }

            // Seed Rental Contracts (Skipped: all vehicles start never rented)
            if (!context.RentalContracts.Any())
            {
                // Starting with empty history
            }

            // Seed Km Entries (Skipped: all vehicles start at 0 km)
            if (!context.KmEntries.Any())
            {
                // Starting with empty history
            }

            // Seed Maintenance (Skipped: all vehicles start clean)
            if (!context.Maintenances.Any())
            {
                // Starting with empty history
            }

            // Seed Consumable Logs (Skipped: all vehicles start clean)
            if (!context.ConsumableLogs.Any())
            {
                // Starting with empty history
            }

            // Seed Insurance Policies
            if (!context.InsurancePolicies.Any())
            {
                var vehicle1 = context.Vehicles.FirstOrDefault(v => v.Matricule == "123-AB-45");
                var vehicle2 = context.Vehicles.FirstOrDefault(v => v.Matricule == "456-EF-78");
                var vehicle5 = context.Vehicles.FirstOrDefault(v => v.Matricule == "987-CD-65");

                if (vehicle1 != null && vehicle2 != null && vehicle5 != null)
                {
                    var policies = new List<InsurancePolicy>
                    {
                        // Valid comprehensive policy
                        new()
                        {
                            VehicleId = vehicle1.Id,
                            InsurerName = "AXA Assurances",
                            PolicyNumber = "POL-AXA-9018273",
                            CoverageType = "Tous Risques",
                            StartDate = DateTime.UtcNow.AddMonths(-6),
                            ExpiryDate = DateTime.UtcNow.AddMonths(6),
                            PremiumAmount = 620.00m,
                            InsuredValue = 18000.00m,
                            AgentContact = "axaalger@axa.dz",
                        },
                        // Expiring policy (5 days remaining)
                        new()
                        {
                            VehicleId = vehicle2.Id,
                            InsurerName = "Alliance Assurances",
                            PolicyNumber = "POL-ALL-82192",
                            CoverageType = "Tous Risques",
                            StartDate = DateTime.UtcNow.AddYears(-1).AddDays(5),
                            ExpiryDate = DateTime.UtcNow.AddDays(5), // EXPIRES IN 5 DAYS
                            PremiumAmount = 580.00m,
                            InsuredValue = 20000.00m,
                            AgentContact = "allianceoran@alliance.dz",
                        },
                        // Expired policy (10 days ago)
                        new()
                        {
                            VehicleId = vehicle5.Id,
                            InsurerName = "SAA Assurances",
                            PolicyNumber = "POL-SAA-112233",
                            CoverageType = "Responsabilité Civile",
                            StartDate = DateTime.UtcNow.AddYears(-1).AddDays(-10),
                            ExpiryDate = DateTime.UtcNow.AddDays(-10), // EXPIRED 10 DAYS AGO
                            PremiumAmount = 450.00m,
                            InsuredValue = 40000.00m,
                            AgentContact = "saa@saa.dz",
                        },
                    };

                    context.InsurancePolicies.AddRange(policies);
                    context.SaveChanges();
                }
            }

            // Seed Technical Inspections
            if (!context.TechnicalInspections.Any())
            {
                var vehicle1 = context.Vehicles.FirstOrDefault(v => v.Matricule == "123-AB-45");
                var vehicle4 = context.Vehicles.FirstOrDefault(v => v.Matricule == "LIV-04-DZ");
                var vehicle5 = context.Vehicles.FirstOrDefault(v => v.Matricule == "987-CD-65");

                if (vehicle1 != null && vehicle4 != null && vehicle5 != null)
                {
                    var inspections = new List<TechnicalInspection>
                    {
                        // Passed inspection
                        new()
                        {
                            VehicleId = vehicle1.Id,
                            InspectionDate = DateTime.UtcNow.AddYears(-1),
                            ExpiryDate = DateTime.UtcNow.AddYears(1),
                            Result = "Pass",
                            CenterName = "Contrôle Technique Alger Centre",
                            CenterAddress = "Rue Hassiba Ben Bouali, Alger",
                            Cost = 2500.00m,
                            Remarks = "Aucune défaillance majeure.",
                        },
                        // Failed inspection (requires contre-visite)
                        new()
                        {
                            VehicleId = vehicle4.Id,
                            InspectionDate = DateTime.UtcNow.AddDays(-3),
                            ExpiryDate = DateTime.UtcNow.AddMonths(2), // Counter-inspection required within 2 months
                            Result = "Fail",
                            CenterName = "Dekra Contrôle Technique Oran",
                            CenterAddress = "Zone Industrielle, Oran",
                            Cost = 3000.00m,
                            Remarks = "Défaillance critique sur injecteurs et opacité des fumées.",
                        },
                        // Expiring inspection (14 days remaining)
                        new()
                        {
                            VehicleId = vehicle5.Id,
                            InspectionDate = DateTime.UtcNow.AddYears(-2).AddDays(14),
                            ExpiryDate = DateTime.UtcNow.AddDays(14), // EXPIRES IN 14 DAYS
                            Result = "Pass",
                            CenterName = "Centre de Contrôle Technique Constantine",
                            CenterAddress = "Route de Batna, Constantine",
                            Cost = 2500.00m,
                            Remarks = "Plaquettes arrières à surveiller.",
                        },
                    };

                    context.TechnicalInspections.AddRange(inspections);
                    context.SaveChanges();
                }
            }

            // Seed Fuel Logs (Skipped: all vehicles start never rented and at 0 km)
            if (!context.FuelLogs.Any())
            {
                // Starting with empty history
            }
        }
    }
}
