-- Drop existing function and recreate with parameter names matching backend fallback
DROP FUNCTION IF EXISTS public.sp_aplicaciones_crear(character varying, character varying, text);

CREATE OR REPLACE FUNCTION sp_aplicaciones_crear(
    p_caplicacionesnombre VARCHAR(100),
    p_caplicacionescodigo VARCHAR(50),
    p_caplicacionesdescripcion TEXT DEFAULT NULL
)
RETURNS TABLE(
    naplicacionesid INTEGER,
    caplicacionesnombre VARCHAR(100),
    caplicacionescodigo VARCHAR(50),
    cappregistrostokenacceso VARCHAR(255),
    cappregistrossecretoapp VARCHAR(255),
    daplicacionesfechacreacion TIMESTAMPTZ,
    nconfiguracionescreadas INTEGER
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_nAplicacionesId INTEGER;
    v_cAppRegistrosTokenAcceso VARCHAR(255);
    v_cAppRegistrosSecretoApp VARCHAR(255);
    v_nConfiguracionesCreadas INTEGER;
BEGIN
    -- Ensure application code is unique
    IF EXISTS (SELECT 1 FROM "Aplicaciones" WHERE "cAplicacionesCodigo" = p_caplicacionescodigo) THEN
        RAISE EXCEPTION 'Ya existe una aplicación con el código: %', p_caplicacionescodigo;
    END IF;

    -- Generate tokens
    v_cAppRegistrosTokenAcceso := 'AT_' || encode(gen_random_bytes(16), 'hex');
    v_cAppRegistrosSecretoApp := 'ST_' || encode(gen_random_bytes(24), 'hex');

    -- Insert application
    INSERT INTO "Aplicaciones" (
        "cAplicacionesNombre",
        "cAplicacionesCodigo",
        "cAplicacionesDescripcion",
        "bAplicacionesEsActiva",
        "dAplicacionesFechaCreacion"
    ) VALUES (
        p_caplicacionesnombre,
        p_caplicacionescodigo,
        COALESCE(p_caplicacionesdescripcion, ''),
        TRUE,
        NOW()
    ) RETURNING "nAplicacionesId" INTO v_nAplicacionesId;

    -- Insert application tokens record (meeting NOT NULL constraints)
    INSERT INTO "AppRegistros" (
        "nAppRegistrosAplicacionId",
        "cAppRegistrosCodigoApp",
        "cAppRegistrosNombreApp",
        "cAppRegistrosTokenAcceso",
        "cAppRegistrosSecretoApp",
        "bAppRegistrosEsActivo",
        "dAppRegistrosFechaCreacion",
        "dAppRegistrosFechaExpiracion",
        "jAppRegistrosConfiguracionesAdicionales"
    ) VALUES (
        v_nAplicacionesId,
        p_caplicacionescodigo,
        p_caplicacionesnombre,
        v_cAppRegistrosTokenAcceso,
        v_cAppRegistrosSecretoApp,
        TRUE,
        NOW(),
        NOW() + INTERVAL '1 year',
        '{}'
    );

    -- Default configurations (minimal)
    v_nConfiguracionesCreadas := 0;

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
        'Adjuntos habilitados por defecto',
        TRUE,
        NOW(),
        NOW()
    );
    v_nConfiguracionesCreadas := v_nConfiguracionesCreadas + 1;

    -- Return created application info
    RETURN QUERY
    SELECT 
        v_nAplicacionesId,
        p_caplicacionesnombre,
        p_caplicacionescodigo,
        v_cAppRegistrosTokenAcceso,
        v_cAppRegistrosSecretoApp,
        NOW()::TIMESTAMPTZ,
        v_nConfiguracionesCreadas;
END;
$$;

GRANT EXECUTE ON FUNCTION sp_aplicaciones_crear(VARCHAR, VARCHAR, TEXT) TO authenticated;
GRANT EXECUTE ON FUNCTION sp_aplicaciones_crear(VARCHAR, VARCHAR, TEXT) TO anon;
