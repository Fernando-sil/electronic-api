namespace api.Entities;

public class Brand
{
    public int Id { get; set; }
    public string BrandName { get; set; } = string.Empty;
    public bool IsPopular { get; set; } = false;
    public string? ImageUrl { get; set; } = "https://upload.wikimedia.org/wikipedia/commons/thumb/4/46/Question_mark_%28black%29.svg/800px-Question_mark_%28black%29.svg.png";
    public List<Category>? Categories { get; set; }
}