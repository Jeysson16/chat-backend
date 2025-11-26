-- =============================================
-- Chat Modular API - Stored Procedures
-- Supabase Migration Script
-- =============================================

-- =============================================
-- SP: sp_auth_login
-- Descripción: Autenticación de usuario
-- =============================================
CREATE OR REPLACE FUNCTION sp_auth_login(
    cChatUsuariosEmail VARCHAR(255),
    cChatUsuariosPassword VARCHAR(255)
)
RETURNS TABLE(
    success BOOLEAN,
    message TEXT,
    user_id UUID,
    user_data JSONB
) 
LANGUAGE plpgsql
SECURITY DEFINER
AS $$
DECLARE
    v_user_id UUID;
    v_password_hash VARCHAR(255);
    v_user_data JSONB;
BEGIN
    -- Buscar usuario por email
    SELECT c, password_hash INTO v_user_id, v_password_hash
    FROM public.chat_usuarios 
    WHERE email = cChatUsuariosEmail AND estado = 'activo';
    
    -- Verificar si el usuario existe
    IF v_user_id IS NULL THEN
        RETURN QUERY SELECT FALSE, 'Usuario no encontrado o inactivo'::TEXT, NULL::UUID, NULL::JSONB;
        RETURN;
    END IF;
    
    -- Verificar contraseña (en producción usar crypt)
    IF v_password_hash != crypt(cChatUsuariosPassword, v_password_hash) THEN
        RETURN QUERY SELECT FALSE, 'Credenciales inválidas'::TEXT, NULL::UUID, NULL::JSONB;
        RETURN;
    END IF;
    
    -- Actualizar último acceso
    UPDATE public.chat_usuarios 
    SET ultimo_acceso = NOW(), updated_at = NOW()
    WHERE c = v_user_id;
    
    -- Obtener datos del usuario
    SELECT jsonb_build_object(
        'id', c,
        'email', email,
        'nombre', nombre,
        'apellido', apellido,
        'telefono', telefono,
        'avatar_url', avatar_url,
        'estado', estado,
        'configuracion_privacidad', configuracion_privacidad
    ) INTO v_user_data
    FROM public.chat_usuarios 
    WHERE c = v_user_id;
    
    RETURN QUERY SELECT TRUE, 'Login exitoso'::TEXT, v_user_id, v_user_data;
END;
$$;

-- =============================================
-- SP: sp_auth_register
-- Descripción: Registro de nuevo usuario
-- =============================================
CREATE OR REPLACE FUNCTION sp_auth_register(
    cChatUsuariosEmail VARCHAR(255),
    cChatUsuariosPassword VARCHAR(255),
    cChatUsuariosNombre VARCHAR(100),
    cChatUsuariosApellido VARCHAR(100) DEFAULT NULL,
    cChatUsuariosTelefono VARCHAR(20) DEFAULT NULL
)
RETURNS TABLE(
    success BOOLEAN,
    message TEXT,
    user_id UUID
) 
LANGUAGE plpgsql
SECURITY DEFINER
AS $$
DECLARE
    v_user_id UUID;
    v_password_hash VARCHAR(255);
BEGIN
    -- Verificar si el email ya existe
    IF EXISTS (SELECT 1 FROM public.chat_usuarios WHERE email = cChatUsuariosEmail) THEN
        RETURN QUERY SELECT FALSE, 'El email ya está registrado'::TEXT, NULL::UUID;
        RETURN;
    END IF;
    
    -- Generar hash de contraseña
    v_password_hash := crypt(cChatUsuariosPassword, gen_salt('bf'));
    
    -- Insertar nuevo usuario
    INSERT INTO public.chat_usuarios (email, password_hash, nombre, apellido, telefono, n)
    VALUES (cChatUsuariosEmail, v_password_hash, cChatUsuariosNombre, cChatUsuariosApellido, cChatUsuariosTelefono, cChatUsuariosNombre || ' ' || COALESCE(cChatUsuariosApellido, ''))
    RETURNING c INTO v_user_id;
    
    RETURN QUERY SELECT TRUE, 'Usuario registrado exitosamente'::TEXT, v_user_id;
END;
$$;

-- =============================================
-- SP: sp_chat_get_conversations
-- Descripción: Obtener conversaciones de un usuario
-- =============================================
CREATE OR REPLACE FUNCTION sp_chat_get_conversations(
    p_user_id UUID,
    p_limit INTEGER DEFAULT 50,
    p_offset INTEGER DEFAULT 0
)
RETURNS TABLE(
    success BOOLEAN,
    message TEXT,
    conversations JSONB
) 
LANGUAGE plpgsql
SECURITY DEFINER
AS $$
DECLARE
    v_conversations JSONB;
BEGIN
    -- Verificar que el usuario existe
    IF NOT EXISTS (SELECT 1 FROM public.chat_usuarios WHERE c = p_user_id) THEN
        RETURN QUERY SELECT FALSE, 'Usuario no encontrado'::TEXT, NULL::JSONB;
        RETURN;
    END IF;
    
    -- Obtener conversaciones del usuario
    SELECT jsonb_agg(
        jsonb_build_object(
            'id', conv.c,
            'nombre', conv.n,
            'tipo', conv.tipo,
            'descripcion', conv.descripcion,
            'imagen_url', conv.imagen_url,
            'estado', conv.estado,
            'fecha_creacion', conv.d,
            'fecha_union', uc.fecha_union,
            'rol', uc.rol,
            'ultimo_mensaje', (
                SELECT jsonb_build_object(
                    'id', m.c,
                    'contenido', m.contenido,
                    'fecha', m.d,
                    'usuario_nombre', u.nombre,
                    'tipo', m.tipo
                )
                FROM public.chat_mensajes m
                JOIN public.chat_usuarios u ON m.usuario_id = u.c
                WHERE m.conversacion_id = conv.c
                ORDER BY m.d DESC
                LIMIT 1
            ),
            'mensajes_no_leidos', (
                SELECT COUNT(*)
                FROM public.chat_mensajes m
                LEFT JOIN public.chat_mensajes_lecturas l ON m.c = l.mensaje_id AND l.usuario_id = p_user_id
                WHERE m.conversacion_id = conv.c 
                AND m.usuario_id != p_user_id
                AND l.mensaje_id IS NULL
            )
        )
    ) INTO v_conversations
    FROM public.chat_conversaciones conv
    JOIN public.chat_usuarios_conversaciones uc ON conv.c = uc.conversacion_id
    WHERE uc.usuario_id = p_user_id 
    AND uc.estado = 'activo'
    AND conv.estado = 'activa'
    ORDER BY conv.updated_at DESC
    LIMIT p_limit OFFSET p_offset;
    
    RETURN QUERY SELECT TRUE, 'Conversaciones obtenidas exitosamente'::TEXT, COALESCE(v_conversations, '[]'::JSONB);
END;
$$;

-- =============================================
-- SP: sp_chat_get_messages
-- Descripción: Obtener mensajes de una conversación
-- =============================================
CREATE OR REPLACE FUNCTION sp_chat_get_messages(
    p_user_id UUID,
    p_conversation_id UUID,
    p_limit INTEGER DEFAULT 50,
    p_offset INTEGER DEFAULT 0,
    p_before_date TIMESTAMP WITH TIME ZONE DEFAULT NULL
)
RETURNS TABLE(
    success BOOLEAN,
    message TEXT,
    messages JSONB
) 
LANGUAGE plpgsql
SECURITY DEFINER
AS $$
DECLARE
    v_messages JSONB;
BEGIN
    -- Verificar que el usuario pertenece a la conversación
    IF NOT EXISTS (
        SELECT 1 FROM public.chat_usuarios_conversaciones 
        WHERE usuario_id = p_user_id 
        AND conversacion_id = p_conversation_id 
        AND estado = 'activo'
    ) THEN
        RETURN QUERY SELECT FALSE, 'No tienes acceso a esta conversación'::TEXT, NULL::JSONB;
        RETURN;
    END IF;
    
    -- Obtener mensajes
    SELECT jsonb_agg(
        jsonb_build_object(
            'id', m.c,
            'contenido', m.contenido,
            'tipo', m.tipo,
            'fecha', m.d,
            'editado', m.editado,
            'fecha_edicion', m.fecha_edicion,
            'estado', m.estado,
            'archivo_url', m.archivo_url,
            'archivo_tipo', m.archivo_tipo,
            'archivo_tamaño', m.archivo_tamaño,
            'metadata', m.metadata,
            'usuario', jsonb_build_object(
                'id', u.c,
                'nombre', u.nombre,
                'apellido', u.apellido,
                'avatar_url', u.avatar_url
            ),
            'mensaje_padre', CASE 
                WHEN m.mensaje_padre_id IS NOT NULL THEN
                    jsonb_build_object(
                        'id', mp.c,
                        'contenido', mp.contenido,
                        'usuario_nombre', up.nombre
                    )
                ELSE NULL
            END,
            'lecturas', (
                SELECT jsonb_agg(
                    jsonb_build_object(
                        'usuario_id', l.usuario_id,
                        'fecha_lectura', l.fecha_lectura,
                        'usuario_nombre', ul.nombre
                    )
                )
                FROM public.chat_mensajes_lecturas l
                JOIN public.chat_usuarios ul ON l.usuario_id = ul.c
                WHERE l.mensaje_id = m.c
            )
        ) ORDER BY m.d ASC
    ) INTO v_messages
    FROM public.chat_mensajes m
    JOIN public.chat_usuarios u ON m.usuario_id = u.c
    LEFT JOIN public.chat_mensajes mp ON m.mensaje_padre_id = mp.c
    LEFT JOIN public.chat_usuarios up ON mp.usuario_id = up.c
    WHERE m.conversacion_id = p_conversation_id
    AND m.estado != 'eliminado'
    AND (p_before_date IS NULL OR m.d < p_before_date)
    ORDER BY m.d DESC
    LIMIT p_limit OFFSET p_offset;
    
    RETURN QUERY SELECT TRUE, 'Mensajes obtenidos exitosamente'::TEXT, COALESCE(v_messages, '[]'::JSONB);
END;
$$;

-- =============================================
-- SP: sp_chat_send_message
-- Descripción: Enviar un mensaje a una conversación
-- =============================================
CREATE OR REPLACE FUNCTION sp_chat_send_message(
    p_user_id UUID,
    p_conversation_id UUID,
    p_contenido TEXT,
    p_tipo VARCHAR(20) DEFAULT 'texto',
    p_archivo_url TEXT DEFAULT NULL,
    p_archivo_tipo VARCHAR(50) DEFAULT NULL,
    p_archivo_tamaño INTEGER DEFAULT NULL,
    p_mensaje_padre_id UUID DEFAULT NULL,
    p_metadata JSONB DEFAULT '{}'
)
RETURNS TABLE(
    success BOOLEAN,
    message TEXT,
    message_id UUID,
    message_data JSONB
) 
LANGUAGE plpgsql
SECURITY DEFINER
AS $$
DECLARE
    v_message_id UUID;
    v_message_data JSONB;
BEGIN
    -- Verificar que el usuario pertenece a la conversación
    IF NOT EXISTS (
        SELECT 1 FROM public.chat_usuarios_conversaciones 
        WHERE usuario_id = p_user_id 
        AND conversacion_id = p_conversation_id 
        AND estado = 'activo'
    ) THEN
        RETURN QUERY SELECT FALSE, 'No tienes acceso a esta conversación'::TEXT, NULL::UUID, NULL::JSONB;
        RETURN;
    END IF;
    
    -- Insertar mensaje
    INSERT INTO public.chat_mensajes (
        conversacion_id, usuario_id, contenido, tipo, archivo_url, 
        archivo_tipo, archivo_tamaño, mensaje_padre_id, metadata, n
    )
    VALUES (
        p_conversation_id, p_user_id, p_contenido, p_tipo, p_archivo_url,
        p_archivo_tipo, p_archivo_tamaño, p_mensaje_padre_id, p_metadata,
        LEFT(p_contenido, 100)
    )
    RETURNING c INTO v_message_id;
    
    -- Actualizar timestamp de la conversación
    UPDATE public.chat_conversaciones 
    SET updated_at = NOW() 
    WHERE c = p_conversation_id;
    
    -- Obtener datos completos del mensaje
    SELECT jsonb_build_object(
        'id', m.c,
        'contenido', m.contenido,
        'tipo', m.tipo,
        'fecha', m.d,
        'archivo_url', m.archivo_url,
        'archivo_tipo', m.archivo_tipo,
        'archivo_tamaño', m.archivo_tamaño,
        'metadata', m.metadata,
        'usuario', jsonb_build_object(
            'id', u.c,
            'nombre', u.nombre,
            'apellido', u.apellido,
            'avatar_url', u.avatar_url
        )
    ) INTO v_message_data
    FROM public.chat_mensajes m
    JOIN public.chat_usuarios u ON m.usuario_id = u.c
    WHERE m.c = v_message_id;
    
    RETURN QUERY SELECT TRUE, 'Mensaje enviado exitosamente'::TEXT, v_message_id, v_message_data;
END;
$$;

-- =============================================
-- SP: sp_chat_mark_messages_read
-- Descripción: Marcar mensajes como leídos
-- =============================================
CREATE OR REPLACE FUNCTION sp_chat_mark_messages_read(
    p_user_id UUID,
    p_conversation_id UUID,
    p_message_ids UUID[] DEFAULT NULL
)
RETURNS TABLE(
    success BOOLEAN,
    message TEXT,
    marked_count INTEGER
) 
LANGUAGE plpgsql
SECURITY DEFINER
AS $$
DECLARE
    v_marked_count INTEGER := 0;
BEGIN
    -- Verificar que el usuario pertenece a la conversación
    IF NOT EXISTS (
        SELECT 1 FROM public.chat_usuarios_conversaciones 
        WHERE usuario_id = p_user_id 
        AND conversacion_id = p_conversation_id 
        AND estado = 'activo'
    ) THEN
        RETURN QUERY SELECT FALSE, 'No tienes acceso a esta conversación'::TEXT, 0;
        RETURN;
    END IF;
    
    -- Marcar mensajes como leídos
    IF p_message_ids IS NOT NULL THEN
        -- Marcar mensajes específicos
        INSERT INTO public.chat_mensajes_lecturas (mensaje_id, usuario_id)
        SELECT unnest(p_message_ids), p_user_id
        ON CONFLICT (mensaje_id, usuario_id) DO NOTHING;
        
        GET DIAGNOSTICS v_marked_count = ROW_COUNT;
    ELSE
        -- Marcar todos los mensajes de la conversación
        INSERT INTO public.chat_mensajes_lecturas (mensaje_id, usuario_id)
        SELECT m.c, p_user_id
        FROM public.chat_mensajes m
        WHERE m.conversacion_id = p_conversation_id
        AND m.usuario_id != p_user_id
        AND NOT EXISTS (
            SELECT 1 FROM public.chat_mensajes_lecturas l 
            WHERE l.mensaje_id = m.c AND l.usuario_id = p_user_id
        );
        
        GET DIAGNOSTICS v_marked_count = ROW_COUNT;
    END IF;
    
    RETURN QUERY SELECT TRUE, 'Mensajes marcados como leídos'::TEXT, v_marked_count;
END;
$$;

-- =============================================
-- SP: sp_app_register
-- Descripción: Registrar evento en audit log
-- =============================================
CREATE OR REPLACE FUNCTION sp_app_register(
    p_ticket VARCHAR(50),
    p_nivel VARCHAR(20),
    p_categoria VARCHAR(100),
    p_mensaje TEXT,
    p_excepcion TEXT DEFAULT NULL,
    p_stack_trace TEXT DEFAULT NULL,
    p_usuario_id UUID DEFAULT NULL,
    p_usuario_nombre VARCHAR(100) DEFAULT NULL,
    p_cliente_ip VARCHAR(45) DEFAULT NULL,
    p_cliente_user_agent TEXT DEFAULT NULL,
    p_request_url TEXT DEFAULT NULL,
    p_request_method VARCHAR(10) DEFAULT NULL,
    p_request_body TEXT DEFAULT NULL,
    p_response_status INTEGER DEFAULT NULL,
    p_response_time_ms INTEGER DEFAULT NULL,
    p_metadata JSONB DEFAULT '{}'
)
RETURNS TABLE(
    success BOOLEAN,
    message TEXT,
    log_id UUID
) 
LANGUAGE plpgsql
SECURITY DEFINER
AS $$
DECLARE
    v_log_id UUID;
BEGIN
    -- Insertar registro de audit log
    INSERT INTO public.app_registros (
        ticket, nivel, categoria, mensaje, excepcion, stack_trace,
        usuario_id, usuario_nombre, cliente_ip, cliente_user_agent,
        request_url, request_method, request_body, response_status,
        response_time_ms, metadata, n
    )
    VALUES (
        p_ticket, p_nivel, p_categoria, p_mensaje, p_excepcion, p_stack_trace,
        p_usuario_id, p_usuario_nombre, p_cliente_ip, p_cliente_user_agent,
        p_request_url, p_request_method, p_request_body, p_response_status,
        p_response_time_ms, p_metadata, p_categoria || ': ' || LEFT(p_mensaje, 100)
    )
    RETURNING c INTO v_log_id;
    
    RETURN QUERY SELECT TRUE, 'Registro creado exitosamente'::TEXT, v_log_id;
END;
$$;

-- =============================================
-- SP: sp_webhook_register
-- Descripción: Registrar webhook para envío
-- =============================================
CREATE OR REPLACE FUNCTION sp_webhook_register(
    p_url TEXT,
    p_evento VARCHAR(100),
    p_payload JSONB,
    p_headers JSONB DEFAULT '{}',
    p_max_intentos INTEGER DEFAULT 3
)
RETURNS TABLE(
    success BOOLEAN,
    message TEXT,
    webhook_id UUID
) 
LANGUAGE plpgsql
SECURITY DEFINER
AS $$
DECLARE
    v_webhook_id UUID;
BEGIN
    -- Insertar registro de webhook
    INSERT INTO public.webhooks_registros (
        url, evento, payload, headers, max_intentos, n
    )
    VALUES (
        p_url, p_evento, p_payload, p_headers, p_max_intentos,
        p_evento || ' -> ' || LEFT(p_url, 100)
    )
    RETURNING c INTO v_webhook_id;
    
    RETURN QUERY SELECT TRUE, 'Webhook registrado exitosamente'::TEXT, v_webhook_id;
END;
$$;

-- =============================================
-- SP: sp_persona_sync
-- Descripción: Sincronizar datos de persona
-- =============================================
CREATE OR REPLACE FUNCTION sp_persona_sync(
    p_user_id UUID,
    p_datos_persona JSONB
)
RETURNS TABLE(
    success BOOLEAN,
    message TEXT,
    user_data JSONB
) 
LANGUAGE plpgsql
SECURITY DEFINER
AS $$
DECLARE
    v_user_data JSONB;
BEGIN
    -- Verificar que el usuario existe
    IF NOT EXISTS (SELECT 1 FROM public.chat_usuarios WHERE c = p_user_id) THEN
        RETURN QUERY SELECT FALSE, 'Usuario no encontrado'::TEXT, NULL::JSONB;
        RETURN;
    END IF;
    
    -- Actualizar datos del usuario
    UPDATE public.chat_usuarios 
    SET 
        nombre = COALESCE(p_datos_persona->>'nombre', nombre),
        apellido = COALESCE(p_datos_persona->>'apellido', apellido),
        telefono = COALESCE(p_datos_persona->>'telefono', telefono),
        avatar_url = COALESCE(p_datos_persona->>'avatar_url', avatar_url),
        configuracion_privacidad = COALESCE(
            p_datos_persona->'configuracion_privacidad', 
            configuracion_privacidad
        ),
        updated_at = NOW()
    WHERE c = p_user_id;
    
    -- Obtener datos actualizados
    SELECT jsonb_build_object(
        'id', c,
        'email', email,
        'nombre', nombre,
        'apellido', apellido,
        'telefono', telefono,
        'avatar_url', avatar_url,
        'estado', estado,
        'configuracion_privacidad', configuracion_privacidad,
        'ultimo_acceso', ultimo_acceso
    ) INTO v_user_data
    FROM public.chat_usuarios 
    WHERE c = p_user_id;
    
    RETURN QUERY SELECT TRUE, 'Datos sincronizados exitosamente'::TEXT, v_user_data;
END;
$$;