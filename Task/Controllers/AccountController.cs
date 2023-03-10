using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tasks.Entities;
using Tasks.Models;

namespace Tasks.Controllers;

public class AccountController:Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AccountController(
        ILogger<AccountController> logger,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager
        )
    {
        _logger = logger ;
        _signInManager = signInManager;
        _userManager = userManager;
    }

//    [HttpGet("/signup")]
    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignUp([FromForm] UserCreateDto userCreateDto, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
            return View(userCreateDto);

        // var username =await _userManager.FindByNameAsync(userCreateDto.UserName!);
        // if(username != null)
        // {
        //     ModelState.AddModelError(nameof(userCreateDto.UserName), "There is a user with this username");
        //     return View(userCreateDto);
        // }

        var user = await _userManager.FindByEmailAsync(userCreateDto.Email!);
        if (user != null)
        {
            ModelState.AddModelError(nameof(userCreateDto.Email), "Email is registered.");
            return View(userCreateDto);
        }

        user = userCreateDto.Adapt<IdentityUser>();
        var result = await _userManager.CreateAsync(user, userCreateDto.Password!);

        if (!result.Succeeded)
        {
            ModelState.AddModelError("Username", "There is a user with this username");
            return View(userCreateDto);
        }

        await _signInManager.SignInAsync(user, isPersistent: true);
        
        if (!string.IsNullOrEmpty(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction(nameof(SignIn));
    }
    // [HttpGet("/signin")]
    public IActionResult SignIn()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(UserLoginDto model)
    {
        if(!ModelState.IsValid) return View();
     
        var user = await _userManager.FindByNameAsync(model.UserName!);
        
        if (user == null)
        {
            ModelState.AddModelError(nameof(model.UserName), "Username is not registered.");
            return View();
        }
        
        var result = await _signInManager.PasswordSignInAsync(user, model.Password!, true, true);
        
       
        return LocalRedirect($"/Product/List");

        // return View();    
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction("SignIn", "Account");
    }
}