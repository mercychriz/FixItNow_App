using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixITNowWebApp.DTOs
{
    public class UserProfileDto
    {
        public string PhoneNumber { get; set; }
        public string PreferredServices { get; set; }
        public string ProfilePicture { get; set; }
        public string IdCard { get; set; }
        public int UserID { get; set; }
    }
}