using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class GenerarTokensRequest
    {
        [DataMember(EmitDefaultValue = false)]
        [Required(ErrorMessage = "El código de aplicación es requerido")]
        public string cCodigoAplicacion { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cNombreAplicacion { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        [Range(1, 8760, ErrorMessage = "Las horas de validez deben estar entre 1 y 8760 (1 año)")]
        public int nHorasValidez { get; set; } = 24;

        [DataMember(EmitDefaultValue = false)]
        public string cDescripcion { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cUsuarioCreador { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public bool bEsProduccion { get; set; } = false;

        [DataMember(EmitDefaultValue = false)]
        public string cPermisos { get; set; } = string.Empty;
    }
}