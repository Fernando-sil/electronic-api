using api.DataContext;
using api.DTO.ItemDTO;
using api.DTO.SpecificationItemDTO;
using api.Entities;
using api.Helpers;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class SpecificationItemRepository : ISpecificationItem
{
    private readonly DatabaseContext _context;

    public SpecificationItemRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<ResponseRepository<GetItemDTO>> AddSpecToItem(AddSpecificationItemDTO addSpecificationItemDTO)
    {
        var response = new ResponseRepository<GetItemDTO>();
        var failResponse = new FailResponse();
        var item = await _context.Items.FirstOrDefaultAsync(item => item.Id == addSpecificationItemDTO.ItemId);
        if(item is null) {
            failResponse.GenerateFailResponse(response, "Item not found");
            return response;
            }
        
        var spec = await _context.Specifications.FirstOrDefaultAsync(spec => spec.Id == addSpecificationItemDTO.SpecificationId);
        if(spec is null){
            failResponse.GenerateFailResponse(response, "Specification not found");
            return response;
        }

        var itemSpecifications = new SpecificationItem{
            ItemId = item!.Id,
            SpecificationId = spec!.Id,
            Value = addSpecificationItemDTO.Value
        };

        item.ItemSpecifications!.Add(itemSpecifications);
        await _context.SaveChangesAsync();
        response.Data = (GetItemDTO)item;
        return response;
    }
}