-- Verificar si existe SICOM en AppRegistros
SELECT 
    cAppRegistrosCodigoApp,
    cAppRegistrosNombreApp,
    cAppRegistrosTokenAcceso,
    cAppRegistrosSecretoApp,
    bAppRegistrosEsActivo,
    dAppRegistrosFechaCreacion
FROM AppRegistros 
WHERE cAppRegistrosCodigoApp = 'SICOM_CHAT_2024';