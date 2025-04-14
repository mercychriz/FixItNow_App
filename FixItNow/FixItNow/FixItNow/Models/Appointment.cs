using Newtonsoft.Json;
using System;


namespace FixItNow.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int UserProfileId { get; set; }
        public int ServiceProviderId { get; set; }
        public DateTime AppointmentDate { get; set; }

        [JsonConverter(typeof(TimeSpanConverter))] 
        public TimeSpan AppointmentTime { get; set; }

        public string CustomerName { get; set; }
    }

}
