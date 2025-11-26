-- Migration: Create complete schema with all required tables (Fixed)
-- Date: 2024-12-25
-- Description: Creates all tables including Aplicaciones, Empresas, Contactos, and related entities

-- Create Aplicaciones table
CREATE TABLE IF NOT EXISTS Aplicaciones (
    nAplicacionesId UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    cAplicacionesNombre VARCHAR(100) NOT NULL,
    cAplicacionesDescripcion VARCHAR(500),
    dAplicacionesFechaCreacion TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- Create Empresas table
CREATE TABLE IF NOT EXISTS Empresas (
    nEmpresasId UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nEmpresasAplicacionId UUID NOT NULL,
    cEmpresasNombre VARCHAR(100) NOT NULL,
    cEmpresasDominio VARCHAR(100),
    cEmpresasDescripcion VARCHAR(500),
    bEmpresasActiva BOOLEAN DEFAULT TRUE,
    dEmpresasFechaCreacion TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    FOREIGN KEY (nEmpresasAplicacionId) REFERENCES Aplicaciones(nAplicacionesId) ON DELETE CASCADE
);

-- Create Usuarios table (enhanced version)
CREATE TABLE IF NOT EXISTS Usuarios (
    cUsuariosId VARCHAR(450) PRIMARY KEY,
    nUsuariosEmpresaId UUID,
    nUsuariosAplicacionId UUID,
    cUsuariosNombre VARCHAR(255) NOT NULL,
    cUsuariosEmail VARCHAR(255) NOT NULL,
    cUsuariosPerCodigo VARCHAR(100) NOT NULL,
    cUsuariosPerJurCodigo VARCHAR(100) NOT NULL,
    cUsuariosAvatar VARCHAR(500),
    bUsuariosEstaEnLinea BOOLEAN DEFAULT FALSE,
    bUsuariosActivo BOOLEAN DEFAULT TRUE,
    dUsuariosUltimaConexion TIMESTAMP WITH TIME ZONE,
    dUsuariosFechaCreacion TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    dUsuariosFechaModificacion TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    FOREIGN KEY (nUsuariosEmpresaId) REFERENCES Empresas(nEmpresasId) ON DELETE SET NULL,
    FOREIGN KEY (nUsuariosAplicacionId) REFERENCES Aplicaciones(nAplicacionesId) ON DELETE SET NULL
);

-- Create Contactos table
CREATE TABLE IF NOT EXISTS Contactos (
    nContactosId UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nContactosEmpresaId UUID NOT NULL,
    nContactosAplicacionId UUID NOT NULL,
    cContactosUsuarioSolicitanteId VARCHAR(450) NOT NULL,
    cContactosUsuarioContactoId VARCHAR(450) NOT NULL,
    cContactosEstado VARCHAR(20) NOT NULL DEFAULT 'Pendiente' CHECK (cContactosEstado IN ('Pendiente', 'Aceptado', 'Rechazado', 'Bloqueado')),
    cContactosNombreContacto VARCHAR(255),
    cContactosEmailContacto VARCHAR(255),
    cContactosTelefonoContacto VARCHAR(50),
    cContactosNotasContacto TEXT,
    bContactosActivo BOOLEAN DEFAULT TRUE,
    dContactosFechaSolicitud TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    dContactosFechaAceptacion TIMESTAMP WITH TIME ZONE,
    dContactosFechaCreacion TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    dContactosFechaModificacion TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    FOREIGN KEY (nContactosEmpresaId) REFERENCES Empresas(nEmpresasId) ON DELETE CASCADE,
    FOREIGN KEY (nContactosAplicacionId) REFERENCES Aplicaciones(nAplicacionesId) ON DELETE CASCADE,
    FOREIGN KEY (cContactosUsuarioSolicitanteId) REFERENCES Usuarios(cUsuariosId) ON DELETE CASCADE,
    FOREIGN KEY (cContactosUsuarioContactoId) REFERENCES Usuarios(cUsuariosId) ON DELETE CASCADE
);

-- Create ConfiguracionEmpresa table
CREATE TABLE IF NOT EXISTS ConfiguracionEmpresa (
    nConfiguracionEmpresaId UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nConfiguracionEmpresaAplicacionId UUID NOT NULL,
    nConfiguracionEmpresaEmpresaId UUID NOT NULL,
    bConfiguracionEmpresaPermitirChatPublico BOOLEAN DEFAULT FALSE,
    bConfiguracionEmpresaRequiereAprobacionContacto BOOLEAN DEFAULT TRUE,
    bConfiguracionEmpresaPermitirBusquedaUsuarios BOOLEAN DEFAULT TRUE,
    cConfiguracionEmpresaConfiguracionesAdicionales JSONB,
    dConfiguracionEmpresaFechaCreacion TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    dConfiguracionEmpresaFechaModificacion TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    FOREIGN KEY (nConfiguracionEmpresaAplicacionId) REFERENCES Aplicaciones(nAplicacionesId) ON DELETE CASCADE,
    FOREIGN KEY (nConfiguracionEmpresaEmpresaId) REFERENCES Empresas(nEmpresasId) ON DELETE CASCADE,
    UNIQUE(nConfiguracionEmpresaAplicacionId, nConfiguracionEmpresaEmpresaId)
);

-- Create AppRegistro table (enhanced)
CREATE TABLE IF NOT EXISTS AppRegistro (
    nAppRegistroId SERIAL PRIMARY KEY,
    cAppRegistroCodigoApp VARCHAR(100) NOT NULL UNIQUE,
    cAppRegistroNombreApp VARCHAR(255) NOT NULL,
    cAppRegistroTokenAcceso VARCHAR(500) NOT NULL,
    cAppRegistroSecretoApp VARCHAR(500),
    bAppRegistroEsActivo BOOLEAN DEFAULT TRUE,
    dAppRegistroFechaCreacion TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    dAppRegistroFechaExpiracion TIMESTAMP WITH TIME ZONE,
    cAppRegistroConfiguracionesAdicionales JSONB
);

-- Create TokenRegistro table
CREATE TABLE IF NOT EXISTS TokenRegistro (
    nTokenRegistroId SERIAL PRIMARY KEY,
    cTokenRegistroCodigoApp VARCHAR(100) NOT NULL,
    cTokenRegistroPerJurCodigo VARCHAR(100) NOT NULL,
    cTokenRegistroPerCodigo VARCHAR(100) NOT NULL,
    cTokenRegistroJwtToken TEXT NOT NULL,
    cTokenRegistroRefreshToken TEXT,
    cTokenRegistroUsuarioId VARCHAR(450),
    dTokenRegistroFechaExpiracion TIMESTAMP WITH TIME ZONE NOT NULL,
    bTokenRegistroEsActivo BOOLEAN DEFAULT TRUE,
    dTokenRegistroFechaCreacion TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    FOREIGN KEY (cTokenRegistroUsuarioId) REFERENCES Usuarios(cUsuariosId) ON DELETE SET NULL
);

-- Create indexes for better performance
CREATE INDEX IF NOT EXISTS idx_Empresas_aplicacion ON Empresas(nEmpresasAplicacionId);
CREATE INDEX IF NOT EXISTS idx_Usuarios_empresa ON Usuarios(nUsuariosEmpresaId);
CREATE INDEX IF NOT EXISTS idx_Usuarios_aplicacion ON Usuarios(nUsuariosAplicacionId);
CREATE INDEX IF NOT EXISTS idx_Usuarios_per_codigo ON Usuarios(cUsuariosPerCodigo);
CREATE INDEX IF NOT EXISTS idx_Usuarios_per_jur_codigo ON Usuarios(cUsuariosPerJurCodigo);
CREATE INDEX IF NOT EXISTS idx_Contactos_empresa ON Contactos(nContactosEmpresaId);
CREATE INDEX IF NOT EXISTS idx_Contactos_aplicacion ON Contactos(nContactosAplicacionId);
CREATE INDEX IF NOT EXISTS idx_Contactos_solicitante ON Contactos(cContactosUsuarioSolicitanteId);
CREATE INDEX IF NOT EXISTS idx_Contactos_contacto ON Contactos(cContactosUsuarioContactoId);
CREATE INDEX IF NOT EXISTS idx_Contactos_estado ON Contactos(cContactosEstado);

-- Create stored procedures for contact management
CREATE OR REPLACE FUNCTION USP_Contactos_Crear(
    nContactosEmpresaId UUID,
    nContactosAplicacionId UUID,
    cContactosUsuarioSolicitanteId VARCHAR(450),
    cContactosUsuarioContactoId VARCHAR(450),
    cContactosNombreContacto VARCHAR(255) DEFAULT NULL,
    cContactosEmailContacto VARCHAR(255) DEFAULT NULL,
    cContactosTelefonoContacto VARCHAR(50) DEFAULT NULL,
    cContactosNotasContacto TEXT DEFAULT NULL
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

-- Insert default application and company for testing
INSERT INTO Aplicaciones (nAplicacionesId, cAplicacionesNombre, cAplicacionesDescripcion)
VALUES (
    '00000000-0000-0000-0000-000000000001'::UUID,
    'Chat Application Default',
    'Aplicación de chat por defecto para pruebas'
) ON CONFLICT (nAplicacionesId) DO NOTHING;

INSERT INTO Empresas (nEmpresasId, nEmpresasAplicacionId, cEmpresasNombre, cEmpresasDominio, cEmpresasDescripcion)
VALUES (
    '00000000-0000-0000-0000-000000000001'::UUID,
    '00000000-0000-0000-0000-000000000001'::UUID,
    'Empresa Default',
    'default.com',
    'Empresa por defecto para pruebas'
) ON CONFLICT (nEmpresasId) DO NOTHING;

INSERT INTO ConfiguracionEmpresa (
    nConfiguracionEmpresaAplicacionId,
    nConfiguracionEmpresaEmpresaId,
    bConfiguracionEmpresaPermitirChatPublico,
    bConfiguracionEmpresaRequiereAprobacionContacto,
    bConfiguracionEmpresaPermitirBusquedaUsuarios
) VALUES (
    '00000000-0000-0000-0000-000000000001'::UUID,
    '00000000-0000-0000-0000-000000000001'::UUID,
    false,
    true,
    true
) ON CONFLICT (nConfiguracionEmpresaAplicacionId, nConfiguracionEmpresaEmpresaId) DO NOTHING;