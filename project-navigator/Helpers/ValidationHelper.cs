using System.ComponentModel.DataAnnotations;

namespace project_navigator.Helpers;

public static class ValidationHelper
{
    public static readonly string IncorrectDataTitle = "Неверные данные";
    public static readonly string SignInErrorTitle = "Ошибка авторизации";
    public static readonly string SignInErrorMessage = "Неверное имя пользователя или пароль";


    public static string ConcatenateErrors(IEnumerable<ValidationResult> errors)
    {
        var errorMessages = new List<string?>();

        foreach (var propertyErrors in errors)
            errorMessages.Add(propertyErrors.ErrorMessage);

        var allErrorMessages = string.Join("\n", errorMessages);
        return allErrorMessages;
    }
}