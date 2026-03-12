using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Portfolio.Asp.Data;
using Portfolio.Asp.Repositories;
using Portfolio.Asp.Services;
using Portfolio.Asp.Services.UserSer;

var builder = WebApplication.CreateBuilder(args);

// 1. Controllers და Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. S3 სერვისის რეგისტრაცია (Dependency Injection)
builder.Services.AddScoped<S3Service>();

// 3. Repository & UserService
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserService, UserService>();

// 4. CORS კონფიგურაცია
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 5. PostgreSQL მონაცემთა ბაზა
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// 6. FormOptions: 1 GB limit
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 1_073_741_824; // 1 GB
});

// 7. Kestrel: 1 GB max request size
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 1_073_741_824; // 1 GB
});

var app = builder.Build();

// 8. Swagger UI
app.UseSwagger();
app.UseSwaggerUI(options => {
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

// 9. CORS
app.UseCors();

// 10. Static files
app.UseStaticFiles();

// 11. Authorization
app.UseAuthorization();

// 12. Map Controllers
app.MapControllers();

// 13. დინამიური პორტი
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");