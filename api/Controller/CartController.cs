using api.DTO.CartDTO;
using api.DTO.CartItemDTO;
using api.Interfaces;
using api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controller;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartRepository _cartRepository;

    public CartController(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }
    [Authorize]
    [HttpPost("create-cart")]
    public async Task<ActionResult<ResponseRepository<string>>> CreateCart(){
        var response = await _cartRepository.CreateCart();
        if(!response.Success) return BadRequest(response);
        return Ok(response);
    }
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<ResponseRepository<GetCartDTO>>> GetUserCart(){
        var response = await _cartRepository.GetUserCart();
        if(!response.Success){
            return NotFound(response);
        }
        return Ok(response);
    }
}