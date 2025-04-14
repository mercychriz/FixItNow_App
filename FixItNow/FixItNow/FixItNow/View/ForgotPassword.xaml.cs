using FixItNow.ViewModel;
using Xamarin.Forms;

namespace FixItNow.View
{
    public partial class ForgotPassword : ContentPage
    {
        private ForgotPasswordViewModel viewModel;

        public ForgotPassword()
        {
            InitializeComponent();

            viewModel = new ForgotPasswordViewModel
            {
                
            };

            BindingContext = viewModel;
        }
    }
}
