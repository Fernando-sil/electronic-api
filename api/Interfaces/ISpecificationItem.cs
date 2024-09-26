using api.DTO.ItemDTO;
using api.DTO.SpecificationItemDTO;
using api.Repository;

namespace api.Interfaces;

public interface ISpecificationItem
{
    Task<ResponseRepository<GetItemDTO>> AddSpecToItem(AddSpecificationItemDTO addSpecificationItemDTO);
}