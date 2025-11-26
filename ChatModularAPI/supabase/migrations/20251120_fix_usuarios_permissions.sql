-- Fix Usuarios table permissions and RLS policies so PostgREST can see it

-- Enable Row Level Security if not already enabled
ALTER TABLE public."Usuarios" ENABLE ROW LEVEL SECURITY;

-- Drop existing policies if they exist
DROP POLICY IF EXISTS "Usuarios select policy" ON public."Usuarios";
DROP POLICY IF EXISTS "Usuarios insert policy" ON public."Usuarios";
DROP POLICY IF EXISTS "Usuarios update policy" ON public."Usuarios";
DROP POLICY IF EXISTS "Usuarios delete policy" ON public."Usuarios";

-- Create policies for authenticated users (full access)
CREATE POLICY "Usuarios select policy" ON public."Usuarios" 
    FOR SELECT TO authenticated 
    USING (true);

CREATE POLICY "Usuarios insert policy" ON public."Usuarios" 
    FOR INSERT TO authenticated 
    WITH CHECK (true);

CREATE POLICY "Usuarios update policy" ON public."Usuarios" 
    FOR UPDATE TO authenticated 
    USING (true);

CREATE POLICY "Usuarios delete policy" ON public."Usuarios" 
    FOR DELETE TO authenticated 
    USING (true);

-- Grant permissions to roles
GRANT SELECT, INSERT, UPDATE, DELETE ON public."Usuarios" TO authenticated;
GRANT SELECT ON public."Usuarios" TO anon;

-- Ensure primary key is properly defined
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'usuarios_pk') THEN
        ALTER TABLE public."Usuarios" 
            ADD CONSTRAINT usuarios_pk PRIMARY KEY ("nUsuariosId");
    END IF;
END $$;

-- Create index on commonly queried fields for better performance
CREATE INDEX IF NOT EXISTS idx_usuarios_email ON public."Usuarios" ("cUsuariosEmail");
CREATE INDEX IF NOT EXISTS idx_usuarios_username ON public."Usuarios" ("cUsuariosUsername");
CREATE INDEX IF NOT EXISTS idx_usuarios_online ON public."Usuarios" ("bUsuariosEstaEnLinea");

-- Insert or update the default JSANCHEZ user
INSERT INTO public."Usuarios" (
    "nUsuariosId", 
    "cUsuariosNombre", 
    "cUsuariosEmail", 
    "cUsuariosUsername",
    "bUsuariosActivo",
    "bUsuarioVerificado",
    "dUsuariosFechaCreacion"
) VALUES (
    'JSANCHEZ',
    'Juan Sanchez',
    'jsanchez@example.com',
    'jsanchez',
    true,
    true,
    now()
) ON CONFLICT ("nUsuariosId") DO UPDATE SET
    "cUsuariosNombre" = EXCLUDED."cUsuariosNombre",
    "cUsuariosEmail" = EXCLUDED."cUsuariosEmail",
    "cUsuariosUsername" = EXCLUDED."cUsuariosUsername",
    "bUsuariosActivo" = EXCLUDED."bUsuariosActivo",
    "bUsuarioVerificado" = EXCLUDED."bUsuarioVerificado";