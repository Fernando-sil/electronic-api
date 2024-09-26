using System.ComponentModel.DataAnnotations;

namespace api.DTO.EmailDTO;

public class SendEmailDTO
{
    public string To { get; set; } = string.Empty;
    [EmailAddress]
    public string EmailAddress { get; set; } = string.Empty;
}