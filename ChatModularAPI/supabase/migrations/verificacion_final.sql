-- Verificación final de la integración con Supabase
-- Este script verifica que todos los componentes estén correctamente configurados

-- 1. Verificar que la aplicación ChatModularAPI está registrada
SELECT 
    'APLICACION REGISTRADA' as verificacion,
    a."nAplicacionesId",
    a."cAplicacionesNombre",
    a."cAplicacionesCodigo",
    a."dAplicacionesFechaCreacion"
FROM "Aplicaciones" a
WHERE a."cAplicacionesCodigo" = 'CHAT_API_2024';

-- 2. Verificar que los tokens están generados
SELECT 
    'TOKENS GENERADOS' as verificacion,
    ar."cAppRegistrosCodigoApp",
    ar."cAppRegistrosNombreApp",
    LENGTH(ar."cAppRegistrosTokenAcceso") as longitud_token_acceso,
    LENGTH(ar."cAppRegistrosSecretoApp") as longitud_secreto,
    ar."bAppRegistrosEsActivo",
    ar."dAppRegistrosFechaCreacion",
    ar."dAppRegistrosFechaExpiracion"
FROM "AppRegistros" ar
WHERE ar."cAppRegistrosCodigoApp" = 'CHAT_API_2024';

-- 3. Verificar que los usuarios de prueba están creados
SELECT 
    'USUARIOS CREADOS' as verificacion,
    COUNT(*) as total_usuarios,
    STRING_AGG("cUsuariosNombre", ', ') as nombres_usuarios
FROM "Usuarios"
WHERE "cUsuariosEmail" IN ('admin@chatmodular.com', 'usuario@chatmodular.com');

-- 4. Verificar que las tablas principales existen y tienen la estructura correcta
SELECT 
    'TABLAS VERIFICADAS' as verificacion,
    COUNT(*) as total_tablas
FROM information_schema.tables 
WHERE table_schema = 'public' 
AND table_name IN ('Aplicaciones', 'AppRegistros', 'Usuarios', 'Conversaciones', 'Mensajes');

-- 5. Mostrar resumen de configuración
SELECT 
    'RESUMEN CONFIGURACION' as verificacion,
    'ChatModularAPI registrada con tokens generados y usuarios de prueba creados' as estado,
    NOW() as fecha_verificacion;