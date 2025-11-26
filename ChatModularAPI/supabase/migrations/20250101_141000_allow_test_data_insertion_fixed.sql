-- Migración para permitir inserción de datos de prueba
-- Fecha: 2025-01-01 14:10:00

-- Eliminar políticas existentes si existen
DROP POLICY IF EXISTS "Allow test data insertion" ON "Usuarios";
DROP POLICY IF EXISTS "Allow test conversations insertion" ON "Conversaciones";
DROP POLICY IF EXISTS "Allow test participants insertion" ON "Participantes";
DROP POLICY IF EXISTS "Allow test messages insertion" ON "Mensajes";
DROP POLICY IF EXISTS "Allow test data reading usuarios" ON "Usuarios";
DROP POLICY IF EXISTS "Allow test data reading conversaciones" ON "Conversaciones";
DROP POLICY IF EXISTS "Allow test data reading participantes" ON "Participantes";
DROP POLICY IF EXISTS "Allow test data reading mensajes" ON "Mensajes";

-- Crear política temporal para permitir inserción de datos de prueba en Usuarios
CREATE POLICY "Allow test data insertion" ON "Usuarios" 
FOR INSERT WITH CHECK (true);

-- Crear política temporal para permitir inserción de datos de prueba en Conversaciones
CREATE POLICY "Allow test conversations insertion" ON "Conversaciones" 
FOR INSERT WITH CHECK (true);

-- Crear política temporal para permitir inserción de datos de prueba en Participantes
CREATE POLICY "Allow test participants insertion" ON "Participantes" 
FOR INSERT WITH CHECK (true);

-- Crear política temporal para permitir inserción de datos de prueba en Mensajes
CREATE POLICY "Allow test messages insertion" ON "Mensajes" 
FOR INSERT WITH CHECK (true);

-- Crear política para permitir lectura de todas las tablas para datos de prueba
CREATE POLICY "Allow test data reading usuarios" ON "Usuarios" 
FOR SELECT USING (true);

CREATE POLICY "Allow test data reading conversaciones" ON "Conversaciones" 
FOR SELECT USING (true);

CREATE POLICY "Allow test data reading participantes" ON "Participantes" 
FOR SELECT USING (true);

CREATE POLICY "Allow test data reading mensajes" ON "Mensajes" 
FOR SELECT USING (true);