using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class ChangePasswordDto
    {
        [DataMember(EmitDefaultValue = false)]
        [Required(ErrorMessage = "La contraseña actual es requerida")]
        public string CurrentPassword { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        [Required(ErrorMessage = "La nueva contraseña es requerida")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
        public string NewPassword { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        [Required(ErrorMessage = "La confirmación de contraseña es requerida")]
        [Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string? cUsuarioEmail { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string? cUsuarioId { get; set; }
    }
}