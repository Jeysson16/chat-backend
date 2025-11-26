-- Migración para corregir la tabla ConfiguracionAplicacion
-- Eliminar la tabla incorrecta y crear la correcta con estándar PascalCase/Húngaro

-- Eliminar la tabla incorrecta
DROP TABLE IF EXISTS configuracionaplicacion CASCADE;

-- Eliminar la secuencia asociada
DROP SEQUENCE IF EXISTS "ConfiguracionAplicacion_nConfiguracionAplicacionId_seq" CASCADE;

-- Crear la secuencia correcta
CREATE SEQUENCE "ConfiguracionAplicacion_nConfiguracionAplicacionId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

-- Crear la tabla ConfiguracionAplicacion con el estándar correcto
CREATE TABLE "ConfiguracionAplicacion" (
    "nConfiguracionAplicacionId" INTEGER DEFAULT nextval('"ConfiguracionAplicacion_nConfiguracionAplicacionId_seq"'::regclass) NOT NULL,
    "nConfiguracionAplicacionAplicacionId" INTEGER NOT NULL,
    "cConfiguracionAplicacionClave" VARCHAR(255) NOT NULL,
    "cConfiguracionAplicacionValor" TEXT,
    "cConfiguracionAplicacionDescripcion" TEXT,
    "bConfiguracionAplicacionEsActiva" BOOLEAN DEFAULT true,
    "dConfiguracionAplicacionFechaCreacion" TIMESTAMPTZ DEFAULT now(),
    "dConfiguracionAplicacionFechaActualizacion" TIMESTAMPTZ DEFAULT now(),
    
    CONSTRAINT "pk_ConfiguracionAplicacion" PRIMARY KEY ("nConfiguracionAplicacionId"),
    CONSTRAINT "fk_ConfiguracionAplicacion_Aplicacion" FOREIGN KEY ("nConfiguracionAplicacionAplicacionId") 
        REFERENCES "Aplicaciones"("nAplicacionesId") ON DELETE CASCADE,
    CONSTRAINT "uq_ConfiguracionAplicacion_Aplicacion_Clave" UNIQUE ("nConfiguracionAplicacionAplicacionId", "cConfiguracionAplicacionClave")
);

-- Crear índices
CREATE INDEX "idx_ConfiguracionAplicacion_AplicacionId" ON "ConfiguracionAplicacion"("nConfiguracionAplicacionAplicacionId");
CREATE INDEX "idx_ConfiguracionAplicacion_Clave" ON "ConfiguracionAplicacion"("cConfiguracionAplicacionClave");
CREATE INDEX "idx_ConfiguracionAplicacion_Activa" ON "ConfiguracionAplicacion"("bConfiguracionAplicacionEsActiva");

-- Agregar comentarios
COMMENT ON TABLE "ConfiguracionAplicacion" IS 'Tabla de configuraciones específicas por aplicación - Estándar: PascalCase/Húngaro';
COMMENT ON COLUMN "ConfiguracionAplicacion"."nConfiguracionAplicacionId" IS 'ID único de la configuración';
COMMENT ON COLUMN "ConfiguracionAplicacion"."nConfiguracionAplicacionAplicacionId" IS 'ID de la aplicación asociada';
COMMENT ON COLUMN "ConfiguracionAplicacion"."cConfiguracionAplicacionClave" IS 'Clave de configuración';
COMMENT ON COLUMN "ConfiguracionAplicacion"."cConfiguracionAplicacionValor" IS 'Valor de configuración';
COMMENT ON COLUMN "ConfiguracionAplicacion"."cConfiguracionAplicacionDescripcion" IS 'Descripción de la configuración';
COMMENT ON COLUMN "ConfiguracionAplicacion"."bConfiguracionAplicacionEsActiva" IS 'Indica si la configuración está activa';
COMMENT ON COLUMN "ConfiguracionAplicacion"."dConfiguracionAplicacionFechaCreacion" IS 'Fecha de creación del registro';
COMMENT ON COLUMN "ConfiguracionAplicacion"."dConfiguracionAplicacionFechaActualizacion" IS 'Fecha de última actualización';

-- Función para actualizar fecha de modificación
CREATE OR REPLACE FUNCTION update_configuracion_aplicacion_updated_at()
RETURNS TRIGGER AS $$
BEGIN
    NEW."dConfiguracionAplicacionFechaActualizacion" = now();
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Trigger para actualizar automáticamente la fecha de modificación
CREATE TRIGGER trigger_update_configuracion_aplicacion_updated_at
    BEFORE UPDATE ON "ConfiguracionAplicacion"
    FOR EACH ROW
    EXECUTE FUNCTION update_configuracion_aplicacion_updated_at();

-- Habilitar Row Level Security
ALTER TABLE "ConfiguracionAplicacion" ENABLE ROW LEVEL SECURITY;

-- Insertar configuraciones por defecto para aplicaciones existentes
INSERT INTO "ConfiguracionAplicacion" (
    "nConfiguracionAplicacionAplicacionId",
    "cConfiguracionAplicacionClave",
    "cConfiguracionAplicacionValor",
    "cConfiguracionAplicacionDescripcion"
)
SELECT 
    a."nAplicacionesId",
    'max_usuarios_concurrentes',
    '100',
    'Número máximo de usuarios concurrentes permitidos'
FROM "Aplicaciones" a
WHERE a."bAplicacionesEsActiva" = true;

INSERT INTO "ConfiguracionAplicacion" (
    "nConfiguracionAplicacionAplicacionId",
    "cConfiguracionAplicacionClave",
    "cConfiguracionAplicacionValor",
    "cConfiguracionAplicacionDescripcion"
)
SELECT 
    a."nAplicacionesId",
    'tiempo_sesion_minutos',
    '1440',
    'Tiempo de sesión en minutos (24 horas por defecto)'
FROM "Aplicaciones" a
WHERE a."bAplicacionesEsActiva" = true;

INSERT INTO "ConfiguracionAplicacion" (
    "nConfiguracionAplicacionAplicacionId",
    "cConfiguracionAplicacionClave",
    "cConfiguracionAplicacionValor",
    "cConfiguracionAplicacionDescripcion"
)
SELECT 
    a."nAplicacionesId",
    'notificaciones_habilitadas',
    'true',
    'Indica si las notificaciones están habilitadas para la aplicación'
FROM "Aplicaciones" a
WHERE a."bAplicacionesEsActiva" =