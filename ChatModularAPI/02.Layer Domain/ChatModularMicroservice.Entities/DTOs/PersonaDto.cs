using System;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class PersonaDto
    {
        [DataMember(EmitDefaultValue = false)]
        public string cPersonasNombre { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cPersonasApellido { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cPersonasEmail { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cPersonasTelefono { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cPersonasAvatar { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cPersonasBiografia { get; set; } = string.Empty;
        
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
        
        [DataMember(EmitDefaultValue = false)]
        public string cPersonasCodigo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cPersonasConfiguracionPrivacidad { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cPersonasConfiguracionNotificaciones { get; set; } = string.Empty;
    }
}