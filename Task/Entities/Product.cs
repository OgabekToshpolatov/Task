using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tasks.Entities;

public class Product
{
    [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    [Required]
    public string? Title { get; set; }
    [Required]
    public int Quantity { get; set; }
    [Required] 
    public double Price { get; set; } 
} 