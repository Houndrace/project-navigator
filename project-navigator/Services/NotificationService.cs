using Wpf.Ui.Controls;

namespace project_navigator.Services;

public interface INotificationService
{
    void SetInfoBarPresenter(InfoBar infoBar);

    Task OpenInfoBarAsync(string title, string message,
        InfoBarSeverity severity = InfoBarSeverity.Informational,
        TimeSpan? timeout = null);

    Task CloseInfoBarAsync();
}

public class NotificationService : INotificationService
{
    private InfoBar? _infoBarPrersenter;

    public void SetInfoBarPresenter(InfoBar infoBar)
    {
        _infoBarPrersenter = infoBar;
    }

    public async Task OpenInfoBarAsync(string title, string message,
        InfoBarSeverity severity = InfoBarSeverity.Informational,
        TimeSpan? timeout = null)
    {
        ArgumentNullException.ThrowIfNull(_infoBarPrersenter);
        //var realTimeout = timeout ?? TimeSpan.FromSeconds(5);
        _infoBarPrersenter.Title = title;
        _infoBarPrersenter.Message = message;
        _infoBarPrersenter.Severity = severity;

        await Task.Delay(500);
        _infoBarPrersenter.IsOpen = true;
    }

    public Task CloseInfoBarAsync()
    {
        ArgumentNullException.ThrowIfNull(_infoBarPrersenter);

        _infoBarPrersenter.IsOpen = false;
        return Task.CompletedTask;
    }
}