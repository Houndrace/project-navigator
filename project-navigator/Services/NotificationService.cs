using Wpf.Ui.Controls;

namespace project_navigator.Services;

public interface INotificationService
{
    void SetInfoBarPresenter(InfoBar infoBar);
    void OpenInfoBar(string title, string message,
        InfoBarSeverity severity = InfoBarSeverity.Informational,
        TimeSpan? timeout = null);

    void CloseInfoBar();
}

public class NotificationService : INotificationService
{
    private InfoBar? _infoBarPrersenter;

    public void SetInfoBarPresenter(InfoBar infoBar)
    {
        _infoBarPrersenter = infoBar;
    }

    public void OpenInfoBar(string title, string message,
        InfoBarSeverity severity = InfoBarSeverity.Informational,
        TimeSpan? timeout = null)
    {
        ArgumentNullException.ThrowIfNull(_infoBarPrersenter);
        //var realTimeout = timeout ?? TimeSpan.FromSeconds(5);
        _infoBarPrersenter.Title = title;
        _infoBarPrersenter.Message = message;
        _infoBarPrersenter.Severity = severity;
        _infoBarPrersenter.IsOpen = true;
    }

    public void CloseInfoBar()
    {
        ArgumentNullException.ThrowIfNull(_infoBarPrersenter);

        _infoBarPrersenter.IsOpen = false;
    }
}