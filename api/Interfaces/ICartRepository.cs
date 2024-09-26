using api.DTO.CartDTO;
using api.Repository;

namespace api.Interfaces;

public interface ICartRepository
{
    Task<ResponseRepository<string>> CreateCart();
    Task<ResponseRepository<GetCartDTO>> GetUserCart();
}