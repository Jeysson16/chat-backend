using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class ActivoGetCalendarioDTO
    {
        // Modificar el SP, que la fecha sea FechaActual.
        [DataMember(EmitDefaultValue = false)]
        public  string cActivoDescripcion { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cTipoActivoNombre { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cClaseActivoNombre { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cCategoriaActivoNombre { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cCalendarioNombre { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cCalendarioDescripcion { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cConDescripcion { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cCalendarioLink { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public int nEstadoActivoCodigo { get; set; }

    }
}
