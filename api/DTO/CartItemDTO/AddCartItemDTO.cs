using api.Entities;

namespace api.DTO.CartItemDTO;

public class AddCartItemDTO
{
    public int Quantity { get; set; }
    public Guid ItemId { get; set; }
    // public Item? Item { get; set; }

    public static explicit operator AddCartItemDTO(CartItem cartItem){
        return new AddCartItemDTO{
            Quantity = cartItem.Quantity,
            ItemId = cartItem.ItemId
        };
    }

    public CartItem FromAddCartItemDTO(){
        return new CartItem{
            ItemId = ItemId,
            Quantity = Quantity,
            // Item = Item
        };
    }
}