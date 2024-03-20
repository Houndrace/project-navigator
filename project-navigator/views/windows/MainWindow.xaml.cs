using System.Windows;
using System.Windows.Media;
using Microsoft.Extensions.DependencyInjection;
using project_navigator.services;
using project_navigator.view_models.windows;
using project_navigator.views.pages;
using Wpf.Ui;
using Wpf.Ui.Appearance;

namespace project_navigator.views.windows;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow(MainViewModel mainViewModel)
    {
        InitializeComponent();

        mainViewModel.RootFrame = RootFrame;
        mainViewModel.SnackbarPresenter = SnackbarPresenter;
        mainViewModel.ConfigureServices();
        mainViewModel.NavigateToInitialPage();
    }
}