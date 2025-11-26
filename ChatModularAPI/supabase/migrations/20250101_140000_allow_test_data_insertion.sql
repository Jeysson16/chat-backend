-- Migración para permitir inserción de datos de prueba
-- Fecha: 2025-01-01 14:00:00

-- Crear política temporal para permitir inserción de datos de prueba en Usuarios
CREATE POLICY IF NOT EXISTS "Allow test data insertion" ON "Usuarios" 
FOR INSERT WITH CHECK (true);

-- Crear política temporal para permitir inserción de datos de prueba en Conversaciones
CREATE POLICY IF NOT EXISTS "Allow test conversations insertion" ON "Conversaciones" 
FOR INSERT WITH CHECK (true);

-- Crear política temporal para permitir inserción de datos de prueba en Participantes
CREATE POLICY IF NOT EXISTS "Allow test participants insertion" ON "Participantes" 
FOR INSERT WITH CHECK (true);

-- Crear política temporal para permitir inserción de datos de prueba en Mensajes
CREATE POLICY IF NOT EXISTS "Allow test messages insertion" ON "Mensajes" 
FOR INSERT WITH CHECK (true);

-- Crear política para permitir lectura de todas las tablas para datos de prueba
CREATE POLICY IF NOT EXISTS "Allow test data reading usuarios" ON "Usuarios" 
FOR SELECT USING (true);

CREATE POLICY IF NOT EXISTS "Allow test data reading conversaciones" ON "Conversaciones" 
FOR SELECT USING (true);

CREATE POLICY IF NOT EXISTS "Allow test data reading participantes" ON "Participantes" 
FOR SELECT USING (true);

CREATE POLICY IF NOT EXISTS "Allow test data reading mensajes" ON "Mensajes" 
FOR SELECT USING (true);