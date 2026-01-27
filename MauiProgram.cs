

using Microsoft.Extensions.Logging;
using MyJournal.Services;
using MyJournal.Models;
using MyJournal.Services.Interfaces;

namespace MyJournal
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
                });

            builder.Services.AddMauiBlazorWebView();
            //builder.Services.AddFluentUIComponents();
            builder.Services.AddSingleton<ThemeService>();
            builder.Services.AddSingleton<IDatabaseService,DatabaseService>();
            builder.Services.AddScoped<AuthSession>();
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddScoped<IToastService, ToastService>();

#if WINDOWS
            builder.Services.AddScoped<IPrintService, MyJournal.Services.PrintService>();

#endif

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
