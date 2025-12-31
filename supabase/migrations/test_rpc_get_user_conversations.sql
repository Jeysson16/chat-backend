-- Prueba de la función USP_Chat_GetUserConversations
-- Ajusta valores de ejemplo según tus datos

SELECT * FROM public."USP_Chat_GetUserConversations"(
  cappcodigo := 'SICOM_CHAT_2024',
  cusuarioid := '1000000001',
  npage := 1,
  npagesize := 20,
  perjurcodigo := 'DEFAULT'
);
