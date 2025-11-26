-- Migración para eliminar tablas antiguas del chat
-- Fecha: 2024-12-25
-- Descripción: Elimina las tablas con nomenclatura antigua que ya no se utilizan

-- Eliminar tablas de auditoría y logs primero (no tienen dependencias)
DROP TABLE IF EXISTS webhooks_registros CASCADE;
DROP TABLE IF EXISTS app_registros CASCADE;
DROP TABLE IF EXISTS token_registros CASCADE;

-- Eliminar tablas de chat con dependencias en orden correcto
DROP TABLE IF EXISTS chat_mensajes_lecturas CASCADE;
DROP TABLE IF EXISTS chat_mensajes CASCADE;
DROP TABLE IF EXISTS chat_usuarios_conversaciones CASCADE;
DROP TABLE IF EXISTS chat_conversaciones CASCADE;
DROP TABLE IF EXISTS chat_usuarios CASCADE;

-- Eliminar tablas de configuración si existen
DROP TABLE IF EXISTS auditoria_configuracion CASCADE;
DROP TABLE IF EXISTS respaldo_configuracion CASCADE;
DROP TABLE IF EXISTS plantilla_configuracion CASCADE;
DROP TABLE IF EXISTS configuracion_usuario CASCADE;
DROP TABLE IF EXISTS configuracion_empresa CASCADE;
DROP TABLE IF EXISTS configuracion_aplicacion CASCADE;

-- Mensaje de confirmación
SELECT 'Tablas antiguas eliminadas correctamente' as resultado;