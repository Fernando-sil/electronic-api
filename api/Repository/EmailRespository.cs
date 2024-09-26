using api.DataContext;
using api.DTO.EmailDTO;
using api.Entities;
using api.Helpers;
using api.Interfaces;
using api.Templates;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using MimeKit;

namespace api.Repository;

public class EmailRespository : IEmailRepository
{
    private readonly IConfiguration _configuration;
    private readonly DatabaseContext _context;
    private readonly ICurrentUserInterface _currentUser;

    public EmailRespository(IConfiguration configuration, DatabaseContext context, ICurrentUserInterface currentUser)
    {
        _configuration = configuration;
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<ResponseRepository<string>> RecoverPasswordEmail(PasswordRecoveryRequestDTO passwordRecoveryRequestDTO)
    {
        var response = new ResponseRepository<string>();
        var failResponse = new FailResponse();
        var user = await _context.Users.FirstOrDefaultAsync(u => u.EmailAddress == passwordRecoveryRequestDTO.Email && u.UserName == passwordRecoveryRequestDTO.UserName);
        if(user is null){
            failResponse.GenerateFailResponse(response, "User not found");
            return response;
        }

        var userName = _configuration.GetSection("Email:EmailUserName").Value;
        var password = _configuration.GetSection("Email:EmailPassword").Value;
        var host = _configuration.GetSection("Email:EmailHost").Value;
        var email = SetEmailElements(userName!, passwordRecoveryRequestDTO.Email, "Recover your password");
        
        SetEmailBody(template => template.PasswordRecoveryEmail,user!,email,"[recovery-code]");
        await SetEmailConfigurations(host!, userName!, password!, email);

        response.Message = "Email sent";
        return response;
    }

    public async Task<ResponseRepository<string>> VerifyEmail(SendEmailDTO sendEmailDTO)
    {
        var response = new ResponseRepository<string>();
        var failResponse = new FailResponse();
        var user = await _currentUser.GetUser();
        if(user is null){
           failResponse.GenerateFailResponse(response, "User not found");
           return response;
        }
        
        var userName = _configuration.GetSection("Email:EmailUserName").Value;
        var password = _configuration.GetSection("Email:EmailPassword").Value;
        var host = _configuration.GetSection("Email:EmailHost").Value;

        var email = SetEmailElements(userName!, sendEmailDTO.EmailAddress, "Activate your account");

        SetEmailBody(template => template.ConfirmEmail,user!,email,"[activation-code]");
        await SetEmailConfigurations(host!, userName!, password!, email);
       
        response.Message = "Email sent";
        return response;
    }
    private MimeMessage SetEmailElements(string userName, string to, string subject){
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(userName));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        return email;
    }
    private async Task SetEmailConfigurations(string host, string userName, string password, MimeMessage email){
        using var smtp = new SmtpClient();
        smtp.Connect(host, 587, SecureSocketOptions.StartTls);
        smtp.Authenticate(userName, password);
        await smtp.SendAsync(email);
        smtp.Disconnect(true);
    }

    private void SetEmailBody(Func<EmailTemplates,string> predicate, User user, MimeMessage email, string stringToReplace){
        var emailTemplate = new EmailTemplates();
        var body = predicate(emailTemplate);
        body = body.Replace(stringToReplace,user!.ActivationCode.ToString());
        var bodyBuilder = new BodyBuilder();
        bodyBuilder.HtmlBody = body;
        email.Body = bodyBuilder.ToMessageBody();
        user.ActivationCode = new Guid();
    }
}