using System.Text;
using apiCatedra3.src.Data;
using apiCatedra3.src.Helpers;
using apiCatedra3.src.interfaces;
using apiCatedra3.src.models;
using apiCatedra3.src.Repositories;
using apiCatedra3.src.Services;
using CloudinaryDotNet;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

Env.Load();

var CloudinarySettings = builder.Configuration.GetSection("ClaudinarySettings").Get<ClaudinarySettings>();
var CloudinaryAccount = new Account(
    CloudinarySettings!.Cloudname,
    CloudinarySettings.Apikey,
    CloudinarySettings.ApiSecret
);
var cloudinary = new Cloudinary(CloudinaryAccount);



builder.Services.AddSingleton(cloudinary);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPostRepository, PostRepository>();



string nameDb = Environment.GetEnvironmentVariable("DATABASE_URL") ?? "Data Source=app.db";
builder.Services.AddDbContext<AplicationDBContext>(opt => opt.UseSqlite(nameDb));

builder.Services.AddIdentity<AppUser, IdentityRole>(
    opt => {
        opt.Password.RequireDigit = true;
        opt.Password.RequireLowercase = false;
        opt.Password.RequireUppercase = false;
        opt.Password.RequireNonAlphanumeric = false;
        opt.Password.RequiredLength = 6;
       
    }
).AddEntityFrameworkStores<AplicationDBContext>();

builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme =
    opt.DefaultChallengeScheme =
    opt.DefaultForbidScheme = 
    opt.DefaultScheme =
    opt.DefaultSignInScheme =
    opt.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options => {
    options.Events = new JwtBearerEvents{
        OnMessageReceived = context => {
            var accessToken = context.Request.Cookies["access_token"];
            
            if (!string.IsNullOrEmpty(accessToken))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = Environment.GetEnvironmentVariable("JWT_Issuer"),
        ValidateAudience = true,
        ValidAudience = Environment.GetEnvironmentVariable("JWT_Audience"),
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SigningKey") ?? throw new ArgumentNullException("Jwt:SigningKey"))),
    };
});



builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "API Cátedra 3",
        Version = "v1",
        Description = "API para gestionar publicaciones y usuarios."
    });

    // Configurar el esquema de seguridad JWT
    option.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Ingresa el token JWT en el formato: Bearer {tu_token}"
    });

    // Requerir el esquema de seguridad en las operaciones
    option.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Cátedra 3 v1");
    });
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();



app.Run();


