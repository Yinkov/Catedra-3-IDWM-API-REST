using System.Text;
using apiCatedra3.src.Data;
using apiCatedra3.src.Helpers;
using apiCatedra3.src.interfaces;
using apiCatedra3.src.models;
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






var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();



app.Run();


