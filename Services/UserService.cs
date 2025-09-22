using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TransportApi.Models;
using System;

namespace TransportApi.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoDbService _mongoDbService;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;

        public UserService(IMongoDbService mongoDbService, IPasswordHasher<User> passwordHasher, IConfiguration configuration)
        {
            _mongoDbService = mongoDbService;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        public async Task<User> RegisterAsync(UserRegistrationDto userDto)
        {
            var existingUser = await _mongoDbService.Users.Find(u => u.Username == userDto.Username || u.Email == userDto.Email).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                return null;
                //throw new Exception("Tên đăng nhập hoặc email đã tồn tại.");
            }

            var user = new User
            {
                Name = userDto.Name,
                Username = userDto.Username,
                Email = userDto.Email,
                Password = userDto.Password
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, user.Password);
            // Assign default role from DB (Member)
            var memberRole = await _mongoDbService.Roles.Find(r => r.Name == "Member").FirstOrDefaultAsync();
            Console.WriteLine("Member role: " + (memberRole != null ? memberRole.Name : "null"));
            if (memberRole != null)
            {
                user.RoleIds.Add(memberRole.Id);
            }
            await _mongoDbService.Users.InsertOneAsync(user);
            return user;
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = await _mongoDbService.Users.Find(u => u.Username == username).FirstOrDefaultAsync();
            if (user == null)
            {
                return null;
            }
            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (passwordVerificationResult == PasswordVerificationResult.Failed)
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

            // Add roles and permissions as claims
            if (user.RoleIds != null && user.RoleIds.Count > 0)
            {
                var roles = _mongoDbService.Roles.Find(r => user.RoleIds.Contains(r.Id)).ToList();
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Name));
                    if (role.PermissionIds != null && role.PermissionIds.Count > 0)
                    {
                        var perms = _mongoDbService.Permissions.Find(p => role.PermissionIds.Contains(p.Id)).ToList();
                        foreach (var perm in perms)
                        {
                            claims.Add(new Claim("permission", perm.Name));
                        }
                    }
                }
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
}