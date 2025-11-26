using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class PrivacySettingsDto
    {
        [DataMember(EmitDefaultValue = false)]
        public bool bPersonasPermitirMensajesDesconocidos { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool bPersonasMostrarEstadoEnLinea { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool bPersonasMostrarUltimaVezVisto { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool bPersonasPermitirConfirmacionesLectura { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool bPersonasPermitirInvitacionesGrupos { get; set; }
    }
}