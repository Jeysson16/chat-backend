using System;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class AuthResponseDto
    {
        [DataMember(EmitDefaultValue = false)]
        public string Token { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string RefreshToken { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime ExpiresAt { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public UserInfoDto User { get; set; } = new UserInfoDto();
        
        [DataMember(EmitDefaultValue = false)]
        public bool IsSuccess { get; set; } = true;
        
        [DataMember(EmitDefaultValue = false)]
        public bool Success { get; set; } = true;
        
        [DataMember(EmitDefaultValue = false)]
        public string Message { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string AppCode { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string AppName { get; set; } = string.Empty;
    }
}