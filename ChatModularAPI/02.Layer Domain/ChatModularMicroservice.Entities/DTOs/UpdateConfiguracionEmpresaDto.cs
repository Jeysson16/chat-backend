using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class UpdateConfiguracionEmpresaDto
    {
        [DataMember(EmitDefaultValue = false)]
        [Required(ErrorMessage = "El ID de la configuración es requerido")]
        public int NConfiguracionEmpresaId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [StringLength(500)]
        public string? CConfiguracionEmpresaValor { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [StringLength(50)]
        public string? CConfiguracionEmpresaTipo { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [StringLength(500)]
        public string? CConfiguracionEmpresaDescripcion { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool? BConfiguracionEmpresaEsActiva { get; set; } = true;
    }
}