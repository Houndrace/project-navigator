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
            await Task.Delay(TimeSpan.FromSeconds(5), _cts.Token);
            if (!await _userService.Authorize(Username, Password, _cts.Token))
            {
                DisplayError("Ошибка авторизации", "Неверное имя пользователя или пароль");
                return false;
            }

            _navService.Navigate<HomePage>();
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

        return true;
    }

    [RelayCommand]
    private void NavigateToRegistration()
    {
        _cts.Cancel();
        _navService.Navigate<RegistrationPage>();
    }

    private void ShowProgressBar()
    {
        ProgressBarVisibility = Visibility.Visible;
    }

    private void HideProgressBar()
    {
        ProgressBarVisibility = Visibility.Hidden;
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