using FixITNowWebApp.ViewModel;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using FixITNowWebApp.Services;
using FixITNowWebApp.Models;
using System.Diagnostics;
using FixITNowWebApp.DTOs;

namespace FixITNowWebApp.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly ApiService _apiService = new ApiService();

        [HttpGet]
        public ActionResult Create()
        {
            var model = new UserProfileViewModel
            {
                UserID = Convert.ToInt32(Session["UserId"]) 
            };

            return View("~/Views/Users/Create.cshtml", model);
        }


        [HttpPost]
        public async Task<ActionResult> Create(UserProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Users/Create.cshtml", model);

            if (model.ProfileImageFile == null || model.IdCardImageFile == null)
            {
                ModelState.AddModelError("", "Both profile picture and ID card are required.");
                return View("~/Views/Users/Create.cshtml", model);
            }

            using (var profileStream = new MemoryStream())
            using (var idStream = new MemoryStream())
            {
                model.ProfileImageFile.InputStream.CopyTo(profileStream);
                model.IdCardImageFile.InputStream.CopyTo(idStream);

                var dto = new UserProfileDto
                {
                    PhoneNumber = model.PhoneNumber,
                    PreferredServices = model.PreferredServices,
                    ProfilePicture = Convert.ToBase64String(profileStream.ToArray()),
                    IdCard = Convert.ToBase64String(idStream.ToArray()),
                    UserID = model.UserID
                };

                var result = await _apiService.SaveUserProfileAsync(dto);
                Debug.WriteLine($"📡 SaveUserProfileAsync: Success={result.Success}, Error={result.ErrorMessage}");


                if (result.Success)
                {
                    TempData["Message"] = "Profile saved successfully!";
                    return RedirectToAction("UserLanding", "Account");
                }


                ModelState.AddModelError("", result.ErrorMessage);
                return View("~/Views/Users/Create.cshtml", model);
            }
        }
    }

}
