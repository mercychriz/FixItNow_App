using FixItNow.Models;
using FixItNow.Services;
using FixItNow.View;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FixItNow.ViewModel
{
    public class ServiceProviderLandingViewModel : BaseViewModel
    {
        private readonly ApiService _apiService = new ApiService();
        private readonly INavigation _navigation;

        private string _businessName;
        public string BusinessName
        {
            get => _businessName;
            set => SetProperty(ref _businessName, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private ImageSource _profileImage;
        public ImageSource ProfileImage
        {
            get => _profileImage;
            set => SetProperty(ref _profileImage, value);
        }

        public ObservableCollection<Service> Services { get; set; } = new ObservableCollection<Service>();
        public ObservableCollection<Appointment> Appointments { get; set; } = new ObservableCollection<Appointment>();

        public ICommand AddNewServiceCommand { get; }
        public ICommand EditServiceCommand { get; }
        public ICommand DeleteServiceCommand { get; }
        public ICommand MarkCompletedCommand { get; }

        public ServiceProviderLandingViewModel(INavigation navigation)
        {
            _navigation = navigation;

            AddNewServiceCommand = new Command(async () => await AddNewService());
            EditServiceCommand = new Command<Service>(async (service) => await EditService(service));
            DeleteServiceCommand = new Command<Service>(async (service) => await DeleteService(service));
            MarkCompletedCommand = new Command<Appointment>(async (appointment) => await MarkCompleted(appointment));

            LoadData();
        }

        private async void LoadData()
        {
            await EnsureServiceProviderIdExists();
            await LoadServiceProviderProfile();
            await LoadServices();
            await LoadAppointments();
        }

        private async Task EnsureServiceProviderIdExists()
        {
            var serviceProviderId = Preferences.Get("ServiceProviderId", 0);
            var userId = Preferences.Get("UserID", 0);

            if (serviceProviderId == 0 && userId > 0)
            {
                var profile = await _apiService.GetServiceProviderProfileAsync(userId);

                if (profile != null && profile.ServiceProviderId > 0)
                {
                    Preferences.Set("ServiceProviderId", profile.ServiceProviderId);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "ServiceProviderId not found! Please login again.", "OK");
                }
            }
        }

        private async Task LoadServiceProviderProfile()
        {
            IsBusy = true;
            try
            {
                var serviceProviderId = Preferences.Get("ServiceProviderId", 0);

                if (serviceProviderId == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "ServiceProviderId not found!", "OK");
                    return;
                }

                var profile = await _apiService.GetServiceProviderByIdAsync(serviceProviderId);

                if (profile != null)
                {
                    BusinessName = profile.BusinessName;
                    Description = profile.ServicesOffered;

                    if (!string.IsNullOrEmpty(profile.ProfileImagePath))
                    {
                        byte[] imageBytes = Convert.FromBase64String(profile.ProfileImagePath);
                        ProfileImage = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                    }
                    else
                    {
                        ProfileImage = "default_profile.png";
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load profile: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task LoadServices()
        {
            IsBusy = true;
            try
            {
                var serviceProviderId = Preferences.Get("ServiceProviderId", 0);

                var services = await _apiService.GetServicesByProviderIdAsync(serviceProviderId);

                Services.Clear();
                foreach (var service in services)
                {
                    Services.Add(service);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load services: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task LoadAppointments()
        {
            IsBusy = true;
            try
            {
                var serviceProviderId = Preferences.Get("ServiceProviderId", 0);
                var appointments = await _apiService.GetAppointmentsByProviderIdAsync(serviceProviderId);

                Appointments.Clear();
                foreach (var appointment in appointments)
                {
                    Appointments.Add(appointment);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load appointments: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task AddNewService()
        {
            await _navigation.PushAsync(new AddServicePage());
        }

        private async Task EditService(Service service)
        {
            if (service == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please select a service first!", "OK");
                return;
            }

            await _navigation.PushAsync(new EditServicePage(service));
        }

        private async Task DeleteService(Service service)
        {
            if (service == null) return;

            var confirm = await Application.Current.MainPage.DisplayAlert("Delete", $"Delete {service.ServiceName}?", "Yes", "No");
            if (!confirm) return;

            var result = await _apiService.DeleteServiceAsync(service.ServiceId);

            if (result)
            {
                Services.Remove(service);
                await Application.Current.MainPage.DisplayAlert("Deleted", "Service deleted.", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to delete service.", "OK");
            }
        }

        private async Task MarkCompleted(Appointment appointment)
        {
            if (appointment == null) return;

            var confirm = await Application.Current.MainPage.DisplayAlert("Complete", $"Mark appointment for {appointment.CustomerName} as completed?", "Yes", "No");
            if (!confirm) return;

            var result = await _apiService.MarkAppointmentCompletedAsync(appointment.AppointmentId);

            if (result)
            {
                await LoadAppointments();
                await Application.Current.MainPage.DisplayAlert("Completed", "Appointment marked as completed.", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to mark appointment as completed.", "OK");
            }
        }
    }
}
