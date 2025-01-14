using DevOpsExmaProject.Mp3Api.DataAccess;
using DevOpsExmaProject.Mp3Api.Entitys;
using DevOpsExmaProject.Mp3Api.Repositorise.Abstracts;
using DevOpsExmaProject.Mp3Api.Repositorise.Concretes.EFEntityFramework;
using DevOpsExmaProject.Mp3Api.Services.Abstracts;
using DevOpsExmaProject.Mp3Api.Services.Concretes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Db Start

builder.Services.AddDbContext<Mp3DbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    
});

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


builder.Services.AddAuthentication(opt =>
{

    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(opt => {

        opt.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))



        };
    
    
    });


// Authentication End


// Data Access Layer (Dal) Start

builder.Services.AddScoped<IUserDal, EFUserDal>();
builder.Services.AddScoped<IMp3Dal, EFMp3Dal>();

// Data Access Layer (Dal) End 


// Service Start

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMp3Service, Mp3Service>();
builder.Services.AddScoped<IColudinaryService, CloudinaryService>();
builder.Services.AddScoped<IRedisService, RedisService>();
builder.Services.AddScoped<IRabbitMQService, RabbitMQService>();

// Service End

// Singleton Start

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    string host = configuration["Redis:Host"]!;
    int port = int.Parse(configuration["Redis:Port"]!);
    string user = configuration["Redis:User"]!;
    string password = configuration["Redis:Password"]!;

    return ConnectionMultiplexer.Connect(new ConfigurationOptions
    {
        EndPoints = { { host, port } },
        User = user,
        Password = password,
        AbortOnConnectFail = false,
        ConnectRetry = 3,
        ConnectTimeout = 5000
    });
});

// Singleton End



var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
