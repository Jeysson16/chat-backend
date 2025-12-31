-- Recrear función usando tablas CamelCase (ConversacionesChat/ParticipantesChat/UsuariosChat/MensajesChat)

DROP FUNCTION IF EXISTS public."USP_Chat_GetUserConversations"(varchar, varchar, integer, integer, varchar);

CREATE OR REPLACE FUNCTION public."USP_Chat_GetUserConversations"(
  cappcodigo varchar(100),
  cusuarioid varchar(50),
  npage integer DEFAULT 1,
  npagesize integer DEFAULT 50,
  perjurcodigo varchar(50) DEFAULT 'DEFAULT'
)
RETURNS TABLE (
  nconversacioneschatid integer,
  cconversacioneschatappcodigo text,
  cconversacioneschatname text,
  cconversacioneschattype text,
  dtconversacioneschatcreatedat timestamptz,
  dtconversacioneschatupdatedat timestamptz,
  bconversacioneschatisactive boolean,
  clastmessagetext text,
  dtlastmessagetimestamp timestamptz,
  clastmessagesenderid text,
  clastmessagesendername text,
  nunreadcount integer,
  cdisplayname text
)
LANGUAGE sql
AS $$
  WITH base AS (
    SELECT
      c."nConversacionesChatId" AS nconversacioneschatid,
      cappcodigo AS cconversacioneschatappcodigo,
      c."cConversacionesChatNombre" AS cconversacioneschatname,
      c."cConversacionesChatTipo" AS cconversacioneschattype,
      c."dConversacionesChatFechaCreacion" AS dtconversacioneschatcreatedat,
      c."dConversacionesChatFechaActualizacion" AS dtconversacioneschatupdatedat,
      c."bConversacionesChatEstaActiva" AS bconversacioneschatisactive,
      lm."cMensajesChatTexto" AS clastmessagetext,
      lm."dMensajesChatFechaHora" AS dtlastmessagetimestamp,
      lm."cMensajesChatRemitenteId" AS clastmessagesenderid,
      urem."cUsuariosChatNombre" AS clastmessagesendername,
      0 AS nunreadcount,
      CASE
        WHEN c."cConversacionesChatTipo" = 'individual' THEN (
          SELECT u2."cUsuariosChatNombre"
          FROM public."ParticipantesChat" p2
          JOIN public."UsuariosChat" u2 ON u2."cUsuariosChatId" = p2."cParticipantesChatUsuarioId"
          WHERE p2."nParticipantesChatConversacionId" = c."nConversacionesChatId"
            AND p2."cParticipantesChatUsuarioId" <> cusuarioid
          LIMIT 1
        )
        ELSE c."cConversacionesChatNombre"
      END AS cdisplayname
    FROM public."ParticipantesChat" p
    JOIN public."ConversacionesChat" c ON c."nConversacionesChatId" = p."nParticipantesChatConversacionId"
    JOIN public."UsuariosChat" u ON u."cUsuariosChatId" = p."cParticipantesChatUsuarioId"
    LEFT JOIN LATERAL (
      SELECT m.*
      FROM public."MensajesChat" m
      WHERE m."nMensajesChatConversacionId" = c."nConversacionesChatId"
      ORDER BY m."dMensajesChatFechaHora" DESC
      LIMIT 1
    ) lm ON TRUE
    LEFT JOIN public."UsuariosChat" urem ON urem."cUsuariosChatId" = lm."cMensajesChatRemitenteId"
    WHERE p."cParticipantesChatUsuarioId" = cusuarioid
      AND (
        perjurcodigo IS NULL OR perjurcodigo = '' OR perjurcodigo = 'DEFAULT' /* OR u."cUsuariosChatPerJurCodigo" = perjurcodigo */
      )
  )
  SELECT *
  FROM base
  ORDER BY COALESCE(dtlastmessagetimestamp, dtconversacioneschatupdatedat) DESC
  LIMIT GREATEST(npagesize, 1)
  OFFSET GREATEST(npage - 1, 0) * GREATEST(npagesize, 1);
$$;

COMMENT ON FUNCTION public."USP_Chat_GetUserConversations"(varchar, varchar, integer, integer, varchar)
IS 'Lista conversaciones por usuario desde ParticipantesChat con join a UsuariosChat y último mensaje; argumentos en minúsculas.';

GRANT EXECUTE ON FUNCTION public."USP_Chat_GetUserConversations"(varchar, varchar, integer, integer, varchar) TO authenticated;
GRANT EXECUTE ON FUNCTION public."USP_Chat_GetUserConversations"(varchar, varchar, integer, integer, varchar) TO anon;
