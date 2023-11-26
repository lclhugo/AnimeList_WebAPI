using System.Reflection.PortableExecutable;
using System.Text;
using AnimeListApi.Handlers;
using AnimeListApi.Models.Data;
using AnimeListApi.Services;
using AnimeListApi.Services.Anime;
using AnimeListApi.Services.Character;
using AnimeListApi.Services.Manga;
using AnimeListApi.Services.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
var config = builder.Configuration;
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddDbContext<AnimeListContext>(options => options.UseSqlServer(config.GetConnectionString("supabase")));
builder.Services.AddDbContext<AnimeListContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("supabase")));
builder.Services.AddScoped<HttpClient>();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<JikanHandler>();
builder.Services.AddScoped<AnimeService>();
builder.Services.AddScoped<AnimeListService>();
builder.Services.AddScoped<CharacterService>();
builder.Services.AddScoped<FavoriteCharactersService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<MangaService>();
builder.Services.AddScoped<MangaListService>();

var jwtSettings = config.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateLifetime = true,
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { };

