using api.DTO.UserDTO;
using api.Entities;
using api.Helpers;
using api.Repository;

namespace api.Interfaces;

public interface IUserRepository
{
    Task<ResponseRepository<string>> RegisterUser(RegisterUserDTO registerUserDTO);
    Task<ResponseRepository<AuthenticatedUserDTO>> Login(LoginUserDTO loginUserDTO);
    Task<ResponseRepository<AuthenticatedUserDTO>> RefreshToken(string accessToken);
    Task<ResponseRepository<GetUserDTO>> UpdateUser(Guid Id, UpdateUserDTO updateUserDTO);
    Task<ResponseRepository<string>> ActivateUserEmail(Guid activationCode);
    Task<ResponseRepository<string>> ChangePassword(ChangeUserPasswordDTO changeUserPasswordDTO);
    Task<ResponseRepositoryWithCount<List<GetUsersDTO>>> GetUsers(QueryHelper query);
    Task<ResponseRepository<GetUserDTO>> GetUser(Guid id);
    Task<ResponseRepository<string>> Logout();
    Task<ResponseRepository<User>> GetName();
}