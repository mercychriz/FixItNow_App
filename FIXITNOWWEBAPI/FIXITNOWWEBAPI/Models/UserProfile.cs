using System.ComponentModel.DataAnnotations;

namespace FIXITNOWWEBAPI.Models
{
    public class UserProfile
    {
        [Key]
        public int UserProfileId { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string PreferredServices { get; set; }

        // ✅ Accept Base64 Strings
        public string ProfilePicture { get; set; }
        public string IdCard { get; set; }

        public int UserID { get; set; } 
    }
}
