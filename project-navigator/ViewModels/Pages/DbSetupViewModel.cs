using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using project_navigator.db;
using project_navigator.services;
using project_navigator.Views.Pages;

namespace project_navigator.ViewModels.Pages;

//TODO:Сделать отдельную привественную страницу, а сюда редиректить из изменить подключение
public partial class DbSetupViewModel : ObservableObject
{
    private readonly IConfigurationService _configurationService;
    private readonly AppDbContext _dbDbContext;
    private readonly INavService _navService;
    [ObservableProperty] private string _db = "";
    [ObservableProperty] private Visibility _progressBarVisibility = Visibility.Hidden;
    [ObservableProperty] private string _server = "";

    public DbSetupViewModel(INavService navService,
        IConfigurationService configurationService, AppDbContext dbDbContext)
    {
        _navService = navService;
        _configurationService = configurationService;
        _dbDbContext = dbDbContext;
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
            //if (!_configurationService.IsConfigExists()) _configurationService.CreateConfig(connectionString);

            _dbDbContext.Database.SetConnectionString(_configurationService.GetConnectionString());

            await _dbDbContext.Database.MigrateAsync();
            //_configurationService.SaveConfig();

            _navService.Navigate<SignPage>();
            return true;
        }
        catch (OperationCanceledException e)
        {
            Console.WriteLine(e);
            //_validatorHelper.DisplayCanceledError();
            return false;
        }
        finally
        {
            ProgressBarVisibility = Visibility.Hidden;
        }
    }
}