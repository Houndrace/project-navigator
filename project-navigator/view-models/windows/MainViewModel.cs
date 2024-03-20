using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using project_navigator.db;
using project_navigator.services;
using project_navigator.views.pages;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace project_navigator.view_models.windows;

public class MainViewModel
{
    private readonly INavService _navService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ISnackbarService _snackbarService;
    private readonly IConfigurationService _configurationService;
    private readonly ProjNavContext _dbContext;


    public MainViewModel(INavService navService,
        IServiceProvider serviceProvider,
        ISnackbarService snackbarService,
        IConfigurationService configurationService,
        ProjNavContext projNavContext)
    {
        _navService = navService;
        _serviceProvider = serviceProvider;
        _snackbarService = snackbarService;
        _configurationService = configurationService;
        _dbContext = projNavContext;
    }

    public Frame? RootFrame { get; set; }
    public SnackbarPresenter? SnackbarPresenter { get; set; }

    public void NavigateToInitialPage()
    {
        if (_configurationService.IsConfigExists())
        {
            _navService.Navigate<AuthorizationPage>();
        }
        else
        {
            _navService.Navigate<InitialSetupPage>();
        }
        /*try
        {
            // TODO:перенести логику на initialSetupPage и AuthorizationPage
            // TODO:подумать можно ли запрятать эту логику куда нибудь(она здесь не очень выглядит)
            if (!await _dbContext.Database.CanConnectAsync())
                await _dbContext.Database.MigrateAsync();
            ////////////////////////////////////////////////////////////////////////////////////////
            _navService.Navigate<AuthorizationPage>();
        }
        catch (Exception e) when (e is ArgumentException or InvalidOperationException or SqlException)
        {
            Console.WriteLine($"{e.Source}: {e.Message}");
            _navService.Navigate<InitialSetupPage>();
        }*/
    }

    public void ConfigureServices()
    {
        ConfigureNavService();
        ConfigureSnackbarService();
        ConfigureTheme();
    }

    private void ConfigureNavService()
    {
        ArgumentNullException.ThrowIfNull(RootFrame);

        _navService.SetServiceProvider(_serviceProvider);
        _navService.SetFrame(RootFrame);
    }

    private void ConfigureSnackbarService()
    {
        ArgumentNullException.ThrowIfNull(SnackbarPresenter);
        _snackbarService.SetSnackbarPresenter(SnackbarPresenter);
    }

    private void ConfigureTheme()
    {
        var logoColor = (SolidColorBrush?)Application.Current.Resources["LogoColor"];
        if (logoColor != null)
            ApplicationAccentColorManager.Apply(logoColor.Color);
    }
}