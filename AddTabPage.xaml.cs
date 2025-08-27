using Clockin.Models;
using Clockin.Services;
using Clockin.ViewModels;
using System.Text.Json;

namespace Clockin;

public partial class AddTabPage : ContentPage
{
	private const string ContextModelKey = "ContextModelKey";
    private const string LastTabUsedKey = "LastTabUsedKey";

    public AddTabPage()
	{
		InitializeComponent();
	}

	private async void OnAddTabClicked(object sender, EventArgs e)
	{
        TabContext tabContext = new()
        {
            Name = TabName.Text,
            Id = Guid.CreateVersion7()
        };

        ShellContent newTab = new()
        {
			
			Title = tabContext.Name,
			// Icon = cm.Icon
			ContentTemplate = new DataTemplate(() => new MainViewModel()),
			Route = $"TabBar/?id={tabContext.Id}"
		};

		var tabsLength = (Shell.Current as AppShell)?.MainTabBar.Items.Count;
		if (tabsLength > 0 && tabsLength != null)
		{
			int newIndex = (int)tabsLength - 1;
            (Shell.Current as AppShell)?.MainTabBar.Items.Insert(newIndex, newTab);
        }

		//await SecureStorage.SetAsync(ContextModelKey, JsonSerializer.Serialize(tabContext));
		await SecureStorage.Default.SetAsync(tabContext.Id.ToString(), JsonSerializer.Serialize(tabContext));
		await SecureStorage.Default.SetAsync(LastTabUsedKey, tabContext.Id.ToString());
		// once added, go to MainPage with empty data
	}

	private void OnCloseTabClicked(object sender, EventArgs e)
	{
		Console.WriteLine("close");

		// navigate back to MainPage of the previously active tab

	}
}