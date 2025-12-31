DO $$
DECLARE
  v_user TEXT := '1000000001';
  v_cnt_fn INTEGER;
  v_cnt_sp INTEGER;
BEGIN
  RAISE NOTICE '== Iniciando pruebas de consultas de conversaciones ==';

  -- Prueba 1: función nueva basada en Participantes
  SELECT COUNT(*) INTO v_cnt_fn FROM public.fn_listar_conversaciones_por_usuario(v_user);
  RAISE NOTICE 'fn_listar_conversaciones_por_usuario(%): % filas', v_user, v_cnt_fn;
  IF v_cnt_fn = 0 THEN
    RAISE EXCEPTION 'La función fn_listar_conversaciones_por_usuario no devolvió conversaciones para el usuario %', v_user;
  END IF;

  -- Prueba 2: SP/Función legacy si existe (no falla si no existe)
  BEGIN
    -- Intentar ejecutar SP legacy si existe con alguna firma compatible
    v_cnt_sp := 0;
    PERFORM 1 FROM pg_proc p JOIN pg_namespace n ON n.oid = p.pronamespace
      WHERE n.nspname = 'public' AND lower(p.proname) = lower('USP_Chat_GetUserConversations');
    IF FOUND THEN
      -- Probar llamada con firma (varchar, varchar, int, int)
      SELECT COUNT(*) INTO v_cnt_sp FROM (
        SELECT * FROM public."USP_Chat_GetUserConversations"('SICOM_CHAT_2024', v_user, 1, 50)
      ) t;
      RAISE NOTICE 'USP_Chat_GetUserConversations(%): % filas', v_user, v_cnt_sp;
      IF v_cnt_sp = 0 THEN
        RAISE EXCEPTION 'USP_Chat_GetUserConversations no devolvió conversaciones para el usuario %', v_user;
      END IF;
    ELSE
      RAISE NOTICE 'USP_Chat_GetUserConversations no existe en este entorno, se omite la prueba legacy';
    END IF;
  EXCEPTION WHEN undefined_function THEN
    RAISE NOTICE 'USP_Chat_GetUserConversations con la firma esperada no existe; se omite la prueba legacy';
  END;

  -- Coherencia: verificar que alguna conversación tenga al usuario como participante
  PERFORM 1 FROM public."Participantes" p
    WHERE p."cParticipantesUsuarioId" = v_user;
  IF NOT FOUND THEN
    RAISE EXCEPTION 'No hay filas en Participantes para el usuario %; los listados nunca devolverán resultados', v_user;
  END IF;

  RAISE NOTICE '== Pruebas de consultas de conversaciones completadas con éxito ==';
END $$;
