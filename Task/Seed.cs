using Microsoft.AspNetCore.Identity;
using Tasks.Models;

namespace Tasks;

public class Seed 
{
 public static async ValueTask InitializeRoleAsync(IApplicationBuilder app)
 {
    using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
    .CreateScope();

    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<Seed>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    
    var roles = config.GetSection($"Identity:IdentityServer:Roles").Get<string[]>();

    foreach (var role in roles)
    {
        if(!await roleManager.RoleExistsAsync(role))
        {
          var newRole = new IdentityRole(role);

          var result = await roleManager.CreateAsync(newRole);

          if(!result.Succeeded)
          {
            logger.LogInformation("Seed role feiled");
          }
          else
          {
            logger.LogInformation($"Seed role created");
          }
        }
        else
        {
            logger.LogInformation("seed role already exist");
        }
    
    }
    
 }
 public static async ValueTask InitializeUserAsync(IApplicationBuilder app)
  {
    using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<Seed>();
    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var users = config.GetSection($"Identity:IdentityServer:User").Get<UserSeed[]>();

    foreach (var user in users)
    {
       var newUser = new IdentityUser(user.UserName);

       var result = await userManager.CreateAsync(newUser,user.PasswordHash);

       if(result.Succeeded)
       {
        var roleResult = await userManager.AddToRolesAsync(newUser,user.Roles);

            if(roleResult.Succeeded)
            {
            logger.LogInformation($"{user.UserName} to added roles");
            }
            else
            {
                logger.LogInformation($"{user.UserName} to didn't add roles");
            }
       } 
       else
       {
        logger.LogInformation($"{user.UserName} didn't created");
       }
    }
  }
  
}