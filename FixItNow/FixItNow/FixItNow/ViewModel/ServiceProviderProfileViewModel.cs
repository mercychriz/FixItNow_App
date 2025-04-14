using FixItNow.Models;
using FixItNow.Services;
using FixItNow.View;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FixItNow.ViewModel
{
    public class ServiceProviderProfileViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService = new ApiService();
        private readonly INavigation _navigation;

        public ICommand SaveProfileCommand { get; }
        public ICommand UploadProfileImageCommand { get; }
        public ICommand UploadIdCardImageCommand { get; }

        public string BusinessName { get; set; }
        public string ServicesOffered { get; set; }
        public string Location { get; set; }
        public string Price { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public string Bio { get; set; }

        public ImageSource ProfileImage { get; set; }
        public ImageSource IdCardImage { get; set; }

        private byte[] profileImageBytes;
        private byte[] idCardImageBytes;

        public ServiceProviderProfileViewModel(INavigation navigation)
        {
            _navigation = navigation;

            SaveProfileCommand = new Command(async () => await SaveProfileAsync());
            UploadProfileImageCommand = new Command(async () => await UploadProfileImageAsync());
            UploadIdCardImageCommand = new Command(async () => await UploadIdCardImageAsync());
        }

        private async Task SaveProfileAsync()
        {
            if (profileImageBytes == null || idCardImageBytes == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please upload both Profile Picture and ID Card.", "OK");
                return;
            }

            var userIdString = Preferences.Get("UserID", string.Empty);
            if (!int.TryParse(userIdString, out int userId))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid user ID. Please login again.", "OK");
                return;
            }

            var serviceProvider = new ServiceProvider
            {
                UserID = userIdString,
                BusinessName = BusinessName,
                ServicesOffered = ServicesOffered,
                Location = Location,
                Price = Price,
                PhoneNumber = PhoneNumber,
                FullName = FullName,
                Bio = Bio,
                ProfileImagePath = Convert.ToBase64String(profileImageBytes),
                IdCardImagePath = Convert.ToBase64String(idCardImageBytes)
            };

            var result = await _apiService.SaveServiceProviderProfileAsync(serviceProvider);

            if (result.Success)
            {
                var profile = await _apiService.GetServiceProviderProfileAsync(userId);
                if (profile != null && profile.ServiceProviderId > 0)
                {
                    Preferences.Set("ServiceProviderId", profile.ServiceProviderId);
                    await Application.Current.MainPage.DisplayAlert("Success", "Profile saved successfully!", "OK");
                    await _navigation.PushAsync(new ServiceProviderLandingPage());
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Could not fetch your profile after saving.", "OK");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Failed", $"Profile save failed: {result.ErrorMessage}", "OK");
            }
        }

        private async Task UploadProfileImageAsync()
        {
            var result = await FilePicker.PickAsync();
            if (result != null)
            {
                using (var stream = await result.OpenReadAsync())
                {
                    profileImageBytes = ReadFully(stream);
                    ProfileImage = ImageSource.FromStream(() => new MemoryStream(profileImageBytes));
                    OnPropertyChanged(nameof(ProfileImage));
                }
            }
        }

        private async Task UploadIdCardImageAsync()
        {
            var result = await FilePicker.PickAsync();
            if (result != null)
            {
                using (var stream = await result.OpenReadAsync())
                {
                    idCardImageBytes = ReadFully(stream);
                    IdCardImage = ImageSource.FromStream(() => new MemoryStream(idCardImageBytes));
                    OnPropertyChanged(nameof(IdCardImage));
                }
            }
        }

        private byte[] ReadFully(Stream input)
        {
            using (var ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
