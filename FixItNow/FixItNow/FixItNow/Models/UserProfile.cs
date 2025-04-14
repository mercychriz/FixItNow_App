
namespace FixItNow.Models
{
    public class UserProfile
    {
        public int UserProfileId { get; set; }
        public string FullName { get; internal set; }
        public string PhoneNumber { get; set; }
        public string PreferredServices { get; set; }
        public string ProfilePicturePath { get; set; }
        public string IdCardPath { get; set; }
        public string Email { get; set; }  // Add this if needed
        public string ProfileImagePath => ProfilePicturePath; // Alias property

    }
}
