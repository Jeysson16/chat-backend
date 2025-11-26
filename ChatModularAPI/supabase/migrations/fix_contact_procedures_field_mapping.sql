-- Fix stored procedures to match backend expected field names
-- This resolves the field mapping issues in ContactoDto

-- Drop existing functions first
DROP FUNCTION IF EXISTS usp_contactos_listarcontactos(TEXT, TEXT, TEXT, TEXT);
DROP FUNCTION IF EXISTS usp_contactos_listarsolicitudespendientes(TEXT, TEXT, TEXT);

-- Create fixed versions with proper field mapping for ContactoDto
CREATE OR REPLACE FUNCTION usp_contactos_listarcontactos(
    p_usuario_id TEXT,
    p_empresa_id TEXT,
    p_aplicacion_id TEXT,
    p_estado TEXT DEFAULT NULL
)
RETURNS TABLE(
    cContactosId TEXT,
    cUsuariosId TEXT,
    cContactoUsuariosId TEXT,
    cContactosEstado TEXT,
    dContactosCreacion TIMESTAMP WITH TIME ZONE,
    dContactosActualizacion TIMESTAMP WITH TIME ZONE,
    nContactosEmpresaId INTEGER,
    nContactosAplicacionId INTEGER
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        c.c_contactos_id::TEXT as cContactosId,
        c.c_usuarios_id::TEXT as cUsuariosId,
        c.c_contacto_usuarios_id::TEXT as cContactoUsuariosId,
        c.c_contactos_estado::TEXT as cContactosEstado,
        c.d_fecha_creacion as dContactosCreacion,
        COALESCE(c.d_fecha_actualizacion, c.d_fecha_creacion) as dContactosActualizacion,
        CASE 
            WHEN c.n_empresas_id ~ '^[0-9]+$' THEN c.n_empresas_id::INTEGER
            ELSE 0
        END as nContactosEmpresaId,
        CASE 
            WHEN c.n_aplicaciones_id ~ '^[0-9]+$' THEN c.n_aplicaciones_id::INTEGER
            ELSE 0
        END as nContactosAplicacionId
    FROM chatcontactos c
    WHERE (c.c_usuarios_id = p_usuario_id OR c.c_contacto_usuarios_id = p_usuario_id)
        AND c.n_empresas_id = p_empresa_id
        AND c.n_aplicaciones_id = p_aplicacion_id
        AND (p_estado IS NULL OR c.c_contactos_estado = p_estado)
    ORDER BY c.d_fecha_creacion DESC;
END;
$$ LANGUAGE plpgsql;

-- Procedure to list pending contact requests with proper field mapping
CREATE OR REPLACE FUNCTION usp_contactos_listarsolicitudespendientes(
    p_usuario_id TEXT,
    p_empresa_id TEXT,
    p_aplicacion_id TEXT
)
RETURNS TABLE(
    cSolicitudId TEXT,
    cSolicitudUsuarioSolicitanteId TEXT,
    cSolicitudUsuarioSolicitanteNombre TEXT,
    cSolicitudUsuarioSolicitanteEmail TEXT,
    cSolicitudUsuarioSolicitanteAvatar TEXT,
    cSolicitudUsuarioDestinoId TEXT,
    cSolicitudMensaje TEXT,
    cSolicitudEstado TEXT,
    dSolicitudCreacion TIMESTAMP WITH TIME ZONE,
    dSolicitudRespuesta TIMESTAMP WITH TIME ZONE
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        s.c_contactos_id::TEXT as cSolicitudId,
        s.c_usuarios_id::TEXT as cSolicitudUsuarioSolicitanteId,
        'Usuario ' || s.c_usuarios_id as cSolicitudUsuarioSolicitanteNombre,
        s.c_usuarios_id || '@example.com' as cSolicitudUsuarioSolicitanteEmail,
        '' as cSolicitudUsuarioSolicitanteAvatar,
        s.c_contacto_usuarios_id::TEXT as cSolicitudUsuarioDestinoId,
        'Solicitud de contacto' as cSolicitudMensaje,
        s.c_contactos_estado::TEXT as cSolicitudEstado,
        s.d_fecha_creacion as dSolicitudCreacion,
        s.d_fecha_actualizacion as dSolicitudRespuesta
    FROM chatcontactos s
    WHERE s.c_contacto_usuarios_id = p_usuario_id
        AND s.n_empresas_id = p_empresa_id
        AND s.n_aplicaciones_id = p_aplicacion_id
        AND s.c_contactos_estado = 'Pendiente'
    ORDER BY s.d_fecha_creacion DESC;
END;
$$ LANGUAGE plpgsql;

-- Grant permissions to the procedures
GRANT EXECUTE ON FUNCTION usp_contactos_listarcontactos(TEXT, TEXT, TEXT, TEXT) TO anon;
GRANT EXECUTE ON FUNCTION usp_contactos_listarcontactos(TEXT, TEXT, TEXT, TEXT) TO authenticated;
GRANT EXECUTE ON FUNCTION usp_contactos_listarsolicitudespendientes(TEXT, TEXT, TEXT) TO anon;
GRANT EXECUTE ON FUNCTION usp_contactos_listarsolicitudespendientes(TEXT, TEXT, TEXT) TO authenticated;