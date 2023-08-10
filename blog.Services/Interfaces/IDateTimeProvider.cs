namespace blog.Services.Interfaces;

public interface IDateTimeProvider
{
    public DateTimeOffset GetUtcNow();
}