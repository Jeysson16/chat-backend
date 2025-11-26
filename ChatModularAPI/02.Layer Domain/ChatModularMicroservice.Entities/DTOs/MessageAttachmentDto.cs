using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class MessageAttachmentDto
    {
        [DataMember(EmitDefaultValue = false)]
        public string id { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string name { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string url { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string type { get; set; } = "file";

        [DataMember(EmitDefaultValue = false)]
        public long size { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string? mimeType { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string? path { get; set; }
    }
}

