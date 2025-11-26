namespace ChatModularAPI.Configs;

public class JwtConfig
{
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpirationMinutes { get; set; } = 60;
}

public class AppSecurityConfig
{
    public string WebhookSecret { get; set; } = string.Empty;
    public int TokenExpirationDays { get; set; } = 30;
}