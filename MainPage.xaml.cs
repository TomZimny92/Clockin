using Clockin.ViewModels;

namespace Clockin
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel mvm)
        {
            InitializeComponent();
            BindingContext = mvm;
        }
    }
}
