-- Migration: Clean and recreate chat tables with correct nomenclature
-- Date: 2024-12-25
-- Description: Drops all existing chat tables and recreates with Spanish naming convention

-- Drop all existing chat tables (in correct order due to foreign keys)
DROP TABLE IF EXISTS ParticipantesChat CASCADE;
DROP TABLE IF EXISTS MensajesChat CASCADE;
DROP TABLE IF EXISTS ConversacionesChat CASCADE;
DROP TABLE IF EXISTS UsuariosChat CASCADE;

-- Drop any old tables with different naming
DROP TABLE IF EXISTS chatparticipants CASCADE;
DROP TABLE IF EXISTS chatmessages CASCADE;
DROP TABLE IF EXISTS chatconversations CASCADE;
DROP TABLE IF EXISTS chatusers CASCADE;
DROP TABLE IF EXISTS chatParticipants CASCADE;
DROP TABLE IF EXISTS chatMessages CASCADE;
DROP TABLE IF EXISTS chatConversations CASCADE;
DROP TABLE IF EXISTS chatUsers CASCADE;

-- Create UsuariosChat table
CREATE TABLE UsuariosChat (
    cUsuariosChatId VARCHAR(450) PRIMARY KEY,
    cUsuariosChatNombre VARCHAR(255) NOT NULL,
    cUsuariosChatEmail VARCHAR(255) NOT NULL,
    cUsuariosChatAvatar VARCHAR(500),
    bUsuariosChatEstaEnLinea BOOLEAN DEFAULT FALSE,
    dUsuariosChatUltimaVez TIMESTAMP WITH TIME ZONE,
    dUsuariosChatFechaCreacion TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- Create ConversacionesChat table
CREATE TABLE ConversacionesChat (
    nConversacionesChatId SERIAL PRIMARY KEY,
    cConversacionesChatNombre VARCHAR(255),
    cConversacionesChatTipo VARCHAR(50) NOT NULL CHECK (cConversacionesChatTipo IN ('individual', 'group')),
    dConversacionesChatFechaCreacion TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    dConversacionesChatFechaActualizacion TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    bConversacionesChatEstaActiva BOOLEAN DEFAULT TRUE
);

-- Create MensajesChat table
CREATE TABLE MensajesChat (
    nMensajesChatId SERIAL PRIMARY KEY,
    nMensajesChatConversacionId INTEGER NOT NULL,
    cMensajesChatRemitenteId VARCHAR(450) NOT NULL,
    cMensajesChatTexto TEXT,
    cMensajesChatTipo VARCHAR(50) DEFAULT 'text',
    dMensajesChatFechaHora TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    bMensajesChatEstaLeido BOOLEAN DEFAULT FALSE,
    FOREIGN KEY (nMensajesChatConversacionId) REFERENCES ConversacionesChat(nConversacionesChatId) ON DELETE CASCADE,
    FOREIGN KEY (cMensajesChatRemitenteId) REFERENCES UsuariosChat(cUsuariosChatId) ON DELETE CASCADE
);

-- Create ParticipantesChat table
CREATE TABLE ParticipantesChat (
    nParticipantesChatId SERIAL PRIMARY KEY,
    nParticipantesChatConversacionId INTEGER NOT NULL,
    cParticipantesChatUsuarioId VARCHAR(450) NOT NULL,
    cParticipantesChatRol VARCHAR(50) DEFAULT 'member' CHECK (cParticipantesChatRol IN ('admin', 'member')),
    dParticipantesChatFechaUnion TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    bParticipantesChatEstaActivo BOOLEAN DEFAULT TRUE,
    FOREIGN KEY (nParticipantesChatConversacionId) REFERENCES ConversacionesChat(nConversacionesChatId) ON DELETE CASCADE,
    FOREIGN KEY (cParticipantesChatUsuarioId) REFERENCES UsuariosChat(cUsuariosChatId) ON DELETE CASCADE,
    UNIQUE(nParticipantesChatConversacionId, cParticipantesChatUsuarioId)
);

-- Create indexes for better performance
CREATE INDEX idx_MensajesChat_conversacion ON MensajesChat(nMensajesChatConversacionId);
CREATE INDEX idx_MensajesChat_remitente ON MensajesChat(cMensajesChatRemitenteId);
CREATE INDEX idx_MensajesChat_fecha ON MensajesChat(dMensajesChatFechaHora);
CREATE INDEX idx_ParticipantesChat_conversacion ON ParticipantesChat(nParticipantesChatConversacionId);
CREATE INDEX idx_ParticipantesChat_usuario ON ParticipantesChat(cParticipantesChatUsuarioId);

-- Enable Row Level Security
ALTER TABLE UsuariosChat ENABLE ROW LEVEL SECURITY;
ALTER TABLE ConversacionesChat ENABLE ROW LEVEL SECURITY;
ALTER TABLE MensajesChat ENABLE ROW LEVEL SECURITY;
ALTER TABLE ParticipantesChat ENABLE ROW LEVEL SECURITY;

-- Create RLS policies
CREATE POLICY "Users can view their own data" ON UsuariosChat FOR SELECT USING (auth.uid()::text = cUsuariosChatId);
CREATE POLICY "Users can update their own data" ON UsuariosChat FOR UPDATE USING (auth.uid()::text = cUsuariosChatId);

CREATE POLICY "Participants can view conversations" ON ConversacionesChat FOR SELECT USING (
    EXISTS (
        SELECT 1 FROM ParticipantesChat 
        WHERE nParticipantesChatConversacionId = nConversacionesChatId 
        AND cParticipantesChatUsuarioId = auth.uid()::text
        AND bParticipantesChatEstaActivo = true
    )
);

CREATE POLICY "Participants can view messages" ON MensajesChat FOR SELECT USING (
    EXISTS (
        SELECT 1 FROM ParticipantesChat 
        WHERE nParticipantesChatConversacionId = nMensajesChatConversacionId 
        AND cParticipantesChatUsuarioId = auth.uid()::text
        AND bParticipantesChatEstaActivo = true
    )
);

CREATE POLICY "Participants can insert messages" ON MensajesChat FOR INSERT WITH CHECK (
    EXISTS (
        SELECT 1 FROM ParticipantesChat 
        WHERE nParticipantesChatConversacionId = nMensajesChatConversacionId 
        AND cParticipantesChatUsuarioId = auth.uid()::text
        AND bParticipantesChatEstaActivo = true
    )
    AND cMensajesChatRemitenteId = auth.uid()::text
);

CREATE POLICY "Participants can view participants" ON ParticipantesChat FOR SELECT USING (
    EXISTS (
        SELECT 1 FROM ParticipantesChat p2
        WHERE p2.nParticipantesChatConversacionId = nParticipantesChatConversacionId 
        AND p2.cParticipantesChatUsuarioId = auth.uid()::text
        AND p2.bParticipantesChatEstaActivo = true
    )
);