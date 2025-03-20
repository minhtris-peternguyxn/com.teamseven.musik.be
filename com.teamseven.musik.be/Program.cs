using System.Reflection;
using System.Text;
using Azure.Storage.Blobs;
using com.teamseven.musik.be.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication;
using com.teamseven.musik.be.Models.Contexts;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ================= CẤU HÌNH DB =================
builder.Services.AddDbContext<MusikDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//h xài mysql rồi :))


// ================= CẤU HÌNH AUTHENTICATION =================
ConfigureAuthentication(builder.Services, builder.Configuration);

// ================= ĐĂNG KÝ REPOSITORY & SERVICE =================
builder.Services.AddRepositories(Assembly.GetExecutingAssembly());
builder.Services.AddServices(Assembly.GetExecutingAssembly());

// ================= CẤU HÌNH BLOB STORAGE =================
var blobServiceClient = new BlobServiceClient(builder.Configuration["AzureStorage:ConnectionString"]);
builder.Services.AddSingleton(blobServiceClient);

// ================= CẤU HÌNH CORS =================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.WebHost.UseUrls("https://0.0.0.0:5000");
// ================= CẤU HÌNH SWAGGER =================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ================= CẤU HÌNH AUTOMAPPER =================
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// ================= CẤU HÌNH CONTROLLERS =================
builder.Services.AddControllers();

var app = builder.Build();

// ================= MIDDLEWARE =================
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

// ================= HÀM CẤU HÌNH AUTHENTICATION =================
void ConfigureAuthentication(IServiceCollection services, IConfiguration config)
{
    // JWT Authentication
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false, // Không yêu cầu xác thực Lifetime cho app nghe nhạc
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("cr7-is-the-goat"))
            };
        });
    // Add Authorization policies
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("DeliveringStaffPolicy", policy => policy.RequireClaim("role", "Admin"));
        options.AddPolicy("SaleStaffPolicy", policy => policy.RequireClaim("role", "Staff"));
    });

    // Google Authentication
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
