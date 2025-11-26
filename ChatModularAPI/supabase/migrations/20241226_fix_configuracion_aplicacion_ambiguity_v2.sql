-- Eliminar tabla duplicada en minúsculas y corregir stored procedure
-- Versión 2: Eliminar función existente primero

-- Eliminar tabla duplicada en minúsculas
DROP TABLE IF EXISTS public.configuracionaplicacion;

-- Eliminar función existente
DROP FUNCTION IF EXISTS public.sp_aplicaciones_configuracion_crear(INTEGER, VARCHAR, TEXT, VARCHAR, TEXT, BIGINT, INTEGER, BOOLEAN, BOOLEAN, INTEGER, INTEGER, INTEGER, VARCHAR, INTEGER, VARCHAR, VARCHAR);

-- Recrear el stored procedure con referencias explícitas a la tabla correcta
CREATE OR REPLACE FUNCTION public.sp_aplicaciones_configuracion_crear(
    nConfiguracionAplicacionAplicacionId INTEGER,
    cConfiguracionAplicacionNombre VARCHAR(255) DEFAULT 'Aplicación Chat',
    cConfiguracionAplicacionDescripcion TEXT DEFAULT 'Aplicación de chat modular',
    cConfiguracionAplicacionTipoComunicacion VARCHAR(50) DEFAULT 'bidireccional',
    cConfiguracionAplicacionTiposArchivoPermitidos TEXT DEFAULT 'jpg,jpeg,png,gif,pdf,doc,docx,txt',
    nConfiguracionAplicacionTamanoMaximoArchivo BIGINT DEFAULT 10485760, -- 10MB
    nConfiguracionAplicacionArchivosMaximosPorMensaje INTEGER DEFAULT 5,
    bConfiguracionAplicacionHabilitarEncriptacion BOOLEAN DEFAULT true,
    bConfiguracionAplicacionRegistrarComunicaciones BOOLEAN DEFAULT true,
    nConfiguracionAplicacionMensajesPorMinuto INTEGER DEFAULT 60,
    nConfiguracionAplicacionConexionesMaximas INTEGER DEFAULT 100,
    nConfiguracionAplicacionTiempoEsperaConexion INTEGER DEFAULT 30,
    cConfiguracionAplicacionNivelAutenticacion VARCHAR(50) DEFAULT 'basico',
    nConfiguracionAplicacionHorasExpiracionToken INTEGER DEFAULT 24,
    cConfiguracionAplicacionClaveApiExterna VARCHAR(255) DEFAULT NULL,
    cConfiguracionAplicacionUrlBaseDatos VARCHAR(500) DEFAULT NULL
)
RETURNS TABLE(
    "nConfiguracionAplicacionId" INTEGER,
    "nConfiguracionAplicacionAplicacionId" INTEGER,
    "cConfiguracionAplicacionClave" VARCHAR(100),
    "cConfiguracionAplicacionValor" TEXT,
    "cConfiguracionAplicacionDescripcion" TEXT,
    "dConfiguracionAplicacionFechaCreacion" TIMESTAMP,
    "dConfiguracionAplicacionFechaModificacion" TIMESTAMP,
    "bConfiguracionAplicacionActivo" BOOLEAN
)
LANGUAGE plpgsql
AS $$
DECLARE
    nConfiguracionAplicacionId INTEGER;
    rConfiguracionAplicacion RECORD;
BEGIN
    -- Crear configuraciones por defecto para la aplicación
    
    -- Insertar configuración principal
    INSERT INTO public."ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion"
    ) VALUES (
        nConfiguracionAplicacionAplicacionId,
        'nombre',
        cConfiguracionAplicacionNombre,
        'Nombre de la aplicación'
    ) RETURNING "nConfiguracionAplicacionId" INTO nConfiguracionAplicacionId;
    
    -- 2. Descripción
    INSERT INTO public."ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion"
    ) VALUES (
        nConfiguracionAplicacionAplicacionId,
        'descripcion',
        cConfiguracionAplicacionDescripcion,
        'Descripción de la aplicación'
    );
    
    -- 3. Tipo de comunicación
    INSERT INTO public."ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion"
    ) VALUES (
        nConfiguracionAplicacionAplicacionId,
        'tipoComunicacion',
        cConfiguracionAplicacionTipoComunicacion,
        'Tipo de comunicación permitida'
    );
    
    -- 4. Tipos de archivo permitidos
    INSERT INTO public."ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion"
    ) VALUES (
        nConfiguracionAplicacionAplicacionId,
        'tiposArchivoPermitidos',
        cConfiguracionAplicacionTiposArchivoPermitidos,
        'Tipos de archivo permitidos'
    );
    
    -- 5. Tamaño máximo de archivo
    INSERT INTO public."ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion"
    ) VALUES (
        nConfiguracionAplicacionAplicacionId,
        'tamanoMaximoArchivo',
        nConfiguracionAplicacionTamanoMaximoArchivo::TEXT,
        'Tamaño máximo de archivo en bytes'
    );
    
    -- 6. Archivos máximos por mensaje
    INSERT INTO public."ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion"
    ) VALUES (
        nConfiguracionAplicacionAplicacionId,
        'archivosMaximosPorMensaje',
        nConfiguracionAplicacionArchivosMaximosPorMensaje::TEXT,
        'Número máximo de archivos por mensaje'
    );
    
    -- 7. Habilitar encriptación
    INSERT INTO public."ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion"
    ) VALUES (
        nConfiguracionAplicacionAplicacionId,
        'habilitarEncriptacion',
        bConfiguracionAplicacionHabilitarEncriptacion::TEXT,
        'Indica si la encriptación está habilitada'
    );
    
    -- 8. Registrar comunicaciones
    INSERT INTO public."ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion"
    ) VALUES (
        nConfiguracionAplicacionAplicacionId,
        'registrarComunicaciones',
        bConfiguracionAplicacionRegistrarComunicaciones::TEXT,
        'Indica si se deben registrar las comunicaciones'
    );
    
    -- 9. Mensajes por minuto
    INSERT INTO public."ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion"
    ) VALUES (
        nConfiguracionAplicacionAplicacionId,
        'mensajesPorMinuto',
        nConfiguracionAplicacionMensajesPorMinuto::TEXT,
        'Límite de mensajes por minuto'
    );
    
    -- 10. Conexiones máximas
    INSERT INTO public."ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion"
    ) VALUES (
        nConfiguracionAplicacionAplicacionId,
        'conexionesMaximas',
        nConfiguracionAplicacionConexionesMaximas::TEXT,
        'Número máximo de conexiones simultáneas'
    );
    
    -- 11. Tiempo de espera de conexión
    INSERT INTO public."ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion"
    ) VALUES (
        nConfiguracionAplicacionAplicacionId,
        'tiempoEsperaConexion',
        nConfiguracionAplicacionTiempoEsperaConexion::TEXT,
        'Tiempo de espera de conexión en segundos'
    );
    
    -- 12. Nivel de autenticación
    INSERT INTO public."ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion"
    ) VALUES (
        nConfiguracionAplicacionAplicacionId,
        'nivelAutenticacion',
        cConfiguracionAplicacionNivelAutenticacion,
        'Nivel de autenticación requerido'
    );
    
    -- 13. Horas de expiración del token
    INSERT INTO public."ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion"
    ) VALUES (
        nConfiguracionAplicacionAplicacionId,
        'horasExpiracionToken',
        nConfiguracionAplicacionHorasExpiracionToken::TEXT,
        'Horas de expiración del token de acceso'
    );
    
    -- 14. Clave API externa
    INSERT INTO public."ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion"
    ) VALUES (
        nConfiguracionAplicacionAplicacionId,
        'claveApiExterna',
        cConfiguracionAplicacionClaveApiExterna,
        'Clave de API externa'
    );
    
    -- 15. URL de base de datos
    INSERT INTO public."ConfiguracionAplicacion" (
        "nConfiguracionAplicacionAplicacionId",
        "cConfiguracionAplicacionClave",
        "cConfiguracionAplicacionValor",
        "cConfiguracionAplicacionDescripcion"
    ) VALUES (
        nConfiguracionAplicacionAplicacionId,
        'urlBaseDatos',
        cConfiguracionAplicacionUrlBaseDatos,
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
        ca."dConfiguracionAplicacionFechaCreacion",
        ca."dConfiguracionAplicacionFechaModificacion",
        ca."bConfiguracionAplicacionActivo"
    FROM public."ConfiguracionAplicacion" ca
    WHERE ca."nConfiguracionAplicacionAplicacionId" = nConfiguracionAplicacionAplicacionId
    AND ca."cConfiguracionAplicacionClave" = 'nombre'
    LIMIT 1;
    
END;
$$;

-- Otorgar permisos de ejecución
GRANT EXECUTE ON FUNCTION public.sp_aplicaciones_configuracion_crear(INTEGER, VARCHAR, TEXT, VARCHAR, TEXT, BIGINT, INTEGER, BOOLEAN, BOOLEAN, INTEGER, INTEGER, INTEGER, VARCHAR, INTEGER, VARCHAR, VARCHAR) TO authenticated;
GRANT EXECUTE ON FUNCTION public.sp_aplicaciones_configuracion_crear(INTEGER, VARCHAR, TEXT, VARCHAR, TEXT, BIGINT, INTEGER, BOOLEAN, BOOLEAN, INTEGER, INTEGER, INTEGER, VARCHAR, INTEGER, VARCHAR, VARCHAR) TO anon;

-- Comentario para documentación
COMMENT ON FUNCTION public.sp_aplicaciones_configuracion_crear(INTEGER, VARCHAR, TEXT, VARCHAR, TEXT, BIGINT, INTEGER, BOOLEAN, BOOLEAN, INTEGER, INTEGER, INTEGER, VARCHAR, INTEGER, VARCHAR, VARCHAR) IS 'Stored procedure para crear configuraciones por defecto de una aplicación';