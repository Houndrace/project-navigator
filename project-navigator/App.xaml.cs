using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using project_navigator.db;
using project_navigator.services;
using project_navigator.view_models.pages;
using project_navigator.views.pages;
using project_navigator.views.windows;
using Wpf.Ui;

namespace project_navigator;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;

    private App()
    {
        ShowSplashScreen("project-navigator.ico");
        _serviceProvider = InitializeServiceProvider();
    }
    private void ShowSplashScreen(string splashScreenResource)
    {
        var splashScreen = new SplashScreen(splashScreenResource);
        splashScreen.Show(true);
    }

    private IServiceProvider InitializeServiceProvider()
    {
        IServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<MainWindow>();

        serviceCollection.AddDbContext<ProjNavContext>();

        serviceCollection.AddSingleton<INavService, NavService>();
        serviceCollection.AddSingleton<IHashService, HashService>();
        serviceCollection.AddSingleton<IUserService, UserService>();
        serviceCollection.AddSingleton<ISnackbarService, SnackbarService>();

        serviceCollection.AddTransient<AuthorizationPage>();
        serviceCollection.AddTransient<AuthorizationViewModel>();
        serviceCollection.AddTransient<HomePage>();
        serviceCollection.AddTransient<HomeViewModel>();

        return serviceCollection.BuildServiceProvider();
    }


    protected override void OnStartup(StartupEventArgs e)
    {

        var dbContext = _serviceProvider.GetService<ProjNavContext>();
        if (!dbContext.Database.CanConnect())
            dbContext.Database.Migrate();

        var mainWindow = _serviceProvider.GetService<MainWindow>();
        mainWindow.Show();
        base.OnStartup(e);
    }
}