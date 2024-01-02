using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using GatewayService;
using GatewayService.Services;
using GatewayService.Middlewares;
using GatewayService.Utils;
using System.Security.Claims;

IEnumerable<Claim> claims = new List<Claim>
{
    // User identity claims
    new Claim(ClaimTypes.Name, "john.doe"),
    new Claim(ClaimTypes.Email, "john.doe@example.com"),
    new Claim(ClaimTypes.NameIdentifier, "12345"),

    // Custom claims
    new Claim("customClaimType", "customClaimValue"),
    new Claim("role", "admin"),

    // Role claims
    new Claim(ClaimTypes.Role, "Administrator"),
    new Claim(ClaimTypes.Role, "User"),

    // Additional claims
    new Claim("customClaim2", "value2"),
    new Claim("customClaim3", "value3"),
};

var uu = JWTManager.GenerateJwt(claims, DateTime.Now.AddDays(456));
Console.WriteLine($"\n\n{uu}\n\n\n");
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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserService, UsersService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAdminService, AdminService>();
//builder.Services.AddAutoMapper(typeof(Mappings));

builder.Services.AddHttpClient("Product", u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPI"]));
builder.Services.AddHttpClient("User", u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:UserAPI"]));
builder.Services.AddHttpClient("Admin", u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:AdminAPI"]));

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
