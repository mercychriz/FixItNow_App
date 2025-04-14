using FIXITNOWWEBAPI.DTOs;
using FIXITNOWWEBAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyServiceProvider = FIXITNOWWEBAPI.Models.ServiceProvider;

namespace FIXITNOWWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceProviderController : ControllerBase
    {
        private readonly FixItNowDbContext _context;

        public ServiceProviderController(FixItNowDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/ServiceProvider
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceProviderDTO>>> GetServiceProviders()
        {
            var serviceProviders = await _context.ServiceProviders.ToListAsync();
            return serviceProviders.Select(sp => MapToDTO(sp)).ToList();
        }

        // ✅ GET: api/ServiceProvider/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceProviderDTO>> GetServiceProvider(int id)
        {
            var serviceProvider = await _context.ServiceProviders.FindAsync(id);

            if (serviceProvider == null)
                return NotFound();

            return MapToDTO(serviceProvider);
        }

        // ✅ POST: api/ServiceProvider/Create
        [HttpPost("CreateServiceProvider")]
        public async Task<ActionResult<ServiceProviderDTO>> PostServiceProvider(ServiceProviderDTO dto)
        {
            var userId = dto.UserID;

            if (string.IsNullOrEmpty(userId))
                return BadRequest("UserId is required.");

            var exists = await _context.ServiceProviders.AnyAsync(sp => sp.UserID == userId);

            if (exists)
                return Conflict("A profile for this user already exists.");

            var entity = MapToEntity(dto, userId);

            _context.ServiceProviders.Add(entity);
            await _context.SaveChangesAsync();

            var resultDto = MapToDTO(entity);

            return CreatedAtAction(nameof(GetServiceProvider), new { id = entity.ServiceProviderId }, resultDto);
        }

        // ✅ PUT: api/ServiceProvider/Update/{id}
        [HttpPut("UpdateServiceProvider/{id}")]
        public async Task<IActionResult> PutServiceProvider(int id, ServiceProviderDTO dto)
        {
            if (id != dto.ServiceProviderId)
                return BadRequest("ID mismatch.");

            var existingProvider = await _context.ServiceProviders.FindAsync(id);
            if (existingProvider == null)
                return NotFound();

            // Update fields
            existingProvider.FullName = dto.FullName;
            existingProvider.PhoneNumber = dto.PhoneNumber;
            existingProvider.BusinessName = dto.BusinessName;
            existingProvider.ServicesOffered = dto.ServicesOffered;
            existingProvider.Location = dto.Location;
            existingProvider.Price = dto.Price;
            existingProvider.Bio = dto.Bio;
            existingProvider.ProfileImagePath = dto.ProfileImagePath;
            existingProvider.IdCardImagePath = dto.IdCardImagePath;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ✅ DELETE: api/ServiceProvider/Delete/{id}
        [HttpDelete("DeleteServiceProvider/{id}")]
        public async Task<IActionResult> DeleteServiceProvider(int id)
        {
            var serviceProvider = await _context.ServiceProviders.FindAsync(id);
            if (serviceProvider == null)
                return NotFound();

            _context.ServiceProviders.Remove(serviceProvider);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ✅ GET: api/ServiceProvider/HasProfile/{userId}
        [HttpGet("HasProfile/{userId}")]
        public async Task<IActionResult> HasProfile(string userId)
        {
            var exists = await _context.ServiceProviders.AnyAsync(sp => sp.UserID == userId);
            return Ok(new { HasProfile = exists });
        }

        // ✅ GET: api/ServiceProvider/UserProfile/{userId}
        [HttpGet("UserProfile/{userId}")]
        public async Task<ActionResult<ServiceProviderDTO>> GetServiceProviderProfileByUserId(string userId)
        {
            var serviceProvider = await _context.ServiceProviders
                .FirstOrDefaultAsync(sp => sp.UserID == userId);

            if (serviceProvider == null)
                return NotFound(new { Message = $"No profile found for userId {userId}" });

            return Ok(MapToDTO(serviceProvider));
        }

        // ✅ GET: api/ServiceProvider/Search?keyword=someKeyword
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<ServiceProviderDTO>>> SearchServiceProviders([FromQuery] string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return BadRequest("Search keyword is required.");

            var serviceProviders = await _context.ServiceProviders
                .Where(sp => sp.BusinessName.Contains(keyword) ||
                             sp.ServicesOffered.Contains(keyword) ||
                             sp.FullName.Contains(keyword))
                .ToListAsync();

            var result = serviceProviders.Select(sp => MapToDTO(sp)).ToList();

            return Ok(result);
        }

        // ✅ GET: api/ServiceProvider/ByProvider/{providerId}
        [HttpGet("ByProvider/{serviceProviderId}")]
        public async Task<ActionResult<IEnumerable<Service>>> GetServicesByProvider(int serviceProviderId)
        {
            var services = await _context.Services
                .Where(s => s.ServiceProviderId == serviceProviderId)
                .ToListAsync();

            return Ok(services);
        }

        // ✅ POST: api/ServiceProvider/CreateService
        [HttpPost("CreateService")]
        public async Task<ActionResult<Service>> PostService(Service service)
        {
            if (service.ServiceProviderId == 0)
                return BadRequest("ServiceProviderId is required.");

            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetServicesByProvider), new { providerId = service.ServiceProviderId }, service);
        }

        // ✅ PUT: api/ServiceProvider/UpdateService/{id}
        [HttpPut("UpdateService/{id}")]
        public async Task<IActionResult> PutService(int id, Service service)
        {
            if (id != service.ServiceId)
                return BadRequest("Service ID mismatch.");

            _context.Entry(service).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // ✅ DELETE: api/ServiceProvider/DeleteService/{id}
        [HttpDelete("DeleteService/{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
                return NotFound();

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ✅ Private Helper Methods
        private ServiceProviderDTO MapToDTO(MyServiceProvider serviceProvider)
        {
            return new ServiceProviderDTO
            {
                ServiceProviderId = serviceProvider.ServiceProviderId,
                UserID = serviceProvider.UserID,
                FullName = serviceProvider.FullName,
                PhoneNumber = serviceProvider.PhoneNumber,
                BusinessName = serviceProvider.BusinessName,
                ServicesOffered = serviceProvider.ServicesOffered,
                Location = serviceProvider.Location,
                Price = serviceProvider.Price,
                Bio = serviceProvider.Bio,
                ProfileImagePath = serviceProvider.ProfileImagePath,
                IdCardImagePath = serviceProvider.IdCardImagePath
            };
        }

        private MyServiceProvider MapToEntity(ServiceProviderDTO dto, string userId)
        {
            return new MyServiceProvider
            {
                UserID = userId,
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                BusinessName = dto.BusinessName,
                ServicesOffered = dto.ServicesOffered,
                Location = dto.Location,
                Price = dto.Price,
                Bio = dto.Bio,
                ProfileImagePath = dto.ProfileImagePath,
                IdCardImagePath = dto.IdCardImagePath
            };
        }

        private bool ServiceProviderExists(int id)
        {
            return _context.ServiceProviders.Any(e => e.ServiceProviderId == id);
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.ServiceId == id);
        }
    }
}
