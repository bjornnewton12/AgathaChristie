using System.Text;
using AgathaChristie.Application.DTOs;
using AgathaChristie.Application.Interfaces;
using AgathaChristie.Application.Services;
using AgathaChristie.Infrastructure.Data;
using AgathaChristie.Infrastructure.Repositories;
using AuthService = AgathaChristie.Infrastructure.Services.AuthService;
using JwtTokenService = AgathaChristie.Infrastructure.Services.JwtTokenService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default") ?? "Data Source=agatha.db"));

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<IDetectiveRepository, DetectiveRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<DetectiveService>();
builder.Services.AddScoped<GenreService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JwtTokenService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
      options.AddDefaultPolicy(policy => policy.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    await CatalogueSeeder.SeedAsync(db);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseCors();

app.MapPost("/auth/register", async (RegisterRequest req, AuthService auth, JwtTokenService jwt) =>
{
    var user = await auth.RegisterAsync(req.Username, req.Password);
    return user is null ? Results.Conflict("Username already taken") : Results.Ok(new { token = jwt.GenerateToken(user) });
});

app.MapPost("/auth/login", async (LoginRequest req, AuthService auth, JwtTokenService jwt) =>
{
    var user = await auth.LoginAsync(req.Username, req.Password);
    return user is null ? Results.Unauthorized() : Results.Ok(new { token = jwt.GenerateToken(user) });
});

app.MapGet("/books", async (BookService service) =>
    Results.Ok(await service.GetAllAsync()));

app.MapGet("/books/{id:guid}", async (Guid id, BookService service) =>
{
    var book = await service.GetByIdAsync(id);
    return book is null ? Results.NotFound() : Results.Ok(book);
});

app.MapPut("/books/{id:guid}", async (Guid id, BookRequest request, BookService service) =>
{
    var updated = await service.UpdateAsync(id, request);
    return updated is null ? Results.NotFound() : Results.Ok(updated);
});

app.MapGet("/detectives", async (DetectiveService service) =>
    Results.Ok(await service.GetAllAsync()));

app.MapGet("/detectives/{id:guid}", async (Guid id, DetectiveService
service) =>
{
    var detective = await service.GetByIdAsync(id);
    return detective is null ? Results.NotFound() : Results.Ok(detective);
});

app.MapGet("/genres", async (GenreService service) =>
    Results.Ok(await service.GetAllAsync()));

app.MapGet("/genres/{id:guid}", async (Guid id, GenreService service) =>
{
    var genre = await service.GetByIdAsync(id);
    return genre is null ? Results.NotFound() : Results.Ok(genre);
});

app.Run();

record RegisterRequest(string Username, string Password);
record LoginRequest(string Username, string Password);
