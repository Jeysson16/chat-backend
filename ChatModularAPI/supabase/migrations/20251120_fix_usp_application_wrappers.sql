-- =============================================
-- Create uppercase USP_ wrappers for Application RPC
-- This aligns backend nomenclature (USP_*) with Supabase/PostgREST
-- =============================================

-- Get All Applications
CREATE OR REPLACE FUNCTION public."USP_Application_GetAll"()
RETURNS TABLE (
    "nAplicacionesId" integer,
    "cAplicacionesNombre" varchar,
    "cAplicacionesDescripcion" text,
    "cAplicacionesCodigo" varchar,
    "bAplicacionesEsActiva" boolean,
    "dAplicacionesFechaCreacion" timestamptz
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT 
        a."nAplicacionesId",
        a."cAplicacionesNombre",
        a."cAplicacionesDescripcion",
        a."cAplicacionesCodigo",
        a."bAplicacionesEsActiva",
        a."dAplicacionesFechaCreacion"
    FROM public."Aplicaciones" a
    WHERE a."bAplicacionesEsActiva" = TRUE
    ORDER BY a."cAplicacionesNombre";
END;
$$;

GRANT EXECUTE ON FUNCTION public."USP_Application_GetAll"() TO authenticated;
GRANT EXECUTE ON FUNCTION public."USP_Application_GetAll"() TO anon;

-- Get Application by Id
CREATE OR REPLACE FUNCTION public."USP_Application_GetById"(p_nAplicacionesId integer)
RETURNS TABLE (
    "nAplicacionesId" integer,
    "cAplicacionesNombre" varchar,
    "cAplicacionesDescripcion" text,
    "cAplicacionesCodigo" varchar,
    "bAplicacionesEsActiva" boolean,
    "dAplicacionesFechaCreacion" timestamptz
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT 
        a."nAplicacionesId",
        a."cAplicacionesNombre",
        a."cAplicacionesDescripcion",
        a."cAplicacionesCodigo",
        a."bAplicacionesEsActiva",
        a."dAplicacionesFechaCreacion"
    FROM public."Aplicaciones" a
    WHERE a."nAplicacionesId" = p_nAplicacionesId
    LIMIT 1;
END;
$$;

GRANT EXECUTE ON FUNCTION public."USP_Application_GetById"(integer) TO authenticated;
GRANT EXECUTE ON FUNCTION public."USP_Application_GetById"(integer) TO anon;

-- Get Application by Code
CREATE OR REPLACE FUNCTION public."USP_Application_GetByCode"(p_cAplicacionesCodigo varchar)
RETURNS TABLE (
    "nAplicacionesId" integer,
    "cAplicacionesNombre" varchar,
    "cAplicacionesDescripcion" text,
    "cAplicacionesCodigo" varchar,
    "bAplicacionesEsActiva" boolean,
    "dAplicacionesFechaCreacion" timestamptz
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT 
        a."nAplicacionesId",
        a."cAplicacionesNombre",
        a."cAplicacionesDescripcion",
        a."cAplicacionesCodigo",
        a."bAplicacionesEsActiva",
        a."dAplicacionesFechaCreacion"
    FROM public."Aplicaciones" a
    WHERE a."cAplicacionesCodigo" = p_cAplicacionesCodigo
    LIMIT 1;
END;
$$;

GRANT EXECUTE ON FUNCTION public."USP_Application_GetByCode"(varchar) TO authenticated;
GRANT EXECUTE ON FUNCTION public."USP_Application_GetByCode"(varchar) TO anon;

-- Exists by Name (returns 1 or 0)
CREATE OR REPLACE FUNCTION public."USP_Application_ExistsByName"(p_cAplicacionesNombre varchar, p_nExcludeId integer DEFAULT NULL)
RETURNS TABLE (exists_count integer)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT 
        CASE WHEN COUNT(1) > 0 THEN 1 ELSE 0 END
    FROM public."Aplicaciones" a
    WHERE a."cAplicacionesNombre" = p_cAplicacionesNombre
      AND (p_nExcludeId IS NULL OR a."nAplicacionesId" <> p_nExcludeId);
END;
$$;

GRANT EXECUTE ON FUNCTION public."USP_Application_ExistsByName"(varchar, integer) TO authenticated;
GRANT EXECUTE ON FUNCTION public."USP_Application_ExistsByName"(varchar, integer) TO anon;