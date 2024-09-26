using api.DTO.CartItemDTO;
using api.Repository;

namespace api.Interfaces;

public interface ICartItemRepository
{
    Task<ResponseRepository<string>> AddItems(AddCartItemsBulk addCartItemsBulk);
    Task<ResponseRepository<string>> UpdateItems(AddCartItemsBulk addCartItemsBulk);
}