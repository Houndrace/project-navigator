using DbSetupViewModel = project_navigator.ViewModels.Pages.DbSetupViewModel;

namespace project_navigator.views.pages;

public partial class DbSetupPage
{
    public DbSetupPage(DbSetupViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;
        InitializeComponent();
    }

    public DbSetupViewModel ViewModel { get; }
}