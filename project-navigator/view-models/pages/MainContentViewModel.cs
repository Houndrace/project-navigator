using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using project_navigator.services;
using project_navigator.views.pages.MainContent;
using Wpf.Ui.Controls;

namespace project_navigator.view_models.pages;

public partial class MainContentViewModel : ObservableObject
{
    private readonly INavService _navService;

    public MainContentViewModel(INavService navService)
    {
        _navService = navService;
    }

    [ObservableProperty] private ICollection<object> _menuItems = new ObservableCollection<object>
    {
        new NavigationViewItem("Главная", SymbolRegular.Home24, typeof(HomePage)),
    };

    [ObservableProperty] private ICollection<object> _footerMenuItems = new ObservableCollection<object>()
    {
        new NavigationViewItem("Settings", SymbolRegular.Settings24, null)
    };
}