-- Dashboard stats RPC: aggregates counts for admin panel
CREATE OR REPLACE FUNCTION public.usp_dashboard_stats()
RETURNS jsonb
LANGUAGE sql STABLE AS $$
    WITH apps AS (
        SELECT COUNT(*) AS total,
               COALESCE(SUM(CASE WHEN "bAplicacionesEsActiva" = true THEN 1 ELSE 0 END), 0) AS activas
        FROM public."Aplicaciones"
    ), empresas AS (
        SELECT COUNT(*) AS total,
               COALESCE(SUM(CASE WHEN "bEmpresasEsActiva" = true THEN 1 ELSE 0 END), 0) AS activas
        FROM public."Empresas"
    ), usuarios AS (
        SELECT COUNT(*) AS total,
               COALESCE(SUM(CASE WHEN "bUsuariosActivo" = true THEN 1 ELSE 0 END), 0) AS activos,
               COALESCE(SUM(CASE WHEN "bUsuariosEstaEnLinea" = true THEN 1 ELSE 0 END), 0) AS online
        FROM public."Usuarios"
    ), config_app AS (
        SELECT COUNT(*) AS total FROM public."ConfiguracionAplicacion"
    ), config_emp AS (
        SELECT COUNT(*) AS total FROM public."ConfiguracionEmpresa"
    )
    SELECT jsonb_build_object(
        'totalAplicaciones', apps.total,
        'aplicacionesActivas', apps.activas,
        'totalEmpresas', empresas.total,
        'empresasActivas', empresas.activas,
        'totalUsuarios', usuarios.total,
        'usuariosActivos', usuarios.activos,
        'usuariosOnline', usuarios.online,
        'totalConfiguraciones', (config_app.total + config_emp.total),
        'configuracionesAplicacion', config_app.total,
        'configuracionesEmpresa', config_emp.total
    )
    FROM apps, empresas, usuarios, config_app, config_emp;
$$;

GRANT EXECUTE ON FUNCTION public.usp_dashboard_stats() TO authenticated;
