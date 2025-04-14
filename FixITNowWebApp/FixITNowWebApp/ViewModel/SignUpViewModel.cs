using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixITNowWebApp.ViewModel
{
    public class SignUpViewModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string SelectedRole { get; set; }
        public string SelectedGender { get; set; }

        public List<string> Roles { get; set; } = new List<string> { "User", "Service Provider", "Admin" };
        public List<string> Genders { get; set; } = new List<string> { "Male", "Female", "Other" };
    }
}