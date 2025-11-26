using System;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class ConfiguracionAplicacionUnificadaDto
    {
        [DataMember(EmitDefaultValue = false)]
        public int nConfiguracionAplicacionId { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nAplicacionesId { get; set; }

        // Configuraciones de Adjuntos
        [DataMember(EmitDefaultValue = false)]
        public int nAdjuntosMaxTamanoArchivo { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cAdjuntosTiposArchivosPermitidos { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public bool bAdjuntosPermitirAdjuntos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nAdjuntosMaxArchivosSimultaneos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bAdjuntosPermitirImagenes { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bAdjuntosPermitirDocumentos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bAdjuntosPermitirVideos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bAdjuntosPermitirAudio { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bAdjuntosRequiereAprobacion { get; set; }

        // Configuraciones de Chat
        [DataMember(EmitDefaultValue = false)]
        public int nChatMaxLongitudMensaje { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bChatPermitirEmojis { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bChatPermitirMenciones { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bChatPermitirReacciones { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bChatPermitirEdicionMensajes { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bChatPermitirEliminacionMensajes { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nChatTiempoLimiteEdicion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bChatPermitirMensajesPrivados { get; set; }

        // Configuraciones de Conversaciones
        [DataMember(EmitDefaultValue = false)]
        public int nConversacionesMaxSimultaneas { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bConversacionesPermitirChatsGrupales { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nConversacionesMaxParticipantesGrupo { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bConversacionesPermitirCrearGrupos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bConversacionesRequiereAprobacionGrupos { get; set; }

        // Configuraciones de Contactos
        [DataMember(EmitDefaultValue = false)]
        public bool bContactosPermitirAgregar { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bContactosRequiereAprobacion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bContactosPermitirBusquedaGlobal { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bContactosPermitirInvitaciones { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nContactosMaxContactos { get; set; }

        // Configuraciones de Gestión de Contactos
        [DataMember(EmitDefaultValue = false)]
        public string cContactosModoGestion { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cContactosUrlApiPersonas { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cContactosTokenApiPersonas { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public bool bContactosSincronizar { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nContactosTiempoCacheSegundos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bContactosHabilitarCache { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nContactosIntervaloSincronizacionMinutos { get; set; }

        // Configuraciones de Notificaciones
        [DataMember(EmitDefaultValue = false)]
        public bool bNotificacionesEmail { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bNotificacionesPush { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bNotificacionesEnTiempoReal { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bNotificacionesSonido { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bNotificacionesVibracion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cNotificacionesHorarioInicio { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cNotificacionesHorarioFin { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public bool bNotificacionesNoMolestar { get; set; }

        // Configuraciones de Seguridad
        [DataMember(EmitDefaultValue = false)]
        public bool bSeguridadRequiereAutenticacion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bSeguridadEncriptarMensajes { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nSeguridadTiempoSesionMinutos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bSeguridadRequiere2FA { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bSeguridadPermitirSesionesMultiples { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bSeguridadLogearActividad { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nSeguridadMaxIntentosLogin { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nSeguridadTiempoBloqueoMinutos { get; set; }

        // Configuraciones de Interfaz
        [DataMember(EmitDefaultValue = false)]
        public string cInterfazTema { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cInterfazIdioma { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public bool bInterfazModoOscuro { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cInterfazColorPrimario { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cInterfazColorSecundario { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cInterfazFuenteTamano { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public bool bInterfazAnimaciones { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bInterfazSonidos { get; set; }

        // Configuraciones de Almacenamiento
        [DataMember(EmitDefaultValue = false)]
        public string cAlmacenamientoTipo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cAlmacenamientoRuta { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public int nAlmacenamientoMaxEspacioMB { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bAlmacenamientoCompresion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nAlmacenamientoDiasRetencion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bAlmacenamientoLimpiezaAutomatica { get; set; }

        // Configuraciones de Rendimiento
        [DataMember(EmitDefaultValue = false)]
        public int nRendimientoMaxConexionesSimultaneas { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nRendimientoTimeoutConexionSegundos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bRendimientoHabilitarCache { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nRendimientoTiempoCacheSegundos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bRendimientoCompresionMensajes { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nRendimientoMaxMensajesPorMinuto { get; set; }

        // Configuraciones de Integración
        [DataMember(EmitDefaultValue = false)]
        public bool bIntegracionHabilitarWebhooks { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cIntegracionUrlWebhook { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cIntegracionTokenWebhook { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public bool bIntegracionHabilitarAPI { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cIntegracionVersionAPI { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public bool bIntegracionRequiereTokenAPI { get; set; }

        // Configuraciones de Moderación
        [DataMember(EmitDefaultValue = false)]
        public bool bModeracionHabilitarFiltros { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cModeracionPalabrasProhibidas { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public bool bModeracionAutoModerar { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bModeracionRequiereAprobacion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nModeracionMaxReportesPorUsuario { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bModeracionLogearAcciones { get; set; }

        // Configuraciones de Backup
        [DataMember(EmitDefaultValue = false)]
        public bool bBackupHabilitarAutomatico { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nBackupIntervaloHoras { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cBackupRutaDestino { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public bool bBackupCompresion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nBackupDiasRetencion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bBackupIncluirAdjuntos { get; set; }

        // Configuraciones de Auditoría
        [DataMember(EmitDefaultValue = false)]
        public bool bAuditoriaHabilitar { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bAuditoriaLogearMensajes { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bAuditoriaLogearConexiones { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bAuditoriaLogearCambiosConfiguracion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nAuditoriaDiasRetencionLogs { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cAuditoriaRutaLogs { get; set; } = string.Empty;

        // Fechas de control
        [DataMember(EmitDefaultValue = false)]
        public DateTime dConfiguracionFechaCreacion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime dConfiguracionFechaModificacion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cConfiguracionCreadoPor { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cConfiguracionModificadoPor { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public bool bConfiguracionEstaActiva { get; set; }
    }
}