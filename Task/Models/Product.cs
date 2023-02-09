using System.ComponentModel.DataAnnotations;

namespace Tasks.Models;

public class Product
{
    public long Id { get; set; }
    [Required]
    public string? Title { get; set; }
    [Required]
    public int Quantity { get; set; }
    [Required]
    public double Price { get; set; }
}