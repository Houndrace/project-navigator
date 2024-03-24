using project_navigator.ViewModels.Pages;

namespace project_navigator.Views.Pages;

public partial class SignPage
{
    public SignPage(SignViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;
        InitializeComponent();
    }

    public SignViewModel ViewModel { get; }
}