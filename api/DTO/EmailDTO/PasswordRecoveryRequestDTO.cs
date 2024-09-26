using System.ComponentModel.DataAnnotations;

namespace api.DTO.EmailDTO;

public class PasswordRecoveryRequestDTO
{
    [MinLength(3, ErrorMessage ="Minimum number of characters is 3")]
    public string UserName { get; set; } = string.Empty;
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}