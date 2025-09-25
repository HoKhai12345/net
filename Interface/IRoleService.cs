using TransportApi.Models;
using TransportApi.Dto;
using TransportApi.Common;

namespace TransportApi.Interface
{
    public interface IRoleService
    {
        Task<PagedResult<Role>> ListRole(int limit, int page);
        Task<Role> AddRole(RoleDto roleDto);
        Task<Role> UpdateRole(RoleDto roleDto, string id);
        Task<Role> DeleteRole(string id);
    }
}
