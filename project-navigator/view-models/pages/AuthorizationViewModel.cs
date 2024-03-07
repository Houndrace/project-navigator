using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

    public AuthorizationViewModel(IUserService userService, INavService navService, ISnackbarService snackbarService)
    {
        _userService = userService;
        _navService = navService;
        _snackbarService = snackbarService;
    }

    [RelayCommand]
    private void Authorize()
    {
        ValidateAllProperties();
        if (HasErrors)
        {
            // TODO:сделать надстройку над ObservableValidator
            var errorMessages = new List<string?>();

            var allErrors = GetErrors();
            foreach (var propertyErrors in allErrors)
                errorMessages.Add(propertyErrors.ErrorMessage);

            var allErrorMessages = string.Join("\n", errorMessages);

            _snackbarService.Show(
                "Ошибка авторизации",
                allErrorMessages,
                ControlAppearance.Danger,
                new SymbolIcon(SymbolRegular.ArrowSquareUpRight24),
                TimeSpan.FromSeconds(5)
            );
            return;
        }

        try
        {
            if (_userService.IsAuthorized(Username, Password))
                _navService.Navigate<HomePage>();
        }
        catch (InvalidOperationException e)
        {
            _snackbarService.Show(
                "Ошибка",
                e.Message,
                ControlAppearance.Danger,
                new SymbolIcon(SymbolRegular.ArrowSquareUpRight24),
                TimeSpan.FromSeconds(5)
            );
        }
    }
}