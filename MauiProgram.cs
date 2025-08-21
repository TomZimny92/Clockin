using Clockin.Services;
using Clockin.ViewModels;
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
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<IStartupDataService, StartupDataService>();

            builder.Services.AddTransient<MainViewModel>(sp =>
            {
                var ds = sp.GetRequiredService<IStartupDataService>();
                return new MainViewModel(ds);
            });
            //var lastOpenTab = await GetLastTabSelected();

            //builder.Services.AddTransient<MainPage>();
            //builder.Services.AddTransient<MainViewModel>(sp =>
            //{
            //    return new MainViewModel(lastOpenTab);
            //});

            return builder.Build();
        }

        // moved to the "Services" folder under StartupDataService.cs
        //public static async Task<Guid> GetLastTabSelected()
        //{
        //    const string LastSelectedTabKey = "LastSelectedTabKey"; 
        //    var rawTabId = await SecureStorage.Default.GetAsync(LastSelectedTabKey);

        //    if (!String.IsNullOrEmpty(rawTabId) && Guid.TryParse(rawTabId, out Guid id))
        //    {
        //        return id;
        //    }
        //    else
        //    {
        //        return Guid.CreateVersion7();
        //    }
        //}
    }
}
