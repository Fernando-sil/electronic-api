using api.DTO.BrandDTO;
using api.DTO.CategoryDTO;
using api.DTO.GenericDTO;
using api.Repository;

namespace api.Interfaces;

public interface ICategoryRepository
{
    Task<ResponseRepository<GetCategoryDTO>> AddBrandsToCategory(int categoryId, List<AddBrandByIdDTO> addBrandByIdDTOs);
    Task<ResponseRepositoryWithCount<List<GenericGetDTO>>> GetAllCategories();
    Task<ResponseRepository<GetCategoryDTO>> GetCategory(int id);
    Task<ResponseRepository<string>> DeleteCategory(int id);
    Task<ResponseRepositoryWithCount<List<GenericGetDTO>>> AddCategories(List<GenericUpdateDTO> genericUpdateDTOs);
    Task<ResponseRepository<GenericGetDTO>> UpdateCategory(GenericUpdateDTO genericUpdateDTO, int id);
}