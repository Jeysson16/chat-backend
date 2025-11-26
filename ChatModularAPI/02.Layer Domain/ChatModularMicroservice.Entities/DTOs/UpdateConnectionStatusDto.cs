using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class UpdateConnectionStatusDto
    {
        [DataMember(EmitDefaultValue = false)]
        public string Estado { get; set; } = string.Empty; // "online" | "offline"
    }
}