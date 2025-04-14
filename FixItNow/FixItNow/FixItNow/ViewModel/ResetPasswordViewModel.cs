using FixItNow.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FixItNow.ViewModel
{
    public class ResetPasswordViewModel : BaseViewModel
    {
        private readonly ApiService _apiService = new ApiService();
        private readonly INavigation _navigation;

        public string Email { get; set; }
        public string NewPassword { get; set; }

        public ICommand ResetPasswordCommand { get; }

        public ResetPasswordViewModel(INavigation navigation)
        {
            _navigation = navigation;
            ResetPasswordCommand = new Command(async () => await ExecuteResetPassword());
        }

        private async Task ExecuteResetPassword()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(NewPassword))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "All fields are required", "OK");
                return;
            }

            bool success = await _apiService.ResetPasswordAsync(Email, NewPassword);
            if (success)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Password reset successful", "OK");
                await _navigation.PopAsync(); // go back to login
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Reset failed. Try again.", "OK");
            }
        }
    }
}
