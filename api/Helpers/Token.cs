using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using api.DTO.UserDTO;
using api.Entities;
using api.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace api.Helpers;

public class Token :ITokenInterface
{

    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public Token(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public TokenDTO GenerateToken(User user){
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);
        var claims = new[]{
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.EmailAddress),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("emailVerified",user.ConfirmedEmail.ToString())
        };
        var tokenDescriptor = new SecurityTokenDescriptor{
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddHours(1),
            NotBefore = DateTime.Now,
            SigningCredentials = credentials
        };
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = new TokenDTO{
            Token = tokenHandler.WriteToken(token),
            Expires = (DateTime)tokenDescriptor.Expires
        };
       return jwtToken;
    }

    public TokenDTO GenerateRefreshToken(){
        var refreshToken = new TokenDTO{
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.Now.AddDays(7)
        };
        return refreshToken;
}
public string? GetRefeshTokenFromCookie(){
    var refreshToken = _httpContextAccessor?.HttpContext?.Request.Cookies["refreshToken"];
    if(refreshToken is null) return null;
    return refreshToken;
}
public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token){
    var tokenValidationParameters = new TokenValidationParameters{
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value!))
    };
    var tokenHandler = new JwtSecurityTokenHandler();
    var principal = tokenHandler.ValidateToken(token,tokenValidationParameters, out SecurityToken securityToken);
    if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase))
                // throw new SecurityTokenException("Invalid token");
                return null;

            return principal;
  }
  public void DeleteTokenFromCookie(string tokenName){
    _httpContextAccessor.HttpContext!.Response.Cookies.Delete(tokenName);
  }
  public void AddTokenToCookie(string tokenName, string token, CookieOptions cookieOptions){
     _httpContextAccessor.HttpContext!.Response.Cookies.Append(tokenName, token, cookieOptions);
  }
}