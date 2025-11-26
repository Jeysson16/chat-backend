using System;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class UsuarioDto
    {
        [DataMember(EmitDefaultValue = false)]
        public int nUsuariosId { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosNombre { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosEmail { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosTelefono { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public bool bUsuariosActivo { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime dUsuariosFechaCreacion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime dUsuariosFechaActualizacion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosPassword { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosChatEmail { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosChatId { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosChatPassword { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosChatUsername { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public bool bUsuariosChatEstaActivo { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bUsuariosEstaEnLinea { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosConfiguracionNotificaciones { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosConfiguracionPrivacidad { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosChatAvatar { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public bool bUsuariosVerificado { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosChatRol { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosChatNombre { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosTerminoBusqueda { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public int nUsuariosAplicacionId { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nUsuariosEmpresaId { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nUsuariosPagina { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nUsuariosTamanoPagina { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime dUsuariosUltimaConexion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nUsuariosTotalConversaciones { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nUsuariosMensajesEnviados { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nUsuariosMensajesRecibidos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nUsuariosTotalUsuarios { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nUsuariosUsuariosActivos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nUsuariosUsuariosInactivos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nUsuariosUsuariosNuevosHoy { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nUsuariosUsuariosNuevosEsteMes { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosPasswordActual { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosPasswordNueva { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosConfirmarPassword { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosToken { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosNuevaPassword { get; set; } = string.Empty;
    }
}