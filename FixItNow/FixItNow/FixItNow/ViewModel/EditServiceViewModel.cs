using FixItNow.Models;
using FixItNow.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FixItNow.ViewModel
{
    public class EditServiceViewModel : BaseViewModel
    {
        private readonly ApiService _apiService = new ApiService();
        private readonly INavigation _navigation;
        private readonly Service _existingService;

        private string _serviceName;
        public string ServiceName
        {
            get => _serviceName;
            set => SetProperty(ref _serviceName, value);
        }

        private string _serviceDescription;
        public string ServiceDescription
        {
            get => _serviceDescription;
            set => SetProperty(ref _serviceDescription, value);
        }

        private string _price;
        public string Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        public ICommand SaveChangesCommand { get; }

        public EditServiceViewModel(Service service, INavigation navigation)
        {
            _navigation = navigation;

           
            _existingService = new Service
            {
                ServiceId = service.ServiceId,
                ServiceName = service.ServiceName,
                ServiceDescription = service.ServiceDescription,
                Price = service.Price,
                ServiceProviderId = service.ServiceProviderId
            };

            ServiceName = service.ServiceName;
            ServiceDescription = service.ServiceDescription;
            Price = service.Price.ToString();

            SaveChangesCommand = new Command(async () => await SaveServiceAsync());
        }


        private async Task SaveServiceAsync()
        {
            if (string.IsNullOrWhiteSpace(ServiceName) || string.IsNullOrWhiteSpace(Price))
            {
                await Application.Current.MainPage.DisplayAlert("Validation Error", "Please fill all required fields.", "OK");
                return;
            }

            if (!decimal.TryParse(Price, out var parsedPrice))
            {
                await Application.Current.MainPage.DisplayAlert("Validation Error", "Please enter a valid price.", "OK");
                return;
            }

            _existingService.ServiceName = ServiceName;
            _existingService.ServiceDescription = ServiceDescription;
            _existingService.Price = parsedPrice;
            System.Diagnostics.Debug.WriteLine($"ServiceProviderId being sent: {_existingService.ServiceProviderId}");


            var result = await _apiService.UpdateServiceAsync(_existingService);

            if (result)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Service updated successfully.", "OK");
                await _navigation.PopAsync();
            }

            // ⚠️ Removed else-block that says "Failed to update service"
            // The ApiService already shows the real error message now
        }
    }
}
