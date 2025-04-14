using FixItNow.Models;
using FixItNow.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FixItNow.ViewModel
{
    public class AddServiceViewModel : BaseViewModel
    {
        private readonly ApiService _apiService = new ApiService();

        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }
        public string Price { get; set; }

        public ICommand SaveServiceCommand { get; }

        public AddServiceViewModel()
        {
            SaveServiceCommand = new Command(async () => await SaveServiceAsync());
        }

        private async Task SaveServiceAsync()
        {
            if (string.IsNullOrWhiteSpace(ServiceName) ||
                string.IsNullOrWhiteSpace(Price))
            {
                await Application.Current.MainPage.DisplayAlert("Validation Error", "Please fill all required fields.", "OK");
                return;
            }

            if (!decimal.TryParse(Price, out var parsedPrice))
            {
                await Application.Current.MainPage.DisplayAlert("Validation Error", "Please enter a valid price.", "OK");
                return;
            }

            var serviceProviderId = Preferences.Get("ServiceProviderId", 0);

            if (serviceProviderId == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "ServiceProviderId not found.", "OK");
                return;
            }

            var newService = new Service
            {
                ServiceName = ServiceName,
                ServiceDescription = ServiceDescription, 
                Price = parsedPrice,
                ServiceProviderId = serviceProviderId
            };

            var result = await _apiService.AddServiceAsync(newService);

            if (result)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Service added successfully.", "OK");
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to add service.", "OK");
            }
        }


    }
}
