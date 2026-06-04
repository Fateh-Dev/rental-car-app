using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Models;
using Backend.Helpers;

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
                    new()
                    {
                        Matricule = "123-AB-45",
                        Brand = "Peugeot",
                        Model = "208",
                        Year = 2021,
                        Type = "Car",
                        FuelType = "Gasoline",
                        Transmission = "Manual",
                        VIN = "VF31234567890ABC1",
                        EngineNumber = "ENG-208-PETROL",
                        Color = "Grey",
                        SeatsCount = 5,
                        DailyRate = 40.00m,
                        Status = VehicleStatus.Available,
                        PurchaseDate = DateTime.UtcNow.AddYears(-3),
                        PurchasePrice = 18500.00m,
                        InitialKm = 10000,
                        CurrentKm = 45000,
                        Notes = "Véhicule très économique, idéal pour la ville."
                    },
                    new()
                    {
                        Matricule = "987-CD-65",
                        Brand = "Renault",
                        Model = "Clio 5",
                        Year = 2022,
                        Type = "Car",
                        FuelType = "Diesel",
                        Transmission = "Manual",
                        VIN = "VF19876543210DEF2",
                        EngineNumber = "ENG-CLIO-DIESEL",
                        Color = "White",
                        SeatsCount = 5,
                        DailyRate = 45.00m,
                        Status = VehicleStatus.Rented,
                        PurchaseDate = DateTime.UtcNow.AddYears(-2),
                        PurchasePrice = 21000.00m,
                        InitialKm = 5000,
                        CurrentKm = 85200,
                        Notes = "Consommation très basse."
                    },
                    new()
                    {
                        Matricule = "456-EF-78",
                        Brand = "Toyota",
                        Model = "RAV4",
                        Year = 2022,
                        Type = "SUV",
                        FuelType = "Hybrid",
                        Transmission = "Automatic",
                        VIN = "JT14567890123GHI3",
                        EngineNumber = "ENG-RAV4-HYBRID",
                        Color = "Black",
                        SeatsCount = 5,
                        DailyRate = 75.00m,
                        Status = VehicleStatus.Reserved,
                        PurchaseDate = DateTime.UtcNow.AddYears(-2),
                        PurchasePrice = 38000.00m,
                        InitialKm = 2000,
                        CurrentKm = 62000,
                        Notes = "Confortable et spacieux."
                    },
                    new()
                    {
                        Matricule = "654-GH-98",
                        Brand = "Mercedes-Benz",
                        Model = "Sprinter",
                        Year = 2020,
                        Type = "Van",
                        FuelType = "Diesel",
                        Transmission = "Manual",
                        VIN = "WDB6543210987JKL4",
                        EngineNumber = "ENG-SPRINTER-MERC",
                        Color = "White",
                        SeatsCount = 3,
                        DailyRate = 110.00m,
                        Status = VehicleStatus.InMaintenance,
                        PurchaseDate = DateTime.UtcNow.AddYears(-4),
                        PurchasePrice = 45000.00m,
                        InitialKm = 15000,
                        CurrentKm = 145000,
                        Notes = "Véhicule utilitaire pour grands volumes."
                    },
                    new()
                    {
                        Matricule = "789-IJ-01",
                        Brand = "Tesla",
                        Model = "Model 3",
                        Year = 2023,
                        Type = "Car",
                        FuelType = "Electric",
                        Transmission = "Automatic",
                        VIN = "5YJ7890123456MNO5",
                        EngineNumber = "ENG-TESLA-ELEC",
                        Color = "Blue",
                        SeatsCount = 5,
                        DailyRate = 95.00m,
                        Status = VehicleStatus.Available,
                        PurchaseDate = DateTime.UtcNow.AddYears(-1),
                        PurchasePrice = 42500.00m,
                        InitialKm = 100,
                        CurrentKm = 38000,
                        Notes = "Autonomie 490 km."
                    }
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
                        FullName = "Jean Dupont",
                        NationalId = "NID-892718",
                        DateOfBirth = new DateTime(1985, 5, 12),
                        LicenseNumber = "PERMIS-JD-1985",
                        LicenseCategory = "B",
                        LicenseIssueDate = new DateTime(2005, 6, 20),
                        LicenseExpiryDate = DateTime.UtcNow.AddYears(2),
                        Phone = "+33 6 12 34 56 78",
                        Email = "jean.dupont@mail.com",
                        Address = "12 Rue de la Paix, 75002 Paris",
                        Notes = "Client régulier, aucun problème signalé."
                    },
                    new()
                    {
                        FullName = "Marie Laurent",
                        NationalId = "NID-198273",
                        DateOfBirth = new DateTime(1990, 8, 25),
                        LicenseNumber = "PERMIS-ML-1990",
                        LicenseCategory = "B",
                        LicenseIssueDate = new DateTime(2010, 9, 15),
                        LicenseExpiryDate = DateTime.UtcNow.AddDays(-5), // EXPIRED!
                        Phone = "+33 6 98 76 54 32",
                        Email = "marie.laurent@mail.com",
                        Address = "45 Avenue de la République, 69002 Lyon",
                        Notes = "Alerte : permis de conduire expiré !"
                    },
                    new()
                    {
                        FullName = "Alice Martin",
                        NationalId = "NID-561928",
                        DateOfBirth = new DateTime(1992, 11, 3),
                        LicenseNumber = "PERMIS-AM-1992",
                        LicenseCategory = "B",
                        LicenseIssueDate = new DateTime(2012, 12, 10),
                        LicenseExpiryDate = DateTime.UtcNow.AddDays(20), // EXPIRING SOON!
                        Phone = "+33 6 11 12 22 33",
                        Email = "alice.martin@mail.com",
                        Address = "8 Boulevard Victor Hugo, 13001 Marseille",
                        Notes = "Permis expire bientôt."
                    },
                    new()
                    {
                        FullName = "Thomas Bernard",
                        NationalId = "NID-901827",
                        DateOfBirth = new DateTime(1978, 2, 18),
                        LicenseNumber = "PERMIS-TB-1978",
                        LicenseCategory = "B",
                        LicenseIssueDate = new DateTime(1998, 3, 22),
                        LicenseExpiryDate = DateTime.UtcNow.AddYears(5),
                        Phone = "+33 6 44 45 55 66",
                        Email = "thomas.bernard@mail.com",
                        Address = "23 Avenue Jean Jaurès, 33000 Bordeaux",
                        Notes = "Paiement toujours en règle."
                    }
                };

                context.Clients.AddRange(clients);
                context.SaveChanges();
            }

            // Seed Rental Contracts
            if (!context.RentalContracts.Any())
            {
                var vehicle1 = context.Vehicles.FirstOrDefault(v => v.Matricule == "123-AB-45");
                var vehicle2 = context.Vehicles.FirstOrDefault(v => v.Matricule == "987-CD-65");
                var vehicle3 = context.Vehicles.FirstOrDefault(v => v.Matricule == "456-EF-78");

                var client1 = context.Clients.FirstOrDefault(c => c.FullName == "Jean Dupont");
                var client3 = context.Clients.FirstOrDefault(c => c.FullName == "Alice Martin");
                var client4 = context.Clients.FirstOrDefault(c => c.FullName == "Thomas Bernard");

                if (vehicle1 != null && vehicle2 != null && vehicle3 != null && client1 != null && client3 != null && client4 != null)
                {
                    var contracts = new List<RentalContract>
                    {
                        // 1. Completed Contract
                        new()
                        {
                            ContractNumber = "CTR-20260501-001",
                            ClientId = client1.Id,
                            VehicleId = vehicle1.Id,
                            ContractType = "Daily",
                            StartDate = DateTime.UtcNow.AddDays(-30),
                            ExpectedReturnDate = DateTime.UtcNow.AddDays(-25),
                            ActualReturnDate = DateTime.UtcNow.AddDays(-25),
                            KmDeparture = 44000,
                            KmReturn = 44500,
                            KmDriven = 500,
                            DailyRate = 40.00m,
                            RentalDays = 5,
                            TotalAmount = 200.00m,
                            ExtrasCharges = 20.00m, // e.g. Baby seat
                            DiscountAmount = 10.00m,
                            FinalAmountDue = 210.00m,
                            PaymentStatus = PaymentStatus.Paid,
                            PaymentMethod = "Cash",
                            DepositAmount = 500.00m,
                            DepositStatus = "Returned",
                            ContractStatus = ContractStatus.Completed,
                            Notes = "Retour en parfait état, nettoyé."
                        },
                        // 2. Active Contract
                        new()
                        {
                            ContractNumber = "CTR-20260601-002",
                            ClientId = client4.Id,
                            VehicleId = vehicle2.Id,
                            ContractType = "Daily",
                            StartDate = DateTime.UtcNow.AddDays(-2),
                            ExpectedReturnDate = DateTime.UtcNow.AddDays(5),
                            KmDeparture = 85000,
                            DailyRate = 45.00m,
                            RentalDays = 7,
                            TotalAmount = 315.00m,
                            ExtrasCharges = 0.00m,
                            DiscountAmount = 0.00m,
                            FinalAmountDue = 315.00m,
                            PaymentStatus = PaymentStatus.Unpaid,
                            PaymentMethod = "Card",
                            DepositAmount = 500.00m,
                            DepositStatus = "Collected",
                            ContractStatus = ContractStatus.Active,
                            Notes = "Véhicule loué pour déplacement professionnel."
                        },
                        // 3. Draft/Reserved Contract
                        new()
                        {
                            ContractNumber = "CTR-20260603-003",
                            ClientId = client3.Id,
                            VehicleId = vehicle3.Id,
                            ContractType = "Daily",
                            StartDate = DateTime.UtcNow.AddDays(3),
                            ExpectedReturnDate = DateTime.UtcNow.AddDays(13),
                            KmDeparture = 62000,
                            DailyRate = 75.00m,
                            RentalDays = 10,
                            TotalAmount = 750.00m,
                            ExtrasCharges = 50.00m, // GPS + child seat
                            DiscountAmount = 50.00m,
                            FinalAmountDue = 750.00m,
                            PaymentStatus = PaymentStatus.Unpaid,
                            PaymentMethod = "BankTransfer",
                            DepositAmount = 800.00m,
                            DepositStatus = "Collected",
                            ContractStatus = ContractStatus.Draft,
                            Notes = "Réservation pour vacances familiales."
                        }
                    };

                    context.RentalContracts.AddRange(contracts);
                    context.SaveChanges();
                }
            }

            // Seed Km Entries
            if (!context.KmEntries.Any())
            {
                var vehicle1 = context.Vehicles.FirstOrDefault(v => v.Matricule == "123-AB-45");
                var vehicle2 = context.Vehicles.FirstOrDefault(v => v.Matricule == "987-CD-65");

                if (vehicle1 != null && vehicle2 != null)
                {
                    var kmEntries = new List<KmEntry>
                    {
                        // Peugeot 208 history
                        new() { VehicleId = vehicle1.Id, Date = DateTime.UtcNow.AddDays(-30), KmValue = 44000, Source = "ContractStart", Notes = "Départ contrat CTR-20260501-001" },
                        new() { VehicleId = vehicle1.Id, Date = DateTime.UtcNow.AddDays(-25), KmValue = 44500, Source = "ContractReturn", Notes = "Retour contrat CTR-20260501-001" },
                        new() { VehicleId = vehicle1.Id, Date = DateTime.UtcNow.AddDays(-15), KmValue = 45000, Source = "Manual", Notes = "Relevé mensuel interne" },

                        // Renault Clio 5 history
                        new() { VehicleId = vehicle2.Id, Date = DateTime.UtcNow.AddDays(-60), KmValue = 78000, Source = "Manual", Notes = "Contrôle entretien vidange" },
                        new() { VehicleId = vehicle2.Id, Date = DateTime.UtcNow.AddDays(-2), KmValue = 85000, Source = "ContractStart", Notes = "Départ contrat CTR-20260601-002" }
                    };

                    context.KmEntries.AddRange(kmEntries);
                    context.SaveChanges();
                }
            }

            // Seed Maintenance
            if (!context.Maintenances.Any())
            {
                var vehicle4 = context.Vehicles.FirstOrDefault(v => v.Matricule == "654-GH-98");
                var vehicle1 = context.Vehicles.FirstOrDefault(v => v.Matricule == "123-AB-45");

                if (vehicle4 != null && vehicle1 != null)
                {
                    var maintenances = new List<Maintenance>
                    {
                        // Active Corrective maintenance
                        new()
                        {
                            VehicleId = vehicle4.Id,
                            MaintenanceType = "Corrective",
                            DatePerformed = DateTime.UtcNow.AddDays(-1),
                            NextScheduledDate = null,
                            KmAtMaintenance = 145000,
                            WorkshopName = "Garage Central",
                            WorkshopAddress = "24 Avenue des Garages, 75018 Paris",
                            WorkshopContact = "+33 1 45 67 89 10",
                            Description = "Problème d'injecteurs de carburant. Perte de puissance moteur.",
                            LaborCost = 0.00m,
                            PartsCost = 0.00m,
                            TotalCost = 0.00m,
                            Status = MaintenanceStatus.InProgress
                        },
                        // Completed preventive maintenance
                        new()
                        {
                            VehicleId = vehicle1.Id,
                            MaintenanceType = "Preventive",
                            DatePerformed = DateTime.UtcNow.AddDays(-15),
                            NextScheduledDate = DateTime.UtcNow.AddMonths(12),
                            KmAtMaintenance = 44500,
                            WorkshopName = "Speedy Paris 12",
                            WorkshopAddress = "98 Boulevard de Bercy, 75012 Paris",
                            WorkshopContact = "+33 1 23 45 67 89",
                            Description = "Entretien régulier avec changement des plaquettes de frein avant.",
                            LaborCost = 80.00m,
                            PartsCost = 170.00m,
                            TotalCost = 250.00m,
                            InvoiceNumber = "FAC-2026-9817",
                            Status = MaintenanceStatus.Completed
                        }
                    };

                    context.Maintenances.AddRange(maintenances);
                    context.SaveChanges();
                }
            }

            // Seed Consumable Logs
            if (!context.ConsumableLogs.Any())
            {
                var vehicle1 = context.Vehicles.FirstOrDefault(v => v.Matricule == "123-AB-45");
                var vehicle2 = context.Vehicles.FirstOrDefault(v => v.Matricule == "987-CD-65");

                if (vehicle1 != null && vehicle2 != null)
                {
                    var logs = new List<ConsumableLog>
                    {
                        // Peugeot 208 oil change 15 days ago
                        new()
                        {
                            VehicleId = vehicle1.Id,
                            ConsumableType = "OilChange",
                            ReplacementDate = DateTime.UtcNow.AddDays(-15),
                            ReplacementKm = 44500,
                            OilType = "Synthetic",
                            Viscosity = "5W-30",
                            Brand = "Castrol",
                            Notes = "Vidange standard et changement filtre à huile."
                        },
                        // Peugeot 208 Front Brakes replaced 15 days ago
                        new()
                        {
                            VehicleId = vehicle1.Id,
                            ConsumableType = "FrontBrakes",
                            ReplacementDate = DateTime.UtcNow.AddDays(-15),
                            ReplacementKm = 44500,
                            Brand = "Brembo",
                            Axle = "Front",
                            Notes = "Plaquettes changées lors de la maintenance périodique."
                        },
                        // Renault Clio 5 Front tires replaced at 78000 km
                        new()
                        {
                            VehicleId = vehicle2.Id,
                            ConsumableType = "FrontTires",
                            ReplacementDate = DateTime.UtcNow.AddDays(-60),
                            ReplacementKm = 78000,
                            Brand = "Michelin",
                            Size = "195/55 R16",
                            TypeDetail = "4 Saisons",
                            Axle = "Front",
                            Notes = "Remplacement pneu usé."
                        }
                    };

                    context.ConsumableLogs.AddRange(logs);
                    context.SaveChanges();
                }
            }

            // Seed Insurance Policies
            if (!context.InsurancePolicies.Any())
            {
                var vehicle1 = context.Vehicles.FirstOrDefault(v => v.Matricule == "123-AB-45");
                var vehicle2 = context.Vehicles.FirstOrDefault(v => v.Matricule == "987-CD-65");
                var vehicle5 = context.Vehicles.FirstOrDefault(v => v.Matricule == "789-IJ-01");

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
                            CoverageType = "Comprehensive",
                            StartDate = DateTime.UtcNow.AddMonths(-6),
                            ExpiryDate = DateTime.UtcNow.AddMonths(6),
                            PremiumAmount = 620.00m,
                            InsuredValue = 18000.00m,
                            AgentContact = "axaparis@axa.fr"
                        },
                        // Expiring policy (5 days remaining)
                        new()
                        {
                            VehicleId = vehicle2.Id,
                            InsurerName = "Allianz France",
                            PolicyNumber = "POL-ALL-82192",
                            CoverageType = "Comprehensive",
                            StartDate = DateTime.UtcNow.AddYears(-1).AddDays(5),
                            ExpiryDate = DateTime.UtcNow.AddDays(5), // EXPIRES IN 5 DAYS
                            PremiumAmount = 580.00m,
                            InsuredValue = 20000.00m,
                            AgentContact = "allianzlyon@allianz.fr"
                        },
                        // Expired policy (10 days ago)
                        new()
                        {
                            VehicleId = vehicle5.Id,
                            InsurerName = "GMF Assurances",
                            PolicyNumber = "POL-GMF-112233",
                            CoverageType = "Third-Party",
                            StartDate = DateTime.UtcNow.AddYears(-1).AddDays(-10),
                            ExpiryDate = DateTime.UtcNow.AddDays(-10), // EXPIRED 10 DAYS AGO
                            PremiumAmount = 450.00m,
                            InsuredValue = 40000.00m,
                            AgentContact = "gmf@gmf.fr"
                        }
                    };

                    context.InsurancePolicies.AddRange(policies);
                    context.SaveChanges();
                }
            }

            // Seed Technical Inspections
            if (!context.TechnicalInspections.Any())
            {
                var vehicle1 = context.Vehicles.FirstOrDefault(v => v.Matricule == "123-AB-45");
                var vehicle4 = context.Vehicles.FirstOrDefault(v => v.Matricule == "654-GH-98");
                var vehicle5 = context.Vehicles.FirstOrDefault(v => v.Matricule == "789-IJ-01");

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
                            CenterName = "Securitest Paris 12",
                            CenterAddress = "112 Rue de Charenton, 75012 Paris",
                            Cost = 75.00m,
                            Remarks = "Aucune défaillance majeure."
                        },
                        // Failed inspection (requires contre-visite)
                        new()
                        {
                            VehicleId = vehicle4.Id,
                            InspectionDate = DateTime.UtcNow.AddDays(-3),
                            ExpiryDate = DateTime.UtcNow.AddMonths(2), // Counter-inspection required within 2 months
                            Result = "Fail",
                            CenterName = "Dekra Paris Nord",
                            CenterAddress = "45 Avenue de Saint-Ouen, 75017 Paris",
                            Cost = 85.00m,
                            Remarks = "Défaillance critique sur injecteurs et opacité des fumées."
                        },
                        // Expiring inspection (14 days remaining)
                        new()
                        {
                            VehicleId = vehicle5.Id,
                            InspectionDate = DateTime.UtcNow.AddYears(-2).AddDays(14),
                            ExpiryDate = DateTime.UtcNow.AddDays(14), // EXPIRES IN 14 DAYS
                            Result = "Pass",
                            CenterName = "Autovision Paris Sud",
                            CenterAddress = "6 Boulevard Jourdan, 75014 Paris",
                            Cost = 79.00m,
                            Remarks = "Plaquettes arrières à surveiller."
                        }
                    };

                    context.TechnicalInspections.AddRange(inspections);
                    context.SaveChanges();
                }
            }

            // Seed Fuel Logs
            if (!context.FuelLogs.Any())
            {
                var vehicle1 = context.Vehicles.FirstOrDefault(v => v.Matricule == "123-AB-45");
                var vehicle2 = context.Vehicles.FirstOrDefault(v => v.Matricule == "987-CD-65");

                if (vehicle1 != null && vehicle2 != null)
                {
                    var fuelLogs = new List<FuelLog>
                    {
                        // Peugeot 208 logs
                        new() { VehicleId = vehicle1.Id, Date = DateTime.UtcNow.AddDays(-28), KmValue = 44100, Liters = 40.00m, CostPerLiter = 1.85m, TotalCost = 74.00m, StationName = "Shell Paris", FuelType = "Gasoline", IsAnomaly = false },
                        new() { VehicleId = vehicle1.Id, Date = DateTime.UtcNow.AddDays(-20), KmValue = 44700, Liters = 38.00m, CostPerLiter = 1.87m, TotalCost = 71.06m, StationName = "Total Paris", FuelType = "Gasoline", IsAnomaly = false },
                        new() { VehicleId = vehicle1.Id, Date = DateTime.UtcNow.AddDays(-10), KmValue = 45000, Liters = 18.00m, CostPerLiter = 1.89m, TotalCost = 34.02m, StationName = "Esso Paris", FuelType = "Gasoline", IsAnomaly = false },

                        // Renault Clio 5 logs
                        new() { VehicleId = vehicle2.Id, Date = DateTime.UtcNow.AddDays(-15), KmValue = 82000, Liters = 45.00m, CostPerLiter = 1.75m, TotalCost = 78.75m, StationName = "Total Lyon", FuelType = "Diesel", IsAnomaly = false },
                        // Anomaly Log: high fuel consumption
                        new() { VehicleId = vehicle2.Id, Date = DateTime.UtcNow.AddDays(-2), KmValue = 85000, Liters = 48.00m, CostPerLiter = 1.76m, TotalCost = 84.48m, StationName = "Shell Lyon", FuelType = "Diesel", IsAnomaly = true }
                    };

                    context.FuelLogs.AddRange(fuelLogs);
                    context.SaveChanges();
                }
            }
        }
    }
}
