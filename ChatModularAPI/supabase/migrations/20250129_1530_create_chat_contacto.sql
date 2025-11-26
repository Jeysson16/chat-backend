-- Migración para crear tabla ChatContacto
-- Fecha: 2025-01-29 15:30
-- Descripción: Crear tabla ChatContacto para gestión local de contactos

-- Crear tabla ChatContacto para gestión local de contactos
CREATE TABLE IF NOT EXISTS public."ChatContacto" (
    "nContactoId" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "nAplicacionesId" INTEGER NOT NULL,
    "cUsuarioId" VARCHAR(255) NOT NULL,
    "cContactoUsuarioId" VARCHAR(255) NOT NULL,
    "cNombreContacto" VARCHAR(255),
    "cEmailContacto" VARCHAR(255),
    "cTelefonoContacto" VARCHAR(50),
    "cAvatarContacto" VARCHAR(500),
    "cEstadoContacto" VARCHAR(20) DEFAULT 'Activo',
    "bEsFavorito" BOOLEAN DEFAULT false,
    "cNotasContacto" TEXT,
    "dFechaAgregado" TIMESTAMPTZ DEFAULT now(),
    "dFechaUltimaInteraccion" TIMESTAMPTZ,
    "dFechaCreacion" TIMESTAMPTZ DEFAULT now(),
    "dFechaModificacion" TIMESTAMPTZ DEFAULT now(),
    "bEsActivo" BOOLEAN DEFAULT true,
    
    -- Constraints
    CONSTRAINT "unique_user_contact" UNIQUE ("nAplicacionesId", "cUsuarioId", "cContactoUsuarioId"),
    CONSTRAINT "check_different_users" CHECK ("cUsuarioId" != "cContactoUsuarioId"),
    CONSTRAINT "check_estado_contacto" CHECK ("cEstadoContacto" IN ('Activo', 'Bloqueado', 'Eliminado'))
);

-- Agregar foreign keys
ALTER TABLE public."ChatContacto" 
ADD CONSTRAINT "fk_chatcontacto_aplicacion" 
FOREIGN KEY ("nAplicacionesId") REFERENCES public."Aplicaciones"("nAplicacionesId");

ALTER TABLE public."ChatContacto" 
ADD CONSTRAINT "fk_chatcontacto_usuario" 
FOREIGN KEY ("cUsuarioId") REFERENCES public."Usuarios"("nUsuariosId");

ALTER TABLE public."ChatContacto" 
ADD CONSTRAINT "fk_chatcontacto_contacto_usuario" 
FOREIGN KEY ("cContactoUsuarioId") REFERENCES public."Usuarios"("nUsuariosId");

-- Índices para optimizar consultas
CREATE INDEX IF NOT EXISTS "idx_chat_contacto_aplicacion" ON public."ChatContacto"("nAplicacionesId");
CREATE INDEX IF NOT EXISTS "idx_chat_contacto_usuario" ON public."ChatContacto"("cUsuarioId");
CREATE INDEX IF NOT EXISTS "idx_chat_contacto_estado" ON public."ChatContacto"("cEstadoContacto");
CREATE INDEX IF NOT EXISTS "idx_chat_contacto_favorito" ON public."ChatContacto"("bEsFavorito");

-- Trigger para actualizar fecha de modificación
CREATE OR REPLACE FUNCTION update_chat_contacto_timestamp()
RETURNS TRIGGER AS $$
BEGIN
    NEW."dFechaModificacion" = NOW();
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER "tr_chat_contacto_update_timestamp"
    BEFORE UPDATE ON public."ChatContacto"
    FOR EACH ROW
    EXECUTE FUNCTION update_chat_contacto_timestamp();

-- Comentarios para documentación
COMMENT ON TABLE public."ChatContacto" IS 'Tabla para gestión local de contactos por aplicación';
COMMENT ON COLUMN public."ChatContacto"."cEstadoContacto" IS 'Estado del contacto: Activo, Bloqueado, Eliminado'