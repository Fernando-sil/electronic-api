using api.Enums;

namespace api.DTO.UserDTO;

public class UpdateUserDTO
{
    public UserRoles Role { get; set; }
    public bool IsActive { get; set; }
}