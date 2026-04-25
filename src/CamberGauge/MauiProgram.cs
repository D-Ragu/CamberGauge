using CamberGauge.Services;
using CamberGauge.Services.Mock;
using CamberGauge.Services.Real;
using CamberGauge.Settings;
using CamberGauge.ViewModels;
using CamberGauge.Views;
using Microsoft.Extensions.Logging;

namespace CamberGauge;

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
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        if (AppFeatureFlags.UseMockSensorData)
        {
            builder.Services.AddSingleton<IAngleSensorService, MockAngleSensorService>();
        }
        else
        {
            builder.Services.AddSingleton<IAngleSensorService, AccelerometerAngleSensorService>();
        }

        builder.Services.AddTransient<MainPageViewModel>();
        builder.Services.AddTransient<MainPage>();

        return builder.Build();
    }
}