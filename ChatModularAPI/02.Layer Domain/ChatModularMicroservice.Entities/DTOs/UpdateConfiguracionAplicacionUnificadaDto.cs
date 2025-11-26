using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class UpdateConfiguracionAplicacionUnificadaDto
    {
        // Configuraciones de Adjuntos
        [DataMember(EmitDefaultValue = false)]
        public int? NMaxTamanoArchivo { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string? CTiposArchivosPermitidos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BPermitirAdjuntos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int? NMaxArchivosSimultaneos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BPermitirImagenes { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BPermitirDocumentos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BPermitirVideos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BPermitirAudio { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BRequiereAprobacionAdjuntos { get; set; }

        // Configuraciones de Chat
        [DataMember(EmitDefaultValue = false)]
        public int? NMaxLongitudMensaje { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BPermitirEmojis { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BPermitirMenciones { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BPermitirReacciones { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BPermitirEdicionMensajes { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BPermitirEliminacionMensajes { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int? NTiempoLimiteEdicion { get; set; }

        // Configuraciones de Notificaciones
        [DataMember(EmitDefaultValue = false)]
        public bool? BNotificacionesPush { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BNotificacionesEmail { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BNotificacionesSMS { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BNotificarMensajesPrivados { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BNotificarMenciones { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BNotificarReacciones { get; set; }

        // Configuraciones de Seguridad
        [DataMember(EmitDefaultValue = false)]
        public bool? BRequiereAutenticacion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int? NTiempoSesionMinutos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int? NTiempoExpiracionSesion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BRequiere2FA { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BPermitirSesionesMultiples { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int? NMaxIntentosFallidos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int? NTiempoBloqueoMinutos { get; set; }

        // Configuraciones de Moderación
        [DataMember(EmitDefaultValue = false)]
        public bool? BRequiereModeración { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BFiltroContenidoInapropiado { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BPermitirReportes { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BAutoModeración { get; set; }

        // Configuraciones de Personalización
        [DataMember(EmitDefaultValue = false)]
        public string? CColorPrimario { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string? CColorSecundario { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string? CLogoUrl { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string? CFaviconUrl { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string? CTemaPersonalizado { get; set; }

        // Configuraciones de Integración
        [DataMember(EmitDefaultValue = false)]
        public string? CWebhookUrl { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string? CApiKey { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BIntegracionActiva { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string? CConfiguracionesAdicionales { get; set; }

        // Metadatos
        [DataMember(EmitDefaultValue = false)]
        public bool? BEsActiva { get; set; }
    }
}