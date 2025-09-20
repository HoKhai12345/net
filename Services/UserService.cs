using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

public class UserService : IUserService
{
    private readonly IMongoCollection<User> _users;
    private readonly IPasswordHasher<User> password;
    private readonly IConfiguration _configuration;
    public UserService(IConfiguration configuration, IPasswordHasher<User> password)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
    }

    public async Task<User> RegisterAsync(User user)
    {
        var existingUser = await _context.Users.Find(u => u.Username == user.Username || u.Email == user.Email).FirstOrDefaultAsync();
        if (existingUser != null)
        {
            throw new Exception("Tên đăng nhập hoặc email đã tồn tại.");
        }
        user.PasswordHash = _passwordHasher.HashPassword(user, user.password);
        user.Roles.Add("Member");
        await _context.Users.InsertOneAsync(user);
        return user;
    }

    public async Task<User> AuthenticateAsync(string username, string password)
    {
        var user = await _context.Users.Find(u => u.Username == username).FirstOrDefaultAsync();
        if (user == null)
        {
            return null;
        }
        var passwordVerificationRessult = _passwordHasher.VerifyHashedPassword(user, user.password, password);
        if (passwordVerificationResult == passwordVerificationRessult.Failed)
        {
            return null;
        }
        return user;
    }

    public string GenerateJwtToken(User user)
    {
        // 1. Lấy JWT Secret Key từ cấu hình
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = Encoding.ASCII.GetBytes(jwtSettings.GetValue<string>("Secret"));

        // 2. Tạo Claims (danh tính người dùng)
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
        };

        // Thêm tất cả các vai trò của người dùng vào Claims
        foreach (var role in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // 3. Tạo mô tả token
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(jwtSettings.GetValue<double>("ExpiresInMinutes")),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        // 4. Tạo và trả về token
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

}