using api.Entities;

namespace api.DTO.ItemDTO;

public class GetItemsOnSaleDTO
{
    public Guid Id { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public double Score { get; set; }
    public string CategoryName { get; set; } = string.Empty;

    public static explicit operator GetItemsOnSaleDTO(Item item){
        return new GetItemsOnSaleDTO{
            Id = item.Id,
            ItemName = item.ItemName,
            ImageUrl = item.ImageUrl
        };
    }
}