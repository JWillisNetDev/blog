using System.Text;

namespace blog.Services.Validation;

internal ref struct MessageTemplateHandler
{
    private readonly ReadOnlySpan<char> _template;

    private int _position = 0;

    public MessageTemplateHandler(ReadOnlySpan<char> template)
    {
        _template = template;
    }

    private char Current() => _position >= _template.Length ? '\0' : _template[_position];
    private char Peek() => _position + 1 >= _template.Length ? '\0' : _template[_position+1];
    private void Skip() => _position++;

    private ReadOnlySpan<char> ReadTemplateArgument()
    {
        int startPosition = _position;
        
        while (Peek() is not '}' and not '\0')
        {
            Skip();
        }
        Skip();
        if (Current() == '\0') throw new InvalidOperationException();
        
        return _template[startPosition.._position];
    }

    private ReadOnlySpan<char> ReadTemplateLiteral()
    {
        int startPosition = _position;
        while (Peek() is not '{' and not '\0')
        {
            Skip();
        }
        return _template[startPosition..(_position+1)];
    }
    
    public static string Interpolate(ReadOnlySpan<char> template, params object?[] arguments)
    {
        MessageTemplateHandler handler = new(template);
        Dictionary<string, string> cachedArguments = new();
        StringBuilder builder = new();
        int argumentIndex = 0;

        while (handler.Current() != '\0')
        {
            if (handler.Current() == '{')
            {
                handler.Skip();
                string arg = handler.ReadTemplateArgument().ToString();
                handler.Skip();

                if (cachedArguments.TryGetValue(arg, out string? cachedValue))
                {
                    builder.Append(cachedValue);
                }
                else
                {
                    string argument = arguments[argumentIndex++]?.ToString() ?? "null";
                    cachedArguments.Add(arg, argument);
                    builder.Append(argument);
                }
            }
            else
            {
                builder.Append(handler.ReadTemplateLiteral());
                handler.Skip();
            }
        }

        return builder.ToString();
    }
}