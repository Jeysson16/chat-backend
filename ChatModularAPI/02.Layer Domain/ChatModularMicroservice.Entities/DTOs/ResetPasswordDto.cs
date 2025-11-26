using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class ResetPasswordDto
    {
        [DataMember(EmitDefaultValue = false)]
        [Required(ErrorMessage = "El token es requerido")]
        public string Token { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        [Required(ErrorMessage = "La nueva contraseña es requerida")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string NewPassword { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        [Required(ErrorMessage = "La confirmación de contraseña es requerida")]
        [Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cUsuarioEmail { get; set; } = string.Empty;
    }
}