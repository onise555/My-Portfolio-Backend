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

// 4. CORS კონფიგურაცია (გავხსენით ყველაფერი, რომ "Failed to fetch" გამოირიცხოს)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 5. PostgreSQL მონაცემთა ბაზა
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// 6. FormOptions & Kestrel (დიდი ფაილებისთვის)
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 1_073_741_824; // 1 GB
});

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 1_073_741_824; // 1 GB
});

var app = builder.Build();

// 7. Swagger UI - ყოველთვის ჩართული იყოს Railway-ზე სანახავად
app.UseSwagger();
app.UseSwaggerUI(options => {
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty; // Swagger იქნება მთავარ გვერდზე
});

// 8. Middleware-ების სწორი თანმიმდევრობა
app.UseRouting(); // აუცილებელია CORS-ისთვის

app.UseCors("AllowAll"); // ვიყენებთ ჩვენს შექმნილ "AllowAll" პოლიტიკას

app.UseStaticFiles();
app.UseAuthorization();

// 9. Map Controllers
app.MapControllers();

// 10. დინამიური პორტი Railway-სთვის
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");