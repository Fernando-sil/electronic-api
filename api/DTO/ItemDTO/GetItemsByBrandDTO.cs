using api.DTO.GenericDTO;

namespace api.DTO.ItemDTO;

public record GetItemsByBrandDTO(string Brand,List<GenericGetDTO> Categories,List<GetItemsOnSaleDTO> Items);

