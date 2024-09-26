using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace apiTest.Helpers;

public class ActionResultResponse
{
     public void SuccessResponse(ActionResult actionResult){
        actionResult.Should().NotBe(null);
        actionResult.Should().BeOfType(typeof(OkObjectResult));
    }
    public void NotFoundResponse(ActionResult actionResult){
        actionResult.Should().BeOfType(typeof(NotFoundObjectResult));
    }
}