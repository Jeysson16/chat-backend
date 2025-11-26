using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class UpdateUsuarioDto
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
        public string? Telefono { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string? Avatar { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool EsActivo { get; set; }
    }
}