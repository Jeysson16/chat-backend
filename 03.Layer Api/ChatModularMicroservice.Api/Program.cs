using ChatModularMicroservice.Api.Extensions;
using ChatModularMicroservice.Api.Hubs;
using ChatModularMicroservice.Api.Middleware;
using Utils = ChatModularMicroservice.Shared.Utils;
using Microsoft.AspNetCore.SignalR;
using Serilog;
using Microsoft.AspNetCore.Builder;
using ChatModularMicroservice.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ChatModularMicroservice.Shared.Configs;
using System.Threading.Tasks;

string? GetArgValue(string[] a, string name)
{
    for (int i = 0; i < a.Length; i++)
    {
        var current = a[i];
        var prefix = $"--{name}=";
        if (current.Equals($"--{name}", StringComparison.OrdinalIgnoreCase))
        {
            return i + 1 < a.Length ? a[i + 1] : null;
        }
        if (current.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        {
            return current.Substring(prefix.Length);
        }
    }
    return null;
}

var runModeArg = GetArgValue(args, "RunMode");
var options = string.Equals(runModeArg, "Testing", StringComparison.OrdinalIgnoreCase)
    ? new WebApplicationOptions { Args = args, EnvironmentName = "Testing" }
    : new WebApplicationOptions { Args = args };

var builder = WebApplication.CreateBuilder(options);

var configuredUrls =
    builder.Configuration["urls"]
    ?? builder.Configuration["Urls"]
    ?? builder.Configuration["ASPNETCORE_URLS"]
    ?? Environment.GetEnvironmentVariable("ASPNETCORE_URLS")
    ?? "http://localhost:5405;http://localhost:5406";
builder.WebHost.UseUrls(configuredUrls);

Serilog.Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();

builder.Services.AddSupabaseServices(builder.Configuration);

builder.Services.InyeccionDeBD(builder.Configuration);
builder.Services.InyeccionDeDepenciasClases();
builder.Services.InyeccionControllers();
builder.Services.InyeccionOtrosServicios(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4500",
                "http://localhost:4501",
                "https://localhost:4500",
                "https://localhost:4501"
            )
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services.AddSignalR();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "ChatModular API",
        Version = "v1",
        Description = "API para el sistema modular de chat"
    });
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IChatService, ChatService>();

var jwtKey = builder.Configuration["Jwt:Key"] ?? builder.Configuration["Jwt:SecretKey"] ?? string.Empty;
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "ChatModularAPI";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "ChatModularAPI";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chathub"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChatModular API V1");
    c.RoutePrefix = "swagger";
});

app.UseResponseCompression();
app.UseCors("AllowAll");

if (!app.Environment.IsEnvironment("Testing"))
{
    app.UseAuthentication();
    app.UseAuthorization();
}

app.MapControllers();
app.MapHub<ChatHub>("/chathub");

try
{
    Serilog.Log.Information("Iniciando aplicación ChatModular API");
    Serilog.Log.Information("Servidor escuchando en: {Urls}", configuredUrls);
    app.Run();
}
catch (Exception ex)
{
    Serilog.Log.Fatal(ex, "La aplicación falló al iniciar");
}
finally
{
    Serilog.Log.CloseAndFlush();
}
