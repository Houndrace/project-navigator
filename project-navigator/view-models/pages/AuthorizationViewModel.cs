using CommunityToolkit.Mvvm.ComponentModel;

namespace project_navigator.view_models.pages;

public partial class AuthorizationViewModel : ObservableObject
{
    [ObservableProperty] private string _password;
    [ObservableProperty] private string _username;
}