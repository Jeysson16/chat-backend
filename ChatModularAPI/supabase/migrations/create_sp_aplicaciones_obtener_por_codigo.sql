-- =============================================
-- STORED PROCEDURE: sp_aplicaciones_obtener_por_codigo
-- Descripción: Obtiene una aplicación por su código único
-- =============================================

CREATE OR REPLACE FUNCTION sp_aplicaciones_obtener_por_codigo(
    cAplicacionesCodigo VARCHAR(50)
)
RETURNS TABLE(
    nAplicacionesId INTEGER,
    cAplicacionesNombre VARCHAR,
    cAplicacionesDescripcion TEXT,
    cAplicacionesCodigo VARCHAR,
    dAplicacionesFechaCreacion TIMESTAMPTZ,
    bAplicacionesEsActiva BOOLEAN
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        a."nAplicacionesId",
        a."cAplicacionesNombre",
        a."cAplicacionesDescripcion",
        a."cAplicacionesCodigo",
        a."dAplicacionesFechaCreacion",
        a."bAplicacionesEsActiva"
    FROM "Aplicaciones" a
    WHERE a."cAplicacionesCodigo" = cAplicacionesCodigo
      AND a."bAplicacionesEsActiva" = TRUE;
END;
$$ LANGUAGE plpgsql;

-- Mensaje de confirmación
SELECT 'Stored procedure sp_aplicaciones_obtener_por_codigo creado exitosamente' AS resultado;