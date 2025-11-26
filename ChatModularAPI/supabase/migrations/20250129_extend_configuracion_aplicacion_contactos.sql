-- Migración para extender ConfiguracionAplicacion con campos de gestión de contactos
-- Fecha: 2025-01-29
-- Descripción: Agregar campos para modos de gestión de contactos

-- Agregar campos de gestión de contactos a ConfiguracionAplicacion
ALTER TABLE "ConfiguracionAplicacion" 
ADD COLUMN IF NOT EXISTS "cModoGestionContactos" VARCHAR(20) DEFAULT 'LOCAL',
ADD COLUMN IF NOT EXISTS "cUrlApiPersonas" VARCHAR(500),
ADD COLUMN IF NOT EXISTS "bSincronizarContactos" BOOLEAN DEFAULT true,
ADD COLUMN IF NOT EXISTS "nTiempoCacheContactos" INTEGER DEFAULT 300;

-- Agregar comentarios para documentación
COMMENT ON COLUMN "ConfiguracionAplicacion"."cModoGestionContactos" IS 'Modo de gestión de contactos: LOCAL, API_EXTERNA, HIBRIDO';
COMMENT ON COLUMN "ConfiguracionAplicacion"."cUrlApiPersonas" IS 'URL de la API externa para gestión de personas/contactos';
COMMENT ON COLUMN "ConfiguracionAplicacion"."bSincronizarContactos" IS 'Indica si se deben sincronizar los contactos automáticamente';
COMMENT ON COLUMN "ConfiguracionAplicacion"."nTiempoCacheContactos" IS 'Tiempo en segundos para mantener en caché los contactos (60-3600)';

-- Agregar restricciones
ALTER TABLE "ConfiguracionAplicacion" 
ADD CONSTRAINT "chk_modo_gestion_contactos" 
CHECK ("cModoGestionContactos" IN ('LOCAL', 'API_EXTERNA', 'HIBRIDO'));

ALTER TABLE "ConfiguracionAplicacion" 
ADD CONSTRAINT "chk_tiempo_cache_contactos" 
CHECK ("nTiempoCacheContactos" >= 60 AND "nTiempoCacheContactos" <= 3600);

-- Actualizar registros existentes con valores por defecto
UPDATE "ConfiguracionAplicacion" 
SET 
    "cModoGestionContactos" = 'LOCAL',
    "bSincronizarContactos" = true,
    "nTiempoCacheContactos" = 300
WHERE "cModoGestionContactos" IS NULL;