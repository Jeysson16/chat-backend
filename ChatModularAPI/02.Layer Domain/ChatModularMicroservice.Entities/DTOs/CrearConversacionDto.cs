using System;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class CrearConversacionDto
    {
        [DataMember(EmitDefaultValue = false)]
        public string cAppCodigo { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string Nombre { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cConversacionesChatNombre { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cConversacionesChatTipo { get; set; } = "direct";

        [DataMember(EmitDefaultValue = false)]
        public string? cConversacionesChatDescripcion { get; set; }
    }
}