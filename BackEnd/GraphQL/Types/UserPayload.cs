using itsc_dotnet_practice.Models;

namespace itsc_dotnet_practice.GraphQL.Types;

public record UserPayload(int Id, string Username, string FullName, string Phone, string Role)
{
    public static UserPayload FromUser(User user)
    {
        return new UserPayload(
            user.Id,
            user.Username,
            user.FullName,
            user.Phone,
            user.Role
        );
    }
}
