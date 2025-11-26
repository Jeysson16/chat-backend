using System;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class AdquisicionInsertDTO
    {
        [DataMember(EmitDefaultValue = false)]
        public int nAdquisicionCodigo { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string cAdquisicionNombre { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cAdquisicionDescripcion { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public DateTime dAdquisicionFechaCreacion { get; set; } = DateTime.UtcNow;

        [DataMember(EmitDefaultValue = false)]
        public bool bAdquisicionEstado { get; set; } = true;
    }
}