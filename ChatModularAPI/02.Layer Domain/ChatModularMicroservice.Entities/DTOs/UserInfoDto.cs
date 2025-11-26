using System;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class UserInfoDto
    {
        [DataMember(EmitDefaultValue = false)]
        public int Id { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string Nombre { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string Email { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string NombreUsuario { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string Rol { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public bool EstaActivo { get; set; } = true;
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime FechaCreacion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime? FechaActualizacion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cPerJurCodigo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cPerCodigo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuarioNombre { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cUsuarioEmail { get; set; } = string.Empty;
    }
}