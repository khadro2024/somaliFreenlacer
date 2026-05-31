using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SomaliFreelanceMarketplace.Configuration;
using SomaliFreelanceMarketplace.Data;
using SomaliFreelanceMarketplace.Helpers;
using SomaliFreelanceMarketplace.Models.Entities;
using SomaliFreelanceMarketplace.Models.Enums;
using SomaliFreelanceMarketplace.Services;
using SomaliFreelanceMarketplace.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Somali Freelance Marketplace API",
        Version = "v1",
        Description = "SFM - Freelance marketplace for Somalia"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header. Example: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

var connectionString = DatabaseUrlHelper.ResolveConnectionString(builder.Configuration)
    ?? throw new InvalidOperationException("Database connection not configured. Set DATABASE_URL or ConnectionStrings__DefaultConnection.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key is not configured. Set Jwt__Key environment variable.");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

var corsOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? ["http://localhost:5173"];
var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL");
if (!string.IsNullOrWhiteSpace(frontendUrl) && !corsOrigins.Contains(frontendUrl))
    corsOrigins = corsOrigins.Append(frontendUrl).ToArray();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.WithOrigins(corsOrigins).AllowAnyHeader().AllowAnyMethod().AllowCredentials());
});

builder.Services.AddScoped<JwtHelper>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));
builder.Services.AddSingleton<CloudinaryStorageService>();
builder.Services.AddScoped<IMediaService, MediaService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();

    if (!db.Users.Any(u => u.Role == UserRole.Admin))
    {
        db.Users.Add(new User
        {
            FullName = "SFM",
            Email = "admin@sfm.so",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Grils wep12"),
            Role = UserRole.Admin,
            IsVerified = true
        });
        db.SaveChanges();
    }

    if (app.Environment.IsDevelopment())
    {
        var adminLogin = db.Users.FirstOrDefault(u => u.Email == "admin@sfm.so");
        if (adminLogin != null)
        {
            adminLogin.FullName = "SFM";
            adminLogin.PasswordHash = BCrypt.Net.BCrypt.HashPassword("Grils wep12");
            adminLogin.IsSuspended = false;
            adminLogin.IsVerified = true;
            adminLogin.Role = UserRole.Admin;
            db.SaveChanges();
        }

        DemoDataSeeder.Seed(db);
        DemoImageSeeder.Seed(db);
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
