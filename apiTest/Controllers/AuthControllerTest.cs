using System.Collections;
using api.Controller;
using api.DTO.UserDTO;
using api.Enums;
using api.Helpers;
using api.Interfaces;
using api.Repository;
using apiTest.DataGenerator;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace apiTest.Controllers;

public class AuthControllerTest
{
    private readonly IUserRepository _userRepository;

    public AuthControllerTest()
    {
        _userRepository = A.Fake<IUserRepository>();
    }

    [Fact]
    public async Task Register_ReturnSuccess()
    {
        var controller = new AuthController(_userRepository);
        var input = new RegisterUserDTO(){UserName = "User Name", EmailAddress = "username@email.com", Password = "password"};
        var result = await controller.Register(input);

        var actionResult = result.Result;
        
        actionResult.Should().NotBeNull();
        actionResult.Should().BeOfType(typeof(OkObjectResult));
    }
    [Fact]
    public async Task Register_ReturnFail()
    {
        var controller = new AuthController(_userRepository);
        var input = new RegisterUserDTO(){UserName = "User Name", EmailAddress = "username@email.com", Password = "password"};
        A.CallTo(() => _userRepository.RegisterUser(input)).Returns(Task.FromResult(new ResponseRepository<string>{Success = false, Message = "User already exists.", Data=""}));
        var result = await controller.Register(input);
        

        var actionResult = result.Result;
        actionResult.Should().BeOfType(typeof(BadRequestObjectResult));
    }
    [Fact]
    public async Task Login_ReturnSuccess()
    {
        var controller = new AuthController(_userRepository);
        var input = new LoginUserDTO(){UserName = "User Name", Password = "password"};
        // A.CallTo(() => _userRepository.Login(input)).Returns(Task.FromResult(new ResponseRepository<AuthenticatedUserDTO>{Success = false, Message = "User already exists.", Data=new AuthenticatedUserDTO()}));
        var result = await controller.Login(input);

        var actionResult = result.Result;
        
        actionResult.Should().NotBeNull();
        actionResult.Should().BeOfType(typeof(OkObjectResult));
    }
    [Fact]
    public async Task Login_ReturnNotAuthorized()
    {
        var controller = new AuthController(_userRepository);
        var input = new LoginUserDTO(){UserName = "User Name", Password = "password"};
        A.CallTo(() => _userRepository.Login(input)).Returns(Task.FromResult(new ResponseRepository<AuthenticatedUserDTO>{Success = false, Message = "User already exists.", Data=new AuthenticatedUserDTO()}));
        var result = await controller.Login(input);
       
        var actionResult = result.Result;
        
        actionResult.Should().NotBeNull();
        actionResult.Should().BeOfType(typeof(UnauthorizedObjectResult));
    }
    [Fact]
    public async Task RefreshToken_ReturnSuccess(){
        var controller = new AuthController(_userRepository);
        var accessToken = "accessToken";
        // var refreshToken = controller.Request.Cookies["refreshToken"];
        var result = await controller.RefreshToken(accessToken);

        var actionResult = result.Result;

        actionResult.Should().NotBeNull();
        actionResult.Should().BeOfType(typeof(OkObjectResult));
    }
    [Fact]
    public async Task RefreshToken_ReturnBadRequest(){
        var controller = new AuthController(_userRepository);
        var accessToken = "accessToken";
        A.CallTo(() => _userRepository.RefreshToken(accessToken)).Returns(new ResponseRepository<AuthenticatedUserDTO>{Success = false, Message = "", Data=new AuthenticatedUserDTO()});
        var result = await controller.RefreshToken(accessToken);

        var actionResult = result.Result;

        actionResult.Should().NotBeNull();
        actionResult.Should().BeOfType(typeof(UnauthorizedObjectResult));
    }
    [Theory]
    [InlineData("f90d20eb-0e61-4018-bdbb-8484ed4c84b7",UserRoles.Admin, true)]
    [InlineData("a22e3328-48ca-4949-9bcd-29e1622e1648",UserRoles.User, true)]
    [InlineData("ebe7e4b9-e182-4724-b898-68bc936cd087",UserRoles.Admin, true)]
    public async Task Update_ReturnSuccess(Guid id,UserRoles role, bool isActive){
        var controller = new AuthController(_userRepository);
        var result = await controller.UpdateUser(id, new UpdateUserDTO{Role = role, IsActive = isActive});
        var actionResult = result.Result;

        actionResult.Should().NotBeNull();
        actionResult.Should().BeOfType(typeof(OkObjectResult));
    }
    [Theory]
    [InlineData("f90d20eb-0e61-4018-bdbb-8484ed4c84b7")]
    [InlineData("a22e3328-48ca-4949-9bcd-29e1622e1648")]
    [InlineData("ebe7e4b9-e182-4724-b898-68bc936cd087")]
    public async Task ActivateEmail_ReturnSuccess(Guid id){
        var controller = new AuthController(_userRepository);
        var result = await controller.ActivateEmail(id);
        
        var actionResult =  result.Result;

        actionResult.Should().NotBeNull();
        actionResult.Should().BeOfType(typeof(OkObjectResult));
    }
    [Theory]
    [InlineData("f90d20eb-0e61-4018-bdbb-8484ed4c84b7")]
    [InlineData("a22e3328-48ca-4949-9bcd-29e1622e1648")]
    [InlineData("ebe7e4b9-e182-4724-b898-68bc936cd087")]
    public async Task ActivateEmail_ReturnBadRequest(Guid id){
        var controller = new AuthController(_userRepository);
        A.CallTo(() => _userRepository.ActivateUserEmail(id)).Returns(new ResponseRepository<string>{Success = false, Message="User not found"});
        var result = await controller.ActivateEmail(id);
        
        var actionResult =  result.Result;

        actionResult.Should().NotBeNull();
        actionResult.Should().BeOfType(typeof(BadRequestObjectResult));
    }
    [Theory]
    [InlineData("password1","f90d20eb-0e61-4018-bdbb-8484ed4c84b7")]
    [InlineData("password2","a22e3328-48ca-4949-9bcd-29e1622e1648")]
    [InlineData("password3","ebe7e4b9-e182-4724-b898-68bc936cd087")]
    public async Task UpdatePassword_ReturnSuccess(string password,Guid recoveryCode){
        var controller = new AuthController(_userRepository);
        // A.CallTo(() => _userRepository.ActivateUserEmail(recoveryCode)).Returns(new ResponseRepository<string>{Success = false, Message="User not found"});
        var result = await controller.UpdatePassword(new ChangeUserPasswordDTO{Password = password, RecoveryCode = recoveryCode});
        
        var actionResult =  result.Result;

        actionResult.Should().NotBeNull();
        actionResult.Should().BeOfType(typeof(OkObjectResult));
    }
    [Theory]
    [ClassData(typeof(TestDataGenerator))]
    public async Task GetUsers_ReturnSuccess(QueryHelper name){
        var controller = new AuthController(_userRepository);
        var result = await controller.GetUsers(name);
        
        var actionResult =  result.Result;

        actionResult.Should().NotBeNull();
        actionResult.Should().BeOfType(typeof(OkObjectResult));
    }
    [Fact]
    public async Task Logout_ReturnSuccess(){
        var controller = new AuthController(_userRepository);
        var result =  await controller.Logout();

        var valueResult = result.Value;

        valueResult?.Success.Should().Be(true);
   } 
    [Fact]
    public async Task Logout_ReturnBadRequest(){
        var controller = new AuthController(_userRepository);
        A.CallTo(() => _userRepository.Logout()).Returns(new ResponseRepository<string>{Success = false, Message = "You are not logged in."});
        var result =  await controller.Logout();

        var actionResult = result.Result;

        actionResult.Should().BeOfType(typeof(BadRequestObjectResult));
   } 
   [Theory]
    [InlineData("f90d20eb-0e61-4018-bdbb-8484ed4c84b7")]
    [InlineData("a22e3328-48ca-4949-9bcd-29e1622e1648")]
    [InlineData("ebe7e4b9-e182-4724-b898-68bc936cd087")]
    public async Task GetUser_ReturnSuccess(Guid id){
        var controller = new AuthController(_userRepository);
        // A.CallTo(() => _userRepository.ActivateUserEmail(id)).Returns(new ResponseRepository<string>{Success = false, Message="User not found"});
        var result = await controller.GetUser(id);
        
        var actionResult =  result.Result;

        actionResult.Should().NotBeNull();
        actionResult.Should().BeOfType(typeof(OkObjectResult));
    }
   [Theory]
    [InlineData("f90d20eb-0e61-4018-bdbb-8484ed4c84b7")]
    [InlineData("a22e3328-48ca-4949-9bcd-29e1622e1648")]
    [InlineData("ebe7e4b9-e182-4724-b898-68bc936cd087")]
    public async Task GetUser_ReturnBadRequest(Guid id){
        var controller = new AuthController(_userRepository);
        A.CallTo(() => _userRepository.GetUser(id)).Returns(new ResponseRepository<GetUserDTO>{Success = false, Message="User not found"});
        var result = await controller.GetUser(id);
        
        var actionResult =  result.Result;

       
        actionResult.Should().BeOfType(typeof(BadRequestObjectResult));
    }
}

