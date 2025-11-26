-- Stored Procedure: sp_aplicaciones_crear
-- Descripción: Crea una nueva aplicación con tokens de acceso y configuraciones por defecto
-- Parámetros:
--   cAplicacionesNombre: Nombre de la aplicación (requerido)
--   cAplicacionesCodigo: Código único de la aplicación (requerido)
--   cAplicacionesDescripcion: Descripción de la aplicación (opcional)
-- Retorna: Datos de la aplicación creada incluyendo tokens generados

CREATE OR REPLACE FUNCTION sp_aplicaciones_crear(
    cAplicacionesNombre VARCHAR(100),
    cAplicacionesCodigo VARCHAR(50),
    cAplicacionesDescripcion TEXT DEFAULT NULL
)
RETURNS TABLE(
    nAplicacionesId INTEGER,
    cAplicacionesNombre VARCHAR(100),
    cAplicacionesCodigo VARCHAR(50),
    cAppRegistrosTokenAcceso VARCHAR(255),
    cAppRegistrosSecretoApp VARCHAR(255),
    dAplicacionesFechaCreacion TIMESTAMPTZ,
    nConfiguracionesCreadas INTEGER
) AS $$

DECLARE
    nAplicacionesId INTEGER;
    cAppRegistrosTokenAcceso VARCHAR(255);
    cAppRegistrosSecretoApp VARCHAR(255);
    nConfiguracionesCreadas INTEGER;
BEGIN
    -- Verificar que el código de aplicación no exista
    IF EXISTS (SELECT 1 FROM "Aplicaciones" WHERE "cAplicacionesCodigo" = cAplicacionesCodigo) THEN
        RAISE EXCEPTION 'Ya existe una aplicación con el código: %', cAplicacionesCodigo;
    END IF;

    -- Generar tokens únicos
    cAppRegistrosTokenAcceso := 'AT_' || encode(gen_random_bytes(16), 'hex');
    cAppRegistrosSecretoApp := 'ST_' || encode(gen_random_bytes(24), 'hex');

    -- Insertar en la tabla Aplicaciones
    INSERT INTO "Aplicaciones" (
        "cAplicacionesNombre",
        "cAplicacionesDescripcion", 
        "cAplicacionesCodigo",
        "dAplicacionesFechaCreacion",
        "bAplicacionesEsActiva"
    ) VALUES (
        cAplicacionesNombre,
        cAplicacionesDescripcion,
        cAplicacionesCodigo,
        NOW(),
        true
    ) RETURNING "nAplicacionesId" INTO nAplicacionesId;

    -- Insertar en la tabla AppRegistros
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
        cAplicacionesCodigo,
        cAplicacionesNombre,
        cAppRegistrosTokenAcceso,
        cAppRegistrosSecretoApp,
        true,
        NOW(),
        NOW() + INTERVAL '1 year', -- Token válido por 1 año
        '{"version": "1.0", "type": "standard"}'::jsonb
    );

    -- Crear configuraciones por defecto para la aplicación
    INSERT INTO "ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion",
        "bConfiguracionAplicacionEsActiva"
    ) VALUES 
    (nAplicacionesId, 'maxUsuariosConcurrentes', '100', 'Máximo número de usuarios concurrentes permitidos', true),
    (nAplicacionesId, 'tiempoSesionMinutos', '480', 'Tiempo de sesión en minutos (8 horas por defecto)', true),
    (nAplicacionesId, 'permitirChatGrupal', 'true', 'Permite crear chats grupales', true),
    (nAplicacionesId, 'maxArchivosAdjuntos', '10', 'Máximo número de archivos adjuntos por mensaje', true),
    (nAplicacionesId, 'tamanoMaximoArchivoMb', '25', 'Tamaño máximo de archivo en MB', true),
    (nAplicacionesId, 'notificacionesPush', 'true', 'Habilitar notificaciones push', true),
    (nAplicacionesId, 'temaPorDefecto', 'light', 'Tema visual por defecto (light/dark)', true),
    (nAplicacionesId, 'idiomaPorDefecto', 'es', 'Idioma por defecto de la aplicación', true);

    -- Contar configuraciones creadas
    SELECT COUNT(*) INTO nConfiguracionesCreadas 
    FROM "ConfiguracionAplicacion" 
    WHERE "nConfiguracionAplicacionAplicacionId" = nAplicacionesId;

    -- Retornar los datos de la aplicación creada
    RETURN QUERY
    SELECT 
        nAplicacionesId,
        cAplicacionesNombre,
        cAplicacionesCodigo,
        cAppRegistrosTokenAcceso,
        cAppRegistrosSecretoApp,
        NOW(),
        nConfiguracionesCreadas;

END;
$$ LANGUAGE plpgsql;

-- Comentario sobre el procedimiento
COMMENT ON FUNCTION sp_aplicaciones_crear(VARCHAR, VARCHAR, TEXT) IS 
'Stored procedure para crear una nueva aplicación con tokens de acceso y configuración por defecto. 
Genera automáticamente access_token y secret_token únicos, y crea configuraciones predeterminadas para la aplicación.';