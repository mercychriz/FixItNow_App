using System;
using Xamarin.Forms;

namespace FixItNow
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private async void NavigateToLogin(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new View.Login());
        }
        private async void NavigateToSignUP(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new View.SignUp());
        }
    }
}
