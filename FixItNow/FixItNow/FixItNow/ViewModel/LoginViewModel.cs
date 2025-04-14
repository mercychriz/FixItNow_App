
using FixItNow.Services;
using FixItNow.View;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FixItNow.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService = new ApiService();

        public INavigation Navigation { get; set; }

        private string email;
        private string password;

        public string Email
        {
            get => email;
            set { email = value; OnPropertyChanged(nameof(Email)); }
        }

        public string Password
        {
            get => password;
            set { password = value; OnPropertyChanged(nameof(Password)); }
        }

        public ICommand LoginCommand { get; }
        public ICommand ForgotPasswordCommand { get; }
        public ICommand GoToSignUpCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await LoginUserAsync());
            ForgotPasswordCommand = new Command(async () => await ForgotPasswordAsync());
            GoToSignUpCommand = new Command(async () => await GoToSignUpPage());
        }

        private async Task LoginUserAsync()
        {
            var user = await _apiService.LoginUserAsync(Email, Password);

            if (user != null)
            {
                Preferences.Set("UserID", user.UserID); 

                Preferences.Set("Role", user.Role);

                await Application.Current.MainPage.DisplayAlert("Success", $"Welcome {user.FullName}", "OK");

                switch (user.Role)
                {
                    case "Admin":
                        await Navigation.PushAsync(new AdminPage());
                        break;

                    case "User":
                        var userProfile = await _apiService.GetUserProfileByUserIdAsync(user.UserID);
                        if (userProfile != null)
                        {
                            Preferences.Set("UserProfileId", userProfile.UserProfileId);
                            await Navigation.PushAsync(new UserLanding());
                        }
                        else
                        {
                            await Navigation.PushAsync(new UserProfilePage());
                        }
                        break;

                    case "Service Provider":
                        var hasProfile = await _apiService.CheckIfServiceProviderProfileExists(user.UserID);
                        if (hasProfile)
                        {
                            var spProfile = await _apiService.GetServiceProviderProfileAsync(user.UserID);

                            if (spProfile != null)
                            {
                                Preferences.Set("ServiceProviderId", spProfile.ServiceProviderId);
                                await Navigation.PushAsync(new ServiceProviderLandingPage());
                            }
                            else
                            {
                                await Navigation.PushAsync(new ServiceProviderProfilePage());
                            }
                        }
                        else
                        {
                            await Navigation.PushAsync(new ServiceProviderProfilePage());
                        }
                        break;
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Login Failed", "Invalid credentials. Please try again.", "OK");
            }
        }


        private async Task ForgotPasswordAsync()
        {
            await Navigation.PushAsync(new ResetPassword());
        }

        private async Task GoToSignUpPage()
        {
            await Navigation.PushAsync(new SignUp());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
