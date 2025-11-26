-- Crear registro de SICOM usando el stored procedure
SELECT * FROM sp_aplicaciones_crear(
    'SICOM Chat Modular',
    'SICOM_CHAT_2024',
    'Sistema Integral de Comunicación - Chat Modular para empresas'
);