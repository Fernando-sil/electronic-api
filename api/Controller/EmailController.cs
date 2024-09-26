using api.DataContext;
using api.DTO.EmailDTO;
using api.Interfaces;
using api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controller;

[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly IEmailRepository _emailRepository;

    public EmailController(IEmailRepository emailRepository)
    {
        _emailRepository = emailRepository;
    }

    [Authorize]
    [HttpPost("activate-email")]
    public async Task<ActionResult<ResponseRepository<string>>> ActivateAccount(SendEmailDTO sendEmailDTO){
        var response = await _emailRepository.VerifyEmail(sendEmailDTO);
        if(!response.Success) return NotFound(response);
        return Ok(response);
    }
    [HttpPost("recover-password-email")]
    public async Task<ActionResult<ResponseRepository<string>>> RecoverPassword(PasswordRecoveryRequestDTO passwordRecoveryRequestDTO){
        var response = await _emailRepository.RecoverPasswordEmail(passwordRecoveryRequestDTO);
        if(!response.Success) return BadRequest(response);
        return Ok(response);
    }
}