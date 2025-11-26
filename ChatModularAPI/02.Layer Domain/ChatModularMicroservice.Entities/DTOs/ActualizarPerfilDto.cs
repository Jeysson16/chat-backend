using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class ActualizarPerfilDto
    {
        [DataMember(EmitDefaultValue = false)]
        public string cPersonasNombre { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cPersonasApellido { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cPersonasTelefono { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cPersonasAvatar { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cPersonasBiografia { get; set; } = string.Empty;
    }
}