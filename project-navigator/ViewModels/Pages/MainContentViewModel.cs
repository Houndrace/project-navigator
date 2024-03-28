using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using project_navigator.services;
using project_navigator.Views.Pages;
using project_navigator.Views.Pages.MainContent;
using Wpf.Ui.Controls;

namespace project_navigator.ViewModels.Pages;

public partial class MainContentViewModel : ObservableObject
{
    private readonly INavService _navService;

    private static readonly NavigationViewItem ExitItem = new()
    {
        Content = "Выйти",
        Icon = new SymbolIcon(SymbolRegular.ArrowExit20)
    };

    [ObservableProperty] private ICollection<object> _footerMenuItems = new ObservableCollection<object>
    {
        new NavigationViewItem("Администрирование", SymbolRegular.PeopleToolbox20, typeof(DashboardPage)),
        //new NavigationViewItem("Настройки", SymbolRegular.Settings20, typeof(SettingsPage)),
        ExitItem
    };

    [ObservableProperty] private ICollection<object> _menuItems = new ObservableCollection<object>
    {
        new NavigationViewItem("Профиль", SymbolRegular.PersonCircle20, typeof(AccountPage)),
        new NavigationViewItemSeparator(),
        new NavigationViewItem("Главная", SymbolRegular.Home24, typeof(DashboardPage)),
        new NavigationViewItem("Проекты", SymbolRegular.TaskListLtr20, typeof(DashboardPage)),

        new NavigationViewItem("Клиенты", SymbolRegular.TextBulletListSquarePerson20, typeof(DashboardPage)),
        new NavigationViewItem("Отчетность", SymbolRegular.ChartMultiple20, typeof(DashboardPage))
    };

    public MainContentViewModel(INavService navService)
    {
        _navService = navService;
        ExitItem.Command = SignOutCommand;
    }

    [RelayCommand]
    private void SignOut()
    {
        _navService.Navigate<SignPage>();
    }
}