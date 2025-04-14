using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixItNowWebApp.DTOs
{
    public class UserDto
    {
        public int UserID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}