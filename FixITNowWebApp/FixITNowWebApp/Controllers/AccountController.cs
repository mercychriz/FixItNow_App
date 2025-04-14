using FixItNowWebApp.DTOs;
using FixITNowWebApp.Models;
using FixITNowWebApp.Services;
using FixITNowWebApp.ViewModel;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using FixITNowWebApp.Helpers;

namespace FixITNowWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiService _apiService = new ApiService();
        private readonly HttpClient _client;

        public AccountController()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://192.168.24.150:5074/api/");
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public ActionResult Index()
        {
            return View(); 
        }

        // ✅ GET: /Account/Login
        public ActionResult Login()
        {
            return View(new LoginDto());
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var loginData = new
            {
                Email = model.Email.ToLower(),
                PasswordHash = model.Password // ✅ plain password like mobile
            };

            var json = JsonConvert.SerializeObject(loginData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("User/login", content);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Invalid email or password.";
                return View(model);
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserDto>(responseJson);

            if (user == null)
            {
                ViewBag.Error = "Login failed.";
                return View(model);
            }

            // Store session
            Session["User"] = user;
            Session["UserId"] = user.UserID;
            Session["UserRole"] = user.Role;

            switch (user.Role)
            {
                case "Admin":
                    Session["IsAdmin"] = true;
                    return RedirectToAction("Index", "Admin");

                case "User":
                    return RedirectToAction("UserLanding", "Account");

                case "Service Provider":
                    return RedirectToAction("ServiceproviderLandingPage", "Account");

                default:
                    ViewBag.Error = "Unknown role.";
                    return View(model);
            }
        }


        public ActionResult SignUp()
        {
            return View(new SignUpViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> SignUp(SignUpViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            using (HttpClient client = new HttpClient())
            {
                var payload = new
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    PasswordHash = model.Password,
                    Role = model.SelectedRole,
                    Gender = model.SelectedGender
                };

                var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://192.168.24.150:5074/api/User/register", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Registration successful!";
                    return RedirectToAction("Login");
                }

                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Error: {error}");
                return View(model);
            }
        }

        // ✅ User Landing
        public async Task<ActionResult> UserLanding()
        {
            if (Session["UserId"] == null) return RedirectToAction("Login");

            int userId = Convert.ToInt32(Session["UserId"]);
            var profile = await _apiService.GetUserProfileByUserIdAsync(userId);

            if (profile == null)
                return RedirectToAction("Create", "UserProfile");

            var providers = await _apiService.GetServiceProvidersAsync();

            var model = new UserLandingViewModel
            {
                UserEmail = profile.PhoneNumber,
                ProfileImageBase64 = string.IsNullOrEmpty(profile.ProfilePicture)
                    ? null
                    : $"data:image/png;base64,{profile.ProfilePicture}",
                ServiceProviders = providers
            };

            return View("~/Views/Users/UserLanding.cshtml", model);
        }
        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View(new ResetPasswordViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var payload = new
            {
                Email = model.Email,
                NewPassword = model.NewPassword
            };

            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                var response = await client.PostAsync("http://192.168.24.150:5074/api/User/resetpassword", content);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Password reset successful!";
                    return RedirectToAction("Login");
                }

                ModelState.AddModelError("", "Failed to reset password. Please check your email.");
                return View(model);
            }
        }


        // ✅ Service Provider Landing
        public async Task<ActionResult> ServiceproviderLandingPage()
        {
            if (Session["UserId"] == null) return RedirectToAction("Login");

            int userId = Convert.ToInt32(Session["UserId"]);
            var profile = await _apiService.GetServiceProviderProfileByUserIdAsync(userId);

            if (profile == null)
                return RedirectToAction("Create", "ServiceProviderProfile");

            var services = await _apiService.GetServicesByProviderIdAsync(profile.ServiceProviderId);
            var appointments = await _apiService.GetAppointmentsByProviderIdAsync(profile.ServiceProviderId);

            var model = new ServiceProviderLandingViewModel
            {
                BusinessName = profile.BusinessName,
                Bio = profile.Bio,
                ProfileImageBase64 = string.IsNullOrEmpty(profile.ProfileImagePath)
                    ? null
                    : $"data:image/png;base64,{profile.ProfileImagePath}",
                Services = services,
                Appointments = appointments
            };

            return View("~/Views/ServiceProviders/ServiceProviderLandingPage.cshtml", model);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

      
    }
}
