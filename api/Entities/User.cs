using api.Enums;

namespace api.Entities;

public class User
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public byte[] Password { get; set; } = [0];
    public byte[] Salt { get; set; } = [0];
    public string EmailAddress { get; set; } = string.Empty;
    public DateTime JoinedOn { get; set; } = DateTime.Now;
    public bool ConfirmedEmail { get; set; } = false;
    public UserRoles Role { get; set; } = UserRoles.User;
    public bool IsActive { get; set; } = true;
    public Guid? ActivationCode { get; set; }
    public DateTime? ActivationCodeExpires { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpires { get; set; }
    public List<Rating>? Ratings { get; set; }
    public List<Cart>? Carts { get; set; }
}