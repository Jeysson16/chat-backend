-- Eliminar la función existente si existe
DROP FUNCTION IF EXISTS sp_aplicaciones_crear(VARCHAR, VARCHAR, VARCHAR);

-- Crear función corregida para crear aplicaciones con tabla AppRegistros (plural)
CREATE OR REPLACE FUNCTION sp_aplicaciones_crear(
    p_cAplicacionesNombre VARCHAR(255),
    p_cAplicacionesCodigo VARCHAR(100),
    p_cAplicacionesDescripcion VARCHAR(500)
)
RETURNS TABLE(
    nAplicacionesId INTEGER,
    cAplicacionesNombre VARCHAR(255),
    cAplicacionesCodigo VARCHAR(100),
    cAppRegistrosTokenAcceso VARCHAR(500),
    cAppRegistrosSecretoApp VARCHAR(500),
    dAplicacionesFechaCreacion TIMESTAMPTZ,
    nConfiguracionesCreadas INTEGER
)
LANGUAGE plpgsql
AS $$
DECLARE
    nAplicacionesId INTEGER;
    cAppRegistroTokenAcceso VARCHAR(500);
    cAppRegistroSecretoApp VARCHAR(500);
    nConfiguracionesCreadas INTEGER := 0;
BEGIN
    -- Verificar si ya existe una aplicación con el mismo código
    IF EXISTS (SELECT 1 FROM Aplicaciones WHERE cAplicacionesCodigo = p_cAplicacionesCodigo) THEN
        RAISE EXCEPTION 'Ya existe una aplicación con el código: %', p_cAplicacionesCodigo;
    END IF;

    -- Generar tokens únicos para la aplicación
    cAppRegistroTokenAcceso := 'app_' || encode(gen_random_bytes(32), 'hex');
    cAppRegistroSecretoApp := 'secret_' || encode(gen_random_bytes(32), 'hex');

    -- Insertar nueva aplicación
    INSERT INTO Aplicaciones (
        cAplicacionesNombre,
        cAplicacionesDescripcion,
        cAplicacionesCodigo,
        dAplicacionesFechaCreacion,
        bAplicacionesEsActiva
    ) VALUES (
        p_cAplicacionesNombre,
        p_cAplicacionesDescripcion,
        p_cAplicacionesCodigo,
        NOW(),
        TRUE
    ) RETURNING nAplicacion