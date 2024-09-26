using api.DataContext;
using api.DTO.CartItemDTO;
using api.Entities;
using api.Helpers;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class CartItemRepository : ICartItemRepository
{
    private readonly DatabaseContext _context;
    private readonly ICurrentUserInterface _currentUser;

    public CartItemRepository(DatabaseContext context, ICurrentUserInterface currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }
    public async Task<ResponseRepository<string>> AddItems(AddCartItemsBulk addCartItemsBulk)
    {
        var response = new ResponseRepository<string>();
        var failResponse = new FailResponse();
        var userVerified = await IsUserVerified();
        if (userVerified is null){
            failResponse.GenerateFailResponse(response, "User not found.");
            return response;
        }
        if((bool)!userVerified!){
            failResponse.GenerateFailResponse(response, "User not verified.");
            return response;
        }
        Cart newCart = default!;
        var cart = await GetCart();
        if(cart is null || cart!.IsPurchasePaid){
          newCart = await CreateCart();
        }
        var finalCart = cart ?? newCart;
       var userCartItems = GetUserCartItems(addCartItemsBulk, finalCart);

       SetTotalPrice(finalCart,userCartItems);
        _context.CartItems.AddRange(userCartItems);
        await _context.SaveChangesAsync();
        response.Data = "";
        response.Message = "Items added";
        return response;
    }
    public async Task<ResponseRepository<string>> UpdateItems(AddCartItemsBulk addCartItemsBulk)
    {
        var response = new ResponseRepository<string>();
        var failResponse = new FailResponse();
        var userVerified = await IsUserVerified();
        if (userVerified is null){
            failResponse.GenerateFailResponse(response, "User not found");
            return response;
        }
        if(userVerified == false){
            failResponse.GenerateFailResponse(response, "User not verified");
            return response;
        }
        var cart = await GetCart();
        if(cart is null || cart!.IsPurchasePaid){
          failResponse.GenerateFailResponse(response, "Cart not found");
            return response;
        }
        var existingCartItems = await _context.CartItems.Where(cartItems => cartItems.CartId == cart.Id).ToListAsync();
        if(existingCartItems is null){
            failResponse.GenerateFailResponse(response, "No items to update");
            return response;
        }
        _context.CartItems.RemoveRange(existingCartItems);
        var userCartItems = GetUserCartItems(addCartItemsBulk, cart);

        SetTotalPrice(cart, userCartItems);
        _context.CartItems.AddRange(userCartItems);
        await _context.SaveChangesAsync();
        response.Data = "";
        response.Message = "Items added";
        return response;
    }
     private async Task<Cart> CreateCart(){
        var user = await _currentUser.GetUser();
        var newCart = new Cart{
            User = user
        };
        _context.Carts.Add(newCart);
        await _context.SaveChangesAsync();
        return newCart;
    }
    private async Task<Cart?> GetCart(){
       var user = await _currentUser.GetUser();
       if(user is null) return null;
       var cart = await _context.Carts.Include(cartItems => cartItems.CartItems)!.ThenInclude(item => item.Item).OrderBy(c => c.DateCreated).LastOrDefaultAsync(c => c.User!.Id == user.Id);
       return cart; 
    }
    private void SetTotalPrice(Cart cart, List<CartItem> test){
        var totalPrice = test.Sum(s => s.SubTotal);
        cart.Total = totalPrice;
    }

    private List<CartItem> GetUserCartItems(AddCartItemsBulk addCartItemsBulk, Cart cart){
        var itemsToAdd = addCartItemsBulk.CartItems.Select(cartItem =>
        new CartItem{
            ItemId = cartItem.ItemId,
            Quantity = cartItem.Quantity,
            CartId = cart.Id,
            SubTotal = cart.CartItems!.Select(items => items.Item!).Where(i => i.Id == cartItem.ItemId).First().Price*cartItem.Quantity
        }).ToList();
      
        return itemsToAdd;
    }
    private async Task<bool?> IsUserVerified(){
        var user = await _currentUser.GetUser();
        if(user is null) return null;
        return user.ConfirmedEmail;
    }
}

// recovery code
// 3GJTL9CXM14C1BWKFJT93SUK