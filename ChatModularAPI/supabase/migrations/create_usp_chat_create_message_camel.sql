-- Crea función para insertar mensaje en MensajesChat y actualizar última actividad de la conversación

CREATE OR REPLACE FUNCTION public."USP_Chat_CreateMessage"(
  nMensajesChatConversacionId integer,
  cMensajesChatRemitenteId varchar,
  cMensajesChatTexto text,
  cMensajesChatTipo varchar DEFAULT 'text'
)
RETURNS TABLE (
  nMensajesChatId integer,
  nMensajesChatConversacionId_out integer,
  cMensajesChatRemitenteId_out varchar,
  cMensajesChatTexto_out text,
  cMensajesChatTipo_out varchar,
  dMensajesChatFechaHora_out timestamptz,
  bMensajesChatEstaLeido_out boolean
)
LANGUAGE plpgsql
AS $$
DECLARE
  new_id integer;
BEGIN
  INSERT INTO public."MensajesChat" (
    "nMensajesChatConversacionId",
    "cMensajesChatRemitenteId",
    "cMensajesChatTexto",
    "cMensajesChatTipo",
    "dMensajesChatFechaHora",
    "bMensajesChatEstaLeido"
  ) VALUES (
    nMensajesChatConversacionId,
    cMensajesChatRemitenteId,
    cMensajesChatTexto,
    COALESCE(NULLIF(cMensajesChatTipo, ''), 'text'),
    NOW(),
    FALSE
  ) RETURNING "nMensajesChatId" INTO new_id;

  -- Actualizar última actividad de la conversación
  UPDATE public."ConversacionesChat"
    SET "dConversacionesChatFechaActualizacion" = NOW(),
        "dConversacionesChatUltimaActividad" = NOW()
    WHERE "nConversacionesChatId" = nMensajesChatConversacionId;

  RETURN QUERY
  SELECT 
    m."nMensajesChatId",
    m."nMensajesChatConversacionId",
    m."cMensajesChatRemitenteId",
    m."cMensajesChatTexto",
    m."cMensajesChatTipo",
    m."dMensajesChatFechaHora",
    m."bMensajesChatEstaLeido"
  FROM public."MensajesChat" m
  WHERE m."nMensajesChatId" = new_id;
END;
$$;

GRANT EXECUTE ON FUNCTION public."USP_Chat_CreateMessage"(integer, varchar, text, varchar) TO authenticated;
GRANT EXECUTE ON FUNCTION public."USP_Chat_CreateMessage"(integer, varchar, text, varchar) TO anon;
