using api.Controller;
using api.Interfaces;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace apiTest.Controllers;

public class CartControllerTest
{
    private readonly ICartRepository _cartRepository;

    public CartControllerTest()
    {
        _cartRepository = A.Fake<ICartRepository>();
    }

    [Fact]
    public async Task CreateCart_ReturnResponse()
    {
        var controller = new CartController(_cartRepository);
        var result = await controller.CreateCart();
        var actionResult = result.Result;
        
        actionResult.Should().NotBeNull();
        actionResult.Should().BeOfType(typeof(OkObjectResult));
    }
    [Fact]
    public async Task GetUserCart_ReturnResponse()
    {
        var controller = new CartController(_cartRepository);
        var result = await controller.GetUserCart();
        var actionResult = result.Result;
        
        actionResult.Should().NotBeNull();
        actionResult.Should().BeOfType(typeof(OkObjectResult));
    }
}