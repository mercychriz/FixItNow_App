using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;  
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;


namespace FIXITNOWWEBAPI.Models
{
    public class Service
    {
        [Key]
        public int ServiceId { get; set; }

        [Required]
        public string ServiceName { get; set; }

        public string ServiceDescription { get; set; }

        [Precision(18, 2)] 
        public decimal Price { get; set; }

        // Foreign Key to ServiceProvider
        public int ServiceProviderId { get; set; }

        [JsonIgnore]
        [ValidateNever] 
        public ServiceProvider ServiceProvider { get; set; }

    }
}
