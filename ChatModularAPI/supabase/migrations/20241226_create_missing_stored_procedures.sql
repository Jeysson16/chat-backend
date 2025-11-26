-- =============================================
-- Migration: Create missing stored procedures
-- Date: 2024-12-26
-- Description: Creates the missing stored procedures that the backend is expecting
-- =============================================

-- USP_Application_GetAll: Obtener todas las aplicaciones
CREATE OR REPLACE FUNCTION USP_Application_GetAll()
RETURNS TABLE (
    naplicacionesid UUID,
    caplicacionesnombre VARCHAR,
    caplicacionesdescripcion VARCHAR,
    caplicacionescodigo VARCHAR,
    baplicacionesesactiva BOOLEAN,
    daplicacionesfechacreacion TIMESTAMPTZ
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT 
        a.naplicacionesid,
        a.caplicacionesnombre,
        a.caplicacionesdescripcion,
        a.caplicacionescodigo,
        a.baplicacionesesactiva,
        a.daplicacionesfechacreacion
    FROM aplicaciones a
    WHERE a.baplicacionesesactiva = true
    ORDER BY a.caplicacionesnombre;
END;
$$;

-- USP_Empresa_GetAll: Obtener todas las empresas
CREATE OR REPLACE FUNCTION USP_Empresa_GetAll()
RETURNS TABLE (
    nempresasid UUID,
    nempresasaplicacionid UUID,
    cempresasnombre VARCHAR,
    cempresascodigo VARCHAR,
    cempresasdominio VARCHAR,
    bempresasactiva BOOLEAN,
    dempresasfechacreacion TIMESTAMPTZ
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT 
        e.nempresasid,
        e.nempresasaplicacionid,
        e.cempresasnombre,
        e.cempresascodigo,
        e.cempresasdominio,
        e.bempresasactiva,
        e.dempresasfechacreacion
    FROM empresas e
    WHERE e.bempresasactiva = true
    ORDER BY e.cempresasnombre;
END;
$$;

-- USP_TokenRegistro_GetAll: Obtener todos los registros de tokens
CREATE OR REPLACE FUNCTION USP_TokenRegistro_GetAll()
RETURNS TABLE (
    ntokenregistroid INTEGER,
    ctokenregistrocodigoapp VARCHAR,
    ctokenregistroperjurcodigo VARCHAR,
    ctokenregistrotokenacceso VARCHAR,
    ctokenregistrotokenrefresco VARCHAR,
    dtokenregistrofechacreacion TIMESTAMPTZ,
    dtokenregistrofechaexpiracion TIMESTAMPTZ,
    btokenregistroesactivo BOOLEAN
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT 
        tr.ntokenregistroid,
        tr.ctokenregistrocodigoapp,
        tr.ctokenregistroperjurcodigo,
        tr.ctokenregistrotokenacceso,
        tr.ctokenregistrotokenrefresco,
        tr.dtokenregistrofechacreacion,
        tr.dtokenregistrofechaexpiracion,
        tr.btokenregistroesactivo
    FROM tokenregistro tr
    WHERE tr.btokenregistroesactivo = true
    ORDER BY tr.dtokenregistrofechacreacion DESC;
END;
$$;

-- USP_Chat_GetAll: Obtener todas las conversaciones de chat
CREATE OR REPLACE FUNCTION USP_Chat_GetAll()
RETURNS TABLE (
    cchatconversacionesid UUID,
    cchatconversacionesnombre VARCHAR,
    cchatconversacionestipo VARCHAR,
    cchatconversacionesdescripcion TEXT,
    cchatconversacionesimagenurl TEXT,
    cchatconversacionesconfiguracion JSONB,
    cchatconversacionesestado VARCHAR,
    creadopor VARCHAR,
    dchatconversacionesfechacreacion TIMESTAMPTZ
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT 
        cc.cchatconversacionesid,
        cc.cchatconversacionesnombre,
        cc.cchatconversacionestipo,
        cc.cchatconversacionesdescripcion,
        cc.cchatconversacionesimagenurl,
        cc.cchatconversacionesconfiguracion,
        cc.cchatconversacionesestado,
        cc.creadopor,
        cc.dchatconversacionesfechacreacion
    FROM chat_conversaciones cc
    WHERE cc.cchatconversacionesestado = 'activa'
    ORDER BY cc.dchatconversacionesfechacreacion DESC;
END;
$$;

-- USP_ConfiguracionEmpresa_GetAll: Obtener todas las configuraciones de empresa
CREATE OR REPLACE FUNCTION USP_ConfiguracionEmpresa_GetAll()
RETURNS TABLE (
    nconfiguracionempresaid UUID,
    nconfiguracionempresaaplicacionid UUID,
    nconfiguracionempresaempresaid UUID,
    bconfiguracionempresapermitirchatpublico BOOLEAN,
    bconfiguracionempresarequiereaprobacioncontacto BOOLEAN,
    bconfiguracionempresapermitirbusquedausuarios BOOLEAN,
    cconfiguracionempresaconfiguracionesadicionales JSONB,
    dconfiguracionempresafechacreacion TIMESTAMPTZ,
    dconfiguracionempresafechamodificacion TIMESTAMPTZ
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT 
        ce.nconfiguracionempresaid,
        ce.nconfiguracionempresaaplicacionid,
        ce.nconfiguracionempresaempresaid,
        ce.bconfiguracionempresapermitirchatpublico,
        ce.bconfiguracionempresarequiereaprobacioncontacto,
        ce.bconfiguracionempresapermitirbusquedausuarios,
        ce.cconfiguracionempresaconfiguracionesadicionales,
        ce.dconfiguracionempresafechacreacion,
        ce.dconfiguracionempresafechamodificacion
    FROM configuracionempresa ce
    ORDER BY ce.dconfiguracionempresafechacreacion DESC;
END;
$$;

-- Grant permissions to anon and authenticated roles
GRANT SELECT ON aplicaciones TO anon;
GRANT SELECT ON aplicaciones TO authenticated;
GRANT SELECT ON empresas TO anon;
GRANT SELECT ON empresas TO authenticated;
GRANT SELECT ON tokenregistro TO anon;
GRANT SELECT ON tokenregistro TO authenticated;
GRANT SELECT ON chat_conversaciones TO anon;
GRANT SELECT ON chat_conversaciones TO authenticated;
GRANT SELECT ON configuracionempresa TO anon;
GRANT SELECT ON configuracionempresa TO authenticated;

-- Grant execute permissions on functions
GRANT EXECUTE ON FUNCTION USP_Application_GetAll() TO anon;
GRANT EXECUTE ON FUNCTION USP_Application_GetAll() TO authenticated;
GRANT EXECUTE ON FUNCTION USP_Empresa_GetAll() TO anon;
GRANT EXECUTE ON FUNCTION USP_Empresa_GetAll() TO authenticated;
GRANT EXECUTE ON FUNCTION USP_TokenRegistro_GetAll() TO anon;
GRANT EXECUTE ON FUNCTION USP_TokenRegistro_GetAll() TO authenticated;
GRANT EXECUTE ON FUNCTION USP_Chat_GetAll() TO anon;
GRANT EXECUTE ON FUNCTION USP_Chat_GetAll() TO authenticated;
GRANT EXECUTE ON FUNCTION USP_ConfiguracionEmpresa_GetAll() TO anon;
GRANT EXECUTE ON FUNCTION USP_ConfiguracionEmpresa_GetAll() TO authenticated;