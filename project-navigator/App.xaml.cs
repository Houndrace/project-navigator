using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using project_navigator.db;
using project_navigator.helpers;
using project_navigator.services;
using project_navigator.view_models.pages;
using project_navigator.view_models.windows;
using project_navigator.views.pages;
using project_navigator.views.pages.MainContent;
using project_navigator.views.windows;
using Wpf.Ui;

namespace project_navigator;

//TODO: сделать отдельным окном сплеш скрин
/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private const string SplashScreenResource = "project_navigator.ico";
    private readonly IServiceProvider _serviceProvider;

    private App()
    {
        _serviceProvider = InitializeServiceProvider();
    }

    private static IServiceProvider InitializeServiceProvider()
    {
        IServiceCollection serviceCollection = new ServiceCollection();
        //TODO: подумать насчет интерфейсов(исключения можно посмотреть в реализации, не описывать их в интерфейсе, НО описывать в реализации)
        serviceCollection.AddSingleton<ValidatorHelper>();
        serviceCollection.AddSingleton<INavService, NavService>(); // Global navigation
        serviceCollection.AddSingleton<INavigationService, NavigationService>(); // Main content navigation
        serviceCollection.AddSingleton<IHashService, HashService>();
        serviceCollection.AddSingleton<IUserService, UserService>();
        serviceCollection.AddSingleton<ISnackbarService, SnackbarService>();
        serviceCollection.AddSingleton<IConfigurationService, ConfigurationService>();

        serviceCollection.AddDbContext<ProjNavContext>();

        serviceCollection.AddSingleton<MainWindow>();
        serviceCollection.AddSingleton<MainViewModel>();

        serviceCollection.AddTransient<DbSetupPage>();
        serviceCollection.AddTransient<DbSetupViewModel>();
        serviceCollection.AddTransient<AuthorizationPage>();
        serviceCollection.AddTransient<AuthorizationViewModel>();
        serviceCollection.AddTransient<MainContentPage>();
        serviceCollection.AddTransient<MainContentViewModel>();
        serviceCollection.AddTransient<HomePage>();
        serviceCollection.AddTransient<HomeViewModel>();

        return serviceCollection.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var mainWindow = _serviceProvider.GetService<MainWindow>();
        mainWindow.Show();
    }
}