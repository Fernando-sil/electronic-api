using api.Entities;

namespace api.DTO.CartItemDTO;

public class GetCartItemDTO
{
     public Guid ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal SubTotal { get; set; } = 0;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal Price { get; set; }

    public static explicit operator GetCartItemDTO(CartItem cartItem){
        return new GetCartItemDTO{
            ItemId = cartItem.ItemId,
            Quantity = cartItem.Quantity,
            ItemName = cartItem.Item!.ItemName,
            SubTotal = cartItem.SubTotal,
            ImageUrl = cartItem.Item.ImageUrl,
            Price = cartItem.Item.Price
        };
    }
}