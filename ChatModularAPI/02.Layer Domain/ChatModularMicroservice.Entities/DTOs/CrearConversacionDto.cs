using System;
using System.Collections.Generic;
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

        // Nuevos campos para soportar múltiples participantes enviados desde el frontend
        [DataMember(EmitDefaultValue = false)]
        public List<string> participante_ids { get; set; } = new();

        [DataMember(EmitDefaultValue = false)]
        public List<string> participantes { get; set; } = new();

        // Campo opcional enviado por el frontend para el creador
        [DataMember(EmitDefaultValue = false)]
        public string cConversacionesChatUsuarioCreadorId { get; set; } = string.Empty;
    }
}
