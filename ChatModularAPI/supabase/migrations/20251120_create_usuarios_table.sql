-- Create Usuarios table for chat application
CREATE TABLE public.Usuarios (
    nUsuariosId TEXT PRIMARY KEY DEFAULT gen_random_uuid()::text,
    cUsuariosNombre TEXT NOT NULL,
    cUsuariosEmail TEXT NOT NULL UNIQUE,
    cUsuariosAvatar TEXT,
    bUsuariosActivo BOOLEAN DEFAULT true,
    bUsuariosEstaEnLinea BOOLEAN DEFAULT false,
    dUsuariosUltimaConexion TIMESTAMP WITH TIME ZONE,
    cUsuariosUsername TEXT UNIQUE,
    cUsuariosPassword TEXT,
    cUsuariosRol TEXT DEFAULT 'USER',
    cUsuariosPerJurCodigo TEXT,
    cUsuariosPerCodigo TEXT,
    cUsuariosAppCodigo TEXT,
    cUsuarioTokenVerificacion TEXT,
    cUsuarioTokenReset TEXT,
    dUsuarioTokenResetExpiracion TIMESTAMP WITH TIME ZONE,
    dUsuarioCambioPassword TIMESTAMP WITH TIME ZONE,
    cUsuarioConfigPrivacidad TEXT DEFAULT '{}',
    cUsuarioConfigNotificaciones TEXT DEFAULT '{}',
    bUsuarioVerificado BOOLEAN DEFAULT false,
    dFechaCreacion TIMESTAMP WITH TIME ZONE DEFAULT now(),
    dFechaActualizacion TIMESTAMP WITH TIME ZONE DEFAULT now()
);

-- Create indexes for better performance
CREATE INDEX idx_usuarios_email ON public.Usuarios(cUsuariosEmail);
CREATE INDEX idx_usuarios_username ON public.Usuarios(cUsuariosUsername);
CREATE INDEX idx_usuarios_activo ON public.Usuarios(bUsuariosActivo);
CREATE INDEX idx_usuarios_enlinea ON public.Usuarios(bUsuariosEstaEnLinea);

-- Enable Row Level Security
ALTER TABLE public.Usuarios ENABLE ROW LEVEL SECURITY;

-- Create policies for authenticated users
CREATE POLICY "Usuarios select policy" ON public.Usuarios FOR SELECT TO authenticated USING (true);
CREATE POLICY "Usuarios insert policy" ON public.Usuarios FOR INSERT TO authenticated WITH CHECK (true);
CREATE POLICY "Usuarios update policy" ON public.Usuarios FOR UPDATE TO authenticated USING (true);
CREATE POLICY "Usuarios delete policy" ON public.Usuarios FOR DELETE TO authenticated USING (true);

-- Grant permissions
GRANT SELECT ON public.Usuarios TO anon;
GRANT SELECT, INSERT, UPDATE, DELETE ON public.Usuarios TO authenticated;

-- Insert default admin user (JSANCHEZ) that the backend expects
INSERT INTO public.Usuarios (
    nUsuariosId,
    cUsuariosNombre,
    cUsuariosEmail,
    cUsuariosUsername,
    cUsuariosPassword,
    cUsuariosRol,
    bUsuariosActivo,
    cUsuariosAppCodigo
) VALUES (
    '1000000001',
    'Jeysson',
    'jsanchez@example.com',
    'JSANCHEZ',
    '$2a$11$YourHashedPasswordHere',
    'ADMIN',
    true,
    'SICOM_CHAT_2024'
);