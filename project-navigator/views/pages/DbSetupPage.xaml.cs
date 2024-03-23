using System.Windows.Controls;
using project_navigator.view_models.pages;

namespace project_navigator.views.pages;

public partial class DbSetupPage : Page
{
    public DbSetupPage(DbSetupViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;
        InitializeComponent();
    }

    public DbSetupViewModel ViewModel { get; }
}