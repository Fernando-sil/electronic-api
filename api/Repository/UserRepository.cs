using System.Security.Claims;
using api.DataContext;
using api.DTO.UserDTO;
using api.Entities;
using api.Helpers;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace api.Repository;

public class UserRepository : IUserRepository
{
    private readonly DatabaseContext _context;
    private readonly ICurrentUserInterface _currentUser;
    private readonly ITokenInterface _token;
    private readonly IPasswordInterface _password;

    public UserRepository(DatabaseContext context, ITokenInterface token, IPasswordInterface password, ICurrentUserInterface currentUser)
    {
        _context = context;
        _token = token;
        _password = password;
        _currentUser = currentUser;
    }

    public async Task<ResponseRepository<AuthenticatedUserDTO>> Login(LoginUserDTO loginUserDTO)
    {
        var failResponse = new FailResponse();
        var response = new ResponseRepository<AuthenticatedUserDTO>();
        if(loginUserDTO.UserName is null || loginUserDTO.Password is null){
           failResponse.GenerateFailResponse(response, "Incorrect user or password");
           return response;
        }
        var existingUser = await _context.Users.FirstOrDefaultAsync(user => user.UserName == loginUserDTO.UserName);
        if(existingUser is null){
            failResponse.GenerateFailResponse(response, "Incorrect user or password");
            return response;
        }
        var isPasswordValid = _password.IsPasswordValid(loginUserDTO.Password!,existingUser!.Password,existingUser.Salt);
        if(!isPasswordValid){
            failResponse.GenerateFailResponse(response, "Incorrect user or password");
            return response;
        }
        TokenDTO token = await SetUserToken(existingUser);
        var user = (AuthenticatedUserDTO)existingUser;
        user.Token = token.Token;
        response.Data = user;
        return response;
    }
    public async Task<ResponseRepository<string>> RegisterUser(RegisterUserDTO registerUserDTO)
    {
        var failResponse = new FailResponse();
        var response = new ResponseRepository<string>();
        var existingUser = await _context.Users.FirstOrDefaultAsync(user => user.UserName == registerUserDTO.UserName);
        if(existingUser != null){
            failResponse.GenerateFailResponse(response, "User already exists.");
            return response;
        }
        _password.HashPassword(registerUserDTO.Password, out byte[] hashedPassword, out byte[] salt);
        var user = new User{
       Password = hashedPassword,
        Salt = salt,
        UserName = registerUserDTO.UserName,
        EmailAddress = registerUserDTO.EmailAddress,
        ActivationCode = new Guid()
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        response.Message = "New user created";
        return response;
    }
    public async Task<ResponseRepository<GetUserDTO>> UpdateUser(Guid Id, UpdateUserDTO updateUserDTO)
    {
        var failResponse = new FailResponse();
        var response = new ResponseRepository<GetUserDTO>();
        var existingUser = await _context.Users.FirstOrDefaultAsync(user => user.Id == Id);
        if(existingUser is null) {
            failResponse.GenerateFailResponse(response, "User not Found");
            return response;
        }
        existingUser.Role = updateUserDTO.Role;
        existingUser.IsActive = updateUserDTO.IsActive;
        await _context.SaveChangesAsync();
        var user =  (GetUserDTO)existingUser;
    
        response.Data = user;
        return response;
    }
    public async Task<ResponseRepository<string>> ActivateUserEmail(Guid activationCode)
    {
        var failResponse = new FailResponse();
        var response = new ResponseRepository<string>();
        var existingUser = await _context.Users.FirstOrDefaultAsync(user => user.ActivationCode == activationCode);
        if(existingUser is null) {
            failResponse.GenerateFailResponse(response, "User not Found");
            return response;
        }
        existingUser.ConfirmedEmail = true;
        existingUser.ActivationCode = Guid.NewGuid();
        await _context.SaveChangesAsync();
        response.Message = "Email activated";
        return response;
    }
     public async Task<ResponseRepository<string>> ChangePassword(ChangeUserPasswordDTO changeUserPasswordDTO)
    {
        var failResponse = new FailResponse();
        var response = new ResponseRepository<string>();
        var existingUser = await _context.Users.FirstOrDefaultAsync(user => user.ActivationCode == changeUserPasswordDTO.RecoveryCode);
        if(existingUser is null) {
            failResponse.GenerateFailResponse(response, "User not Found");
            return response;
        }
        _password.HashPassword(changeUserPasswordDTO.Password, out byte[] hashedPassword, out byte[] salt);
        existingUser.Password = hashedPassword;
        existingUser.Salt = salt;
        existingUser.ActivationCode = Guid.NewGuid();
        await _context.SaveChangesAsync();
        response.Message = "Password has been changed";
        return response;
    }
    
     public async Task<ResponseRepository<AuthenticatedUserDTO>> RefreshToken(string accessToken)
    {
        var failResponse = new FailResponse();
        var response = new ResponseRepository<AuthenticatedUserDTO>();
        var refreshToken = _token.GetRefeshTokenFromCookie();
        var expiredTokenPrinciple = _token.GetPrincipalFromExpiredToken(accessToken);
        if (expiredTokenPrinciple is null)
        {
            failResponse.GenerateFailResponse(response, "Invalid token.");
            return response;
        }
        var userName = expiredTokenPrinciple.Identity!.Name;
        var existingUser = await _context.Users.FirstOrDefaultAsync(user => user.UserName == userName);
        if (existingUser is null)
        {
            failResponse.GenerateFailResponse(response, "User not found");
            return response;
        }
        if (existingUser!.RefreshToken == null)
        {
            failResponse.GenerateFailResponse(response, "Token revoked");
            return response;
        }
        if (!existingUser!.RefreshToken!.Equals(refreshToken))
        {
            failResponse.GenerateFailResponse(response, "Invalid refresh token");
            return response;
        }
        if (existingUser.RefreshTokenExpires < DateTime.Now)
        {
            failResponse.GenerateFailResponse(response, "Token expired");
            return response;
        }

        TokenDTO token = await SetUserToken(existingUser);
        var user = (AuthenticatedUserDTO)existingUser;
        user.Token = token.Token;
        response.Data = user;
        return response;
    }

    private async Task<TokenDTO> SetUserToken(User existingUser)
    {
        var token = _token.GenerateToken(existingUser);
        var refreshToken = _token.GenerateRefreshToken();
        SetTokenToCookie(refreshToken, "refreshToken");
        await SetUserRefreshToken(refreshToken, existingUser);
        return token;
    }

    public async Task<ResponseRepositoryWithCount<List<GetUsersDTO>>> GetUsers(QueryHelper query){
        var response = new ResponseRepositoryWithCount<List<GetUsersDTO>>();
        
        var existingUser = _context.Users.AsQueryable();
        if(!string.IsNullOrWhiteSpace(query.name)){
            existingUser = existingUser.Where(user => user.UserName.Contains(query.name));
        }
        var user = await existingUser.Select(user => new GetUsersDTO(user.Id,user.UserName, user.Role)).ToListAsync();
        response.Data = user;
        response.Count = user.Count;
        return response;
    }
    public async Task<ResponseRepository<string>> Logout()
    {
        var failResponse = new FailResponse();
        var response = new ResponseRepository<string>();
        var user = await _currentUser.GetUser();
        if(user is null){
            failResponse.GenerateFailResponse(response, "You are not logged in.");
            return response;
        }
        _token.DeleteTokenFromCookie("refreshToken");
        response.Message = "Logged out.";
        return response;
    }
      public async Task<ResponseRepository<GetUserDTO>> GetUser(Guid id)
    {
        var failResponse = new FailResponse();
        var response = new ResponseRepository<GetUserDTO>();
        var existingUser = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
        if(existingUser is null){
            failResponse.GenerateFailResponse(response,"User not found.");
            return response;
        }
        response.Data = (GetUserDTO)existingUser;
        return response;
    }
    private async Task SetUserRefreshToken(TokenDTO token, User user){
        user.RefreshToken = token.Token;
        user.RefreshTokenExpires = token.Expires;
        await _context.SaveChangesAsync();
    }
    private void SetTokenToCookie(TokenDTO token, string keyName){
        
        var cookieOptions = new CookieOptions{
            HttpOnly = true,
            Expires = token.Expires
        };
        _token.AddTokenToCookie(keyName, token.Token, cookieOptions);
    }

    public async Task<ResponseRepository<User>> GetName()
    {
        var failResponse = new FailResponse();
        var response = new ResponseRepository<User>();
        var user = await _currentUser.GetUser();
        if(user is null){
            failResponse.GenerateFailResponse(response,"User not found");
        }
        response.Data = user;

    //     var expiredTokenPrinciple = _token.GetPrincipalFromExpiredToken(accessToken);
    //     if(expiredTokenPrinciple is null) {
    //         failResponse.GenerateFailResponse(response, "Ivalid token.");
    //         return response;
    //     }
    //   response.Data = expiredTokenPrinciple.Identity!.Name;
      return response;
    }
    // public ResponseRepository<string> GetName(string accessToken)
    // {
    //     var failResponse = new FailResponse();
    //     var response = new ResponseRepository<string>();
    //     var expiredTokenPrinciple = _token.GetPrincipalFromExpiredToken(accessToken);
    //     if(expiredTokenPrinciple is null) {
    //         failResponse.GenerateFailResponse(response, "Ivalid token.");
    //         return response;
    //     }
    //   response.Data = expiredTokenPrinciple.Identity!.Name;
    //   return response;
    // }
}

// 2FYS8eUSmaWSfP3Su15ui/JSg41oVfBmEdvWV3JuEbNa+oA+bznxtlzTSvX3K31Non5RWIgPnj+345A2lmyd6A==