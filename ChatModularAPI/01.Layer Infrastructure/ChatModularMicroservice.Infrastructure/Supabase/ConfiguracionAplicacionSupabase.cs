using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ChatModularMicroservice.Domain
{
    [Table("ConfiguracionAplicacion")]
    public class ConfiguracionAplicacionSupabase : BaseModel
    {
        [PrimaryKey("nConfiguracionAplicacionId")]
        [Column("nConfiguracionAplicacionId")]
        public int nConfiguracionAplicacionId { get; set; }

        [Column("nAplicacionesId")]
        public int nAplicacionesId { get; set; }

        [Column("nMaxTamanoArchivo")]
        public int? nMaxTamanoArchivo { get; set; }

        [Column("cTiposArchivosPermitidos")]
        public string? cTiposArchivosPermitidos { get; set; }

        [Column("bPermitirAdjuntos")]
        public bool? bPermitirAdjuntos { get; set; }

        [Column("nMaxCantidadAdjuntos")]
        public int? nMaxCantidadAdjuntos { get; set; }

        [Column("bPermitirVisualizacionAdjuntos")]
        public bool? bPermitirVisualizacionAdjuntos { get; set; }

        [Column("nMaxLongitudMensaje")]
        public int? nMaxLongitudMensaje { get; set; }

        [Column("bPermitirEmojis")]
        public bool? bPermitirEmojis { get; set; }

        [Column("bPermitirMensajesVoz")]
        public bool? bPermitirMensajesVoz { get; set; }

        [Column("bPermitirNotificaciones")]
        public bool? bPermitirNotificaciones { get; set; }

        [Column("bRequiereAutenticacion")]
        public bool? bRequiereAutenticacion { get; set; }

        [Column("bPermitirMensajesAnonimos")]
        public bool? bPermitirMensajesAnonimos { get; set; }

        [Column("nTiempoExpiracionSesion")]
        public int? nTiempoExpiracionSesion { get; set; }
    }
}
