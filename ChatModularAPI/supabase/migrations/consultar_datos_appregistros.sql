-- Consultar todos los datos de AppRegistros para verificar los tokens
SELECT 
    "nAppRegistrosId",
    "nAppRegistrosAplicacionId",
    "cAppRegistrosCodigoApp",
    "cAppRegistrosNombreApp",
    "cAppRegistrosTokenAcceso",
    "cAppRegistrosSecretoApp",
    "bAppRegistrosEsActivo",
    "dAppRegistrosFechaCreacion",
    "dAppRegistrosFechaExpiracion"
FROM "AppRegistros"
WHERE "cAppRegistrosCodigoApp" = 'SICOM_CHAT_2024'