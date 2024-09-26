namespace api.Interfaces;

public interface IPasswordInterface
{
    void HashPassword(string password, out byte[] hashedPassword, out byte[] salt);
    bool IsPasswordValid(string password, byte[] hashedPassword, byte[] salt);
}