-- Verificar que los tokens se insertaron correctamente
SELECT 
    a."nAplicacionesId",
    a."cAplicacionesCodigo",
    a."cAplicacionesNombre",
    ar."nAppRegistrosId",
    ar."cAppRegistrosCodigoApp",
    ar."cAppRegistrosNombreApp",
    ar."cAppRegistrosTokenAcceso",
    ar."cAppRegistrosSecretoApp",
    ar."bAppRegistrosEsActivo",
    ar."dAppRegistrosFechaCreacion"
FROM "Aplicaciones" a
LEFT JOIN "AppRegistros" ar ON a."nAplicacionesId" = ar."nAppRegistrosAplicacionId"
WHERE a."cAplicacionesCodigo" = 'SICOM_CHAT_2024';