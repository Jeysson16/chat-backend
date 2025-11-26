-- Stored procedure to get user conversations
CREATE OR REPLACE FUNCTION USP_Chat_GetUserConversations(
    cAppCodigo VARCHAR(100),
    cUsuarioId VARCHAR(50),
    nPage INTEGER DEFAULT 1,
    nPageSize INTEGER DEFAULT 20
)
RETURNS TABLE (
    nConversacionesChatId INTEGER,
    cConversacionesChatAppCodigo VARCHAR(100),
    cConversacionesChatNombre VARCHAR(255),
    cConversacionesChatTipo VARCHAR(50),
    cConversacionesChatUsuarioCreadorId VARCHAR(50),
    dConversacionesChatFechaCreacion TIMESTAMP,
    dConversacionesChatUltimaActividad TIMESTAMP,
    bConversacionesChatEstaActiva BOOLEAN,
    nTotalCount INTEGER
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        cc.nConversacionesChatId,
        cc.cConversacionesChatAppCodigo,
        cc.cConversacionesChatNombre,
        cc.cConversacionesChatTipo,
        cc.cConversacionesChatUsuarioCreadorId,
        cc.dConversacionesChatFechaCreacion,
        cc.dConversacionesChatUltimaActividad,
        cc.bConversacionesChatEstaActiva,
        COUNT(*) OVER()::INTEGER as nTotalCount
    FROM ConversacionesChat cc
    INNER JOIN ParticipantesChat pc ON cc.nConversacionesChatId = pc.nParticipantesChatConversacionId
    WHERE cc.cConversacionesChatAppCodigo = cAppCodigo
    AND pc.cParticipantesChatUsuarioId = cUsuarioId
    AND cc.bConversacionesChatEstaActiva = true
    ORDER BY cc.dConversacionesChatUltimaActividad DESC
    LIMIT nPageSize
    OFFSET (nPage - 1) * nPageSize;
END;
$$ LANGUAGE plpgsql;

-- Grant permissions
GRANT EXECUTE ON FUNCTION USP_Chat_GetUserConversations(VARCHAR, VARCHAR, INTEGER, INTEGER) TO authenticated;
GRANT EXECUTE ON FUNCTION USP_Chat_GetUserConversations(VARCHAR, VARCHAR, INTEGER, INTEGER) TO anon;