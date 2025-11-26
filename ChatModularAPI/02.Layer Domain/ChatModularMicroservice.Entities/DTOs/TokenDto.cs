using System;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class TokenDto
    {
        [DataMember(EmitDefaultValue = false)]
        public string cAplicacionesCodigo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cAppRegistrosTokenAcceso { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cTokenSecreto { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public bool bTokenEsValido { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cValidacionMensaje { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime dTokenFechaExpiracion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nTokenDiasRestantes { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cAppRegistrosTokenActualizacion { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime dAppRegistrosFechaExpiracion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bTokenEsActivo { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime dTokenFechaCreacion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cTokenUsuarioCreador { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cTokenAccesoActual { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public int nTokenHorasValidez { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cAplicacionesNombre { get; set; } = string.Empty;
    }
}