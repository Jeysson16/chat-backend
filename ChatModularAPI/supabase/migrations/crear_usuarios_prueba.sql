-- Crear usuarios de prueba en la tabla Usuarios
-- Nota: La autenticación se maneja a través de Supabase Auth
-- Estos son registros de perfil de usuario en la aplicación

-- Usuario administrador
INSERT INTO "Usuarios" (
    "nUsuariosId",
    "cUsuariosNombre",
    "cUsuariosEmail",
    "cUsuariosAvatar",
    "bUsuariosEstaEnLinea",
    "dUsuariosFechaCreacion"
) VALUES (
    'admin-uuid-001',
    'Administrador Sistema',
    'admin@chatmodular.com',
    NULL,
    false,
    NOW()
) ON CONFLICT ("nUsuariosId") DO NOTHING;

-- Usuario normal de prueba
INSERT INTO "Usuarios" (
    "nUsuariosId",
    "cUsuariosNombre",
    "cUsuariosEmail",
    "cUsuariosAvatar",
    "bUsuariosEstaEnLinea", 
    "dUsuariosFechaCreacion"
) VALUES (
    'user-uuid-001',
    'Usuario Prueba',
    'usuario@chatmodular.com',
    NULL,
    false,
    NOW()
) ON CONFLICT ("nUsuariosId") DO NOTHING;

-- Verificar usuarios creados
SELECT 
    "nUsuariosId",
    "cUsuariosNombre",
    "cUsuariosEmail",
    "cUsuariosAvatar",
    "bUsuariosEstaEnLinea",
    "dUsuariosFechaCreacion"
FROM "Usuarios"
WHERE "cUsuariosEmail" IN ('admin@chatmodular.com', 'usuario@chatmodular.com');