using System;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class ChatDto
    {
        [DataMember(EmitDefaultValue = false)]
        public long nMensajesId { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public long nMensajesConversacionId { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cMensajesRemitenteId { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cMensajesRemitenteNombre { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cMensajesTexto { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cMensajesTipo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime dMensajesFechaHora { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bMensajesEstaLeido { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public long nConversacionesId { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cConversacionesAplicacionCodigo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cConversacionesNombre { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cConversacionesTipo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime dConversacionesFechaCreacion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bConversacionesEstaActiva { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosId { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosPersonaJuridicaCodigo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosPersonaCodigo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosNombre { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosEmail { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime dUsuariosUltimaConexion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bUsuariosEstaActivo { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bUsuariosEstaEnLinea { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosRol { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosAvatar { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime dUsuariosFechaCreacion { get; set; }
    }
}