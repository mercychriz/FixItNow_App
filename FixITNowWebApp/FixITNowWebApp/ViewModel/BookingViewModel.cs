using System;
using System.ComponentModel.DataAnnotations;

namespace FixITNowWebApp.ViewModel
{
    public class BookingViewModel
    {
        public int ServiceProviderId { get; set; }

        [Display(Name = "Service Provider Name")]
        public string ServiceProviderName { get; set; }

        [Required(ErrorMessage = "Customer name is required.")]
        public string CustomerName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan Time { get; set; }

        [Required]
        [Range(1, 999999)]
        public decimal Amount { get; set; }

        [Required]
        public string Method { get; set; }
    }

}
