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
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                (path.StartsWithSegments("/api/km/export-csv") ||
                 path.StartsWithSegments("/api/report/export-profitability-csv")))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
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
