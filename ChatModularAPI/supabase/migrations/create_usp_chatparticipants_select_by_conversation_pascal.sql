-- Variante usando tablas antiguas PascalCase (Participantes/Usuarios) con alias a nombres de ChatUsuario

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
    u."nUsuariosId"::text AS "cUsuariosChatId",
    u."cUsuariosNombre" AS "cUsuariosChatNombre",
    u."cUsuariosEmail" AS "cUsuariosChatEmail",
    NULL::varchar AS "cUsuariosChatAvatar",
    COALESCE(NULL::varchar, 'USER') AS "cUsuariosChatRol",
    TRUE AS "bUsuariosChatEstaActivo",
    FALSE AS "bUsuariosChatEstaEnLinea",
    NULL::timestamptz AS "dUsuariosChatUltimaConexion"
  FROM public."Participantes" p
  JOIN public."Usuarios" u ON u."nUsuariosId"::text = p."cParticipantesUsuarioId"
  WHERE p."nParticipantesConversacionId" = ConversationId
    AND TRUE;
$$;

GRANT EXECUTE ON FUNCTION public."USP_ChatParticipants_SelectByConversation"(integer) TO authenticated;
GRANT EXECUTE ON FUNCTION public."USP_ChatParticipants_SelectByConversation"(integer) TO anon;
