using Supabase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatModularAPI.Configs;

public class SupabaseConfig
{
    public string Url { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string ServiceRoleKey { get; set; } = string.Empty;
}

public static class SupabaseServiceExtensions
{
    public static IServiceCollection AddSupabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        var supabaseConfig = new SupabaseConfig();
        configuration.GetSection("Supabase").Bind(supabaseConfig);
        
        if (string.IsNullOrEmpty(supabaseConfig.Url) || string.IsNullOrEmpty(supabaseConfig.Key))
        {
            throw new InvalidOperationException("Supabase configuration is missing or incomplete");
        }

        services.AddSingleton(supabaseConfig);

        services.AddScoped<Supabase.Client>(provider =>
        {
            var options = new SupabaseOptions
            {
                AutoConnectRealtime = true,
                AutoRefreshToken = true
            };

            return new Supabase.Client(supabaseConfig.Url, supabaseConfig.ServiceRoleKey, options);
        });

        return services;
    }
}