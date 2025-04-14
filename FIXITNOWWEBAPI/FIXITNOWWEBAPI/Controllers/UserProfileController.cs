using FIXITNOWWEBAPI.DTOs;
using FIXITNOWWEBAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIXITNOWWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly FixItNowDbContext _context;

        public UserProfileController(FixItNowDbContext context)
        {
            _context = context;
        }
        // ✅ GET: api/UserProfile/byuserid/{userId}
        [HttpGet("byuserid/{userId}")]
        public async Task<ActionResult<UserProfile>> GetProfileByUserId(int userId)
        {
            var profile = await _context.UserProfiles
                .Where(p => p.UserID == userId)
                .OrderByDescending(p => p.UserProfileId) // get latest
                .FirstOrDefaultAsync();

            if (profile == null)
                return NotFound();

            return Ok(profile);
        }


        // ✅ POST: api/UserProfile
        [HttpPost]
        public async Task<IActionResult> PostUserProfile([FromBody] UserProfileDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userProfile = new UserProfile
            {
                PhoneNumber = dto.PhoneNumber,
                PreferredServices = dto.PreferredServices,
                ProfilePicture = dto.ProfilePicture, 
                IdCard = dto.IdCard,                 // Base64 string
                UserID = dto.UserID
            };

            _context.UserProfiles.Add(userProfile);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Profile saved successfully!" });
        }
    }

}
