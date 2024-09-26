using api.DTO.RatingDTO;
using api.DTO.SpecificationDTO;
using api.Entities;

namespace api.DTO.ItemDTO;

public class GetItemDTO
{
    public Guid Id { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    // public double Score { get; set; }
    // public GetSpecificationDTO? ItemSpecification { get; set; }
    public List<GetSpecificationDTO>? ItemSpecifications { get; set; }

    public static explicit operator GetItemDTO(Item item){
        return new GetItemDTO{
            Id = item.Id,
            ItemName = item.ItemName,
            Description = item.Description,
            Quantity = item.Quantity,
            Price = item.Price,
            ItemSpecifications = new List<GetSpecificationDTO>()
        };
    }
    
}