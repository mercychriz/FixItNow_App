using FixItNow.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;

namespace FixItNow.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentPage : ContentPage
    {
        public ServiceProvider SelectedServiceProvider { get; set; }
        public DateTime SelectedDate { get; set; }
        public TimeSpan SelectedTime { get; set; }

        public PaymentPage(ServiceProvider selectedServiceProvider, DateTime selectedDate, TimeSpan selectedTime)
        {
            InitializeComponent();

            SelectedServiceProvider = selectedServiceProvider;
            SelectedDate = selectedDate;
            SelectedTime = selectedTime;

            BindingContext = this;
        }

        private async void OnPayNowClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Success", "Payment processed!", "OK");

            // Navigate to UserLanding page
            await Application.Current.MainPage.Navigation.PushAsync(new UserLanding());
        }

    }
}
