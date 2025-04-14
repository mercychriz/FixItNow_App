using FixItNow.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FixItNow.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserLanding : ContentPage
    {
        private UserLandingViewModel viewModel;

        public UserLanding()
        {
            InitializeComponent();

            // Initialize ViewModel
            viewModel = new UserLandingViewModel();

            // Bind ViewModel to the page
            BindingContext = viewModel;
        }

        // Optional: Trigger search from search bar (if not using command directly)
        private void OnSearchPressed(object sender, System.EventArgs e)
        {
            var searchBar = sender as SearchBar;
            if (searchBar != null && viewModel.SearchCommand.CanExecute(searchBar.Text))
            {
                viewModel.SearchCommand.Execute(searchBar.Text);
            }
        }

        // Optional: Trigger booking manually (if not using Command in XAML)
        private async void OnBookAppointmentClicked(object sender, System.EventArgs e)
        {
            var button = sender as Button;
            var selectedProvider = button?.BindingContext as FixItNow.Models.ServiceProvider;

            if (selectedProvider != null && viewModel.BookAppointmentCommand.CanExecute(selectedProvider))
            {
                viewModel.BookAppointmentCommand.Execute(selectedProvider);
            }
        }

        private void OnHomeClicked(object sender, System.EventArgs e)
        {
            if (viewModel.HomeCommand.CanExecute(null))
            {
                viewModel.HomeCommand.Execute(null);
            }
        }

        private void OnBookingsClicked(object sender, System.EventArgs e)
        {
            if (viewModel.BookingsCommand.CanExecute(null))
            {
                viewModel.BookingsCommand.Execute(null);
            }
        }
    }
}
