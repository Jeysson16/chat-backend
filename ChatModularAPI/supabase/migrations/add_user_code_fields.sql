-- Agregar campos de código de usuario a la tabla Usuarios
-- Fecha: 2024-12-25
-- Descripción: Agrega campos cUsuariosPerCodigo y cUsuariosPerJurCodigo para autenticación por código

-- Agregar campos si no existen
DO $$
BEGIN
    -- Agregar cUsuariosPerCodigo si no existe
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_name = 'Usuarios' AND column_name = 'cUsuariosPerCodigo') THEN
        ALTER TABLE "Usuarios" ADD COLUMN "cUsuariosPerCodigo" VARCHAR(100);
    END IF;
    
    -- Agregar cUsuariosPerJurCodigo si no existe
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_name = 'Usuarios' AND column_name = 'cUsuariosPerJurCodigo') THEN
        ALTER TABLE "Usuarios" ADD COLUMN "cUsuariosPerJurCodigo" VARCHAR(100);
    END IF;
    
    -- Agregar campo de contraseña si no existe
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_name = 'Usuarios' AND column_name = 'cUsuariosPassword') THEN
        ALTER TABLE "Usuarios" ADD COLUMN "cUsuariosPassword" VARCHAR(255);
    END IF;
    
    -- Agregar campo activo si no existe
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_name = 'Usuarios' AND column_name = 'bUsuariosActivo') THEN
        ALTER TABLE "Usuarios" ADD COLUMN "bUsuariosActivo" BOOLEAN DEFAULT TRUE;
    END IF;
END $$;

-- Crear índices para mejorar el rendimiento de búsquedas por código
CREATE INDEX IF NOT EXISTS idx_usuarios_per_codigo ON "Usuarios"("cUsuariosPerCodigo");
CREATE INDEX IF NOT EXISTS idx_usuarios_per_jur_codigo ON "Usuarios"("cUsuariosPerJurCodigo");

-- Insertar usuario de prueba JESANCHEZR
INSERT INTO "Usuarios" (
    "nUsuariosId",
    "cUsuariosNombre", 
    "cUsuariosEmail",
    "cUsuariosPerCodigo",
    "cUsuariosPerJurCodigo",
    "cUsuariosPassword",
    "bUsuariosEstaEnLinea",
    "bUsuariosActivo",
    "dUsuariosFechaCreacion"
) VALUES (
    'jesanchezr-001',
    'Jeysson Sanchez',
    'jesanchezr@chatmodular.com',
    'JESANCHEZR',
    'DEFAULT',
    '$2a$11$8K1p/a0dLPMWuLiMpGX9Ou7BQGb2Z8VWvUVFh6A8LRF4YRPQ8/jS2', -- Hash de "Jeysson12345"
    false,
    true,
    NOW()
) ON CONFLICT ("nUsuariosId") DO UPDATE SET
    "cUsuariosPerCodigo" = EXCLUDED."cUsuariosPerCodigo",
    "cUsuariosPerJurCodigo" = EXCLUDED."cUsuariosPerJurCodigo",
    "cUsuariosPassword" = EXCLUDED."cUsuariosPassword",
    "bUsuariosActivo" = EXCLUDED."bUsuariosActivo";

-- Verificar que el usuario fue creado
SELECT 
    "nUsuariosId",
    "cUsuariosNombre",
    "cUsuariosEmail", 
    "cUsuariosPerCodigo",
    "cUsuariosPerJurCodigo",
    "bUsuariosActivo"
FROM "Usuarios" 
WHERE "cUsuariosPerCodigo" = 'JESANCHEZR';