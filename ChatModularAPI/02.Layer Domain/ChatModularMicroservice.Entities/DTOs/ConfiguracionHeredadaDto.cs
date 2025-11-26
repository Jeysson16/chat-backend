using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class ConfiguracionHeredadaDto
    {
        [DataMember(EmitDefaultValue = false)]
        public int nAplicacionesId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string cAplicacionesNombre { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cAplicacionesCodigo { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cConfiguracionEmpresaClave { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cConfiguracionEmpresaValor { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cConfiguracionEmpresaTipo { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cConfiguracionEmpresaDescripcion { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public bool bConfiguracionEmpresaEsActiva { get; set; }

        // Origen
        [DataMember(EmitDefaultValue = false)]
        public string cOrigenConfiguracion { get; set; } = "APLICACION"; // APLICACION/EMPRESA
    }
}