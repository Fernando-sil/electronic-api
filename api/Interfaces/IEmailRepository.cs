using api.DTO.EmailDTO;
using api.Repository;

namespace api.Interfaces;

public interface IEmailRepository
{
    Task<ResponseRepository<string>> VerifyEmail(SendEmailDTO sendEmailDTO);
    Task<ResponseRepository<string>> RecoverPasswordEmail(PasswordRecoveryRequestDTO passwordRecoveryRequestDTO);
}