using System.ComponentModel.DataAnnotations;
using api.Entities;

namespace api.DTO.SpecificationDTO;

public class AddSpecificationDTO
{
    [MinLength(3, ErrorMessage ="Minimum number of characters is 3")]
    public string Spec { get; set; } = string.Empty;

    public Specification FromAddSpecificationDTO(){
        return new Specification{
            Spec = Spec
        };
    }
}