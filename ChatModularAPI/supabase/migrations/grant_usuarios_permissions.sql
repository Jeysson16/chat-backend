-- Grant permissions on Usuarios table to anon and authenticated roles
GRANT SELECT, INSERT, UPDATE ON public."Usuarios" TO anon;
GRANT SELECT, INSERT, UPDATE, DELETE ON public."Usuarios" TO authenticated;