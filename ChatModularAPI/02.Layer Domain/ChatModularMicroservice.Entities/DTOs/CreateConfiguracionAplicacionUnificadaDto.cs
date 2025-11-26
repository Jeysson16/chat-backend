using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class CreateConfiguracionAplicacionUnificadaDto
    {
        [DataMember(EmitDefaultValue = false)]
        [Required(ErrorMessage = "El ID de aplicación es requerido")]
        public int NAplicacionesId { get; set; }

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
        public bool? BNotificacionesEnApp { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BNotificacionesSonido { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BNotificacionesVibracion { get; set; }

        // Configuraciones de Seguridad
        [DataMember(EmitDefaultValue = false)]
        public bool? BRequiereAutenticacion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int? NTiempoSesionMinutos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BRequiere2FA { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BPermitirSesionesMultiples { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int? NMaxIntentosFallidos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int? NTiempoBloqueoMinutos { get; set; }

        // Configuraciones de Privacidad
        [DataMember(EmitDefaultValue = false)]
        public bool? BPermitirMensajesAnonimos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BPermitirHistorialMensajes { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int? NDiasRetencionMensajes { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BEncriptarMensajes { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BPermitirCapturaPantalla { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BPermitirCopiado { get; set; }

        // Configuraciones de Moderación
        [DataMember(EmitDefaultValue = false)]
        public bool? BHabilitarModeracion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BFiltroContenidoInapropiado { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BFiltroSpam { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BRequiereAprobacionMensajes { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int? NMaxReportesPorUsuario { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BAutobanPorReportes { get; set; }

        // Configuraciones de Personalización
        [DataMember(EmitDefaultValue = false)]
        public bool? BPermitirTemas { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string? CTemaPorDefecto { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BPermitirAvatares { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BPermitirEstadosPersonalizados { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BPermitirFondosPersonalizados { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string? CIdiomasPorDefecto { get; set; }

        // Configuraciones de Integración
        [DataMember(EmitDefaultValue = false)]
        public bool? BHabilitarWebhooks { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string? CWebhookUrl { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BHabilitarAPI { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BHabilitarBots { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int? NMaxBotsPermitidos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BHabilitarIntegracionesExternas { get; set; }

        // Configuraciones de Almacenamiento
        [DataMember(EmitDefaultValue = false)]
        public string? CTipoAlmacenamiento { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string? CRutaAlmacenamiento { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int? NMaxEspacioMB { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BCompresionAlmacenamiento { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int? NDiasRetencionAlmacenamiento { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BLimpiezaAutomatica { get; set; }

        // Configuraciones de Rendimiento
        [DataMember(EmitDefaultValue = false)]
        public int? NMaxConexionesSimultaneas { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int? NTimeoutConexionSegundos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BHabilitarCache { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int? NTiempoCacheSegundos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BCompresionMensajes { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int? NMaxMensajesPorMinuto { get; set; }
    }
}