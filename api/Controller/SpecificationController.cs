using api.DTO.GenericDTO;
using api.DTO.SpecificationDTO;
using api.Entities;
using api.Interfaces;
using api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controller;

[ApiController]
[Route("api/specs")]
public class SpecificationController : ControllerBase
{
    private readonly ISpecification _specification;

    public SpecificationController(ISpecification specification)
    {
        _specification = specification;
    }

    [Authorize(Roles = "admin")]
    [HttpPost("add-spec")]
    public async Task<ActionResult<ResponseRepository<Specification>>> AddSpecification([FromBody]AddSpecificationDTO addSpecificationDTO){
        var specification = await _specification.AddSpec(addSpecificationDTO);
        return Ok(specification);
    }
    [Authorize(Roles = "Admin")]
    [HttpPost("add-specs")]
    public async Task<ActionResult<ResponseRepository<List<GenericGetDTO>>>> AddSpecifications([FromBody]List<GenericUpdateDTO> genericUpdateDTOs){
        var specification = await _specification.AddSpecs(genericUpdateDTOs);
        return Ok(specification);
    }
    [Authorize]
    // [Authorize(Roles = "admin")]
    [HttpGet]
    public async Task<ActionResult<ResponseRepositoryWithCount<List<GenericGetDTO>>>> GetSpecs(){
        var response = await _specification.GetAllSpecs();
        return Ok(response);
    }
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<ResponseRepository<GenericGetDTO>>> GetSpec(int id){
        var response = await _specification.GetSpec(id);
        if(!response.Success) return NotFound(response);
        return Ok(response);
    }
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ResponseRepository<string>>> DeleteSpec(int id){
        var response = await _specification.DeleteSpec(id);
        if(!response.Success) return NotFound(response);
        return Ok(response);
    }
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ResponseRepository<GenericGetDTO>>> UpdateSpec([FromRoute]int id, [FromBody] GenericUpdateDTO genericUpdateDTO){
        var response = await _specification.UpdateSpec(genericUpdateDTO, id);
        if(!response.Success) return NotFound(response);
        return Ok(response);
    }

}