using api.DTO.BrandDTO;
using api.DTO.CategoryDTO;
using api.Entities;

namespace api.DTO.ItemDTO;

public class GetItemsDTO
{
    public Guid Id { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public double Score { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public GetItemCategoryDTO? ItemCategory { get; set; }
    public GetBrandDTO? ItemBrand { get; set; }

    public static explicit operator GetItemsDTO(Item item){
        return new GetItemsDTO{
            Id = item.Id,
            ItemName = item.ItemName,
            Description = item.Description,
            Quantity = item.Quantity,
            Price = item.Price,
            Score = (double)item.Score!,
            ImageUrl = item.ImageUrl,
            ItemCategory = (GetItemCategoryDTO)item.Category! ?? default,
            ItemBrand = (GetBrandDTO)item.Brand! ?? default
        };
    }
}