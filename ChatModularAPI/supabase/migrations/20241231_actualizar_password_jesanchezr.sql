-- Actualizar la contraseña del usuario JESANCHEZR con hash correcto
-- Primero verificamos si el usuario existe y actualizamos su contraseña

DO $$
DECLARE
    usuario_existe INTEGER;
    password_hash TEXT;
BEGIN
    -- Verificar si el usuario JESANCHEZR existe
    SELECT COUNT(*) INTO usuario_existe 
    FROM "Usuarios" 
    WHERE "cUsuariosPerCodigo" = 'JESANCHEZR';
    
    IF usuario_existe > 0 THEN
        -- Generar hash de la contraseña "Jeysson12345" usando crypt
        -- Usamos bcrypt con salt generado automáticamente
        password_hash := crypt('Jeysson12345', gen_salt('bf'));
        
        -- Actualizar la contraseña del usuario
        UPDATE "Usuarios" 
        SET "cUsuariosPassword" = password_hash
        WHERE "cUsuariosPerCodigo" = 'JESANCHEZR';
        
        RAISE NOTICE 'Contraseña del usuario JESANCHEZR actualizada exitosamente';
    ELSE
        -- Si no existe, crear el usuario con la contraseña hasheada
        password_hash := crypt('Jeysson12345', gen_salt('bf'));
        
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
        
        RAISE NOTICE 'Usuario JESANCHEZR creado exitosamente con contraseña hasheada';
    END IF;
END $$;

-- Verificar el usuario y mostrar información (sin mostrar la contraseña)
SELECT 
    "nUsuariosId",
    "cUsuariosNombre",
    "cUsuariosEmail", 
    "cUsuariosPerCodigo",
    "cUsuariosPerJurCodigo",
    "bUsuariosActivo",
    "bUsuariosEstaEnLinea",
    "dUsuariosFechaCreacion",
    CASE 
        WHEN "cUsuariosPassword" IS NOT NULL THEN 'Contraseña configurada'
        ELSE 'Sin contraseña'
    END as password_status
FROM "Usuarios" 
WHERE "cUsuariosPerCodigo" = 'JESANCHEZR';