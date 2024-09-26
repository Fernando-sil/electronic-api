using api.DTO.BrandDTO;
using api.DTO.CategoryDTO;
using api.DTO.GenericDTO;
using api.Interfaces;
using api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controller;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    [HttpPost("{id}/add-brand-to-category")]
    public async Task<ActionResult<ResponseRepository<GetCategoryDTO>>> AddBrandsToCategory([FromRoute]int id,[FromBody] List<AddBrandByIdDTO> addBrandByIdDTOs){
        var newBrand = await _categoryRepository.AddBrandsToCategory(id, addBrandByIdDTOs);
        if(!newBrand.Success) return NotFound(newBrand);
        return Ok(newBrand);
    }
    [HttpGet("all-categories")]
    public async Task<ActionResult<ResponseRepositoryWithCount<List<GenericGetDTO>>>> GetAllCategories(){
        var categories = await _categoryRepository.GetAllCategories();
        return Ok(categories);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ResponseRepository<GetCategoryDTO>>> GetCategory(int id){
        var category = await _categoryRepository.GetCategory(id);
        if(!category.Success) return NotFound(category);
        return Ok(category);
    }
    [Authorize(Roles ="Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<string>> DeleteCategory(int id){
        var category = await _categoryRepository.DeleteCategory(id);
        if(!category.Success) return NotFound(category);
        return Ok(category);
    }
    [Authorize(Roles ="Admin")]
    [HttpPost("add-categories")]
    public async Task<ActionResult<ResponseRepositoryWithCount<List<GenericGetDTO>>>> AddCategories(List<GenericUpdateDTO> genericUpdateDTOs){
        var categoies = await _categoryRepository.AddCategories(genericUpdateDTOs);
        return Ok(categoies);
    }
    [Authorize(Roles ="Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ResponseRepository<GetBrandDTO>>> UpdateCategory(int id, GenericUpdateDTO genericUpdateDTO){
        var response = await _categoryRepository.UpdateCategory(genericUpdateDTO,id);
        if(!response.Success) return NotFound(response);
        return Ok(response);
    }
}