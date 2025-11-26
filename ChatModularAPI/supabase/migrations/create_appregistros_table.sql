-- Crear tabla AppRegistros para almacenar tokens de aplicación
-- Tabla de registro de aplicaciones - Estándar: PascalCase/Húngaro

CREATE TABLE IF NOT EXISTS "AppRegistros" (
    "nAppRegistrosId" SERIAL PRIMARY KEY,
    "nAppRegistrosAplicacionId" INTEGER NOT NULL,
    "cAppRegistrosCodigoApp" VARCHAR(100) NOT NULL UNIQUE,
    "cAppRegistrosNombreApp" VARCHAR(255) NOT NULL,
    "cAppRegistrosTokenAcceso" VARCHAR(500) NOT NULL,
    "cAppRegistrosSecretoApp" VARCHAR(500) NOT NULL,
    "bAppRegistrosEsActivo" BOOLEAN DEFAULT true,
    "dAppRegistrosFechaCreacion" TIMESTAMPTZ DEFAULT now(),
    "dAppRegistrosFechaExpiracion" TIMESTAMPTZ DEFAULT (now() + INTERVAL '1 year'),
    "jAppRegistrosConfiguracionesAdicionales" JSONB DEFAULT '{}'::jsonb,
    
    -- Foreign key constraint
    CONSTRAINT "fk_appregistros_aplicacion" 
        FOREIGN KEY ("nAppRegistrosAplicacionId") 
        REFERENCES "Aplicaciones"("nAplicacionesId") 
        ON DELETE CASCADE
);

-- Índices para optimizar consultas
CREATE INDEX IF NOT EXISTS "idx_appregistros_codigo_app" ON "AppRegistros"("cAppRegistrosCodigoApp");
CREATE INDEX IF NOT EXISTS "idx_appregistros_aplicacion_id" ON "AppRegistros"("nAppRegistrosAplicacionId");
CREATE INDEX IF NOT EXISTS "idx_appregistros_token_acceso" ON "AppRegistros"("cAppRegistrosTokenAcceso");

-- Comentarios
COMMENT ON TABLE "AppRegistros" IS 'Tabla de registro de aplicaciones - Estándar: PascalCase/Húngaro';
COMMENT ON COLUMN "AppRegistros"."nAppRegistrosId" IS 'ID único del registro de aplicación';
COMMENT ON COLUMN "AppRegistros"."nAppRegistrosAplicacionId" IS 'ID de la aplicación asociada';
COMMENT ON COLUMN "AppRegistros"."cAppRegistrosCodigoApp" IS 'Código único de la aplicación';
COMMENT ON COLUMN "AppRegistros"."cAppRegistrosNombreApp" IS 'Nombre de la aplicación';
COMMENT ON COLUMN "AppRegistros"."cAppRegistrosTokenAcceso" IS 'Token de acceso para la aplicación';
COMMENT ON COLUMN "AppRegistros"."cAppRegistrosSecretoApp" IS 'Secreto de la aplicación';
COMMENT ON COLUMN "AppRegistros"."bAppRegistrosEsActivo" IS 'Indica si el registro está activo';
COMMENT ON COLUMN "AppRegistros"."dAppRegistrosFechaCreacion" IS 'Fecha de creación del registro';
COMMENT ON COLUMN "AppRegistros"."dAppRegistrosFechaExpiracion" IS 'Fecha de expiración del registro';
COMMENT ON COLUMN "AppRegistros"."jAppRegistrosConfiguracionesAdicionales" IS 'Configuraciones adicionales en formato JSON';