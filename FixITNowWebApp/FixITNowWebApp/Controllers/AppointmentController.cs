using FixITNowWebApp.ViewModel;
using FixITNowWebApp.Services;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FixITNowWebApp.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly ApiService _apiService = new ApiService();

        [HttpGet]
        public async Task<ActionResult> Book(int id)
        {
            var provider = await _apiService.GetServiceProviderByIdAsync(id);
            if (provider == null)
            {
                TempData["Error"] = "Service provider not found.";
                return RedirectToAction("UserLanding", "Account");
            }

            var model = new BookingViewModel
            {
                ServiceProviderId = provider.ServiceProviderId,
                ServiceProviderName = provider.BusinessName,
                Date = DateTime.Today,
                Time = DateTime.Now.TimeOfDay
            };

            return View("~/Views/Users/Payment.cshtml", model); 
        }

        [HttpPost]
        public ActionResult ConfirmPayment(BookingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Users/Payment.cshtml", model);
            }

            // Simulate saving payment and appointment if needed

            TempData["Message"] = "Payment successful!";
            return RedirectToAction("UserLanding", "Account");
        }
    }

}
