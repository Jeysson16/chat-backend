using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class ConfiguracionAdjuntosDto
    {
        [DataMember(EmitDefaultValue = false)]
        public int? NMaxTamanoArchivo { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string? CTiposArchivosPermitidos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BPermitirAdjuntos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int? NMaxCantidadAdjuntos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BPermitirImagenes { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BPermitirDocumentos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BPermitirVideos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BPermitirAudio { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool? BRequiereAprobacionAdjuntos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string? CExtensionesPermitidas { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int? NTamanoMaximoImagen { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int? NTamanoMaximoVideo { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int? NTamanoMaximoAudio { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int? NTamanoMaximoDocumento { get; set; }
    }
}