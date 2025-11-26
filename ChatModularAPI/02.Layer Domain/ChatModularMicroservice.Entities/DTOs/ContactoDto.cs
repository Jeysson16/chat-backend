using System;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class ContactoDto
    {
        [DataMember(EmitDefaultValue = false)]
        public string cContactosId { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosId { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cContactoUsuariosId { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cContactosEstado { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime dContactosCreacion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime dContactosActualizacion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nContactosId { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nContactosEmpresaId { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nContactosAplicacionId { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cContactosUsuarioSolicitanteId { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cContactosUsuarioContactoId { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cContactosNombreContacto { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cContactosEmailContacto { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cContactosTelefonoContacto { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cContactosNotasContacto { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public bool bContactosActivo { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime dContactosFechaSolicitud { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime dContactosFechaAceptacion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime dContactosFechaCreacion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime dContactosFechaModificacion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cContactosIdContacto { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cContactosNombre { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cContactosApellido { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cContactosEmail { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cContactosTelefono { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cContactosCargo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cContactosDepartamento { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cContactosOrigen { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cContactosIdExterno { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public bool bContactosSincronizadoConApi { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime dContactosFechaUltimaSincronizacion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bContactosPermitirChat { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bContactosNotificacionesActivas { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cSolicitudId { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cSolicitudUsuarioSolicitanteId { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cSolicitudUsuarioDestinoId { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cSolicitudMensaje { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cSolicitudEstado { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime dSolicitudCreacion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime dSolicitudRespuesta { get; set; }
    }
}