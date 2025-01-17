using DevOpsExmaProject.Mp3WebApi.DataAccess;
using DevOpsExmaProject.Mp3WebApi.Entitys;
using DevOpsExmaProject.Mp3WebApi.Repositorise.Abstracts;
using DevOpsExmaProject.Mp3WebApi.Repositorise.Concretes.EFEntityFramework;
using DevOpsExmaProject.Mp3WebApi.Services.Abstracts;
using DevOpsExmaProject.Mp3WebApi.Services.Concretes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext Configuration - PostgreSQL i?in yap?land?rma
builder.Services.AddDbContext<Mp3DbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

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
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
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

// Data Access Layer (DAL)
builder.Services.AddScoped<IUserDal, EFUserDal>();
builder.Services.AddScoped<IMp3Dal, EFMp3Dal>();

// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMp3Service, Mp3Service>();
builder.Services.AddScoped<IColudinaryService, CloudinaryService>();
builder.Services.AddScoped<IRedisService, RedisService>();
builder.Services.AddScoped<IRabbitMQService, RabbitMQService>();
builder.Services.AddScoped<IFileService, FileService>();

// Redis Configuration
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse("redis:6379", true);
    configuration.AbortOnConnectFail = true;
    return ConnectionMultiplexer.Connect(configuration);
});

var app = builder.Build();

// Swagger Configuration (Enabled in Development)
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

// Middleware Configuration
//app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();



app.MapControllers();

app.Run();
