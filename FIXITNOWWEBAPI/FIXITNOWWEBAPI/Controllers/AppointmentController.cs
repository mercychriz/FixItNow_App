using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIXITNOWWEBAPI.Models;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly FixItNowDbContext _context;

    public AppointmentsController(FixItNowDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> BookAppointment([FromBody] Appointment appointment)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return Ok(appointment);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal error: {ex.Message}");
        }
    }


    [HttpGet("provider/{providerId}")]
    public async Task<IActionResult> GetAppointmentsByProvider(int providerId)
    {
        var appointments = await _context.Appointments
            .Where(a => a.ServiceProviderId == providerId)
            .ToListAsync();

        return Ok(appointments);
    }
}
