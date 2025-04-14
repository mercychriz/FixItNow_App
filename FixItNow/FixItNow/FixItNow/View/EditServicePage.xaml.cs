using FixItNow.Models;
using FixItNow.ViewModel;
using Xamarin.Forms;

namespace FixItNow.View
{
    public partial class EditServicePage : ContentPage
    {
        public EditServicePage(Service selectedService)
        {
            InitializeComponent();

            // Pass selectedService and Navigation to ViewModel
            BindingContext = new EditServiceViewModel(selectedService, this.Navigation);
        }
    }
}
