using Clockin.ViewModels;
using System.Collections.ObjectModel;
using System.Text.Json;

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
