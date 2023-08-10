namespace blog.Services.Validation;

public record ValidationError(IReadOnlyList<string> Messages)
{
    public IReadOnlyList<string> Messages { get; } = Messages;
    
    public ValidationError(IEnumerable<string> messages) : this(messages.ToList().AsReadOnly())
    {
    }
}