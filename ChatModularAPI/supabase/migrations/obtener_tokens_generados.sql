-- Obtener los tokens generados para la aplicación ChatModularAPI
-- Estos valores se usarán en el archivo .env del backend

SELECT 
    'JWT_SECRET_KEY=' || ar."cAppRegistrosSecretoApp" as jwt_secret_config,
    'ACCESS_TOKEN=' || ar."cAppRegistrosTokenAcceso" as access_token_config,
    ar."cAppRegistrosTokenAcceso" as token_acceso_solo,
    ar."cAppRegistrosSecretoApp" as secreto_app_solo
FROM "AppRegistros" ar
WHERE ar."cAppRegistrosCodigoApp" = 'CHAT_API_2024'
  AND ar."bAppRegistrosEsActivo" = true;