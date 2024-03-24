using project_navigator.ViewModels.Pages;

namespace project_navigator.Views.Pages.MainContent;

public partial class DashboardPage
{
    public DashboardPage(DashboardViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;
        InitializeComponent();
    }

    public DashboardViewModel ViewModel { get; }
}