using System.Text.Json.Serialization;

namespace api.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserRoles
{
    User = 1,
    Admin = 2,
    Manager = 3
}