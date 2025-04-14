using System;
using System.ComponentModel.DataAnnotations;

namespace FIXITNOWWEBAPI.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        [Required]
        public int UserProfileId { get; set; }

        [Required]
        public int ServiceProviderId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan AppointmentTime { get; set; }

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; }

        public bool IsCompleted { get; set; } = false;
    }
}