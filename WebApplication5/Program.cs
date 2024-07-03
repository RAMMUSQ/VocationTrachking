using System.Text;
using Core.Interfaces;
using Core.Services;
using Infrastructure.Repositories;
using WebApplication5.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplication5.Infrastructure.Repositories;
using WebApplication5.Interfaces;
using WebApplication5.Middlewares;
using WebApplication5.Data;
using WebApplication5.Infrastructure.Services;

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
builder.Services.AddScoped<ILeaveRequestService ,LeaveRequestService >();
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
builder.Services.AddSwaggerGen();
//builder.Services.AddAutoMapper(typeof(Startup));

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
