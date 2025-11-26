using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class CreateEmpresaDto
    {
        [DataMember(EmitDefaultValue = false)]
        [Required(ErrorMessage = "El ID de aplicación es requerido")]
        public int nEmpresasAplicacionId { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        [Required(ErrorMessage = "El nombre de la empresa es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string cEmpresasNombre { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        [Required(ErrorMessage = "El código de la empresa es requerido")]
        [StringLength(20, ErrorMessage = "El código no puede exceder 20 caracteres")]
        public string cEmpresasCodigo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        [StringLength(100, ErrorMessage = "El dominio no puede exceder 100 caracteres")]
        public string cEmpresasDominio { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string cEmpresasDescripcion { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public bool bEmpresasActiva { get; set; } = true;
    }
}