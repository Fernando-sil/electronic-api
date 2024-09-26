using api.Controller;
using api.DTO.BrandDTO;
using api.DTO.CategoryDTO;
using api.DTO.GenericDTO;
using api.Interfaces;
using api.Repository;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace apiTest.Controllers;

public class CategoryControllerTest
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryControllerTest()
    {
        _categoryRepository = A.Fake<ICategoryRepository>();
    }

    [Fact]
    public async Task AddBrandsToCategories_ReturnSuccess()
    {
        var controller = new CategoryController(_categoryRepository);
        var result = await controller.AddBrandsToCategory(2,new List<AddBrandByIdDTO>());

        var actionResult = result.Result;

        SuccessResponse(actionResult!);
    }
    [Fact]
    public async Task AddBrandsToCategories_ReturnNotFound()
    {
        var controller = new CategoryController(_categoryRepository);
        A.CallTo(() => _categoryRepository.AddBrandsToCategory(2, new List<AddBrandByIdDTO>())).Returns(new ResponseRepository<GetCategoryDTO>{Success=false, Message="Categry not found", Data = new GetCategoryDTO()});
        var result = await controller.AddBrandsToCategory(2,new List<AddBrandByIdDTO>());

        var actionResult = result.Result;

        NotFoundResponse(actionResult!);
    }
    [Fact]
    public async Task GetAllCategories_ReturnSuccess(){
        var controller = new CategoryController(_categoryRepository);
        var result = await controller.GetAllCategories();

        var actionResult = result.Result;

        SuccessResponse(actionResult!);
    }
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task GetCategory_ReturnSuccess(int id){
        var controller = new CategoryController(_categoryRepository);
        var result = await controller.GetCategory(id);

        var actionResult = result.Result;

        SuccessResponse(actionResult!);
    }
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task GetCategory_ReturnNotFound(int id){
        var controller = new CategoryController(_categoryRepository);
        A.CallTo(() => _categoryRepository.GetCategory(id)).Returns(new ResponseRepository<GetCategoryDTO>{Success = false, Message = "Category not found.", Data = new GetCategoryDTO()});
        var result = await controller.GetCategory(id);

        var actionResult = result.Result;

       NotFoundResponse(actionResult!);
    }
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task DeleteCategory_ReturnSuccess(int id){
        var controller = new CategoryController(_categoryRepository);
        var result = await controller.DeleteCategory(id);

        var actionResult = result.Result;

        SuccessResponse(actionResult!);
    }
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task DeleteCategory_ReturnNotFound(int id){
        var controller = new CategoryController(_categoryRepository);
        A.CallTo(() => _categoryRepository.DeleteCategory(id)).Returns(new ResponseRepository<string>{Success = false, Message = "Category not found.", Data = ""});
        var result = await controller.DeleteCategory(id);

        var actionResult = result.Result;

       NotFoundResponse(actionResult!);
    }
    [Fact]
    public async Task AddCategories_ReturnSuccess(){
        var controller = new CategoryController(_categoryRepository);
        var result = await controller.AddCategories(new List<GenericUpdateDTO>());

        var actionResult = result.Result;

        SuccessResponse(actionResult!);
    }
    [Fact]
    public async Task UpdateCategory_ReturnSuccess(){
        var controller = new CategoryController(_categoryRepository);
        var result = await controller.UpdateCategory(1,new GenericUpdateDTO("category1", true,"imageUrl"));

        var actionResult = result.Result;

        SuccessResponse(actionResult!);
    }
    [Fact]
    public async Task UpdateCategory_ReturnNotFound(){
        var controller = new CategoryController(_categoryRepository);
        A.CallTo(() => _categoryRepository.UpdateCategory(new GenericUpdateDTO("category1", true,"imageUrl"),1)).Returns(new ResponseRepository<GenericGetDTO>{Success = false, Message = "Not Found", Data = new GenericGetDTO(1,"category1")});
        var result = await controller.UpdateCategory(1,new GenericUpdateDTO("category1", true,"imageUrl"));

        var actionResult = result.Result;

        NotFoundResponse(actionResult!);
    }

    private void SuccessResponse(ActionResult actionResult){
        actionResult.Should().NotBe(null);
        actionResult.Should().BeOfType(typeof(OkObjectResult));
    }
    private void NotFoundResponse(ActionResult actionResult){
        actionResult.Should().BeOfType(typeof(NotFoundObjectResult));
    }
}