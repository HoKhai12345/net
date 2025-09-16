using TransportApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Phần 1: Đăng ký tất cả các dịch vụ (Services Configuration)
// Tất cả dòng code có "builder.Services." đều phải ở đây.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Đăng ký MongoDbSettings từ appsettings.json
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Đăng ký IMongoDbService và MongoDbService vào DI Container
builder.Services.AddSingleton<IMongoDbService, MongoDbService>();

// Đăng ký VehicleService để có thể inject vào Controller (nếu có)
builder.Services.AddSingleton<VehicleService>();

// Xây dựng ứng dụng
var app = builder.Build();

// Phần 2: Cấu hình HTTP request pipeline (Middleware)
// Tất cả dòng code có "app.Use..." đều phải ở đây.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Phần 3: Chạy ứng dụng
app.Run();