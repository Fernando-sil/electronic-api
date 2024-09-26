using System.ComponentModel.DataAnnotations;
using api.Entities;

namespace api.DTO.RatingDTO;

public class AddRatingDTO
{
    [Range(1,5)]
    public double Rate { get; set; }
    [Required]
    [MinLength(3, ErrorMessage ="Minimum number of characters is 3")]
    public string Comment { get; set; } = string.Empty;
    public Guid ItemId { get; set; }

    public Rating ToRating(){
        return new Rating{
            Rate = Rate,
            Comment = Comment
        };
    }
}