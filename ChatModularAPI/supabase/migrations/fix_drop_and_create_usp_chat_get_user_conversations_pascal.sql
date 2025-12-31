-- Ajuste: recrear función usando tablas PascalCase existentes (Participantes/Conversaciones/Usuarios/Mensajes)

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
      c."nConversacionesId" AS nconversacioneschatid,
      c."cConversacionesAppCodigo" AS cconversacioneschatappcodigo,
      c."cConversacionesNombre" AS cconversacioneschatname,
      c."cConversacionesTipo" AS cconversacioneschattype,
      c."dConversacionesFechaCreacion" AS dtconversacioneschatcreatedat,
      c."dConversacionesFechaActualizacion" AS dtconversacioneschatupdatedat,
      c."bConversacionesEsActiva" AS bconversacioneschatisactive,
      lm."cMensajesTexto" AS clastmessagetext,
      lm."dMensajesFechaCreacion" AS dtlastmessagetimestamp,
      lm."cMensajesRemitenteId" AS clastmessagesenderid,
      urem."cUsuariosNombre" AS clastmessagesendername,
      0 AS nunreadcount,
      CASE
        WHEN c."cConversacionesTipo" = 'individual' THEN (
          SELECT u2."cUsuariosNombre"
          FROM public."Participantes" p2
          JOIN public."Usuarios" u2 ON u2."nUsuariosId"::text = p2."cParticipantesUsuarioId"
          WHERE p2."nParticipantesConversacionId" = c."nConversacionesId"
            AND p2."cParticipantesUsuarioId" <> cusuarioid
          LIMIT 1
        )
        ELSE c."cConversacionesNombre"
      END AS cdisplayname
    FROM public."Participantes" p
    JOIN public."Conversaciones" c ON c."nConversacionesId" = p."nParticipantesConversacionId"
    JOIN public."Usuarios" u ON u."nUsuariosId"::text = p."cParticipantesUsuarioId"
    LEFT JOIN LATERAL (
      SELECT m.*
      FROM public."Mensajes" m
      WHERE m."nMensajesConversacionId" = c."nConversacionesId"
      ORDER BY m."dMensajesFechaCreacion" DESC
      LIMIT 1
    ) lm ON TRUE
    LEFT JOIN public."Usuarios" urem ON urem."nUsuariosId"::text = lm."cMensajesRemitenteId"
    WHERE p."cParticipantesUsuarioId" = cusuarioid
      AND c."cConversacionesAppCodigo" = cappcodigo
      AND (
        perjurcodigo IS NULL OR perjurcodigo = '' OR perjurcodigo = 'DEFAULT' OR u."cUsuariosPerJurCodigo" = perjurcodigo
      )
  )
  SELECT *
  FROM base
  ORDER BY COALESCE(dtlastmessagetimestamp, dtconversacioneschatupdatedat) DESC
  LIMIT GREATEST(npagesize, 1)
  OFFSET GREATEST(npage - 1, 0) * GREATEST(npagesize, 1);
$$;

COMMENT ON FUNCTION public."USP_Chat_GetUserConversations"(varchar, varchar, integer, integer, varchar)
IS 'Lista conversaciones por usuario desde Participantes con join a Usuarios; filtros por empresa opcional. (Tablas PascalCase)';

GRANT EXECUTE ON FUNCTION public."USP_Chat_GetUserConversations"(varchar, varchar, integer, integer, varchar) TO authenticated;
GRANT EXECUTE ON FUNCTION public."USP_Chat_GetUserConversations"(varchar, varchar, integer, integer, varchar) TO anon;
