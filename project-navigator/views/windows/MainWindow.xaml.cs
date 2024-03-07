using System.Windows;
using System.Windows.Media;
using project_navigator.services;
using project_navigator.views.pages;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace project_navigator.views.windows;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : FluentWindow
{
    public MainWindow(INavService navService, IServiceProvider serviceProvider, ISnackbarService snackbarService)
    {
        InitializeComponent();

        navService.SetServiceProvider(serviceProvider);
        navService.SetFrame(RootFrame);
        navService.Navigate<AuthorizationPage>();
        snackbarService.SetSnackbarPresenter(SnackbarPresenter);

        ApplicationThemeManager.ApplySystemTheme();
        var logoColor = (SolidColorBrush?)Application.Current.Resources["LogoColor"];
        if (logoColor != null)
            ApplicationAccentColorManager.Apply(logoColor.Color);
    }
}