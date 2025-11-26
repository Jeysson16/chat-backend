using System;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class CreateApplicationResponseDto
    {
        [DataMember(EmitDefaultValue = false)]
        public int nAplicacionesId { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cAplicacionesNombre { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cAplicacionesDescripcion { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cAplicacionesCodigo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public bool bAplicacionesEsActiva { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime dAplicacionesFechaCreacion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cAppRegistroTokenAcceso { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cAppRegistroSecretoApp { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime? dAppRegistroFechaExpiracion { get; set; }
    }
}