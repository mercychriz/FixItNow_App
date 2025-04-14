using FixItNow.Models;
using System;
using Xamarin.Forms;

namespace FixItNow.View
{
    public partial class ConfirmationPage : ContentPage
    {
        private ServiceProvider _selectedServiceProvider;
        private DateTime _selectedDate;
        private TimeSpan _selectedTime;

        public ConfirmationPage(ServiceProvider serviceProvider, DateTime selectedDate, TimeSpan selectedTime)
        {
            InitializeComponent();

            _selectedServiceProvider = serviceProvider;
            _selectedDate = selectedDate;
            _selectedTime = selectedTime;

            // Set the confirmation message
            BookingDetailsLabel.Text = $"Your appointment with {_selectedServiceProvider.BusinessName} is scheduled for {_selectedDate.ToShortDateString()} at {_selectedTime}.";
        }

        private async void OnBackToHomeClicked(object sender, EventArgs e)
        {
            // Navigate back to UserLandingPage
            await Navigation.PushAsync(new UserLanding());
        }
    }
}
