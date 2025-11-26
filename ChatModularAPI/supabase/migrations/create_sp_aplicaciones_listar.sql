-- Crear stored procedure para listar aplicaciones
-- Este procedimiento devuelve todas las aplicaciones activas del sistema

CREATE OR REPLACE FUNCTION public.sp_aplicaciones_listar()
RETURNS TABLE (
    nAplicacionesId integer,
    cAplicacionesNombre character varying,
    cAplicacionesDescripcion text,
    cAplicacionesCodigo character varying,
    dAplicacionesFechaCreacion timestamp with time zone,
    bAplicacionesEsActiva boolean
)
LANGUAGE plpgsql
SECURITY DEFINER
AS $$
BEGIN
    RETURN QUERY
    SELECT 
        a.nAplicacionesId,
        a.cAplicacionesNombre,
        a.cAplicacionesDescripcion,
        a.cAplicacionesCodigo,
        a.dAplicacionesFechaCreacion,
        a.bAplicacionesEsActiva
    FROM public.Aplicaciones a
    WHERE a.bAplicacionesEsActiva = true
    ORDER BY a.dAplicacionesFechaCreacion DESC;
END;
$$;

-- Otorgar permisos de ejecución
GRANT EXECUTE ON FUNCTION public.sp_aplicaciones_listar() TO authenticated;
GRANT EXECUTE ON FUNCTION public.sp_aplicaciones_listar() TO anon;

-- Comentario para documentación
COMMENT ON FUNCTION public.sp_aplicaciones_listar() IS 'Stored procedure para listar todas las aplicaciones activas del sistema con nomenclatura en español';