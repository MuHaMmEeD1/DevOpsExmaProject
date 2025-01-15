using DevOpsExmaProject.Identity.DataAccess;
using DevOpsExmaProject.Identity.Entitys;
using DevOpsExmaProject.Identity.Repositorise.Abstracts;
using DevOpsExmaProject.Identity.Repositorise.Concretes.EFEntityFramework;
using DevOpsExmaProject.Identity.Services.Abstracts;
using DevOpsExmaProject.Identity.Services.Concretes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// DbContext Configuration
builder.Services.AddDbContext<Mp3DbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DockerConnection")));

// Identity Configuration
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 1;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredUniqueChars = 0;
})
.AddEntityFrameworkStores<Mp3DbContext>()
.AddDefaultTokenProviders();

// JWT Authentication
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
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins(builder.Configuration["ClientUrl"]!) // ClientUrl appsettings.json veya ortam de?i?keninden ?ekiliyor
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Data Access Layer (Dal)
builder.Services.AddScoped<IUserDal, EFUserDal>();

// Service Layer
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISMTPService, SMTPService>();

var app = builder.Build();

// Middleware
app.UseCors("AllowSpecificOrigin");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
