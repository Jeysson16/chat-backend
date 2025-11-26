-- Migración para crear tablas de gestión de contactos y solicitudes de amistad
-- Fecha: 2025-01-29
-- Descripción: Crear ChatContacto y ChatSolicitudAmistad

-- Crear tabla ChatContacto para gestión local de contactos
CREATE TABLE IF NOT EXISTS public.ChatContacto (
    nContactoId UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nAplicacionesId INTEGER NOT NULL,
    cUsuarioId VARCHAR(255) NOT NULL,
    cContactoUsuarioId VARCHAR(255) NOT NULL,
    cNombreContacto VARCHAR(255),
    cEmailContacto VARCHAR(255),
    cTelefonoContacto VARCHAR(50),
    cAvatarContacto VARCHAR(500),
    cEstadoContacto VARCHAR(20) DEFAULT 'Activo' CHECK (cEstadoContacto IN ('Activo', 'Bloqueado', 'Eliminado')),
    bEsFavorito BOOLEAN DEFAULT false,
    cNotasContacto TEXT,
    dFechaAgregado TIMESTAMPTZ DEFAULT now(),
    dFechaUltimaInteraccion TIMESTAMPTZ,
    dFechaCreacion TIMESTAMPTZ DEFAULT now(),
    dFechaModificacion TIMESTAMPTZ DEFAULT now(),
    bEsActivo BOOLEAN DEFAULT true,
    
    -- Constraints
    CONSTRAINT unique_user_contact UNIQUE (nAplicacionesId, cUsuarioId, cContactoUsuarioId),
    CONSTRAINT check_different_users CHECK (cUsuarioId != cContactoUsuarioId)
);

-- Agregar foreign keys después de crear la tabla
ALTER TABLE public.ChatContacto 
ADD CONSTRAINT fk_chatcontacto_aplicacion 
FOREIGN KEY (nAplicacionesId) REFERENCES public."Aplicaciones"("nAplicacionesId");

ALTER TABLE public.ChatContacto 
ADD CONSTRAINT fk_chatcontacto_usuario 
FOREIGN KEY (cUsuarioId) REFERENCES public."Usuarios"("nUsuariosId");

ALTER TABLE public.ChatContacto 
ADD CONSTRAINT fk_chatcontacto_contacto_usuario 
FOREIGN KEY (cContactoUsuarioId) REFERENCES public."Usuarios"("nUsuariosId");

-- Tabla para solicitudes de amistad
CREATE TABLE IF NOT EXISTS "ChatSolicitudAmistad" (
    "nChatSolicitudAmistadId" SERIAL PRIMARY KEY,
    "nAplicacionesId" INTEGER NOT NULL,
    "nUsuarioSolicitanteId" INTEGER NOT NULL,
    "nUsuarioDestinatarioId" INTEGER NOT NULL,
    "cEstadoSolicitud" VARCHAR(20) DEFAULT 'PENDIENTE', -- PENDIENTE, ACEPTADA, RECHAZADA, CANCELADA
    "cMensajeSolicitud" TEXT,
    "dFechaSolicitud" TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    "dFechaRespuesta" TIMESTAMP WITH TIME ZONE,
    "cMotivoRechazo" VARCHAR(500),
    "bEsActiva" BOOLEAN DEFAULT true,
    
    -- Claves foráneas
    CONSTRAINT "fk_solicitud_aplicacion" 
        FOREIGN KEY ("nAplicacionesId") 
        REFERENCES "Aplicaciones"("nAplicacionesId") 
        ON DELETE CASCADE,
    
    CONSTRAINT "fk_solicitud_usuario_solicitante" 
        FOREIGN KEY ("nUsuarioSolicitanteId") 
        REFERENCES "Usuarios"("nUsuariosId") 
        ON DELETE CASCADE,
    
    CONSTRAINT "fk_solicitud_usuario_destinatario" 
        FOREIGN KEY ("nUsuarioDestinatarioId") 
        REFERENCES "Usuarios"("nUsuariosId") 
        ON DELETE CASCADE,
    
    -- Restricciones
    CONSTRAINT "chk_solicitud_estado" 
        CHECK ("cEstadoSolicitud" IN ('PENDIENTE', 'ACEPTADA', 'RECHAZADA', 'CANCELADA')),
    
    CONSTRAINT "chk_usuarios_diferentes" 
        CHECK ("nUsuarioSolicitanteId" != "nUsuarioDestinatarioId"),
    
    -- Índices únicos para evitar solicitudes duplicadas
    CONSTRAINT "uk_solicitud_usuarios_app" 
        UNIQUE ("nAplicacionesId", "nUsuarioSolicitanteId", "nUsuarioDestinatarioId")
);

-- Índices para optimizar consultas
CREATE INDEX IF NOT EXISTS "idx_chat_contacto_aplicacion" ON "ChatContacto"("nAplicacionesId");
CREATE INDEX IF NOT EXISTS "idx_chat_contacto_estado" ON "ChatContacto"("cContactoEstado");
CREATE INDEX IF NOT EXISTS "idx_chat_contacto_favorito" ON "ChatContacto"("bEsFavorito");
CREATE INDEX IF NOT EXISTS "idx_chat_contacto_email" ON "ChatContacto"("cContactoEmail");

CREATE INDEX IF NOT EXISTS "idx_solicitud_aplicacion" ON "ChatSolicitudAmistad"("nAplicacionesId");
CREATE INDEX IF NOT EXISTS "idx_solicitud_solicitante" ON "ChatSolicitudAmistad"("nUsuarioSolicitanteId");
CREATE INDEX IF NOT EXISTS "idx_solicitud_destinatario" ON "ChatSolicitudAmistad"("nUsuarioDestinatarioId");
CREATE INDEX IF NOT EXISTS "idx_solicitud_estado" ON "ChatSolicitudAmistad"("cEstadoSolicitud");
CREATE INDEX IF NOT EXISTS "idx_solicitud_fecha" ON "ChatSolicitudAmistad"("dFechaSolicitud");

-- Triggers para actualizar fecha de modificación
CREATE OR REPLACE FUNCTION update_chat_contacto_timestamp()
RETURNS TRIGGER AS $$
BEGIN
    NEW."dFechaActualizacion" = NOW();
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER "tr_chat_contacto_update_timestamp"
    BEFORE UPDATE ON "ChatContacto"
    FOR EACH ROW
    EXECUTE FUNCTION update_chat_contacto_timestamp();

-- Comentarios para documentación
COMMENT ON TABLE "ChatContacto" IS 'Tabla para gestión local de contactos por aplicación';
COMMENT ON TABLE "ChatSolicitudAmistad" IS 'Tabla para gestión de solicitudes de amistad entre usuarios';

COMMENT ON COLUMN "ChatContacto"."cContactoEstado" IS 'Estado del contacto: ACTIVO, BLOQUEADO, ELIMINADO';
COMMENT ON COLUMN "ChatContacto"."cGrupos" IS 'Array JSON de grupos a los que pertenece el contacto';
COMMENT ON COLUMN "ChatContacto"."cMetadatos" IS 'JSON con metadatos adicionales del contacto';

COMMENT ON COLUMN "ChatSolicitudAmistad"."cEstadoSolicitud" IS 'Estado de la solicitud: PENDIENTE, ACEPTADA, RECHAZADA, CANCELADA';
COMMENT ON COLUMN "ChatSolicitudAmistad"."cMensajeSolicitud" IS 'Mensaje opcional que acompaña la solicitud';
COMMENT ON COLUMN "ChatSolicitudAmistad"."cMotivoRechazo" IS 'Motivo opcional del rechazo de la solicitud';