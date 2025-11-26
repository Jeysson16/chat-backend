using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ChatModularMicroservice.Entities.Models
{
    [Table("ConfiguracionAplicacion")]
    public class ConfiguracionAplicacion : BaseModel
    {
        [PrimaryKey("nConfiguracionAplicacionId")]
        public int nConfiguracionAplicacionId { get; set; }

        [Column("nAplicacionesId")]
        [Required]
        public int nAplicacionesId { get; set; }

        // Configuraciones de adjuntos
        [Column("nMaxTamanoArchivo")]
        [Range(1, 104857600)] // Máximo 100MB en bytes
        public int nMaxTamanoArchivo { get; set; } = 10485760; // 10MB por defecto

        [Column("cTiposArchivosPermitidos")]
        [StringLength(1000)]
        public string cTiposArchivosPermitidos { get; set; } = "jpg,jpeg,png,gif,pdf,doc,docx,txt,mp3,wav,mp4,avi";

        [Column("bPermitirAdjuntos")]
        public bool bPermitirAdjuntos { get; set; } = true;

        [Column("nMaxCantidadAdjuntos")]
        [Range(1, 20)]
        public int nMaxCantidadAdjuntos { get; set; } = 5;

        [Column("bPermitirVisualizacionAdjuntos")]
        public bool bPermitirVisualizacionAdjuntos { get; set; } = true;

        // Configuraciones de chat
        [Column("nMaxLongitudMensaje")]
        [Range(1, 10000)]
        public int nMaxLongitudMensaje { get; set; } = 1000;

        [Column("bPermitirEmojis")]
        public bool bPermitirEmojis { get; set; } = true;

        [Column("bPermitirMensajesVoz")]
        public bool bPermitirMensajesVoz { get; set; } = true;

        [Column("bPermitirNotificaciones")]
        public bool bPermitirNotificaciones { get; set; } = true;

        // Configuraciones de seguridad
        [Column("bRequiereAutenticacion")]
        public bool bRequiereAutenticacion { get; set; } = true;

        [Column("bPermitirMensajesAnonimos")]
        public bool bPermitirMensajesAnonimos { get; set; } = false;

        [Column("nTiempoExpiracionSesion")]
        [Range(300, 86400)] // Entre 5 minutos y 24 horas
        public int nTiempoExpiracionSesion { get; set; } = 3600; // 1 hora por defecto

        // Configuraciones de gestión de contactos
        [Column("cModoGestionContactos")]
        [StringLength(20)]
        public string cModoGestionContactos { get; set; } = "LOCAL"; // LOCAL, API_EXTERNA, HIBRIDO

        [Column("cUrlApiPersonas")]
        [StringLength(500)]
        public string? cUrlApiPersonas { get; set; }

        [Column("bSincronizarContactos")]
        public bool bSincronizarContactos { get; set; } = true;

        [Column("nTiempoCacheContactos")]
        [Range(60, 3600)] // Entre 1 minuto y 1 hora
        public int nTiempoCacheContactos { get; set; } = 300; // 5 minutos por defecto

        // Metadatos
        [Column("dFechaCreacion")]
        public DateTime dFechaCreacion { get; set; }

        [Column("dFechaActualizacion")]
        public DateTime dFechaActualizacion { get; set; }

        [Column("bEsActiva")]
        public bool bEsActiva { get; set; } = true;

        // Propiedades alias para compatibilidad con código existente
        public int Id 
        { 
            get => nConfiguracionAplicacionId; 
            set => nConfiguracionAplicacionId = value; 
        }

        public string AllowedFileTypes 
        { 
            get => cTiposArchivosPermitidos; 
            set => cTiposArchivosPermitidos = value; 
        }

        public int MaxFileSize 
        { 
            get => nMaxTamanoArchivo; 
            set => nMaxTamanoArchivo = value; 
        }

        public int MaxFilesPerMessage 
        { 
            get => nMaxCantidadAdjuntos; 
            set => nMaxCantidadAdjuntos = value; 
        }

        public bool AllowAttachments 
        { 
            get => bPermitirAdjuntos; 
            set => bPermitirAdjuntos = value; 
        }

        public DateTime CreatedAt 
        { 
            get => dFechaCreacion; 
            set => dFechaCreacion = value; 
        }

        public DateTime UpdatedAt 
        { 
            get => dFechaActualizacion; 
            set => dFechaActualizacion = value; 
        }

        public bool IsActive 
        { 
            get => bEsActiva; 
            set => bEsActiva = value; 
        }

        // Propiedades alias para gestión de contactos
        public string ContactManagementMode 
        { 
            get => cModoGestionContactos; 
            set => cModoGestionContactos = value; 
        }

        public string? PersonsApiUrl 
        { 
            get => cUrlApiPersonas; 
            set => cUrlApiPersonas = value; 
        }

        public bool SynchronizeContacts 
        { 
            get => bSincronizarContactos; 
            set => bSincronizarContactos = value; 
        }

        public int ContactsCacheTime 
        { 
            get => nTiempoCacheContactos; 
            set => nTiempoCacheContactos = value; 
        }
        // Propiedades legacy usadas por repositorio basado en SP
        public int nConfiguracionAplicacionAplicacionId { get; set; }
        public string? cConfiguracionAplicacionClave { get; set; }
        public string? cConfiguracionAplicacionValor { get; set; }
        public string? cConfiguracionAplicacionDescripcion { get; set; }
        public bool bConfiguracionAplicacionEsActiva { get; set; }
    }
}
