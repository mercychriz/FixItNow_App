using FixItNow.ViewModel;
using Xamarin.Forms;

namespace FixItNow.View
{
    public partial class AddServicePage : ContentPage
    {
        private readonly AddServiceViewModel _viewModel;

        public AddServicePage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new AddServiceViewModel();
        }
    }
}
