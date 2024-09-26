using api.Entities;

namespace api.DTO.RatingDTO;

public class GetRatingDTO
{
    public int Id { get; set; }
    public double Rate { get; set; }
    public string Comment { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;

    public static explicit operator GetRatingDTO(Rating rating){
        return new GetRatingDTO{
            Id = rating.Id,
            Rate = rating.Rate,
            Comment = rating.Comment,
            UserName = rating.User!.UserName,
            // ItemName = rating.Item!.ItemName
        };
    }
}