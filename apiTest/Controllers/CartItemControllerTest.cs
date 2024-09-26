using api.Controller;
using api.DTO.CartItemDTO;
using api.Interfaces;
using api.Repository;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace apiTest.Controllers;

public class CartItemControllerTest
{
    private readonly ICartItemRepository _cartItemRepository;

    public CartItemControllerTest()
    {
        _cartItemRepository = A.Fake<ICartItemRepository>();
    }

    [Fact]
    public async void AddCartItems_ReturnSuccess()
    {
        var controller = new CartItemController(_cartItemRepository);
        var result = await controller.AddCartItems(new AddCartItemsBulk());
        
        var actionResult = result.Result;

        actionResult.Should().NotBe(null);
        actionResult.Should().BeOfType(typeof(OkObjectResult));
    }
    [Fact]
    public async void UpdateCartItems_ReturnSuccess()
    {
        var controller = new CartItemController(_cartItemRepository);
        var result = await controller.UpdateCartItems(new AddCartItemsBulk());
        
        var actionResult = result.Result;

        actionResult.Should().NotBe(null);
        actionResult.Should().BeOfType(typeof(OkObjectResult));
    }
}