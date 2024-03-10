using System.Windows.Controls;
using project_navigator.view_models.pages;

namespace project_navigator.views.pages;

public partial class RegistrationPage : Page
{
    public RegistrationPage(RegistrationViewModel viewModel)
    {
        InitializeComponent();

        ViewModel = viewModel;
        DataContext = this;
    }

    public RegistrationViewModel ViewModel { get; }
}