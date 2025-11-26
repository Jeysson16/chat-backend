-- Migration: Create new chat tables with Spanish naming convention and correct prefixes
-- Date: 2024-12-25
-- Description: Creates the new chat system tables with Spanish names and proper field prefixes

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
CREATE INDEX idx_MensajesChat_nMensajesChatConversacionId ON MensajesChat(nMensajesChatConversacionId);
CREATE INDEX idx_MensajesChat_cMensajesChatRemitenteId ON MensajesChat(cMensajesChatRemitenteId);
CREATE INDEX idx_MensajesChat_dMensajesChatFechaHora ON MensajesChat(dMensajesChatFechaHora);
CREATE INDEX idx_ParticipantesChat_nParticipantesChatConversacionId ON ParticipantesChat(nParticipantesChatConversacionId);
CREATE INDEX idx_ParticipantesChat_cParticipantesChatUsuarioId ON ParticipantesChat(cParticipantesChatUsuarioId);
CREATE INDEX idx_UsuariosChat_cUsuariosChatEmail ON UsuariosChat(cUsuariosChatEmail);

-- Enable Row Level Security (RLS)
ALTER TABLE UsuariosChat ENABLE ROW LEVEL SECURITY;
ALTER TABLE ConversacionesChat ENABLE ROW LEVEL SECURITY;
ALTER TABLE MensajesChat ENABLE ROW LEVEL SECURITY;
ALTER TABLE ParticipantesChat ENABLE ROW LEVEL SECURITY;

-- Create RLS policies for UsuariosChat
CREATE POLICY "Users can view all users" ON UsuariosChat FOR SELECT USING (true);
CREATE POLICY "Users can update their own profile" ON UsuariosChat FOR UPDATE USING (auth.uid()::text = cUsuariosChatId);
CREATE POLICY "Users can insert their own profile" ON UsuariosChat FOR INSERT WITH CHECK (auth.uid()::text = cUsuariosChatId);

-- Create RLS policies for ConversacionesChat
CREATE POLICY "Users can view conversations they participate in" ON ConversacionesChat FOR SELECT 
USING (
    EXISTS (
        SELECT 1 FROM ParticipantesChat 
        WHERE nParticipantesChatConversacionId = nConversacionesChatId 
        AND cParticipantesChatUsuarioId = auth.uid()::text 
        AND bParticipantesChatEstaActivo = true
    )
);

CREATE POLICY "Users can create conversations" ON ConversacionesChat FOR INSERT WITH CHECK (true);
CREATE POLICY "Users can update conversations they participate in" ON ConversacionesChat FOR UPDATE 
USING (
    EXISTS (
        SELECT 1 FROM ParticipantesChat 
        WHERE nParticipantesChatConversacionId = nConversacionesChatId 
        AND cParticipantesChatUsuarioId = auth.uid()::text 
        AND bParticipantesChatEstaActivo = true
    )
);

-- Create RLS policies for MensajesChat
CREATE POLICY "Users can view messages from conversations they participate in" ON MensajesChat FOR SELECT 
USING (
    EXISTS (
        SELECT 1 FROM ParticipantesChat 
        WHERE nParticipantesChatConversacionId = MensajesChat.nMensajesChatConversacionId 
        AND cParticipantesChatUsuarioId = auth.uid()::text 
        AND bParticipantesChatEstaActivo = true
    )
);

CREATE POLICY "Users can create messages in conversations they participate in" ON MensajesChat FOR INSERT 
WITH CHECK (
    cMensajesChatRemitenteId = auth.uid()::text AND
    EXISTS (
        SELECT 1 FROM ParticipantesChat 
        WHERE nParticipantesChatConversacionId = MensajesChat.nMensajesChatConversacionId 
        AND cParticipantesChatUsuarioId = auth.uid()::text 
        AND bParticipantesChatEstaActivo = true
    )
);

CREATE POLICY "Users can update their own messages" ON MensajesChat FOR UPDATE 
USING (cMensajesChatRemitenteId = auth.uid()::text);

-- Create RLS policies for ParticipantesChat
CREATE POLICY "Users can view participants from conversations they participate in" ON ParticipantesChat FOR SELECT 
USING (
    EXISTS (
        SELECT 1 FROM ParticipantesChat cp2 
        WHERE cp2.nParticipantesChatConversacionId = ParticipantesChat.nParticipantesChatConversacionId 
        AND cp2.cParticipantesChatUsuarioId = auth.uid()::text 
        AND cp2.bParticipantesChatEstaActivo = true
    )
);

CREATE POLICY "Users can add participants to conversations they admin" ON ParticipantesChat FOR INSERT 
WITH CHECK (
    EXISTS (
        SELECT 1 FROM ParticipantesChat 
        WHERE nParticipantesChatConversacionId = ParticipantesChat.nParticipantesChatConversacionId 
        AND cParticipantesChatUsuarioId = auth.uid()::text 
        AND cParticipantesChatRol = 'admin' 
        AND bParticipantesChatEstaActivo = true
    )
);

CREATE POLICY "Users can update participants in conversations they admin" ON ParticipantesChat FOR UPDATE 
USING (
    EXISTS (
        SELECT 1 FROM ParticipantesChat 
        WHERE nParticipantesChatConversacionId = ParticipantesChat.nParticipantesChatConversacionId 
        AND cParticipantesChatUsuarioId = auth.uid()::text 
        AND cParticipantesChatRol = 'admin' 
        AND bParticipantesChatEstaActivo = true
    )
);

-- Grant permissions to authenticated users
GRANT SELECT, INSERT, UPDATE ON UsuariosChat TO authenticated;
GRANT SELECT, INSERT, UPDATE ON ConversacionesChat TO authenticated;
GRANT SELECT, INSERT, UPDATE ON MensajesChat TO authenticated;
GRANT SELECT, INSERT, UPDATE ON ParticipantesChat TO authenticated;

-- Grant usage on sequences
GRANT USAGE ON SEQUENCE ConversacionesChat_nConversacionesChatId_seq TO authenticated;
GRANT USAGE ON SEQUENCE MensajesChat_nMensajesChatId_seq TO authenticated;
GRANT USAGE ON SEQUENCE ParticipantesChat_nParticipantesChatId_seq TO authenticated;

-- Grant basic read access to anon users (for public features if needed)
GRANT SELECT ON UsuariosChat TO anon;