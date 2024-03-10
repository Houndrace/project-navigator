using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using project_navigator.services;
using project_navigator.views.pages;

namespace project_navigator.view_models.pages;

public partial class HomeViewModel : ObservableObject
{
    private readonly INavService _navService;

    public HomeViewModel(INavService navService)
    {
        _navService = navService;
    }

    [RelayCommand]
    private void NavigateToAuthorization()
    {
        _navService.Navigate<AuthorizationPage>();
    }
}