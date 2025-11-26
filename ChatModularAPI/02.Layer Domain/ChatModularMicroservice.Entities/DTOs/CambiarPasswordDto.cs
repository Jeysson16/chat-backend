using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class CambiarPasswordDto
    {
        [DataMember(EmitDefaultValue = false)]
        [Required]
        public string PasswordActual { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        [Required]
        [MinLength(8)]
        public string NuevaPassword { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        [Required]
        [Compare("NuevaPassword", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmarPassword { get; set; } = string.Empty;
    }
}