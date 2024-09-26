using api.DTO.BrandDTO;
using api.DTO.GenericDTO;

namespace api.DTO.ItemDTO;

public record GetItemsByCategoryDTO(string Category,List<GetBrandDTO> Brands, List<TempDTO> Items);