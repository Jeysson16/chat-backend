-- Stored procedure de prueba para crear configuración básica de aplicación
CREATE OR REPLACE FUNCTION sp_aplicaciones_configuracion_crear_test(
    aplicacion_id INTEGER
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
BEGIN
    -- Simplemente devolver datos ficticios para probar
    RETURN QUERY
    SELECT 
        1 as "nConfiguracionAplicacionId",
        aplicacion_id as "nConfiguracionAplicacionAplicacionId",
        'test'::VARCHAR as "cConfiguracionAplicacionClave",
        'test value'::TEXT as "cConfiguracionAplicacionValor",
        'test description'::TEXT as "cConfiguracionAplicacionDescripcion",
        true as "bConfiguracionAplicacionEsActiva",
        now() as "dConfiguracionAplicacionFechaCreacion",
        now() as "dConfiguracionAplicacionFechaActualizacion";
END;
$$;