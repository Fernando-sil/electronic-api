using System.ComponentModel.DataAnnotations;
using api.Entities;

namespace api.DTO.SpecificationItemDTO;

public class AddSpecificationItemDTO
{
    [MinLength(3, ErrorMessage ="Minimum number of characters is 3")]
    public string Value { get; set; } = string.Empty;
   
    public int SpecificationId { get; set; }
    
    public Guid ItemId { get; set; }

    public SpecificationItem FromAddSpecificationItemDTO(){
        return new SpecificationItem{
            Value = Value,
            SpecificationId = SpecificationId,
            ItemId = ItemId
        };
    }
}