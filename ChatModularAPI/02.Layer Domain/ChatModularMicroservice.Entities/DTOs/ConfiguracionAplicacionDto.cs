using System;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class ConfiguracionAplicacionDto
    {
        [DataMember(EmitDefaultValue = false)]
        public int Id { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string CAppCodigo { get; set; } = string.Empty;
        
        // Configuraciones de adjuntos
        [DataMember(EmitDefaultValue = false)]
        public int NMaxTamanoArchivo { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string CTiposArchivosPermitidos { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public bool BPermitirAdjuntos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int NMaxCantidadAdjuntos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool BPermitirVisualizacionAdjuntos { get; set; }
        
        // Configuraciones de chat
        [DataMember(EmitDefaultValue = false)]
        public int NMaxLongitudMensaje { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool BPermitirEmojis { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool BPermitirMensajesVoz { get; set; }
        
        // Configuraciones de notificaciones
        [DataMember(EmitDefaultValue = false)]
        public bool BPermitirNotificaciones { get; set; }
        
        // Configuraciones de seguridad
        [DataMember(EmitDefaultValue = false)]
        public bool BRequiereAutenticacion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int NTiempoExpiracionSesion { get; set; }
        
        // Nuevas configuraciones
        [DataMember(EmitDefaultValue = false)]
        public bool BPermitirMensajesAnonimos { get; set; }
        
        // Metadatos
        [DataMember(EmitDefaultValue = false)]
        public bool BEsActiva { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime CreatedAt { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime UpdatedAt { get; set; }
    }
}