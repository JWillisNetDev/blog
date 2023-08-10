using Microsoft.EntityFrameworkCore;
using blog.Data.Models;

namespace blog.Data;

public class BlogDbContext : DbContext
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
    {
    }

    public DbSet<Post> Posts => Set<Post>();

    public DbSet<User> Users => Set<User>();
}