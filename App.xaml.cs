namespace Clockin
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //SetMainPageAsync();
        }

        //private async void SetMainPageAsync()
        //{
        //    var app = await MauiProgram.CreateMauiApp();
        //    Windows[0].Page = app.Services.GetRequiredService<MainPage>();
        //}

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}