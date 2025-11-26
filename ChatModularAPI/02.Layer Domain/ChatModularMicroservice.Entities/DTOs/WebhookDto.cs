using System;
using System.Runtime.Serialization;

namespace ChatModularMicroservice.Entities.DTOs
{
    [DataContract]
    public class WebhookDto
    {
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookCodigoAplicacion { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookUrl { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookTokenSecreto { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public bool bWebhookHabilitado { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookEventosHabilitados { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public int nWebhookTimeoutSegundos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nWebhookMaxReintentos { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bWebhookValidarCertificadoSSL { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookHeadersPersonalizados { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public bool bWebhookExitoso { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookMensajeError { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public int nWebhookCodigoRespuesta { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookRespuestaServidor { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime dWebhookFechaEnvio { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public int nWebhookIntentosRealizados { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookTiempoRespuesta { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookTipoEvento { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime dWebhookFechaEvento { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookEventoId { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookMetadatosAdicionales { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookPerCodigo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookParticipantePerCodigo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookParticipantePerJurCodigo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public int nWebhookConversacionId { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookTipoConversacion { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookMensajeInicial { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookRemitentePerCodigo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookRemitentePerJurCodigo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookMensajeId { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookContenidoMensaje { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookTipoMensaje { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime dWebhookFechaMensaje { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookEstadoAnterior { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookEstadoNuevo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public DateTime dWebhookUltimaConexion { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public bool bWebhookEstaEnLinea { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookContactoPerCodigo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookContactoPerJurCodigo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public int nWebhookContactoId { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookContactoNombre { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookContactoEmail { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookContactoBloqueadoPerCodigo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookContactoBloqueadoPerJurCodigo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookMotivoBloqueo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookAccionAnterior { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookContactoDesbloqueadoPerCodigo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookContactoDesbloqueadoPerJurCodigo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookDestinatarioPerCodigo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookDestinatarioPerJurCodigo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookMensaje { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public int nWebhookSolicitudId { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookSolicitantePerCodigo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookSolicitantePerJurCodigo { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookRespuesta { get; set; } = string.Empty;
        
        [DataMember(EmitDefaultValue = false)]
        public string cWebhookMotivoRechazo { get; set; } = string.Empty;
    }
}