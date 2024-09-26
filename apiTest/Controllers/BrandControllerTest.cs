
using api.Controller;
using api.DTO.GenericDTO;
using api.Interfaces;
using api.Repository;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace apiTest.Controllers;

public class BrandControllerTest
{
    private readonly IBrandRepository _brandRepository;

    public BrandControllerTest()
    {
        _brandRepository = A.Fake<IBrandRepository>();
    }

    [Fact]
    public async Task GetBrands_ReturnSuccess()
    {
        var controller = new BrandController(_brandRepository);

        var result = await controller.GetAllBrands();

        var actionResult = result.Result;
        
        actionResult.Should().NotBeNull();
        actionResult.Should().BeOfType(typeof(OkObjectResult));
    }
    // [Fact]
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public async Task GetBrand_ReturnSuccess(int id)
    {
        var controller = new BrandController(_brandRepository);
        // var brandId = 1;

        var result = await controller.GetBrand(id);

        var actionResult = result.Result;
        
        actionResult.Should().NotBeNull();
        actionResult.Should().BeOfType(typeof(OkObjectResult));
    }
    [Fact]
    public async Task GetBrand_ReturnNotFound()
    {
        var controller = new BrandController(_brandRepository);
        // var brandId = 1;

        var result = await controller.GetBrand(78952);

        var actionResult = result.Result as NotFoundObjectResult;
        
        // actionResult.Should().NotBeNull();
        actionResult?.StatusCode.Should().Be(404);
    }
    [Fact]
    public async Task AddBrands_ReturnSuccess_And_CorrectCount()
    {
        var controller = new BrandController(_brandRepository);
        var brands = new List<GenericUpdateDTO>(){
            new GenericUpdateDTO("Brand 1", true, "image"),
            new GenericUpdateDTO("Brand 2", false, "image"),
            new GenericUpdateDTO("Brand 3", true, "image"),
        };

        var result = await controller.AddBrands(brands);

        var actionResult = result.Result;
        var valueResult = result.Value;
        
        actionResult.Should().NotBeNull();
        actionResult.Should().BeOfType(typeof(OkObjectResult));
        valueResult?.Data?.Count.Should().Be(3);
    }
    [Theory]
    [InlineData(1,"brand1", false, "imageUrl")]
    [InlineData(2,"brand2", true, "imageUrl")]
    [InlineData(3,"brand3", true, "imageUrl")]
    [InlineData(4,"brand4", false, "imageUrl")]
    public async Task UpdateBrand_ReturnSuccess(int id,string name, bool isPopular, string imageUrl)
    {
        var controller = new BrandController(_brandRepository);
        var result = await controller.UpdateBrand(id,new GenericUpdateDTO(name, isPopular, imageUrl));
        var valueResult = result?.Value?.Success;
        
        valueResult.Should().Be(true);
        valueResult.Should().NotBe(false);
       
    }
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public async Task DeleteBrand_ReturnSuccess(int id)
    {
        var controller = new BrandController(_brandRepository);
        var result = await controller.DeleteBrand(id);
        var valueResult = result?.Value?.Success;
        
        valueResult.Should().Be(true);
        valueResult.Should().NotBe(false);
       
    }
    [Fact]
    public async Task DeleteBrand_ReturnFail()
    {
        var controller = new BrandController(_brandRepository);
        A.CallTo(()=> _brandRepository.DeleteBrand(1)).Returns(new ResponseRepository<string>{Success = false, Message = "Brand not found.", Data=""});
        var result = await controller.DeleteBrand(1);

        var actionResult = result.Result as NotFoundObjectResult;
        
        // actionResult.Should().NotBeNull();
        actionResult?.StatusCode.Should().Be(404);
    }
}