-- =====================================================
-- MIGRACIÓN DE CONFIGURACIÓN DE APLICACIONES
-- Fecha: 2024-12-26
-- Propósito: Agregar tabla ConfiguracionAplicacion
-- =====================================================

-- =====================================================
-- PASO 1: CREAR SECUENCIA PARA ConfiguracionAplicacion
-- =====================================================

-- Crear secuencia para la tabla ConfiguracionAplicacion
CREATE SEQUENCE IF NOT EXISTS "ConfiguracionAplicacion_nConfiguracionAplicacionId_seq";

-- =====================================================
-- PASO 2: CREAR TABLA ConfiguracionAplicacion
-- =====================================================

-- Crear tabla ConfiguracionAplicacion siguiendo el estándar PascalCase/Húngaro
CREATE TABLE IF NOT EXISTS ConfiguracionAplicacion (
    nConfiguracionAplicacionId INTEGER DEFAULT nextval('"ConfiguracionAplicacion_nConfiguracionAplicacionId_seq"'::regclass) PRIMARY KEY,
    nConfiguracionAplicacionAplicacionId INTEGER NOT NULL,
    cConfiguracionAplicacionClave VARCHAR(100) NOT NULL,
    cConfiguracionAplicacionValor TEXT,
    cConfiguracionAplicacionDescripcion TEXT,
    bConfiguracionAplicacionEsActiva BOOLEAN DEFAULT true,
    dConfiguracionAplicacionFechaCreacion TIMESTAMPTZ DEFAULT NOW(),
    dConfiguracionAplicacionFechaActualizacion TIMESTAMPTZ DEFAULT NOW(),
    
    -- Constraint único para clave por aplicación
    CONSTRAINT uk_configuracion_aplicacion_clave 
        UNIQUE (nConfiguracionAplicacionAplicacionId, cConfiguracionAplicacionClave)
);

-- =====================================================
-- PASO 3: CREAR ÍNDICES PARA OPTIMIZACIÓN
-- =====================================================

-- Índices para ConfiguracionAplicacion
CREATE INDEX IF NOT EXISTS idx_configuracion_aplicacion_aplicacion 
    ON ConfiguracionAplicacion(nConfiguracionAplicacionAplicacionId);

CREATE INDEX IF NOT EXISTS idx_configuracion_aplicacion_clave 
    ON ConfiguracionAplicacion(cConfiguracionAplicacionClave);

CREATE INDEX IF NOT EXISTS idx_configuracion_aplicacion_activa 
    ON ConfiguracionAplicacion(bConfiguracionAplicacionEsActiva);

-- =====================================================
-- PASO 4: AGREGAR COMENTARIOS
-- =====================================================

COMMENT ON TABLE ConfiguracionAplicacion IS 'Tabla de configuraciones específicas por aplicación - Estándar: PascalCase/Húngaro';

-- =====================================================
-- PASO 5: HABILITAR RLS EN NUEVA TABLA
-- =====================================================

-- Habilitar Row Level Security
ALTER TABLE ConfiguracionAplicacion ENABLE ROW LEVEL SECURITY;