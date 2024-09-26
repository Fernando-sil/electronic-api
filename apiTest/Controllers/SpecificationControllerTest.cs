using api.Controller;
using api.DTO.GenericDTO;
using api.Interfaces;
using api.Repository;
using apiTest.Helpers;
using FakeItEasy;

namespace apiTest.Controllers;

public class SpecificationControllerTest
{
    private readonly ISpecification _specification;

    public SpecificationControllerTest()
    {
        _specification = A.Fake<ISpecification>();
    }

    [Fact]
    public async Task AddSpecifications_ReturnSuccess()
    {
        var controller = new SpecificationController(_specification);
        var response = new ActionResultResponse();
        var specs = new List<GenericUpdateDTO>{
            new GenericUpdateDTO("spec1"),
            new GenericUpdateDTO("spec2"),
            new GenericUpdateDTO("spec3")
        };
        var result = await controller.AddSpecifications(specs);

        var actionResult = result.Result;

        response.SuccessResponse(actionResult!);
    }
    [Fact]
    public async Task GetSpecs_ReturnSuccess(){
        var controller = new SpecificationController(_specification);
        var response = new ActionResultResponse();
        var result = await controller.GetSpecs();

        var actionResult = result.Result;

        response.SuccessResponse(actionResult!);
    }
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task GetSpec_ReturnSuccess(int id){
        var controller = new SpecificationController(_specification);
        var response = new ActionResultResponse();
        var result = await controller.GetSpec(id);

        var actionResult = result.Result;

        response.SuccessResponse(actionResult!);
    }
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task GetSpec_ReturnNotFound(int id){
        var controller = new SpecificationController(_specification);
        var response = new ActionResultResponse();
        A.CallTo(() => _specification.GetSpec(id)).Returns(new ResponseRepository<GenericGetDTO>{Success = false, Data = new GenericGetDTO(1, "name1"), Message = "Not found"});
        var result = await controller.GetSpec(id);

        var actionResult = result.Result;

        response.NotFoundResponse(actionResult!);
    }
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task DeleteSpec_ReturnSuccess(int id){
        var controller = new SpecificationController(_specification);
        var response = new ActionResultResponse();
        var result = await controller.DeleteSpec(id);

        var actionResult = result.Result;

        response.SuccessResponse(actionResult!);
    }
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task DeleteSpec_ReturnNotFound(int id){
        var controller = new SpecificationController(_specification);
        var response = new ActionResultResponse();
        A.CallTo(() => _specification.DeleteSpec(id)).Returns(new ResponseRepository<string>{Success = false, Data = "", Message = "Not found"});
        var result = await controller.DeleteSpec(id);

        var actionResult = result.Result;

        response.NotFoundResponse(actionResult!);
    }
    [Fact]
    public async Task UpdateSpecs_ReturnSuccess(){
        var controller = new SpecificationController(_specification);
        var response = new ActionResultResponse();
        var result = await controller.UpdateSpec(1, new GenericUpdateDTO("spec1"));

        var actionResult = result.Result;

        response.SuccessResponse(actionResult!);
    }
    [Fact]
    public async Task UpdateSpecs_ReturnNotFound(){
        var controller = new SpecificationController(_specification);
        var response = new ActionResultResponse();
        A.CallTo(() => _specification.UpdateSpec(new GenericUpdateDTO("spec1",false,""),1)).Returns(new ResponseRepository<GenericGetDTO>{Success = false, Message = "not found.", Data = new GenericGetDTO(1, "spec1")});
        var result = await controller.UpdateSpec(1, new GenericUpdateDTO("spec1"));

        var actionResult = result.Result;

        response.NotFoundResponse(actionResult!);
    }
}