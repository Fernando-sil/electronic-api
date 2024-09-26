using api.DTO.CategoryDTO;
using api.Entities;

namespace api.DTO.BrandDTO;

public class GetBrandWithCategoriesDTO
{
    public int Id { get; set; }
    public string BrandName { get; set; } = string.Empty;
    public bool IsPopular { get; set; }
    public List<GetItemCategoryDTO>? Categories { get; set; }

    public static explicit operator GetBrandWithCategoriesDTO(Brand brand){
        return new GetBrandWithCategoriesDTO{
            Id = brand.Id,
            BrandName = brand.BrandName,
            IsPopular = brand.IsPopular,
            Categories = brand.Categories?.Select(b => (GetItemCategoryDTO)b).ToList()
        };
    }
}