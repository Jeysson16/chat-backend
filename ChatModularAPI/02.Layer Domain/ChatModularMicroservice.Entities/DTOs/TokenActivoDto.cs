using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class TokenActivoDto
    {
        [DataMember(EmitDefaultValue = false)]
        public string cTokenId { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cCodigoAplicacion { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cTokenAcceso { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public bool bEsActivo { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool bEsValido { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime dFechaExpiracion { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime dFechaCreacion { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string cUsuarioId { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public int nDiasRestantes { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string cEstado { get; set; } = string.Empty;
    }
}