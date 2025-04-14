using FixITNowWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixITNowWebApp.ViewModel
{
    public class UserLandingViewModel
    {
        public string UserFullName { get; set; }
        public string UserEmail { get; set; }
        public string ProfileImageBase64 { get; set; } // base64 or URL
        public List<ServiceProvider> ServiceProviders { get; set; }
    }
}