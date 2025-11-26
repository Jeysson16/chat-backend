using System;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class EnviarMensajeDto
    {
        [DataMember(EmitDefaultValue = false)]
        public long nConversacionesChatId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string cMensajesChatTexto { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cMensajesChatTipo { get; set; } = "text";
    }
}