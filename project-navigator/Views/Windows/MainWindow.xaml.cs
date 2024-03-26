using System.Windows.Forms;
using System.Windows.Media;
using project_navigator.services;
using project_navigator.Services;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Application = System.Windows.Application;

namespace project_navigator.views.windows;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow(IServiceProvider serviceProvider,
        INavService navService, INotificationService notificationService)
    {
        SystemThemeWatcher.Watch(this, updateAccents: false);
        InitializeComponent();

        notificationService.SetInfoBarPresenter(InfoBarPresenter);
        navService.SetServiceProvider(serviceProvider);
        navService.SetFrame(RootFrame);
        var primaryBrush = (SolidColorBrush?)Application.Current.Resources["PrimaryBrush"];
        if (primaryBrush != null)
            ApplicationAccentColorManager.Apply(primaryBrush.Color);
    }
}