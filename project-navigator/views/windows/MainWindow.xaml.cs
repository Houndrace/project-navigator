using Wpf.Ui.Appearance;

namespace project_navigator.views.windows;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        ApplicationThemeManager.ApplySystemTheme();
    }
}