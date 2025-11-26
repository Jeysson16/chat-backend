using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class AddParticipantDto
    {
        [DataMember(EmitDefaultValue = false)]
        public long ConversationId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public long UserId { get; set; }
    }
}