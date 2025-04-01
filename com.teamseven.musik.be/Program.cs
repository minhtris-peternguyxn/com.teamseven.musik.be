using System.Reflection;
using System.Text;
using Azure.Storage.Blobs;
using com.teamseven.musik.be.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using com.teamseven.musik.be.Models.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using com.teamseven.musik.be.Services;

var builder = WebApplication.CreateBuilder(args);

// ================= CẤU HÌNH DB =================
builder.Services.AddDbContext<MusikDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Vui lòng nhập Bearer Token (VD: Bearer eyJhbGciOi...)",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
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
            new string[] { }
        }
    });
});

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
app.UseDatabaseKeepAlive();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

// ================= HÀM CẤU HÌNH AUTHENTICATION =================
void ConfigureAuthentication(IServiceCollection services, IConfiguration config)
{
    // Lấy khóa JWT từ appsettings.json
    var jwtKey = config["Jwt:Key"];
    if (string.IsNullOrEmpty(jwtKey))
    {
        throw new InvalidOperationException("JWT Key is missing in configuration.");
    }

    // JWT Authentication
    services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false, // Không kiểm tra hết hạn
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)) // Dùng khóa từ config
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("JWT Authentication failed: " + context.Exception.Message);
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("JWT Token validated successfully.");
                return Task.CompletedTask;
            }
        };
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

    // Authorization policies
    services.AddAuthorization(options =>
    {
        options.AddPolicy("DeliveringStaffPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
        options.AddPolicy("SaleStaffPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "Staff"));
    });
}