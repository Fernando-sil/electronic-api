using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace api.Entities;

public class Item
{
    public Guid Id { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; } = 0;
    [Range(0.00,15_000.00)]
    [Precision(7,2)]
    public decimal Price { get; set; }
    [Range(1.0,5.0)]
    public double? Score { get; set; } = 1.0;
    public bool IsOnSale { get; set; } = false;
    public string ImageUrl { get; set; } = string.Empty;
    public Category? Category { get; set; }
    public int? CategoryId { get; set; }
    public Brand? Brand { get; set; }
    public int? BrandId { get; set; }
    public List<SpecificationItem>? ItemSpecifications { get; set; }
    public List<Rating>? Ratings { get; set; }
    public List<CartItem>? CartItems { get; set; }
}