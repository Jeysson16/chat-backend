using System;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class ConfiguracionChatDto
    {
        [DataMember(EmitDefaultValue = false)]
        public Guid nEmpresasId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Guid nAplicacionesId { get; set; }

        // Configuraciones de Contactos
        [DataMember(EmitDefaultValue = false)]
        public bool bContactosPermitirSolicitudes { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool bContactosRequiereAprobacion { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string cContactosTipoListado { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public bool bContactosPermiteGlobales { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool bContactosAutoAceptar { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int nContactosLimitePorUsuario { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool bContactosNotificarNuevos { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool bContactosPermitirBusquedaPorEmail { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool bContactosPermitirBusquedaPorTelefono { get; set; }

        // Configuraciones de Chat General
        [DataMember(EmitDefaultValue = false)]
        public bool bChatPermiteChatDirecto { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool bChatPermiteChatGrupal { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int nChatMaximoParticipantesGrupo { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool bChatPermitirMensajesTexto { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool bChatPermitirEnvioArchivos { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int nChatTamanoMaximoArchivoMB { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string cChatTiposArchivosPermitidos { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false)]
        public bool bChatPermitirMensajesVoz { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int nChatDuracionMaximaMensajeVoz { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool bChatPermitirVideollamadas { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int nChatTiempoEliminacionAutomaticaDias { get; set; }

        // Configuraciones de Notificaciones
        [DataMember(EmitDefaultValue = false)]
        public bool bNotificacionesHabilitarPush { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool bNotificacionesHabilitarEmail { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool bNotificacionesHabilitarSonidos { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool bNotificacionesMostrarVistaPrevia { get; set; }

        // Configuraciones de Seguridad
        [DataMember(EmitDefaultValue = false)]
        public bool bSeguridadRequiereAutenticacionDosFactor { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int nSeguridadTiempoExpiracionSesionMinutos { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool bSeguridadPermitirMultiplesDispositivos { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool bSeguridadRequiereVerificacionDispositivoNuevo { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool bSeguridadHabilitarCifradoEndToEnd { get; set; }
    }
}