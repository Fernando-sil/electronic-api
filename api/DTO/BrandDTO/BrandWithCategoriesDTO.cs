using System.ComponentModel.DataAnnotations;
using api.Entities;

namespace api.DTO.BrandDTO;

public class BrandWithCategoriesDTO
{
    [MinLength(3, ErrorMessage ="Minimum number of characters is 3")]
    public string BrandName { get; set; } = string.Empty;
    public List<Category>? Categories { get; set; }

    public static explicit operator BrandWithCategoriesDTO(Brand brand){
        return new BrandWithCategoriesDTO{
            BrandName = brand.BrandName,
            Categories = brand.Categories
        };
    }
}