using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tasks.Data;
using Tasks.Models;
using Tasks.Services;

namespace Tasks.Controllers;

public class ProductController:Controller
{
    private readonly ILogger<ProductController> _logger;
    private readonly IProductService _service;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly AppDbContext _context;

    public ProductController(
        IProductService service,
        ILogger<ProductController> logger,
        UserManager<IdentityUser> userManager,
        AppDbContext context 
        )
    {
        _logger = logger ;
        _service = service ;
        _userManager =  userManager ;
        _context = context ;
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
      var products = await _service.GetAll();
        
       return View(products.Data);
    }
    public IActionResult AddProduct() => View();

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> AddProduct(Product model)
    {
        if(!ModelState.IsValid) return View();

        if(model.Price <= 0)
        {
            ModelState.AddModelError("Price", "The value cannot be minus or 0");
            return View(model);
        }
        if(model.Quantity <= 0)
        {
            ModelState.AddModelError("Quantity", "The value cannot be minus or 0");
            return View(model);
        }
        var user1 = User.Identity!.Name;

         var user = _userManager.Users.FirstOrDefault(x => x.UserName == user1);

        await _service.CreateAsync(model,user!.Id.ToString());

        return RedirectToAction(nameof(List));

    }

    public async Task<IActionResult> Update(long id) { 
        var result = await _service.GetByIdAsync(id);
        var product = result.Data;
        
        return View(product);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdatePost(long id, Product model)
    {
        var user1 = User.Identity!.Name;
        var user = _userManager.Users.FirstOrDefault(x => x.UserName == user1);
        
        if(model.Title is null)
        {
            ModelState.AddModelError("Title", "Title is not null");
            return View(model);
        }
        if(model.Price <= 0)
        {
            ModelState.AddModelError("Price", "The value cannot be minus or 0");
            return View(model);
        }
        if(model.Quantity <= 0)
        {
            ModelState.AddModelError("Quantity", "The value cannot be minus or 0");
            return View(model);
        }

        await _service.Update(id, model,user!.Id.ToString());

        return RedirectToAction(nameof(List));
    }

    [Authorize(Roles = "admin")]
    public IActionResult Remove(int id)
    {
        var user1 = User.Identity!.Name;
        var user = _userManager.Users.FirstOrDefault(x => x.UserName == user1);
        var product = _service.Remove(id,user!.Id.ToString());
        return RedirectToAction(nameof(List));
    }

    [Authorize(Roles = "admin")]
    public IActionResult History()
    {
      var histories = _context.Histories!.ToList();

      if(histories is null)  return View();
      
      return View(histories);
    }
}