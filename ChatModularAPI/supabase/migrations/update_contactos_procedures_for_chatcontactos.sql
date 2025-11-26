-- =============================================
-- Updated Stored Procedures for chatcontactos table
-- These procedures use the modern chatcontactos schema
-- =============================================

-- =============================================
-- SP: usp_contactos_listarcontactos (Updated for chatcontactos)
-- Descripción: Listar contactos de un usuario usando chatcontactos
-- =============================================
CREATE OR REPLACE FUNCTION usp_contactos_listarcontactos(
    p_usuario_id VARCHAR(450),
    p_empresa_id UUID,
    p_aplicacion_id UUID,
    p_estado VARCHAR(20) DEFAULT NULL
)
RETURNS TABLE(
    contacto_id UUID,
    usuario_solicitante_id VARCHAR(450),
    usuario_contacto_id VARCHAR(450),
    empresa_id UUID,
    aplicacion_id UUID,
    nombre_contacto VARCHAR(255),
    email_contacto VARCHAR(255),
    telefono_contacto VARCHAR(50),
    estado VARCHAR(20),
    fecha_solicitud TIMESTAMPTZ,
    fecha_aceptacion TIMESTAMPTZ,
    fecha_creacion TIMESTAMPTZ,
    notas TEXT,
    tipo_solicitud VARCHAR(20),
    es_solicitante BOOLEAN
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        c.c_contactos_id::UUID,
        c.c_usuarios_id,
        c.c_contacto_usuarios_id,
        p_empresa_id::UUID,
        p_aplicacion_id::UUID,
        'Usuario ' || c.c_contacto_usuarios_id::VARCHAR(255),
        c.c_contacto_usuarios_id::VARCHAR(255) || '@example.com',
        ''::VARCHAR(50),
        CASE 
            WHEN c.c_contactos_estado = 'Activo' THEN 'Aceptado'
            WHEN c.c_contactos_estado = 'Pendiente' THEN 'Pendiente'
            WHEN c.c_contactos_estado = 'Bloqueado' THEN 'Bloqueado'
            ELSE c.c_contactos_estado
        END::VARCHAR(20),
        c.d_fecha_creacion,
        CASE 
            WHEN c.c_contactos_estado = 'Activo' THEN c.d_fecha_actualizacion
            ELSE NULL
        END,
        c.d_fecha_creacion,
        ''::TEXT,
        CASE 
            WHEN c.c_usuarios_id = p_usuario_id THEN 'Enviada'
            ELSE 'Recibida'
        END::VARCHAR(20),
        c.c_usuarios_id = p_usuario_id
    FROM chatcontactos c
    WHERE c.n_empresas_id = p_empresa_id::TEXT
      AND c.n_aplicaciones_id = p_aplicacion_id::TEXT
      AND (c.c_usuarios_id = p_usuario_id OR c.c_contacto_usuarios_id = p_usuario_id)
      AND (p_estado IS NULL OR 
           (p_estado = 'Aceptado' AND c.c_contactos_estado = 'Activo') OR
           (p_estado = 'Pendiente' AND c.c_contactos_estado = 'Pendiente') OR
           (p_estado = 'Bloqueado' AND c.c_contactos_estado = 'Bloqueado'))
    ORDER BY c.d_fecha_creacion DESC;
END;
$$ LANGUAGE plpgsql;

-- =============================================
-- SP: usp_contactos_listarsolicitudespendientes (Updated for chatcontactos)
-- Descripción: Listar solicitudes pendientes de un usuario
-- =============================================
CREATE OR REPLACE FUNCTION usp_contactos_listarsolicitudespendientes(
    p_usuario_id VARCHAR(450),
    p_empresa_id UUID,
    p_aplicacion_id UUID
)
RETURNS TABLE(
    contacto_id UUID,
    usuario_solicitante_id VARCHAR(450),
    nombre_solicitante VARCHAR(255),
    email_solicitante VARCHAR(255),
    fecha_solicitud TIMESTAMPTZ,
    notas TEXT
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        c.c_contactos_id::UUID,
        c.c_usuarios_id,
        'Usuario ' || c.c_usuarios_id::VARCHAR(255),
        c.c_usuarios_id::VARCHAR(255) || '@example.com',
        c.d_fecha_creacion,
        ''::TEXT
    FROM chatcontactos c
    WHERE c.c_contacto_usuarios_id = p_usuario_id
      AND c.n_empresas_id = p_empresa_id::TEXT
      AND c.n_aplicaciones_id = p_aplicacion_id::TEXT
      AND c.c_contactos_estado = 'Pendiente'
    ORDER BY c.d_fecha_creacion DESC;
END;
$$ LANGUAGE plpgsql;

-- Mensaje de confirmación
SELECT 'Stored procedures actualizados para usar chatcontactos exitosamente' AS resultado;