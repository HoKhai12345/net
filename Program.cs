using TransportApi.Services;
using TransportApi.Models;
using TransportApi.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // Sửa port của bạn ở đây
      .AllowAnyHeader()
      .AllowAnyMethod();
        });
});

// Phần 1: Đăng ký tất cả các dịch vụ (Services Configuration)
// Tất cả dòng code có "builder.Services." đều phải ở đây.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Đăng ký PasswordHasher
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// Đăng ký IMongoDbService và MongoDbService vào DI Container
builder.Services.AddSingleton<IMongoDbService, MongoDbService>();

// Đăng ký IUserService
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IRoleService, RolesService>();

// Seed default role at startup
builder.Services.AddHostedService<RoleSeederHostedService>();

// Cấu hình JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings.GetValue<string>("Secret"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// Xây dựng ứng dụng
var app = builder.Build();

// Phần 2: Cấu hình HTTP request pipeline (Middleware)
// Tất cả dòng code có "app.Use..." đều phải ở đây.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// MongoDB: Globally ignore extra elements for legacy documents
if (!BsonClassMap.IsClassMapRegistered(typeof(User)))
{
    BsonClassMap.RegisterClassMap<User>(cm =>
    {
        cm.AutoMap();
        cm.SetIgnoreExtraElements(true);
    });
}
if (!BsonClassMap.IsClassMapRegistered(typeof(Role)))
{
    BsonClassMap.RegisterClassMap<Role>(cm =>
    {
        cm.AutoMap();
        cm.SetIgnoreExtraElements(true);
    });
}
if (!BsonClassMap.IsClassMapRegistered(typeof(Permission)))
{
    BsonClassMap.RegisterClassMap<Permission>(cm =>
    {
        cm.AutoMap();
        cm.SetIgnoreExtraElements(true);
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ApiResponseMiddleware>();
app.UseCors(); // Phải ở đây
app.MapControllers();

// Phần 3: Chạy ứng dụng
app.Run();