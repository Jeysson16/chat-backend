-- Create the missing chatcontacto table that the backend is expecting
-- Based on the error logs showing "relation 'public.chatcontacto' does not exist"

CREATE TABLE IF NOT EXISTS public.chatcontacto (
    c_contactos_id TEXT PRIMARY KEY DEFAULT gen_random_uuid()::text,
    c_usuarios_id TEXT NOT NULL,
    c_contacto_usuarios_id TEXT NOT NULL,
    c_contactos_estado TEXT DEFAULT 'Activo',
    n_empresas_id TEXT NOT NULL,
    n_aplicaciones_id TEXT NOT NULL,
    d_fecha_creacion TIMESTAMP WITH TIME ZONE DEFAULT now(),
    d_fecha_actualizacion TIMESTAMP WITH TIME ZONE DEFAULT now()
);

-- Grant permissions
GRANT SELECT ON public.chatcontacto TO anon;
GRANT ALL ON public.chatcontacto TO authenticated;
GRANT ALL ON public.chatcontacto TO service_role;

-- Enable RLS
ALTER TABLE public.chatcontacto ENABLE ROW LEVEL SECURITY;

-- Create RLS policies
CREATE POLICY "Allow read access to contacts" ON public.chatcontacto
    FOR SELECT
    USING (true);

CREATE POLICY "Allow users to manage their contacts" ON public.chatcontacto
    FOR ALL
    USING (
        c_usuarios_id = auth.uid()::text OR 
        c_contacto_usuarios_id = auth.uid()::text
    );