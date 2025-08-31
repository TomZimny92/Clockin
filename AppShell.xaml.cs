using Clockin.Models;
using Clockin.Services;
using Clockin.ViewModels;
using System.Text.Json;

namespace Clockin
{
    public partial class AppShell : Shell
    {
        private const string TabCountKey = "TabCountKey";
        private const string TabIDListKey = "TabIDListKey";

        private readonly IStartupDataService _startupDataService;

        public AppShell(IStartupDataService sds)
        {
            InitializeComponent();

            Routing.RegisterRoute("mainpage", typeof(MainPage));
            Routing.RegisterRoute("addtabpage", typeof(AddTabPage));
            Routing.RegisterRoute("summarypage", typeof(SummaryPage));
            Routing.RegisterRoute("preferencespage", typeof(PreferencesPage));
            Routing.RegisterRoute("mainviewmodel", typeof(MainViewModel));

            _startupDataService = sds;
            PopulateTabBar();
            InitializeData();
        }

        private async void PopulateTabBar()
        {
            // get the tab count (probably from securestorage)
            string? tabListRaw = await SecureStorage.Default.GetAsync(TabIDListKey);
            if (!string.IsNullOrEmpty(tabListRaw))
            {
                // loop the number of tabs and add a ShellContent to the TabBar
                // not sure how to do that
                var tabIds = JsonSerializer.Deserialize<List<Guid>>(tabListRaw);
                if (tabIds != null)
                {
                    for (int i = 0; i < tabIds.Count; i++)
                    {
                        string? rawTabData = await SecureStorage.Default.GetAsync(tabIds[i].ToString());
                        if (rawTabData != null)
                        {
                            var tabData = JsonSerializer.Deserialize<TabContext>(rawTabData);
                            ShellContent newTab = new()
                            {
                                Title = tabData?.Name,

                            };
                        }
                    }
                }                    
                
            }
            else
            {
                // uhhh, route to AddTabPage?
            }
        }

        private async void InitializeData()
        {
            Guid? lastUsedTab = await _startupDataService.GetLastTabSelectedAsync();
            if (lastUsedTab != null)
            {
                // route the guid to the MainViewModel
                await Shell.Current.GoToAsync($"mainviewmodel?tabId={lastUsedTab}");

            }
            else
            {
                // route the user to the AddTabPage
                await Shell.Current.GoToAsync($"addtabpage");
            }
        }
    }
}
