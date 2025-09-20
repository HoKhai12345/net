using TransportApi.Models; // Hoặc namespace chứa các lớp User, UserRegistrationDto, UserLoginDto
using TransportApi.Services; // Hoặc namespace chứa IUserService

// ... Các using khác
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
	private readonly IUserService _userService;
	private readonly IConfiguration _configuration;

	public AuthController(IUserService userService, IConfiguration configuration)
	{
		_userService = userService;
		_configuration = configuration;
	}

	[HttpPost("register")]
	public async Task<IActionResult> Register([FromBody] UserRegistrationDto userDto)
	{
		// Kiểm tra xem dữ liệu đầu vào có hợp lệ không
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		// Gọi đến Service để xử lý logic đăng ký
		var user = await _userService.RegisterAsync(userDto);

		// Trả về kết quả thành công
		return Ok(new { Message = "Đăng ký thành công." });
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] UserLoginDto userDto)
	{
		// Gọi đến Service để xác thực người dùng
		var user = await _userService.AuthenticateAsync(userDto.Username, userDto.Password);

		if (user == null)
		{
			return Unauthorized(new { Message = "Tên đăng nhập hoặc mật khẩu không đúng." });
		}

		// Sau khi xác thực, tạo JWT Token
		var token = _userService.GenerateJwtToken(user);

		// Trả về token và thông tin người dùng
		return Ok(new { Token = token, User = user });
	}
}