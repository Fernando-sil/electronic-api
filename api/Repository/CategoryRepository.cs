using api.DataContext;
using api.DTO.BrandDTO;
using api.DTO.CategoryDTO;
using api.DTO.GenericDTO;
using api.Entities;
using api.Helpers;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class CategoryRepository : ICategoryRepository
{
    private readonly DatabaseContext _context;

    public CategoryRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<ResponseRepository<GetCategoryDTO>> AddBrandsToCategory(int categoryId, List<AddBrandByIdDTO> addBrandByIdDTOs)
    {
        var response = new ResponseRepository<GetCategoryDTO>();
        var failResponse = new FailResponse();
        var existingCategory = await _context.Categories.Include(brand => brand.Brands).FirstOrDefaultAsync(category => category.Id == categoryId);
        if(existingCategory is null){
            failResponse.GenerateFailResponse(response, "Category not found");
            return response;
        }
        var test = addBrandByIdDTOs.Select(b=>b.Id);
        var allBrands = await _context.Brands.Where(brand => test.Contains(brand.Id)).ToListAsync();
        if(allBrands.Count == 0){
            failResponse.GenerateFailResponse(response, "One or more brands not found");
            return response;
        }
        existingCategory.Brands = allBrands;
        var category = (GetCategoryDTO)existingCategory;
        await _context.SaveChangesAsync();
        response.Data = category;
        return response;
    }

    public async Task<ResponseRepositoryWithCount<List<GenericGetDTO>>> AddCategories(List<GenericUpdateDTO> genericUpdateDTOs)
    {
        var response = new ResponseRepositoryWithCount<List<GenericGetDTO>>();
        var categories = genericUpdateDTOs.Select(category => new Category{CategoryName = category.Name});
        _context.Categories.AddRange(categories);
        await _context.SaveChangesAsync();
        var categoriesResponse = categories.Select(category => (GenericGetDTO)category).ToList();
        response.Count = categoriesResponse.Count;
        response.Data = categoriesResponse;
        return response;
    }

    public async Task<ResponseRepository<string>> DeleteCategory(int id)
    {
        var response = new ResponseRepository<string>();
        var failResponse = new FailResponse();
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
         if(category is null){
            failResponse.GenerateFailResponse(response, "Category not found");
            return response;
        }
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        response.Data = "";
        response.Message = $"Category {category.CategoryName} deleted";
        return response;
    }

    public async Task<ResponseRepositoryWithCount<List<GenericGetDTO>>> GetAllCategories()
    {
        var response = new ResponseRepositoryWithCount<List<GenericGetDTO>>();
        var existingCategories = await _context.Categories.ToListAsync();
        var categories = existingCategories.Select(category => (GenericGetDTO)category).ToList();
        response.Count = categories.Count;
        response.Data = categories;
        return response;
    }

    public async Task<ResponseRepository<GetCategoryDTO>> GetCategory(int id)
    {
        var response = new ResponseRepository<GetCategoryDTO>();
        var failResponse = new FailResponse();
        var category = await _context.Categories.Include(brand => brand.Brands).FirstOrDefaultAsync(c => c.Id == id);
        if(category is null){
            failResponse.GenerateFailResponse(response, "Category not found");
            return response;
        }
        var displayCategory = (GetCategoryDTO)category;
        response.Data = displayCategory;
        return response;
    }

    public async Task<ResponseRepository<GenericGetDTO>> UpdateCategory(GenericUpdateDTO genericUpdateDTO, int id)
    {
        var response = new ResponseRepository<GenericGetDTO>();
        var failResponse = new FailResponse();
        var exisitingCategory = await _context.Categories.FirstOrDefaultAsync(category => category.Id ==id);
        if(exisitingCategory is null){
            failResponse.GenerateFailResponse(response, "Category not found");
            return response;
        }
        exisitingCategory.CategoryName = genericUpdateDTO.Name;
        await _context.SaveChangesAsync();
        var updatedCategory = (GenericGetDTO)exisitingCategory;
        response.Data = updatedCategory;
        return response;
    }
}