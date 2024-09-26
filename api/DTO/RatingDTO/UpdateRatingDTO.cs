using System.ComponentModel.DataAnnotations;
using api.Entities;

namespace api.DTO.RatingDTO;

public class UpdateRatingDTO
{
    [Range(1,5)]
    public double Rate { get; set; }
    [Required]
    [MinLength(3, ErrorMessage ="Minimum number of characters is 3")]
    public string Comment { get; set; } = string.Empty;

    public static explicit operator UpdateRatingDTO(Rating rating){
        return new UpdateRatingDTO{
            Rate = rating.Rate,
            Comment = rating.Comment
        };
    }
}