using System;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class ChatMensajeDto
    {
        [DataMember(EmitDefaultValue = false)]
        public long nMensajesChatId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public long nMensajesChatConversacionId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string cMensajesChatRemitenteId { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cMensajesChatTexto { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cMensajesChatTipo { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public DateTime dMensajesChatFechaHora { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool bMensajesChatEstaLeido { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string cMensajesChatRemitenteNombre { get; set; } = string.Empty;
    }
}