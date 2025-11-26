-- Add bUsuarioVerificado column to Usuarios table
ALTER TABLE public."Usuarios" 
ADD COLUMN "bUsuarioVerificado" boolean DEFAULT false NOT NULL;