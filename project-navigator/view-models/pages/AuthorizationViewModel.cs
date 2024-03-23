using System.ComponentModel.DataAnnotations;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using project_navigator.db;
using project_navigator.helpers;
using project_navigator.services;
using project_navigator.views.pages;
using project_navigator.views.pages.MainContent;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace project_navigator.view_models.pages;

public partial class AuthorizationViewModel : ObservableValidator
{
    private readonly INavService _navService;
    private readonly ValidatorHelper _validatorHelper;
    private readonly IUserService _userService;

    [ObservableProperty]
    [Required(ErrorMessage = "Необходимо ввести имя пользователя")]
    [MinLength(5, ErrorMessage = "Минимальная длина имени пользователя 5 знаков")]
    [MaxLength(40, ErrorMessage = "Максимальная длина имени пользователя 40 знаков")]
    private string _username = "";

    [ObservableProperty]
    [Required(ErrorMessage = "Необходимо ввести пароль")]
    [MinLength(8, ErrorMessage = "Минимальная длина пароля 8 знаков")]
    [MaxLength(50, ErrorMessage = "Максимальная длина пароля 50 знаков")]
    private string _password = "";

    [ObservableProperty] private string _connectionStatus = "Не подключено";
    [ObservableProperty] private SymbolRegular _connectionIcon = SymbolRegular.DatabaseWarning20;

    [ObservableProperty] private Visibility _progressBarVisibility = Visibility.Hidden;

    public AuthorizationViewModel(IUserService userService, INavService navService, ValidatorHelper validatorHelper,
        ProjNavContext projNavContext)
    {
        _userService = userService;
        _navService = navService;
        _validatorHelper = validatorHelper;

        var connectionTask = projNavContext.Database.CanConnectAsync();

        if (connectionTask.Result)
        {
            ConnectionStatus = "Подключено";
            ConnectionIcon = SymbolRegular.Database20;
        }
    }

    [RelayCommand]
    private async Task<bool> Authorize()
    {
        ValidateAllProperties();
        if (HasErrors)
        {
            _validatorHelper.DisplayError("Неверные данные", _validatorHelper.ConcatenateErrors(GetErrors()));
            return false;
        }

        ProgressBarVisibility = Visibility.Visible;
        await Task.Delay(TimeSpan.FromSeconds(5));
        try
        {
            if (!await _userService.AuthorizeAsync(Username, Password))
            {
                _validatorHelper.DisplayError("Ошибка авторизации", "Неверное имя пользователя или пароль");
                return false;
            }

            _navService.Navigate<MainContentPage>();
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

        return true;
    }
}