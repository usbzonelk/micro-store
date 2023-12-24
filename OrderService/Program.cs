using Microsoft.EntityFrameworkCore;
using OrderService;
using OrderService.Models;
using OrderService.Data;
using OrderService.Service.IService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<OrderServiceDBContext>(dbOptions => dbOptions.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserService, UsersService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddHttpClient("Product", u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPI"]));
builder.Services.AddHttpClient("User", u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:UserAPI"]));
builder.Services.AddHttpClient("Cart", u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:CartAPI"]));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
