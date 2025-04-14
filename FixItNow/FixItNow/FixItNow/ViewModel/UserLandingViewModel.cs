using FixItNow.Models;
using FixItNow.Services;
using FixItNow.View;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FixItNow.ViewModel
{
    public class UserLandingViewModel : BaseViewModel
    {
        private readonly ApiService _apiService = new ApiService();

        private string _userFullName;
        public string UserFullName
        {
            get => _userFullName;
            set => SetProperty(ref _userFullName, value);
        }

        private string _userEmail;
        public string UserEmail
        {
            get => _userEmail;
            set => SetProperty(ref _userEmail, value);
        }

        private ImageSource _userProfileImage;
        public ImageSource UserProfileImage
        {
            get => _userProfileImage;
            set => SetProperty(ref _userProfileImage, value);
        }

        public ObservableCollection<ServiceProvider> ServiceProviders { get; set; } = new ObservableCollection<ServiceProvider>();

        public ICommand SearchCommand { get; }
        public ICommand BookAppointmentCommand { get; }
        public ICommand HomeCommand { get; }
        public ICommand BookingsCommand { get; }

        public UserLandingViewModel()
        {
            SearchCommand = new Command<string>(async (keyword) => await ExecuteSearch(keyword));
            BookAppointmentCommand = new Command<ServiceProvider>(async (provider) => await ExecuteBookAppointment(provider));
            HomeCommand = new Command(async () => await ExecuteHome());
            BookingsCommand = new Command(async () => await ExecuteBookings());

            InitializeData();
        }

        private async void InitializeData()
        {
            await LoadUserProfile();
            await LoadServiceProviders();
        }

        private async Task LoadUserProfile()
        {
            try
            {
                IsBusy = true;

                // ✅ Get integer directly from preferences
                int userId = Preferences.Get("UserID", 0);

                if (userId == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "User ID not found. Please login again.", "OK");
                    return;
                }

                var profile = await _apiService.GetUserProfileByUserIdAsync(userId);

                if (profile == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Profile Missing", "Please create your profile.", "OK");
                    await App.Current.MainPage.Navigation.PushAsync(new UserProfilePage());
                    return;
                }

                UserFullName = profile.FullName;
                UserEmail = profile.PhoneNumber;
                UserProfileImage = string.IsNullOrEmpty(profile.ProfilePicturePath)
                    ? "default_profile.png"
                    : ImageSource.FromUri(new Uri(profile.ProfilePicturePath));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load user profile: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }


        private async Task LoadServiceProviders()
        {
            IsBusy = true;
            try
            {
                var providers = await _apiService.GetServiceProvidersAsync();
                ServiceProviders.Clear();

                foreach (var provider in providers)
                {
                    ServiceProviders.Add(provider);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load providers: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ExecuteSearch(string keyword)
        {
            IsBusy = true;
            try
            {
                var providers = await _apiService.SearchServiceProvidersAsync(keyword);
                ServiceProviders.Clear();

                foreach (var provider in providers)
                {
                    ServiceProviders.Add(provider);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Search Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ExecuteBookAppointment(ServiceProvider provider)
        {
            if (provider == null) return;
            await App.Current.MainPage.Navigation.PushAsync(new BookingPage(provider));
        }

        private async Task ExecuteHome()
        {
            await LoadServiceProviders();
        }

        private async Task ExecuteBookings()
        {
            // Update if you have a real bookings page
            await Application.Current.MainPage.DisplayAlert("Bookings", "Feature coming soon...", "OK");
            // Example:
            // await App.Current.MainPage.Navigation.PushAsync(new UserBookingsPage());
        }
    }
}
