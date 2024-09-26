using System.ComponentModel.DataAnnotations;

namespace api.DTO.UserDTO;

public class LoginUserDTO
{
    [MinLength(3, ErrorMessage ="Minimum number of characters is 3")]
     public string UserName { get; set; } = string.Empty;
     [MinLength(8, ErrorMessage ="Minimum number of characters is 8")]
    public string Password { get; set; } = string.Empty;
}