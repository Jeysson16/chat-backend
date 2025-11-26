using System;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class SolicitudContactoDto
    {
        [DataMember(EmitDefaultValue = false)]
        public int nSolicitudId { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuarioSolicitanteId { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuarioDestinoId { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cMensajeSolicitud { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cEstadoSolicitud { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime dFechaSolicitud { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime? dFechaRespuesta { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nEmpresaId { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nAplicacionId { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuarioSolicitanteNombre { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuarioSolicitanteEmail { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuarioSolicitanteAvatar { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuarioDestinoNombre { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuarioDestinoEmail { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuarioDestinoAvatar { get; set; } = string.Empty;
    }
}