using FixItNow.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FixItNow.ViewModel
{
    public class SignUpViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService = new ApiService();

        public ICommand RegisterCommand { get; }
        public ICommand GoToLoginCommand { get; }

        // Bindable Properties
        private string fullname;
        private string email;
        private string password;
        private string confirmPassword;
        private string selectedRole;
        private string selectedGender;

        // Roles and Genders collections for Picker ItemsSources
        public ObservableCollection<string> Roles { get; set; }
        public ObservableCollection<string> Genders { get; set; }

        public string FullName { get => fullname; set { fullname = value; OnPropertyChanged(nameof(FullName)); } }
        public string Email { get => email; set { email = value; OnPropertyChanged(nameof(Email)); } }
        public string Password { get => password; set { password = value; OnPropertyChanged(nameof(Password)); } }
        public string ConfirmPassword { get => confirmPassword; set { confirmPassword = value; OnPropertyChanged(nameof(ConfirmPassword)); } }
        public string SelectedRole { get => selectedRole; set { selectedRole = value; OnPropertyChanged(nameof(SelectedRole)); } }
        public string SelectedGender { get => selectedGender; set { selectedGender = value; OnPropertyChanged(nameof(SelectedGender)); } }

        public INavigation Navigation { get; set; }

        public SignUpViewModel()
        {
            RegisterCommand = new Command(async () => await RegisterUserAsync());
            GoToLoginCommand = new Command(async () => await GoToLoginPage());

            // ✅ Populate the Pickers
            Roles = new ObservableCollection<string>
            {
                "Admin",
                "User",
                "Service Provider"
            };

            Genders = new ObservableCollection<string>
            {
                "Male",
                "Female",
                "Other"
            };
        }

        private async Task RegisterUserAsync()
        {
            if (Password != ConfirmPassword)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Passwords do not match.", "OK");
                return;
            }

            var result = await _apiService.RegisterUserAsync(FullName, Email, Password, SelectedRole, SelectedGender);

            if (result == "Success")
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Registration successful. Please log in.", "OK");

                // ✅ Make sure Login() page is referenced correctly
                await Navigation.PushAsync(new FixItNow.View.Login());
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Registration Failed", result, "OK");
            }
        }


        private async Task GoToLoginPage()
        {
            await Navigation.PushAsync(new View.Login());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
