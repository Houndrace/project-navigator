using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using project_navigator.db;
using project_navigator.helpers;
using project_navigator.services;
using project_navigator.views.pages;

namespace project_navigator.view_models.pages;

public partial class InitialSetupViewModel : ObservableObject
{
    private readonly ValidatorHelper _validatorHelper;
    private readonly INavService _navService;
    private readonly IConfigurationService _configurationService;
    private readonly ProjNavContext _dbContext;
    [ObservableProperty] private string _server = "";
    [ObservableProperty] private string _db = "";
    [ObservableProperty] private Visibility _progressBarVisibility = Visibility.Hidden;

    public InitialSetupViewModel(ValidatorHelper validatorHelper, INavService navService,
        IConfigurationService configurationService, ProjNavContext dbContext)
    {
        _validatorHelper = validatorHelper;
        _navService = navService;
        _configurationService = configurationService;
        _dbContext = dbContext;
    }

    [RelayCommand]
    private async Task<bool> ConnectToDb()
    {
        ProgressBarVisibility = Visibility.Visible;
        await Task.Delay(TimeSpan.FromSeconds(5));
        try
        {
            var connectionString =
                $"Server={Server};Database={Db};Trusted_Connection=True;TrustServerCertificate=True;";
            _configurationService.CreateConfig(connectionString);

            _dbContext.Database.SetConnectionString(_configurationService.GetConnectionString());

            if (!await _dbContext.Database.CanConnectAsync())
            {
                _validatorHelper.DisplayError("Ошибка подключения", "Проверьте название сервера и базы данных");
                return false;
            }

            await _dbContext.Database.MigrateAsync();
        }
        catch (OperationCanceledException e)
        {
            _validatorHelper.DisplayCanceledError();
            return false;
        }
        finally
        {
            ProgressBarVisibility = Visibility.Hidden;
        }

        _navService.Navigate<AuthorizationPage>();
        return true;
    }
}