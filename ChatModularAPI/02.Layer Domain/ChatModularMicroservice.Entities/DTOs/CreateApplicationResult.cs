using ChatModularMicroservice.Entities.Models;
using System;

namespace ChatModularMicroservice.Entities.DTOs
{
    public class CreateApplicationResult
    {
        public int naplicacionesid { get; set; }
        public string caplicacionesnombre { get; set; } = string.Empty;
        public string caplicacionescodigo { get; set; } = string.Empty;
        public string cappregistrostokenacceso { get; set; } = string.Empty;
        public string cappregistrossecretoapp { get; set; } = string.Empty;
        public DateTime daplicacionesfechacreacion { get; set; }
        public int nconfiguracionescreadas { get; set; }
    }
}