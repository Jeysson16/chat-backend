-- Add missing columns to Usuarios table (excluding bUsuarioVerificado which already exists)
ALTER TABLE public."Usuarios" 
ADD COLUMN "cUsuarioTokenVerificacion" varchar(255) DEFAULT NULL,
ADD COLUMN "dUsuarioCambioPassword" timestamptz DEFAULT NULL,
ADD COLUMN "cUsuarioConfigPrivacidad" varchar(500) DEFAULT NULL,
ADD COLUMN "cUsuarioConfigNotificaciones" varchar(500) DEFAULT NULL;