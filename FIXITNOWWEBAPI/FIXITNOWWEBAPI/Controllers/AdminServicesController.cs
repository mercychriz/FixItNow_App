using FIXITNOWWEBAPI.DTOs;
using FIXITNOWWEBAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AdminServicesController : ControllerBase
{
    private readonly FixItNowDbContext _context;

    public AdminServicesController(FixItNowDbContext context)
    {
        _context = context;
    }

    // ✅ GET ALL SERVICES
    [HttpGet]
    public async Task<IActionResult> GetServices()
    {
        var services = await _context.Services.ToListAsync();
        return Ok(services);
    }

    // ✅ ADD SERVICE
    [HttpPost]
    public async Task<IActionResult> AddService([FromBody] ServiceDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var providerExists = await _context.ServiceProviders
            .AnyAsync(p => p.ServiceProviderId == dto.ServiceProviderId);

        if (!providerExists)
            return BadRequest(new { Message = $"ServiceProvider with ID {dto.ServiceProviderId} not found." });

        var service = new Service
        {
            ServiceName = dto.ServiceName,
            ServiceDescription = dto.ServiceDescription,
            Price = dto.Price,
            ServiceProviderId = dto.ServiceProviderId,
            ServiceProvider = null  // Always nullify navigation property!
        };

        _context.Services.Add(service);
        await _context.SaveChangesAsync();

        return Ok(service);
    }
    // ✅ GET service by ID (needed for edit page)
    // GET: api/AdminServices/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetServiceById(int id)
    {
        var service = await _context.Services.FindAsync(id);
        if (service == null)
            return NotFound();

        var dto = new ServiceResponseDto
        {
            ServiceId = service.ServiceId,
            ServiceName = service.ServiceName,
            ServiceDescription = service.ServiceDescription,
            Price = service.Price,
            ServiceProviderId = service.ServiceProviderId
        };

        return Ok(dto);
    }


    // PUT: api/AdminServices/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateService(int id, [FromBody] ServiceDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var service = await _context.Services.FindAsync(id);
        if (service == null)
            return NotFound();

        service.ServiceName = dto.ServiceName;
        service.ServiceDescription = dto.ServiceDescription;
        service.Price = dto.Price;
        service.ServiceProviderId = dto.ServiceProviderId;

        await _context.SaveChangesAsync();

        return Ok(service);
    }




    // ✅ DELETE SERVICE BY ID
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteService(int id)
    {
        var service = await _context.Services.FindAsync(id);
        if (service == null)
        {
            return NotFound($"Service with ID {id} not found.");
        }

        _context.Services.Remove(service);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // ✅ BULK DELETE BY ID LIST
    [HttpPost("BulkDelete")]
    public async Task<IActionResult> BulkDelete([FromBody] List<int> ids)
    {
        if (ids == null || !ids.Any())
        {
            return BadRequest("No service IDs provided for deletion.");
        }

        var services = await _context.Services
            .Where(s => ids.Contains(s.ServiceId))
            .ToListAsync();

        if (!services.Any())
        {
            return NotFound("No services found matching the provided IDs.");
        }

        _context.Services.RemoveRange(services);
        await _context.SaveChangesAsync();

        return Ok(new { DeletedCount = services.Count });
    }

    // ✅ DOWNLOAD CSV REPORT
    [HttpGet("Download")]
    public async Task<IActionResult> DownloadReport()
    {
        var services = await _context.Services.ToListAsync();

        var csvBuilder = new StringBuilder();
        csvBuilder.AppendLine("ServiceId,ServiceName,ServiceDescription,Price");

        foreach (var s in services)
        {
            csvBuilder.AppendLine($"{s.ServiceId},{s.ServiceName},{s.ServiceDescription},{s.Price}");
        }

        var bytes = Encoding.UTF8.GetBytes(csvBuilder.ToString());

        return File(bytes, "text/csv", "services_report.csv");
    }
}
