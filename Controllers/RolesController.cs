using TransportApi.Models;
using TransportApi.Dto;
using TransportApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using TransportApi.Interface;
using Microsoft.AspNetCore.Authorization;


namespace TransportApi.Controllers
{
    //[Authorize]
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

        //[AllowAnonymous]
        [HttpGet("list")]
        public async Task<IActionResult> GetList([FromQuery] int limit, [FromQuery] int page)
        {
            var roles = await _roleService.ListRole(limit, page);
            Console.WriteLine("Received request for role list", limit, roles);
            return Ok(new { role = roles });

        }

        [HttpPost("create")]
        public async Task<IActionResult> AddRole([FromBody] RoleDto role)
        {
            Console.WriteLine($"Role object: {role.name}");
            var roles = await _roleService.AddRole(role);
            return Ok(new { role = roles });
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateRole([FromBody] RoleDto role, [FromRoute] string id)
        {
            var roles = await _roleService.UpdateRole(role, id);
            Console.WriteLine("id" + JsonSerializer.Serialize(id) + JsonSerializer.Serialize(roles));
            if (roles != null) {
                return Ok(new { role = roles });
            }
            return BadRequest(new { message = "Update failed due to invalid data or business logic error." });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteRole([FromRoute] string id)
        {
            var roles = await _roleService.DeleteRole(id);
            Console.WriteLine("id" + JsonSerializer.Serialize(id) + JsonSerializer.Serialize(roles));
            if (roles != null)
            {
                return Ok(new { role = roles });
            }
            return BadRequest(new { message = "Update failed due to invalid data or business logic error." });
        }
    }
}