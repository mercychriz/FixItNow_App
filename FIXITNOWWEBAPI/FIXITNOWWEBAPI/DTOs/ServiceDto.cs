using System.ComponentModel.DataAnnotations;

namespace FIXITNOWWEBAPI.DTOs
{
    public class ServiceDto
    {
        [Required]
        public string ServiceName { get; set; }

        public string ServiceDescription { get; set; }

        [Range(0, 999999)]
        public decimal Price { get; set; }

        [Required]
        public int ServiceProviderId { get; set; }
    }

}
