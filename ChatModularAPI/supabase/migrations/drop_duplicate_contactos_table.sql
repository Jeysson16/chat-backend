-- =============================================
-- Drop the duplicate contactos table
-- We now use chatcontactos instead
-- =============================================

DROP TABLE IF EXISTS public.contactos;

SELECT 'Tabla contactos duplicada eliminada exitosamente' AS resultado;