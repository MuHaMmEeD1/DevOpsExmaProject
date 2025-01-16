using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:53353")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.ListenAnyIP(8080); 
//    options.ListenAnyIP(443, listenOptions => 
//    {
//        listenOptions.UseHttps("app/certificates/certificate.pfx", "yourpassword");
//    });
//});

// Ocelot Configuration
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot();

var app = builder.Build();

// Use CORS before Ocelot middleware
app.UseCors("AllowSpecificOrigin");

await app.UseOcelot();

// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
