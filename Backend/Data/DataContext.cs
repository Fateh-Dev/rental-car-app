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
