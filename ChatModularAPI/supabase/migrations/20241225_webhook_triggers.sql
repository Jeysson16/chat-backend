-- Migración para configurar triggers de webhooks en tiempo real
-- Fecha: 2024-12-25

-- =============================================
-- Función para enviar webhooks automáticamente
-- =============================================
CREATE OR REPLACE FUNCTION notify_webhook_event()
RETURNS TRIGGER AS $$
DECLARE
    webhook_payload JSONB;
    event