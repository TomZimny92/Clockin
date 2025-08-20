using Clockin.Models;
using Clockin.ViewModels;
using System.Text.Json;

namespace Clockin;

public partial class AddTabPage : ContentPage
{
	private const string ContextModelKey = "ContextModelKey";

	public AddTabPage()
	{
		InitializeComponent();
	}

	private async void OnAddTabClicked(object sender, EventArgs e)
	{
		ContextModel cm = new();
		cm.Name = TabName.Text;
		cm.Id = Guid.CreateVersion7();

		ShellContent newTab = new ShellContent
		{
			Title = cm.Name,
			// Icon = cm.Icon
			ContentTemplate = new DataTemplate(() => new MainViewModel()),
			Route = $"MainPage?id={cm.Id}"
		};

		var tabsLength = (Shell.Current as AppShell)?.MainTabBar.Items.Count;
		if (tabsLength > 0 && tabsLength != null)
		{
			int newIndex = (int)tabsLength - 1;
            (Shell.Current as AppShell)?.MainTabBar.Items.Insert(newIndex, newTab);
        }
        
		
		await SecureStorage.SetAsync(ContextModelKey, JsonSerializer.Serialize(cm));
	}

	private void OnCloseTabClicked(object sender, EventArgs e)
	{
		Console.WriteLine("close");
	}
}