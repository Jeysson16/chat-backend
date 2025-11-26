using System;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class AuthDto
    {
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosNombre { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosEmail { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosNombreUsuario { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosContrasena { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosConfirmarContrasena { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cPersonasJuridicasCodigo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cPersonasCodigo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public bool bAutenticacionExitosa { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cAutenticacionMensaje { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cTokensToken { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime dTokensFechaExpiracion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosRol { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosContrasenaActual { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosNuevaContrasena { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuariosCodigoUsuario { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public bool bUsuariosRecordarme { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cTokensAcceso { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cTokensSecreto { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cAplicacionesCodigo { get; set; } = string.Empty;
    }
}