using System.ComponentModel.DataAnnotations;

namespace ChatModularMicroservice.Entities.DTOs;

/// <summary>
/// DTO temporal para el registro de usuario sin validación de tokens (pruebas)
/// </summary>
public class RegisterDtoTesting
{
    // Tokens de aplicación - opcionales para pruebas
    public string? cTokenAcceso { get; set; }
    public string? cTokenSecreto { get; set; }

    // Datos de usuario
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string cUsuariosNombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    public string cUsuariosEmail { get; set; } = string.Empty;

    // Alias opcional para compatibilidad (username/código de usuario)
    [StringLength(50, ErrorMessage = "El código de usuario no puede exceder 50 caracteres")]
    public string? cUsuariosCodigo { get; set; }

    [Required(ErrorMessage = "La contraseña es requerida")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
    public string cUsuariosPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "La confirmación de contraseña es requerida")]
    [Compare("cUsuariosPassword", ErrorMessage = "Las contraseñas no coinciden")]
    public string cUsuariosConfirmarPassword { get; set; } = string.Empty;

    // Código de empresa/persona jurídica y persona
    public string? cUsuariosPerJurCodigo { get; set; } = "DEFAULT";

    public string? cUsuariosPerCodigo { get; set; } = string.Empty;
}