-- =============================================
-- Stored Procedures para ConfiguracionAplicacion
-- Gestión de configuración unificada por aplicación
-- Versión: 3.0 - Actualizado para coincidir con estructura real de tabla
-- Fecha: 2025-01-01
-- =============================================

-- =============================================
-- USP_ConfiguracionAplicacion_Obtener
-- Obtiene la configuración completa de una aplicación
-- =============================================
CREATE OR REPLACE FUNCTION USP_ConfiguracionAplicacion_Obtener(
    p_AplicacionId INTEGER
)
RETURNS TABLE(
    nConfiguracionAplicacionId INTEGER,
    nAplicacionesId INTEGER,
    -- Archivos adjuntos
    nMaxTamanoArchivo INTEGER,
    cTiposArchivosPermitidos TEXT,
    bPermitirAdjuntos BOOLEAN,
    nMaxCantidadAdjuntos INTEGER,
    bPermitirVisualizacionAdjuntos BOOLEAN,
    -- Chat
    nMaxLongitudMensaje INTEGER,
    bPermitirEmojis BOOLEAN,
    bPermitirMensajesVoz BOOLEAN,
    -- Notificaciones
    bPermitirNotificaciones BOOLEAN,
    -- Seguridad
    bRequiereAutenticacion BOOLEAN,
    bPermitirMensajesAnonimos BOOLEAN,
    nTiempoExpiracionSesion INTEGER,
    -- Metadatos
    dFechaCreacion TIMESTAMPTZ,
    dFechaActualizacion TIMESTAMPTZ,
    bEsActiva BOOLEAN
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        ca."nConfiguracionAplicacionId",
        ca."nAplicacionesId",
        -- Archivos adjuntos
        ca."nMaxTamanoArchivo",
        ca."cTiposArchivosPermitidos",
        ca."bPermitirAdjuntos",
        ca."nMaxCantidadAdjuntos",
        ca."bPermitirVisualizacionAdjuntos",
        -- Chat
        ca."nMaxLongitudMensaje",
        ca."bPermitirEmojis",
        ca."bPermitirMensajesVoz",
        -- Notificaciones
        ca."bPermitirNotificaciones",
        -- Seguridad
        ca."bRequiereAutenticacion",
        ca."bPermitirMensajesAnonimos",
        ca."nTiempoExpiracionSesion",
        -- Metadatos
        ca."dFechaCreacion",
        ca."dFechaActualizacion",
        ca."bEsActiva"
    FROM "ConfiguracionAplicacion" ca
    WHERE ca."nAplicacionesId" = p_AplicacionId
      AND ca."bEsActiva" = true;
END;
$$ LANGUAGE plpgsql;

-- =============================================
-- USP_ConfiguracionAplicacion_Crear
-- Crea una nueva configuración de aplicación con valores por defecto
-- =============================================
CREATE OR REPLACE FUNCTION USP_ConfiguracionAplicacion_Crear(
    p_AplicacionId INTEGER
)
RETURNS INTEGER AS $$
DECLARE
    v_ConfiguracionId INTEGER;
BEGIN
    INSERT INTO "ConfiguracionAplicacion" (
        "nAplicacionesId"
    ) VALUES (
        p_AplicacionId
    ) RETURNING "nConfiguracionAplicacionId" INTO v_ConfiguracionId;

    RETURN v_ConfiguracionId;
EXCEPTION
    WHEN OTHERS THEN
        RETURN -1;
END;
$$ LANGUAGE plpgsql;

-- =============================================
-- USP_ConfiguracionAplicacion_Actualizar
-- Actualiza configuración específica de una aplicación
-- =============================================
CREATE OR REPLACE FUNCTION USP_ConfiguracionAplicacion_Actualizar(
    p_AplicacionId INTEGER,
    p_ConfiguracionJson JSONB
)
RETURNS BOOLEAN AS $$
DECLARE
    v_Existe BOOLEAN;
BEGIN
    -- Verificar si existe la configuración
    SELECT EXISTS(
        SELECT 1 FROM "ConfiguracionAplicacion" 
        WHERE "nAplicacionesId" = p_AplicacionId
          AND "bEsActiva" = true
    ) INTO v_Existe;

    IF NOT v_Existe THEN
        -- Crear configuración si no existe
        PERFORM USP_ConfiguracionAplicacion_Crear(p_AplicacionId);
    END IF;

    -- Actualizar configuración usando JSONB
    UPDATE "ConfiguracionAplicacion" 
    SET 
        -- Archivos adjuntos
        "nMaxTamanoArchivo" = COALESCE((p_ConfiguracionJson->>'nMaxTamanoArchivo')::INTEGER, "nMaxTamanoArchivo"),
        "cTiposArchivosPermitidos" = COALESCE(p_ConfiguracionJson->>'cTiposArchivosPermitidos', "cTiposArchivosPermitidos"),
        "bPermitirAdjuntos" = COALESCE((p_ConfiguracionJson->>'bPermitirAdjuntos')::BOOLEAN, "bPermitirAdjuntos"),
        "nMaxCantidadAdjuntos" = COALESCE((p_ConfiguracionJson->>'nMaxCantidadAdjuntos')::INTEGER, "nMaxCantidadAdjuntos"),
        "bPermitirVisualizacionAdjuntos" = COALESCE((p_ConfiguracionJson->>'bPermitirVisualizacionAdjuntos')::BOOLEAN, "bPermitirVisualizacionAdjuntos"),
        -- Chat
        "nMaxLongitudMensaje" = COALESCE((p_ConfiguracionJson->>'nMaxLongitudMensaje')::INTEGER, "nMaxLongitudMensaje"),
        "bPermitirEmojis" = COALESCE((p_ConfiguracionJson->>'bPermitirEmojis')::BOOLEAN, "bPermitirEmojis"),
        "bPermitirMensajesVoz" = COALESCE((p_ConfiguracionJson->>'bPermitirMensajesVoz')::BOOLEAN, "bPermitirMensajesVoz"),
        -- Notificaciones
        "bPermitirNotificaciones" = COALESCE((p_ConfiguracionJson->>'bPermitirNotificaciones')::BOOLEAN, "bPermitirNotificaciones"),
        -- Seguridad
        "bRequiereAutenticacion" = COALESCE((p_ConfiguracionJson->>'bRequiereAutenticacion')::BOOLEAN, "bRequiereAutenticacion"),
        "bPermitirMensajesAnonimos" = COALESCE((p_ConfiguracionJson->>'bPermitirMensajesAnonimos')::BOOLEAN, "bPermitirMensajesAnonimos"),
        "nTiempoExpiracionSesion" = COALESCE((p_ConfiguracionJson->>'nTiempoExpiracionSesion')::INTEGER, "nTiempoExpiracionSesion"),
        -- Metadatos
        "dFechaActualizacion" = NOW()
    WHERE "nAplicacionesId" = p_AplicacionId
      AND "bEsActiva" = true;

    RETURN FOUND;
EXCEPTION
    WHEN OTHERS THEN
        RETURN FALSE;
END;
$$ LANGUAGE plpgsql;

-- =============================================
-- USP_ConfiguracionAplicacion_ObtenerAdjuntos
-- Obtiene solo la configuración relacionada con adjuntos
-- =============================================
CREATE OR REPLACE FUNCTION USP_ConfiguracionAplicacion_ObtenerAdjuntos(
    p_AplicacionId INTEGER
)
RETURNS TABLE(
    nMaxTamanoArchivo INTEGER,
    cTiposArchivosPermitidos TEXT,
    bPermitirAdjuntos BOOLEAN,
    nMaxCantidadAdjuntos INTEGER,
    bPermitirVisualizacionAdjuntos BOOLEAN
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        ca."nMaxTamanoArchivo",
        ca."cTiposArchivosPermitidos",
        ca."bPermitirAdjuntos",
        ca."nMaxCantidadAdjuntos",
        ca."bPermitirVisualizacionAdjuntos"
    FROM "ConfiguracionAplicacion" ca
    WHERE ca."nAplicacionesId" = p_AplicacionId
      AND ca."bEsActiva" = true;
END;
$$ LANGUAGE plpgsql;

-- =============================================
-- USP_ConfiguracionAplicacion_Eliminar
-- Elimina (desactiva) una configuración de aplicación
-- =============================================
CREATE OR REPLACE FUNCTION USP_ConfiguracionAplicacion_Eliminar(
    p_AplicacionId INTEGER
)
RETURNS BOOLEAN AS $$
BEGIN
    UPDATE "ConfiguracionAplicacion" 
    SET 
        "bEsActiva" = false,
        "dFechaActualizacion" = NOW()
    WHERE "nAplicacionesId" = p_AplicacionId;

    RETURN FOUND;
EXCEPTION
    WHEN OTHERS THEN
        RETURN FALSE;
END;
$$ LANGUAGE plpgsql;

-- =============================================
-- Comentarios para documentación
-- =============================================
COMMENT ON FUNCTION USP_ConfiguracionAplicacion_Obtener IS 'Obtiene la configuración completa de una aplicación por ID';
COMMENT ON FUNCTION USP_ConfiguracionAplicacion_Crear IS 'Crea una nueva configuración de aplicación con valores por defecto';
COMMENT ON FUNCTION USP_ConfiguracionAplicacion_Actualizar IS 'Actualiza configuración específica usando JSONB';
COMMENT ON FUNCTION USP_ConfiguracionAplicacion_ObtenerAdjuntos IS 'Obtiene solo la configuración relacionada con adjuntos';
COMMENT ON FUNCTION USP_ConfiguracionAplicacion_Eliminar IS 'Elimina (desactiva) una configuración de aplicación';

-- =============================================
-- Permisos
-- =============================================
GRANT EXECUTE ON FUNCTION USP_ConfiguracionAplicacion_Obtener TO authenticated;
GRANT EXECUTE ON FUNCTION USP_ConfiguracionAplicacion_Crear TO authenticated;
GRANT EXECUTE ON FUNCTION USP_ConfiguracionAplicacion_Actualizar TO authenticated;
GRANT EXECUTE ON FUNCTION USP_ConfiguracionAplicacion_ObtenerAdjuntos TO authenticated;
GRANT EXECUTE ON FUNCTION USP_ConfiguracionAplicacion_Eliminar TO authenticated;