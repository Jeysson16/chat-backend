-- Obtener solo los valores de los tokens
SELECT 
    ar."cAppRegistrosTokenAcceso" as access_token,
    ar."cAppRegistrosSecretoApp" as secret_app
FROM "AppRegistros" ar
WHERE ar."cAppRegistrosCodigoApp" = 'SICOM_CHAT_2024';