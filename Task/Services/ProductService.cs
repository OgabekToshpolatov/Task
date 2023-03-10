using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Tasks.Models;
using Tasks.Repositories.Interfaces;

namespace Tasks.Services;
[Authorize]
public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ProductService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IHistoryRepository _historyRepository;

    public ProductService(
        IProductRepository productRepository,
        ILogger<ProductService> logger,
        IConfiguration configuration,
        IHistoryRepository historyRepository  
    )
    {
        _productRepository = productRepository ;
        _logger = logger ;
        _configuration  = configuration ;
        _historyRepository = historyRepository ;
    }
    
    public async ValueTask<Result<Product>> CreateAsync(Product model,string userId)
    {
        try
        {
            if(model is null) return new("Model is null");
            
             string? vatt = _configuration.GetValue("VAT","");
             var vat = double.Parse(vatt!);            
  
            var price = Extensions.Calculate.TotalPrice(vat, model.Quantity, model.Price);

            model.Price = price;
            
            var createdProduct = await _productRepository.AddAsync(model.Adapt<Entities.Product>());
            if(createdProduct is null)
              return new("Product yaratilmadi");
              string newValues = $"[Title:{model.Title}] || [Price:{model.Price}] || [Quantity:{model.Quantity}]";
              string changed = EChanged.Create.ToString();
            _historyRepository.Save(userId ,string.Empty,newValues,changed,DateTime.Now,model.Id);
           return new(true) {Data = createdProduct.Adapt<Models.Product>()};
        }
        catch (System.Exception e)
        {
            _logger.LogInformation($"Product yaratilmadi {e.Message}");
            throw new Exception(e.Message);
        }
    }

    public async ValueTask<Result<List<Product>>> GetAll()
    {
        try
        {
            var products = _productRepository.GetAll()
                             .Select( x => x.Adapt<Models.Product>()).ToList();

            if(products.Count == 0) return new("Productlar mavjud emas ");

            return new(true) {Data = products};
        }
        catch (System.Exception e)
        {
            _logger.LogInformation($"Products mavjud emas  {e.Message}");
            throw new Exception(e.Message);
        }
    }

    public async  ValueTask<Result<Product>> GetByIdAsync(long id)
    {
        var product = _productRepository.GetById(id);
            if(product is null) return new("Bunday Product mavjud emas ");

             return new(true) {Data = product.Adapt<Models.Product>()};
    }

    public async ValueTask<Result<Product>> Remove(long id,string userId)
    {
        try
        {
            var product = _productRepository.GetById(id);
            if(product is null) return new("Bunday Product mavjud emas ");

            var removedProduct = await _productRepository.Remove(product);
            string oldValues = $"[Title:{product.Title}] || [Price:{product.Price}] || [Quantity:{product.Quantity}]";
            string changed = EChanged.Delete.ToString();
            _historyRepository.Save(userId ,oldValues,string.Empty,changed,DateTime.Now,product.Id);
            return new(true) {Data = removedProduct.Adapt<Models.Product>()};
        }
        catch (System.Exception e)
        {
            _logger.LogInformation($"Product ochirilmadi  {e.Message}");
            throw new Exception(e.Message);
        }
    }

    public async ValueTask<Result<Product>> Update(long id, Product model,string userId)
    {
        try
        {
             var product = _productRepository.GetById(id);
            
            if(product is null)
              return new("Bu Product mavjud emas");
             string oldValues = $"[Title:{model.Title}] || [Price:{model.Price}] || [Quantity:{product.Quantity}]";
            string? vatt = _configuration.GetValue("VAT","");
             var vat = double.Parse(vatt!); 
        
            var price = Extensions.Calculate.TotalPrice(vat, model.Quantity, model.Price);
            
            product.Title = model.Title;
            product.Quantity = model.Quantity;
            product.Price = price;

            var updatedProduct = await _productRepository.Update(product);
            string newValues = $"[Title:{model.Title}] || [Price:{model.Price}] || [Quantity:{product.Quantity}]";
            string changed = EChanged.Update.ToString();
            _historyRepository.Save(userId ,oldValues,newValues,changed,DateTime.Now,product.Id); 
           return new(true) {Data = updatedProduct.Adapt<Models.Product>()};
        }
        catch (System.Exception e)
        {
            _logger.LogInformation($"Product update bolmadi {model}");
           throw new Exception(e.Message);
        }
    }
}