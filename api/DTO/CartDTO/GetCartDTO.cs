using api.DTO.CartItemDTO;
using api.Entities;

namespace api.DTO.CartDTO;

public class GetCartDTO
{
    public DateTime DateCreated { get; set; } = DateTime.Now;
    public bool IsPurchasePaid { get; set; } = false;
    public DateTime? DatePurchasePaid { get; set; }
    public string UserName { get; set; } = string.Empty;
    public List<GetCartItemDTO>? CartItems { get; set; }
    public decimal Total { get; set; }

    public static explicit operator GetCartDTO(Cart cart){
        var cartItems = cart.CartItems!.Select(item => (GetCartItemDTO)item);
        return new GetCartDTO{
            DateCreated = cart.DateCreated,
            IsPurchasePaid = cart.IsPurchasePaid,
            DatePurchasePaid = cart.DatePurchasePaid,
            UserName = cart.User!.UserName,
            CartItems = cartItems.ToList(),
            Total = cartItems.Sum(s => s.SubTotal)
        };
    }
}