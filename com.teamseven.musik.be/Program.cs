﻿using System.Reflection;
using System.Text;
using Azure.Storage.Blobs;
using com.teamseven.musik.be.Repositories.interfaces;
using com.teamseven.musik.be.Repositories.impl;
using com.teamseven.musik.be.Services.Authentication;
using com.teamseven.musik.be.Services.QueryDB;
using com.teamseven.musik.be.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// ========== CẤU HÌNH DATABASE ==========
builder.Services.AddDbContext<com.teamseven.musik.be.Models.Contexts.MusikDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ========== CẤU HÌNH AUTHENTICATION ==========
ConfigureAuthentication(builder.Services, builder.Configuration);

// ========== ĐĂNG KÝ REPOSITORY & SERVICE ==========
builder.Services.AddRepositories(Assembly.GetExecutingAssembly());
builder.Services.AddServices(Assembly.GetExecutingAssembly());

// ========== CẤU HÌNH BLOB STORAGE ==========
var blobServiceClient = new BlobServiceClient(builder.Configuration["AzureStorage:ConnectionString"]);
builder.Services.AddSingleton(blobServiceClient);

// ========== CẤU HÌNH CORS ==========
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// ========== CẤU HÌNH SWAGGER ==========
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ========== CẤU HÌNH AUTOMAPPER ==========
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// ========== CẤU HÌNH CONTROLLERS ==========
builder.Services.AddControllers();

var app = builder.Build();

// ========== MIDDLEWARE ==========
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

// ========== HÀM CẤU HÌNH AUTHENTICATION ==========
void ConfigureAuthentication(IServiceCollection services, IConfiguration config)
{
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false, // Không cần Lifetime cho App nghe nhạc
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("cr7-is-the-goat"))
            };
        });

    services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = config["Authentication:Google:ClientId"];
        googleOptions.ClientSecret = config["Authentication:Google:ClientSecret"];
        googleOptions.SaveTokens = true;
        googleOptions.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
        googleOptions.ClaimActions.MapJsonKey("urn:google:locale", "locale", "string");
    });
}
