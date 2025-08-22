namespace Clockin
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //SetMainPageAsync();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}