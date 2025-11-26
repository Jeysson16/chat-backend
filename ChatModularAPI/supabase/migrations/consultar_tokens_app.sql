-- Consultar los tokens generados para la aplicación ChatModularAPI
SELECT 
    ar."cAppRegistrosCodigoApp" as codigo_app,
    ar."cAppRegistrosNombreApp" as nombre_app,
    ar."cAppRegistrosTokenAcceso" as token_acceso,
    ar."cAppRegistrosSecretoApp" as secreto_app,
    ar."dAppRegistrosFechaCreacion" as fecha_creacion,
    ar."dAppRegistrosFechaExpiracion" as fecha_expiracion
FROM "AppRegistros" ar
WHERE ar."cAppRegistrosCodigoApp" = 'CHAT_API_2024';