using System.ComponentModel.DataAnnotations;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using project_navigator.db;
using project_navigator.Helpers;
using project_navigator.services;
using project_navigator.Services;
using project_navigator.Views.Pages.MainContent;
using Serilog;
using Wpf.Ui.Controls;

namespace project_navigator.ViewModels.Pages;

public partial class SignViewModel : ObservableValidator
{
    private readonly INavService _navService;
    private readonly INotificationService _notificationService;
    private readonly AppDbContext _appDbContext;
    private readonly ISignService _signService;

    [ObservableProperty]
    [Required(ErrorMessage = "Необходимо ввести пароль")]
    [MinLength(5, ErrorMessage = "Минимальная длина пароля 5 знаков")]
    [MaxLength(50, ErrorMessage = "Максимальная длина пароля 50 знаков")]
    private string _password = "";

    [ObservableProperty] private Visibility _progressBarVisibility = Visibility.Hidden;

    [ObservableProperty]
    [Required(ErrorMessage = "Необходимо ввести имя пользователя")]
    [MinLength(5, ErrorMessage = "Минимальная длина имени пользователя 5 знаков")]
    [MaxLength(40, ErrorMessage = "Максимальная длина имени пользователя 40 знаков")]
    private string _username = "";

    public SignViewModel(ISignService signService, INavService navService, INotificationService notificationService,
        AppDbContext appDbContext)
    {
        _signService = signService;
        _navService = navService;
        _notificationService = notificationService;
        _appDbContext = appDbContext;

        InitializeConnectionAsync().Wait();
    }


    private Task InitializeConnectionAsync()
    {
        if (!_appDbContext.Database.CanConnectAsync().Result)
            _notificationService.OpenInfoBarAsync(ValidationHelper.DbConnectionErrorTitle,
                ValidationHelper.DbConnectionErrorMessage,
                InfoBarSeverity.Error);

        return Task.CompletedTask;
    }

    [RelayCommand]
    private async Task SignInAsync()
    {
        await _notificationService.CloseInfoBarAsync();
        ValidateAllProperties();
        if (HasErrors)
        {
            Log.Error("The user entered incorrect data");
            await _notificationService.OpenInfoBarAsync(ValidationHelper.IncorrectDataTitle,
                ValidationHelper.ConcatenateErrors(GetErrors()),
                InfoBarSeverity.Error);
            return;
        }

        try
        {
            ProgressBarVisibility = Visibility.Visible;

            if (!await _signService.SignInAsync(Username, Password))
            {
                await _notificationService.OpenInfoBarAsync(ValidationHelper.SignInErrorTitle,
                    ValidationHelper.SignInErrorMessage,
                    InfoBarSeverity.Error);
                return;
            }
        }
        catch (OperationCanceledException e)
        {
            Log.Error(e, "The Sign In operation was canceled");
            return;
        }
        catch (Exception e)
        {
            Log.Error(e, "A Sign In error occurred");
            await _notificationService.OpenInfoBarAsync(ValidationHelper.SignInErrorTitle,
                ValidationHelper.DbConnectionErrorMessage,
                InfoBarSeverity.Error);
            return;
        }
        finally
        {
            ProgressBarVisibility = Visibility.Hidden;
        }

        _navService.Navigate<MainContentPage>();
    }
}