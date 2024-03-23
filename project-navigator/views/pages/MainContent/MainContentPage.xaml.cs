using System.Collections.ObjectModel;
using System.Windows.Controls;
using project_navigator.view_models.pages;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace project_navigator.views.pages.MainContent;

public partial class MainContentPage : Page
{
    public MainContentPage(INavigationService mainContentNavService, IServiceProvider serviceProvider,
        MainContentViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();

        mainContentNavService.SetNavigationControl(NavigationView);
        NavigationView.SetServiceProvider(serviceProvider);
    }

    public MainContentViewModel ViewModel { get; }
}