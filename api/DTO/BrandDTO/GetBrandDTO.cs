using api.Entities;

namespace api.DTO.BrandDTO;

public class GetBrandDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsPopular { get; set; }
    public string ImageUrl { get; set; } = string.Empty;

    public static explicit operator GetBrandDTO(Brand brand){
        return new GetBrandDTO{
            Id = brand.Id,
            Name = brand.BrandName,
            IsPopular = brand.IsPopular,
            ImageUrl = brand.ImageUrl ?? ""
        };
    }

    public Brand ToBrand(){
        return new Brand{
            Id = Id,
            BrandName = Name,
            IsPopular = IsPopular,
            ImageUrl = ImageUrl
        };
    }
}