-- Migración: Migrar ConfiguracionAplicacion a estructura unificada
-- Fecha: 2025-01-01 12:00:00
-- Descripción: Migra de estructura clave-valor a estructura unificada con columnas específicas

-- Paso 1: Crear tabla temporal para backup
CREATE TABLE ConfiguracionAplicacion_backup AS 
SELECT * FROM "ConfiguracionAplicacion";

-- Paso 2: Renombrar tabla existente
ALTER TABLE "ConfiguracionAplicacion" RENAME TO "ConfiguracionAplicacion_old";

-- Paso 3: Crear nueva tabla ConfiguracionAplicacion con estructura unificada
CREATE TABLE "ConfiguracionAplicacion" (
    "nConfiguracionAplicacionId" SERIAL PRIMARY KEY,
    "nAplicacionesId" INTEGER NOT NULL REFERENCES "Aplicaciones"("nAplicacionesId"),
    
    -- Configuraciones de adjuntos
    "nMaxTamanoArchivo" INTEGER DEFAULT 10485760, -- 10MB en bytes
    "cTiposArchivosPermitidos" TEXT DEFAULT 'jpg,jpeg,png,gif,pdf,doc,docx,txt,mp3,wav,mp4,avi',
    "bPermitirAdjuntos" BOOLEAN DEFAULT true,
    "nMaxCantidadAdjuntos" INTEGER DEFAULT 5,
    "bPermitirVisualizacionAdjuntos" BOOLEAN DEFAULT true,
    
    -- Configuraciones de chat
    "nMaxLongitudMensaje" INTEGER DEFAULT 1000,
    "bPermitirEmojis" BOOLEAN DEFAULT true,
    "bPermitirMensajesVoz" BOOLEAN DEFAULT true,
    "bPermitirNotificaciones" BOOLEAN DEFAULT true,
    
    -- Configuraciones de seguridad
    "bRequiereAutenticacion" BOOLEAN DEFAULT true,
    "bPermitirMensajesAnonimos" BOOLEAN DEFAULT false,
    "nTiempoExpiracionSesion" INTEGER DEFAULT 3600, -- en segundos
    
    -- Metadatos
    "dFechaCreacion" TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    "dFechaActualizacion" TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    "bEsActiva" BOOLEAN DEFAULT true,
    
    -- Índices únicos
    UNIQUE("nAplicacionesId")
);

-- Paso 4: Comentarios para documentación
COMMENT ON TABLE "ConfiguracionAplicacion" IS 'Tabla de configuraciones unificadas por aplicación - Estándar: PascalCase/Húngaro';
COMMENT ON COLUMN "ConfiguracionAplicacion"."nConfiguracionAplicacionId" IS 'ID único de la configuración';
COMMENT ON COLUMN "ConfiguracionAplicacion"."nAplicacionesId" IS 'ID de la aplicación asociada';
COMMENT ON COLUMN "ConfiguracionAplicacion"."nMaxTamanoArchivo" IS 'Tamaño máximo de archivo en bytes';
COMMENT ON COLUMN "ConfiguracionAplicacion"."cTiposArchivosPermitidos" IS 'Tipos de archivos permitidos separados por coma';
COMMENT ON COLUMN "ConfiguracionAplicacion"."bPermitirAdjuntos" IS 'Indica si se permiten adjuntos';

-- Paso 5: Insertar configuración por defecto para aplicaciones existentes
INSERT INTO "ConfiguracionAplicacion" (
    "nAplicacionesId",
    "nMaxTamanoArchivo",
    "cTiposArchivosPermitidos",
    "bPermitirAdjuntos",
    "nMaxCantidadAdjuntos",
    "bPermitirVisualizacionAdjuntos",
    "nMaxLongitudMensaje",
    "bPermitirEmojis",
    "bPermitirMensajesVoz",
    "bPermitirNotificaciones",
    "bRequiereAutenticacion",
    "bPermitirMensajesAnonimos",
    "nTiempoExpiracionSesion"
)
SELECT 
    "nAplicacionesId",
    10485760, -- 10MB
    'jpg,jpeg,png,gif,pdf,doc,docx,txt,mp3,wav,mp4,avi',
    true,
    5,
    true,
    1000,
    true,
    true,
    true,
    true,
    false,
    3600
FROM "Aplicaciones" 
WHERE "bAplicacionesEsActiva" = true;

-- Paso 6: Crear función para actualizar fecha de modificación
CREATE OR REPLACE FUNCTION update_configuracion_aplicacion_updated_at()
RETURNS TRIGGER AS $$
BEGIN
    NEW."dFechaActualizacion" = NOW();
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Paso 7: Crear trigger para actualización automática
CREATE TRIGGER trigger_update_configuracion_aplicacion_updated_at
    BEFORE UPDATE ON "ConfiguracionAplicacion"
    FOR EACH ROW
    EXECUTE FUNCTION update_configuracion_aplicacion_updated_at();

-- Paso 8: Habilitar RLS (Row Level Security)
ALTER TABLE "ConfiguracionAplicacion" ENABLE ROW LEVEL SECURITY;

-- Paso 9: Crear políticas RLS básicas
CREATE POLICY "Permitir lectura de configuraciones" ON "ConfiguracionAplicacion"
    FOR SELECT USING (true);

CREATE POLICY "Permitir actualización de configuraciones" ON "ConfiguracionAplicacion"
    FOR UPDATE USING (true);