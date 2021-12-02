using AirSoft.Data;
using AirSoft.Service.Common;
using AirSoft.Service.Contracts;
using AirSoft.Service.Contracts.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using System.Text;
using System.Text.Json.Serialization;
using AirSoft.Service.Contracts.Auth;
using AirSoft.Service.Implementations;
using AirSoft.Service.Implementations.Auth;
using AirSoft.Service.Implementations.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    var parentDir = hostingContext.HostingEnvironment.ContentRootPath;
    var path = string.Concat(parentDir, "\\ConfigSource\\appsettings.json");

    config.AddJsonFile(path);
});
var settingsSection = builder.Configuration.GetSection(nameof(AppSettings));
var appSettings = settingsSection.Get<AppSettings>();
builder.Services.Configure<AppSettings>(settingsSection);
var configService = new ConfigService(appSettings);

builder.Host.UseNLog();
// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = appSettings.Jwt?.Issuer,
            ValidAudience = appSettings.Jwt?.Issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Jwt?.Key ?? throw new ApplicationException("Jwt settings in null")))
        };
    });


builder.Services.AddDbContext<AirSoftDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AirSoftDatabase"), opt => opt.MigrationsHistoryTable("__EFMigrationsHistory", "dbo")));
builder.Services.AddScoped<IDbContext, AirSoftDbContext>();
builder.Services.AddScoped<IConfigService, ConfigService>(p => configService);
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDataService, DataService>();

builder.Services.AddControllers();
builder.Services.AddMvc().AddJsonOptions(x => x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
using var context = serviceScope.ServiceProvider.GetService<IDbContext>();
context?.Initialize();

string root = app.Environment.ContentRootPath;
NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(root + "\\ConfigSource\\NLog.config");
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseCors(b =>
{
    b.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();