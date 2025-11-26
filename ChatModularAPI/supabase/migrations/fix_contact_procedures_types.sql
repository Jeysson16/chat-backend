-- Fix data type compatibility issues in contact stored procedures
-- Drop existing functions first
DROP FUNCTION IF EXISTS usp_contactos_listarcontactos(VARCHAR, UUID, UUID, VARCHAR);
DROP FUNCTION IF EXISTS usp_contactos_listarsolicitudespendientes(VARCHAR, UUID, UUID);

-- Create fixed versions with proper type casting
CREATE OR REPLACE FUNCTION usp_contactos_listarcontactos(
    p_usuario_id TEXT,
    p_empresa_id TEXT,
    p_aplicacion_id TEXT,
    p_estado TEXT DEFAULT NULL
)
RETURNS TABLE(
    contacto_id TEXT,
    usuario_solicitante_id TEXT,
    usuario_contacto_id TEXT,
    contacto_estado TEXT,
    fecha_creacion TIMESTAMP WITH TIME ZONE,
    fecha_modificacion TIMESTAMP WITH TIME ZONE,
    empresa_id INTEGER,
    aplicacion_id INTEGER
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        c.c_contactos_id::TEXT as contacto_id,
        c.c_usuarios_id::TEXT as usuario_solicitante_id,
        c.c_contacto_usuarios_id::TEXT as usuario_contacto_id,
        c.c_contactos_estado::TEXT as contacto_estado,
        c.d_fecha_creacion as fecha_creacion,
        COALESCE(c.d_fecha_actualizacion, c.d_fecha_creacion) as fecha_modificacion,
        CASE 
            WHEN c.n_empresas_id ~ '^[0-9]+$' THEN c.n_empresas_id::INTEGER
            ELSE 0
        END as empresa_id,
        CASE 
            WHEN c.n_aplicaciones_id ~ '^[0-9]+$' THEN c.n_aplicaciones_id::INTEGER
            ELSE 0
        END as aplicacion_id
    FROM chatcontactos c
    WHERE (c.c_usuarios_id = p_usuario_id OR c.c_contacto_usuarios_id = p_usuario_id)
        AND c.n_empresas_id = p_empresa_id
        AND c.n_aplicaciones_id = p_aplicacion_id
        AND (p_estado IS NULL OR c.c_contactos_estado = p_estado)
    ORDER BY c.d_fecha_creacion DESC;
END;
$$ LANGUAGE plpgsql;

-- Procedure to list pending contact requests
CREATE OR REPLACE FUNCTION usp_contactos_listarsolicitudespendientes(
    p_usuario_id TEXT,
    p_empresa_id TEXT,
    p_aplicacion_id TEXT
)
RETURNS TABLE(
    solicitud_id TEXT,
    usuario_solicitante_id TEXT,
    usuario_solicitante_nombre TEXT,
    usuario_solicitante_email TEXT,
    usuario_solicitante_avatar TEXT,
    usuario_destino_id TEXT,
    mensaje_solicitud TEXT,
    estado_solicitud TEXT,
    fecha_solicitud TIMESTAMP WITH TIME ZONE,
    fecha_respuesta TIMESTAMP WITH TIME ZONE
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        s.c_contactos_id::TEXT as solicitud_id,
        s.c_usuarios_id::TEXT as usuario_solicitante_id,
        'Usuario ' || s.c_usuarios_id as usuario_solicitante_nombre,
        s.c_usuarios_id || '@example.com' as usuario_solicitante_email,
        '' as usuario_solicitante_avatar,
        s.c_contacto_usuarios_id::TEXT as usuario_destino_id,
        'Solicitud de contacto' as mensaje_solicitud,
        s.c_contactos_estado::TEXT as estado_solicitud,
        s.d_fecha_creacion as fecha_solicitud,
        s.d_fecha_actualizacion as fecha_respuesta
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