-- Stored Procedure: sp_aplicaciones_listar (Fixed Version)
-- Descripción: Lista todas las aplicaciones registradas en el sistema
-- Parámetros: Ninguno
-- Retorna: Lista completa de aplicaciones con sus datos básicos

-- Drop existing function if it exists
DROP FUNCTION IF EXISTS sp_aplicaciones_listar();

-- Create the corrected function based on actual table structure
CREATE OR REPLACE FUNCTION sp_aplicaciones_listar()
RETURNS TABLE(
    nAplicacionesId INTEGER,
    cAplicacionesNombre VARCHAR,
    cAplicacionesDescripcion TEXT,
    cAplicacionesCodigo VARCHAR,
    bAplicacionesEsActiva BOOLEAN,
    dAplicacionesFechaCreacion TIMESTAMPTZ
) AS $$
BEGIN
    -- Retornar todas las aplicaciones ordenadas por fecha de creación descendente
    RETURN QUERY
    SELECT 
        a."nAplicacionesId",
        a."cAplicacionesNombre",
        a."cAplicacionesDescripcion",
        a."cAplicacionesCodigo",
        a."bAplicacionesEsActiva",
        a."dAplicacionesFechaCreacion"
    FROM "Aplicaciones" a
    ORDER BY a."dAplicacionesFechaCreacion" DESC;
    
EXCEPTION
    WHEN OTHERS THEN
        RAISE EXCEPTION 'Error al listar aplicaciones: %', SQLERRM;
END;
$$ LANGUAGE plpgsql;

-- Comentario sobre la función
COMMENT ON FUNCTION sp_aplicaciones_listar() IS 'Lista todas las aplicaciones registradas en el sistema ordenadas por fecha de creación descendente';