-- =============================================
-- STORED PROCEDURE: sp_aplicaciones_existe_por_nombre
-- Descripción: Verifica si existe una aplicación por nombre
-- =============================================

CREATE OR REPLACE FUNCTION sp_aplicaciones_existe_por_nombre(
    cAplicacionesNombre VARCHAR,
    nAplicacionesIdExcluir INTEGER DEFAULT NULL
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
        a.nAplicacionesId,
        a.cAplicacionesNombre,
        a.cAplicacionesDescripcion,
        a.cAplicacionesCodigo,
        a.dAplicacionesFechaCreacion,
        a.bAplicacionesEsActiva
    FROM "Aplicaciones" a
    WHERE a.cAplicacionesNombre = cAplicacionesNombre
      AND a.bAplicacionesEsActiva = TRUE
      AND (nAplicacionesIdExcluir IS NULL OR a.nAplicacionesId != nAplicacionesIdExcluir);
END;
$$ LANGUAGE plpgsql;

-- Mensaje de confirmación
SELECT 'Stored procedure sp_aplicaciones_existe_por_nombre creado exitosamente' AS resultado;