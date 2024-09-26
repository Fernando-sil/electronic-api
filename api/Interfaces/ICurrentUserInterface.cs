using api.Entities;

namespace api.Interfaces;

public interface ICurrentUserInterface
{
    Task<User?> GetUser();
    Task<Guid?> GetUserId();
}