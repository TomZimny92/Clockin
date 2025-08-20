using Clockin.Models;
using Clockin.ViewModels;

namespace Clockin;

public partial class AddTabPage : ContentPage
{
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
			ContentTemplate = new DataTemplate(() => new MainViewModel(cm.Id)),
			Route = $"MainPage?id={cm.Id}"
		};

		(Shell.Current as AppShell).MainTabBar.Items.Add(newTab);
	}
}