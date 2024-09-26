namespace api.DTO.UserDTO;

public class RefreshTokenDTO
{
    public required string Token { get; set; }
    public DateTime Expires { get; set; }
}