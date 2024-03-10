using System.ComponentModel.DataAnnotations;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using project_navigator.helpers;
using project_navigator.models;
using project_navigator.services;
using project_navigator.views.pages;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace project_navigator.view_models.pages;

public partial class RegistrationViewModel : ObservableValidator
{
    private readonly INavService _navService;
    private readonly ISnackbarService _snackbarService;
    private readonly IUserService _userService;
    private CancellationTokenSource _cts = new();

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

    [ObservableProperty] private Visibility _progressBarVisibility = Visibility.Hidden;

    public RegistrationViewModel(IUserService userService, INavService navService, ISnackbarService snackbarService)
    {
        _userService = userService;
        _navService = navService;
        _snackbarService = snackbarService;
    }

    [RelayCommand]
    private async Task<bool> Register()
    {
        ValidateAllProperties();
        if (HasErrors)
        {
            DisplayError("Неверные данные", ConcatenatePropertyErrors());
            return false;
        }

        ShowProgressBar();

        var regData = new RegistrationDto()
        {
            UserName = Username,
            Password = Password
        };

        try
        {
            await Task.Delay(TimeSpan.FromSeconds(5), _cts.Token);
            await _userService.Register(regData, _cts.Token);
        }
        catch (TaskCanceledException e)
        {
            Console.WriteLine($"{e.Task} - {e.Message}");
            return false;
        }
        catch (OperationCanceledException e)
        {
            Console.WriteLine($"Операция бд - {e.Message}");
            return false;
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

        DisplaySuccess("Успех", "Пользователь успешно зарегистрирован");
        _navService.Navigate<AuthorizationPage>();
        return true;
    }

    [RelayCommand]
    private void NavigateToAuthorization()
    {
        _cts.Cancel();
        _navService.Navigate<AuthorizationPage>();
    }

    private void ShowProgressBar()
    {
        ProgressBarVisibility = Visibility.Visible;
    }

    private void HideProgressBar()
    {
        ProgressBarVisibility = Visibility.Hidden;
    }

    private void DisplaySuccess(string title, string message)
    {
        _snackbarService.Show(title, message, ControlAppearance.Success,
            new SymbolIcon(SymbolRegular.CheckmarkCircle24), TimeSpan.FromSeconds(5));
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