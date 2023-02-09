using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Tasks.Models;

public class UserCreateDto
{             
    [Required]
    [DataType(DataType.Text)]
    public string? UserName { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Compare(nameof(Password))]
    [DisplayName("Confirm password")]
    public string? ConfirmPassword { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    [Remote("IsEmailInUse", "Account")]
    public string? Email { get; set; }
    
    [Required]
    [DisplayName("Phone number")]
    public string? PhoneNumber { get; set; }
}