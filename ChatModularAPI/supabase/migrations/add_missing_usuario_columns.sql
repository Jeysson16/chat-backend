-- Add missing columns to Usuarios table
ALTER TABLE public."Usuarios" 
ADD COLUMN "bUsuarioVerificado" boolean DEFAULT false NOT NULL,
ADD COLUMN "cUsuarioTokenVerificacion" varchar(255) DEFAULT NULL,
ADD COLUMN "dUsuarioCambioPassword" timestamptz DEFAULT NULL,
ADD COLUMN "cUsuarioConfigPrivacidad" varchar(500) DEFAULT NULL,
ADD COLUMN "cUsuarioConfigNotificaciones" varchar(500) DEFAULT NULL;