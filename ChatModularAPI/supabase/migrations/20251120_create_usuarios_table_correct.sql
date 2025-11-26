-- Create the missing Usuarios table with the exact structure expected by the backend
CREATE TABLE IF NOT EXISTS public."Usuarios" (
    "nUsuariosId" TEXT PRIMARY KEY DEFAULT gen_random_uuid()::text,
    "cUsuariosNombre" TEXT NOT NULL,
    "cUsuariosEmail" TEXT NOT NULL,
    "cUsuariosAvatar" TEXT,
    "bUsuariosEstaEnLinea" BOOLEAN DEFAULT false,
    "dUsuariosUltimaConexion" TIMESTAMP WITH TIME ZONE,
    "dUsuariosFechaCreacion" TIMESTAMP WITH TIME ZONE DEFAULT now(),
    "cUsuariosPerCodigo" TEXT,
    "cUsuariosPerJurCodigo" TEXT,
    "cUsuariosUsername" TEXT,
    "cUsuariosPassword" TEXT,
    "bUsuariosActivo" BOOLEAN DEFAULT true,
    "bUsuarioVerificado" BOOLEAN DEFAULT false,
    "cUsuarioTokenVerificacion" TEXT,
    "dUsuarioCambioPassword" TIMESTAMP WITH TIME ZONE,
    "cUsuarioConfigPrivacidad" TEXT,
    "cUsuarioConfigNotificaciones" TEXT
);

-- Enable Row Level Security
ALTER TABLE public."Usuarios" ENABLE ROW LEVEL SECURITY;

-- Create policies for authenticated users
CREATE POLICY "Usuarios select policy" ON public."Usuarios" FOR SELECT TO authenticated USING (true);
CREATE POLICY "Usuarios insert policy" ON public."Usuarios" FOR INSERT TO authenticated WITH CHECK (true);
CREATE POLICY "Usuarios update policy" ON public."Usuarios" FOR UPDATE TO authenticated USING (true);
CREATE POLICY "Usuarios delete policy" ON public."Usuarios" FOR DELETE TO authenticated USING (true);

-- Grant permissions
GRANT SELECT, INSERT, UPDATE, DELETE ON public."Usuarios" TO authenticated;
GRANT SELECT ON public."Usuarios" TO anon;

-- Insert default user JSANCHEZ that the backend expects
INSERT INTO public."Usuarios" (
    "nUsuariosId", 
    "cUsuariosNombre", 
    "cUsuariosEmail", 
    "cUsuariosUsername",
    "bUsuariosActivo",
    "bUsuarioVerificado"
) VALUES (
    'JSANCHEZ',
    'Juan Sanchez',
    'jsanchez@example.com',
    'jsanchez',
    true,
    true
) ON CONFLICT ("nUsuariosId") DO NOTHING;