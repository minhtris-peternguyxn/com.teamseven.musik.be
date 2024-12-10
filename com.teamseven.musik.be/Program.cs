using com.teamseven.musik.be;
using com.teamseven.musik.be.Repositories;
using com.teamseven.musik.be.Repositories.impl;
using com.teamseven.musik.be.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Cấu hình DbContext
builder.Services.AddDbContext<MusikDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Cấu hình JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            // Tạm thời bỏ qua issuer, khi frontend thống nhất chung 1 server thì tính sau
            // ValidIssuer = "your-issuer",
            // ValidAudience = "your-audience",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("cr7-is-the-goat")) // Secret key
        };
    });

// Đăng ký các dịch vụ và repository
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<PasswordEncryptionService>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<RegisterService>();
builder.Services.AddSingleton<EmailService>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Không bật HTTPS
// app.UseHttpsRedirection();

app.UseAuthentication(); // Bật middleware Authentication
app.UseAuthorization();

app.MapControllers();

app.Run();
