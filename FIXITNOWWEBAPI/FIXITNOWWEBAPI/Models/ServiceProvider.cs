using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FIXITNOWWEBAPI.Models
{
    public class ServiceProvider
    {
        [Key]
        public int ServiceProviderId { get; set; }

        [Required]
        public string UserID { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [RegularExpression(@"^\+?\d{10,15}$")]
        public string PhoneNumber { get; set; }

        [Required]
        public string BusinessName { get; set; }

        [Required]
        [StringLength(255)]
        public string ServicesOffered { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Price { get; set; }

        [StringLength(500)]
        public string Bio { get; set; }

        [Required]
        [Url]
        public string ProfileImagePath { get; set; }

        [Required]
        [Url]
        public string IdCardImagePath { get; set; }

        // 🔥 Add this ONLY if you have a navigation property for Services
        [JsonIgnore]  // 👉 Prevents circular reference loops
        public ICollection<Service> Services { get; set; }
    }
}
