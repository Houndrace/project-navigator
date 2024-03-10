using CommunityToolkit.Mvvm.ComponentModel;
using project_navigator.services;

namespace project_navigator.view_models.pages;

public partial class HomeViewModel : ObservableObject
{
    private readonly INavService _navService;

    public HomeViewModel(INavService navService)
    {
        _navService = navService;
    }
}