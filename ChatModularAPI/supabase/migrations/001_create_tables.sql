-- =============================================
-- Chat Modular API - Database Schema
-- Supabase Migration Script
-- =============================================

-- Enable necessary extensions
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
CREATE EXTENSION IF NOT EXISTS "pgcrypto";

-- =============================================
-- Table: chat_usuarios
-- =============================================
CREATE TABLE IF NOT EXISTS public.chat_usuarios (
    c UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    n VARCHAR(100) NOT NULL,
    d TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    email VARCHAR(255) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100),
    telefono VARCHAR(20),
    avatar_url TEXT,
    estado VARCHAR(20) DEFAULT 'activo' CHECK (estado IN ('activo', 'inactivo', 'suspendido')),
    ultimo_acceso TIMESTAMP WITH TIME ZONE,
    configuracion_privacidad JSONB DEFAULT '{"mostrar_estado": true, "permitir_mensajes": true}',
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- =============================================
-- Table: chat_conversaciones
-- =============================================
CREATE TABLE IF NOT EXISTS public.chat_conversaciones (
    c UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    n VARCHAR(200) NOT NULL,
    d TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    tipo VARCHAR(20) DEFAULT 'privada' CHECK (tipo IN ('privada', 'grupo', 'canal')),
    descripcion TEXT,
    imagen_url TEXT,
    configuracion JSONB DEFAULT '{}',
    estado VARCHAR(20) DEFAULT 'activa' CHECK (estado IN ('activa', 'archivada', 'eliminada')),
    creado_por UUID REFERENCES public.chat_usuarios(c),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- =============================================
-- Table: chat_usuarios_conversaciones (Many-to-Many)
-- =============================================
CREATE TABLE IF NOT EXISTS public.chat_usuarios_conversaciones (
    c UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    usuario_id UUID NOT NULL REFERENCES public.chat_usuarios(c) ON DELETE CASCADE,
    conversacion_id UUID NOT NULL REFERENCES public.chat_conversaciones(c) ON DELETE CASCADE,
    rol VARCHAR(20) DEFAULT 'miembro' CHECK (rol IN ('admin', 'moderador', 'miembro')),
    fecha_union TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    fecha_salida TIMESTAMP WITH TIME ZONE,
    estado VARCHAR(20) DEFAULT 'activo' CHECK (estado IN ('activo', 'silenciado', 'bloqueado')),
    permisos JSONB DEFAULT '{"puede_escribir": true, "puede_invitar": false}',
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    UNIQUE(usuario_id, conversacion_id)
);

-- =============================================
-- Table: chat_mensajes
-- =============================================
CREATE TABLE IF NOT EXISTS public.chat_mensajes (
    c UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    n TEXT NOT NULL,
    d TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    conversacion_id UUID NOT NULL REFERENCES public.chat_conversaciones(c) ON DELETE CASCADE,
    usuario_id UUID NOT NULL REFERENCES public.chat_usuarios(c),
    tipo VARCHAR(20) DEFAULT 'texto' CHECK (tipo IN ('texto', 'imagen', 'archivo', 'audio', 'video', 'sistema')),
    contenido TEXT NOT NULL,
    metadata JSONB DEFAULT '{}',
    archivo_url TEXT,
    archivo_tipo VARCHAR(50),
    archivo_tamaño INTEGER,
    mensaje_padre_id UUID REFERENCES public.chat_mensajes(c),
    editado BOOLEAN DEFAULT FALSE,
    fecha_edicion TIMESTAMP WITH TIME ZONE,
    estado VARCHAR(20) DEFAULT 'enviado' CHECK (estado IN ('enviado', 'entregado', 'leido', 'eliminado')),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- =============================================
-- Table: chat_mensajes_lecturas
-- =============================================
CREATE TABLE IF NOT EXISTS public.chat_mensajes_lecturas (
    c UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    mensaje_id UUID NOT NULL REFERENCES public.chat_mensajes(c) ON DELETE CASCADE,
    usuario_id UUID NOT NULL REFERENCES public.chat_usuarios(c) ON DELETE CASCADE,
    fecha_lectura TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    UNIQUE(mensaje_id, usuario_id)
);

-- =============================================
-- Table: app_registros (Audit Log)
-- =============================================
CREATE TABLE IF NOT EXISTS public.app_registros (
    c UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    n VARCHAR(200) NOT NULL,
    d TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    ticket VARCHAR(50) UNIQUE NOT NULL,
    nivel VARCHAR(20) DEFAULT 'info' CHECK (nivel IN ('trace', 'debug', 'info', 'warn', 'error', 'fatal')),
    categoria VARCHAR(100) NOT NULL,
    mensaje TEXT NOT NULL,
    excepcion TEXT,
    stack_trace TEXT,
    usuario_id UUID REFERENCES public.chat_usuarios(c),
    usuario_nombre VARCHAR(100),
    cliente_ip VARCHAR(45),
    cliente_user_agent TEXT,
    cliente_dispositivo VARCHAR(100),
    request_id VARCHAR(100),
    request_url TEXT,
    request_method VARCHAR(10),
    request_body TEXT,
    response_status INTEGER,
    response_time_ms INTEGER,
    procesado BOOLEAN DEFAULT FALSE,
    metadata JSONB DEFAULT '{}',
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- =============================================
-- Table: token_registros
-- =============================================
CREATE TABLE IF NOT EXISTS public.token_registros (
    c UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    n VARCHAR(100) NOT NULL,
    d TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    usuario_id UUID NOT NULL REFERENCES public.chat_usuarios(c) ON DELETE CASCADE,
    token_hash VARCHAR(255) NOT NULL,
    tipo VARCHAR(20) DEFAULT 'access' CHECK (tipo IN ('access', 'refresh', 'reset')),
    expira_en TIMESTAMP WITH TIME ZONE NOT NULL,
    revocado BOOLEAN DEFAULT FALSE,
    fecha_revocacion TIMESTAMP WITH TIME ZONE,
    ip_origen VARCHAR(45),
    user_agent TEXT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- =============================================
-- Table: webhooks_registros
-- =============================================
CREATE TABLE IF NOT EXISTS public.webhooks_registros (
    c UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    n VARCHAR(200) NOT NULL,
    d TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    url TEXT NOT NULL,
    evento VARCHAR(100) NOT NULL,
    payload JSONB NOT NULL,
    headers JSONB DEFAULT '{}',
    estado VARCHAR(20) DEFAULT 'pendiente' CHECK (estado IN ('pendiente', 'enviado', 'fallido', 'reintentando')),
    intentos INTEGER DEFAULT 0,
    max_intentos INTEGER DEFAULT 3,
    respuesta_codigo INTEGER,
    respuesta_body TEXT,
    fecha_envio TIMESTAMP WITH TIME ZONE,
    fecha_proximo_intento TIMESTAMP WITH TIME ZONE,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- =============================================
-- Indexes for Performance
-- =============================================

-- chat_usuarios indexes
CREATE INDEX IF NOT EXISTS idx_chat_usuarios_email ON public.chat_usuarios(email);
CREATE INDEX IF NOT EXISTS idx_chat_usuarios_estado ON public.chat_usuarios(estado);
CREATE INDEX IF NOT EXISTS idx_chat_usuarios_ultimo_acceso ON public.chat_usuarios(ultimo_acceso);

-- chat_conversaciones indexes
CREATE INDEX IF NOT EXISTS idx_chat_conversaciones_tipo ON public.chat_conversaciones(tipo);
CREATE INDEX IF NOT EXISTS idx_chat_conversaciones_estado ON public.chat_conversaciones(estado);
CREATE INDEX IF NOT EXISTS idx_chat_conversaciones_creado_por ON public.chat_conversaciones(creado_por);

-- chat_usuarios_conversaciones indexes
CREATE INDEX IF NOT EXISTS idx_chat_usuarios_conv_usuario ON public.chat_usuarios_conversaciones(usuario_id);
CREATE INDEX IF NOT EXISTS idx_chat_usuarios_conv_conversacion ON public.chat_usuarios_conversaciones(conversacion_id);
CREATE INDEX IF NOT EXISTS idx_chat_usuarios_conv_estado ON public.chat_usuarios_conversaciones(estado);

-- chat_mensajes indexes
CREATE INDEX IF NOT EXISTS idx_chat_mensajes_conversacion ON public.chat_mensajes(conversacion_id);
CREATE INDEX IF NOT EXISTS idx_chat_mensajes_usuario ON public.chat_mensajes(usuario_id);
CREATE INDEX IF NOT EXISTS idx_chat_mensajes_fecha ON public.chat_mensajes(d);
CREATE INDEX IF NOT EXISTS idx_chat_mensajes_tipo ON public.chat_mensajes(tipo);
CREATE INDEX IF NOT EXISTS idx_chat_mensajes_estado ON public.chat_mensajes(estado);

-- chat_mensajes_lecturas indexes
CREATE INDEX IF NOT EXISTS idx_chat_lecturas_mensaje ON public.chat_mensajes_lecturas(mensaje_id);
CREATE INDEX IF NOT EXISTS idx_chat_lecturas_usuario ON public.chat_mensajes_lecturas(usuario_id);

-- app_registros indexes
CREATE INDEX IF NOT EXISTS idx_app_registros_ticket ON public.app_registros(ticket);
CREATE INDEX IF NOT EXISTS idx_app_registros_nivel ON public.app_registros(nivel);
CREATE INDEX IF NOT EXISTS idx_app_registros_categoria ON public.app_registros(categoria);
CREATE INDEX IF NOT EXISTS idx_app_registros_fecha ON public.app_registros(d);
CREATE INDEX IF NOT EXISTS idx_app_registros_usuario ON public.app_registros(usuario_id);
CREATE INDEX IF NOT EXISTS idx_app_registros_procesado ON public.app_registros(procesado);

-- token_registros indexes
CREATE INDEX IF NOT EXISTS idx_token_registros_usuario ON public.token_registros(usuario_id);
CREATE INDEX IF NOT EXISTS idx_token_registros_tipo ON public.token_registros(tipo);
CREATE INDEX IF NOT EXISTS idx_token_registros_expira ON public.token_registros(expira_en);
CREATE INDEX IF NOT EXISTS idx_token_registros_revocado ON public.token_registros(revocado);

-- webhooks_registros indexes
CREATE INDEX IF NOT EXISTS idx_webhooks_estado ON public.webhooks_registros(estado);
CREATE INDEX IF NOT EXISTS idx_webhooks_evento ON public.webhooks_registros(evento);
CREATE INDEX IF NOT EXISTS idx_webhooks_fecha_envio ON public.webhooks_registros(fecha_envio);
CREATE INDEX IF NOT EXISTS idx_webhooks_proximo_intento ON public.webhooks_registros(fecha_proximo_intento);

-- =============================================
-- Triggers for updated_at
-- =============================================

-- Function to update updated_at timestamp
CREATE OR REPLACE FUNCTION update_updated_at_column()
RETURNS TRIGGER AS $$
BEGIN
    NEW.updated_at = NOW();
    RETURN NEW;
END;
$$ language 'plpgsql';

-- Apply triggers
CREATE TRIGGER update_chat_usuarios_updated_at BEFORE UPDATE ON public.chat_usuarios FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();
CREATE TRIGGER update_chat_conversaciones_updated_at BEFORE UPDATE ON public.chat_conversaciones FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();
CREATE TRIGGER update_chat_mensajes_updated_at BEFORE UPDATE ON public.chat_mensajes FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();
CREATE TRIGGER update_webhooks_registros_updated_at BEFORE UPDATE ON public.webhooks_registros FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();