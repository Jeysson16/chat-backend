-- Add cUsuariosUsername column to Usuarios table
ALTER TABLE "Usuarios" 
ADD COLUMN IF NOT EXISTS "cUsuariosUsername" VARCHAR(255);

-- Create index for faster username lookups
CREATE INDEX IF NOT EXISTS "idx_usuarios_username" ON "Usuarios" ("cUsuariosUsername");