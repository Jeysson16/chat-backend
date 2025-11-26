-- Eliminar función existente y recrear con nomenclatura corregida
DROP FUNCTION IF EXISTS sp_aplicaciones_crear(VARCHAR, VARCHAR, TEXT);

-- Stored Procedure: sp_aplicaciones_crear
-- Descripción: Crea una nueva aplicación con tokens de acceso y configuraciones por defecto
-- Parámetros:
--   cAplicacionesNombre: Nombre de la aplicación (requerido)
--   cAppRegistroCodigoApp: Código único de la aplicación (requerido)
--   cAplicacionesDescripcion: Descripción de la aplicación (opcional)
-- Retorna: Datos de la aplicación creada incluyendo tokens generados

CREATE OR REPLACE FUNCTION sp_aplicaciones_crear(
    cAplicacionesNombre VARCHAR(100),
    cAppRegistroCodigoApp VARCHAR(100),
    cAplicacionesDescripcion TEXT DEFAULT NULL
)
RETURNS TABLE(
    resultado_nAplicacionesId INTEGER,
    resultado_cAplicacionesNombre VARCHAR(255),
    resultado_cAplicacionesCodigo VARCHAR(50),
    resultado_cAppRegistroCodigoApp VARCHAR(50),
    resultado_cAppRegistroTokenAcceso VARCHAR(255),
    resultado_cAppRegistroSecretoApp VARCHAR(255),
    resultado_dAplicacionesFechaCreacion TIMESTAMPTZ,
    resultado_nConfiguracionesCreadas INTEGER
)
LANGUAGE plpgsql
AS $$
DECLARE
    var_nAplicacionesId INTEGER;
    var_cAppRegistroTokenAcceso VARCHAR(500);
    var_cAppRegistroSecretoApp VARCHAR(500);
    var_nConfiguracionesCreadas INTEGER;
BEGIN
    -- Verificar si la aplicación ya existe
    IF EXISTS (SELECT 1 FROM "AppRegistros" WHERE "cAppRegistrosCodigoApp" = sp_aplicaciones_crear.cAppRegistroCodigoApp) THEN
        RAISE EXCEPTION 'Ya existe una aplicación con el código: %', sp_aplicaciones_crear.cAppRegistroCodigoApp;
    END IF;

    -- Generar tokens únicos para acceso y seguridad
    var_cAppRegistroTokenAcceso := 'AT_' || encode(gen_random_bytes(16), 'hex');
    var_cAppRegistroSecretoApp := 'ST_' || encode(gen_random_bytes(24), 'hex');

    -- Insertar nueva aplicación
    INSERT INTO "Aplicaciones" (
        "cAplicacionesNombre",
        "cAplicacionesDescripcion",
        "cAplicacionesCodigo",
        "dAplicacionesFechaCreacion"
    ) VALUES (
        sp_aplicaciones_crear.cAplicacionesNombre,
        sp_aplicaciones_crear.cAplicacionesDescripcion,
        sp_aplicaciones_crear.cAppRegistroCodigoApp,
        NOW()
    ) RETURNING "Aplicaciones"."nAplicacionesId" INTO var_nAplicacionesId;

    -- Insertar registro de aplicación con tokens
    INSERT INTO "AppRegistros" (
        "cAppRegistrosCodigoApp",
        "cAppRegistrosNombreApp",
        "cAppRegistrosTokenAcceso",
        "cAppRegistrosSecretoApp",
        "bAppRegistrosEsActivo",
        "dAppRegistrosFechaCreacion",
        "dAppRegistrosFechaExpiracion",
        "jAppRegistrosConfiguracionesAdicionales"
    ) VALUES (
        sp_aplicaciones_crear.cAppRegistroCodigoApp,
        sp_aplicaciones_crear.cAplicacionesNombre,
        var_cAppRegistroTokenAcceso,
        var_cAppRegistroSecretoApp,
        TRUE,
        NOW(),
        NOW() + INTERVAL '1 year',
        '{}'::jsonb
    );

    -- Insertar configuraciones por defecto (comentado hasta crear sp_aplicaciones_configuracion_crear)
    -- PERFORM sp_aplicaciones_configuracion_crear(
    --     var_nAplicacionesId,
    --     sp_aplicaciones_crear.cAplicacionesNombre,
    --     sp_aplicaciones_crear.cAplicacionesDescripcion,
    --     'API',
    --     'jpg,png,gif,pdf,doc,docx',
    --     10485760,
    --     true,
    --     true,
    --     100,
    --     1000,
    --     'MEDIUM',
    --     24,
    --     NULL,
    --     NULL
    -- );

    var_nConfiguracionesCreadas := 0;

    -- Retornar información de la aplicación creada
    RETURN QUERY
    SELECT 
        a."nAplicacionesId",
        a."cAplicacionesNombre",
        a."cAplicacionesCodigo",
        ar."cAppRegistrosCodigoApp",
        ar."cAppRegistrosTokenAcceso",
        ar."cAppRegistrosSecretoApp",
        a."dAplicacionesFechaCreacion",
        var_nConfiguracionesCreadas
    FROM "Aplicaciones" a
    JOIN "AppRegistros" ar ON a."cAplicacionesCodigo" = ar."cAppRegistrosCodigoApp"
    WHERE a."nAplicacionesId" = var_nAplicacionesId;

END;
$$;

-- Otorgar permisos de ejecución
GRANT EXECUTE ON FUNCTION sp_aplicaciones_crear TO authenticated;
GRANT EXECUTE ON FUNCTION sp_aplicaciones_crear TO service_role;