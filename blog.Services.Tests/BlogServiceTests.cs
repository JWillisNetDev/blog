using blog.Services.Interfaces;
using blog.Services.Validation;
using Microsoft.EntityFrameworkCore;

namespace blog.Services.Tests;

public class BlogServiceTests : IDisposable
{
    private AutoMocker Mocker { get; }

    public BlogServiceTests()
    {
        Mocker = new AutoMocker().WithInMemoryDatabase();
    }
    
    [Fact]
    public async Task CreatePost_CreatesPostIntoDatabase()
    {
        // Arrange
        DateTimeOffset expectedOffset = new DateTimeOffset(2023, 1, 1, 12, 0, 0, TimeSpan.Zero);
        Mocker.Setup<IDateTimeProvider, DateTimeOffset>(dtp => dtp.GetUtcNow())
            .Returns(expectedOffset);

        Post expectedPost = new Post()
        {
            Title = "Hello, world!",
            Content = "This is a test post.",
        };
        
        BlogService sut = Mocker.CreateInstance<BlogService>();
        
        // Act
        Result<Post, ValidationError> result = await sut.CreatePost(expectedPost);
        
        // Assert
        result.Match(actual =>
        {
            Assert.Equal(1, actual.Id);
            Assert.True(Guid.TryParse(actual.PublicId, out _));
            Assert.Equal(expectedPost.Title, actual.Title);
            Assert.Equal(expectedPost.Content, actual.Content);
            Assert.Equal(expectedOffset, actual.PostedOnUtc);
        }, e => Assert.Fail(string.Join(Environment.NewLine, e.Messages)));
    }
    public void Dispose()
    {
        ((IDisposable)Mocker.Get<IDbContextFactory<BlogDbContext>>()).Dispose();
        GC.SuppressFinalize(this);
    }
}