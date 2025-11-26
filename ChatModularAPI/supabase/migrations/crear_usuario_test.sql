-- Crear usuario de prueba verificado
-- Este usuario puede hacer login inmediatamente

-- Insertar en la tabla Usuarios (perfil de aplicación)
INSERT INTO "Usuarios" (
    "nUsuariosId",
    "cUsuariosNombre", 
    "cUsuariosEmail",
    "cUsuariosAvatar",
    "bUsuariosEstaEnLinea",
    "dUsuariosFechaCreacion"
) VALUES (
    'test-user-001',
    'Usuario Test',
    'test@chatmodular.com',
    NULL,
    false,
    NOW()
) ON CONFLICT ("nUsuariosId") DO NOTHING;

-- Verificar usuario creado
SELECT 
    "nUsuariosId",
    "cUsuariosNombre",
    "cUsuariosEmail",
    "bUsuariosEstaEnLinea",
    "dUsuariosFechaCreacion"
FROM "Usuarios"
WHERE "cUsuariosEmail" = 'test@chatmodular.com';