-- Verificar las columnas reales de la tabla ChatContacto
SELECT 
    column_name,
    data_type,
    is_nullable,
    column_default
FROM information_schema.columns 
WHERE table_schema = 'public' 
AND table_name = 'ChatContacto'
ORDER BY ordinal_position;

-- También verificar con mayúsculas/minúsculas
SELECT 
    column_name,
    data_type,
    is_nullable,
    column_default
FROM information_schema.columns 
WHERE table_schema = 'public' 
AND lower(table_name) = 'chatcontacto'
ORDER BY ordinal_position;