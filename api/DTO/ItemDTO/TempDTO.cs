namespace api.DTO.ItemDTO;

public class TempDTO
{
    public Guid Id { get; set; }
public string ItemName { get; set; } = string.Empty;
public string ImageUrl { get; set; } = string.Empty;
public decimal Price { get; set; }
public double Score { get; set; }
public string BrandName { get; set; } = string.Empty;
}