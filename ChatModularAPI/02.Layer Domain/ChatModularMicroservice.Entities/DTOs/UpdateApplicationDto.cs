using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class UpdateApplicationDto
    {
        [DataMember(EmitDefaultValue = false)]
        [Required(ErrorMessage = "El ID de la aplicación es requerido")]
        public int nAplicacionesId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string? cAplicacionesNombre { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string? cAplicacionesDescripcion { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [StringLength(50, ErrorMessage = "El código no puede exceder 50 caracteres")]
        public string? cAplicacionesCodigo { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool? bAplicacionesEsActiva { get; set; }
    }
}