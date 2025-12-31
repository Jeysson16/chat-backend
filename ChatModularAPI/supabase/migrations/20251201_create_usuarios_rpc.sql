CREATE OR REPLACE FUNCTION public.usp_usuarios_search(
    cTerminoBusqueda text DEFAULT NULL
)
RETURNS SETOF public."Usuarios"
LANGUAGE sql STABLE AS $$
    SELECT u.*
    FROM public."Usuarios" u
    WHERE (
        cTerminoBusqueda IS NULL OR cTerminoBusqueda = '' OR
        lower(u."cUsuariosNombre") LIKE '%' || lower(cTerminoBusqueda) || '%' OR
        lower(u."cUsuariosEmail")  LIKE '%' || lower(cTerminoBusqueda) || '%' OR
        lower(COALESCE(u."cUsuariosUsername", '')) LIKE '%' || lower(cTerminoBusqueda) || '%' OR
        lower(u."nUsuariosId")     LIKE '%' || lower(cTerminoBusqueda) || '%'
    )
    ORDER BY u."dUsuariosFechaCreacion" DESC;
$$;

CREATE OR REPLACE FUNCTION public.usp_usuarios_total()
RETURNS integer
LANGUAGE sql STABLE AS $$
    SELECT COUNT(*) FROM public."Usuarios";
$$;

CREATE OR REPLACE FUNCTION public.usp_usuarios_activos_total()
RETURNS integer
LANGUAGE sql STABLE AS $$
    SELECT COUNT(*) FROM public."Usuarios" WHERE "bUsuariosActivo" = true;
$$;

CREATE OR REPLACE FUNCTION public.usp_usuarios_online_total()
RETURNS integer
LANGUAGE sql STABLE AS $$
    SELECT COUNT(*) FROM public."Usuarios" WHERE "bUsuariosEstaEnLinea" = true;
$$;

GRANT EXECUTE ON FUNCTION public.usp_usuarios_search(text) TO authenticated;
GRANT EXECUTE ON FUNCTION public.usp_usuarios_total() TO authenticated;
GRANT EXECUTE ON FUNCTION public.usp_usuarios_activos_total() TO authenticated;
GRANT EXECUTE ON FUNCTION public.usp_usuarios_online_total() TO authenticated;
