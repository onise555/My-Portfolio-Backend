using Microsoft.EntityFrameworkCore;
using Portfolio.Asp.Data;
using Portfolio.Asp.Services; // დაამატე ეს S3Service-ისთვის

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1. S3 სერვისის რეგისტრაცია (Dependency Injection)
builder.Services.AddScoped<S3Service>();

// CORS კონფიგურაცია
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// მონაცემთა ბაზა (PostgreSQL)
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();

// Swagger (Production-ზეც ჩართულია)
app.UseSwagger();
app.UseSwaggerUI(options => {
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseCors();

// მხოლოდ სტანდარტული static files (CSS, JS), /uploads აღარ გვინდა
app.UseStaticFiles();

app.UseAuthorization();
app.MapControllers();

// პორტის დინამიური მართვა
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");