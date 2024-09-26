namespace api.DTO.RatingDTO;

public class TestRatingDTO
{
    public int Id { get; set; }
    public double Rate { get; set; }
    public string Comment { get; set; } = string.Empty;
    public Guid UserId { get; set; }
}