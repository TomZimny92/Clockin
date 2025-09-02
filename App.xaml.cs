using Clockin.Services;

namespace Clockin
{
    public partial class App : Application
    {
        private readonly IStartupDataService _startupDataService;

        public App(IStartupDataService sds)
        {
            _startupDataService = sds;
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell(_startupDataService));
        }
    }
}