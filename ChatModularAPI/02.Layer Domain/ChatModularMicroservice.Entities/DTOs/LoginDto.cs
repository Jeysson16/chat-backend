using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// DTO para el login de usuario (nomenclaturas en español)
    /// </summary>
    [DataContract]
    public class LoginDto
    {
        /// <summary>
        /// Código del usuario
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        [Required(ErrorMessage = "El código de usuario es requerido")]
        public string cUsuarioCodigo { get; set; } = string.Empty;

        /// <summary>
        /// Contraseña del usuario
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        [Required(ErrorMessage = "La contraseña es requerida")]
        public string cUsuarioContra { get; set; } = string.Empty;

        /// <summary>
        /// Recordar sesión
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool bRecordarme { get; set; } = false;

        /// <summary>
        /// Token de acceso de la aplicación (opcional)
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string? cTokenAcceso { get; set; }

        /// <summary>
        /// Token secreto de la aplicación (opcional)
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string? cTokenSecreto { get; set; }

        /// <summary>
        /// Código de empresa (cPerJurCodigo) opcional para validar pertenencia
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string? cPerJurCodigo { get; set; }

        // Alias de compatibilidad hacia atrás (no se serializan)
        [JsonIgnore]
        [IgnoreDataMember]
        public string UserCode { get => cUsuarioCodigo; set => cUsuarioCodigo = value; }

        [JsonIgnore]
        [IgnoreDataMember]
        public string Password { get => cUsuarioContra; set => cUsuarioContra = value; }

        [JsonIgnore]
        [IgnoreDataMember]
        public bool RememberMe { get => bRecordarme; set => bRecordarme = value; }

        [JsonIgnore]
        [IgnoreDataMember]
        public string? AccessToken { get => cTokenAcceso; set => cTokenAcceso = value; }

        [JsonIgnore]
        [IgnoreDataMember]
        public string? SecretToken { get => cTokenSecreto; set => cTokenSecreto = value; }

        [JsonIgnore]
        [IgnoreDataMember]
        public string? PerJurCodigo { get => cPerJurCodigo; set => cPerJurCodigo = value; }
    }
}