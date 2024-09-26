using api.DTO.BrandDTO;
using api.DTO.CategoryDTO;
using api.DTO.RatingDTO;
using api.DTO.SpecificationDTO;
using api.Entities;

namespace api.DTO.ItemDTO;

public class GetItemByIdDTO
{
    public Guid Id { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public double AverageRating { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public GetItemCategoryDTO? ItemCategory { get; set; }
    public GetBrandDTO? ItemBrand { get; set; }
    public List<GetSpecificationDTO>? ItemSpecifications { get; set; }
    public List<GetItemRatingDTO>? Ratings { get; set; }

    public static explicit operator GetItemByIdDTO(Item item){
        var itemRating = item.Ratings;
        return new GetItemByIdDTO{
            Id = item.Id,
            ItemName = item.ItemName,
            Description = item.Description,
            Quantity = item.Quantity,
            Price = item.Price,
            AverageRating = (double)item.Score!,
            ImageUrl = item.ImageUrl,
            ItemCategory = (GetItemCategoryDTO)item.Category! ?? default,
            ItemBrand = (GetBrandDTO)item.Brand! ?? default,
            ItemSpecifications = item.ItemSpecifications?.Select(spec => (GetSpecificationDTO)spec).ToList() ?? [],
            Ratings = item.Ratings!.Select(rating => (GetItemRatingDTO)rating).ToList() ?? new List<GetItemRatingDTO>()
        };
    }
}