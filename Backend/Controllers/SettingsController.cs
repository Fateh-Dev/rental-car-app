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
