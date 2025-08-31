using Clockin.Services;
using Clockin.ViewModels;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;

namespace Clockin
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<IStartupDataService, StartupDataService>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();


            return builder.Build();
        }
    }
}
