using System;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class UsuarioEstadisticasDto
    {
        [DataMember(EmitDefaultValue = false)]
        public Guid UsuarioId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int TotalMensajes { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int TotalContactos { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? UltimaConexion { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int ConversacionesActivas { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int ConversacionesArchivadas { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int TokensActivos { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int TokensRevocados { get; set; }
    }
}