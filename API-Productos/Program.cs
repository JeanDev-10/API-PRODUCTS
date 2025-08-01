using API_Productos.Context;
using API_Productos.Interfaces;
using API_Productos.Repositories;
using API_Productos.Services;
using FluentValidation;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//conexión a bd
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Servicios personalizados
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

//agregar servicio de validacion con fluent validation
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
// rate limiter
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.Window = TimeSpan.FromSeconds(60);
        opt.PermitLimit = 50;
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable rate limiting
app.UseRateLimiter();
app.UseAuthorization();
//añadimos el rate limiter a los controladores
app.MapControllers().RequireRateLimiting("fixed");

app.Run();
