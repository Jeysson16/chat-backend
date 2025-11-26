using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class ValidarTokenRequest
    {
        [DataMember(EmitDefaultValue = false)]
        [Required(ErrorMessage = "El código de aplicación es requerido")]
        public string cCodigoAplicacion { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        [Required(ErrorMessage = "El token de acceso es requerido")]
        public string cTokenAcceso { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cTokenSecreto { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public bool bValidarSecreto { get; set; } = true;

        [DataMember(EmitDefaultValue = false)]
        public bool bValidarExpiracion { get; set; } = true;

        [DataMember(EmitDefaultValue = false)]
        public string cUsuarioValidador { get; set; } = string.Empty;
    }
}