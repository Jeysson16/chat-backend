-- =====================================================
-- Migración: Crear tabla ConfiguracionAplicacion unificada
-- Fecha: 2024-12-29
-- Descripción: Tabla para configuración unificada de aplicaciones
-- =====================================================

-- Crear tabla ConfiguracionAplicacion con estructura unificada
CREATE TABLE IF NOT EXISTS ConfiguracionAplicacion (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    cAppCodigo VARCHAR(20) NOT NULL REFERENCES AppRegistro(cAppCodigo) ON DELETE CASCADE,
    
    -- Configuraciones de adjuntos
    nMaxTamanoArchivo INTEGER DEFAULT 10485760 CHECK (nMaxTamanoArchivo > 0 AND nMaxTamanoArchivo <= 104857600), -- 10MB por defecto, máximo 100MB
    cTiposArchivosPermitidos TEXT DEFAULT 'jpg,jpeg,png,gif,pdf,doc,docx,txt,mp3,wav,mp4,avi',
    bPermitirAdjuntos BOOLEAN DEFAULT true,
    nMaxCantidadAdjuntos INTEGER DEFAULT 5 CHECK (nMaxCantidadAdjuntos > 0 AND nMaxCantidadAdjuntos <= 20),
    bPermitirVisualizacionAdjuntos BOOLEAN DEFAULT true,
    
    -- Configuraciones de chat
    nMaxLongitudMensaje INTEGER DEFAULT 1000 CHECK (nMaxLongitudMensaje > 0 AND nMaxLongitudMensaje <= 10000),
    bPermitirEmojis BOOLEAN DEFAULT true,
    bPermitirMensajesVoz BOOLEAN DEFAULT true,
    bPermitirNotificaciones BOOLEAN DEFAULT true,
    
    -- Configuraciones de seguridad
    bRequiereAutenticacion BOOLEAN DEFAULT true,
    bPermitirMensajesAnonimos BOOLEAN DEFAULT false,
    nTiempoExpiracionSesion INTEGER DEFAULT 3600 CHECK (nTiempoExpiracionSesion >= 300 AND nTiempoExpiracionSesion <= 86400), -- Entre 5 minutos y 24 horas
    
    -- Metadatos
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    bEsActiva BOOLEAN DEFAULT true,
    
    -- Restricción única por aplicación
    UNIQUE(cAppCodigo)
);

-- Índices para optimizar consultas
CREATE INDEX IF NOT EXISTS idx_configuracion_aplicacion_app_codigo ON ConfiguracionAplicacion(cAppCodigo);
CREATE INDEX IF NOT EXISTS idx_configuracion_aplicacion_activa ON ConfiguracionAplicacion(bEsActiva);
CREATE INDEX IF NOT EXISTS idx_configuracion_aplicacion_adjuntos ON ConfiguracionAplicacion(bPermitirAdjuntos);

-- Trigger para actualizar updated_at automáticamente
CREATE TRIGGER update_configuracion_aplicacion_updated_at 
    BEFORE UPDATE ON ConfiguracionAplicacion 
    FOR EACH ROW 
    EXECUTE FUNCTION update_updated_at_column();

-- Permisos para Supabase
GRANT SELECT ON ConfiguracionAplicacion TO anon;
GRANT ALL PRIVILEGES ON ConfiguracionAplicacion TO authenticated;

-- Comentarios en la tabla
COMMENT ON TABLE ConfiguracionAplicacion IS 'Configuración unificada por aplicación para adjuntos, chat y seguridad';
COMMENT ON COLUMN ConfiguracionAplicacion.nMaxTamanoArchivo IS 'Tamaño máximo de archivo en bytes (por defecto 10MB)';
COMMENT ON COLUMN ConfiguracionAplicacion.cTiposArchivosPermitidos IS 'Lista de extensiones de archivos permitidos separados por coma';
COMMENT ON COLUMN ConfiguracionAplicacion.nMaxCantidadAdjuntos IS 'Cantidad máxima de adjuntos por mensaje';
COMMENT ON COLUMN ConfiguracionAplicacion.nMaxLongitudMensaje IS 'Longitud máxima de mensaje en caracteres';
COMMENT ON COLUMN ConfiguracionAplicacion.nTiempoExpiracionSesion IS 'Tiempo de expiración de sesión en segundos';

-- Insertar configuraciones por defecto para aplicaciones existentes
INSERT INTO ConfiguracionAplicacion (cAppCodigo)
SELECT cAppCodigo 
FROM AppRegistro 
WHERE bAppActivo = true
ON CONFLICT (cAppCodigo) DO NOTHING;

-- Verificar que la migración se ejecutó correctamente
DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'configuracionaplicacion') THEN
        RAISE NOTICE 'Tabla ConfiguracionAplicacion creada exitosamente';
    ELSE
        RAISE EXCEPTION 'Error: No se pudo crear la tabla ConfiguracionAplicacion';
    END IF;
END $$;