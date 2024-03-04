using project_navigator.services;
using project_navigator.views.pages;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace project_navigator.views.windows;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : FluentWindow
{
    public MainWindow(INavService navService, IServiceProvider serviceProvider)
    {
        InitializeComponent();

        navService.SetServiceProvider(serviceProvider);
        navService.SetFrame(RootFrame);
        navService.Navigate<AuthorizationPage>();
        ApplicationThemeManager.ApplySystemTheme();
    }
}