using api.DTO.BrandDTO;
using api.Entities;

namespace api.DTO.CategoryDTO;

public class GetCategoryDTO
{
    public int Id { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public List<GetBrandDTO>? Brands { get; set; }

    public static explicit operator GetCategoryDTO(Category category){
        return new GetCategoryDTO{
            Id = category.Id,
            CategoryName = category.CategoryName,
            Brands = category.Brands?.Select(brand => (GetBrandDTO)brand).ToList()
        };
    }
}