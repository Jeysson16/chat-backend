using Microsoft.AspNetCore.Authorization;

namespace ChatModularMicroservice.Entities.Attributes;

/// <summary>
/// Atributo personalizado para autorización
/// </summary>
public class AuthorizeAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute
{
    public AuthorizeAttribute()
    {
    }

    public AuthorizeAttribute(string policy) : base(policy)
    {
    }
}