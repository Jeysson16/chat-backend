using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class ConfiguracionEmpresaAgrupadaDto
    {
        // Empresa
        [DataMember(EmitDefaultValue = false)]
        public int nEmpresasId { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cEmpresasNombre { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cEmpresasCodigo { get; set; } = string.Empty;

        // Aplicación
        [DataMember(EmitDefaultValue = false)]
        public int nAplicacionesId { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cAplicacionesNombre { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cAplicacionesCodigo { get; set; } = string.Empty;

        // Configuraciones
        [DataMember(EmitDefaultValue = false)]
        public List<ConfiguracionEmpresaDto> configuraciones { get; set; } = new();
    }
}