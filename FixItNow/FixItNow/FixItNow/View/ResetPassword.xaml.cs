using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FixItNow.ViewModel;

namespace FixItNow.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResetPassword : ContentPage
    {
        public ResetPassword()
        {
            InitializeComponent();
            BindingContext = new ResetPasswordViewModel(Navigation);
        }
    }
}
