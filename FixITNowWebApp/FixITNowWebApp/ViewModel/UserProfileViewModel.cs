using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixITNowWebApp.ViewModel
{
    public class UserProfileViewModel
    {
        public string PhoneNumber { get; set; }
        public string PreferredServices { get; set; }

        public string ProfileImageBase64 { get; set; }
        public string IdCardImageBase64 { get; set; }

        public HttpPostedFileBase ProfileImageFile { get; set; }
        public HttpPostedFileBase IdCardImageFile { get; set; }

        public int UserID { get; set; }
    }
}