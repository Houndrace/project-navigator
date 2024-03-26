using System.Reflection;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using project_navigator.db;
using project_navigator.Helpers;
using project_navigator.services;
using project_navigator.Services;
using project_navigator.ViewModels.Pages;
using project_navigator.Views.Pages;
using project_navigator.Views.Pages.MainContent;
using project_navigator.views.windows;
using Serilog;
using Wpf.Ui;
using AppContext = project_navigator.db.AppContext;
using MainContentViewModel = project_navigator.ViewModels.Pages.MainContentViewModel;

namespace project_navigator;

//TODO: сделать отдельным окном сплеш скрин
public partial class App
{
    private static readonly IHost Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
        .ConfigureAppConfiguration(c =>
        {
            c.SetBasePath(System.AppContext.BaseDirectory);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateLogger();
        })
        .ConfigureServices(
            (_, services) =>
            {
                // App Host
                services.AddHostedService<ApplicationHostService>();

                // Main window container
                services.AddSingleton<MainWindow>();
                // SplashScreen window
                services.AddSingleton<SplashScreenWindow>();
                // Services and helpers
                services.AddSingleton<INavService, NavService>(); // Global navigation
                services.AddSingleton<INavigationService, NavigationService>(); // Main content navigation
                services.AddSingleton<IHashService, HashService>();
                services.AddSingleton<ISignService, SignService>();
                services.AddSingleton<ISnackbarService, SnackbarService>();
                services.AddSingleton<IConfigurationService, ConfigurationService>();
                services.AddSingleton<INotificationService, NotificationService>();
                services.AddSingleton<IDbDataInitializer, DbDataInitializer>();
                // Database
                services.AddDbContext<AppContext>();
                // Top-level pages
                services.AddSingleton<MainContentPage>();
                services.AddSingleton<MainContentViewModel>();
                services.AddSingleton<DashboardPage>();
                services.AddSingleton<DashboardViewModel>();

                // Other pages
                services.AddTransientFromNamespace(Assembly.GetExecutingAssembly(), "project_navigator.Views");
                services.AddTransientFromNamespace(Assembly.GetExecutingAssembly(), "project_navigator.ViewModels");
            }
        )
        .Build();

    private void OnStartup(object sender, StartupEventArgs e)
    {
        Host.StartAsync();
    }

    private void OnExit(object sender, ExitEventArgs e)
    {
        Host.StopAsync().Wait();

        Host.Dispose();
    }
}