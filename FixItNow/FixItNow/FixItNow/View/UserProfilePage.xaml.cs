using FixItNow.ViewModel;
using Xamarin.Forms;


namespace FixItNow.View
{
    public partial class UserProfilePage : ContentPage
    {
        private UserProfileViewModel viewModel;

        public UserProfilePage()
        {
            InitializeComponent();

            viewModel = new UserProfileViewModel(Navigation);
            BindingContext = viewModel;
        }

    }
}
