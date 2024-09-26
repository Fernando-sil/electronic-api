using api.DataContext;
using api.DTO.CartDTO;
using api.DTO.CartItemDTO;
using api.Entities;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class CartRepository : ICartRepository
{
    private readonly IDbContextFactory<DatabaseContext> _contextFactory;
    private readonly ICurrentUserInterface _currentUser;

    public CartRepository(IDbContextFactory<DatabaseContext> contextFactory, ICurrentUserInterface currentUser)
    {
        _contextFactory = contextFactory;
        _currentUser = currentUser;
    }

    public async Task<ResponseRepository<string>> CreateCart()
    {
        using var _context = _contextFactory.CreateDbContext();
        var response = new ResponseRepository<string>();
        var user = await _currentUser.GetUser();
        if (user is null)
        {
            response.Success = false;
            response.Message = "User not found";
            return response;
        }
        var cart = await GetCart();
        if (cart is null || cart.IsPurchasePaid)
        {
            var newCart = new Cart
            {
                User = user
            };
            _context.Carts.Add(newCart);
            await _context.SaveChangesAsync();
            response.Data = "";
            response.Success = true;
            response.Message = "Cart Created";
            return response;
        }
        else if (cart != null && !cart.IsPurchasePaid)
            response.Data = "";
        response.Success = false;
        response.Message = "Cart with unpaid items exsists";
        return response;
    }

    public async Task<ResponseRepository<GetCartDTO>> GetUserCart()
    {
          using var _context = _contextFactory.CreateDbContext();
        var response = new ResponseRepository<GetCartDTO>();
        var user = await _currentUser.GetUser();
        if(user is null){
            response.Success = false;
            response.Message = "User not found";
            return response;
        }
        var userCart = await _context.Carts.Include(user => user.User).Include(item => item.CartItems!).ThenInclude(i => i.Item).Where(cart => cart.User!.Id == user.Id).Select(p => new GetCartDTO{
            DateCreated = p.DateCreated,
            IsPurchasePaid = p.IsPurchasePaid,
            DatePurchasePaid = p.DatePurchasePaid,
            UserName = p.User!.UserName,
            CartItems = p.CartItems!.Select(c =>(GetCartItemDTO)c).ToList(),
            Total = p.CartItems!.Sum(s => s.SubTotal)
        }).ToListAsync();
        if(userCart is null){
            response.Success = false;
            response.Message = "Cart not found";
            return response;
        }
        var cart = userCart.First();
        if(cart.IsPurchasePaid){
            response.Data = (GetCartDTO)new Cart{User = user};
        } else {
        response.Data = cart;

        }
        return response;
    }
    private async Task<Cart?> GetCart(){
        using var _context = _contextFactory.CreateDbContext();
        var userId = await _currentUser.GetUserId();
        if(userId is null) return null;
        var cart = await _context.Carts.OrderBy(c => c.DateCreated).LastOrDefaultAsync(c => c.User!.Id == userId);
        if(cart is null) return null;
        return cart;
    }
}
