-- Consultar tokens específicos para SICOM
SELECT 
    'JWT_SECRET_KEY=' || "cAppRegistrosSecretoApp" as jwt_secret,
    'ACCESS_TOKEN=' || "cAppRegistrosTokenAcceso" as access_token,
    'VITE_ACCESS_TOKEN=' || "cAppRegistrosTokenAcceso" as vite_access_token,
    'VITE_APP_CODE=SICOM_CHAT_2024' as vite_app_code
FROM "AppRegistros" 
WHERE "cAppRegistrosCodigoApp" = 'SICOM_CHAT_2024';