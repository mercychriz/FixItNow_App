using FIXITNOWWEBAPI.Dtos;
using FIXITNOWWEBAPI.DTOs;
using FIXITNOWWEBAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FIXITNOWWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly FixItNowDbContext _context;

        public UserController(FixItNowDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/User (Get all users)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }
        private async Task SendPasswordResetEmail(string email, string resetLink)
        {
            var fromAddress = new System.Net.Mail.MailAddress("your-email@gmail.com", "FixItNow Support");
            var toAddress = new System.Net.Mail.MailAddress(email);
            const string fromPassword = "YourEmailPassword";

            const string subject = "Password Reset Request";
            string body = $"Click the link to reset your password: {resetLink}";

            var smtp = new System.Net.Mail.SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new System.Net.Mail.MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                await smtp.SendMailAsync(message);
            }
        }

            // ✅ GET: api/User/5 (Get user by id)
            [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            return Ok(user);
        }

        // ✅ PUT: api/User/5 (Update user)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserID)
            {
                return BadRequest(new { message = "User ID mismatch." });
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound(new { message = "User not found." });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "User updated successfully." });
        }

        // ✅ POST: api/User (Create user manually - optional)
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.UserID }, user);
        }

        // ✅ POST: api/User/register (User Registration)
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            if (userDto == null)
                return BadRequest(new { message = "Invalid user data." });

            // ❗ Single Admin Check
            if (userDto.Role == "Admin")
            {
                var existingAdmin = await _context.Users.FirstOrDefaultAsync(u => u.Role == "Admin");
                if (existingAdmin != null)
                {
                    return BadRequest(new { message = "An Admin already exists." });
                }
            }

            // ❗ Check if Email already exists (soft check)
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "Email already registered." });
            }

            // ❗ Hash the password before saving
            string hashedPassword = HashPassword(userDto.PasswordHash);

            var newUser = new User
            {
                FullName = userDto.FullName,
                Email = userDto.Email,
                PasswordHash = hashedPassword,
                Role = userDto.Role,
                Gender = userDto.Gender
            };

            try
            {
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Registration successful." });
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("IX_Users_Email") == true)
                {
                    return BadRequest(new { message = "Email already registered (DB check)." });
                }

                return StatusCode(500, new { message = "Database error during registration." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Unexpected error: {ex.Message}" });
            }
        }

        // ✅ POST: api/User/login (User Login)
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
                return BadRequest(new { message = "Invalid login data." });

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            string hashedInputPassword = HashPassword(loginDto.PasswordHash);

            if (user.PasswordHash != hashedInputPassword)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            // Don't return the password!
            var result = new
            {
                user.UserID,
                user.FullName,
                user.Email,
                user.Role,
                user.Gender
            };

            return Ok(result);
        }
        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == forgotPasswordDto.Email);

            if (user == null)
                return NotFound("No user with this email found.");

            // Remove old token if it exists
            var existingRequest = await _context.PasswordResetRequests
                .FirstOrDefaultAsync(r => r.UserId == user.UserID);

            if (existingRequest != null)
            {
                _context.PasswordResetRequests.Remove(existingRequest);
                await _context.SaveChangesAsync();
            }

            // Generate new reset token
            var resetToken = Guid.NewGuid().ToString();
            var expiryTime = DateTime.UtcNow.AddHours(1);

            var passwordResetRequest = new PasswordResetRequest
            {
                UserId = user.UserID,
                Token = resetToken,
                ExpiryTime = expiryTime
            };

            _context.PasswordResetRequests.Add(passwordResetRequest);
            await _context.SaveChangesAsync();

            // Create a reset link
            var resetLink = $@"http://localhost/resetpassword?token={resetToken}";


            // Send the reset link via email
            await SendPasswordResetEmail(user.Email, resetLink);

            return Ok(new { message = "Password reset link has been sent to your email." });
        }


        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
                return NotFound("User not found.");

            user.PasswordHash = dto.NewPassword; // Hash if needed

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok("Password reset successful.");
        }



        [HttpGet("CheckEmail")]
        public async Task<IActionResult> CheckEmail(string email)
        {
            var exists = await _context.Users.AnyAsync(u => u.Email == email);
            return Ok(new { exists });
        }



        // ✅ DELETE: api/User/5 (Delete user)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User deleted successfully." });
        }

        // ✅ Helper to check if user exists
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }

        // ✅ Simple SHA256 password hashing (consider using Identity or more secure solutions later)
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}
