namespace blog.Services.Validation;

public class ValidationResult
{
    public bool IsValid => !_errors.Any();

    private readonly Dictionary<string, List<string>> _errors = new();

    public IReadOnlyDictionary<string, IReadOnlyList<string>> Errors => _errors
        .ToDictionary(kvp => kvp.Key, kvp => (IReadOnlyList<string>)kvp.Value);

    public ValidationResult()
    {
    }

    public void AddError(string name, string message)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentException.ThrowIfNullOrEmpty(message);
        
        if (_errors.TryGetValue(name, out var errorBuffer))
        {
            errorBuffer.Add(message);
        }
        else
        {
            _errors.Add(name, new List<string> { message });
        }
    }
}