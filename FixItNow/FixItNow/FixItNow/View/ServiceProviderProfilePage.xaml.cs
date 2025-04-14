using FixItNow.ViewModel;
using Xamarin.Forms;

namespace FixItNow.View
{
    public partial class ServiceProviderProfilePage : ContentPage
    {
        public ServiceProviderProfilePage()
        {
            InitializeComponent();
            BindingContext = new ServiceProviderProfileViewModel(Navigation);
        }
    }
}
