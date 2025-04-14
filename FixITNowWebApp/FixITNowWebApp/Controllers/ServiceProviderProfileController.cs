using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using FixITNowWebApp.Services;
using FixITNowWebApp.ViewModel;
using FixITNowWebApp.Models;
using System.Diagnostics;

namespace FixITNowWebApp.Controllers
{
    public class ServiceProviderProfileController : Controller
    {
        private readonly ApiService _apiService = new ApiService();

        [HttpGet]
        public ActionResult Create()
        {
            var model = new ServiceProviderProfileViewModel
            {
                UserID = Convert.ToInt32(Session["UserId"]) 
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(ServiceProviderProfileViewModel model)
        {
            model.UserID = Convert.ToInt32(Session["UserId"]); 

            if (!ModelState.IsValid)
            {
                Debug.WriteLine("❌ ModelState is invalid.");
                return View(model);
            }

            if (model.ProfileImageFile == null || model.IdCardImageFile == null)
            {
                ModelState.AddModelError("", "Both profile picture and ID card are required.");
                Debug.WriteLine("❌ Missing files.");
                return View(model);
            }

            using (var profileStream = new MemoryStream())
            using (var idStream = new MemoryStream())
            {
                model.ProfileImageFile.InputStream.CopyTo(profileStream);
                model.IdCardImageFile.InputStream.CopyTo(idStream);

                var serviceProvider = new ServiceProvider
                {
                    UserID = model.UserID.ToString(),
                    BusinessName = model.BusinessName,
                    ServicesOffered = model.ServicesOffered,
                    Location = model.Location,
                    Price = model.Price,
                    PhoneNumber = model.PhoneNumber,
                    FullName = model.FullName,
                    Bio = model.Bio,
                    ProfileImagePath = Convert.ToBase64String(profileStream.ToArray()),
                    IdCardImagePath = Convert.ToBase64String(idStream.ToArray())
                };

                Debug.WriteLine($"📤 Sending profile for UserID: {serviceProvider.UserID}");

                var result = await _apiService.SaveServiceProviderProfileAsync(serviceProvider);

                Debug.WriteLine($"✅ API response: Success = {result.Success}");

                if (result.Success)
                {
                    TempData["Message"] = "Profile saved successfully!";
                    Debug.WriteLine("➡️ Redirecting to ServiceProviderLandingPage...");
                    return RedirectToAction("ServiceProviderLandingPage", "Account");
                }

                Debug.WriteLine($"❌ Save failed: {result.ErrorMessage}");
                ModelState.AddModelError("", result.ErrorMessage);
                return View(model);
            }
        }
    }
}
