using System;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class ConfiguracionDto
    {
        [DataMember(EmitDefaultValue = false)]
        public int nConfiguracionId { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cConfiguracionClave { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cConfiguracionValor { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cConfiguracionDescripcion { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cConfiguracionTipo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public bool bConfiguracionActiva { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime dConfiguracionFechaCreacion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime dConfiguracionFechaActualizacion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bConfiguracionEsActiva { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nConfiguracionTiempoExpiracionSesion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bConfiguracionPermitirNotificaciones { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bConfiguracionRequiereAutenticacion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nConfiguracionMaxCantidadAdjuntos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nConfiguracionMaxLongitudMensaje { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bConfiguracionPermitirEmojis { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nConfiguracionMaxTamanoArchivo { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cConfiguracionTiposArchivosPermitidos { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public bool bConfiguracionPermitirAdjuntos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bConfiguracionPermitirVisualizacionAdjuntos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cAplicacionesCodigo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public int nAplicacionesId { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nConfiguracionAplicacionId { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public Guid nConfiguracionId2 { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bConfiguracionPermitirMensajesAnonimos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cContactosModoGestion { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cContactosUrlApiPersonas { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public bool bContactosSincronizar { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nContactosTiempoCacheSegundos { get; set; }
    }
}