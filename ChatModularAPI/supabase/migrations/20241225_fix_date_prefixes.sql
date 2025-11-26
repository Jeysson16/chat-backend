-- Migración para corregir prefijos de fechas de 'dt' a 'd'
-- Fecha: 2024-12-25

-- Corregir tabla usuarioschat
ALTER TABLE usuarioschat 
RENAME COLUMN dtultimavez TO dultimavez;

ALTER TABLE usuarioschat 
RENAME COLUMN dtfechacreacion TO dfechacreacion;

-- Corregir tabla conversacioneschat
ALTER TABLE conversacioneschat 
RENAME COLUMN dtfechacreacion TO dfechacreacion;

ALTER TABLE conversacioneschat 
RENAME COLUMN dtfechaactualizacion TO dfechaactualizacion;

-- Corregir tabla mensajeschat
ALTER TABLE mensajeschat 
RENAME COLUMN dtfechahora TO dfechahora;

-- Corregir tabla participanteschat
ALTER TABLE participanteschat 
RENAME COLUMN dtfechaunion TO dfechaunion;

-- Recrear índices con nombres corregidos
DROP INDEX IF EXISTS idx_MensajesChat_dtFechaHora;
CREATE INDEX idx_MensajesChat_dFechaHora ON mensajeschat(dfechahora);

-- Comentario de finalización
COMMENT ON TABLE usuarioschat IS 'Tabla de usuarios del chat con prefijos corregidos';
COMMENT ON TABLE conversacioneschat IS 'Tabla de conversaciones del chat con prefijos corregidos';
COMMENT ON TABLE mensajeschat IS 'Tabla de mensajes del chat con prefijos corregidos';
COMMENT ON TABLE participanteschat IS 'Tabla de participantes del chat con prefijos corregidos';