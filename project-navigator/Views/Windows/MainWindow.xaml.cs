using System.Windows;
using System.Windows.Media;
using project_navigator.services;
using Wpf.Ui;
using Wpf.Ui.Appearance;

namespace project_navigator.views.windows;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow(IServiceProvider serviceProvider,
        INavService navService,
        ISnackbarService snackbarService)
    {
        InitializeComponent();

        navService.SetServiceProvider(serviceProvider);
        navService.SetFrame(RootFrame);
        snackbarService.SetSnackbarPresenter(SnackbarPresenter);

        var primaryBrush = (SolidColorBrush?)Application.Current.Resources["PrimaryBrush"];
        if (primaryBrush != null)
            ApplicationAccentColorManager.Apply(primaryBrush.Color);
    }
}