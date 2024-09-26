using api.DTO.RatingDTO;
using api.Interfaces;
using api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controller;

[ApiController]
[Route("api/ratings")]
public class RatingController : ControllerBase
{
    private readonly IRatingRepository _ratingRepository;

    public RatingController(IRatingRepository ratingRepository)
    {
        _ratingRepository = ratingRepository;
    }
    [Authorize]
    [HttpPost("add-rating")]
    public async Task<ActionResult<ResponseRepository<GetRatingDTO>>> AddRating(AddRatingDTO addRatingDTO){
        var response = await _ratingRepository.AddRating(addRatingDTO);
       
        if(!response.Success) {
            if(response.Message.Contains("User")) return Unauthorized(response);
            return NotFound(response);
        }
        return Ok(response);
    }
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ResponseRepository<string>>> DeleteRating(int id){
        var deleteRating = await _ratingRepository.DeleteRating(id);
        if(!deleteRating.Success){
            if(deleteRating.Message.Contains("User")) return Unauthorized(deleteRating);
            return NotFound(deleteRating);
        }
        return Ok(deleteRating);
    }
    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<ResponseRepository<UpdateRatingDTO>>> UpdateRating([FromRoute]int id, [FromBody] UpdateRatingDTO updateRatingDTO){
        var updateRating = await _ratingRepository.UpdateRating(updateRatingDTO, id);
        if(!updateRating.Success){
            if(updateRating.Message.Contains("User")) return Unauthorized(updateRating);
            return NotFound(updateRating);
        }
        return Ok(updateRating);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ResponseRepository<GetRatingDTO>>> GetRating([FromRoute]int id){
        var updateRating = await _ratingRepository.GetRating(id);
        if(!updateRating.Success){
            if(updateRating.Message.Contains("User")) return Unauthorized(updateRating);
            return NotFound(updateRating);
        }
        return Ok(updateRating);
    }
}