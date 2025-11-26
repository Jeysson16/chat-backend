-- Corregir nomenclatura de parámetros en stored procedures
-- Eliminar funciones existentes y recrearlas con nomenclatura correcta

-- Eliminar funciones existentes
DROP FUNCTION IF EXISTS public.USP_Contactos_Crear(UUID, UUID, VARCHAR, VARCHAR, VARCHAR, VARCHAR, VARCHAR, TEXT);
DROP FUNCTION IF EXISTS public.USP_Contactos_Aceptar(UUID, VARCHAR);
DROP FUNCTION IF EXISTS public.USP_Contactos_Rechazar(UUID, VARCHAR);
DROP FUNCTION IF EXISTS public.USP_Contactos_Bloquear(UUID, VARCHAR);

-- Recrear función USP_Contactos_Crear con nomenclatura correcta
CREATE OR REPLACE FUNCTION USP_Contactos_Crear(
    nContactosEmpresaId UUID,
    nContactosAplicacionId UUID,
    cContactosUsuarioSolicitanteId VARCHAR(450),
    cContactosUsuarioContactoId VARCHAR(450),
    cContactosNombreContacto VARCHAR(255),
    cContactosEmailContacto VARCHAR(255),
    cContactosTelefonoContacto VARCHAR(20),
    cContactosNotasContacto TEXT
)
RETURNS UUID
LANGUAGE plpgsql
AS $$
DECLARE
    nContactosIdNuevo UUID;
    nContactosIdExistente UUID;
BEGIN
    -- Verificar si ya existe un contacto entre estos usuarios
    SELECT nContactosId INTO nContactosIdExistente
    FROM Contactos
    WHERE nContactosEmpresaId = nContactosEmpresaId
    AND nContactosAplicacionId = nContactosAplicacionId
    AND ((cContactosUsuarioSolicitanteId = cContactosUsuarioSolicitanteId AND cContactosUsuarioContactoId = cContactosUsuarioContactoId)
         OR (cContactosUsuarioSolicitanteId = cContactosUsuarioContactoId AND cContactosUsuarioContactoId = cContactosUsuarioSolicitanteId))
    AND bContactosActivo = true;

    IF nContactosIdExistente IS NOT NULL THEN
        RAISE EXCEPTION 'Ya existe una relación de contacto entre estos usuarios';
    END IF;

    -- Crear nuevo contacto
    nContactosIdNuevo := gen_random_uuid();
    
    INSERT INTO Contactos (
        nContactosId,
        nContactosEmpresaId,
        nContactosAplicacionId,
        cContactosUsuarioSolicitanteId,
        cContactosUsuarioContactoId,
        cContactosEstado,
        cContactosNombreContacto,
        cContactosEmailContacto,
        cContactosTelefonoContacto,
        cContactosNotasContacto,
        bContactosActivo,
        dContactosFechaSolicitud,
        dContactosFechaCreacion,
        dContactosFechaModificacion
    ) VALUES (
        nContactosIdNuevo,
        nContactosEmpresaId,
        nContactosAplicacionId,
        cContactosUsuarioSolicitanteId,
        cContactosUsuarioContactoId,
        'Pendiente',
        cContactosNombreContacto,
        cContactosEmailContacto,
        cContactosTelefonoContacto,
        cContactosNotasContacto,
        true,
        NOW(),
        NOW(),
        NOW()
    );

    RETURN nContactosIdNuevo;
END;
$$;

-- Recrear función USP_Contactos_Aceptar con nomenclatura correcta
CREATE OR REPLACE FUNCTION USP_Contactos_Aceptar(
    nContactosId UUID,
    cUsuariosId VARCHAR(450)
)
RETURNS BOOLEAN
LANGUAGE plpgsql
AS $$
DECLARE
    nFilasAfectadas INTEGER;
BEGIN
    UPDATE Contactos
    SET cContactosEstado = 'Aceptado',
        dContactosFechaAceptacion = NOW(),
        dContactosFechaModificacion = NOW()
    WHERE nContactosId = nContactosId
    AND cContactosUsuarioContactoId = cUsuariosId
    AND cContactosEstado = 'Pendiente'
    AND bContactosActivo = true;

    GET DIAGNOSTICS nFilasAfectadas = ROW_COUNT;
    RETURN nFilasAfectadas > 0;
END;
$$;

-- Recrear función USP_Contactos_Rechazar con nomenclatura correcta
CREATE OR REPLACE FUNCTION USP_Contactos_Rechazar(
    nContactosId UUID,
    cUsuariosId VARCHAR(450)
)
RETURNS BOOLEAN
LANGUAGE plpgsql
AS $$
DECLARE
    nFilasAfectadas INTEGER;
BEGIN
    UPDATE Contactos
    SET cContactosEstado = 'Rechazado',
        dContactosFechaModificacion = NOW()
    WHERE nContactosId = nContactosId
    AND cContactosUsuarioContactoId = cUsuariosId
    AND cContactosEstado = 'Pendiente'
    AND bContactosActivo = true;

    GET DIAGNOSTICS nFilasAfectadas = ROW_COUNT;
    RETURN nFilasAfectadas > 0;
END;
$$;

-- Recrear función USP_Contactos_Bloquear con nomenclatura correcta
CREATE OR REPLACE FUNCTION USP_Contactos_Bloquear(
    nContactosId UUID,
    cUsuariosId VARCHAR(450)
)
RETURNS BOOLEAN
LANGUAGE plpgsql
AS $$
DECLARE
    nFilasAfectadas INTEGER;
BEGIN
    UPDATE Contactos
    SET cContactosEstado = 'Bloqueado',
        dContactosFechaModificacion = NOW()
    WHERE nContactosId = nContactosId
    AND (cContactosUsuarioSolicitanteId = cUsuariosId OR cContactosUsuarioContactoId = cUsuariosId)
    AND bContactosActivo = true;

    GET DIAGNOSTICS nFilasAfectadas = ROW_COUNT;
    RETURN nFilasAfectadas > 0;
END;
$$;