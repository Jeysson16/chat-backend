-- Create wrappers expected by backend repository
-- Ensure functions exist with exact names/signatures used in code

CREATE OR REPLACE FUNCTION public.usp_application_getall()
RETURNS SETOF public."Aplicaciones"
LANGUAGE sql
AS $$
  SELECT *
  FROM public."Aplicaciones" a
  WHERE a."bAplicacionesEsActiva" = TRUE
  ORDER BY a."cAplicacionesNombre" ASC;
$$;

GRANT EXECUTE ON FUNCTION public.usp_application_getall() TO authenticated;
GRANT EXECUTE ON FUNCTION public.usp_application_getall() TO anon;

CREATE OR REPLACE FUNCTION public.usp_application_getbyid(p_nAplicacionesId integer)
RETURNS SETOF public."Aplicaciones"
LANGUAGE sql
AS $$
  SELECT *
  FROM public."Aplicaciones" a
  WHERE a."nAplicacionesId" = p_nAplicacionesId
  LIMIT 1;
$$;

GRANT EXECUTE ON FUNCTION public.usp_application_getbyid(integer) TO authenticated;
GRANT EXECUTE ON FUNCTION public.usp_application_getbyid(integer) TO anon;

CREATE OR REPLACE FUNCTION public."USP_Application_GetByCode"(p_cAplicacionesCodigo varchar)
RETURNS SETOF public."Aplicaciones"
LANGUAGE sql
AS $$
  SELECT *
  FROM public."Aplicaciones" a
  WHERE a."cAplicacionesCodigo" = p_cAplicacionesCodigo
  LIMIT 1;
$$;

GRANT EXECUTE ON FUNCTION public."USP_Application_GetByCode"(varchar) TO authenticated;
GRANT EXECUTE ON FUNCTION public."USP_Application_GetByCode"(varchar) TO anon;
