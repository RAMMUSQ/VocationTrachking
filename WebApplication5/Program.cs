using System.Text;
using Application.Interfaces;
using Application.Services;
using Domain.Enums;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Infrastructure.Data;
using Infrastructure.Services;
using Presentation.Middlewares;
using WebApplication5;


var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false);

// JWT Configuration
var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value;
var jwtKey = builder.Configuration.GetSection("Jwt:Key").Value;

// Services
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var conStr = builder.Configuration.GetConnectionString("MySqlConStr");
    options.UseMySql(conStr, ServerVersion.AutoDetect(conStr));
});
builder.Services.AddScoped<ILeaveRequestService, LeaveRequestService>();
builder.Services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApplication5", Version = "v1" });

    // Enum türlerini schema olarak ekleyin
    c.MapType<Leavetype>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = new List<IOpenApiAny>
        {
            new OpenApiString(Leavetype.Paternity.ToString()),
            new OpenApiString(Leavetype.Maternity.ToString()),
            new OpenApiString(Leavetype.Sick.ToString()),
            new OpenApiString(Leavetype.Bereavement.ToString()),
            new OpenApiString(Leavetype.Annual.ToString()),
            new OpenApiString(Leavetype.Birthday.ToString())
        }
    });

    // Swagger için diğer gerekli ayarlar
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication(); // Kimlik doğrulama middleware'ini yetkilendirmeden önce kullanın
app.UseMiddleware<AuthMiddleware>(); // AuthMiddleware burada olmalı
app.UseAuthorization();
app.MapControllers();

app.Run();