using Wpf.Ui;
using MainContentViewModel = project_navigator.ViewModels.Pages.MainContentViewModel;

namespace project_navigator.Views.Pages.MainContent;

public partial class MainContentPage
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