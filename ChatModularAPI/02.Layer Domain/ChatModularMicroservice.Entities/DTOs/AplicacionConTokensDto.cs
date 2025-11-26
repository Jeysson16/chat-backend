using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class AplicacionConTokensDto
    {
        [DataMember(EmitDefaultValue = false)]
        public ApplicationDto Aplicacion { get; set; } = new ApplicationDto();

        [DataMember(EmitDefaultValue = false)]
        public TokensAplicacionDto Tokens { get; set; } = new TokensAplicacionDto();
    }
}