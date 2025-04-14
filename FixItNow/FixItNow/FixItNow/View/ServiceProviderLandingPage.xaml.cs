using FixItNow.ViewModel;
using Xamarin.Forms;

namespace FixItNow.View
{
    public partial class ServiceProviderLandingPage : ContentPage
    {
        public ServiceProviderLandingPage()
        {
            InitializeComponent();
            BindingContext = new ServiceProviderLandingViewModel(this.Navigation);
        }
    }
}
