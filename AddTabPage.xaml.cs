using Clockin.Models;
using Clockin.Services;
using Clockin.ViewModels;
using System.Text.Json;

namespace Clockin;

public partial class AddTabPage : ContentPage
{
	private readonly IStartupDataService _startupDataService;

	private const string ContextModelKey = "ContextModelKey";

	public AddTabPage()
	{
		InitializeComponent();
	}

	private async void OnAddTabClicked(object sender, EventArgs e)
	{
		TabContext cm = new();
		cm.Name = TabName.Text;
		cm.Id = Guid.CreateVersion7();

		ShellContent newTab = new()
        {
			Title = cm.Name,
			// Icon = cm.Icon
			ContentTemplate = new DataTemplate(() => new MainViewModel(_startupDataService)),
			Route = $"MainPage?id={cm.Id}"
		};

		var tabsLength = (Shell.Current as AppShell)?.MainTabBar.Items.Count;
		if (tabsLength > 0 && tabsLength != null)
		{
			int newIndex = (int)tabsLength - 1;
            (Shell.Current as AppShell)?.MainTabBar.Items.Insert(newIndex, newTab);
        }        
		
		await SecureStorage.SetAsync(ContextModelKey, JsonSerializer.Serialize(cm));

		// once added, go to MainPage with empty data
	}

	private void OnCloseTabClicked(object sender, EventArgs e)
	{
		Console.WriteLine("close");

		// navigate back to MainPage of the previously active tab

	}
}