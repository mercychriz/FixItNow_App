using FixItNow.Models;
using FixItNow.Services;
using FixItNow.View;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FixItNow.ViewModel
{
    public class UserProfileViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService = new ApiService();
        private readonly INavigation _navigation;

        public ICommand SaveProfileCommand { get; }
        public ICommand UploadProfilePictureCommand { get; }
        public ICommand UploadIdCardCommand { get; }

        public string PhoneNumber { get; set; }
        public string PreferredServices { get; set; }

        public ImageSource ProfileImage { get; set; }
        public ImageSource IdCardImage { get; set; }

        private byte[] profileImageBytes;
        private byte[] idCardImageBytes;

        public UserProfileViewModel(INavigation navigation)
        {
            _navigation = navigation;

            SaveProfileCommand = new Command(async () => await SaveProfileAsync());
            UploadProfilePictureCommand = new Command(async () => await UploadProfilePictureAsync());
            UploadIdCardCommand = new Command(async () => await UploadIdCardAsync());
        }

        public async Task SaveProfileAsync()
        {
            try
            {
                if (profileImageBytes == null || idCardImageBytes == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Please upload both Profile Picture and ID Card.", "OK");
                    return;
                }

                var userProfile = new UserProfileDto
                {
                    PhoneNumber = PhoneNumber,
                    PreferredServices = PreferredServices,
                    ProfilePicture = Convert.ToBase64String(profileImageBytes),
                    IdCard = Convert.ToBase64String(idCardImageBytes),
                    UserID = int.Parse(Xamarin.Essentials.Preferences.Get("UserID", "0"))
                };


                var result = await _apiService.SaveUserProfileAsync(userProfile);

                if (result.Success)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Profile saved successfully!", "OK");
                    await _navigation.PushAsync(new UserLanding());
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Failed", $"Failed to save profile.\n{result.ErrorMessage}", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Exception", $"An unexpected error occurred: {ex.Message}", "OK");
            }
        }





        public async Task UploadProfilePictureAsync()
        {
            var result = await Xamarin.Essentials.FilePicker.PickAsync();
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

        public async Task UploadIdCardAsync()
        {
            var result = await Xamarin.Essentials.FilePicker.PickAsync();
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
