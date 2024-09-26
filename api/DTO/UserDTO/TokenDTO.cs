namespace api.DTO.UserDTO;

public class TokenDTO
{
    public required string Token { get; set; }
    public DateTime Expires { get; set; }
}