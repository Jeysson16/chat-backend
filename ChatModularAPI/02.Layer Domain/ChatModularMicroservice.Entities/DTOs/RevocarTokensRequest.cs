using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class RevocarTokensRequest
    {
        [DataMember(EmitDefaultValue = false)]
        [Required(ErrorMessage = "El código de aplicación es requerido")]
        public string cCodigoAplicacion { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cTokenAcceso { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cMotivoRevocacion { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public bool bRevocarTodos { get; set; } = false;

        [DataMember(EmitDefaultValue = false)]
        public string cUsuarioRevocador { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public DateTime dFechaRevocacion { get; set; } = DateTime.UtcNow;

        [DataMember(EmitDefaultValue = false)]
        public bool bForzarRevocacion { get; set; } = false;
    }
}