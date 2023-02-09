using Tasks.Data;
using Tasks.Entities;
using Tasks.Repositories.Interfaces;

namespace Tasks.Repositories;

public class ProductRepository:GenericRepository<Product> , IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context){}
}