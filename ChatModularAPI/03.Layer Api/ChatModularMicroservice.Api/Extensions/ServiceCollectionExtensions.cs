using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;
using System.Data.Common;
using System.Reflection;
using ChatModularMicroservice.Entities;
// using ChatModularMicroservice.Api.Filters; // Removed - filter class not present
using ChatModularMicroservice.Infrastructure;
using ChatModularMicroservice.Infrastructure.Repositories;
using ChatModularMicroservice.Repository;
using ChatModularMicroservice.Shared.Configs;
using ChatModularMicroservice.Domain;
using ChatModularMicroservice.Shared.Utils;

namespace ChatModularMicroservice.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection InyeccionDeBD(this IServiceCollection services, IConfiguration Configuration)
        {
            // Intentar obtener cadena de conexión "cnBD"; si no existe, usar "DefaultConnection"
            var cnString = Configuration.GetConnectionString("cnBD")
                            ?? Configuration.GetConnectionString("DefaultConnection")
                            ?? string.Empty;

            // En modo Testing, si sigue vacía, no romper la DI; usar una cadena de conexión segura por defecto
            var env = Configuration["ASPNETCORE_ENVIRONMENT"] ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? string.Empty;
            if (string.IsNullOrWhiteSpace(cnString) && string.Equals(env, "Testing", StringComparison.OrdinalIgnoreCase))
            {
                // Nota: Esta cadena apunta a master y permite abrir conexión sin requerir BD específica del proyecto
                cnString = "Server=localhost;Database=master;Trusted_Connection=true;TrustServerCertificate=true;";
            }
            services.AddScoped<IConnectionFactory>(provider => new ConnectionFactory(cnString));
            return services;
        }
        public static IServiceCollection InyeccionDeDepenciasClases(this IServiceCollection services)
        {
            var executableLocation = Assembly.GetEntryAssembly()?.Location ?? AppContext.BaseDirectory;
            var pathAssembly = Path.GetDirectoryName(executableLocation);
            var allTypesDll = Directory.GetFiles(pathAssembly, "ChatModularMicroservice*.dll", SearchOption.TopDirectoryOnly)
             .Select(Assembly.LoadFrom).ToList();

            var allTypes = allTypesDll
             .SelectMany(assembly => assembly.GetTypes())
             .ToList();

            services.AddScoped<IApplicationRepository, ApplicationSupabaseRepository>();
            services.AddScoped<ITokenRepository, TokenSupabaseRepository>();

            // Registro automático de dominios
            allTypes.Where(type => type.Name.EndsWith("Domain"))
                .ToList().ForEach(domainType =>
                {
                    services.AddScoped(domainType);
                });

            // Registro explícito de servicios
            // Adquisición no forma parte del alcance actual; se elimina el registro

            // Registrar servicios necesarios para los controladores
            services.AddHttpContextAccessor();
            // Repositorios/Adapters explícitos que no coinciden con el registro automático
            services.AddScoped<ChatModularMicroservice.Infrastructure.Repositories.AppRegistroRepository>();
            services.AddScoped<IAppRegistroRepository, ChatModularMicroservice.Infrastructure.Repositories.AppRegistroRepositoryAdapter>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            // Registro explícito de ChatService para resolver IChatService en controladores y hubs
            services.AddScoped<IChatService, ChatService>();
            // Registrar ApplicationService (capa Domain) para resolver IApplicationService en AplicacionesController
            services.AddScoped<IApplicationService, ChatModularMicroservice.Domain.ApplicationService>();
            // Registrar ContactoService (capa Domain) para resolver IContactoService en ContactoController
            services.AddScoped<IContactoService, ContactoService>();
            // Registrar UsuarioService para resolver IUsuarioService en UsuariosController
            services.AddScoped<IUsuarioService, UsuarioService>();
            
            // Registrar ConfiguracionAplicacionUnificadaService para resolver IConfiguracionAplicacionUnificadaService en ConfiguracionAplicacionUnificadaController
            services.AddScoped<IConfiguracionAplicacionUnificadaService, ConfiguracionAplicacionUnificadaService>();
            
            // Registrar EmpresaServiceAdapter para resolver IEmpresaService en EmpresasController
            services.AddScoped<IEmpresaService, ChatModularMicroservice.Api.Services.EmpresaServiceAdapter>();
            services.AddScoped<IEmpresaRepository, ChatModularMicroservice.Infrastructure.Repositories.EmpresaSupabaseRepository>();
            
            // Registrar ConfiguracionEmpresaServiceAdapter para resolver IConfiguracionEmpresaService en ConfiguracionEmpresaController
            services.AddScoped<IConfiguracionEmpresaService, ChatModularMicroservice.Api.Services.ConfiguracionEmpresaServiceAdapter>();
            // Registrar repositorio faltante para configuraciones de empresa (hexagonal: infraestructura repositorio, dominio servicio)
            services.AddScoped<IConfiguracionEmpresaRepository, ChatModularMicroservice.Infrastructure.Repositories.ConfiguracionEmpresaRepository>();
            
            // Registrar ConfiguracionServiceAdapter para resolver IConfiguracionService
            services.AddScoped<IConfiguracionService, ChatModularMicroservice.Api.Services.ConfiguracionServiceAdapter>();
            
            // Registrar ConfiguracionChatService para resolver IConfiguracionChatService en ConfiguracionChatController
            services.AddScoped<IConfiguracionChatService, ChatModularMicroservice.Domain.Services.ConfiguracionChatService>();
            
            // No registrar PersonaService ya que no existe un adapter correspondiente
            
            services.AddScoped<IContactoRepository, ContactoSupabaseRepository>();
            
            // Registro explícito de IChatRepository para resolver dependencias de ChatService
            services.AddScoped<IChatRepository, ChatRepository>();

            // Registrar IUsuarioRepository para resolver dependencias de AuthService y UsuarioService
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            
            // Registrar IConfiguracionAplicacionUnificadaRepository para resolver dependencias de ConfiguracionAplicacionUnificadaService
            services.AddScoped<IConfiguracionAplicacionUnificadaRepository, ConfiguracionAplicacionRepository>();

            return services;
        }
        public static IServiceCollection InyeccionControllers(this IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            ValidacionFiltros(services);
            // Swagger registration moved to Program.cs to avoid duplicate registration

            return services;
        }
        private static void ValidacionFiltros(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                // CustomExceptionFilter removed; using default behavior
                // options.Filters.Add<CustomExceptionFilter>();
            }).
                ConfigureApiBehaviorOptions(options =>
                {
                    var builtInFactory = options.InvalidModelStateResponseFactory;

                    options.InvalidModelStateResponseFactory = context =>
                    {
                        BadRequestResponse badResponse = new BadRequestResponse();
                        var logger = context.HttpContext.RequestServices
                                            .GetRequiredService<ILogger<Program>>();

                        var modelo = context.ModelState;
                        var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
                        var parameter = actionDescriptor?.Parameters.FirstOrDefault(p => p.BindingInfo.BindingSource.Id.Equals("Body", StringComparison.OrdinalIgnoreCase));

                        if (!modelo.IsValid)
                        {
                            var errors = modelo.Keys
                            .SelectMany(key => modelo[key].Errors.Select(x => new EResponse
                            {
                                cDescripcion = $"El campo '{key}' de la entidad no es válido. Error: {x.ErrorMessage}",
                                Info = "Ingrese un valor válido para seguir con la solicitud"
                            })).ToList();

                            badResponse.LstError.AddRange(errors);
                            return new BadRequestObjectResult(badResponse);
                        }
                        return builtInFactory(context);
                    };
                });

        }
        public static IServiceCollection InyeccionOtrosServicios(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                //options.Providers.Add<GzipCompressionProvider>();
                options.Providers.Add<BrotliCompressionProvider>();
                options.MimeTypes = new[] { "application/json" };
            });
            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = System.IO.Compression.CompressionLevel.Fastest;
            });

            // Registrar configuración de logger para middlewares que la requieren
            services.AddSingleton(LoggerConfig.GetDevelopmentConfig());
            return services;
        }
    }
}
