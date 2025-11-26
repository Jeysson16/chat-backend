using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class BuscarPersonaDto
    {
        [DataMember(EmitDefaultValue = false)]
        public string cBusquedaTermino { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cAplicacionesCodigo { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public int nPaginacionPagina { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int nPaginacionTamanoPagina { get; set; }
    }
}