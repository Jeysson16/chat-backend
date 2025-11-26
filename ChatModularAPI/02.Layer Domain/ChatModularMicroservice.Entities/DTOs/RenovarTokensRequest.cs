using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class RenovarTokensRequest
    {
        [DataMember(EmitDefaultValue = false)]
        [Required(ErrorMessage = "El código de aplicación es requerido")]
        public string cCodigoAplicacion { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        [Required(ErrorMessage = "El token de acceso actual es requerido")]
        public string cTokenAccesoActual { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cTokenSecreto { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        [Range(1, 8760, ErrorMessage = "Las horas de validez deben estar entre 1 y 8760 (1 año)")]
        public int nNuevasHorasValidez { get; set; } = 24;

        [DataMember(EmitDefaultValue = false)]
        public string cMotivoRenovacion { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cUsuarioRenovador { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public bool bMantenerTokenAnterior { get; set; } = false;
    }
}