using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace blog.Services.Validation;

public class Validator<T>
    where T : class
{
    public ValidationResult Validate(T @object)
    {
        ValidationResult result = new();
        
        var properties = typeof(T).GetProperties();
        foreach (var property in properties)
        {
            var attributes = property.GetCustomAttributes<ValidationAttribute>().ToList();
            if (attributes.Count == 0) continue;

            object? value = property.GetValue(@object);
            foreach (var attribute in attributes)
            {
                if (!attribute.IsValid(value))
                {
                    result.AddError(property.Name, attribute.FormatErrorMessage(property.Name));
                }
            }
        }
        
        return result;
    }
    
}