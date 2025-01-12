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

// Db Start
builder.Services.AddDbContext<Mp3DbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Db End

// Authentication Start
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
// Authentication End

// CORS Start
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:52669")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
// CORS End

// Data Access Layer (Dal) Start
builder.Services.AddScoped<IUserDal, EFUserDal>();
// Data Access Layer (Dal) End

// Service Start
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISMTPService, SMTPService>();
// Service End

var app = builder.Build();

// Use CORS
app.UseCors("AllowSpecificOrigin");

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
