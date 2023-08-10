using blog.Data;
using blog.Data.Models;
using blog.Services.Interfaces;
using blog.Services.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace blog.Services;

public class BlogService
{
    private readonly IDbContextFactory<BlogDbContext> _dbFactory;
    private readonly IDateTimeProvider _dateTimeProvider;

    public BlogService(IDbContextFactory<BlogDbContext> dbFactory,
        IDateTimeProvider dateTimeProvider)
    {
        _dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
        _dateTimeProvider = dateTimeProvider ?? throw new ArgumentException(nameof(dateTimeProvider));
    }
    
    public async Task<Result<Post, ValidationError>> CreatePost(Post post)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        post.PostedOnUtc = _dateTimeProvider.GetUtcNow();
        EntityEntry<Post> entry = await db.Posts.AddAsync(post);
        await db.SaveChangesAsync();
        return entry.Entity;
    }
}