using System.Security.Claims;
using api.DataContext;
using api.Entities;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Helpers;

public class CurrentUser : ICurrentUserInterface
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly DatabaseContext _context;

    public CurrentUser(DatabaseContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<User?> GetUser(){
        var userName = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
        if(userName is null){
            return null;
        }
        var user = await _context.Users.FirstOrDefaultAsync(user => user.UserName == userName);
        return user;
    }
    public async Task<Guid?> GetUserId(){
       var user = await GetUser();
       if(user is null){
            return null;
        }
        var userId = user.Id;
        return userId;
    }
}