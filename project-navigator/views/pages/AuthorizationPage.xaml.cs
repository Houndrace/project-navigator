using System.Windows.Controls;
using project_navigator.view_models.pages;

namespace project_navigator.views.pages;

public partial class AuthorizationPage : Page
{
    public AuthorizationPage(AuthorizationViewModel viewModel)
    {
        InitializeComponent();

        ViewModel = viewModel;
        DataContext = this;
    }

    public AuthorizationViewModel ViewModel { get; }
}