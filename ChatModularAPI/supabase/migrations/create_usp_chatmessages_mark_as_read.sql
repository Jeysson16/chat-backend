-- Marca mensajes como leídos para un usuario en una conversación

CREATE OR REPLACE FUNCTION public."USP_ChatMessages_MarkAsRead"(
  ConversationId integer,
  UserId varchar
)
RETURNS boolean
LANGUAGE plpgsql
AS $$
DECLARE
  updated_count integer;
BEGIN
  UPDATE public."MensajesChat"
  SET "bMensajesChatEstaLeido" = TRUE
  WHERE "nMensajesChatConversacionId" = ConversationId
    AND "cMensajesChatRemitenteId" <> UserId;

  GET DIAGNOSTICS updated_count = ROW_COUNT;
  RETURN updated_count >= 0;
END;
$$;

GRANT EXECUTE ON FUNCTION public."USP_ChatMessages_MarkAsRead"(integer, varchar) TO authenticated;
GRANT EXECUTE ON FUNCTION public."USP_ChatMessages_MarkAsRead"(integer, varchar) TO anon;
