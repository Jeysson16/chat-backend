-- Lista participantes de una conversación con datos de UsuariosChat

CREATE OR REPLACE FUNCTION public."USP_ChatParticipants_SelectByConversation"(
  ConversationId integer
)
RETURNS TABLE (
  cUsuariosChatId varchar,
  cUsuariosChatNombre varchar,
  cUsuariosChatEmail varchar,
  cUsuariosChatAvatar varchar,
  cUsuariosChatRol varchar,
  bUsuariosChatEstaActivo boolean,
  bUsuariosChatEstaEnLinea boolean,
  dUsuariosChatUltimaConexion timestamptz
)
LANGUAGE sql
AS $$
  SELECT 
    u."cUsuariosChatId",
    u."cUsuariosChatNombre",
    u."cUsuariosChatEmail",
    u."cUsuariosChatAvatar",
    COALESCE(u."cUsuariosChatRol", 'USER') AS cUsuariosChatRol,
    TRUE AS bUsuariosChatEstaActivo,
    COALESCE(u."bUsuariosChatEstaEnLinea", FALSE) AS bUsuariosChatEstaEnLinea,
    u."dUsuariosChatUltimaVez" AS dUsuariosChatUltimaConexion
  FROM public."ParticipantesChat" p
  JOIN public."UsuariosChat" u ON u."cUsuariosChatId" = p."cParticipantesChatUsuarioId"
  WHERE p."nParticipantesChatConversacionId" = ConversationId
    AND p."bParticipantesChatEstaActivo" = TRUE;
$$;

GRANT EXECUTE ON FUNCTION public."USP_ChatParticipants_SelectByConversation"(integer) TO authenticated;
GRANT EXECUTE ON FUNCTION public."USP_ChatParticipants_SelectByConversation"(integer) TO anon;
