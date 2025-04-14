using FixItNow.ViewModel;
using Xamarin.Forms;

namespace FixItNow.View 
{
    public partial class SignUp : ContentPage
    {
        private SignUpViewModel viewModel;

        public SignUp()
        {
            InitializeComponent(); 

            viewModel = new SignUpViewModel
            {
                Navigation = this.Navigation
            };

            BindingContext = viewModel;
        }
    }
}
