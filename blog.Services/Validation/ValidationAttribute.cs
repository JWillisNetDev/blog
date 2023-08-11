namespace blog.Services.Validation;

[AttributeUsage(AttributeTargets.Property)]
public abstract class ValidationAttribute : Attribute
{
    protected string Interpolate(string template, params object?[] arguments) => MessageTemplateHandler.Interpolate(template, arguments);
    
    public abstract bool IsValid(object? value);
    public abstract string FormatErrorMessage(string propertyName);
}