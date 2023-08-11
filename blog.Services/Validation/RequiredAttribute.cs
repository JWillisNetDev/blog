namespace blog.Services.Validation;

public class RequiredAttribute : ValidationAttribute
{
    public override bool IsValid(object? value) => value is null;
    public override string FormatErrorMessage(string propertyName) => $"Value of `{propertyName}` cannot be null (was null)";
}