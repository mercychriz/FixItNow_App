using System.Windows.Input;
using Xamarin.Forms;
using FixItNow.Services;
using System.Threading.Tasks;


namespace FixItNow.ViewModel
{
    public class ForgotPasswordViewModel : BaseViewModel
    {
        private readonly ApiService _apiService = new ApiService();
        public INavigation Navigation { get; set; }

        private string email;
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public ICommand ResetPasswordCommand { get; }

        public ForgotPasswordViewModel()
        {
            ResetPasswordCommand = new Command(async () => await ExecuteResetPassword());
        }

        private async Task ExecuteResetPassword()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                await Application.Current.MainPage.DisplayAlert("Validation Error", "Email cannot be empty.", "OK");
                return;
            }

            bool result = await _apiService.ForgotPasswordAsync(Email);

            if (result)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Reset instructions sent. Enter your token and new password.", "OK");

                // Navigate to ResetPassword page and pass email or token (as required)
                await Navigation.PushAsync(new View.ResetPassword());
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Email not found. Try again.", "OK");
            }
        }
    }
}
