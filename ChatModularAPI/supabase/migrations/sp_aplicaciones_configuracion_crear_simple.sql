-- Stored procedure simplificado para crear configuración básica de aplicación
CREATE OR REPLACE FUNCTION sp_aplicaciones_configuracion_crear_simple(
    aplicacion_id INTEGER,
    cNombre VARCHAR(255) DEFAULT 'Aplicación Chat'
)
RETURNS TABLE(
    "nConfiguracionAplicacionId" INTEGER,
    "nConfiguracionAplicacionAplicacionId" INTEGER,
    "cConfiguracionAplicacionClave" VARCHAR,
    "cConfiguracionAplicacionValor" TEXT,
    "cConfiguracionAplicacionDescripcion" TEXT,
    "bConfiguracionAplicacionEsActiva" BOOLEAN,
    "dConfiguracionAplicacionFechaCreacion" TIMESTAMPTZ,
    "dConfiguracionAplicacionFechaActualizacion" TIMESTAMPTZ
)
LANGUAGE plpgsql
AS $$
DECLARE
    config_id INTEGER;
BEGIN
    -- Verificar que la aplicación existe
    IF NOT EXISTS (SELECT 1 FROM "Aplicaciones" WHERE "nAplicacionesId" = aplicacion_id) THEN
        RAISE EXCEPTION 'La aplicación con ID % no existe', aplicacion_id;
    END IF;

    -- Crear configuración básica para la aplicación
    INSERT INTO "ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion"
    ) VALUES (
        aplicacion_id,
        'nombre',
        cNombre,
        'Nombre de la aplicación'
    ) RETURNING "nConfiguracionAplicacionId" INTO config_id;
    
    -- Retornar la configuración creada
    RETURN QUERY
    SELECT 
        ca."nConfiguracionAplicacionId",
        ca."nConfiguracionAplicacionAplicacionId",
        ca."cConfiguracionAplicacionClave",
        ca."cConfiguracionAplicacionValor",
        ca."cConfiguracionAplicacionDescripcion",
        ca."bConfiguracionAplicacionEsActiva",
        ca."dConfiguracionAplicacionFechaCreacion",
        ca."dConfiguracionAplicacionFechaActualizacion"
    FROM "ConfiguracionAplicacion" ca
    WHERE ca."nConfiguracionAplicacionId" = config_id;
    
END;
$$;