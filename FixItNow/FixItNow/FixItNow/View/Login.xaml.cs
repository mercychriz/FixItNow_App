using FixItNow.ViewModel;
using Xamarin.Forms;

namespace FixItNow.View
{
    public partial class Login : ContentPage
    {
        private LoginViewModel viewModel;

        public Login()
        {
            InitializeComponent();

            viewModel = new LoginViewModel
            {
                Navigation = this.Navigation
            };

            BindingContext = viewModel;
        }
    }
}
