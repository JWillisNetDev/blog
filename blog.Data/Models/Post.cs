using System.ComponentModel.DataAnnotations;

namespace blog.Data.Models;

public class Post : BaseEntity
{
    [Required, StringLength(128)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(8_000)]
    public string Content { get; set; } = string.Empty;

    [Required]
    public DateTimeOffset PostedOnUtc { get; set; }
}