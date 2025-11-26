-- Verificar si la aplicación SICOM_CHAT_2024 existe y sus tokens
SELECT 
    a."nAplicacionesId",
    a."cAplicacionesNombre",
    a."cAplicacionesCodigo",
    a."bAplicacionesEsActiva",
    a."dAplicacionesFechaCreacion",
    ar."cAppRegistrosTokenAcceso",
    ar."cAppRegistrosSecretoApp",
    ar."bAppRegistrosEsActivo",
    ar."dAppRegistrosFechaExpiracion"
FROM "Aplicaciones" a
LEFT JOIN "AppRegistros" ar ON a."nAplicacionesId" = ar."nAppRegistrosAplicacionId"
WHERE a."cAplicacionesCodigo" = 'SICOM_CHAT_2024'