using TransportApi.Models;
using TransportApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace TransportApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IConfiguration _configuration;

        public RolesController(IRoleService roleService, IConfiguration configuration)
        {
            _roleService = roleService;
            _configuration = configuration;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetList([FromQuery] int limit)
        {
            var roles = await _roleService.ListRole(limit);
            Console.WriteLine("Received request for role list", limit, roles);
            return Ok(new { role = roles });

        }

        [HttpPost("create")]
        public async Task<IActionResult> AddRole([FromBody] RoleDto role)
        {
            var roles = await _roleService.AddRole(role);
            return Ok(new { role = roles });
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateRole([FromBody] RoleDto role, [FromRoute] string id)
        {
            Console.WriteLine("id", id, role);
            var roles = await _roleService.UpdateRole(role, id);
            return Ok(new { role = roles });
        }
    }
}