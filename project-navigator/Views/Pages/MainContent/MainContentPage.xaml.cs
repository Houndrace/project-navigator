using project_navigator.services;
using Wpf.Ui;
using Wpf.Ui.Controls;
using MainContentViewModel = project_navigator.ViewModels.Pages.MainContentViewModel;

namespace project_navigator.Views.Pages.MainContent;

public partial class MainContentPage
{
    public MainContentPage(INavigationService mainContentNavService, IServiceProvider serviceProvider,
        MainContentViewModel viewModel, INavService navService)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
        //NavigationView.SelectedItem.Click += (sender, args) => navService.Navigate<>()
        mainContentNavService.SetNavigationControl(NavigationView);
        NavigationView.SetServiceProvider(serviceProvider);
    }

    public MainContentViewModel ViewModel { get; }
}