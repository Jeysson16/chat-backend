-- Remueve usuario de conversación en tabla PascalCase Participantes

CREATE OR REPLACE FUNCTION public."USP_Chat_RemoveUserFromConversation"(
  conversationid integer,
  usuarioid varchar
)
RETURNS boolean
LANGUAGE plpgsql
AS $$
DECLARE
  deleted_count integer;
BEGIN
  DELETE FROM public."Participantes"
  WHERE "nParticipantesConversacionId" = conversationid
    AND "cParticipantesUsuarioId" = usuarioid;
  GET DIAGNOSTICS deleted_count = ROW_COUNT;
  RETURN deleted_count > 0;
END;
$$;

GRANT EXECUTE ON FUNCTION public."USP_Chat_RemoveUserFromConversation"(integer, varchar) TO authenticated;
GRANT EXECUTE ON FUNCTION public."USP_Chat_RemoveUserFromConversation"(integer, varchar) TO anon;
