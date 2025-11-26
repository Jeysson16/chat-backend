-- =============================================
-- Stored Procedures para Sistema de Contactos
-- Descripción: Procedimientos almacenados para gestión completa de contactos
-- Fecha: 2024-12-26
-- Tablas: contactos, usuarios, empresas, aplicaciones
-- =============================================

-- =============================================
-- SP: USP_Contactos_BuscarUsuarios
-- Descripción: Buscar usuarios disponibles para contacto por empresa y aplicación
-- =============================================
CREATE OR REPLACE FUNCTION USP_Contactos_BuscarUsuarios(
    cTerminoBusqueda VARCHAR(255),
    nEmpresaId UUID,
    nAplicacionId UUID,
    cUsuarioSolicitanteId VARCHAR(450),
    cTipoListado VARCHAR(50) DEFAULT 'empresa'
)
RETURNS TABLE(
    cUsuariosId VARCHAR(450),
    cUsuariosNombre VARCHAR(255),
    cUsuariosEmail VARCHAR(255),
    cUsuariosPerCodigo VARCHAR(50),
    cUsuariosPerJurCodigo VARCHAR(50),
    nUsuariosEmpresaId UUID,
    nUsuariosAplicacionId UUID,
    dUsuariosUltimaConexion TIMESTAMPTZ,
    bUsuariosActivo BOOLEAN,
    cContactosEstado VARCHAR(20)
) AS $$
BEGIN
    RETURN QUERY
    WITH usuarios_filtrados AS (
        SELECT 
            u.cUsuariosId,
            u.cUsuariosNombre,
            u.cUsuariosEmail,
            u.cUsuariosPerCodigo,
            u.cUsuariosPerJurCodigo,
            u.nUsuariosEmpresaId,
            u.nUsuariosAplicacionId,
            u.dUsuariosUltimaConexion,
            u.bUsuariosActivo
        FROM usuarios u
        WHERE u.cUsuariosId != cUsuarioSolicitanteId
          AND u.bUsuariosActivo = true
          AND (cTerminoBusqueda IS NULL OR cTerminoBusqueda = '' OR
               u.cUsuariosNombre ILIKE '%' || cTerminoBusqueda || '%' OR
               u.cUsuariosEmail ILIKE '%' || cTerminoBusqueda || '%' OR
               u.cUsuariosPerCodigo ILIKE '%' || cTerminoBusqueda || '%')
          AND CASE 
                WHEN cTipoListado = 'empresa' THEN u.nUsuariosEmpresaId = nEmpresaId
                WHEN cTipoListado = 'aplicacion' THEN u.nUsuariosAplicacionId = nAplicacionId
                WHEN cTipoListado = 'empresa_aplicacion' THEN 
                    u.nUsuariosEmpresaId = nEmpresaId AND u.nUsuariosAplicacionId = nAplicacionId
                ELSE true
              END
    ),
    contactos_existentes AS (
        SELECT 
            CASE 
                WHEN c.cContactosUsuarioSolicitanteId = cUsuarioSolicitanteId 
                THEN c.cContactosUsuarioContactoId
                ELSE c.cContactosUsuarioSolicitanteId
            END as usuarioContactoId,
            c.cContactosEstado
        FROM contactos c
        WHERE c.nContactosEmpresaId = nEmpresaId
          AND c.nContactosAplicacionId = nAplicacionId
          AND c.bContactosActivo = true
          AND (c.cContactosUsuarioSolicitanteId = cUsuarioSolicitanteId OR 
               c.cContactosUsuarioContactoId = cUsuarioSolicitanteId)
    )
    SELECT 
        uf.cUsuariosId,
        uf.cUsuariosNombre,
        uf.cUsuariosEmail,
        uf.cUsuariosPerCodigo,
        uf.cUsuariosPerJurCodigo,
        uf.nUsuariosEmpresaId,
        uf.nUsuariosAplicacionId,
        uf.dUsuariosUltimaConexion,
        uf.bUsuariosActivo,
        COALESCE(ce.cContactosEstado, 'Sin contacto')
    FROM usuarios_filtrados uf
    LEFT JOIN contactos_existentes ce ON uf.cUsuariosId = ce.usuarioContactoId
    ORDER BY uf.cUsuariosNombre;
END;
$$ LANGUAGE plpgsql;

-- =============================================
-- SP: USP_Contactos_EnviarSolicitud
-- Descripción: Enviar solicitud de contacto
-- =============================================
CREATE OR REPLACE FUNCTION USP_Contactos_EnviarSolicitud(
    cContactosUsuarioSolicitanteId VARCHAR(450),
    cContactosUsuarioContactoId VARCHAR(450),
    nContactosEmpresaId UUID,
    nContactosAplicacionId UUID,
    cContactosNotasContacto TEXT DEFAULT NULL,
    bRequiereSolicitud BOOLEAN DEFAULT true,
    bAutoAceptar BOOLEAN DEFAULT false
)
RETURNS TABLE(
    success BOOLEAN,
    nContactosId UUID,
    cContactosEstado VARCHAR(20),
    mensaje VARCHAR(500)
) AS $$
DECLARE
    vContactoId UUID;
    vEstado VARCHAR(20);
    vContactoExistente RECORD;
BEGIN
    -- Verificar si ya existe un contacto
    SELECT * INTO vContactoExistente
    FROM contactos c
    WHERE c.nContactosEmpresaId = nContactosEmpresaId
      AND c.nContactosAplicacionId = nContactosAplicacionId
      AND ((c.cContactosUsuarioSolicitanteId = cContactosUsuarioSolicitanteId AND c.cContactosUsuarioContactoId = cContactosUsuarioContactoId) OR
           (c.cContactosUsuarioSolicitanteId = cContactosUsuarioContactoId AND c.cContactosUsuarioContactoId = cContactosUsuarioSolicitanteId));

    IF FOUND THEN
        RETURN QUERY SELECT false, vContactoExistente.nContactosId, vContactoExistente.cContactosEstado, 'Ya existe un contacto entre estos usuarios'::VARCHAR(500);
        RETURN;
    END IF;

    -- Determinar estado inicial
    IF NOT bRequiereSolicitud OR bAutoAceptar THEN
        vEstado := 'Aceptado';
    ELSE
        vEstado := 'Pendiente';
    END IF;

    -- Crear nuevo contacto
    vContactoId := gen_random_uuid();
    
    INSERT INTO contactos (
        nContactosId,
        nContactosEmpresaId,
        nContactosAplicacionId,
        cContactosUsuarioSolicitanteId,
        cContactosUsuarioContactoId,
        cContactosEstado,
        cContactosNotasContacto,
        bContactosActivo,
        dContactosFechaSolicitud,
        dContactosFechaAceptacion,
        dContactosFechaCreacion,
        dContactosFechaModificacion
    ) VALUES (
        vContactoId,
        nContactosEmpresaId,
        nContactosAplicacionId,
        cContactosUsuarioSolicitanteId,
        cContactosUsuarioContactoId,
        vEstado,
        cContactosNotasContacto,
        true,
        NOW(),
        CASE WHEN vEstado = 'Aceptado' THEN NOW() ELSE NULL END,
        NOW(),
        NOW()
    );

    RETURN QUERY SELECT true, vContactoId, vEstado, 'Solicitud enviada exitosamente'::VARCHAR(500);
END;
$$ LANGUAGE plpgsql;

-- =============================================
-- SP: USP_Contactos_AceptarSolicitud
-- Descripción: Aceptar solicitud de contacto
-- =============================================
CREATE OR REPLACE FUNCTION USP_Contactos_AceptarSolicitud(
    nContactosId UUID,
    cUsuariosId VARCHAR(450)
)
RETURNS TABLE(
    success BOOLEAN,
    mensaje VARCHAR(500)
) AS $$
DECLARE
    vContacto RECORD;
BEGIN
    -- Verificar que el contacto existe y el usuario puede aceptarlo
    SELECT * INTO vContacto
    FROM contactos c
    WHERE c.nContactosId = nContactosId
      AND c.cContactosUsuarioContactoId = cUsuariosId
      AND c.cContactosEstado = 'Pendiente'
      AND c.bContactosActivo = true;

    IF NOT FOUND THEN
        RETURN QUERY SELECT false, 'Solicitud no encontrada o no autorizada'::VARCHAR(500);
        RETURN;
    END IF;

    -- Actualizar estado a aceptado
    UPDATE contactos 
    SET cContactosEstado = 'Aceptado',
        dContactosFechaAceptacion = NOW(),
        dContactosFechaModificacion = NOW()
    WHERE nContactosId = pContactoId;

    RETURN QUERY SELECT true, 'Solicitud aceptada exitosamente'::VARCHAR(500);
END;
$$ LANGUAGE plpgsql;

-- =============================================
-- SP: USP_Contactos_RechazarSolicitud
-- Descripción: Rechazar solicitud de contacto
-- =============================================
CREATE OR REPLACE FUNCTION USP_Contactos_RechazarSolicitud(
    pContactoId UUID,
    pUsuarioId VARCHAR(450)
)
RETURNS TABLE(
    success BOOLEAN,
    mensaje VARCHAR(500)
) AS $$
DECLARE
    vContacto RECORD;
BEGIN
    -- Verificar que el contacto existe y el usuario puede rechazarlo
    SELECT * INTO vContacto
    FROM contactos c
    WHERE c.nContactosId = pContactoId
      AND c.cContactosUsuarioContactoId = pUsuarioId
      AND c.cContactosEstado = 'Pendiente'
      AND c.bContactosActivo = true;

    IF NOT FOUND THEN
        RETURN QUERY SELECT false, 'Solicitud no encontrada o no autorizada'::VARCHAR(500);
        RETURN;
    END IF;

    -- Actualizar estado a rechazado
    UPDATE contactos 
    SET cContactosEstado = 'Rechazado',
        dContactosFechaModificacion = NOW()
    WHERE nContactosId = pContactoId;

    RETURN QUERY SELECT true, 'Solicitud rechazada exitosamente'::VARCHAR(500);
END;
$$ LANGUAGE plpgsql;

-- =============================================
-- SP: USP_Contactos_ListarPorUsuario
-- Descripción: Listar contactos de un usuario
-- =============================================
CREATE OR REPLACE FUNCTION USP_Contactos_ListarPorUsuario(
    pUsuarioId VARCHAR(450),
    pEmpresaId UUID,
    pAplicacionId UUID,
    pEstado VARCHAR(20) DEFAULT NULL
)
RETURNS TABLE(
    nContactosId UUID,
    cContactosUsuarioSolicitanteId VARCHAR(450),
    cContactosUsuarioContactoId VARCHAR(450),
    nContactosEmpresaId UUID,
    nContactosAplicacionId UUID,
    cContactosNombreContacto VARCHAR(255),
    cContactosEmailContacto VARCHAR(255),
    cContactosTelefonoContacto VARCHAR(50),
    cContactosEstado VARCHAR(20),
    dContactosFechaSolicitud TIMESTAMPTZ,
    dContactosFechaAceptacion TIMESTAMPTZ,
    dContactosFechaCreacion TIMESTAMPTZ,
    cContactosNotasContacto TEXT,
    tipoSolicitud VARCHAR(20),
    esSolicitante BOOLEAN
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        c.nContactosId,
        c.cContactosUsuarioSolicitanteId,
        c.cContactosUsuarioContactoId,
        c.nContactosEmpresaId,
        c.nContactosAplicacionId,
        c.cContactosNombreContacto,
        c.cContactosEmailContacto,
        c.cContactosTelefonoContacto,
        c.cContactosEstado,
        c.dContactosFechaSolicitud,
        c.dContactosFechaAceptacion,
        c.dContactosFechaCreacion,
        c.cContactosNotasContacto,
        CASE 
            WHEN c.cContactosUsuarioSolicitanteId = pUsuarioId THEN 'Enviada'
            ELSE 'Recibida'
        END::VARCHAR(20),
        c.cContactosUsuarioSolicitanteId = pUsuarioId
    FROM contactos c
    WHERE c.nContactosEmpresaId = pEmpresaId
      AND c.nContactosAplicacionId = pAplicacionId
      AND c.bContactosActivo = true
      AND (c.cContactosUsuarioSolicitanteId = pUsuarioId OR c.cContactosUsuarioContactoId = pUsuarioId)
      AND (pEstado IS NULL OR c.cContactosEstado = pEstado)
    ORDER BY c.dContactosFechaSolicitud DESC;
END;
$$ LANGUAGE plpgsql;

-- =============================================
-- SP: USP_Contactos_ListarSolicitudesPendientes
-- Descripción: Listar solicitudes pendientes de un usuario
-- =============================================
CREATE OR REPLACE FUNCTION USP_Contactos_ListarSolicitudesPendientes(
    pUsuarioId VARCHAR(450),
    pEmpresaId UUID,
    pAplicacionId UUID
)
RETURNS TABLE(
    nContactosId UUID,
    cContactosUsuarioSolicitanteId VARCHAR(450),
    cUsuariosNombre VARCHAR(255),
    cUsuariosEmail VARCHAR(255),
    dContactosFechaSolicitud TIMESTAMPTZ,
    cContactosNotasContacto TEXT
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        c.nContactosId,
        c.cContactosUsuarioSolicitanteId,
        us.cUsuariosNombre,
        us.cUsuariosEmail,
        c.dContactosFechaSolicitud,
        c.cContactosNotasContacto
    FROM contactos c
    INNER JOIN usuarios us ON c.cContactosUsuarioSolicitanteId = us.cUsuariosId
    WHERE c.cContactosUsuarioContactoId = pUsuarioId
      AND c.nContactosEmpresaId = pEmpresaId
      AND c.nContactosAplicacionId = pAplicacionId
      AND c.cContactosEstado = 'Pendiente'
      AND c.bContactosActivo = true
    ORDER BY c.dContactosFechaSolicitud DESC;
END;
$$ LANGUAGE plpgsql;

-- =============================================
-- SP: USP_Contactos_VerificarPermisoChatDirecto
-- Descripción: Verificar si dos usuarios pueden crear chat directo
-- =============================================
CREATE OR REPLACE FUNCTION USP_Contactos_VerificarPermisoChatDirecto(
    pUsuario1Id VARCHAR(450),
    pUsuario2Id VARCHAR(450),
    pEmpresaId UUID,
    pAplicacionId UUID
)
RETURNS TABLE(
    puedeChatear BOOLEAN,
    cContactosEstado VARCHAR(20),
    mensaje VARCHAR(500)
) AS $$
DECLARE
    vContacto RECORD;
BEGIN
    -- Buscar contacto existente entre los usuarios
    SELECT * INTO vContacto
    FROM contactos c
    WHERE c.nContactosEmpresaId = pEmpresaId
      AND c.nContactosAplicacionId = pAplicacionId
      AND c.bContactosActivo = true
      AND ((c.cContactosUsuarioSolicitanteId = pUsuario1Id AND c.cContactosUsuarioContactoId = pUsuario2Id) OR
           (c.cContactosUsuarioSolicitanteId = pUsuario2Id AND c.cContactosUsuarioContactoId = pUsuario1Id));

    IF NOT FOUND THEN
        RETURN QUERY SELECT false, 'Sin contacto'::VARCHAR(20), 'No existe contacto entre los usuarios'::VARCHAR(500);
        RETURN;
    END IF;

    IF vContacto.cContactosEstado = 'Aceptado' THEN
        RETURN QUERY SELECT true, vContacto.cContactosEstado, 'Usuarios pueden chatear'::VARCHAR(500);
    ELSE
        RETURN QUERY SELECT false, vContacto.cContactosEstado, 'Contacto no está aceptado'::VARCHAR(500);
    END IF;
END;
$$ LANGUAGE plpgsql;

-- =============================================
-- SP: USP_Contactos_BloquearContacto
-- Descripción: Bloquear un contacto
-- =============================================
CREATE OR REPLACE FUNCTION USP_Contactos_BloquearContacto(
    pContactoId UUID,
    pUsuarioId VARCHAR(450)
)
RETURNS TABLE(
    success BOOLEAN,
    mensaje VARCHAR(500)
) AS $$
DECLARE
    vContacto RECORD;
BEGIN
    -- Verificar que el contacto existe y el usuario puede bloquearlo
    SELECT * INTO vContacto
    FROM contactos c
    WHERE c.nContactosId = pContactoId
      AND (c.cContactosUsuarioSolicitanteId = pUsuarioId OR c.cContactosUsuarioContactoId = pUsuarioId)
      AND c.bContactosActivo = true;

    IF NOT FOUND THEN
        RETURN QUERY SELECT false, 'Contacto no encontrado o no autorizado'::VARCHAR(500);
        RETURN;
    END IF;

    -- Actualizar estado a bloqueado
    UPDATE contactos 
    SET cContactosEstado = 'Bloqueado',
        dContactosFechaModificacion = NOW()
    WHERE nContactosId = pContactoId;

    RETURN QUERY SELECT true, 'Contacto bloqueado exitosamente'::VARCHAR(500);
END;
$$ LANGUAGE plpgsql;

-- =============================================
-- SP: USP_Contactos_DesbloquearContacto
-- Descripción: Desbloquear un contacto
-- =============================================
CREATE OR REPLACE FUNCTION USP_Contactos_DesbloquearContacto(
    pContactoId UUID,
    pUsuarioId VARCHAR(450)
)
RETURNS TABLE(
    success BOOLEAN,
    mensaje VARCHAR(500)
) AS $$
DECLARE
    vContacto RECORD;
BEGIN
    -- Verificar que el contacto existe y el usuario puede desbloquearlo
    SELECT * INTO vContacto
    FROM contactos c
    WHERE c.nContactosId = pContactoId
      AND (c.cContactosUsuarioSolicitanteId = pUsuarioId OR c.cContactosUsuarioContactoId = pUsuarioId)
      AND c.cContactosEstado = 'Bloqueado'
      AND c.bContactosActivo = true;

    IF NOT FOUND THEN
        RETURN QUERY SELECT false, 'Contacto bloqueado no encontrado o no autorizado'::VARCHAR(500);
        RETURN;
    END IF;

    -- Actualizar estado a aceptado
    UPDATE contactos 
    SET cContactosEstado = 'Aceptado',
        dContactosFechaModificacion = NOW()
    WHERE nContactosId = pContactoId;

    RETURN QUERY SELECT true, 'Contacto desbloqueado exitosamente'::VARCHAR(500);
END;
$$ LANGUAGE plpgsql;

-- Mensaje de confirmación
SELECT 'Stored procedures para sistema de contactos creados exitosamente' AS resultado;