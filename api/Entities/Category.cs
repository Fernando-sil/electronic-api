
namespace api.Entities;

public class Category
{
    public int Id { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public List<Brand>? Brands { get; set; }
}