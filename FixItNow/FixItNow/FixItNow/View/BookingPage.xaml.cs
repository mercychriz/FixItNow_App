using FixItNow.Models;
using FixItNow.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FixItNow.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookingPage : ContentPage
    {
        public BookingPage(ServiceProvider provider)
        {
            InitializeComponent();

           
            BindingContext = new BookingViewModel(provider);
        }
    }
}
