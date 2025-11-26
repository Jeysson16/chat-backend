using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class CreateConfiguracionEmpresaDto
    {
        [DataMember(EmitDefaultValue = false)]
        [Required(ErrorMessage = "El ID de la empresa es requerido")]
        public int NEmpresasId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [Required(ErrorMessage = "El ID de la aplicación es requerido")]
        public int NAplicacionesId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [Required]
        [StringLength(100)]
        public string CConfiguracionEmpresaClave { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        [Required]
        [StringLength(500)]
        public string CConfiguracionEmpresaValor { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        [Required]
        [StringLength(50)]
        public string CConfiguracionEmpresaTipo { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        [StringLength(500)]
        public string? CConfiguracionEmpresaDescripcion { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool BConfiguracionEmpresaEsActiva { get; set; } = true;
    }
}