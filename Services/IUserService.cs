using TransportApi.Models;

namespace TransportApi.Services
{
    public interface IUserService
    {
        Task<User> RegisterAsync(UserRegistrationDto userDto);
        Task<User> AuthenticateAsync(string username, string password);
        string GenerateJwtToken(User user);
    }
}
