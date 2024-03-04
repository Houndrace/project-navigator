using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using project_navigator.view_models.pages;
using project_navigator.views.pages;
using project_navigator.views.windows;

namespace project_navigator;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly ServiceProvider _serviceProvider;

    private App()
    {
        IServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<MainWindow>();

        serviceCollection.AddTransient<AuthorizationPage>();
        serviceCollection.AddTransient<AuthorizationViewModel>();

        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var mainWindow = _serviceProvider.GetService<MainWindow>();
        mainWindow.Show();
        base.OnStartup(e);
    }
}