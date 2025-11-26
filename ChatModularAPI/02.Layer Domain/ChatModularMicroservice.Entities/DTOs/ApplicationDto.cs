using System;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class ApplicationDto
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
    }
}