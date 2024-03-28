using project_navigator.ViewModels.Pages;

namespace project_navigator.Views.Pages.MainContent;

public partial class AccountPage
{
    public AccountPage(AccountViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;
        InitializeComponent();
    }

    public AccountViewModel ViewModel { get; }
}