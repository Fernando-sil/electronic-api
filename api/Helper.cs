using api.DTO.UserDTO;
using api.Entities;
using api.Repository;

namespace api;

public class Helper<T>
{
    public required ResponseRepository<T> Response { get; set; }
    public User? User { get; set; }
}