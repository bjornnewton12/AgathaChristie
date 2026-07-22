using System.Text;
using System.Text.RegularExpressions;
using AgathaChristie.Application.DTOs;
using AgathaChristie.Application.Interfaces;
using AgathaChristie.Application.Services;
using AgathaChristie.Application.UseCases.Auth.CheckUsername;
using AgathaChristie.Application.UseCases.Auth.LoginUser;
using AgathaChristie.Application.UseCases.Auth.RegisterUser;
using AgathaChristie.Application.UseCases.Users.GetCurrentUser;
using AgathaChristie.Application.UseCases.UserBooks.GetUserBooks;
using AgathaChristie.Application.UseCases.UserBooks.UpdateUserBook;
using AgathaChristie.Infrastructure.Data;
using AgathaChristie.Infrastructure.Repositories;
using AgathaChristie.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<IDetectiveRepository, DetectiveRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<DetectiveService>();
builder.Services.AddScoped<GenreService>();
builder.Services.AddScoped<IMovieAdaptationRepository, MovieAdaptationRepository>();
builder.Services.AddScoped<MovieAdaptationService>();
builder.Services.AddScoped<ITVAdaptationRepository, TVAdaptationRepository>();
builder.Services.AddScoped<TVAdaptationService>();

builder.Services.AddHttpClient<ITmdbClient, TmdbClient>(client =>
{
    client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserBookRepository, UserBookRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<RegisterUserHandler>();
builder.Services.AddScoped<LoginUserHandler>();
builder.Services.AddScoped<CheckUsernameHandler>();
builder.Services.AddScoped<GetCurrentUserHandler>();
builder.Services.AddScoped<GetUserBooksHandler>();
builder.Services.AddScoped<UpdateUserBookHandler>();

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

var allowedOrigins = builder.Configuration["AllowedOrigins"]?.Split(',') ?? ["http://localhost:5173"];
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy => policy.WithOrigins(allowedOrigins).AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
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

app.MapControllers();

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

app.MapPost("/books/{bookId:guid}/movieadaptations", async (Guid bookId, MovieAdaptationRequest request, MovieAdaptationService service) =>
{
    if (Regex.IsMatch(request.TmdbUrl, @"tv/\d+"))
        return Results.BadRequest("That's a TV show link. Add it under TV adaptations instead.");

    try
    {
        var created = await service.CreateAsync(bookId, request);
        return created is null
            ? Results.BadRequest("Could not find that movie on TMDB. Check the link")
            : Results.Created($"/books/{bookId}/movieadaptations/{created.Id}", created);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(ex.Message);
    }
}).RequireAuthorization();

app.MapPost("/books/{bookId:guid}/tvadaptations", async (Guid bookId, TVAdaptationRequest request, TVAdaptationService service) =>
{
    if (Regex.IsMatch(request.TmdbUrl, @"movie/\d+"))
        return Results.BadRequest("That's a movie link. Add it under Movie adaptations instead.");

    try
    {
        var created = await service.CreateAsync(bookId, request);
        return created is null
            ? Results.BadRequest("Could not find that show or episode on TMDB.")
            : Results.Created($"/books/{bookId}/tvadaptations/{created.Id}", created);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(ex.Message);
    }
}).RequireAuthorization();

app.MapGet("/detectives", async (DetectiveService service) =>
    Results.Ok(await service.GetAllAsync()));

app.MapGet("/detectives/{id:guid}", async (Guid id, DetectiveService service) =>
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
