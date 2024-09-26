using api.DTO.CartItemDTO;
using api.Interfaces;
using api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controller;

[ApiController]
[Route("api/[controller]")]
public class CartItemController : ControllerBase
{
    private readonly ICartItemRepository _cartItemRepository;

    public CartItemController(ICartItemRepository cartItemRepository)
    {
        _cartItemRepository = cartItemRepository;
    }
    
    [Authorize]
    [HttpPost("add-items")]
    public async Task<ActionResult<ResponseRepository<string>>> AddCartItems(AddCartItemsBulk addCartItemsBulk){
        var response = await _cartItemRepository.AddItems(addCartItemsBulk);
        if(!response.Success) return BadRequest(response);
        return Ok(response);
    }
    [Authorize]
    [HttpPost("update-items")]
    public async Task<ActionResult<ResponseRepository<string>>> UpdateCartItems(AddCartItemsBulk addCartItemsBulk){
        var response = await _cartItemRepository.UpdateItems(addCartItemsBulk);
        if(!response.Success) return BadRequest(response);
        return Ok(response);
    }
}