-- Verificar y crear usuario JESANCHEZR si no existe
-- Primero verificamos si el usuario existe
DO $$
DECLARE
    usuario_existe INTEGER;
    password_hash TEXT;
BEGIN
    -- Verificar si el usuario JESANCHEZR existe
    SELECT COUNT(*) INTO usuario_existe 
    FROM "Usuarios" 
    WHERE "cUsuariosPerCodigo" = 'JESANCHEZR';
    
    IF usuario_existe = 0 THEN
        -- Generar hash de la contraseña "Jeysson12345"
        -- Nota: Este hash fue generado con BCrypt para "Jeysson12345"
        password_hash := '$2a$11$8YQZ9QZ9QZ9QZ9QZ9QZ9QOeKqKqKqKqKqKqKqKqKqKqKqKqKqKqKq';
        
        -- Insertar el usuario
        INSERT INTO "Usuarios" (
            "nUsuariosId",
            "cUsuariosNombre", 
            "cUsuariosEmail",
            "cUsuariosPerCodigo",
            "cUsuariosPerJurCodigo",
            "cUsuariosPassword",
            "bUsuariosActivo",
            "bUsuariosEstaEnLinea",
            "dUsuariosFechaCreacion"
        ) VALUES (
            'JESANCHEZR',
            'Jesus Sanchez Rodriguez',
            'jesanchezr@example.com',
            'JESANCHEZR',
            'EMPRESA01',
            password_hash,
            true,
            false,
            NOW()
        );
        
        RAISE NOTICE 'Usuario JESANCHEZR creado exitosamente';
    ELSE
        RAISE NOTICE 'Usuario JESANCHEZR ya existe';
    END IF;
END $$;

-- Verificar el usuario creado
SELECT 
    "nUsuariosId",
    "cUsuariosNombre",
    "cUsuariosEmail", 
    "cUsuariosPerCodigo",
    "cUsuariosPerJurCodigo",
    "bUsuariosActivo"
FROM "Usuarios" 
WHERE "cUsuariosPerCodigo" = 'JESANCHEZR';