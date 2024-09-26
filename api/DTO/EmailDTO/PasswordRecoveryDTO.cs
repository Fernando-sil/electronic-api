using System.ComponentModel.DataAnnotations;

namespace api.DTO.EmailDTO;

public class PasswordRecoveryDTO
{
    [MinLength(8, ErrorMessage ="Minimum number of characters is 8")]
    public string Password { get; set; } = string.Empty;
    public Guid RecoveryCode { get; set; }

}