using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class UpdateEstadoConexionDto
    {
        [DataMember(EmitDefaultValue = false)]
        [Required]
        public bool EstaEnLinea { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime? FechaConexion { get; set; }
    }
}