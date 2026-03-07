using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Portfolio.Asp.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// მონაცემთა ბაზა
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();

// Swagger-ის ჩართვა Production-ზეც (რომ ონლაინ დაინახოთ)
app.UseSwagger();
app.UseSwaggerUI(options => {
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty; // Swagger გაიხსნება პირდაპირ მთავარ გვერდზე
});

app.UseCors();
app.UseStaticFiles();

// ფაილების ატვირთვის საქაღალდეების მართვა
var uploadRoot = Environment.GetEnvironmentVariable("UPLOAD_ROOT");
if (!string.IsNullOrWhiteSpace(uploadRoot))
{
    Directory.CreateDirectory(uploadRoot);
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(uploadRoot),
        RequestPath = "/uploads"
    });
}
else
{
    var localUploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
    Directory.CreateDirectory(localUploads);
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(localUploads),
        RequestPath = "/uploads"
    });
}

app.UseAuthorization();
app.MapControllers();

// Health Check / Default Route - რომ 404 არ ამოაგდოს
app.MapGet("/health", () => "API is running smoothly!");

// პორტის აღება გარემო ცვლადიდან (Railway/Render სტანდარტი)
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");