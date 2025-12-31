-- Ajuste: eliminar y recrear la función para alinear tipo de retorno

DROP FUNCTION IF EXISTS public."USP_Chat_GetUserConversations"(varchar, varchar, integer, integer, varchar);

CREATE OR REPLACE FUNCTION public."USP_Chat_GetUserConversations"(
  cappcodigo varchar(100),
  cusuarioid varchar(50),
  npage integer DEFAULT 1,
  npagesize integer DEFAULT 50,
  perjurcodigo varchar(50) DEFAULT 'DEFAULT'
)
RETURNS TABLE (
  nconversacioneschatid bigint,
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
      c."nconversacioneschatid" AS nconversacioneschatid,
      c."cconversacioneschatappcodigo" AS cconversacioneschatappcodigo,
      c."cconversacioneschatnombre" AS cconversacioneschatname,
      c."cconversacioneschattipo" AS cconversacioneschattype,
      c."dconversacioneschatfechacreacion" AS dtconversacioneschatcreatedat,
      c."updated_at" AS dtconversacioneschatupdatedat,
      c."bconversacioneschatestaactiva" AS bconversacioneschatisactive,
      lm."cmensajeschattexto" AS clastmessagetext,
      lm."dmensajeschatfechahora" AS dtlastmessagetimestamp,
      lm."cmensajeschatremitenteid" AS clastmessagesenderid,
      urem."cusuarioschatnombre" AS clastmessagesendername,
      0 AS nunreadcount,
      CASE
        WHEN c."cconversacioneschattipo" = 'individual' THEN (
          SELECT u2."cusuarioschatnombre"
          FROM public."participanteschat" p2
          JOIN public."usuarioschat" u2 ON u2."cusuarioschatid" = p2."cparticipanteschatusuarioid"
          WHERE p2."nparticipanteschatconversacionid" = c."nconversacioneschatid"
            AND p2."cparticipanteschatusuarioid" <> cusuarioid
          LIMIT 1
        )
        ELSE c."cconversacioneschatnombre"
      END AS cdisplayname
    FROM public."participanteschat" p
    JOIN public."conversacioneschat" c ON c."nconversacioneschatid" = p."nparticipanteschatconversacionid"
    JOIN public."usuarioschat" u ON u."cusuarioschatid" = p."cparticipanteschatusuarioid"
    LEFT JOIN LATERAL (
      SELECT m.*
      FROM public."mensajeschat" m
      WHERE m."nmensajeschatconversacionid" = c."nconversacioneschatid"
      ORDER BY m."dmensajeschatfechahora" DESC
      LIMIT 1
    ) lm ON TRUE
    LEFT JOIN public."usuarioschat" urem ON urem."cusuarioschatid" = lm."cmensajeschatremitenteid"
    WHERE p."cparticipanteschatusuarioid" = cusuarioid
      AND c."cconversacioneschatappcodigo" = cappcodigo
      AND (
        perjurcodigo IS NULL OR perjurcodigo = '' OR perjurcodigo = 'DEFAULT' OR u."cusuarioschatperjurcodigo" = perjurcodigo
      )
  )
  SELECT *
  FROM base
  ORDER BY COALESCE(dtlastmessagetimestamp, dtconversacioneschatupdatedat) DESC
  LIMIT GREATEST(npagesize, 1)
  OFFSET GREATEST(npage - 1, 0) * GREATEST(npagesize, 1);
$$;

COMMENT ON FUNCTION public."USP_Chat_GetUserConversations"(varchar, varchar, integer, integer, varchar)
IS 'Lista conversaciones de participanteschat por usuario, filtrando por app y perjur; columnas snake_case compatibles con PostgREST.';

GRANT EXECUTE ON FUNCTION public."USP_Chat_GetUserConversations"(varchar, varchar, integer, integer, varchar) TO authenticated;
GRANT EXECUTE ON FUNCTION public."USP_Chat_GetUserConversations"(varchar, varchar, integer, integer, varchar) TO anon;
