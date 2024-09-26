using api.DataContext;
using api.DTO.BrandDTO;
using api.DTO.GenericDTO;
using api.Entities;
using api.Helpers;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class BrandRepository : IBrandRepository
    {
        private readonly DatabaseContext _context;

        public BrandRepository(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<ResponseRepository<GetBrandDTO>> AddBrand(BrandDTO brandDTO)
        {
            var response = new ResponseRepository<GetBrandDTO>();
            var brand = brandDTO.ToBrand();
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
            var displayBrand = (GetBrandDTO)brand;
            response.Data = displayBrand;
            return response;
        }

        public async Task<ResponseRepository<List<GenericGetDTO>>> AddBrands(List<GenericUpdateDTO> genericUpdateDTOs)
        {
            var response = new ResponseRepository<List<GenericGetDTO>>();
            var brandsToAdd = genericUpdateDTOs.Select(brand => new Brand{BrandName = brand.Name}).ToList();
            _context.Brands.AddRange(brandsToAdd);
            await _context.SaveChangesAsync();
            var brands = brandsToAdd.Select(brand => new GenericGetDTO(brand.Id, brand.BrandName));
            response.Data = brands.ToList();
            return response;
        }

        public async Task<ResponseRepository<string>> DeleteBrand(int id)
        {
            var response = new ResponseRepository<string>();
            var failResponse = new FailResponse();
            var brand = await _context.Brands.FirstOrDefaultAsync(brand => brand.Id == id);
            if(brand is null){
               failResponse.GenerateFailResponse(response, "Brand not found");
                return response;
            }
            _context.Remove(brand);
            await _context.SaveChangesAsync();
            response.Data = "";
            return response;
        }

        public async Task<ResponseRepositoryWithCount<List<GetBrandDTO>>> GetAllBrands()
        {
            var response = new ResponseRepositoryWithCount<List<GetBrandDTO>>();
            var brands = await _context.Brands.ToListAsync();
            var displayBrands = brands.Select(brand => (GetBrandDTO)brand).ToList();
            response.Count = displayBrands.Count;
            response.Data = displayBrands;
            return response;
        }

        public async Task<ResponseRepository<GetBrandWithCategoriesDTO>> GetBrand(int id)
        {
            var response = new ResponseRepository<GetBrandWithCategoriesDTO>();
            var failResponse = new FailResponse();
            var existingBrand = await _context.Brands.Include(cat => cat.Categories).FirstOrDefaultAsync(brand => brand.Id == id);
            if(existingBrand is null){
                failResponse.GenerateFailResponse(response, "Brand not found");
                return response;
            }
            var brand = (GetBrandWithCategoriesDTO)existingBrand;
            response.Data = brand;
            return response;
        }

        public async Task<ResponseRepositoryWithCount<List<GetBrandDTO>>> GetPopularBrands()
        {
            var response = new ResponseRepositoryWithCount<List<GetBrandDTO>>();
            var brands = await _context.Brands.Include(category => category.Categories).Where(brand => brand.IsPopular == true).Select(b => new GetBrandDTO{Id = b.Id,Name=b.BrandName,ImageUrl=b.ImageUrl!}).ToListAsync();
            if(brands.Count == 0) response.Message = "No brands marked as popular";
            response.Count = brands.Count;
            response.Data = brands;
            return response;
        }

        public async Task<ResponseRepository<GetBrandDTO>> UpdateBrand(int id, GenericUpdateDTO genericUpdateDTO)
        {
            var response = new ResponseRepository<GetBrandDTO>();
            var failResponse = new FailResponse();
            var existingBrand = await _context.Brands.Include(cat => cat.Categories).FirstOrDefaultAsync(brand => brand.Id == id);
            if(existingBrand is null){
                failResponse.GenerateFailResponse(response, "Brand not found");
                return response;
            }
            existingBrand.BrandName = genericUpdateDTO.Name;
            existingBrand.IsPopular = (bool)genericUpdateDTO.IsPopular!;
            existingBrand.ImageUrl = genericUpdateDTO.ImageUrl!;
            await _context.SaveChangesAsync();
            var brand = (GetBrandDTO)existingBrand;
            response.Data = brand;
            return response;
        }
    }
}