using System.Windows.Controls;
using project_navigator.view_models.pages;

namespace project_navigator.views.pages;

public partial class InitialSetupPage : Page
{
    public InitialSetupPage(InitialSetupViewModel viewModel)
    {
        InitializeComponent();

        ViewModel = viewModel;
        DataContext = this;
    }

    public InitialSetupViewModel ViewModel { get; }
}