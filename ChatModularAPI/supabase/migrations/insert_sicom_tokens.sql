-- Insertar tokens para SICOM_CHAT_2024 en AppRegistros
DO $$
DECLARE
    v_aplicacion_id INTEGER;
BEGIN
    -- Obtener el ID de la aplicación SICOM_CHAT_2024
    SELECT "nAplicacionesId" INTO v_aplicacion_id 
    FROM "Aplicaciones" 
    WHERE "cAplicacionesCodigo" = 'SICOM_CHAT_2024';
    
    -- Si la aplicación existe, insertar los tokens
    IF v_aplicacion_id IS NOT NULL THEN
        INSERT INTO "AppRegistros" (
            "nAppRegistrosAplicacionId",
            "cAppRegistrosCodigoApp",
            "cAppRegistrosNombreApp",
            "cAppRegistrosTokenAcceso",
            "cAppRegistrosSecretoApp",
            "bAppRegistrosEsActivo",
            "dAppRegistrosFechaCreacion",
            "dAppRegistrosFechaExpiracion"
        ) VALUES (
            v_aplicacion_id,
            'SICOM_CHAT_2024',
            'SICOM Chat Application 2024',
            'AT_6cbeb6faba662bffb1e9b0cdc3a96670',
            'ST_0b2c702f3fd200f0fc9ddb02624a21bc9d8be4fa96672504',
            true,
            now(),
            now() + INTERVAL '1 year'
        )
        ON CONFLICT ("cAppRegistrosCodigoApp") 
        DO UPDATE SET
            "cAppRegistrosTokenAcceso" = EXCLUDED."cAppRegistrosTokenAcceso",
            "cAppRegistrosSecretoApp" = EXCLUDED."cAppRegistrosSecretoApp",
            "bAppRegistrosEsActivo" = true;
            
        RAISE NOTICE 'Tokens insertados/actualizados para SICOM_CHAT_2024 con aplicación ID: %', v_aplicacion_id;
    ELSE
        RAISE NOTICE 'No se encontró la aplicación SICOM_CHAT_2024';
    END IF;
END $$