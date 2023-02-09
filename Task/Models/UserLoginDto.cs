using System.ComponentModel.DataAnnotations;

namespace Tasks.Models;

public class UserLoginDto
{
    [Required]
    // [DataType(DataType.Text)]
    public string? UserName { get; set; }

    [Required]
    // [DataType(DataType.Password)]
    public string? Password { get; set; }
    
}