using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class TokensAplicacionDto
    {
        [DataMember(EmitDefaultValue = false)]
        public string cCodigoAplicacion { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cTokenAcceso { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cTokenSecreto { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public DateTime dFechaExpiracion { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool bEsActivo { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string cNombreAplicacion { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public DateTime dFechaCreacion { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int nHorasValidez { get; set; }
    }
}