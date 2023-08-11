using System.Runtime.CompilerServices;

namespace blog.Services.Validation;

public class StringLengthAttribute : ValidationAttribute
{
    internal const string CannotBeNullMessageTemplate = "Value of `{Value of `propertyName}` cannot be null (was: null)";
    internal const string OverMaxLengthMessageTemplate = "Value of `{propertyName}` cannot be greater than {maxLength} (was: {length})";
    internal const string BelowMinLengthMessageTemplate = "Length of `{propertyName}` is less than {MinLength} (was {_stringLength})";
    
    private enum StringLengthValidationErrorType
    {
        None = 0,
        OverMaxLength, BelowMinLength, Null,
    }

    private StringLengthValidationErrorType _validationErrorType = StringLengthValidationErrorType.None;
    private int _stringLength = 0;
    
    public int MaxLength { get; }
    public int MinLength { get; init; } = 0;
    public bool AllowNull { get; init; } = true;
    
    public StringLengthAttribute(int maxLength)
    {
        MaxLength = maxLength;
    }
    
    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            _validationErrorType = AllowNull ?
                StringLengthValidationErrorType.None :
                StringLengthValidationErrorType.Null;
            return AllowNull;
        }
        
        if (value is not string stringValue) throw new InvalidOperationException();

        _stringLength = stringValue.Length;
        if (_stringLength > MaxLength)
        {
            _validationErrorType = StringLengthValidationErrorType.OverMaxLength;
            return false;
        }
        
        if (_stringLength < MinLength)
        {
            _validationErrorType = StringLengthValidationErrorType.BelowMinLength;
            return false;
        }
        
        return true;
    }

    public override string FormatErrorMessage(string propertyName) => _validationErrorType switch
    {
        StringLengthValidationErrorType.Null => Interpolate(CannotBeNullMessageTemplate, propertyName),
        StringLengthValidationErrorType.OverMaxLength => Interpolate(OverMaxLengthMessageTemplate, propertyName, MaxLength, _stringLength),
        StringLengthValidationErrorType.BelowMinLength => Interpolate(BelowMinLengthMessageTemplate,propertyName, MinLength, _stringLength),
        _ => string.Empty,
    };
}