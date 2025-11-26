-- Stored procedure para crear configuraciones de aplicación
CREATE OR REPLACE FUNCTION sp_aplicaciones_configuracion_crear(
    aplicacion_id INTEGER,
    cNombre VARCHAR(255) DEFAULT 'Aplicación Chat',
    cDescripcion TEXT DEFAULT 'Aplicación de chat modular',
    cTipoComunicacion VARCHAR(50) DEFAULT 'bidireccional',
    cTiposArchivoPermitidos TEXT DEFAULT 'jpg,jpeg,png,gif,pdf,doc,docx,txt',
    nTamanoMaximoArchivo BIGINT DEFAULT 10485760, -- 10MB
    nArchivosMaximosPorMensaje INTEGER DEFAULT 5,
    bHabilitarEncriptacion BOOLEAN DEFAULT true,
    bRegistrarComunicaciones BOOLEAN DEFAULT true,
    nMensajesPorMinuto INTEGER DEFAULT 60,
    nConexionesMaximas INTEGER DEFAULT 100,
    nTiempoEsperaConexion INTEGER DEFAULT 30,
    cNivelAutenticacion VARCHAR(50) DEFAULT 'basico',
    nHorasExpiracionToken INTEGER DEFAULT 24,
    cClaveApiExterna VARCHAR(255) DEFAULT NULL,
    cUrlBaseDatos VARCHAR(500) DEFAULT NULL
)
RETURNS TABLE(
    "nConfiguracionAplicacionId" INTEGER,
    "nConfiguracionAplicacionAplicacionId" INTEGER,
    "cConfiguracionAplicacionClave" VARCHAR,
    "cConfiguracionAplicacionValor" TEXT,
    "cConfiguracionAplicacionDescripcion" TEXT,
    "bConfiguracionAplicacionEsActiva" BOOLEAN,
    "dConfiguracionAplicacionFechaCreacion" TIMESTAMPTZ,
    "dConfiguracionAplicacionFechaActualizacion" TIMESTAMPTZ
)
LANGUAGE plpgsql
AS $$
DECLARE
    config_id INTEGER;
    config_record RECORD;
BEGIN
    -- Crear configuraciones por defecto para la aplicación
    
    -- 1. Nombre de la aplicación
    INSERT INTO "ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion"
    ) VALUES (
        aplicacion_id,
        'nombre',
        cNombre,
        'Nombre de la aplicación'
    ) RETURNING "nConfiguracionAplicacionId" INTO config_id;
    
    -- 2. Descripción
    INSERT INTO "ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion"
    ) VALUES (
        aplicacion_id,
        'descripcion',
        cDescripcion,
        'Descripción de la aplicación'
    );
    
    -- 3. Tipo de comunicación
    INSERT INTO "ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion"
    ) VALUES (
        aplicacion_id,
        'tipoComunicacion',
        cTipoComunicacion,
        'Tipo de comunicación permitida'
    );
    
    -- 4. Tipos de archivo permitidos
    INSERT INTO "ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion"
    ) VALUES (
        aplicacion_id,
        'tiposArchivoPermitidos',
        cTiposArchivoPermitidos,
        'Tipos de archivo permitidos'
    );
    
    -- 5. Tamaño máximo de archivo
    INSERT INTO "ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion"
    ) VALUES (
        aplicacion_id,
        'tamanoMaximoArchivo',
        nTamanoMaximoArchivo::TEXT,
        'Tamaño máximo de archivo en bytes'
    );
    
    -- 6. Archivos máximos por mensaje
    INSERT INTO "ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion"
    ) VALUES (
        aplicacion_id,
        'archivosMaximosPorMensaje',
        nArchivosMaximosPorMensaje::TEXT,
        'Número máximo de archivos por mensaje'
    );
    
    -- 7. Habilitar encriptación
    INSERT INTO "ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion"
    ) VALUES (
        aplicacion_id,
        'habilitarEncriptacion',
        bHabilitarEncriptacion::TEXT,
        'Indica si la encriptación está habilitada'
    );
    
    -- 8. Registrar comunicaciones
    INSERT INTO "ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion"
    ) VALUES (
        aplicacion_id,
        'registrarComunicaciones',
        bRegistrarComunicaciones::TEXT,
        'Indica si se deben registrar las comunicaciones'
    );
    
    -- 9. Mensajes por minuto
    INSERT INTO "ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion"
    ) VALUES (
        aplicacion_id,
        'mensajesPorMinuto',
        nMensajesPorMinuto::TEXT,
        'Límite de mensajes por minuto'
    );
    
    -- 10. Conexiones máximas
    INSERT INTO "ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion"
    ) VALUES (
        aplicacion_id,
        'conexionesMaximas',
        nConexionesMaximas::TEXT,
        'Número máximo de conexiones simultáneas'
    );
    
    -- 11. Tiempo de espera de conexión
    INSERT INTO "ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion"
    ) VALUES (
        aplicacion_id,
        'tiempoEsperaConexion',
        nTiempoEsperaConexion::TEXT,
        'Tiempo de espera de conexión en segundos'
    );
    
    -- 12. Nivel de autenticación
    INSERT INTO "ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion"
    ) VALUES (
        aplicacion_id,
        'nivelAutenticacion',
        cNivelAutenticacion,
        'Nivel de autenticación requerido'
    );
    
    -- 13. Horas de expiración del token
    INSERT INTO "ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion"
    ) VALUES (
        aplicacion_id,
        'horasExpiracionToken',
        nHorasExpiracionToken::TEXT,
        'Horas de expiración del token de acceso'
    );
    
    -- 14. Clave API externa
    INSERT INTO "ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion"
    ) VALUES (
        aplicacion_id,
        'claveApiExterna',
        cClaveApiExterna,
        'Clave de API externa'
    );
    
    -- 15. URL de base de datos
    INSERT INTO "ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion"
    ) VALUES (
        aplicacion_id,
        'urlBaseDatos',
        cUrlBaseDatos,
        'URL de la base de datos'
    );
    
    -- Retornar la primera configuración creada como referencia
    RETURN QUERY
    SELECT 
        ca."nConfiguracionAplicacionId",
        ca."nConfiguracionAplicacionAplicacionId",
        ca."cConfiguracionAplicacionClave",
        ca."cConfiguracionAplicacionValor",
        ca."cConfiguracionAplicacionDescripcion",
        ca."bConfiguracionAplicacionEsActiva",
        ca."dConfiguracionAplicacionFechaCreacion",
        ca."dConfiguracionAplicacionFechaActualizacion"
    FROM "ConfiguracionAplicacion" ca
    WHERE ca."nConfiguracionAplicacionAplicacionId" = aplicacion_id
    AND ca."cConfiguracionAplicacionClave" = 'nombre'
    LIMIT 1;
    
END;
$$;