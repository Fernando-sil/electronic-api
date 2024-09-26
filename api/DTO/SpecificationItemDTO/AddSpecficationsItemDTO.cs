using System.ComponentModel.DataAnnotations;

namespace api.DTO.SpecificationItemDTO;

public class AddSpecficationsItemDTO
{
    [MinLength(3, ErrorMessage ="Minimum number of characters is 3")]
    public string Value { get; set; } = string.Empty;
    public int SpecificationId { get; set; }
}