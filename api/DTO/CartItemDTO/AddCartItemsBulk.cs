using api.Entities;

namespace api.DTO.CartItemDTO;

public class AddCartItemsBulk
{
    public List<AddCartItemDTO> CartItems { get; set; } = default!;
}