CREATE OR REPLACE FUNCTION public.fn_listar_conversaciones_por_usuario(cUsuarioId text)
RETURNS TABLE (
  nconversacioneschatid integer,
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
  SELECT
    c."nConversacionesId" AS nconversacioneschatid,
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
          AND p2."cParticipantesUsuarioId" <> cUsuarioId
        LIMIT 1
      )
      ELSE c."cConversacionesNombre"
    END AS cdisplayname
  FROM public."Conversaciones" c
  JOIN public."Participantes" p ON p."nParticipantesConversacionId" = c."nConversacionesId"
  LEFT JOIN LATERAL (
    SELECT m.*
    FROM public."Mensajes" m
    WHERE m."nMensajesConversacionId" = c."nConversacionesId"
    ORDER BY m."dMensajesFechaCreacion" DESC
    LIMIT 1
  ) lm ON TRUE
  LEFT JOIN public."Usuarios" urem ON urem."nUsuariosId"::text = lm."cMensajesRemitenteId"
  WHERE p."cParticipantesUsuarioId" = cUsuarioId
  ORDER BY COALESCE(lm."dMensajesFechaCreacion", c."dConversacionesFechaActualizacion") DESC;
$$;

COMMENT ON FUNCTION public.fn_listar_conversaciones_por_usuario(text) IS 'Lista conversaciones donde el usuario es participante, con último mensaje y nombre de visualización.';
