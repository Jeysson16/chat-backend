-- Mostrar tokens de SICOM para actualizar archivos .env
SELECT 
    'ACCESS_TOKEN=' || ar."cAppRegistrosTokenAcceso" as backend_access_token,
    'JWT_SECRET_KEY=' || ar."cAppRegistrosSecretoApp" as backend_jwt_secret,
    'VITE_ACCESS_TOKEN=' || ar."cAppRegistrosTokenAcceso" as frontend_access_token,
    ar."cAppRegistrosTokenAcceso" as access_token_value,
    ar."cAppRegistrosSecretoApp" as secret_app_value,
    ar."cAppRegistrosNombreApp" as app_name,
    ar."cAppRegistrosCodigoApp" as app_code
FROM "AppRegistros" ar
WHERE ar."cAppRegistrosCodigoApp" = 'SICOM_CHAT_2024';