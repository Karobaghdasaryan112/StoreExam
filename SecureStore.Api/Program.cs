using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Microsoft.OpenApi.Models;
using SecureStore.Api.ApplicationLayer.Common.ExceptionHandlers;
using SecureStore.Api.ApplicationLayer.Common.Mapping;
using SecureStore.Api.ApplicationLayer.Common.Extentions;
using SecureStore.Api.InfrastructureLayer.Datas.Data;
using SecureStore.Api.InfrastructureLayer.Config;
using SecureStore.Api.InfrastructureLayer.UnitOfWorks;
using SecureStore.Api.InfrastructureLayer.Utils;
using SecureStore.Api.InfrastructureLayer.Repositories.Implementations;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;
using SecureStore.Api.ApplicationLayer.Hendlers.QueryHandlers.Users;
using SecureStore.Api.ApplicationLayer.FluentValidation.QueriesValidator;
using SecureStore.Api.ApplicationLayer.FluentValidation.Utils;
using SecureStore.Api.ApplicationLayer.Common.OperationsWithCaching.Interfaces;
using SecureStore.Api.ApplicationLayer.Common.OperationsWithCaching.Implementations;
using SecureStore.Api.DomainLayer.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=;Database=postgres"));


builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));


var JwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();       

    

var AppSettings = builder.Configuration.GetSection("ConnectionString").Get<AppSettings>();

var RedisSettings = builder.Configuration.GetSection("ConnectionString").Get<RedisSettings>();




builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}
).AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters()
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = false,
    ValidIssuer = JwtSettings.Issuer,
    ValidAudience = JwtSettings.Audience,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.SecretKey)),
});


builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<JwtTokenHelper>();

builder.Services.AddScoped<MappingProfile>();

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetUserDTOByUserNameQueryHandler).Assembly));

builder.Services.AddValidatorsFromAssembly(typeof(UserLoginQueryValidator).Assembly);
builder.Services.AddScoped<ArgumentsValidator>();




builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();




builder.Services.AddScoped<IEntityOperationWithCaching<Product>, EntityOperationWithCaching<Product>>();
builder.Services.AddScoped<IEntityOperationWithCaching<CartItem>, EntityOperationWithCaching<CartItem>>();





builder.Services.AddScoped<ValidationExceptionHandler>();
builder.Services.AddScoped<DatabaseExceptionHandler>();




builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();



builder.Services.AddStackExchangeRedisCache(redisoption =>
{
    redisoption.Configuration = RedisSettings.RedisConnection;
});


builder.Services.AddSwaggerGen(c =>
{

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Введите JWT токен в формате 'Bearer <токен>'",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
        c.RoutePrefix = "swagger"; 
    });

}


app.UseExceptionMiddleware();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();