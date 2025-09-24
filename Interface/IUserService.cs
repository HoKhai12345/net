using TransportApi.Models;
using TransportApi.Dto;

namespace TransportApi.Interface
{
    public interface IUserService
    {
        Task<User> RegisterAsync(UserRegistrationDto userDto);
        Task<User> AuthenticateAsync(string username, string password);
        string GenerateJwtToken(User user);
    }
}
