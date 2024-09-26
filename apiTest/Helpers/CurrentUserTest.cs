using System.Security.Claims;
using api.Entities;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;


namespace apiTest.Helpers;

public class CurrentUserTest
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserTest()
    {
        _httpContextAccessor = A.Fake<IHttpContextAccessor>();
    }

    [Fact]
    public void GetUser_ReturnSuccess()
    {
        var claimTypes =  _httpContextAccessor.HttpContext?.User.FindFirstValue("test");
        var expected = new List<User>(){new User(){UserName = claimTypes!}};
       var result = expected.FirstOrDefault(user => user.UserName == claimTypes);
       result.Should().NotBeNull();
       result.Should().BeOfType(typeof(User));
    }
    [Fact]
    public void GetUser_ReturnNull_UserdDoesNotExist()
    {
        var claimTypes =  _httpContextAccessor.HttpContext?.User.FindFirstValue("test");
        var expected = new List<User>(){new User(){UserName = "not test"}};
       var result = expected.FirstOrDefault(user => user.UserName == claimTypes);
       result.Should().BeNull();
    }
}