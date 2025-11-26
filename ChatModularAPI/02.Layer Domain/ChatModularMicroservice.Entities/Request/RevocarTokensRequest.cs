using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities
{
    [DataContract]
    public class RevocarTokensRequest : BaseRequest
    {
        [DataMember(EmitDefaultValue = false)]
        [Required(ErrorMessage = "El código de aplicación es requerido")]
        public string cCodigoAplicacion { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        [Required(ErrorMessage = "El token de acceso es requerido")]
        public string cTokenAcceso { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public string cMotivoRevocacion { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public bool bRevocarTodos { get; set; } = false;
    }
}