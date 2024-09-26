using api.Entities;
using api.Enums;

namespace api.DTO.UserDTO;

public class GetUserDTO
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public bool ConfirmedEmail { get; set; }
    public UserRoles Role { get; set; }
    public bool IsActive { get; set; }
    // public string? RefreshToken { get; set; }
    // public DateTime? RefreshTokenExpires { get; set; }

    public static explicit operator GetUserDTO(User user){
        return new GetUserDTO{
            Id = user.Id,
            UserName = user.UserName,
            EmailAddress = user.EmailAddress,
            ConfirmedEmail = user.ConfirmedEmail,
            Role = user.Role,
            IsActive = user.IsActive
        };
    }
}