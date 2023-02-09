using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tasks.Entities;

namespace Tasks.Data;

public class AppDbContext:IdentityDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options) {}

    public DbSet<Product>? Products { get; set; }

    public DbSet<History>? Histories { get; set; }
}