using api.DTO.RatingDTO;
using api.Entities;
using api.Repository;

namespace api.Interfaces;

public interface IRatingRepository
{
    Task<ResponseRepository<GetRatingDTO>> AddRating(AddRatingDTO addRatingDTO);
    Task<ResponseRepository<string>> DeleteRating(int id);
    Task<ResponseRepository<UpdateRatingDTO>> UpdateRating(UpdateRatingDTO updateRatingDTO, int id);
    Task<ResponseRepository<GetRatingDTO>> GetRating(int id);
}