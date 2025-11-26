using System;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class BuscarUsuarioDto
    {
        [DataMember(EmitDefaultValue = false)]
        public string? cTerminoBusqueda { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? nEmpresasId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? nAplicacionesId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string? cUsuariosEstado { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool? bUsuariosEsActivo { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? nPagina { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? nTamanoPagina { get; set; }
    }
}