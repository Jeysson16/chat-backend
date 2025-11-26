-- Migración para eliminar tablas con nomenclatura antigua y aplicar las nuevas con nomenclatura corregida
-- Fecha: 2024-12-25
-- Descripción: Elimina las tablas chatusers, chatconversations, chatmessages, chatparticipants con nomenclatura antigua
--              y crea las nuevas tablas con nomenclatura corregida (c para strings, sin prefijo i para IDs)

-- Eliminar tablas con nomenclatura antigua
DROP TABLE IF EXISTS chatparticipants CASCADE;
DROP TABLE IF EXISTS chatmessages CASCADE;
DROP TABLE IF EXISTS chatconversations CASCADE;
DROP TABLE IF EXISTS chatusers CASCADE;

-- Eliminar secuencias asociadas
DROP SEQUENCE IF EXISTS chatconversations_ichatconversationsid_seq CASCADE;
DROP SEQUENCE IF EXISTS chatmessages_ichatmessagesid_seq CASCADE;
DROP SEQUENCE IF EXISTS chatparticipants_ichatparticipantsid_seq CASCADE;

-- Crear tabla chatUsers con nomenclatura corregida
CREATE TABLE chatUsers (
    cChatUsersId VARCHAR(255) PRIMARY KEY,
    cChatUsersName VARCHAR(255) NOT NULL,
    cChatUsersEmail VARCHAR(255) NOT NULL UNIQUE,
    cChatUsersAvatar VARCHAR(255),
    bChatUsersIsOnline BOOLEAN DEFAULT FALSE,
    dtChatUsersLastSeen TIMESTAMPTZ,
    dtChatUsersCreatedAt TIMESTAMPTZ DEFAULT NOW()
);

-- Crear tabla chatConversations con nomenclatura corregida
CREATE TABLE chatConversations (
    ChatConversationsId SERIAL PRIMARY KEY,
    cChatConversationsName VARCHAR(255),
    cChatConversationType VARCHAR(50) NOT NULL CHECK (cChatConversationType IN ('individual', 'group')),
    dtChatConversationsCreatedAt TIMESTAMPTZ DEFAULT NOW(),
    dtChatConversationsUpdatedAt TIMESTAMPTZ DEFAULT NOW(),
    bChatConversationsIsActive BOOLEAN DEFAULT TRUE
);

-- Crear tabla chatMessages con nomenclatura corregida
CREATE TABLE chatMessages (
    ChatMessagesId SERIAL PRIMARY KEY,
    ChatMessagesConversationId INTEGER NOT NULL,
    cChatMessagesSenderId VARCHAR(255) NOT NULL,
    cChatMessagesText TEXT,
    cChatMessagesType VARCHAR(50) DEFAULT 'text',
    dtChatMessagesTimestamp TIMESTAMPTZ DEFAULT NOW(),
    bChatMessagesIsRead BOOLEAN DEFAULT FALSE,
    FOREIGN KEY (ChatMessagesConversationId) REFERENCES chatConversations(ChatConversationsId) ON DELETE CASCADE,
    FOREIGN KEY (cChatMessagesSenderId) REFERENCES chatUsers(cChatUsersId) ON DELETE CASCADE
);

-- Crear tabla chatParticipants con nomenclatura corregida
CREATE TABLE chatParticipants (
    ChatParticipantsId SERIAL PRIMARY KEY,
    ChatParticipantsConversationId INTEGER NOT NULL,
    cChatParticipantsUserId VARCHAR(255) NOT NULL,
    cChatParticipantsRole VARCHAR(50) DEFAULT 'member' CHECK (cChatParticipantsRole IN ('admin', 'member')),
    dtChatParticipantsJoinedAt TIMESTAMPTZ DEFAULT NOW(),
    bChatParticipantsIsActive BOOLEAN DEFAULT TRUE,
    FOREIGN KEY (ChatParticipantsConversationId) REFERENCES chatConversations(ChatConversationsId) ON DELETE CASCADE,
    FOREIGN KEY (cChatParticipantsUserId) REFERENCES chatUsers(cChatUsersId) ON DELETE CASCADE,
    UNIQUE(ChatParticipantsConversationId, cChatParticipantsUserId)
);

-- Crear índices para optimizar consultas
CREATE INDEX idx_chatMessages_conversation ON chatMessages(ChatMessagesConversationId);
CREATE INDEX idx_chatMessages_sender ON chatMessages(cChatMessagesSenderId);
CREATE INDEX idx_chatMessages_timestamp ON chatMessages(dtChatMessagesTimestamp);
CREATE INDEX idx_chatParticipants_conversation ON chatParticipants(ChatParticipantsConversationId);
CREATE INDEX idx_chatParticipants_user ON chatParticipants(cChatParticipantsUserId);
CREATE INDEX idx_chatUsers_email ON chatUsers(cChatUsersEmail);

-- Habilitar RLS (Row Level Security) en todas las tablas
ALTER TABLE chatUsers ENABLE ROW LEVEL SECURITY;
ALTER TABLE chatConversations ENABLE ROW LEVEL SECURITY;
ALTER TABLE chatMessages ENABLE ROW LEVEL SECURITY;
ALTER TABLE chatParticipants ENABLE ROW LEVEL SECURITY;

-- Políticas RLS para chatUsers
CREATE POLICY "Users can view all users" ON chatUsers FOR SELECT USING (true);
CREATE POLICY "Users can update their own profile" ON chatUsers FOR UPDATE USING (auth.uid()::text = cChatUsersId);
CREATE POLICY "Users can insert their own profile" ON chatUsers FOR INSERT WITH CHECK (auth.uid()::text = cChatUsersId);

-- Políticas RLS para chatConversations
CREATE POLICY "Users can view conversations they participate in" ON chatConversations FOR SELECT USING (
    EXISTS (
        SELECT 1 FROM chatParticipants 
        WHERE ChatParticipantsConversationId = ChatConversationsId 
        AND cChatParticipantsUserId = auth.uid()::text
        AND bChatParticipantsIsActive = true
    )
);

CREATE POLICY "Users can create conversations" ON chatConversations FOR INSERT WITH CHECK (true);

CREATE POLICY "Participants can update conversations" ON chatConversations FOR UPDATE USING (
    EXISTS (
        SELECT 1 FROM chatParticipants 
        WHERE ChatParticipantsConversationId = ChatConversationsId 
        AND cChatParticipantsUserId = auth.uid()::text
        AND bChatParticipantsIsActive = true
    )
);

-- Políticas RLS para chatMessages
CREATE POLICY "Users can view messages in their conversations" ON chatMessages FOR SELECT USING (
    EXISTS (
        SELECT 1 FROM chatParticipants 
        WHERE ChatParticipantsConversationId = ChatMessagesConversationId 
        AND cChatParticipantsUserId = auth.uid()::text
        AND bChatParticipantsIsActive = true
    )
);

CREATE POLICY "Users can send messages to their conversations" ON chatMessages FOR INSERT WITH CHECK (
    EXISTS (
        SELECT 1 FROM chatParticipants 
        WHERE ChatParticipantsConversationId = ChatMessagesConversationId 
        AND cChatParticipantsUserId = auth.uid()::text
        AND bChatParticipantsIsActive = true
    )
    AND cChatMessagesSenderId = auth.uid()::text
);

-- Políticas RLS para chatParticipants
CREATE POLICY "Users can view participants in their conversations" ON chatParticipants FOR SELECT USING (
    EXISTS (
        SELECT 1 FROM chatParticipants cp2
        WHERE cp2.ChatParticipantsConversationId = ChatParticipantsConversationId 
        AND cp2.cChatParticipantsUserId = auth.uid()::text
        AND cp2.bChatParticipantsIsActive = true
    )
);

CREATE POLICY "Users can add participants to conversations they're in" ON chatParticipants FOR INSERT WITH CHECK (
    EXISTS (
        SELECT 1 FROM chatParticipants 
        WHERE ChatParticipantsConversationId = ChatParticipantsConversationId 
        AND cChatParticipantsUserId = auth.uid()::text
        AND bChatParticipantsIsActive = true
    )
);

-- Otorgar permisos a usuarios autenticados
GRANT ALL ON chatUsers TO authenticated;
GRANT ALL ON chatConversations TO authenticated;
GRANT ALL ON chatMessages TO authenticated;
GRANT ALL ON chatParticipants TO authenticated;

-- Otorgar permisos en secuencias
GRANT USAGE, SELECT ON SEQUENCE chatConversations_ChatConversationsId_seq TO authenticated;
GRANT USAGE, SELECT ON SEQUENCE chatMessages_ChatMessagesId_seq TO authenticated;
GRANT USAGE, SELECT ON SEQUENCE chatParticipants_ChatParticipantsId_seq TO authenticated;

-- Otorgar permisos a usuarios anónimos (solo lectura limitada)
GRANT SELECT ON chatUsers TO anon;
GRANT SELECT ON chatConversations TO anon;
GRANT SELECT ON chatMessages TO anon;
GRANT SELECT ON chatParticipants TO anon;