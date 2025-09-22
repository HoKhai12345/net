using TransportApi.Models;

namespace TransportApi.Services
{
    public interface IRoleService
    {
        Task<List<Role>> ListRole(int limit);
        Task<Role> AddRole(string name);
    }
}
