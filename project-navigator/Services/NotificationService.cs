using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using Wpf.Ui;
using Wpf.Ui.Controls;
using MessageBox = System.Windows.Forms.MessageBox;

namespace project_navigator.Services;

public enum MessageType
{
    None,
    Error,
    Info,
    Warning,
    Success
}

public interface INotificationService
{
    void DisplayMessage(string title, string content, MessageType messageType = MessageType.None,
        TimeSpan? timeout = null);
}

public class NotificationService : INotificationService
{
    private readonly ISnackbarService _snackbarService;

    public NotificationService(ISnackbarService snackbarService)
    {
        _snackbarService = snackbarService;
    }

    public void DisplayMessage(string title, string content, MessageType messageType = MessageType.None,
        TimeSpan? timeout = null)
    {
        var icon = new SymbolIcon();
        var realTimeout = timeout ?? TimeSpan.FromSeconds(5);

        switch (messageType)
        {
            case MessageType.None:
                icon = null;
                break;
            case MessageType.Error:
                icon.Symbol = SymbolRegular.ErrorCircle20;
                break;
            case MessageType.Info:
                icon.Symbol = SymbolRegular.Info20;
                break;
            case MessageType.Warning:
                icon.Symbol = SymbolRegular.Warning20;
                break;
            case MessageType.Success:
                icon.Symbol = SymbolRegular.CheckmarkCircle20;
                break;
        }

        _snackbarService.Show(title, content, ControlAppearance.Secondary,
            icon, realTimeout);
    }
}