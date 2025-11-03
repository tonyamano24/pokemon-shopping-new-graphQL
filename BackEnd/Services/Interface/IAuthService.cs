using itsc_dotnet_practice.Models;
using itsc_dotnet_practice.Models.Dtos;
using System.Threading.Tasks;

namespace itsc_dotnet_practice.Services.Interface;

public interface IAuthService
{
    Task<User?> Authenticate(LoginRequestDto login);
    User? ValidateBasicAuth(string authHeader);
    string GenerateJwtToken(User user);
    Task<User> Register(RegisterRequestDto register);

}
