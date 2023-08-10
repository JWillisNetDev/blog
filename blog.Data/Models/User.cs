using System.ComponentModel.DataAnnotations;

namespace blog.Data.Models;

public class User : BaseEntity
{
    [Required, StringLength(40)]
    public string UserName { get; set; }

    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
    
    [EmailAddress]
    public string? Email { get; set; }
}