using FixITNowWebApp.ViewModel;
using FixITNowWebApp.DTOs;
using FixITNowWebApp.Services;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FixITNowWebApp.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApiService _apiService = new ApiService();

        [HttpGet]
        public async Task<ActionResult> Payment(int providerId, DateTime date, TimeSpan time)
        {
            var provider = await _apiService.GetServiceProviderByIdAsync(providerId);

            if (provider == null)
            {
                TempData["Error"] = "Provider not found.";
                return RedirectToAction("UserLanding", "Account");
            }

            var model = new BookingViewModel
            {
                ServiceProviderId = providerId,
                ServiceProviderName = provider.FullName,
                Date = date,
                Time = time
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ConfirmPayment(BookingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Payment", model);
            }

            // 🧠 Optional: Save booking logic here

            TempData["Message"] = "Payment successful!";
            return RedirectToAction("UserLanding", "Account");
        }
    }
}
