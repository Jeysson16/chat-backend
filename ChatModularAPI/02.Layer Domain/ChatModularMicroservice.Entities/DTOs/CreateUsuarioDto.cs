using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class CreateUsuarioDto
    {
        [DataMember(EmitDefaultValue = false)]
        [Required]
        public string Nombre { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string? Username { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        // Campos opcionales de códigos para integraciones
        [DataMember(EmitDefaultValue = false)]
        public string? PerCodigo { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string? PerJurCodigo { get; set; }
    }
}