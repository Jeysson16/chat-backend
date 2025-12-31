-- Unificar funciones de participantes para usar esquema CamelCase (ParticipantesChat)

-- Agregar usuario a conversación en ParticipantesChat
CREATE OR REPLACE FUNCTION public."USP_Chat_AddUserToConversation"(
  conversationid integer,
  usuarioid varchar
)
RETURNS boolean
LANGUAGE plpgsql
AS $$
BEGIN
  BEGIN
    INSERT INTO public."ParticipantesChat" (
      "nParticipantesChatConversacionId",
      "cParticipantesChatUsuarioId",
      "bParticipantesChatEstaActivo"
    ) VALUES (
      conversationid,
      usuarioid,
      TRUE
    );
  EXCEPTION WHEN unique_violation THEN
    UPDATE public."ParticipantesChat"
      SET "bParticipantesChatEstaActivo" = TRUE
      WHERE "nParticipantesChatConversacionId" = conversationid
        AND "cParticipantesChatUsuarioId" = usuarioid;
    RETURN TRUE;
  END;
  RETURN TRUE;
END;
$$;

GRANT EXECUTE ON FUNCTION public."USP_Chat_AddUserToConversation"(integer, varchar) TO authenticated;
GRANT EXECUTE ON FUNCTION public."USP_Chat_AddUserToConversation"(integer, varchar) TO anon;

-- Remover usuario de conversación (marcar inactivo) en ParticipantesChat
CREATE OR REPLACE FUNCTION public."USP_Chat_RemoveUserFromConversation"(
  conversationid integer,
  usuarioid varchar
)
RETURNS boolean
LANGUAGE plpgsql
AS $$
DECLARE
  updated_count integer;
BEGIN
  UPDATE public."ParticipantesChat"
    SET "bParticipantesChatEstaActivo" = FALSE
    WHERE "nParticipantesChatConversacionId" = conversationid
      AND "cParticipantesChatUsuarioId" = usuarioid;
  GET DIAGNOSTICS updated_count = ROW_COUNT;
  RETURN updated_count > 0;
END;
$$;

GRANT EXECUTE ON FUNCTION public."USP_Chat_RemoveUserFromConversation"(integer, varchar) TO authenticated;
GRANT EXECUTE ON FUNCTION public."USP_Chat_RemoveUserFromConversation"(integer, varchar) TO anon;
