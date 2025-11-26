using System;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class ChatUsuarioDto
    {
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosChatId { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosChatNombre { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosChatEmail { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosChatAvatar { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public bool bUsuariosChatEstaActivo { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bUsuariosChatEstaEnLinea { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime dUsuariosChatFechaCreacion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime? dUsuariosChatUltimaVez { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosChatTelefono { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosChatEstado { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public int nEmpresaId { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nAplicacionId { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bHasExistingConversation { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int? nExistingConversationId { get; set; }
    }
}