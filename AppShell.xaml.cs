using Clockin.ViewModels;

namespace Clockin
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("mainpage", typeof(MainPage));
            Routing.RegisterRoute("addtabpage", typeof(AddTabPage));
            Routing.RegisterRoute("summarypage", typeof(SummaryPage));
            Routing.RegisterRoute("preferencespage", typeof(PreferencesPage));
            Routing.RegisterRoute("mainviewmodel", typeof(MainViewModel));
        }
    }
}
