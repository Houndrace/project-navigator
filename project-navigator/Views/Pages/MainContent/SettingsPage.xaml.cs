using System.Windows.Controls;
using project_navigator.ViewModels.Pages;

namespace project_navigator.Views.Pages.MainContent;

public partial class SettingsPage : Page
{
    public SettingsPage(SettingsViewModel settingsViewModel)
    {
        ViewModel = settingsViewModel;
        DataContext = this;
        InitializeComponent();
    }

    public SettingsViewModel ViewModel { get; }
}