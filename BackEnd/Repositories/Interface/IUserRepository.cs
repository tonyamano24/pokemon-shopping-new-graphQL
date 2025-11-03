using itsc_dotnet_practice.Models;
using itsc_dotnet_practice.Models.Dtos;
using System.Threading.Tasks;

namespace itsc_dotnet_practice.Repositories.Interface;

public interface IUserRepository
{
    Task<User?> GetUser(string username, string password);
    Task<User?> GetUserByUsername(string username);
    Task<User> CreateUser(User user);
}
