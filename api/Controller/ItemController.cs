using api.DTO.ItemDTO;
using api.DTO.SpecificationItemDTO;
using api.Helpers;
using api.Interfaces;
using api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controller;

[ApiController]
[Route("api/[controller]")]
public class ItemController : ControllerBase
{
    private readonly IItemRepository _itemRepository;

    public ItemController(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }
    [Authorize(Roles ="Admin")]
    [HttpPost("add-item")]
    public async Task<ActionResult<ResponseRepository<GetItemDTO>>> AddItem(AddItemDTO addItemDTO){
        var response = await _itemRepository.AddItem(addItemDTO);
        return Ok(response);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ResponseRepository<GetItemByIdDTO>>> GetitemById(Guid id){
        var item = await _itemRepository.GetItemById(id);
        if(!item.Success) return NotFound(item);
        return Ok(item);
    }
    [HttpGet("all-items")]
    public async Task<ActionResult<ResponseRepositoryWithCount<List<GetItemsDTO>>>> GetAllItems([FromQuery] QueryHelper query){
        var response = await _itemRepository.GetAllItems(query);
        return Ok(response);
    }
    [HttpPut("{id}"),Authorize(Roles ="Admin")]
    public async Task<ActionResult<ResponseRepository<GetItemByIdDTO>>> UpdateItem([FromBody] UpdateItemDTO updateItemDTO, [FromRoute] Guid id){
        var response = await _itemRepository.UpdateItem(updateItemDTO, id);
        if(!response.Success) return NotFound(response);
        return Ok(response);
    }
    [HttpDelete("{id}"), Authorize(Roles ="Admin")]
    public async Task<ActionResult<ResponseRepository<string>>> DeleteItem([FromRoute] Guid id){
        var response =  await _itemRepository.DeleteItem(id);
        if(!response.Success) return NotFound(response);
        return Ok(response);
    }
    [HttpGet("items-on-sale")]
    public async Task<ActionResult<ResponseRepositoryWithCount<GetItemsOnSaleDTO>>> GetItemsOnSale(){
        var response = await _itemRepository.GetItemsOnSale();
        return Ok(response);
    }
    [HttpPost("{id}/add-specs-to-item"), Authorize(Roles ="Admin")]
    public async Task<ActionResult<ResponseRepository<GetItemDTO>>> AddSpecsToItem([FromBody] List<AddSpecficationsItemDTO> addSpecficationsItemDTO, [FromRoute] Guid id){
        var response = await _itemRepository.AddSpecsToItem(addSpecficationsItemDTO, id);
        if(!response.Success) return NotFound(response);
        return response;
    }
    [HttpGet("category/{id}")]
    public async Task<ActionResult<ResponseRepositoryWithCount<GetItemsByCategoryDTO>>> GetItemsByCategory([FromRoute] int id, [FromQuery] QueryHelper query){
        var response = await _itemRepository.GetItemsByCategory(id, query);
        if(!response.Success) return NotFound(response);
        return Ok(response);
    }
    [HttpGet("brand/{id}")]
    public async Task<ActionResult<ResponseRepositoryWithCount<GetItemsByBrandDTO>>> GetItemsByBrand([FromRoute] int id, [FromQuery] QueryHelper query){
        var response = await _itemRepository.GetItemsByBrand(id, query);
        if(!response.Success) return NotFound(response);
        return Ok(response);
    }
}