using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FIXITNOWWEBAPI.Dtos
{
    public class UserDto
    {
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string Role { get; set; }

        public string Gender { get; set; }
    }
}
