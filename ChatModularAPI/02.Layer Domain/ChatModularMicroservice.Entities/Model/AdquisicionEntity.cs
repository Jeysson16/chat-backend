using System;

namespace ChatModularMicroservice.Entities.Model
{
    public class AdquisicionEntity
    {
        public int nAdquisicionCodigo { get; set; }
        public string cAdquisicionNombre { get; set; } = string.Empty;
        public string cAdquisicionDescripcion { get; set; } = string.Empty;
        public DateTime dAdquisicionFechaCreacion { get; set; } = DateTime.UtcNow;
        public bool bAdquisicionEstado { get; set; } = true;

        public AdquisicionEntity() { }
    }
}