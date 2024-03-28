using CommunityToolkit.Mvvm.ComponentModel;
using project_navigator.models;
using project_navigator.services;


namespace project_navigator.ViewModels.Pages;

public partial class AccountViewModel : ObservableValidator
{
    [ObservableProperty] private User? _signedInUser;

    public AccountViewModel(ISignService signService)
    {
        SignedInUser = signService.LastSignedInUser;
    }
}