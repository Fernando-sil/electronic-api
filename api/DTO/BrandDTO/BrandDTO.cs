using System.ComponentModel.DataAnnotations;
using api.Entities;

namespace api.DTO.BrandDTO;

public class BrandDTO
{
    [MinLength(3, ErrorMessage ="Minimum number of characters is 3")]
    public string Name { get; set; } = string.Empty;

    public Brand ToBrand(){
        return new Brand{
            BrandName = Name
        };
    }
}