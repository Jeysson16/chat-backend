using System;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class ConfiguracionEmpresaDto
    {
        [DataMember(EmitDefaultValue = false)]
        public int nConfiguracionEmpresaId { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nEmpresasId { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nAplicacionesId { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cConfiguracionEmpresaClave { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cConfiguracionEmpresaValor { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cConfiguracionEmpresaTipo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cConfiguracionEmpresaDescripcion { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime dConfiguracionEmpresaFechaCreacion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime dConfiguracionEmpresaFechaActualizacion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bConfiguracionEmpresaEsActiva { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cEmpresasNombre { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cEmpresasCodigo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cAplicacionesNombre { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cAplicacionesCodigo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cConfiguracionValorAplicacion { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cConfiguracionValorEmpresa { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cConfiguracionTipo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cConfiguracionDescripcion { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public bool bConfiguracionEsPersonalizada { get; set; }
    }
}