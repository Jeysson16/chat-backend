using System;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class JwtConfig
    {
        [DataMember(EmitDefaultValue = false)]
        public string cJwtSecreto { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cJwtEmisor { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cJwtAudiencia { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public int nJwtExpiracionMinutos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nJwtRefreshTokenExpiracionDias { get; set; }
    }
}