using api.Entities;

namespace api.DTO.CategoryDTO;

public class GetItemCategoryDTO
{
    public int Id { get; set; }
    public string CategoryName { get; set; } = string.Empty;

    public static explicit operator GetItemCategoryDTO(Category category){
        return new GetItemCategoryDTO{
            Id = category.Id,
            CategoryName = category.CategoryName
        };
    }

    public Category ToCategory(){
        return new Category{
            Id = Id,
            CategoryName = CategoryName
        };
    }
}