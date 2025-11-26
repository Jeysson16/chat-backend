-- Eliminar función existente y recrear sp_aplicaciones_crear
-- Primero eliminar la función existente si existe

DROP FUNCTION IF EXISTS public.sp_aplicaciones_crear(character varying, character varying, text);
DROP FUNCTION IF EXISTS public.sp_aplicaciones_crear(varchar, varchar, text);

-- Crear stored procedure para crear aplicaciones
CREATE OR REPLACE FUNCTION sp_aplicaciones_crear(
    p_cAplicacionesNombre VARCHAR(100),
    p_cAplicacionesCodigo VARCHAR(50),
    p_cAplicacionesDescripcion TEXT DEFAULT NULL
)
RETURNS TABLE(
    naplicacionesid INTEGER,
    caplicacionesnombre VARCHAR(100),
    caplicacionescodigo VARCHAR(50),
    cappregistrostokenacceso VARCHAR(255),
    cappregistrossecretoapp VARCHAR(255),
    daplicacionesfechacreacion TIMESTAMPTZ,
    nconfiguracionescreadas INTEGER
) AS $$

DECLARE
    v_nAplicacionesId INTEGER;
    v_cAppRegistrosTokenAcceso VARCHAR(255);
    v_cAppRegistrosSecretoApp VARCHAR(255);
    v_nConfiguracionesCreadas INTEGER;
BEGIN
    -- Verificar que el código de aplicación no exista
    IF EXISTS (SELECT 1 FROM "Aplicaciones" WHERE "cAplicacionesCodigo" = p_cAplicacionesCodigo) THEN
        RAISE EXCEPTION 'Ya existe una aplicación con el código: %', p_cAplicacionesCodigo;
    END IF;

    -- Generar tokens únicos
    v_cAppRegistrosTokenAcceso := 'AT_' || encode(gen_random_bytes(16), 'hex');
    v_cAppRegistrosSecretoApp := 'ST_' || encode(gen_random_bytes(20), 'hex');

    -- Insertar la nueva aplicación
    INSERT INTO "Aplicaciones" (
        "cAplicacionesNombre",
        "cAplicacionesCodigo", 
        "cAplicacionesDescripcion",
        "bAplicacionesEsActiva",
        "dAplicacionesFechaCreacion"
    ) VALUES (
        p_cAplicacionesNombre,
        p_cAplicacionesCodigo,
        COALESCE(p_cAplicacionesDescripcion, ''),
        TRUE,
        NOW()
    ) RETURNING "nAplicacionesId" INTO v_nAplicacionesId;

    -- Insertar registro de tokens en AppRegistros
    INSERT INTO "AppRegistros" (
        "nAppRegistrosAplicacionId",
        "cAppRegistrosTokenAcceso",
        "cAppRegistrosSecretoApp",
        "dAppRegistrosFechaCreacion",
        "dAppRegistrosFechaExpiracion",
        "bAppRegistrosEsActivo"
    ) VALUES (
        v_nAplicacionesId,
        v_cAppRegistrosTokenAcceso,
        v_cAppRegistrosSecretoApp,
        NOW(),
        NOW() + INTERVAL '1 year',
        TRUE
    );

    -- Crear configuraciones por defecto
    v_nConfiguracionesCreadas := 0;

    -- Configuración de adjuntos
    INSERT INTO "ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion",
        "bConfiguracionAplicacionActivo",
        "dConfiguracionAplicacionFechaCreacion",
        "dConfiguracionAplicacionFechaModificacion"
    ) VALUES (
        v_nAplicacionesId,
        'ADJUNTOS_HABILITADOS',
        'true',
        'Configuración para habilitar adjuntos en la aplicación',
        TRUE,
        NOW(),
        NOW()
    );
    v_nConfiguracionesCreadas := v_nConfiguracionesCreadas + 1;

    -- Configuración de tamaño máximo de adjuntos
    INSERT INTO "ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion",
        "bConfiguracionAplicacionActivo",
        "dConfiguracionAplicacionFechaCreacion",
        "dConfiguracionAplicacionFechaModificacion"
    ) VALUES (
        v_nAplicacionesId,
        'ADJUNTOS_TAMAÑO_MAXIMO_MB',
        '10',
        'Tamaño máximo permitido para adjuntos en MB',
        TRUE,
        NOW(),
        NOW()
    );
    v_nConfiguracionesCreadas := v_nConfiguracionesCreadas + 1;

    -- Retornar los datos de la aplicación creada
    RETURN QUERY
    SELECT 
        v_nAplicacionesId,
        p_cAplicacionesNombre,
        p_cAplicacionesCodigo,
        v_cAppRegistrosTokenAcceso,
        v_cAppRegistrosSecretoApp,
        NOW()::TIMESTAMPTZ,
        v_nConfiguracionesCreadas;

EXCEPTION
    WHEN OTHERS THEN
        RAISE EXCEPTION 'Error al crear aplicación: %', SQLERRM;
END;
$$ LANGUAGE plpgsql;

-- Otorgar permisos de ejecución
GRANT EXECUTE ON FUNCTION sp_aplicaciones_crear(VARCHAR, VARCHAR, TEXT) TO authenticated;
GRANT EXECUTE ON FUNCTION sp_aplicaciones_crear(VARCHAR, VARCHAR, TEXT) TO anon;

-- Comentario para documentación
COMMENT ON FUNCTION sp_aplicaciones_crear(VARCHAR, VARCHAR, TEXT) IS 'Stored procedure para crear una nueva aplicación con tokens de acceso y configuración por defecto';