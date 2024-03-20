using System.ComponentModel.DataAnnotations;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace project_navigator.helpers;

public class ValidatorHelper
{
    private readonly ISnackbarService _snackbarService;

    public ValidatorHelper(ISnackbarService snackbarService)
    {
        _snackbarService = snackbarService;
    }

    public string ConcatenateErrors(IEnumerable<ValidationResult> errors)
    {
        var errorMessages = new List<string?>();

        foreach (var propertyErrors in errors)
            errorMessages.Add(propertyErrors.ErrorMessage);

        var allErrorMessages = string.Join("\n", errorMessages);
        return allErrorMessages;
    }

    public void DisplayError(string title, string message)
    {
        _snackbarService.Show(title, message, ControlAppearance.Secondary,
            new SymbolIcon(SymbolRegular.ErrorCircle24), TimeSpan.FromSeconds(5));
    }

    public void DisplayCanceledError()
    {
        _snackbarService.Show("Уведомление", "Операция была отменена.", ControlAppearance.Secondary,
            new SymbolIcon(SymbolRegular.ErrorCircle24), TimeSpan.FromSeconds(5));
    }
}