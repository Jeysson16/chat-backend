-- Ejecutar stored procedure para registrar aplicación Chat Modular Frontend
-- Este script registra la aplicación y genera los tokens necesarios

SELECT * FROM sp_aplicaciones_crear(
    'Chat Modular Frontend',
    'CHAT_FRONTEND_001',
    'Aplicación frontend para sistema de chat modular con Angular'
);