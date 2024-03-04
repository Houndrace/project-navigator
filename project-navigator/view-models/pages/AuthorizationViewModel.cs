using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using project_navigator.services;

namespace project_navigator.view_models.pages;

public partial class AuthorizationViewModel : ObservableObject
{
    private readonly IHashService _hashService;
    [ObservableProperty] private string _password = "";
    [ObservableProperty] private string _username = "";

    public AuthorizationViewModel(IHashService hashService)
    {
        _hashService = hashService;
    }

    [RelayCommand]
    private void Authorize()
    {
        var f = _hashService.HashString(Password);
    }
}