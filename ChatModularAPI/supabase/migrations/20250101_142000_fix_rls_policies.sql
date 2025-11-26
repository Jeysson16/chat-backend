-- Migración para corregir políticas RLS que causan recursión infinita
-- Fecha: 2025-01-01 14:20:00

-- Deshabilitar temporalmente RLS en todas las tablas de chat para permitir inserción de datos de prueba
ALTER TABLE "Usuarios" DISABLE ROW LEVEL SECURITY;
ALTER TABLE "Conversaciones" DISABLE ROW LEVEL SECURITY;
ALTER TABLE "Participantes" DISABLE ROW LEVEL SECURITY;
ALTER TABLE "Mensajes" DISABLE ROW LEVEL SECURITY;

-- Eliminar todas las políticas existentes para evitar conflictos
DROP POLICY IF EXISTS "Allow test data insertion" ON "Usuarios";
DROP POLICY IF EXISTS "Allow test conversations insertion" ON "Conversaciones";
DROP POLICY IF EXISTS "Allow test participants insertion" ON "Participantes";
DROP POLICY IF EXISTS "Allow test messages insertion" ON "Mensajes";
DROP POLICY IF EXISTS "Allow test data reading usuarios" ON "Usuarios";
DROP POLICY IF EXISTS "Allow test data reading conversaciones" ON "Conversaciones";
DROP POLICY IF EXISTS "Allow test data reading participantes" ON "Participantes";
DROP POLICY IF EXISTS "Allow test data reading mensajes" ON "Mensajes";

-- Eliminar políticas que pueden causar recursión
DROP POLICY IF EXISTS "Users can view users in their company" ON "Usuarios";
DROP POLICY IF EXISTS "Users can update their own data" ON "Usuarios";
DROP POLICY IF EXISTS "Users can view their contacts" ON "Contactos";
DROP POLICY IF EXISTS "Users can create contact requests" ON "Contactos";
DROP POLICY IF EXISTS "Users can update their contact requests" ON "Contactos";