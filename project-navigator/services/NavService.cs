using System.Windows.Controls;

namespace project_navigator.services;

public interface INavService
{
    void Navigate<T>() where T : Page;
    void SetServiceProvider(IServiceProvider serviceProvider);
    void SetFrame(Frame rootFrame);
}

public class NavService : INavService
{
    private Frame? _rootFrame;
    private IServiceProvider? _serviceProvider;

    public void Navigate<T>() where T : Page
    {
        var navPage = _serviceProvider.GetService(typeof(T));
        _rootFrame.Navigate(navPage);
    }

    public void SetServiceProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void SetFrame(Frame rootFrame)
    {
        _rootFrame = rootFrame;
    }
}