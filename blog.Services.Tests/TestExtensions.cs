using blog.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace blog.Services.Tests;

internal static class TestExtensions
{
    public static AutoMocker WithInMemoryDatabase(this AutoMocker mocker)
    {
        mocker.With<IDbContextFactory<BlogDbContext>, TestDbContextFactory>();
        return mocker;
    }

    public static AutoMocker WithDateTimeProvider(this AutoMocker mocker, DateTimeOffset offset)
    {
        mocker.With<IDateTimeProvider>();
        mocker.GetMock<IDateTimeProvider>()
            .Setup(dt => dt.GetUtcNow())
            .Returns(offset);
        return mocker;
    }
}