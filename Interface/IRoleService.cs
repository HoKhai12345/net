using TransportApi.Models;
using TransportApi.Dto;

namespace TransportApi.Interface
{
    public interface IRoleService
    {
        Task<List<Role>> ListRole(int limit);
        Task<Role> AddRole(RoleDto roleDto);
        Task<Role> UpdateRole(RoleDto roleDto, string id);
    }
}
