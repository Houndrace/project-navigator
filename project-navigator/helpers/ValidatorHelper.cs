using System.ComponentModel.DataAnnotations;

namespace project_navigator.helpers;

public class ValidatorHelper
{
    public string ConcatenateErrors(IEnumerable<ValidationResult> errors)
    {
        var errorMessages = new List<string?>();

        foreach (var propertyErrors in errors)
            errorMessages.Add(propertyErrors.ErrorMessage);

        var allErrorMessages = string.Join("\n", errorMessages);
        return allErrorMessages;
    }
}