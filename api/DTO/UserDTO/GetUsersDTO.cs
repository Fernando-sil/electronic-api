using api.Enums;

namespace api.DTO.UserDTO;

public record GetUsersDTO(Guid Id, string UserName, UserRoles Role);
