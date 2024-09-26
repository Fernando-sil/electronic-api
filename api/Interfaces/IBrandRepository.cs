using api.DTO.BrandDTO;
using api.DTO.GenericDTO;
using api.Repository;

namespace api.Interfaces;

public interface IBrandRepository
{
    Task<ResponseRepository<GetBrandDTO>> AddBrand(BrandDTO brandDTO);
    Task<ResponseRepository<List<GenericGetDTO>>> AddBrands(List<GenericUpdateDTO> genericUpdateDTOs);
    // Task<ResponseRepository<BrandWithCategoriesDTO>> AddBrand(AddBrandDTO brandDTO);
    Task<ResponseRepositoryWithCount<List<GetBrandDTO>>> GetAllBrands();
    Task<ResponseRepository<GetBrandWithCategoriesDTO>> GetBrand(int id);
    Task<ResponseRepository<GetBrandDTO>> UpdateBrand(int id, GenericUpdateDTO genericUpdateDTO);
    Task<ResponseRepository<string>> DeleteBrand(int id);
    Task<ResponseRepositoryWithCount<List<GetBrandDTO>>> GetPopularBrands();
}