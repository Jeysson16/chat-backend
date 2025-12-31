-- Agrega usuario a conversación en tabla PascalCase Participantes

CREATE OR REPLACE FUNCTION public."USP_Chat_AddUserToConversation"(
  conversationid integer,
  usuarioid varchar
)
RETURNS boolean
LANGUAGE plpgsql
AS $$
BEGIN
  BEGIN
    INSERT INTO public."Participantes" (
      "nParticipantesConversacionId",
      "cParticipantesUsuarioId"
    ) VALUES (
      conversationid,
      usuarioid
    );
  EXCEPTION WHEN unique_violation THEN
    -- Ya existe, considerar éxito
    RETURN true;
  END;
  RETURN true;
END;
$$;

GRANT EXECUTE ON FUNCTION public."USP_Chat_AddUserToConversation"(integer, varchar) TO authenticated;
GRANT EXECUTE ON FUNCTION public."USP_Chat_AddUserToConversation"(integer, varchar) TO anon;
