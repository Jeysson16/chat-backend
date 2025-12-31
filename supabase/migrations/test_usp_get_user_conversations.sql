DO $$
DECLARE
  v_user TEXT := '1000000001';
  v_app TEXT := 'SICOM_CHAT_2024';
  v_cnt INTEGER;
  v_part_cnt INTEGER;
BEGIN
  SELECT COUNT(*) INTO v_part_cnt FROM public."Participantes" p WHERE p."cParticipantesUsuarioId" = v_user;
  IF v_part_cnt = 0 THEN
    RAISE EXCEPTION 'No existen Participantes para usuario %', v_user;
  END IF;

  SELECT COUNT(*) INTO v_cnt FROM public."USP_Chat_GetUserConversations"(v_app, v_user, 1, 50, 'DEFAULT');
  IF v_cnt = 0 THEN
    RAISE EXCEPTION 'USP_Chat_GetUserConversations no retornó filas para usuario %', v_user;
  END IF;

  PERFORM 1 FROM (
    SELECT b.nconversacioneschatid
    FROM public."USP_Chat_GetUserConversations"(v_app, v_user, 1, 50, 'DEFAULT') b
    JOIN public."Participantes" p ON p."nParticipantesConversacionId" = b.nconversacioneschatid AND p."cParticipantesUsuarioId" = v_user
  ) t;
  IF NOT FOUND THEN
    RAISE EXCEPTION 'Las filas retornadas no contienen al usuario % como participante', v_user;
  END IF;
END $$;
