using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ChatModularMicroservice.Entities.Models
{
    /// <summary>
    /// Entidad para la configuración unificada de aplicaciones
    /// Representa un registro único por aplicación con todas las configuraciones como columnas
    /// </summary>
    [Table("ConfiguracionAplicacion")]
    public class ConfiguracionAplicacionUnificada : BaseModel
    {
        [PrimaryKey("nConfiguracionAplicacionId")]
        public int NConfiguracionAplicacionId { get; set; }

        [Column("nAplicacionesId")]
        [Required]
        public int NAplicacionesId { get; set; }

        // === CONFIGURACIONES DE ADJUNTOS ===
        [Column("nMaxTamanoArchivo")]
        public int? NMaxTamanoArchivo { get; set; } = 10485760; // 10MB por defecto

        [Column("cTiposArchivosPermitidos")]
        [StringLength(500)]
        public string? CTiposArchivosPermitidos { get; set; } = "jpg,jpeg,png,gif,pdf,doc,docx,txt";

        [Column("bPermitirAdjuntos")]
        public bool? BPermitirAdjuntos { get; set; } = true;

        [Column("nMaxArchivosSimultaneos")]
        public int? NMaxArchivosSimultaneos { get; set; } = 5;

        [Column("bPermitirImagenes")]
        public bool? BPermitirImagenes { get; set; } = true;

        [Column("bPermitirDocumentos")]
        public bool? BPermitirDocumentos { get; set; } = true;

        [Column("bPermitirVideos")]
        public bool? BPermitirVideos { get; set; } = false;

        [Column("bPermitirAudio")]
        public bool? BPermitirAudio { get; set; } = false;

        [Column("bRequiereAprobacionAdjuntos")]
        public bool? BRequiereAprobacionAdjuntos { get; set; } = false;

        // === CONFIGURACIONES DE CHAT ===
        [Column("nMaxLongitudMensaje")]
        public int? NMaxLongitudMensaje { get; set; } = 1000;

        [Column("bPermitirEmojis")]
        public bool? BPermitirEmojis { get; set; } = true;

        [Column("bPermitirMenciones")]
        public bool? BPermitirMenciones { get; set; } = true;

        [Column("bPermitirReacciones")]
        public bool? BPermitirReacciones { get; set; } = true;

        [Column("bPermitirEdicionMensajes")]
        public bool? BPermitirEdicionMensajes { get; set; } = true;

        [Column("bPermitirEliminacionMensajes")]
        public bool? BPermitirEliminacionMensajes { get; set; } = true;

        [Column("nTiempoLimiteEdicion")]
        public int? NTiempoLimiteEdicion { get; set; } = 300; // 5 minutos

        [Column("bPermitirMensajesPrivados")]
        public bool? BPermitirMensajesPrivados { get; set; } = true;

        // === CONFIGURACIONES DE CONVERSACIONES ===
        [Column("nMaxConversacionesSimultaneas")]
        public int? NMaxConversacionesSimultaneas { get; set; } = 10;

        [Column("bPermitirChatsGrupales")]
        public bool? BPermitirChatsGrupales { get; set; } = true;

        [Column("nMaxParticipantesGrupo")]
        public int? NMaxParticipantesGrupo { get; set; } = 50;

        [Column("bPermitirCrearGrupos")]
        public bool? BPermitirCrearGrupos { get; set; } = true;

        [Column("bRequiereAprobacionGrupos")]
        public bool? BRequiereAprobacionGrupos { get; set; } = false;

        // === CONFIGURACIONES DE CONTACTOS ===
        [Column("bPermitirAgregarContactos")]
        public bool? BPermitirAgregarContactos { get; set; } = true;

        [Column("bRequiereAprobacionContactos")]
        public bool? BRequiereAprobacionContactos { get; set; } = false;

        [Column("bPermitirBusquedaGlobal")]
        public bool? BPermitirBusquedaGlobal { get; set; } = true;

        [Column("bPermitirInvitaciones")]
        public bool? BPermitirInvitaciones { get; set; } = true;

        [Column("nMaxContactos")]
        public int? NMaxContactos { get; set; } = 500;

        [Column("cContactosModoGestion")]
        [StringLength(50)]
        public string? CContactosModoGestion { get; set; } = "LOCAL";

        // === CONFIGURACIONES DE NOTIFICACIONES ===
        [Column("bNotificacionesEmail")]
        public bool? BNotificacionesEmail { get; set; } = true;

        [Column("bNotificacionesPush")]
        public bool? BNotificacionesPush { get; set; } = true;

        [Column("bNotificacionesEscritorio")]
        public bool? BNotificacionesEscritorio { get; set; } = true;

        [Column("bNotificacionesSonido")]
        public bool? BNotificacionesSonido { get; set; } = true;

        [Column("bNotificarMenciones")]
        public bool? BNotificarMenciones { get; set; } = true;

        [Column("bNotificarMensajesPrivados")]
        public bool? BNotificarMensajesPrivados { get; set; } = true;

        // === CONFIGURACIONES DE SEGURIDAD ===
        [Column("nTiempoExpiracionSesion")]
        public int? NTiempoExpiracionSesion { get; set; } = 1440; // 24 horas en minutos

        [Column("bRequiereAutenticacion")]
        public bool? BRequiereAutenticacion { get; set; } = true;

        [Column("bPermitirSesionesMultiples")]
        public bool? BPermitirSesionesMultiples { get; set; } = true;

        [Column("bRequiere2FA")]
        public bool? BRequiere2FA { get; set; } = false;

        [Column("bEncriptarMensajes")]
        public bool? BEncriptarMensajes { get; set; } = true;

        [Column("bRegistrarActividad")]
        public bool? BRegistrarActividad { get; set; } = true;

        // === CONFIGURACIONES DE INTERFAZ ===
        [Column("bPermitirTemaOscuro")]
        public bool? BPermitirTemaOscuro { get; set; } = true;

        [Column("bPermitirPersonalizacion")]
        public bool? BPermitirPersonalizacion { get; set; } = true;

        [Column("cIdiomaDefecto")]
        [StringLength(10)]
        public string? CIdiomaDefecto { get; set; } = "es";

        [Column("cZonaHorariaDefecto")]
        [StringLength(50)]
        public string? CZonaHorariaDefecto { get; set; } = "America/Mexico_City";

        // === CONFIGURACIONES DE ALMACENAMIENTO ===
        [Column("nCuotaAlmacenamientoMB")]
        public int? NCuotaAlmacenamientoMB { get; set; } = 1024; // 1GB

        [Column("nDiasRetencionMensajes")]
        public int? NDiasRetencionMensajes { get; set; } = 365;

        [Column("bPermitirBackupAutomatico")]
        public bool? BPermitirBackupAutomatico { get; set; } = true;

        // === CONFIGURACIONES DE INTEGRACIONES ===
        [Column("bPermitirWebhooks")]
        public bool? BPermitirWebhooks { get; set; } = false;

        [Column("bPermitirAPI")]
        public bool? BPermitirAPI { get; set; } = false;

        [Column("bPermitirBots")]
        public bool? BPermitirBots { get; set; } = false;

        [Column("nMaxIntegraciones")]
        public int? NMaxIntegraciones { get; set; } = 5;

        // === METADATOS ===
        [Column("dFechaCreacion")]
        [Required]
        public DateTime DFechaCreacion { get; set; } = DateTime.UtcNow;

        [Column("dFechaActualizacion")]
        [Required]
        public DateTime DFechaActualizacion { get; set; } = DateTime.UtcNow;

        [Column("cCreadoPor")]
        [StringLength(100)]
        public string? CCreadoPor { get; set; }

        [Column("cActualizadoPor")]
        [StringLength(100)]
        public string? CActualizadoPor { get; set; }

        [Column("bEsActiva")]
        [Required]
        public bool BEsActiva { get; set; } = true;

        // === PROPIEDADES DE NAVEGACIÓN ===
        public virtual Aplicacion? Aplicacion { get; set; }

        // === PROPIEDADES ALIAS PARA COMPATIBILIDAD ===
        public int Id 
        { 
            get => NConfiguracionAplicacionId; 
            set => NConfiguracionAplicacionId = value; 
        }

        public int ApplicationId 
        { 
            get => NAplicacionesId; 
            set => NAplicacionesId = value; 
        }

        public DateTime CreatedAt 
        { 
            get => DFechaCreacion; 
            set => DFechaCreacion = value; 
        }

        public DateTime UpdatedAt 
        { 
            get => DFechaActualizacion; 
            set => DFechaActualizacion = value; 
        }

        public bool IsActive 
        { 
            get => BEsActiva; 
            set => BEsActiva = value; 
        }
    }
}
