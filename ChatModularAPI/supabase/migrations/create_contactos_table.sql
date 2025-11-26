-- =============================================
-- Create missing contactos table for legacy stored procedures
-- Based on the structure expected by USP_Contactos_* procedures
-- =============================================

CREATE TABLE IF NOT EXISTS public.contactos (
    ncontactosid UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    ncontactosempresaid UUID NOT NULL,
    ncontactosaplicacionid UUID NOT NULL,
    ccontactosusuariosolicitanteid VARCHAR(450) NOT NULL,
    ccontactosusuariocontactoid VARCHAR(450) NOT NULL,
    ccontactosestado VARCHAR(20) DEFAULT 'Pendiente' CHECK (ccontactosestado IN ('Pendiente', 'Aceptado', 'Rechazado', 'Bloqueado')),
    ccontactosnombrecontacto VARCHAR(255),
    ccontactosemailcontacto VARCHAR(255),
    ccontactostelefonocontacto VARCHAR(50),
    ccontactosnotascontacto TEXT,
    bcontactosactivo BOOLEAN DEFAULT true,
    dcontactosfechasolicitud TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    dcontactosfechaaceptacion TIMESTAMP WITH TIME ZONE,
    dcontactosfechacreacion TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    dcontactosfechamodificacion TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    UNIQUE(ncontactosempresaid, ncontactosaplicacionid, ccontactosusuariosolicitanteid, ccontactosusuariocontactoid)
);

-- Create indexes for performance
CREATE INDEX IF NOT EXISTS idx_contactos_empresa_aplicacion ON public.contactos(ncontactosempresaid, ncontactosaplicacionid);
CREATE INDEX IF NOT EXISTS idx_contactos_usuario_solicitante ON public.contactos(ccontactosusuariosolicitanteid);
CREATE INDEX IF NOT EXISTS idx_contactos_usuario_contacto ON public.contactos(ccontactosusuariocontactoid);
CREATE INDEX IF NOT EXISTS idx_contactos_estado ON public.contactos(ccontactosestado);
CREATE INDEX IF NOT EXISTS idx_contactos_activo ON public.contactos(bcontactosactivo);

-- Create trigger for updated_at
CREATE OR REPLACE FUNCTION update_contactos_updated_at()
RETURNS TRIGGER AS $$
BEGIN
    NEW.dcontactosfechamodificacion = NOW();
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER update_contactos_updated_at 
    BEFORE UPDATE ON public.contactos 
    FOR EACH ROW 
    EXECUTE FUNCTION update_contactos_updated_at();

-- Grant permissions
GRANT SELECT ON public.contactos TO anon;
GRANT ALL PRIVILEGES ON public.contactos TO authenticated;

-- Add comments
COMMENT ON TABLE public.contactos IS 'Tabla de contactos para el sistema de chat - legacy schema for stored procedures';
COMMENT ON COLUMN public.contactos.ncontactosid IS 'ID único del contacto';
COMMENT ON COLUMN public.contactos.ncontactosempresaid IS 'ID de la empresa';
COMMENT ON COLUMN public.contactos.ncontactosaplicacionid IS 'ID de la aplicación';
COMMENT ON COLUMN public.contactos.ccontactosusuariosolicitanteid IS 'ID del usuario que envió la solicitud';
COMMENT ON COLUMN public.contactos.ccontactosusuariocontactoid IS 'ID del usuario contacto';
COMMENT ON COLUMN public.contactos.ccontactosestado IS 'Estado del contacto (Pendiente, Aceptado, Rechazado, Bloqueado)';
COMMENT ON COLUMN public.contactos.ccontactosnombrecontacto IS 'Nombre del contacto';
COMMENT ON COLUMN public.contactos.ccontactosemailcontacto IS 'Email del contacto';
COMMENT ON COLUMN public.contactos.ccontactostelefonocontacto IS 'Teléfono del contacto';
COMMENT ON COLUMN public.contactos.ccontactosnotascontacto IS 'Notas adicionales del contacto';
COMMENT ON COLUMN public.contactos.bcontactosactivo IS 'Indica si el contacto está activo';
COMMENT ON COLUMN public.contactos.dcontactosfechasolicitud IS 'Fecha de la solicitud';
COMMENT ON COLUMN public.contactos.dcontactosfechaaceptacion IS 'Fecha de aceptación';
COMMENT ON COLUMN public.contactos.dcontactosfechacreacion IS 'Fecha de creación';
COMMENT ON COLUMN public.contactos.dcontactosfechamodificacion IS 'Fecha de última modificación';