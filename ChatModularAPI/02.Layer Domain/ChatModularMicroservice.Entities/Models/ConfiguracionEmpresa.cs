using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ChatModularMicroservice.Entities.Models
{
    [Table("ConfiguracionEmpresa")]
    public class ConfiguracionEmpresa : BaseModel
    {
        [PrimaryKey("nConfigEmpresaId")]
        public int nConfigEmpresaId { get; set; }

        // Configuración Básica
        [Column("cConfigEmpresaNombre")]
        [Required]
        [StringLength(100)]
        public string cConfigEmpresaNombre { get; set; } = string.Empty;

        [Column("cConfigEmpresaDescripcion")]
        [StringLength(500)]
        public string? cConfigEmpresaDescripcion { get; set; }

        [Column("cConfigEmpresaDominio")]
        [Required]
        [StringLength(100)]
        public string cConfigEmpresaDominio { get; set; } = string.Empty;

        // Configuración de Marca
        [Column("cConfigEmpresaColorPrimario")]
        [Required]
        [StringLength(7)]
        public string cConfigEmpresaColorPrimario { get; set; } = "#233559";

        [Column("cConfigEmpresaColorSecundario")]
        [Required]
        [StringLength(7)]
        public string cConfigEmpresaColorSecundario { get; set; } = "#C90000";

        [Column("cConfigEmpresaUrlLogo")]
        [StringLength(500)]
        public string? cConfigEmpresaUrlLogo { get; set; }

        [Column("cConfigEmpresaFuentePersonalizada")]
        [StringLength(200)]
        public string cConfigEmpresaFuentePersonalizada { get; set; } = "Roboto, Arial, sans-serif";

        // Límites de Usuario y Recursos
        [Column("nConfigEmpresaMaxUsuarios")]
        [Range(1, 10000)]
        public int nConfigEmpresaMaxUsuarios { get; set; } = 100;

        [Column("nConfigEmpresaMaxCanales")]
        [Range(1, 1000)]
        public int nConfigEmpresaMaxCanales { get; set; } = 50;

        [Column("nConfigEmpresaCuotaAlmacenamientoGB")]
        [Range(1, 1000)]
        public int nConfigEmpresaCuotaAlmacenamientoGB { get; set; } = 10;

        [Column("nConfigEmpresaTiempoSesionMinutos")]
        [Range(5, 1440)]
        public int nConfigEmpresaTiempoSesionMinutos { get; set; } = 30;

        // Banderas de Características
        [Column("bConfigEmpresaHabilitarCompartirArchivos")]
        [Required]
        public bool bConfigEmpresaHabilitarCompartirArchivos { get; set; } = true;

        [Column("bConfigEmpresaHabilitarNotificaciones")]
        [Required]
        public bool bConfigEmpresaHabilitarNotificaciones { get; set; } = true;

        [Column("bConfigEmpresaHabilitarIntegraciones")]
        [Required]
        public bool bConfigEmpresaHabilitarIntegraciones { get; set; } = false;

        [Column("bConfigEmpresaHabilitarAnaliticas")]
        [Required]
        public bool bConfigEmpresaHabilitarAnaliticas { get; set; } = true;

        [Column("bConfigEmpresaEsActiva")]
        [Required]
        public bool bConfigEmpresaEsActiva { get; set; } = true;

        [Column("dConfigEmpresaFechaCreacion")]
        [Required]
        public DateTime dConfigEmpresaFechaCreacion { get; set; } = DateTime.UtcNow;

        [Column("dConfigEmpresaFechaActualizacion")]
        [Required]
        public DateTime dConfigEmpresaFechaActualizacion { get; set; } = DateTime.UtcNow;

        // Claves Foráneas
        [Column("nConfigEmpresaAplicacionId")]
        [Required]
        public int nConfigEmpresaAplicacionId { get; set; }

        [Column("nConfigEmpresaEmpresaId")]
        [Required]
        public int nConfigEmpresaEmpresaId { get; set; }

        // Propiedades de navegación
        public virtual Aplicacion? Aplicacion { get; set; }
        public virtual Empresa? Empresa { get; set; }

        // Propiedades alias para compatibilidad con código existente
        public int Id 
        { 
            get => nConfigEmpresaId; 
            set => nConfigEmpresaId = value; 
        }

        public string Name 
        { 
            get => cConfigEmpresaNombre; 
            set => cConfigEmpresaNombre = value; 
        }

        public string? Description 
        { 
            get => cConfigEmpresaDescripcion; 
            set => cConfigEmpresaDescripcion = value; 
        }

        public string Domain 
        { 
            get => cConfigEmpresaDominio; 
            set => cConfigEmpresaDominio = value; 
        }

        public string PrimaryColor 
        { 
            get => cConfigEmpresaColorPrimario; 
            set => cConfigEmpresaColorPrimario = value; 
        }

        public string SecondaryColor 
        { 
            get => cConfigEmpresaColorSecundario; 
            set => cConfigEmpresaColorSecundario = value; 
        }

        public string? LogoUrl 
        { 
            get => cConfigEmpresaUrlLogo; 
            set => cConfigEmpresaUrlLogo = value; 
        }

        public string CustomFont 
        { 
            get => cConfigEmpresaFuentePersonalizada; 
            set => cConfigEmpresaFuentePersonalizada = value; 
        }

        public int MaxUsers 
        { 
            get => nConfigEmpresaMaxUsuarios; 
            set => nConfigEmpresaMaxUsuarios = value; 
        }

        public int MaxChannels 
        { 
            get => nConfigEmpresaMaxCanales; 
            set => nConfigEmpresaMaxCanales = value; 
        }

        public int StorageQuotaGB 
        { 
            get => nConfigEmpresaCuotaAlmacenamientoGB; 
            set => nConfigEmpresaCuotaAlmacenamientoGB = value; 
        }

        public int SessionTimeoutMinutes 
        { 
            get => nConfigEmpresaTiempoSesionMinutos; 
            set => nConfigEmpresaTiempoSesionMinutos = value; 
        }

        public bool EnableFileSharing 
        { 
            get => bConfigEmpresaHabilitarCompartirArchivos; 
            set => bConfigEmpresaHabilitarCompartirArchivos = value; 
        }

        public bool EnableNotifications 
        { 
            get => bConfigEmpresaHabilitarNotificaciones; 
            set => bConfigEmpresaHabilitarNotificaciones = value; 
        }

        public bool EnableIntegrations 
        { 
            get => bConfigEmpresaHabilitarIntegraciones; 
            set => bConfigEmpresaHabilitarIntegraciones = value; 
        }

        public bool EnableAnalytics 
        { 
            get => bConfigEmpresaHabilitarAnaliticas; 
            set => bConfigEmpresaHabilitarAnaliticas = value; 
        }

        public DateTime CreatedAt 
        { 
            get => dConfigEmpresaFechaCreacion; 
            set => dConfigEmpresaFechaCreacion = value; 
        }

        public DateTime UpdatedAt 
        { 
            get => dConfigEmpresaFechaActualizacion; 
            set => dConfigEmpresaFechaActualizacion = value; 
        }

        public int ApplicationId 
        { 
            get => nConfigEmpresaAplicacionId; 
            set => nConfigEmpresaAplicacionId = value; 
        }

        public int CompanyId 
        { 
            get => nConfigEmpresaEmpresaId; 
            set => nConfigEmpresaEmpresaId = value; 
        }
    }

    // Alias para compatibilidad hacia atrás
    public class CompanyConfiguration : ConfiguracionEmpresa
    {
    }
}
