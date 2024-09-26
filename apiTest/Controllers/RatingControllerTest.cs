using api.Controller;
using api.DTO.RatingDTO;
using api.Interfaces;
using api.Repository;
using apiTest.Helpers;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;

namespace apiTest.Controllers;

public class RatingControllerTest
{
    private readonly IRatingRepository _ratingRepository;

    public RatingControllerTest()
    {
        _ratingRepository = A.Fake<IRatingRepository>();
    }

    [Theory]
    [InlineData(1.2,"comment1","8c476b1a-ec19-401f-a83b-07de6865b8cd")]
    [InlineData(4.9,"comment1","e79e819e-068d-4358-b88f-dc312ffdb79e")]
    [InlineData(3.5,"comment1","ff13d94f-7590-4c61-8cbd-f52e3a372c3b")]
    public async Task AddRating_ReturnSuccess(double rate, string comment, Guid itemId)
    {
        var controller = new RatingController(_ratingRepository);
        var response = new ActionResultResponse();
        var result = await controller.AddRating(new AddRatingDTO{Rate = rate, Comment = comment, ItemId = itemId});

        var actionResult = result.Result;

        response.SuccessResponse(actionResult!);
    }
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task DeleteRating_ReturnSuccess(int id)
    {
        var controller = new RatingController(_ratingRepository);
        var response = new ActionResultResponse();
        var result = await controller.DeleteRating(id);

        var actionResult = result.Result;

        response.SuccessResponse(actionResult!);
    }
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task DeleteRating_ReturnNotFound(int id)
    {
        var controller = new RatingController(_ratingRepository);
        var response = new ActionResultResponse();
        A.CallTo(() => _ratingRepository.DeleteRating(id)).Returns(new ResponseRepository<string>{Success = false, Message = "Rating not found", Data = ""});
        var result = await controller.DeleteRating(id);

        var actionResult = result.Result;

        response.NotFoundResponse(actionResult!);
    }
    [Fact]
    public async Task UpdateRating_ReturnSuccess(){
        var controller = new RatingController(_ratingRepository);
        var response = new ActionResultResponse();
        var result = await controller.UpdateRating(1,new UpdateRatingDTO());

        var actionResult = result.Result;

        response.SuccessResponse(actionResult!);
    }
    [Fact]
    public async Task GetRating_ReturnSuccess(){
        var controller = new RatingController(_ratingRepository);
        var response = new ActionResultResponse();
        var result = await controller.GetRating(1);

        var actionResult = result.Result;
        

        response.SuccessResponse(actionResult!);
    }
    [Fact]
    public async Task GetRating_ReturnNotFound(){
        var controller = new RatingController(_ratingRepository);
        var response = new ActionResultResponse();
        A.CallTo(() => _ratingRepository.GetRating(1)).Returns(new ResponseRepository<GetRatingDTO>{Success = false, Message = "Not found", Data = new GetRatingDTO()});
        var result = await controller.GetRating(1);

        var actionResult = result.Result;
        

        response.NotFoundResponse(actionResult!);
    }
}