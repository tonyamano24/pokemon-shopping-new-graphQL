using System.Threading.Tasks;
using HotChocolate;
using itsc_dotnet_practice.GraphQL.Types;
using itsc_dotnet_practice.Models.Dtos;
using itsc_dotnet_practice.Services.Interface;
using HotChocolate.Types;

namespace itsc_dotnet_practice.GraphQL.Mutations;

[ExtendObjectType(Name = "Mutation")]
public class AuthMutations
{
    private static GraphQLException BuildError(string message) =>
        new GraphQLException(ErrorBuilder.New().SetMessage(message).Build());

    public async Task<AuthPayload> LoginAsync(
        LoginRequestDto input,
        [Service] IAuthService authService)
    {
        var user = await authService.Authenticate(input);
        if (user == null)
        {
            throw BuildError("Invalid username or password.");
        }

        var token = authService.GenerateJwtToken(user);
        return new AuthPayload(token, UserPayload.FromUser(user));
    }

    public async Task<UserPayload> RegisterAsync(
        RegisterRequestDto input,
        [Service] IAuthService authService)
    {
        var user = await authService.Register(input);
        return UserPayload.FromUser(user);
    }
}
