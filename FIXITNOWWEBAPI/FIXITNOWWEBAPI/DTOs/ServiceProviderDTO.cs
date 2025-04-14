using System.ComponentModel.DataAnnotations;

namespace FIXITNOWWEBAPI.DTOs
{
    public class ServiceProviderDTO
    {
        public int ServiceProviderId { get; set; }

        [Required(ErrorMessage = "UserId is required.")]
        public string UserID { get; set; }

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string BusinessName { get; set; }

        public string ServicesOffered { get; set; }

        public string Location { get; set; }

        public string Price { get; set; }

        public string Bio { get; set; }

        public string ProfileImagePath { get; set; }

        public string IdCardImagePath { get; set; }

        
    }
}
