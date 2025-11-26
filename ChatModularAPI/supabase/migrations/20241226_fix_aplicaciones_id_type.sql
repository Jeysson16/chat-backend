-- Migration: Fix Aplicaciones table ID type from UUID to INTEGER
-- Date: 2024-12-26
-- Description: Changes nAplicacionesId from UUID to INTEGER to match stored procedures

-- First, drop all foreign key constraints that reference Aplicaciones
ALTER TABLE IF EXISTS "Empresas" DROP CONSTRAINT IF EXISTS "empresas_naplicacionesaplicacionid_fkey";
ALTER TABLE IF EXISTS "Usuarios" DROP CONSTRAINT IF EXISTS "usuarios_naplicacionesaplicacionid_fkey";
ALTER TABLE IF EXISTS "Contactos" DROP CONSTRAINT IF EXISTS "contactos_naplicacionesaplicacionid_fkey";
ALTER TABLE IF EXISTS "ConfiguracionEmpresa" DROP CONSTRAINT IF EXISTS "configuracionempresa_nconfiguracionempresaaplicacionid_fkey";
ALTER TABLE IF EXISTS "ConfiguracionAplicacion" DROP CONSTRAINT IF EXISTS "configuracionaplicacion_nconfiguracionaplicacionaplicacionid_fkey";
ALTER TABLE IF EXISTS "AppRegistros" DROP CONSTRAINT IF EXISTS "appregistros_nappregistrosaplicacionid_fkey";
ALTER TABLE IF EXISTS "TokenRegistros" DROP CONSTRAINT IF EXISTS "tokenregistros_ntokenregistrosaplicacionid_fkey";
ALTER TABLE IF EXISTS "WebhookRegistros" DROP CONSTRAINT IF EXISTS "webhookregistros_nwebhookregistrosaplicacionid_fkey";

-- Drop the existing table and recreate with INTEGER ID
DROP TABLE IF EXISTS "Aplicaciones" CASCADE;

-- Create Aplicaciones table with INTEGER ID
CREATE TABLE "Aplicaciones" (
    "nAplicacionesId" SERIAL PRIMARY KEY,
    "cAplicacionesNombre" VARCHAR(100) NOT NULL,
    "cAplicacionesDescripcion" VARCHAR(500),
    "cAplicacionesCodigo" VARCHAR(50) UNIQUE NOT NULL,
    "dAplicacionesFechaCreacion" TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    "bAplicacionesEsActiva" BOOLEAN DEFAULT TRUE
);

-- Add indexes
CREATE INDEX "idx_Aplicaciones_Codigo" ON "Aplicaciones"("cAplicacionesCodigo");
CREATE INDEX "idx_Aplicaciones_Activa" ON "Aplicaciones"("bAplicacionesEsActiva");

-- Add comments
COMMENT ON TABLE "Aplicaciones" IS 'Tabla de aplicaciones del sistema';
COMMENT ON COLUMN "Aplicaciones"."nAplicacionesId" IS 'ID único de la aplicación (INTEGER)';
COMMENT ON COLUMN "Aplicaciones"."cAplicacionesNombre" IS 'Nombre de la aplicación';
COMMENT ON COLUMN "Aplicaciones"."cAplicacionesDescripcion" IS 'Descripción de la aplicación';
COMMENT ON COLUMN "Aplicaciones"."cAplicacionesCodigo" IS 'Código único de la aplicación';
COMMENT ON COLUMN "Aplicaciones"."dAplicacionesFechaCreacion" IS 'Fecha de creación de la aplicación';
COMMENT ON COLUMN "Aplicaciones"."bAplicacionesEsActiva" IS 'Indica si la aplicación está activa';

-- Insert test application
INSERT INTO "Aplicaciones" (
    "cAplicacionesNombre",
    "cAplicacionesDescripcion", 
    "cAplicacionesCodigo",
    "bAplicacionesEsActiva"
) VALUES (
    'Chat Modular Test',
    'Aplicación de prueba para el sistema de chat modular',
    'CHAT_TEST',
    true
) ON CONFLICT ("cAplicacionesCodigo") DO NOTHING;

-- Recreate foreign key constraints with correct INTEGER type
-- Note: Other tables will need to be updated to use INTEGER instead of UUID for foreign keys