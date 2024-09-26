using api.DTO.ItemDTO;
using api.DTO.SpecificationItemDTO;
using api.Helpers;
using api.Repository;


namespace api.Interfaces;

public interface IItemRepository
{
    Task<ResponseRepository<GetItemDTO>> AddItem(AddItemDTO addItemDTO);
    Task<ResponseRepository<GetItemDTO>> AddSpecsToItem(List<AddSpecficationsItemDTO> addSpecficationsItemDTO,Guid itemId);
    Task<ResponseRepository<GetItemByIdDTO>> GetItemById(Guid id);
    Task<ResponseRepositoryWithCount<List<GetItemsDTO>>> GetAllItems(QueryHelper query);
    Task<ResponseRepository<GetItemByIdDTO>> UpdateItem(UpdateItemDTO updateItemDTO, Guid id);
    Task<ResponseRepository<string>> DeleteItem(Guid id);
    Task<ResponseRepositoryWithCount<List<GetItemsOnSaleDTO>>> GetItemsOnSale();
    Task<ResponseRepositoryWithCount<GetItemsByCategoryDTO>> GetItemsByCategory(int categoryId, QueryHelper query);
    Task<ResponseRepositoryWithCount<GetItemsByBrandDTO>> GetItemsByBrand(int BrandId, QueryHelper query);
}