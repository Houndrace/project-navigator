using System.ComponentModel.DataAnnotations;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using project_navigator.helpers;
using project_navigator.services;
using project_navigator.views.pages;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace project_navigator.view_models.pages;

public partial class AuthorizationViewModel : ObservableValidator
{
    private readonly INavService _navService;
    private readonly ISnackbarService _snackbarService;
    private readonly IUserService _userService;

    [ObservableProperty]
    [Required(ErrorMessage = "Имя пользователя обязателено")]
    [MinLength(5, ErrorMessage = "Минимальная длина имени пользователя 5 знаков")]
    [MaxLength(40, ErrorMessage = "Максимальная длина имени пользователя 40 знаков")]
    private string _username = "";

    [ObservableProperty]
    [Required(ErrorMessage = "Пароль обязателен")]
    [MinLength(8, ErrorMessage = "Минимальная длина пароля 8 знаков")]
    [MaxLength(50, ErrorMessage = "Максимальная длина пароля 50 знаков")]
    private string _password = "";

    [ObservableProperty] private Visibility _progressBarVisibility = Visibility.Hidden;

    public AuthorizationViewModel(IUserService userService, INavService navService, ISnackbarService snackbarService)
    {
        _userService = userService;
        _navService = navService;
        _snackbarService = snackbarService;
    }

    [RelayCommand]
    private async Task<bool> Authorize()
    {
        ValidateAllProperties();
        if (HasErrors)
        {
            DisplayError("Неверные данные", ConcatenatePropertyErrors());
            return false;
        }

        ShowProgressBar();
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
            if (!await _userService.IsAuthorized(Username, Password))
            {
                DisplayError("Ошибка авторизации", "Неверное имя пользователя или пароль");
                return false;
            }

            NavigateToHomePage();
        }
        catch (Exception e)
        {
            DisplayError("Ошибка", e.Message);
            return false;
        }
        finally
        {
            HideProgressBar();
        }

        return true;
    }

    private void ShowProgressBar()
    {
        ProgressBarVisibility = Visibility.Visible;
    }

    private void HideProgressBar()
    {
        ProgressBarVisibility = Visibility.Hidden;
    }

    private void NavigateToHomePage()
    {
        _navService.Navigate<HomePage>();
    }

    private void DisplayError(string title, string message)
    {
        _snackbarService.Show(title, message, ControlAppearance.Secondary,
            new SymbolIcon(SymbolRegular.ErrorCircle24), TimeSpan.FromSeconds(5));
    }

    private string ConcatenatePropertyErrors()
    {
        return new ValidatorHelper().ConcatenateErrors(GetErrors());
    }
}