using Microsoft.AspNetCore.Identity;

namespace Tasks.Models;

public class UserSeed:IdentityUser
{
    public string[]? Roles { get; set; }
}