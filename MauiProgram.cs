

using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;
using MyJournal.Services;
using MyJournal.Components.Models;
using MyJournal.Services.AuthService;
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
            builder.Services.AddFluentUIComponents();
            builder.Services.AddSingleton<ThemeService>();
            builder.Services.AddSingleton<MyJournal.Services.DatabaseService>();
            builder.Services.AddScoped<AuthSession>();
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddScoped<MyJournal.Services.ToastService>();



#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
