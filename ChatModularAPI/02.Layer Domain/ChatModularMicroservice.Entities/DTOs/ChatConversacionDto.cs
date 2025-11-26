using System;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class ChatConversacionDto
    {
        [DataMember(EmitDefaultValue = false)]
        public long nConversacionesChatId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string cConversacionesChatAppCodigo { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cConversacionesChatNombre { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string? cConversacionesChatDescripcion { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string cConversacionesChatTipo { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cConversacionesChatUsuarioCreadorId { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public DateTime dConversacionesChatFechaCreacion { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? dConversacionesChatUltimaActividad { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool bConversacionesChatEstaActiva { get; set; }
    }
}