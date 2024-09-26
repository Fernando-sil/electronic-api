using System.ComponentModel.DataAnnotations;
using api.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.DTO.ItemDTO;

public class UpdateItemDTO
{
    
   [MinLength(3, ErrorMessage ="Minimum number of characters is 3")]
    public string ItemName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    [Range(1,300, ErrorMessage = "Minimum amount must be greater than 0.")]
    public int Quantity { get; set; } = 0;
    [Range(0.00,15_000.00, ErrorMessage = "Price must be greater than $0.00")]
    [Precision(7,2)]
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public int BrandId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;

    public void ToItem(Item item){
            item.ItemName = ItemName;
            item.Description = Description;
            item.Quantity = Quantity;
            item.Price = Price;
            item.CategoryId = CategoryId;
            item.BrandId = BrandId;
            item.ImageUrl = ImageUrl;
    }
    // public Item ToItem(UpdateItemDTO updateItemDTO){
    //     return new Item{
    //         ItemName = ItemName,
    //         Description = Description,
    //         Quantity = Quantity,
    //         Price = Price,
    //         CategoryId = CategoryId,
    //         BrandId = BrandId
    //     };
    // }
}