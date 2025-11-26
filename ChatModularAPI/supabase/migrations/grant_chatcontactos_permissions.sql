-- Grant permissions for chatcontactos table to resolve access issues
-- This fixes the "relation 'public.chatcontactos' does not exist" error

-- Grant SELECT permission to anon role (for unauthenticated requests)
GRANT SELECT ON public.chatcontactos TO anon;

-- Grant all permissions to authenticated role (for authenticated requests)
GRANT ALL ON public.chatcontactos TO authenticated;

-- Grant SELECT permission to service_role (for server-side operations)
GRANT ALL ON public.chatcontactos TO service_role;

-- Check current permissions
SELECT grantee, table_name, privilege_type 
FROM information_schema.role_table_grants 
WHERE table_schema = 'public' 
AND table_name = 'chatcontactos' 
AND grantee IN ('anon', 'authenticated', 'service_role')
ORDER BY grantee;