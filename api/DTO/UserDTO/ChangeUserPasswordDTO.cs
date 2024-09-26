namespace api.DTO.UserDTO;

public class ChangeUserPasswordDTO
{
    public string Password { get; set; } = string.Empty;
    public Guid RecoveryCode { get; set; }
}