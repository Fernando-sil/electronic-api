using System.Security.Claims;
using api.DTO.UserDTO;
using api.Entities;

namespace api.Interfaces;

public interface ITokenInterface
{
    TokenDTO GenerateToken(User user);
    TokenDTO GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    string? GetRefeshTokenFromCookie();
    void DeleteTokenFromCookie(string tokenName);
    void AddTokenToCookie(string tokenName, string token,CookieOptions cookieOptions);
}