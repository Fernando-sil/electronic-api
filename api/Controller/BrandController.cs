using api.DTO.BrandDTO;
using api.DTO.GenericDTO;
using api.Interfaces;
using api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controller;

[ApiController]
[Route("api/[controller]")]
public class BrandController : ControllerBase
{
    private readonly IBrandRepository _brandRepository;

    public BrandController(IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }

    [HttpPost("add-brand")]
    public async Task<ActionResult<ResponseRepository<GetBrandDTO>>> AddBrand([FromBody] BrandDTO brandDTO){
        var response = await _brandRepository.AddBrand(brandDTO);
        return Ok(response);
    }
    [HttpPost("add-brands"),Authorize(Roles ="Admin")]
        public async Task<ActionResult<ResponseRepository<List<GenericGetDTO>>>> AddBrands([FromBody] List<GenericUpdateDTO> genericUpdateDTOs){
        var response = await _brandRepository.AddBrands(genericUpdateDTOs);
        return Ok(response);
    }
    [HttpGet("all-brands")]
    public async Task<ActionResult<ResponseRepositoryWithCount<List<GetBrandDTO>>>> GetAllBrands(){
        var brands = await _brandRepository.GetAllBrands();
        return Ok(brands);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ResponseRepository<GetBrandWithCategoriesDTO>>> GetBrand(int id){
        var response = await _brandRepository.GetBrand(id);
        if(!response.Success) return NotFound(response);
        return Ok(response);
    }
    [HttpPut("{id}"),Authorize(Roles ="Admin")]
    public async Task<ActionResult<ResponseRepository<GetBrandDTO>>> UpdateBrand(int id, GenericUpdateDTO genericUpdateDTO){
        var response = await _brandRepository.UpdateBrand(id,genericUpdateDTO);
        if(!response.Success) return BadRequest(response);
        return response;
    }
    [HttpGet("popular-brands")]
    public async Task<ActionResult<ResponseRepositoryWithCount<List<GetBrandDTO>>>> GetPopularBrands(){
        var response = await _brandRepository.GetPopularBrands();
        return Ok(response);
    }
    [HttpDelete("{id}"),Authorize(Roles ="Admin")]
    public async Task<ActionResult<ResponseRepository<string>>> DeleteBrand([FromRoute] int id){
        var response = await _brandRepository.DeleteBrand(id);
        if(!response.Success) return BadRequest(response);
        return response;
    }
}