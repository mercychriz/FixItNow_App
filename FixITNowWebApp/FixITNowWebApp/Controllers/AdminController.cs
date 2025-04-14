using System.Linq;
using System.Web.Mvc;
using FixItNowWebApp.DTOs;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using FixITNowWebApp.Services;
using FixITNowWebApp.DTOs;
using FixITNowWebApp.Services;
using System.Diagnostics;


namespace FixItNowWebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly ApiService _apiService; // ✅ Declare

        public AdminController()
        {
            _client.BaseAddress = new System.Uri("http://192.168.24.150:5074/api/AdminServices/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _apiService = new ApiService(); // ✅ Initialize
        }

        public async Task<ActionResult> Index()
        {
            var response = await _client.GetAsync("");
            var services = new List<ServiceDto>();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                services = JsonConvert.DeserializeObject<List<ServiceDto>>(json);
            }

            return View(services);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var response = await _client.DeleteAsync($"{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Service deleted.";
            }
            else
            {
                TempData["Error"] = "Delete failed.";
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> DownloadCsv()
        {
            var result = await _client.GetAsync("Download");
            if (!result.IsSuccessStatusCode) return Content("Download failed");

            var bytes = await result.Content.ReadAsByteArrayAsync();
            return File(bytes, "text/csv", "services_report.csv");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Add(ServiceDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var success = await _apiService.AddServiceAsync(model);  // ✅ Call to Web API

            if (success)
            {
                TempData["Message"] = "Service added successfully!";
                return RedirectToAction("Index"); // ✅ Redirect back to dashboard
            }

            TempData["Error"] = "Failed to add service.";
            return View(model);
        }


        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var service = await _apiService.GetServiceByIdAsync(id);
            if (service == null)
            {
                TempData["Error"] = "Service not found.";
                return RedirectToAction("Index");
            }

            return View(service);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ServiceDto model)
        {
            Debug.WriteLine("🟡 ENTERED POST Edit");
            Debug.WriteLine($"🧪 ServiceId: {model.ServiceId}, ProviderId: {model.ServiceProviderId}");

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Validation failed.";
                return View(model);
            }

            var success = await _apiService.UpdateServiceAsync(model.ServiceId, model);

            if (success)
            {
                TempData["Message"] = "Service updated successfully!";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Failed to update service.";
            return View(model);
        }




    }
}
