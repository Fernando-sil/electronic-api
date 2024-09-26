namespace api.Entities;

public class Rating
{
    public int Id { get; set; }
    public double Rate { get; set; }
    public string Comment { get; set; } = string.Empty;
    public Item? Item { get; set; }
    public Guid ItemId { get; set; }
    public User? User { get; set; }
    public Guid UserId { get; set; }
}