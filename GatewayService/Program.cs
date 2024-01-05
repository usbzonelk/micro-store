using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using GatewayService;
using GatewayService.Services;
using GatewayService.Middlewares;
using GatewayService.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("4b46723a0157db12a8e00dc8cc839a63360ca1e14f83f0d40450aa1a3f876de5")),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true
    };
});



builder.Services.AddScoped<IUserService, UsersService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<ICartService, CartServiceC>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddAutoMapper(typeof(Mappings));

builder.Services.AddHttpClient("Product", u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPI"]));
builder.Services.AddHttpClient("User", u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:UserAPI"]));
builder.Services.AddHttpClient("Cart", u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:CartAPI"]));
builder.Services.AddHttpClient("Admin", u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:AdminAPI"]));
builder.Services.AddHttpClient("Order", u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:OrderAPI"]));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();