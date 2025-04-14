using System.ComponentModel.DataAnnotations;
using System.Web;

namespace FixITNowWebApp.ViewModel
{
    public class ServiceProviderProfileViewModel
    {
        [Required]
        public string BusinessName { get; set; }

        [Required]
        public string ServicesOffered { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Price { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Bio { get; set; }

        [Required]
        public HttpPostedFileBase ProfileImageFile { get; set; }

        [Required]
        public HttpPostedFileBase IdCardImageFile { get; set; }

        public int UserID { get; set; }
    }
}
