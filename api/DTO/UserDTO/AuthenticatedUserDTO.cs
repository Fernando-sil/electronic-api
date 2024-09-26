using api.Entities;
using api.Enums;

namespace api.DTO.UserDTO;

public class AuthenticatedUserDTO
{
    public Guid Id { get; set; }
     public string UserName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public bool ConfirmedEmail { get; set; }
    public UserRoles Role { get; set; }
    public bool IsActive { get; set; }
    public string Token { get; set; } = string.Empty;

    public static explicit operator AuthenticatedUserDTO(User user){
        return new AuthenticatedUserDTO{
            Id = user.Id,
            UserName = user.UserName,
            EmailAddress = user.EmailAddress,
            ConfirmedEmail = user.ConfirmedEmail,
            Role = user.Role,
            IsActive = user.IsActive,
        };
    }
}