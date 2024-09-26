using api.Controller;
using api.DTO.EmailDTO;
using api.Interfaces;
using apiTest.Helpers;
using FakeItEasy;

namespace apiTest.Controllers;

public class EmailControllerTest
{
    private readonly IEmailRepository _emailRepository;

    public EmailControllerTest()
    {
        _emailRepository = A.Fake<IEmailRepository>();
    }

    [Theory]
    [InlineData("to1","email1")]
    [InlineData("to2","email2")]
    [InlineData("to3","email3")]
    public async Task ActivateAccount_ReturnSuccess(string to, string email)
    {
        var controller = new EmailController(_emailRepository);
        var response = new ActionResultResponse();
        var result = await controller.ActivateAccount(new SendEmailDTO{To = to, EmailAddress = email});

        var actionResult = result.Result;

        response.SuccessResponse(actionResult!);
    }
    [Theory]
    [InlineData("user1","email1")]
    [InlineData("user2","email2")]
    [InlineData("user3","email3")]
    public async Task RecoverPassword_ReturnSuccess(string userName, string email)
    {
        var controller = new EmailController(_emailRepository);
        var response = new ActionResultResponse();
        var result = await controller.RecoverPassword(new PasswordRecoveryRequestDTO{UserName = userName, Email = email});

        var actionResult = result.Result;

        response.SuccessResponse(actionResult!);
    }
}