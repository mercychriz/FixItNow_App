using FixItNow.Models;
using FixItNow.Services;
using FixItNow.View;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FixItNow.ViewModel
{
    public class BookingViewModel : BaseViewModel
    {
        private readonly ApiService _apiService = new ApiService();

        public DateTime SelectedDate { get; set; } = DateTime.Today;
        public TimeSpan SelectedTime { get; set; } = DateTime.Now.TimeOfDay;
        public DateTime MinimumDate => DateTime.Today;
        public DateTime MaximumDate => DateTime.Today.AddMonths(1);

        private string _customerName;
        public string CustomerName
        {
            get => _customerName;
            set => SetProperty(ref _customerName, value);
        }

        public ServiceProvider SelectedProvider { get; set; }

        public ICommand ConfirmBookingCommand { get; }

        public BookingViewModel(ServiceProvider provider)
        {
            SelectedProvider = provider;
            ConfirmBookingCommand = new Command(async () => await ConfirmBookingAsync());
        }

        private async Task ConfirmBookingAsync()
        {
            if (string.IsNullOrWhiteSpace(CustomerName))
            {
                await Application.Current.MainPage.DisplayAlert("Validation", "Customer name is required.", "OK");
                return;
            }

            int userId = Preferences.Get("UserID", 0);

            if (userId == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "User ID is invalid. Please login again.", "OK");
                return;
            }

            var appointment = new Appointment
            {
                UserProfileId = userId,
                ServiceProviderId = SelectedProvider.ServiceProviderId,
                AppointmentDate = SelectedDate.Date,
                AppointmentTime = SelectedTime,
                CustomerName = CustomerName
            };

            string result = await _apiService.BookAppointmentAsync(appointment);

            if (result.ToLower().StartsWith("success"))
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Appointment booked successfully!", "OK");

                // Redirect to payment page
                await Application.Current.MainPage.Navigation.PushAsync(
                    new PaymentPage(SelectedProvider, SelectedDate, SelectedTime)
                );
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Booking Failed", result, "OK");
            }
        }


    }
}

