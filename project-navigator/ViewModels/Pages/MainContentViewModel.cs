using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using project_navigator.services;
using project_navigator.Views.Pages.MainContent;
using Wpf.Ui.Controls;

namespace project_navigator.ViewModels.Pages;

public partial class MainContentViewModel : ObservableObject
{
    private readonly INavService _navService;

    [ObservableProperty] private ICollection<object> _footerMenuItems = new ObservableCollection<object>
    {
        new NavigationViewItem("Settings", SymbolRegular.Settings24, null)
    };

    [ObservableProperty] private ICollection<object> _menuItems = new ObservableCollection<object>
    {
        new NavigationViewItem("Главная", SymbolRegular.Home24, typeof(DashboardPage))
    };

    public MainContentViewModel(INavService navService)
    {
        _navService = navService;
    }
}