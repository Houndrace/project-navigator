namespace project_navigator.view_models.pages;

public class HomeViewModel
{
    
public partial class HomeViewModel : ObservableObject
{
    private readonly INavService _navService;

    public HomeViewModel(INavService navService)
    {
        _navService = navService;
    }
}