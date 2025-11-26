-- Insertar aplicación SICOM_CHAT_2024 directamente en las tablas
-- Primero verificar si ya existe
DO $$
DECLARE
    v_nAplicacionesId INTEGER;
    v_cAppRegistrosTokenAcceso VARCHAR(255) := 'AT_6cbeb6faba662bffb1e9b0cdc3a96670';
    v_cAppRegistrosSecretoApp VARCHAR(255) := 'ST_0b2c702f3fd200f0fc9ddb02624a21bc9d8be4fa96672504';
BEGIN
    -- Verificar si la aplicación ya existe
    IF NOT EXISTS (SELECT 1 FROM "Aplicaciones" WHERE "cAplicacionesCodigo" = 'SICOM_CHAT_2024') THEN
        
        -- Insertar la aplicación
        INSERT INTO "Aplicaciones" (
            "cAplicacionesNombre",
            "cAplicacionesCodigo", 
            "cAplicacionesDescripcion",
            "bAplicacionesEsActiva",
            "dAplicacionesFechaCreacion"
        ) VALUES (
            'Chat Modular Frontend',
            'SICOM_CHAT_2024',
            'Aplicación frontend para el sistema de chat modular',
            TRUE,
            NOW()
        ) RETURNING "nAplicacionesId" INTO v_nAplicacionesId;

        -- Insertar registro de tokens en AppRegistros
        INSERT INTO "AppRegistros" (
            "nAppRegistrosAplicacionId",
            "cAppRegistrosTokenAcceso",
            "cAppRegistrosSecretoApp",
            "dAppRegistrosFechaCreacion",
            "dAppRegistrosFechaExpiracion",
            "bAppRegistrosEsActivo"
        ) VALUES (
            v_nAplicacionesId,
            v_cAppRegistrosTokenAcceso,
            v_cAppRegistrosSecretoApp,
            NOW(),
            NOW() + INTERVAL '1 year',
            TRUE
        );

        -- Crear configuraciones por defecto
        INSERT INTO "ConfiguracionAplicacion" (
            "nConfiguracionAplicacionAplicacionId",
            "cConfiguracionAplicacionClave",
            "cConfiguracionAplicacionValor",
            "cConfiguracionAplicacionDescripcion",
            "bConfiguracionAplicacionActivo",
            "dConfiguracionAplicacionFechaCreacion",
            "dConfiguracionAplicacionFechaModificacion"
        ) VALUES 
        (
            v_nAplicacionesId,
            'ADJUNTOS_HABILITADOS',
            'true',
            'Configuración para habilitar adjuntos en la aplicación',
            TRUE,
            NOW(),
            NOW()
        ),
        (
            v_nAplicacionesId,
            'ADJUNTOS_TAMAÑO_MAXIMO_MB',
            '10',
            'Tamaño máximo permitido para adjuntos en MB',
            TRUE,
            NOW(),
            NOW()
        );

        RAISE NOTICE 'Aplicación SICOM_CHAT_2024 creada exitosamente con ID: %', v_nAplicacionesId;
    ELSE
        RAISE NOTICE 'La aplicación SICOM_CHAT_2024 ya existe';
    END IF;
END;
$$;