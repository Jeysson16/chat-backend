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

// Construir opciones del host antes de crear el builder para permitir forzar entorno vía args (--RunMode Testing)
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

// Configurar URLs (permitir override por args/env). Default a 5281 para evitar conflictos.
// Nota: el argumento de línea de comandos es "--urls" en minúsculas.
var configuredUrls =
    builder.Configuration["urls"]
    ?? builder.Configuration["Urls"]
    ?? builder.Configuration["ASPNETCORE_URLS"]
    ?? Environment.GetEnvironmentVariable("ASPNETCORE_URLS")
    ?? "http://localhost:7000;http://localhost:7001";
builder.WebHost.UseUrls(configuredUrls);

// Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Agregar servicios al contenedor
builder.Services.AddControllers();

// Registrar servicios de la capa de dominio (usando los métodos de extensión correctos)
builder.Services.InyeccionDeBD(builder.Configuration);
builder.Services.InyeccionDeDepenciasClases();
builder.Services.InyeccionControllers();
builder.Services.InyeccionOtrosServicios(builder.Configuration);

// Registrar servicios de Supabase
builder.Services.AddSupabaseServices(builder.Configuration);

// Configurar CORS
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
              .AllowCredentials(); // Importante para SignalR
    });
});

// Agregar SignalR
builder.Services.AddSignalR();

// Agregar endpoints API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Los servicios ya están registrados en InyeccionDeDepenciasClases, no es necesario duplicarlos aquí

// Configurar autenticación JWT
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

// Configurar el pipeline de solicitudes HTTP
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Testing"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseResponseCompression();

app.UseCors("AllowAll");

app.UseHttpsRedirection();

// IMPORTANTE: Registrar middlewares personalizados ANTES de autenticación/autorización
// para que puedan acceder a la información de la solicitud sin restricciones
if (!app.Environment.IsEnvironment("Testing"))
{
    app.UseMiddleware<RequestBodyMiddleware>();
    app.UseMiddleware<AuthValidationMiddleware>();
}

// Desactivar autenticación/autorización en modo Testing para evitar interferencias en e2e
if (!app.Environment.IsEnvironment("Testing"))
{
    app.UseAuthentication();
    app.UseAuthorization();
}

app.MapControllers();

// Configurar SignalR hubs
app.MapHub<ChatHub>("/chathub");

try
{
    Log.Information("Iniciando aplicación ChatModular API");
    Log.Information("Servidor escuchando en: {Urls}", configuredUrls);
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "La aplicación falló al iniciar");
}
finally
{
    Log.CloseAndFlush();
}