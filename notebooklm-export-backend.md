# Parc Auto - Backend (.NET C#)

## Architecture: ASP.NET Core Web API with SQLite

---
### Program.cs (Entry Point)
```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using Backend.Data;
using Backend.Helpers;
using Backend.Models;

var builder = WebApplication.CreateBuilder(args);

// Add HttpContextAccessor to support Auditing in DataContext
builder.Services.AddHttpContextAccessor();

// Configure EF Core SQLite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=parc_auto.db";
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(connectionString));

// Add controllers with JSON settings to ignore cycles and serialize enums as strings
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Configure Swagger with JWT authorization support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Parc Auto Rental API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Configure JWT Authentication
var secretKey = builder.Configuration["Jwt:Secret"] ?? "super_secret_key_that_is_long_enough_for_sha256_admin_parc_auto_2026";
var key = Encoding.UTF8.GetBytes(secretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "ParcAutoAPI",
        ValidAudience = builder.Configuration["Jwt:Audience"] ?? "ParcAutoClient",
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(); // Serve files from wwwroot/ e.g. uploaded vehicle photos, policy documents

// Ensure uploads folder exists in wwwroot
var uploadsRoot = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "uploads");
if (!Directory.Exists(uploadsRoot))
{
    Directory.CreateDirectory(uploadsRoot);
}

app.UseCors("AllowAngular");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Apply migrations and seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<DataContext>();
        context.Database.Migrate(); // Apply any pending migrations

        // Check if admin user exists, else seed it
        if (!context.Users.Any())
        {
            var (hash, salt) = HashHelper.CreatePasswordHash("AdminPassword123!");
            var defaultAdmin = new User
            {
                Username = "admin",
                FullName = "Administrator",
                PasswordHash = hash,
                Salt = salt
            };
            context.Users.Add(defaultAdmin);
            context.SaveChanges();
        }

        // Seed default settings if empty
        if (!context.GlobalSettings.Any())
        {
            var defaultSettings = new GlobalSettings();
            context.GlobalSettings.Add(defaultSettings);
            context.SaveChanges();
        }

        // Seed default consumable configs if empty
        if (!context.ConsumableConfigs.Any())
        {
            var configs = new List<ConsumableConfig>
            {
                new() { ConsumableType = "OilChange", IntervalKm = 10000, IntervalMonths = 12 },
                new() { ConsumableType = "OilFilter", IntervalKm = 10000, IntervalMonths = 12 },
                new() { ConsumableType = "AirFilter", IntervalKm = 20000, IntervalMonths = 24 },
                new() { ConsumableType = "FuelFilter", IntervalKm = 30000, IntervalMonths = 24 },
                new() { ConsumableType = "CabinFilter", IntervalKm = 20000, IntervalMonths = 12 },
                new() { ConsumableType = "FrontBrakes", IntervalKm = 40000, IntervalMonths = 0 },
                new() { ConsumableType = "RearBrakes", IntervalKm = 60000, IntervalMonths = 0 },
                new() { ConsumableType = "FrontTires", IntervalKm = 50000, IntervalMonths = 48 },
                new() { ConsumableType = "RearTires", IntervalKm = 50000, IntervalMonths = 48 },
                new() { ConsumableType = "Battery", IntervalKm = 0, IntervalMonths = 36 }
            };
            context.ConsumableConfigs.AddRange(configs);
            context.SaveChanges();
        }

        // Seed mock data for all tables
        DbSeeder.SeedMockData(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during database migration/seeding.");
    }
}

app.Run();

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Backend\Models\AuditLog.cs
```csharp
using System;

namespace Backend.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Username { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty; // Create, Update, Delete
        public string EntityName { get; set; } = string.Empty;
        public string EntityId { get; set; } = string.Empty;
        public string ChangesJson { get; set; } = string.Empty; // Store JSON diff or details
    }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Backend\Models\Client.cs
```csharp
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
        public string Email { get; set; } = string.Empty;

        [MaxLength(250)]
        public string Address { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;
    }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Backend\Models\Consumable.cs
```csharp
using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    // Configuration thresholds for consumables
    public class ConsumableConfig
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string ConsumableType { get; set; } = string.Empty; // e.g. OilChange, AirFilter, FrontBrakes, etc.

        public int IntervalKm { get; set; } // km interval (0 if not km-based)
        public int IntervalMonths { get; set; } // month interval (0 if not time-based)
    }

    // Replacement records
    public class ConsumableLog
    {
        public int Id { get; set; }

        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

        [Required]
        [MaxLength(100)]
        public string ConsumableType { get; set; } = string.Empty; // Predefined or User-defined

        public DateTime ReplacementDate { get; set; }
        public int ReplacementKm { get; set; }

        // Specific details
        [MaxLength(100)]
        public string OilType { get; set; } = string.Empty; // Viscosity like 5W-40, for OilChange
        [MaxLength(50)]
        public string Viscosity { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Brand { get; set; } = string.Empty; // Tire brand, Battery brand, etc.
        [MaxLength(100)]
        public string Size { get; set; } = string.Empty; // Tire size
        [MaxLength(100)]
        public string TypeDetail { get; set; } = string.Empty; // Battery capacity, Tire type, filter type
        [MaxLength(50)]
        public string Axle { get; set; } = string.Empty; // Front / Rear axle for Brakes/Tires

        public string Notes { get; set; } = string.Empty;
    }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Backend\Models\FuelLog.cs
```csharp
using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class FuelLog
    {
        public int Id { get; set; }

        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

        public DateTime Date { get; set; }

        public int KmValue { get; set; } // Odometer km at fill-up
        public decimal Liters { get; set; }
        public decimal CostPerLiter { get; set; }
        public decimal TotalCost { get; set; } // Liters * CostPerLiter

        [MaxLength(150)]
        public string StationName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string FuelType { get; set; } = string.Empty;

        public bool IsAnomaly { get; set; } = false; // Flagged if deviates from average
    }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Backend\Models\GlobalSettings.cs
```csharp
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class GlobalSettings
    {
        public int Id { get; set; }

        [MaxLength(10)]
        public string CurrencySymbol { get; set; } = "â‚¬";

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
        public string VehicleTypesJson { get; set; } = "[\"Car\", \"SUV\", \"Van\", \"Truck\", \"Motorcycle\"]";
        public string FuelTypesJson { get; set; } = "[\"Gasoline\", \"Diesel\", \"Electric\", \"Hybrid\", \"LPG\"]";
        public string MaintenanceTypesJson { get; set; } = "[\"Preventive\", \"Corrective\", \"AccidentRepair\", \"Inspection\"]";
        public string ExtrasJson { get; set; } = "[{\"Name\":\"GPS\",\"Price\":5.0},{\"Name\":\"Child Seat\",\"Price\":3.0},{\"Name\":\"Additional Driver\",\"Price\":10.0}]";
    }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Backend\Models\InsurancePolicy.cs
```csharp
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

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Backend\Models\KmEntry.cs
```csharp
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

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Backend\Models\Maintenance.cs
```csharp
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

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Backend\Models\RentalContract.cs
```csharp
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

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Backend\Models\TechnicalInspection.cs
```csharp
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

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Backend\Models\User.cs
```csharp
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string Salt { get; set; } = string.Empty;

        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;
    }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Backend\Models\Vehicle.cs
```csharp
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

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Backend\Data\DataContext.cs
```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Data
{
    public class DataContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DataContext(DbContextOptions<DataContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<RentalContract> RentalContracts => Set<RentalContract>();
        public DbSet<KmEntry> KmEntries => Set<KmEntry>();
        public DbSet<Maintenance> Maintenances => Set<Maintenance>();
        public DbSet<ConsumableConfig> ConsumableConfigs => Set<ConsumableConfig>();
        public DbSet<ConsumableLog> ConsumableLogs => Set<ConsumableLog>();
        public DbSet<InsurancePolicy> InsurancePolicies => Set<InsurancePolicy>();
        public DbSet<TechnicalInspection> TechnicalInspections => Set<TechnicalInspection>();
        public DbSet<FuelLog> FuelLogs => Set<FuelLog>();
        public DbSet<GlobalSettings> GlobalSettings => Set<GlobalSettings>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure unique constraints
            modelBuilder.Entity<Vehicle>()
                .HasIndex(v => v.Matricule)
                .IsUnique();

            modelBuilder.Entity<Vehicle>()
                .HasIndex(v => v.VIN)
                .IsUnique();

            modelBuilder.Entity<Client>()
                .HasIndex(c => c.NationalId)
                .IsUnique();

            modelBuilder.Entity<RentalContract>()
                .HasIndex(rc => rc.ContractNumber)
                .IsUnique();
        }

        public override int SaveChanges()
        {
            OnBeforeSaveChanges();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            OnBeforeSaveChanges();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void OnBeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditLog>();
            var currentUsername = GetCurrentUsername();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is AuditLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var auditLog = new AuditLog
                {
                    Timestamp = DateTime.UtcNow,
                    Username = currentUsername,
                    EntityName = entry.Metadata.DisplayName(),
                    Action = entry.State.ToString()
                };

                // Try to get primary key
                var keyProperties = entry.Metadata.FindPrimaryKey()?.Properties;
                if (keyProperties != null)
                {
                    var keys = keyProperties.Select(p => entry.Property(p.Name).CurrentValue?.ToString()).ToList();
                    auditLog.EntityId = string.Join(",", keys);
                }

                // Capture changes
                var changes = new Dictionary<string, object?>();
                if (entry.State == EntityState.Added)
                {
                    foreach (var prop in entry.Properties)
                    {
                        changes[prop.Metadata.Name] = prop.CurrentValue;
                    }
                }
                else if (entry.State == EntityState.Deleted)
                {
                    foreach (var prop in entry.Properties)
                    {
                        changes[prop.Metadata.Name] = prop.OriginalValue;
                    }
                }
                else if (entry.State == EntityState.Modified)
                {
                    foreach (var prop in entry.Properties)
                    {
                        if (prop.IsModified)
                        {
                            changes[prop.Metadata.Name] = new
                            {
                                Original = prop.OriginalValue,
                                Current = prop.CurrentValue
                            };
                        }
                    }
                }

                auditLog.ChangesJson = JsonSerializer.Serialize(changes);
                auditEntries.Add(auditLog);
            }

            if (auditEntries.Count > 0)
            {
                AuditLogs.AddRange(auditEntries);
            }
        }

        private string GetCurrentUsername()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var usernameClaim = user.FindFirst(ClaimTypes.Name) ?? user.FindFirst(ClaimTypes.NameIdentifier);
                if (usernameClaim != null)
                {
                    return usernameClaim.Value;
                }
            }
            return "System";
        }
    }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Backend\Data\DbSeeder.cs
```csharp
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
                    // --- 4x LIVAN X3 PRO ---
                    new()
                    {
                        Matricule = "123-AB-45", // Used in mock relations
                        Brand = "Livan",
                        Model = "X3 Pro",
                        Year = 2023,
                        Type = "SUV",
                        FuelType = "Gasoline",
                        Transmission = "Automatic",
                        VIN = "VF31LIVAN11111111",
                        EngineNumber = "ENG-LIVAN-01",
                        Color = "White",
                        SeatsCount = 5,
                        DailyRate = 60.00m,
                        Status = VehicleStatus.Available,
                        PurchaseDate = DateTime.UtcNow.AddMonths(-10),
                        PurchasePrice = 22000.00m,
                        InitialKm = 1000,
                        CurrentKm = 15000,
                        Notes = "VÃ©hicule nÂ°1"
                    },
                    new()
                    {
                        Matricule = "456-EF-78", // Used in mock relations
                        Brand = "Livan",
                        Model = "X3 Pro",
                        Year = 2023,
                        Type = "SUV",
                        FuelType = "Gasoline",
                        Transmission = "Automatic",
                        VIN = "VF31LIVAN22222222",
                        EngineNumber = "ENG-LIVAN-02",
                        Color = "Grey",
                        SeatsCount = 5,
                        DailyRate = 60.00m,
                        Status = VehicleStatus.Reserved,
                        PurchaseDate = DateTime.UtcNow.AddMonths(-10),
                        PurchasePrice = 22000.00m,
                        InitialKm = 1000,
                        CurrentKm = 14500,
                        Notes = "VÃ©hicule nÂ°2"
                    },
                    new()
                    {
                        Matricule = "LIV-03-DZ",
                        Brand = "Livan",
                        Model = "X3 Pro",
                        Year = 2023,
                        Type = "SUV",
                        FuelType = "Gasoline",
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
                        CurrentKm = 12000,
                        Notes = "VÃ©hicule nÂ°3"
                    },
                    new()
                    {
                        Matricule = "LIV-04-DZ",
                        Brand = "Livan",
                        Model = "X3 Pro",
                        Year = 2023,
                        Type = "SUV",
                        FuelType = "Gasoline",
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
                        CurrentKm = 11000,
                        Notes = "VÃ©hicule nÂ°4"
                    },

                    // --- 4x VOLKSWAGEN THARU ---
                    new()
                    {
                        Matricule = "987-CD-65", // Used in mock relations
                        Brand = "Volkswagen",
                        Model = "Tharu",
                        Year = 2022,
                        Type = "SUV",
                        FuelType = "Gasoline",
                        Transmission = "Automatic",
                        VIN = "VW1THARU11111111",
                        EngineNumber = "ENG-THARU-01",
                        Color = "Black",
                        SeatsCount = 5,
                        DailyRate = 85.00m,
                        Status = VehicleStatus.Rented,
                        PurchaseDate = DateTime.UtcNow.AddYears(-2),
                        PurchasePrice = 35000.00m,
                        InitialKm = 2000,
                        CurrentKm = 42000,
                        Notes = "VÃ©hicule nÂ°1"
                    },
                    new()
                    {
                        Matricule = "789-IJ-01", // Used in mock relations
                        Brand = "Volkswagen",
                        Model = "Tharu",
                        Year = 2022,
                        Type = "SUV",
                        FuelType = "Gasoline",
                        Transmission = "Automatic",
                        VIN = "VW1THARU22222222",
                        EngineNumber = "ENG-THARU-02",
                        Color = "Grey",
                        SeatsCount = 5,
                        DailyRate = 85.00m,
                        Status = VehicleStatus.Available,
                        PurchaseDate = DateTime.UtcNow.AddYears(-2),
                        PurchasePrice = 35000.00m,
                        InitialKm = 2000,
                        CurrentKm = 38000,
                        Notes = "VÃ©hicule nÂ°2"
                    },
                    new()
                    {
                        Matricule = "THA-03-DZ",
                        Brand = "Volkswagen",
                        Model = "Tharu",
                        Year = 2023,
                        Type = "SUV",
                        FuelType = "Gasoline",
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
                        CurrentKm = 15000,
                        Notes = "VÃ©hicule nÂ°3"
                    },
                    new()
                    {
                        Matricule = "THA-04-DZ",
                        Brand = "Volkswagen",
                        Model = "Tharu",
                        Year = 2023,
                        Type = "SUV",
                        FuelType = "Gasoline",
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
                        CurrentKm = 14000,
                        Notes = "VÃ©hicule nÂ°4"
                    },

                    // --- 1x FIAT DOBLO VITRÃ‰ ---
                    new()
                    {
                        Matricule = "654-GH-98", // Used in mock relations
                        Brand = "Fiat",
                        Model = "Doblo VitrÃ©",
                        Year = 2021,
                        Type = "Van",
                        FuelType = "Diesel",
                        Transmission = "Automatic",
                        VIN = "WDB6543210987JKL4",
                        EngineNumber = "ENG-DOBLO-01",
                        Color = "White",
                        SeatsCount = 5,
                        DailyRate = 70.00m,
                        Status = VehicleStatus.InMaintenance,
                        PurchaseDate = DateTime.UtcNow.AddYears(-3),
                        PurchasePrice = 25000.00m,
                        InitialKm = 5000,
                        CurrentKm = 85000,
                        Notes = "VÃ©hicule utilitaire nÂ°1"
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
                        Notes = "Client rÃ©gulier, aucun problÃ¨me signalÃ©."
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
                        Address = "45 Avenue de la RÃ©publique, 69002 Lyon",
                        Notes = "Alerte : permis de conduire expirÃ© !"
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
                        Notes = "Permis expire bientÃ´t."
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
                        Address = "23 Avenue Jean JaurÃ¨s, 33000 Bordeaux",
                        Notes = "Paiement toujours en rÃ¨gle."
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
                            Notes = "Retour en parfait Ã©tat, nettoyÃ©."
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
                            Notes = "VÃ©hicule louÃ© pour dÃ©placement professionnel."
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
                            Notes = "RÃ©servation pour vacances familiales."
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
                        new() { VehicleId = vehicle1.Id, Date = DateTime.UtcNow.AddDays(-30), KmValue = 44000, Source = "ContractStart", Notes = "DÃ©part contrat CTR-20260501-001" },
                        new() { VehicleId = vehicle1.Id, Date = DateTime.UtcNow.AddDays(-25), KmValue = 44500, Source = "ContractReturn", Notes = "Retour contrat CTR-20260501-001" },
                        new() { VehicleId = vehicle1.Id, Date = DateTime.UtcNow.AddDays(-15), KmValue = 45000, Source = "Manual", Notes = "RelevÃ© mensuel interne" },

                        // Renault Clio 5 history
                        new() { VehicleId = vehicle2.Id, Date = DateTime.UtcNow.AddDays(-60), KmValue = 78000, Source = "Manual", Notes = "ContrÃ´le entretien vidange" },
                        new() { VehicleId = vehicle2.Id, Date = DateTime.UtcNow.AddDays(-2), KmValue = 85000, Source = "ContractStart", Notes = "DÃ©part contrat CTR-20260601-002" }
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
                            Description = "ProblÃ¨me d'injecteurs de carburant. Perte de puissance moteur.",
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
                            Description = "Entretien rÃ©gulier avec changement des plaquettes de frein avant.",
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
                            Notes = "Vidange standard et changement filtre Ã  huile."
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
                            Notes = "Plaquettes changÃ©es lors de la maintenance pÃ©riodique."
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
                            Notes = "Remplacement pneu usÃ©."
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
                            Remarks = "Aucune dÃ©faillance majeure."
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
                            Remarks = "DÃ©faillance critique sur injecteurs et opacitÃ© des fumÃ©es."
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
                            Remarks = "Plaquettes arriÃ¨res Ã  surveiller."
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

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Backend\Helpers\HashHelper.cs
```csharp
using System;
using System.Security.Cryptography;
using System.Text;

namespace Backend.Helpers
{
    public static class HashHelper
    {
        public static (string hash, string salt) CreatePasswordHash(string password)
        {
            using var hmac = new HMACSHA512();
            var salt = Convert.ToBase64String(hmac.Key);
            var hash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            return (hash, salt);
        }

        public static bool VerifyPasswordHash(string password, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            using var hmac = new HMACSHA512(saltBytes);
            var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            return computedHash == storedHash;
        }
    }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Backend\Helpers\TokenHelper.cs
```csharp
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Backend.Models;

namespace Backend.Helpers
{
    public static class TokenHelper
    {
        public static string GenerateJwtToken(User user, IConfiguration config)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = config["Jwt:Secret"] ?? "super_secret_key_that_is_long_enough_for_sha256_admin_parc_auto_2026";
            var key = Encoding.UTF8.GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, "Admin")
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = config["Jwt:Issuer"] ?? "ParcAutoAPI",
                Audience = config["Jwt:Audience"] ?? "ParcAutoClient"
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Backend\Controllers\AlertController.cs
```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AlertController : ControllerBase
    {
        private readonly DataContext _context;

        public AlertController(DataContext context)
        {
            _context = context;
        }

        public class AlertItem
        {
            public string Type { get; set; } = string.Empty; // Maintenance, Consumable, Insurance, Inspection, OdometerInactivity, DriverLicense
            public string Severity { get; set; } = "Info"; // Info, Warning, Critical
            public string Target { get; set; } = string.Empty; // Vehicle plate or Client name
            public string Message { get; set; } = string.Empty;
            public int? VehicleId { get; set; }
            public int? ClientId { get; set; }
            public string DaysOrKmLeftText { get; set; } = string.Empty;
            public DateTime DateConcerned { get; set; }
        }

        [HttpGet]
        public async Task<IActionResult> GetAlerts()
        {
            var alerts = new List<AlertItem>();
            var settings = await _context.GlobalSettings.FirstOrDefaultAsync() ?? new GlobalSettings();
            var vehicles = await _context.Vehicles.ToListAsync();
            var clients = await _context.Clients.ToListAsync();

            var today = DateTime.UtcNow.Date;

            // 1. Insurance Expiry
            var insuranceThresholds = settings.InsuranceExpiryDaysThresholds.Split(',').Select(int.Parse).ToList();
            var maxInsuranceThreshold = insuranceThresholds.Max();

            var policies = await _context.InsurancePolicies
                .GroupBy(p => p.VehicleId)
                .Select(g => g.OrderByDescending(p => p.ExpiryDate).FirstOrDefault())
                .ToListAsync();

            foreach (var policy in policies)
            {
                if (policy == null) continue;
                var vehicle = vehicles.FirstOrDefault(v => v.Id == policy.VehicleId);
                var daysRemaining = (policy.ExpiryDate.Date - today).Days;

                if (daysRemaining < 0)
                {
                    alerts.Add(new AlertItem
                    {
                        Type = "Insurance",
                        Severity = "Critical",
                        Target = vehicle?.Matricule ?? $"Vehicle #{policy.VehicleId}",
                        Message = $"Insurance expired on {policy.ExpiryDate:yyyy-MM-dd}.",
                        VehicleId = policy.VehicleId,
                        DaysOrKmLeftText = $"{Math.Abs(daysRemaining)} days overdue",
                        DateConcerned = policy.ExpiryDate
                    });
                }
                else if (daysRemaining <= maxInsuranceThreshold)
                {
                    string severity = "Info";
                    if (daysRemaining <= 7) severity = "Critical";
                    else if (daysRemaining <= 15) severity = "Warning";

                    alerts.Add(new AlertItem
                    {
                        Type = "Insurance",
                        Severity = severity,
                        Target = vehicle?.Matricule ?? $"Vehicle #{policy.VehicleId}",
                        Message = $"Insurance expiring soon in {daysRemaining} days.",
                        VehicleId = policy.VehicleId,
                        DaysOrKmLeftText = $"{daysRemaining} days remaining",
                        DateConcerned = policy.ExpiryDate
                    });
                }
            }

            // 2. Technical Inspection Expiry
            var inspectionThresholds = settings.InspectionExpiryDaysThresholds.Split(',').Select(int.Parse).ToList();
            var maxInspectionThreshold = inspectionThresholds.Max();

            var inspections = await _context.TechnicalInspections
                .GroupBy(i => i.VehicleId)
                .Select(g => g.OrderByDescending(i => i.ExpiryDate).FirstOrDefault())
                .ToListAsync();

            foreach (var inspection in inspections)
            {
                if (inspection == null) continue;
                var vehicle = vehicles.FirstOrDefault(v => v.Id == inspection.VehicleId);
                var daysRemaining = (inspection.ExpiryDate.Date - today).Days;

                if (inspection.Result.ToLower() == "fail")
                {
                    alerts.Add(new AlertItem
                    {
                        Type = "Inspection",
                        Severity = "Critical",
                        Target = vehicle?.Matricule ?? $"Vehicle #{inspection.VehicleId}",
                        Message = "Technical inspection failed! Vehicle requires repairs.",
                        VehicleId = inspection.VehicleId,
                        DaysOrKmLeftText = "FAILED",
                        DateConcerned = inspection.InspectionDate
                    });
                }
                else if (daysRemaining < 0)
                {
                    alerts.Add(new AlertItem
                    {
                        Type = "Inspection",
                        Severity = "Critical",
                        Target = vehicle?.Matricule ?? $"Vehicle #{inspection.VehicleId}",
                        Message = $"Technical inspection expired on {inspection.ExpiryDate:yyyy-MM-dd}.",
                        VehicleId = inspection.VehicleId,
                        DaysOrKmLeftText = $"{Math.Abs(daysRemaining)} days overdue",
                        DateConcerned = inspection.ExpiryDate
                    });
                }
                else if (daysRemaining <= maxInspectionThreshold)
                {
                    string severity = "Info";
                    if (daysRemaining <= 7) severity = "Critical";
                    else if (daysRemaining <= 15) severity = "Warning";

                    alerts.Add(new AlertItem
                    {
                        Type = "Inspection",
                        Severity = severity,
                        Target = vehicle?.Matricule ?? $"Vehicle #{inspection.VehicleId}",
                        Message = $"Technical inspection expiring in {daysRemaining} days.",
                        VehicleId = inspection.VehicleId,
                        DaysOrKmLeftText = $"{daysRemaining} days remaining",
                        DateConcerned = inspection.ExpiryDate
                    });
                }
            }

            // 3. Maintenance Due
            var scheduledMaintenances = await _context.Maintenances
                .Where(m => m.Status == MaintenanceStatus.Scheduled)
                .ToListAsync();

            foreach (var maint in scheduledMaintenances)
            {
                var vehicle = vehicles.FirstOrDefault(v => v.Id == maint.VehicleId);
                var daysRemaining = (maint.DatePerformed.Date - today).Days;

                if (daysRemaining < 0)
                {
                    alerts.Add(new AlertItem
                    {
                        Type = "Maintenance",
                        Severity = "Critical",
                        Target = vehicle?.Matricule ?? $"Vehicle #{maint.VehicleId}",
                        Message = $"Scheduled {maint.MaintenanceType} is overdue by {Math.Abs(daysRemaining)} days.",
                        VehicleId = maint.VehicleId,
                        DaysOrKmLeftText = $"{Math.Abs(daysRemaining)} days overdue",
                        DateConcerned = maint.DatePerformed
                    });
                }
                else if (daysRemaining <= settings.MaintenanceNotifyDaysBefore)
                {
                    alerts.Add(new AlertItem
                    {
                        Type = "Maintenance",
                        Severity = "Warning",
                        Target = vehicle?.Matricule ?? $"Vehicle #{maint.VehicleId}",
                        Message = $"Upcoming scheduled {maint.MaintenanceType} in {daysRemaining} days.",
                        VehicleId = maint.VehicleId,
                        DaysOrKmLeftText = $"{daysRemaining} days left",
                        DateConcerned = maint.DatePerformed
                    });
                }
            }

            // 4. Odometer Inactivity
            var allKmLogs = await _context.KmEntries.ToListAsync();
            foreach (var vehicle in vehicles)
            {
                var lastLog = allKmLogs
                    .Where(ke => ke.VehicleId == vehicle.Id)
                    .OrderByDescending(ke => ke.Date)
                    .FirstOrDefault();

                var lastDate = lastLog?.Date ?? vehicle.PurchaseDate;
                var daysInactive = (DateTime.UtcNow - lastDate).Days;

                if (daysInactive > settings.KmInactivityDaysThreshold)
                {
                    alerts.Add(new AlertItem
                    {
                        Type = "OdometerInactivity",
                        Severity = "Warning",
                        Target = vehicle.Matricule,
                        Message = $"No mileage activity registered for {daysInactive} days.",
                        VehicleId = vehicle.Id,
                        DaysOrKmLeftText = $"{daysInactive} days inactive",
                        DateConcerned = lastDate
                    });
                }
            }

            // 5. Driver's License Expiry
            foreach (var client in clients)
            {
                var daysRemaining = (client.LicenseExpiryDate.Date - today).Days;
                if (daysRemaining < 0)
                {
                    alerts.Add(new AlertItem
                    {
                        Type = "DriverLicense",
                        Severity = "Critical",
                        Target = client.FullName,
                        Message = $"Driver's license expired on {client.LicenseExpiryDate:yyyy-MM-dd}.",
                        ClientId = client.Id,
                        DaysOrKmLeftText = $"{Math.Abs(daysRemaining)} days overdue",
                        DateConcerned = client.LicenseExpiryDate
                    });
                }
                else if (daysRemaining <= 30) // license alert fixed warning window
                {
                    alerts.Add(new AlertItem
                    {
                        Type = "DriverLicense",
                        Severity = "Warning",
                        Target = client.FullName,
                        Message = $"Driver's license expiring in {daysRemaining} days.",
                        ClientId = client.Id,
                        DaysOrKmLeftText = $"{daysRemaining} days left",
                        DateConcerned = client.LicenseExpiryDate
                    });
                }
            }

            // 6. Consumables Expiry
            var configs = await _context.ConsumableConfigs.ToListAsync();
            var consumableLogs = await _context.ConsumableLogs.ToListAsync();

            var standardTypes = new List<string>
            {
                "OilChange", "AirFilter", "OilFilter", "FuelFilter", "CabinFilter",
                "FrontBrakes", "RearBrakes", "FrontTires", "RearTires", "Battery"
            };
            var allTypes = configs.Select(c => c.ConsumableType).Union(standardTypes).Distinct();

            foreach (var vehicle in vehicles)
            {
                var vehicleLogs = consumableLogs.Where(l => l.VehicleId == vehicle.Id).ToList();

                foreach (var type in allTypes)
                {
                    var config = configs.FirstOrDefault(c => c.ConsumableType.ToLower() == type.ToLower());
                    var typeLogs = vehicleLogs.Where(l => l.ConsumableType.ToLower() == type.ToLower()).OrderByDescending(l => l.ReplacementDate).ToList();
                    var lastLog = typeLogs.FirstOrDefault();

                    int intervalKm = config?.IntervalKm ?? GetDefaultIntervalKm(type);
                    int intervalMonths = config?.IntervalMonths ?? GetDefaultIntervalMonths(type);

                    DateTime lastDate = lastLog?.ReplacementDate ?? vehicle.PurchaseDate;
                    int lastKm = lastLog?.ReplacementKm ?? vehicle.InitialKm;

                    int kmSince = vehicle.CurrentKm - lastKm;
                    int monthsSince = ((DateTime.UtcNow.Year - lastDate.Year) * 12) + DateTime.UtcNow.Month - lastDate.Month;

                    bool isDueKm = intervalKm > 0 && kmSince >= intervalKm;
                    bool isDueMonths = intervalMonths > 0 && monthsSince >= intervalMonths;

                    bool isWarningKm = intervalKm > 0 && !isDueKm && (intervalKm - kmSince) <= settings.ConsumableNotifyKmBefore;
                    bool isWarningMonths = intervalMonths > 0 && !isDueMonths && ((lastDate.AddMonths(intervalMonths) - DateTime.UtcNow).Days <= settings.ConsumableNotifyDaysBefore);

                    if (isDueKm || isDueMonths)
                    {
                        alerts.Add(new AlertItem
                        {
                            Type = "Consumable",
                            Severity = "Critical",
                            Target = vehicle.Matricule,
                            Message = $"{type} replacement is overdue. Interval: {intervalKm}km / {intervalMonths}m. Current: {kmSince}km / {monthsSince}m.",
                            VehicleId = vehicle.Id,
                            DaysOrKmLeftText = isDueKm ? $"{kmSince - intervalKm} km overdue" : $"{monthsSince - intervalMonths} months overdue",
                            DateConcerned = lastDate
                        });
                    }
                    else if (isWarningKm || isWarningMonths)
                    {
                        alerts.Add(new AlertItem
                        {
                            Type = "Consumable",
                            Severity = "Warning",
                            Target = vehicle.Matricule,
                            Message = $"{type} replacement approaching due threshold. Current: {kmSince}km / {monthsSince}m.",
                            VehicleId = vehicle.Id,
                            DaysOrKmLeftText = isWarningKm ? $"{intervalKm - kmSince} km left" : $"{(lastDate.AddMonths(intervalMonths) - DateTime.UtcNow).Days} days left",
                            DateConcerned = lastDate
                        });
                    }
                }
            }

            return Ok(new
            {
                count = alerts.Count,
                criticalCount = alerts.Count(a => a.Severity == "Critical"),
                warningCount = alerts.Count(a => a.Severity == "Warning"),
                infoCount = alerts.Count(a => a.Severity == "Info"),
                alerts = alerts.OrderByDescending(a => a.Severity == "Critical").ThenByDescending(a => a.Severity == "Warning").ToList()
            });
        }

        private static int GetDefaultIntervalKm(string type)
        {
            return type.ToLower() switch
            {
                "oilchange" => 10000,
                "oilfilter" => 10000,
                "airfilter" => 20000,
                "fuelfilter" => 30000,
                "cabinfilter" => 20000,
                "frontbrakes" => 40000,
                "rearbrakes" => 60000,
                "fronttires" => 50000,
                "reartires" => 50000,
                _ => 0
            };
        }

        private static int GetDefaultIntervalMonths(string type)
        {
            return type.ToLower() switch
            {
                "oilchange" => 12,
                "battery" => 36,
                "fronttires" => 48,
                "reartires" => 48,
                _ => 0
            };
        }
    }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Backend\Controllers\AuthController.cs
```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Helpers;
using Backend.Models;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;

        public AuthController(DataContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public class LoginDto
        {
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == dto.Username.ToLower());
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            if (!HashHelper.VerifyPasswordHash(dto.Password, user.PasswordHash, user.Salt))
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            var token = TokenHelper.GenerateJwtToken(user, _config);
            return Ok(new
            {
                token,
                user = new
                {
                    user.Id,
                    user.Username,
                    user.FullName
                }
            });
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username)) return Unauthorized();

            var user = await _context.Users
                .Select(u => new { u.Id, u.Username, u.FullName })
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null) return NotFound(new { message = "User not found" });

            return Ok(user);
        }

        public class UpdateProfileDto
        {
            public string FullName { get; set; } = string.Empty;
        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username)) return Unauthorized();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return NotFound(new { message = "User not found" });

            user.FullName = dto.FullName;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Profile updated successfully", user = new { user.Id, user.Username, user.FullName } });
        }

        public class ChangePasswordDto
        {
            public string CurrentPassword { get; set; } = string.Empty;
            public string NewPassword { get; set; } = string.Empty;
        }

        [Authorize]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username)) return Unauthorized();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return NotFound(new { message = "User not found" });

            if (!HashHelper.VerifyPasswordHash(dto.CurrentPassword, user.PasswordHash, user.Salt))
            {
                return BadRequest(new { message = "Incorrect current password" });
            }

            var (hash, salt) = HashHelper.CreatePasswordHash(dto.NewPassword);
            user.PasswordHash = hash;
            user.Salt = salt;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Password changed successfully" });
        }
    }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Backend\Controllers\ClientController.cs
```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly DataContext _context;

        public ClientController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _context.Clients.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var lowerSearch = search.ToLower();
                query = query.Where(c => c.FullName.ToLower().Contains(lowerSearch) ||
                                         c.Phone.ToLower().Contains(lowerSearch) ||
                                         c.Email.ToLower().Contains(lowerSearch) ||
                                         c.NationalId.ToLower().Contains(lowerSearch) ||
                                         c.LicenseNumber.ToLower().Contains(lowerSearch));
            }

            var totalCount = await query.CountAsync();
            var clients = await query
                .OrderByDescending(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                totalCount,
                page,
                pageSize,
                data = clients
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound(new { message = "Client not found" });

            // Include history
            var history = await _context.RentalContracts
                .Where(c => c.ClientId == id)
                .Select(c => new
                {
                    c.Id,
                    c.ContractNumber,
                    c.VehicleId,
                    VehicleBrand = c.Vehicle != null ? c.Vehicle.Brand : "",
                    VehicleModel = c.Vehicle != null ? c.Vehicle.Model : "",
                    VehicleMatricule = c.Vehicle != null ? c.Vehicle.Matricule : "",
                    c.StartDate,
                    c.ExpectedReturnDate,
                    c.ActualReturnDate,
                    c.FinalAmountDue,
                    c.ContractStatus,
                    c.PaymentStatus
                })
                .OrderByDescending(c => c.StartDate)
                .ToListAsync();

            return Ok(new
            {
                client,
                history
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Client client)
        {
            // Verify national ID uniqueness
            if (await _context.Clients.AnyAsync(c => c.NationalId == client.NationalId))
            {
                return BadRequest(new { message = $"Client with National ID / Passport '{client.NationalId}' is already registered." });
            }

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = client.Id }, new { client });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Client updated)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound(new { message = "Client not found" });

            if (updated.NationalId != client.NationalId && await _context.Clients.AnyAsync(c => c.NationalId == updated.NationalId))
            {
                return BadRequest(new { message = $"Client with National ID / Passport '{updated.NationalId}' is already registered." });
            }

            client.FullName = updated.FullName;
            client.NationalId = updated.NationalId;
            client.DateOfBirth = updated.DateOfBirth;
            client.LicenseNumber = updated.LicenseNumber;
            client.LicenseCategory = updated.LicenseCategory;
            client.LicenseIssueDate = updated.LicenseIssueDate;
            client.LicenseExpiryDate = updated.LicenseExpiryDate;
            client.Phone = updated.Phone;
            client.Email = updated.Email;
            client.Address = updated.Address;
            client.Notes = updated.Notes;

            await _context.SaveChangesAsync();
            return Ok(client);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound(new { message = "Client not found" });

            // Check contracts
            var hasContracts = await _context.RentalContracts.AnyAsync(c => c.ClientId == id);
            if (hasContracts)
            {
                return BadRequest(new { message = "Cannot delete a client with rental history. Deactivate or log remarks instead." });
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Client deleted successfully" });
        }
    }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Backend\Controllers\ConsumableController.cs
```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ConsumableController : ControllerBase
    {
        private readonly DataContext _context;

        public ConsumableController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("configs")]
        public async Task<IActionResult> GetConfigs()
        {
            var configs = await _context.ConsumableConfigs.ToListAsync();
            return Ok(configs);
        }

        [HttpPost("configs")]
        public async Task<IActionResult> SaveConfig([FromBody] ConsumableConfig config)
        {
            var existing = await _context.ConsumableConfigs
                .FirstOrDefaultAsync(c => c.ConsumableType.ToLower() == config.ConsumableType.ToLower());

            if (existing != null)
            {
                existing.IntervalKm = config.IntervalKm;
                existing.IntervalMonths = config.IntervalMonths;
            }
            else
            {
                _context.ConsumableConfigs.Add(config);
            }

            await _context.SaveChangesAsync();
            return Ok(config);
        }

        [HttpGet("vehicle/{vehicleId}")]
        public async Task<IActionResult> GetVehicleStatus(int vehicleId)
        {
            var vehicle = await _context.Vehicles.FindAsync(vehicleId);
            if (vehicle == null) return NotFound(new { message = "Vehicle not found" });

            var configs = await _context.ConsumableConfigs.ToListAsync();
            var logs = await _context.ConsumableLogs
                .Where(l => l.VehicleId == vehicleId)
                .OrderByDescending(l => l.ReplacementDate)
                .ToListAsync();

            var globalSettings = await _context.GlobalSettings.FirstOrDefaultAsync() ?? new GlobalSettings();
            var kmAlertThreshold = globalSettings.ConsumableNotifyKmBefore;
            var daysAlertThreshold = globalSettings.ConsumableNotifyDaysBefore;

            // Predefined standard types if not in DB configs yet, let's create a list
            var standardTypes = new List<string>
            {
                "OilChange",
                "AirFilter", "OilFilter", "FuelFilter", "CabinFilter",
                "FrontBrakes", "RearBrakes",
                "FrontTires", "RearTires",
                "Battery"
            };

            var allTypes = configs.Select(c => c.ConsumableType).Union(standardTypes).Distinct();

            var statusReport = new List<object>();

            foreach (var type in allTypes)
            {
                var config = configs.FirstOrDefault(c => c.ConsumableType.ToLower() == type.ToLower());
                var typeLogs = logs.Where(l => l.ConsumableType.ToLower() == type.ToLower()).ToList();
                var lastLog = typeLogs.FirstOrDefault();

                // Get interval limits
                int intervalKm = config?.IntervalKm ?? GetDefaultIntervalKm(type);
                int intervalMonths = config?.IntervalMonths ?? GetDefaultIntervalMonths(type);

                // Last replacement details
                DateTime lastDate = lastLog?.ReplacementDate ?? vehicle.PurchaseDate;
                int lastKm = lastLog?.ReplacementKm ?? vehicle.InitialKm;

                // Calculations
                int kmSince = vehicle.CurrentKm - lastKm;
                int monthsSince = ((DateTime.UtcNow.Year - lastDate.Year) * 12) + DateTime.UtcNow.Month - lastDate.Month;

                bool isDueKm = false;
                bool isDueMonths = false;
                bool isWarningKm = false;
                bool isWarningMonths = false;

                if (intervalKm > 0)
                {
                    isDueKm = kmSince >= intervalKm;
                    isWarningKm = !isDueKm && (intervalKm - kmSince) <= kmAlertThreshold;
                }

                if (intervalMonths > 0)
                {
                    isDueMonths = monthsSince >= intervalMonths;
                    var remainingDays = (lastDate.AddMonths(intervalMonths) - DateTime.UtcNow).Days;
                    isWarningMonths = !isDueMonths && remainingDays <= daysAlertThreshold;
                }

                string status = "OK";
                if (isDueKm || isDueMonths)
                {
                    status = "Due";
                }
                else if (isWarningKm || isWarningMonths)
                {
                    status = "Warning";
                }

                statusReport.Add(new
                {
                    ConsumableType = type,
                    LastReplacementDate = lastLog?.ReplacementDate,
                    LastReplacementKm = lastLog?.ReplacementKm,
                    KmSinceReplacement = kmSince,
                    MonthsSinceReplacement = monthsSince,
                    IntervalKm = intervalKm,
                    IntervalMonths = intervalMonths,
                    Status = status,
                    LogsCount = typeLogs.Count,
                    Details = lastLog
                });
            }

            return Ok(new
            {
                vehicleId,
                currentKm = vehicle.CurrentKm,
                statusReport,
                logs
            });
        }

        [HttpPost("log")]
        public async Task<IActionResult> AddLog([FromBody] ConsumableLog log)
        {
            var vehicle = await _context.Vehicles.FindAsync(log.VehicleId);
            if (vehicle == null) return BadRequest(new { message = "Vehicle not found" });

            if (log.ReplacementKm < vehicle.InitialKm)
            {
                return BadRequest(new { message = $"Odometer replacement value cannot be less than vehicle's initial mileage ({vehicle.InitialKm} km)." });
            }

            // Update vehicle current odometer if replacement km is higher
            if (log.ReplacementKm > vehicle.CurrentKm)
            {
                vehicle.CurrentKm = log.ReplacementKm;
                var kmEntry = new KmEntry
                {
                    VehicleId = vehicle.Id,
                    Date = log.ReplacementDate,
                    KmValue = log.ReplacementKm,
                    Source = "Manual",
                    Notes = $"Odometer updated via consumable replacement log ({log.ConsumableType})."
                };
                _context.KmEntries.Add(kmEntry);
            }

            _context.ConsumableLogs.Add(log);
            await _context.SaveChangesAsync();
            return Ok(log);
        }

        [HttpDelete("log/{id}")]
        public async Task<IActionResult> DeleteLog(int id)
        {
            var log = await _context.ConsumableLogs.FindAsync(id);
            if (log == null) return NotFound(new { message = "Consumable replacement log not found" });

            _context.ConsumableLogs.Remove(log);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Replacement log deleted" });
        }

        private static int GetDefaultIntervalKm(string type)
        {
            return type.ToLower() switch
            {
                "oilchange" => 10000,
                "oilfilter" => 10000,
                "airfilter" => 20000,
                "fuelfilter" => 30000,
                "cabinfilter" => 20000,
                "frontbrakes" => 40000,
                "rearbrakes" => 60000,
                "fronttires" => 50000,
                "reartires" => 50000,
                _ => 0 // Custom / Battery doesn't use km
            };
        }

        private static int GetDefaultIntervalMonths(string type)
        {
            return type.ToLower() switch
            {
                "oilchange" => 12,
                "battery" => 36, // 3 years
                "fronttires" => 48,
                "reartires" => 48,
                _ => 0
            };
        }
    }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Backend\Controllers\ContractController.cs
```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ContractController : ControllerBase
    {
        private readonly DataContext _context;

        public ContractController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? search,
            [FromQuery] string? status,
            [FromQuery] string? paymentStatus,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _context.RentalContracts
                .Include(c => c.Client)
                .Include(c => c.Vehicle)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var lowerSearch = search.ToLower();
                query = query.Where(c => c.ContractNumber.ToLower().Contains(lowerSearch) ||
                                         (c.Client != null && c.Client.FullName.ToLower().Contains(lowerSearch)) ||
                                         (c.Vehicle != null && (c.Vehicle.Brand.ToLower().Contains(lowerSearch) || c.Vehicle.Model.ToLower().Contains(lowerSearch) || c.Vehicle.Matricule.ToLower().Contains(lowerSearch))));
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                if (Enum.TryParse<ContractStatus>(status, true, out var statusEnum))
                {
                    query = query.Where(c => c.ContractStatus == statusEnum);
                }
            }

            if (!string.IsNullOrWhiteSpace(paymentStatus))
            {
                if (Enum.TryParse<PaymentStatus>(paymentStatus, true, out var payEnum))
                {
                    query = query.Where(c => c.PaymentStatus == payEnum);
                }
            }

            var totalCount = await query.CountAsync();
            var contracts = await query
                .OrderByDescending(c => c.StartDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                totalCount,
                page,
                pageSize,
                data = contracts
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var contract = await _context.RentalContracts
                .Include(c => c.Client)
                .Include(c => c.Vehicle)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contract == null) return NotFound(new { message = "Contract not found" });
            return Ok(contract);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RentalContract contract)
        {
            // Verify vehicle status
            var vehicle = await _context.Vehicles.FindAsync(contract.VehicleId);
            if (vehicle == null) return BadRequest(new { message = "Vehicle not found" });

            if (contract.ContractStatus == ContractStatus.Active || contract.ContractStatus == ContractStatus.Draft)
            {
                if (vehicle.Status != VehicleStatus.Available && vehicle.Status != VehicleStatus.Reserved)
                {
                    return BadRequest(new { message = $"Vehicle is not available for rental. Current status: {vehicle.Status}" });
                }
            }

            // Verify client
            var client = await _context.Clients.FindAsync(contract.ClientId);
            if (client == null) return BadRequest(new { message = "Client not found" });

            // Warning if client's driver's license is expired
            bool isLicenseExpired = client.LicenseExpiryDate < DateTime.UtcNow.Date;

            // Generate unique Contract number if empty
            if (string.IsNullOrWhiteSpace(contract.ContractNumber))
            {
                var count = await _context.RentalContracts.CountAsync();
                contract.ContractNumber = $"CTR-{DateTime.UtcNow:yyyyMMdd}-{count + 1:D3}";
            }

            // Pre-fill fields
            contract.KmDeparture = vehicle.CurrentKm;
            contract.DailyRate = contract.DailyRate > 0 ? contract.DailyRate : vehicle.DailyRate;

            // Compute days and amounts
            var days = (contract.ExpectedReturnDate.Date - contract.StartDate.Date).Days;
            contract.RentalDays = days > 0 ? days : 1;
            contract.TotalAmount = contract.RentalDays * contract.DailyRate;
            contract.FinalAmountDue = contract.TotalAmount + contract.AdditionalCharges + contract.ExtrasCharges - contract.DiscountAmount;

            if (contract.PaymentStatus == PaymentStatus.Paid)
            {
                contract.AmountPaid = contract.FinalAmountDue;
            }

            if (contract.ContractStatus == ContractStatus.Active)
            {
                vehicle.Status = VehicleStatus.Rented;

                // Log automatic km entry
                var kmEntry = new KmEntry
                {
                    VehicleId = vehicle.Id,
                    Date = contract.StartDate,
                    KmValue = contract.KmDeparture,
                    Source = "ContractStart",
                    Notes = $"Odometer recorded at start of contract {contract.ContractNumber}."
                };
                _context.KmEntries.Add(kmEntry);
            }
            else if (contract.ContractStatus == ContractStatus.Draft)
            {
                vehicle.Status = VehicleStatus.Reserved;
            }

            _context.RentalContracts.Add(contract);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = contract.Id }, new { contract, warning = isLicenseExpired ? "Client's driver's license is expired!" : null });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RentalContract updated)
        {
            var contract = await _context.RentalContracts
                .Include(c => c.Vehicle)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contract == null) return NotFound(new { message = "Contract not found" });

            var vehicle = contract.Vehicle;
            if (vehicle == null) return BadRequest(new { message = "Associated vehicle not found" });

            // Handle transition states
            if (contract.ContractStatus != updated.ContractStatus)
            {
                if (updated.ContractStatus == ContractStatus.Active)
                {
                    // Transition Draft -> Active
                    vehicle.Status = VehicleStatus.Rented;
                    contract.KmDeparture = vehicle.CurrentKm;

                    var kmEntry = new KmEntry
                    {
                        VehicleId = vehicle.Id,
                        Date = DateTime.UtcNow,
                        KmValue = contract.KmDeparture,
                        Source = "ContractStart",
                        Notes = $"Odometer recorded at start of contract {contract.ContractNumber}."
                    };
                    _context.KmEntries.Add(kmEntry);
                }
                else if (updated.ContractStatus == ContractStatus.Cancelled)
                {
                    // Transition -> Cancelled
                    vehicle.Status = VehicleStatus.Available;
                }
            }

            contract.ContractType = updated.ContractType;
            contract.StartDate = updated.StartDate;
            contract.ExpectedReturnDate = updated.ExpectedReturnDate;
            contract.DailyRate = updated.DailyRate;
            contract.RentalDays = updated.RentalDays;
            contract.AdditionalCharges = updated.AdditionalCharges;
            contract.ExtrasCharges = updated.ExtrasCharges;
            contract.DiscountAmount = updated.DiscountAmount;
            contract.FinalAmountDue = (contract.RentalDays * contract.DailyRate) + contract.AdditionalCharges + contract.ExtrasCharges - contract.DiscountAmount;
            contract.PaymentStatus = updated.PaymentStatus;
            contract.PaymentMethod = updated.PaymentMethod;
            contract.AmountPaid = updated.AmountPaid;
            contract.DepositAmount = updated.DepositAmount;
            contract.DepositStatus = updated.DepositStatus;
            contract.ContractStatus = updated.ContractStatus;
            contract.Notes = updated.Notes;

            if (contract.PaymentStatus == PaymentStatus.Paid)
            {
                contract.AmountPaid = contract.FinalAmountDue;
            }

            await _context.SaveChangesAsync();
            return Ok(contract);
        }

        public class ReturnVehicleDto
        {
            public int KmReturn { get; set; }
            public DateTime ReturnDate { get; set; }
            public decimal FuelPenalty { get; set; }
            public decimal DamageFees { get; set; }
            public decimal ExtrasCharges { get; set; }
            public bool SetInMaintenance { get; set; } // If damage noted, set vehicle status to Maintenance
            public string ReturnNotes { get; set; } = string.Empty;
        }

        [HttpPost("{id}/return")]
        public async Task<IActionResult> ReturnVehicle(int id, [FromBody] ReturnVehicleDto dto)
        {
            var contract = await _context.RentalContracts
                .Include(c => c.Vehicle)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contract == null) return NotFound(new { message = "Contract not found" });
            if (contract.ContractStatus != ContractStatus.Active)
            {
                return BadRequest(new { message = "Only active contracts can be completed." });
            }

            var vehicle = contract.Vehicle;
            if (vehicle == null) return BadRequest(new { message = "Vehicle not found" });

            if (dto.KmReturn < contract.KmDeparture)
            {
                return BadRequest(new { message = $"Return km ({dto.KmReturn}) cannot be less than departure km ({contract.KmDeparture})." });
            }

            // Update contract Return details
            contract.ActualReturnDate = dto.ReturnDate;
            contract.KmReturn = dto.KmReturn;
            contract.KmDriven = dto.KmReturn - contract.KmDeparture;

            // Calculate late fee
            decimal lateFee = 0;
            if (dto.ReturnDate > contract.ExpectedReturnDate)
            {
                var lateSpan = dto.ReturnDate - contract.ExpectedReturnDate;
                var lateDays = Math.Ceiling(lateSpan.TotalDays);
                lateFee = (decimal)lateDays * contract.DailyRate;
            }

            contract.LateReturnFee = lateFee;
            contract.FuelPenalty = dto.FuelPenalty;
            contract.DamageFees = dto.DamageFees;
            contract.ExtrasCharges += dto.ExtrasCharges;

            // Recalculate total due
            contract.FinalAmountDue = contract.TotalAmount + contract.AdditionalCharges + contract.ExtrasCharges + contract.LateReturnFee + contract.FuelPenalty + contract.DamageFees - contract.DiscountAmount;
            contract.ContractStatus = ContractStatus.Completed;
            if (contract.FinalAmountDue == 0 || contract.PaymentStatus == PaymentStatus.Paid)
            {
                contract.PaymentStatus = PaymentStatus.Paid;
            }
            else
            {
                contract.PaymentStatus = PaymentStatus.PartiallyPaid; // Auto tag as partially paid if return fees added
            }

            // Update Vehicle current odometer and status
            vehicle.CurrentKm = dto.KmReturn;
            vehicle.Status = dto.SetInMaintenance ? VehicleStatus.InMaintenance : VehicleStatus.Available;

            // Log km timeline auto entry
            var kmEntry = new KmEntry
            {
                VehicleId = vehicle.Id,
                Date = dto.ReturnDate,
                KmValue = dto.KmReturn,
                Source = "ContractReturn",
                Notes = $"Odometer recorded at vehicle return for contract {contract.ContractNumber}."
            };
            _context.KmEntries.Add(kmEntry);

            await _context.SaveChangesAsync();

            return Ok(new { message = "Vehicle returned successfully", contract });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var contract = await _context.RentalContracts
                .Include(c => c.Vehicle)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contract == null) return NotFound(new { message = "Contract not found" });

            if (contract.ContractStatus == ContractStatus.Active || contract.ContractStatus == ContractStatus.Draft)
            {
                if (contract.Vehicle != null)
                {
                    contract.Vehicle.Status = VehicleStatus.Available;
                }
            }

            _context.RentalContracts.Remove(contract);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Contract deleted successfully" });
        }
    }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Backend\Controllers\FuelController.cs
```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FuelController : ControllerBase
    {
        private readonly DataContext _context;

        public FuelController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("vehicle/{vehicleId}")]
        public async Task<IActionResult> GetVehicleLogs(int vehicleId)
        {
            var logs = await _context.FuelLogs
                .Where(f => f.VehicleId == vehicleId)
                .OrderBy(f => f.KmValue)
                .ToListAsync();

            // Calculate L/100km for each log
            var result = logs.Select((log, index) =>
            {
                decimal consumption = 0;
                int kmDiff = 0;

                if (index > 0)
                {
                    var prevLog = logs[index - 1];
                    kmDiff = log.KmValue - prevLog.KmValue;
                    if (kmDiff > 0)
                    {
                        consumption = (log.Liters / kmDiff) * 100;
                    }
                }

                return new
                {
                    Log = log,
                    KmDrivenSinceLastFill = kmDiff,
                    ConsumptionL100 = Math.Round(consumption, 2)
                };
            }).OrderByDescending(r => r.Log.Date).ToList();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddLog([FromBody] FuelLog log)
        {
            var vehicle = await _context.Vehicles.FindAsync(log.VehicleId);
            if (vehicle == null) return BadRequest(new { message = "Vehicle not found" });

            if (log.KmValue < vehicle.CurrentKm)
            {
                return BadRequest(new { message = $"Odometer value ({log.KmValue} km) cannot be less than current vehicle odometer ({vehicle.CurrentKm} km)." });
            }

            log.TotalCost = log.Liters * log.CostPerLiter;

            // Get historical logs to detect anomalies
            var existingLogs = await _context.FuelLogs
                .Where(f => f.VehicleId == log.VehicleId)
                .OrderBy(f => f.KmValue)
                .ToListAsync();

            if (existingLogs.Count > 0)
            {
                // Calculate new log consumption
                var lastLog = existingLogs.Last();
                int kmDiff = log.KmValue - lastLog.KmValue;
                if (kmDiff > 0)
                {
                    decimal currentConsumption = (log.Liters / kmDiff) * 100;

                    // Calculate average consumption from past logs
                    var consumptions = new List<decimal>();
                    for (int i = 1; i < existingLogs.Count; i++)
                    {
                        var diff = existingLogs[i].KmValue - existingLogs[i - 1].KmValue;
                        if (diff > 0)
                        {
                            consumptions.Add((existingLogs[i].Liters / diff) * 100);
                        }
                    }

                    if (consumptions.Count > 0)
                    {
                        var average = consumptions.Average();
                        // Deviaiton > 30% is flagged as anomaly
                        if (currentConsumption > average * 1.3m || currentConsumption < average * 0.7m)
                        {
                            log.IsAnomaly = true;
                        }
                    }
                }
            }

            _context.FuelLogs.Add(log);

            // Update vehicle current odometer if higher
            if (log.KmValue > vehicle.CurrentKm)
            {
                vehicle.CurrentKm = log.KmValue;
                var kmEntry = new KmEntry
                {
                    VehicleId = vehicle.Id,
                    Date = log.Date,
                    KmValue = log.KmValue,
                    Source = "Manual",
                    Notes = $"Odometer updated via fuel fill-up log."
                };
                _context.KmEntries.Add(kmEntry);
            }

            await _context.SaveChangesAsync();
            return Ok(log);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLog(int id)
        {
            var log = await _context.FuelLogs.FindAsync(id);
            if (log == null) return NotFound(new { message = "Fuel log not found" });

            _context.FuelLogs.Remove(log);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Fuel log deleted" });
        }

        [HttpGet("export-csv")]
        public async Task<IActionResult> ExportCsv([FromQuery] int? vehicleId)
        {
            var query = _context.FuelLogs.Include(f => f.Vehicle).AsQueryable();
            if (vehicleId.HasValue)
            {
                query = query.Where(f => f.VehicleId == vehicleId.Value);
            }

            var logs = await query.OrderBy(f => f.KmValue).ToListAsync();

            var builder = new StringBuilder();
            builder.AppendLine("ID,Vehicle,Matricule,Date,Odometer (Km),Liters,Cost/L,Total Cost,Station,Fuel Type,Anomaly");

            for (int i = 0; i < logs.Count; i++)
            {
                var item = logs[i];
                var vehicleName = item.Vehicle != null ? $"{item.Vehicle.Brand} {item.Vehicle.Model}" : "";
                var matricule = item.Vehicle != null ? item.Vehicle.Matricule : "";
                builder.AppendLine($"{item.Id},\"{vehicleName}\",\"{matricule}\",\"{item.Date:yyyy-MM-dd HH:mm}\",{item.KmValue},{item.Liters},{item.CostPerLiter},{item.TotalCost},\"{item.StationName}\",\"{item.FuelType}\",{item.IsAnomaly}");
            }

            var csvBytes = Encoding.UTF8.GetBytes(builder.ToString());
            return File(csvBytes, "text/csv", $"Rapport_Carburant_{(vehicleId.HasValue ? $"Vehicule_{vehicleId}" : "Total")}_{DateTime.UtcNow:yyyyMMdd}.csv");
        }
    }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Backend\Controllers\InsuranceController.cs
```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InsuranceController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _env;

        public InsuranceController(DataContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet("vehicle/{vehicleId}")]
        public async Task<IActionResult> GetPoliciesByVehicle(int vehicleId)
        {
            var policies = await _context.InsurancePolicies
                .Where(ip => ip.VehicleId == vehicleId)
                .OrderByDescending(ip => ip.ExpiryDate)
                .ToListAsync();

            var current = policies.FirstOrDefault();
            var settings = await _context.GlobalSettings.FirstOrDefaultAsync() ?? new GlobalSettings();
            var thresholds = settings.InsuranceExpiryDaysThresholds.Split(',').Select(int.Parse).ToList();

            var report = policies.Select(p => {
                var daysRemaining = (p.ExpiryDate.Date - DateTime.UtcNow.Date).Days;
                string status = "Valid";
                if (daysRemaining < 0)
                {
                    status = "Expired";
                }
                else if (thresholds.Any(t => daysRemaining <= t))
                {
                    status = "ExpiringSoon";
                }

                return new
                {
                    Policy = p,
                    DaysRemaining = daysRemaining,
                    Status = status
                };
            }).ToList();

            return Ok(new { current = report.FirstOrDefault(), history = report.Skip(1).ToList(), all = report });
        }

        [HttpPost]
        public async Task<IActionResult> AddPolicy([FromBody] InsurancePolicy policy)
        {
            var vehicle = await _context.Vehicles.FindAsync(policy.VehicleId);
            if (vehicle == null) return BadRequest(new { message = "Vehicle not found" });

            _context.InsurancePolicies.Add(policy);
            await _context.SaveChangesAsync();
            return Ok(policy);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePolicy(int id, [FromBody] InsurancePolicy updated)
        {
            var policy = await _context.InsurancePolicies.FindAsync(id);
            if (policy == null) return NotFound(new { message = "Insurance policy not found" });

            policy.InsurerName = updated.InsurerName;
            policy.PolicyNumber = updated.PolicyNumber;
            policy.CoverageType = updated.CoverageType;
            policy.StartDate = updated.StartDate;
            policy.ExpiryDate = updated.ExpiryDate;
            policy.PremiumAmount = updated.PremiumAmount;
            policy.InsuredValue = updated.InsuredValue;
            policy.AgentContact = updated.AgentContact;

            if (!string.IsNullOrWhiteSpace(updated.DocumentPath))
            {
                policy.DocumentPath = updated.DocumentPath;
            }

            await _context.SaveChangesAsync();
            return Ok(policy);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePolicy(int id)
        {
            var policy = await _context.InsurancePolicies.FindAsync(id);
            if (policy == null) return NotFound(new { message = "Policy not found" });

            _context.InsurancePolicies.Remove(policy);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Insurance policy deleted" });
        }

        [HttpPost("upload-policy")]
        public async Task<IActionResult> UploadPolicy(IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest(new { message = "No file uploaded" });

            var uploadsFolder = Path.Combine(_env.ContentRootPath, "wwwroot", "uploads", "policies");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var relativePath = $"/uploads/policies/{fileName}";
            return Ok(new { documentPath = relativePath });
        }
    }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Backend\Controllers\KmController.cs
```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class KmController : ControllerBase
    {
        private readonly DataContext _context;

        public KmController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("vehicle/{vehicleId}")]
        public async Task<IActionResult> GetVehicleHistory(int vehicleId)
        {
            var history = await _context.KmEntries
                .Where(k => k.VehicleId == vehicleId)
                .OrderByDescending(k => k.Date)
                .ToListAsync();

            return Ok(history);
        }

        [HttpPost]
        public async Task<IActionResult> AddManualEntry([FromBody] KmEntry entry)
        {
            var vehicle = await _context.Vehicles.FindAsync(entry.VehicleId);
            if (vehicle == null) return BadRequest(new { message = "Vehicle not found" });

            if (entry.KmValue < vehicle.CurrentKm)
            {
                return BadRequest(new { message = $"Odometer reading ({entry.KmValue} km) cannot be less than current vehicle odometer ({vehicle.CurrentKm} km)." });
            }

            entry.Source = "Manual";
            if (entry.Date == default)
            {
                entry.Date = DateTime.UtcNow;
            }

            _context.KmEntries.Add(entry);

            // Update vehicle current odometer
            vehicle.CurrentKm = entry.KmValue;

            await _context.SaveChangesAsync();
            return Ok(entry);
        }

        [HttpGet("inactivity-report")]
        public async Task<IActionResult> GetInactivityReport()
        {
            var settings = await _context.GlobalSettings.FirstOrDefaultAsync() ?? new GlobalSettings();
            var thresholdDays = settings.KmInactivityDaysThreshold;
            var targetDate = DateTime.UtcNow.AddDays(-thresholdDays);

            var inactiveVehicles = await _context.Vehicles
                .Select(v => new
                {
                    v.Id,
                    v.Matricule,
                    v.Brand,
                    v.Model,
                    v.Status,
                    v.CurrentKm,
                    LastActivityDate = _context.KmEntries
                        .Where(ke => ke.VehicleId == v.Id)
                        .Select(ke => ke.Date)
                        .OrderByDescending(d => d)
                        .FirstOrDefault()
                })
                .ToListAsync();

            var result = inactiveVehicles
                .Select(v => new
                {
                    v.Id,
                    v.Matricule,
                    v.Brand,
                    v.Model,
                    v.Status,
                    v.CurrentKm,
                    v.LastActivityDate,
                    DaysInactive = v.LastActivityDate == default ? (DateTime.UtcNow - DateTime.UtcNow).Days : (DateTime.UtcNow - v.LastActivityDate).Days
                })
                .Where(v => v.LastActivityDate == default || v.DaysInactive > thresholdDays)
                .OrderByDescending(v => v.DaysInactive)
                .ToList();

            return Ok(new
            {
                thresholdDays,
                vehicles = result
            });
        }

        [HttpGet("export-csv")]
        public async Task<IActionResult> ExportCsv([FromQuery] int? vehicleId)
        {
            var query = _context.KmEntries.Include(k => k.Vehicle).AsQueryable();
            if (vehicleId.HasValue)
            {
                query = query.Where(k => k.VehicleId == vehicleId.Value);
            }

            var entries = await query.OrderByDescending(k => k.Date).ToListAsync();

            var builder = new StringBuilder();
            builder.AppendLine("ID,Vehicle,Matricule,Date,Odometer (Km),Source,Notes");

            foreach (var item in entries)
            {
                var vehicleName = item.Vehicle != null ? $"{item.Vehicle.Brand} {item.Vehicle.Model}" : "";
                var matricule = item.Vehicle != null ? item.Vehicle.Matricule : "";
                builder.AppendLine($"{item.Id},\"{vehicleName}\",\"{matricule}\",\"{item.Date:yyyy-MM-dd HH:mm}\",{item.KmValue},\"{item.Source}\",\"{item.Notes.Replace("\"", "\"\"")}\"");
            }

            var csvBytes = Encoding.UTF8.GetBytes(builder.ToString());
            return File(csvBytes, "text/csv", $"Suivi_Kilometrage_{(vehicleId.HasValue ? $"Vehicule_{vehicleId}" : "Total")}_{DateTime.UtcNow:yyyyMMdd}.csv");
        }
    }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Backend\Controllers\MaintenanceController.cs
```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MaintenanceController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _env;

        public MaintenanceController(DataContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? status, [FromQuery] int? vehicleId)
        {
            var query = _context.Maintenances
                .Include(m => m.Vehicle)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
            {
                if (Enum.TryParse<MaintenanceStatus>(status, true, out var statusEnum))
                {
                    query = query.Where(m => m.Status == statusEnum);
                }
            }

            if (vehicleId.HasValue)
            {
                query = query.Where(m => m.VehicleId == vehicleId.Value);
            }

            var list = await query.OrderByDescending(m => m.DatePerformed).ToListAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var maintenance = await _context.Maintenances
                .Include(m => m.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (maintenance == null) return NotFound(new { message = "Maintenance not found" });
            return Ok(maintenance);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Maintenance maintenance)
        {
            var vehicle = await _context.Vehicles.FindAsync(maintenance.VehicleId);
            if (vehicle == null) return BadRequest(new { message = "Vehicle not found" });

            maintenance.TotalCost = maintenance.LaborCost + maintenance.PartsCost;

            if (maintenance.Status == MaintenanceStatus.InProgress)
            {
                vehicle.Status = VehicleStatus.InMaintenance;
            }

            _context.Maintenances.Add(maintenance);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = maintenance.Id }, maintenance);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Maintenance updated)
        {
            var maintenance = await _context.Maintenances.FindAsync(id);
            if (maintenance == null) return NotFound(new { message = "Maintenance record not found" });

            var vehicle = await _context.Vehicles.FindAsync(maintenance.VehicleId);
            if (vehicle == null) return BadRequest(new { message = "Vehicle not found" });

            // Handle transition states
            if (maintenance.Status != updated.Status)
            {
                if (updated.Status == MaintenanceStatus.InProgress)
                {
                    vehicle.Status = VehicleStatus.InMaintenance;
                }
                else if (updated.Status == MaintenanceStatus.Completed)
                {
                    vehicle.Status = VehicleStatus.Available;
                }
                else if (updated.Status == MaintenanceStatus.Scheduled && maintenance.Status == MaintenanceStatus.InProgress)
                {
                    vehicle.Status = VehicleStatus.Available;
                }
            }

            maintenance.MaintenanceType = updated.MaintenanceType;
            maintenance.DatePerformed = updated.DatePerformed;
            maintenance.NextScheduledDate = updated.NextScheduledDate;
            maintenance.KmAtMaintenance = updated.KmAtMaintenance;
            maintenance.WorkshopName = updated.WorkshopName;
            maintenance.WorkshopAddress = updated.WorkshopAddress;
            maintenance.WorkshopContact = updated.WorkshopContact;
            maintenance.Description = updated.Description;
            maintenance.LaborCost = updated.LaborCost;
            maintenance.PartsCost = updated.PartsCost;
            maintenance.TotalCost = updated.LaborCost + updated.PartsCost;
            maintenance.InvoiceNumber = updated.InvoiceNumber;
            maintenance.Status = updated.Status;

            if (!string.IsNullOrWhiteSpace(updated.InvoiceFilePath))
            {
                maintenance.InvoiceFilePath = updated.InvoiceFilePath;
            }

            await _context.SaveChangesAsync();
            return Ok(maintenance);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var maintenance = await _context.Maintenances.FindAsync(id);
            if (maintenance == null) return NotFound(new { message = "Maintenance record not found" });

            var vehicle = await _context.Vehicles.FindAsync(maintenance.VehicleId);
            if (vehicle != null && maintenance.Status == MaintenanceStatus.InProgress)
            {
                vehicle.Status = VehicleStatus.Available; // Return vehicle back if servicing cancelled
            }

            _context.Maintenances.Remove(maintenance);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Maintenance record deleted" });
        }

        [HttpGet("calendar")]
        public async Task<IActionResult> GetCalendar()
        {
            // Get all upcoming scheduled events
            var events = await _context.Maintenances
                .Include(m => m.Vehicle)
                .Where(m => m.Status == MaintenanceStatus.Scheduled || (m.NextScheduledDate != null && m.NextScheduledDate >= DateTime.UtcNow.AddMonths(-1)))
                .Select(m => new
                {
                    m.Id,
                    Title = $"{m.MaintenanceType} - {(m.Vehicle != null ? m.Vehicle.Brand + " " + m.Vehicle.Model : "Unknown Vehicle")}",
                    Start = m.Status == MaintenanceStatus.Scheduled ? m.DatePerformed : m.NextScheduledDate,
                    End = m.Status == MaintenanceStatus.Scheduled ? m.DatePerformed.AddHours(2) : m.NextScheduledDate.Value.AddHours(2),
                    m.Status,
                    m.MaintenanceType,
                    VehicleMatricule = m.Vehicle != null ? m.Vehicle.Matricule : ""
                })
                .ToListAsync();

            return Ok(events);
        }

        [HttpPost("upload-invoice")]
        public async Task<IActionResult> UploadInvoice(IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest(new { message = "No file uploaded" });

            var uploadsFolder = Path.Combine(_env.ContentRootPath, "wwwroot", "uploads", "invoices");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var relativePath = $"/uploads/invoices/{fileName}";
            return Ok(new { invoiceFilePath = relativePath });
        }
    }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Backend\Controllers\ReportController.cs
```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly DataContext _context;

        public ReportController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("fleet-status")]
        public async Task<IActionResult> GetFleetStatus()
        {
            var total = await _context.Vehicles.CountAsync();
            var available = await _context.Vehicles.CountAsync(v => v.Status == VehicleStatus.Available);
            var rented = await _context.Vehicles.CountAsync(v => v.Status == VehicleStatus.Rented);
            var inMaintenance = await _context.Vehicles.CountAsync(v => v.Status == VehicleStatus.InMaintenance);
            var immobilized = await _context.Vehicles.CountAsync(v => v.Status == VehicleStatus.Immobilized);
            var reserved = await _context.Vehicles.CountAsync(v => v.Status == VehicleStatus.Reserved);

            return Ok(new
            {
                Total = total,
                Available = available,
                Rented = rented,
                InMaintenance = inMaintenance,
                Immobilized = immobilized,
                Reserved = reserved
            });
        }

        [HttpGet("revenue")]
        public async Task<IActionResult> GetRevenueReport([FromQuery] DateTime? start, [FromQuery] DateTime? end)
        {
            var query = _context.RentalContracts.AsQueryable();

            if (start.HasValue)
            {
                query = query.Where(c => c.StartDate >= start.Value);
            }
            if (end.HasValue)
            {
                query = query.Where(c => c.StartDate <= end.Value);
            }

            var contracts = await query.ToListAsync();

            var totalRevenue = contracts.Sum(c => c.FinalAmountDue);
            var paidRevenue = contracts.Where(c => c.PaymentStatus == PaymentStatus.Paid).Sum(c => c.FinalAmountDue);
            var unpaidRevenue = totalRevenue - paidRevenue;

            var revenueByVehicle = contracts
                .GroupBy(c => c.VehicleId)
                .Select(g => new
                {
                    VehicleId = g.Key,
                    Amount = g.Sum(c => c.FinalAmountDue)
                }).ToList();

            var result = new List<object>();
            foreach (var item in revenueByVehicle)
            {
                var vehicle = await _context.Vehicles.FindAsync(item.VehicleId);
                result.Add(new
                {
                    VehicleId = item.VehicleId,
                    Brand = vehicle?.Brand ?? "Unknown",
                    Model = vehicle?.Model ?? "",
                    Matricule = vehicle?.Matricule ?? "",
                    Amount = item.Amount
                });
            }

            return Ok(new
            {
                TotalRevenue = totalRevenue,
                PaidRevenue = paidRevenue,
                UnpaidRevenue = unpaidRevenue,
                ByVehicle = result
            });
        }

        [HttpGet("profitability")]
        public async Task<IActionResult> GetProfitabilityReport()
        {
            var vehicles = await _context.Vehicles.ToListAsync();
            var contracts = await _context.RentalContracts.ToListAsync();
            var maintenances = await _context.Maintenances.ToListAsync();
            var fuelLogs = await _context.FuelLogs.ToListAsync();
            var insurances = await _context.InsurancePolicies.ToListAsync();

            var report = vehicles.Select(v =>
            {
                var revenue = contracts.Where(c => c.VehicleId == v.Id).Sum(c => c.FinalAmountDue);
                var maintCost = maintenances.Where(m => m.VehicleId == v.Id).Sum(m => m.TotalCost);
                var fuelCost = fuelLogs.Where(f => f.VehicleId == v.Id).Sum(f => f.TotalCost);
                var insCost = insurances.Where(i => i.VehicleId == v.Id).Sum(i => i.PremiumAmount);

                var totalCost = maintCost + fuelCost + insCost;
                var netProfit = revenue - totalCost;

                // Utilization rate calculation
                var vContracts = contracts.Where(c => c.VehicleId == v.Id && c.ContractStatus != ContractStatus.Cancelled).ToList();
                int rentalDays = vContracts.Sum(c => c.RentalDays);
                int daysSincePurchase = (DateTime.UtcNow.Date - v.PurchaseDate.Date).Days;
                if (daysSincePurchase <= 0) daysSincePurchase = 1;
                decimal utilization = ((decimal)rentalDays / daysSincePurchase) * 100;
                if (utilization > 100) utilization = 100;

                return new
                {
                    v.Id,
                    v.Brand,
                    v.Model,
                    v.Matricule,
                    Revenue = revenue,
                    MaintenanceCost = maintCost,
                    FuelCost = fuelCost,
                    InsuranceCost = insCost,
                    TotalCost = totalCost,
                    Profitability = netProfit,
                    UtilizationRate = Math.Round(utilization, 2)
                };
            }).OrderByDescending(r => r.Profitability).ToList();

            return Ok(report);
        }

        [HttpGet("top-clients")]
        public async Task<IActionResult> GetTopClients()
        {
            var contracts = await _context.RentalContracts
                .Include(c => c.Client)
                .ToListAsync();

            var topClients = contracts
                .Where(c => c.Client != null)
                .GroupBy(c => c.ClientId)
                .Select(g => new
                {
                    ClientId = g.Key,
                    Name = g.First().Client!.FullName,
                    Phone = g.First().Client!.Phone,
                    RentalsCount = g.Count(),
                    TotalRevenue = g.Sum(c => c.FinalAmountDue)
                })
                .OrderByDescending(c => c.TotalRevenue)
                .Take(10)
                .ToList();

            return Ok(topClients);
        }

        [HttpGet("unpaid-contracts")]
        public async Task<IActionResult> GetUnpaidContracts()
        {
            var list = await _context.RentalContracts
                .Include(c => c.Client)
                .Include(c => c.Vehicle)
                .Where(c => c.PaymentStatus != PaymentStatus.Paid && c.ContractStatus != ContractStatus.Cancelled)
                .ToListAsync();

            var sortedList = list.OrderByDescending(c => c.FinalAmountDue).ToList();

            return Ok(sortedList);
        }

        [HttpGet("export-profitability-csv")]
        public async Task<IActionResult> ExportProfitabilityCsv()
        {
            var vehicles = await _context.Vehicles.ToListAsync();
            var contracts = await _context.RentalContracts.ToListAsync();
            var maintenances = await _context.Maintenances.ToListAsync();
            var fuelLogs = await _context.FuelLogs.ToListAsync();
            var insurances = await _context.InsurancePolicies.ToListAsync();

            var builder = new StringBuilder();
            builder.AppendLine("Matricule,Vehicle,Utilization Rate %,Revenue,Maintenance Cost,Fuel Cost,Insurance Cost,Total Cost,Net Profit");

            foreach (var v in vehicles)
            {
                var revenue = contracts.Where(c => c.VehicleId == v.Id).Sum(c => c.FinalAmountDue);
                var maintCost = maintenances.Where(m => m.VehicleId == v.Id).Sum(m => m.TotalCost);
                var fuelCost = fuelLogs.Where(f => f.VehicleId == v.Id).Sum(f => f.TotalCost);
                var insCost = insurances.Where(i => i.VehicleId == v.Id).Sum(i => i.PremiumAmount);
                var totalCost = maintCost + fuelCost + insCost;
                var netProfit = revenue - totalCost;

                var vContracts = contracts.Where(c => c.VehicleId == v.Id && c.ContractStatus != ContractStatus.Cancelled).ToList();
                int rentalDays = vContracts.Sum(c => c.RentalDays);
                int daysSincePurchase = (DateTime.UtcNow.Date - v.PurchaseDate.Date).Days;
                if (daysSincePurchase <= 0) daysSincePurchase = 1;
                decimal utilization = ((decimal)rentalDays / daysSincePurchase) * 100;
                if (utilization > 100) utilization = 100;

                builder.AppendLine($"\"{v.Matricule}\",\"{v.Brand} {v.Model}\",{utilization:F2},{revenue:F2},{maintCost:F2},{fuelCost:F2},{insCost:F2},{totalCost:F2},{netProfit:F2}");
            }

            var csvBytes = Encoding.UTF8.GetBytes(builder.ToString());
            return File(csvBytes, "text/csv", $"Rapport_Rentabilite_Flotte_{DateTime.UtcNow:yyyyMMdd}.csv");
        }
    }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Backend\Controllers\SettingsController.cs
```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly DataContext _context;

        public SettingsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetSettings()
        {
            var settings = await _context.GlobalSettings.FirstOrDefaultAsync();
            if (settings == null)
            {
                settings = new GlobalSettings();
                _context.GlobalSettings.Add(settings);
                await _context.SaveChangesAsync();
            }

            return Ok(settings);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSettings([FromBody] GlobalSettings updated)
        {
            var settings = await _context.GlobalSettings.FirstOrDefaultAsync();
            if (settings == null)
            {
                settings = new GlobalSettings();
                _context.GlobalSettings.Add(settings);
            }

            settings.CurrencySymbol = updated.CurrencySymbol;
            settings.DateFormat = updated.DateFormat;
            settings.KmInactivityDaysThreshold = updated.KmInactivityDaysThreshold;
            settings.InsuranceExpiryDaysThresholds = updated.InsuranceExpiryDaysThresholds;
            settings.InspectionExpiryDaysThresholds = updated.InspectionExpiryDaysThresholds;
            settings.MaintenanceNotifyDaysBefore = updated.MaintenanceNotifyDaysBefore;
            settings.ConsumableNotifyKmBefore = updated.ConsumableNotifyKmBefore;
            settings.ConsumableNotifyDaysBefore = updated.ConsumableNotifyDaysBefore;
            settings.VehicleTypesJson = updated.VehicleTypesJson;
            settings.FuelTypesJson = updated.FuelTypesJson;
            settings.MaintenanceTypesJson = updated.MaintenanceTypesJson;
            settings.ExtrasJson = updated.ExtrasJson;

            await _context.SaveChangesAsync();
            return Ok(settings);
        }
    }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Backend\Controllers\TechnicalInspectionController.cs
```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TechnicalInspectionController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _env;

        public TechnicalInspectionController(DataContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet("vehicle/{vehicleId}")]
        public async Task<IActionResult> GetInspectionsByVehicle(int vehicleId)
        {
            var inspections = await _context.TechnicalInspections
                .Where(ti => ti.VehicleId == vehicleId)
                .OrderByDescending(ti => ti.ExpiryDate)
                .ToListAsync();

            var current = inspections.FirstOrDefault();
            var settings = await _context.GlobalSettings.FirstOrDefaultAsync() ?? new GlobalSettings();
            var thresholds = settings.InspectionExpiryDaysThresholds.Split(',').Select(int.Parse).ToList();

            var report = inspections.Select(i => {
                var daysRemaining = (i.ExpiryDate.Date - DateTime.UtcNow.Date).Days;
                string status = "Valid";
                if (i.Result.ToLower() == "fail")
                {
                    status = "Failed";
                }
                else if (daysRemaining < 0)
                {
                    status = "Expired";
                }
                else if (thresholds.Any(t => daysRemaining <= t))
                {
                    status = "ExpiringSoon";
                }

                return new
                {
                    Inspection = i,
                    DaysRemaining = daysRemaining,
                    Status = status
                };
            }).ToList();

            return Ok(new { current = report.FirstOrDefault(), history = report.Skip(1).ToList(), all = report });
        }

        [HttpPost]
        public async Task<IActionResult> AddInspection([FromBody] TechnicalInspection inspection)
        {
            var vehicle = await _context.Vehicles.FindAsync(inspection.VehicleId);
            if (vehicle == null) return BadRequest(new { message = "Vehicle not found" });

            _context.TechnicalInspections.Add(inspection);
            await _context.SaveChangesAsync();
            return Ok(inspection);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInspection(int id, [FromBody] TechnicalInspection updated)
        {
            var inspection = await _context.TechnicalInspections.FindAsync(id);
            if (inspection == null) return NotFound(new { message = "Technical inspection not found" });

            inspection.InspectionDate = updated.InspectionDate;
            inspection.ExpiryDate = updated.ExpiryDate;
            inspection.Result = updated.Result;
            inspection.CenterName = updated.CenterName;
            inspection.CenterAddress = updated.CenterAddress;
            inspection.Cost = updated.Cost;
            inspection.Remarks = updated.Remarks;

            if (!string.IsNullOrWhiteSpace(updated.DocumentPath))
            {
                inspection.DocumentPath = updated.DocumentPath;
            }

            await _context.SaveChangesAsync();
            return Ok(inspection);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInspection(int id)
        {
            var inspection = await _context.TechnicalInspections.FindAsync(id);
            if (inspection == null) return NotFound(new { message = "Inspection not found" });

            _context.TechnicalInspections.Remove(inspection);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Technical inspection record deleted" });
        }

        [HttpPost("upload-inspection")]
        public async Task<IActionResult> UploadInspection(IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest(new { message = "No file uploaded" });

            var uploadsFolder = Path.Combine(_env.ContentRootPath, "wwwroot", "uploads", "inspections");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var relativePath = $"/uploads/inspections/{fileName}";
            return Ok(new { documentPath = relativePath });
        }
    }
}

```

---
### C:\Users\Djawed\Desktop\Parc Auto\Backend\Controllers\VehicleController.cs
```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _env;

        public VehicleController(DataContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? search,
            [FromQuery] string? type,
            [FromQuery] string? fuelType,
            [FromQuery] string? status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _context.Vehicles.AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                var lowerSearch = search.ToLower();
                query = query.Where(v => v.Brand.ToLower().Contains(lowerSearch) ||
                                         v.Model.ToLower().Contains(lowerSearch) ||
                                         v.Matricule.ToLower().Contains(lowerSearch) ||
                                         v.VIN.ToLower().Contains(lowerSearch));
            }

            // Filters
            if (!string.IsNullOrWhiteSpace(type))
            {
                query = query.Where(v => v.Type.ToLower() == type.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(fuelType))
            {
                query = query.Where(v => v.FuelType.ToLower() == fuelType.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                if (Enum.TryParse<VehicleStatus>(status, true, out var statusEnum))
                {
                    query = query.Where(v => v.Status == statusEnum);
                }
            }

            var totalCount = await query.CountAsync();
            var vehicles = await query
                .OrderByDescending(v => v.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                totalCount,
                page,
                pageSize,
                data = vehicles
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null) return NotFound(new { message = "Vehicle not found" });
            return Ok(vehicle);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Vehicle vehicle)
        {
            // Verify matricule uniqueness
            if (await _context.Vehicles.AnyAsync(v => v.Matricule == vehicle.Matricule))
            {
                return BadRequest(new { message = $"Matricule '{vehicle.Matricule}' is already registered." });
            }

            // Verify VIN uniqueness
            if (await _context.Vehicles.AnyAsync(v => v.VIN == vehicle.VIN))
            {
                return BadRequest(new { message = $"VIN '{vehicle.VIN}' is already registered." });
            }

            // Set current km to initial km initially
            vehicle.CurrentKm = vehicle.InitialKm;

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            // Auto-log initial km entry
            var kmEntry = new KmEntry
            {
                VehicleId = vehicle.Id,
                Date = vehicle.PurchaseDate,
                KmValue = vehicle.InitialKm,
                Source = "Manual",
                Notes = "Initial kilomÃ©trage recorded at registration."
            };
            _context.KmEntries.Add(kmEntry);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, vehicle);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Vehicle updated)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null) return NotFound(new { message = "Vehicle not found" });

            // Verify matricule uniqueness
            if (updated.Matricule != vehicle.Matricule && await _context.Vehicles.AnyAsync(v => v.Matricule == updated.Matricule))
            {
                return BadRequest(new { message = $"Matricule '{updated.Matricule}' is already registered." });
            }

            // Verify VIN uniqueness
            if (updated.VIN != vehicle.VIN && await _context.Vehicles.AnyAsync(v => v.VIN == updated.VIN))
            {
                return BadRequest(new { message = $"VIN '{updated.VIN}' is already registered." });
            }

            vehicle.Brand = updated.Brand;
            vehicle.Model = updated.Model;
            vehicle.Matricule = updated.Matricule;
            vehicle.Year = updated.Year;
            vehicle.Type = updated.Type;
            vehicle.FuelType = updated.FuelType;
            vehicle.Transmission = updated.Transmission;
            vehicle.VIN = updated.VIN;
            vehicle.EngineNumber = updated.EngineNumber;
            vehicle.Color = updated.Color;
            vehicle.SeatsCount = updated.SeatsCount;
            vehicle.DailyRate = updated.DailyRate;
            vehicle.Status = updated.Status;
            vehicle.PurchaseDate = updated.PurchaseDate;
            vehicle.PurchasePrice = updated.PurchasePrice;
            vehicle.Notes = updated.Notes;

            if (!string.IsNullOrWhiteSpace(updated.PhotoPath))
            {
                vehicle.PhotoPath = updated.PhotoPath;
            }

            await _context.SaveChangesAsync();
            return Ok(vehicle);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null) return NotFound(new { message = "Vehicle not found" });

            // Check if rented or has active contracts
            var hasActiveContract = await _context.RentalContracts
                .AnyAsync(c => c.VehicleId == id && (c.ContractStatus == ContractStatus.Active || c.ContractStatus == ContractStatus.Draft));

            if (hasActiveContract)
            {
                return BadRequest(new { message = "Cannot delete a vehicle with active or draft contracts." });
            }

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Vehicle deleted successfully" });
        }

        [HttpPost("upload-photo")]
        public async Task<IActionResult> UploadPhoto(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "No file uploaded." });
            }

            var uploadsFolder = Path.Combine(_env.ContentRootPath, "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var relativePath = $"/uploads/{fileName}";
            return Ok(new { photoPath = relativePath });
        }
    }
}

```


