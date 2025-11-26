-- Ejecutar stored procedure para registrar la aplicación ChatModularAPI
SELECT * FROM sp_aplicaciones_crear(
    'ChatModularAPI',
    'CHAT_API_2024', 
    'API modular para sistema de chat empresarial'
);