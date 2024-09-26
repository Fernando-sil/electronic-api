using System.ComponentModel.DataAnnotations;
using api.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.DTO.ItemDTO;

public class AddItemDTO
{
    [MinLength(3, ErrorMessage ="Minimum number of characters is 3")]
    public string ItemName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    [Range(1,300, ErrorMessage = "Minimum amount must be greater than 0.")]
    public int Quantity { get; set; } = 0;
    [Range(0.00,15_000.00, ErrorMessage = "Price must be greater than $0.00")]
    [Precision(7,2)]
    public decimal Price { get; set; }
    public double? Score { get; set; } = 1.0;
    public int CategoryId { get; set; }
    public int BrandId { get; set; }
    public string ImageUrl { get; set; } = "https://st3.depositphotos.com/1526816/37539/v/450/depositphotos_375399596-stock-illustration-computer-set-keyboard-display-white.jpg";

    public Item ToItem(){
        return new Item{
            ItemName = ItemName,
            Description = Description,
            Quantity = Quantity,
            Price = Price,
            Score = Score,
            CategoryId = CategoryId,
            BrandId = BrandId
        };
    }
}