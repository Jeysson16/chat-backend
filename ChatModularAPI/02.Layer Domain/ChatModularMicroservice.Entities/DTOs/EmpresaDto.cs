using System;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class EmpresaDto
    {
        [DataMember(EmitDefaultValue = false)]
        public int nEmpresasId { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nEmpresasAplicacionId { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cEmpresasNombre { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cEmpresasCodigo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cEmpresasDominio { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cEmpresasDescripcion { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public bool bEmpresasActiva { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime dEmpresasFechaCreacion { get; set; }
    }
}