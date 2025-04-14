using System.Collections.Generic;

using FixITNowWebApp.Models;

namespace FixITNowWebApp.ViewModel
{
    public class ServiceProviderLandingViewModel
    {
        public string BusinessName { get; set; }
        public string Bio { get; set; }
        public string ProfileImageBase64 { get; set; } // string for image: data:image/png;base64,...

        public List<Service> Services { get; set; } = new List<Service>();
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
