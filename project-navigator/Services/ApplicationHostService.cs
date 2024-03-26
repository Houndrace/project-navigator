using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using project_navigator.db;
using project_navigator.Views.Pages;
using project_navigator.views.windows;
using Serilog;

namespace project_navigator.services;

public class ApplicationHostService : IHostedService
{
    private readonly IDbDataInitializer _dataInitializer;
    private readonly INavService _navService;
    private readonly IServiceProvider _serviceProvider;

    public ApplicationHostService(IServiceProvider serviceProvider, INavService navService,
        IDbDataInitializer dataInitializer)
    {
        _serviceProvider = serviceProvider;
        _navService = navService;
        _dataInitializer = dataInitializer;
    }

    /// <summary>
    ///     Triggered when the application host is ready to start the service.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return HandleActivationAsync();
    }

    /// <summary>
    ///     Triggered when the application host is performing a graceful shutdown.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    ///     Creates main window during activation.
    /// </summary>
    private Task HandleActivationAsync()
    {
        if (Application.Current.Windows.OfType<MainWindow>().Any()) return Task.CompletedTask;

        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();

        // TODO: Реализовать первый запуск после установки
        //_navService.Navigate<MainContentPage>();
        _navService.Navigate<SignPage>();
        mainWindow.Show();
        return Task.CompletedTask;
    }
}