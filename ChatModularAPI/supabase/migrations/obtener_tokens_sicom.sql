-- Obtener tokens generados para SICOM
SELECT 
    "cAppRegistrosTokenAcceso",
    "cAppRegistrosSecretoApp",
    "cAppRegistrosCodigoApp",
    "cAppRegistrosNombreApp"
FROM "AppRegistros" 
WHERE "cAppRegistrosCodigoApp" = 'SICOM_CHAT_2024';