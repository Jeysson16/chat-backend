-- =====================================================
-- MIGRACIÓN DE RESTAURACIÓN DE NOTACIÓN HÚNGARA
-- Fecha: 2024-12-26
-- Propósito: Restaurar el estándar correcto de nomenclatura
--           Tablas: PascalCase
--           Columnas: Prefijo húngaro + nombre tabla + campo
-- =====================================================

-- =====================================================
-- PASO 1: RESTAURAR PREFIJOS HÚNGAROS EN TABLAS EXISTENTES
-- =====================================================

-- 1.1 Restaurar prefijos en tabla Usuarios
ALTER TABLE "Usuarios" RENAME COLUMN "id" TO "nUsuariosId";
ALTER TABLE "Usuarios" RENAME COLUMN "nombre" TO "cUsuariosNombre";
ALTER TABLE "Usuarios" RENAME COLUMN "email" TO "cUsuariosEmail";
ALTER TABLE "Usuarios" RENAME COLUMN "avatar" TO "cUsuariosAvatar";
ALTER TABLE "Usuarios" RENAME COLUMN "estaEnLinea" TO "bUsuariosEstaEnLinea";
ALTER TABLE "Usuarios" RENAME COLUMN "ultimaConexion" TO "dUsuariosUltimaConexion";
ALTER TABLE "Usuarios" RENAME COLUMN "fechaCreacion" TO "dUsuariosFechaCreacion";

-- 1.2 Restaurar prefijos en tabla Conversaciones
ALTER TABLE "Conversaciones" RENAME COLUMN "id" TO "nConversacionesId";
ALTER TABLE "Conversaciones" RENAME COLUMN "nombre" TO "cConversacionesNombre";
ALTER TABLE "Conversaciones" RENAME COLUMN "tipo" TO "cConversacionesTipo";
ALTER TABLE "Conversaciones" RENAME COLUMN "fechaCreacion" TO "dConversacionesFechaCreacion";
ALTER TABLE "Conversaciones" RENAME COLUMN "fechaActualizacion" TO "dConversacionesFechaActualizacion";
ALTER TABLE "Conversaciones" RENAME COLUMN "esActiva" TO "bConversacionesEsActiva";

-- 1.3 Restaurar prefijos en tabla Mensajes
ALTER TABLE "Mensajes" RENAME COLUMN "id" TO "nMensajesId";
ALTER TABLE "Mensajes" RENAME COLUMN "conversacionId" TO "nMensajesConversacionId";
ALTER TABLE "Mensajes" RENAME COLUMN "remitenteId" TO "cMensajesRemitenteId";
ALTER TABLE "Mensajes" RENAME COLUMN "texto" TO "cMensajesTexto";
ALTER TABLE "Mensajes" RENAME COLUMN "tipo" TO "cMensajesTipo";
ALTER TABLE "Mensajes" RENAME COLUMN "fechaCreacion" TO "dMensajesFechaCreacion";
ALTER TABLE "Mensajes" RENAME COLUMN "esLeido" TO "bMensajesEsLeido";

-- 1.4 Restaurar prefijos en tabla Participantes
ALTER TABLE "Participantes" RENAME COLUMN "id" TO "nParticipantesId";
ALTER TABLE "Participantes" RENAME COLUMN "conversacionId" TO "nParticipantesConversacionId";
ALTER TABLE "Participantes" RENAME COLUMN "usuarioId" TO "cParticipantesUsuarioId";
ALTER TABLE "Participantes" RENAME COLUMN "rol" TO "cParticipantesRol";
ALTER TABLE "Participantes" RENAME COLUMN "fechaUnion" TO "dParticipantesFechaUnion";
ALTER TABLE "Participantes" RENAME COLUMN "esActivo" TO "bParticipantesEsActivo";

-- 1.5 Restaurar prefijos en tabla Aplicaciones
ALTER TABLE "Aplicaciones" RENAME COLUMN "id" TO "nAplicacionesId";
ALTER TABLE "Aplicaciones" RENAME COLUMN "nombre" TO "cAplicacionesNombre";
ALTER TABLE "Aplicaciones" RENAME COLUMN "descripcion" TO "cAplicacionesDescripcion";
ALTER TABLE "Aplicaciones" RENAME COLUMN "codigo" TO "cAplicacionesCodigo";
ALTER TABLE "Aplicaciones" RENAME COLUMN "fechaCreacion" TO "dAplicacionesFechaCreacion";
ALTER TABLE "Aplicaciones" RENAME COLUMN "esActiva" TO "bAplicacionesEsActiva";

-- 1.6 Restaurar prefijos en tabla Empresas
ALTER TABLE "Empresas" RENAME COLUMN "id" TO "nEmpresasId";
ALTER TABLE "Empresas" RENAME COLUMN "nombre" TO "cEmpresasNombre";
ALTER TABLE "Empresas" RENAME COLUMN "codigo" TO "cEmpresasCodigo";
ALTER TABLE "Empresas" RENAME COLUMN "aplicacionId" TO "nEmpresasAplicacionId";
ALTER TABLE "Empresas" RENAME COLUMN "fechaCreacion" TO "dEmpresasFechaCreacion";
ALTER TABLE "Empresas" RENAME COLUMN "esActiva" TO "bEmpresasEsActiva";

-- 1.7 Restaurar prefijos en tabla ConfiguracionEmpresa
ALTER TABLE "ConfiguracionEmpresa" RENAME COLUMN "id" TO "nConfiguracionEmpresaId";
ALTER TABLE "ConfiguracionEmpresa" RENAME COLUMN "empresaId" TO "nConfiguracionEmpresaEmpresaId";
ALTER TABLE "ConfiguracionEmpresa" RENAME COLUMN "clave" TO "cConfiguracionEmpresaClave";
ALTER TABLE "ConfiguracionEmpresa" RENAME COLUMN "valor" TO "cConfiguracionEmpresaValor";
ALTER TABLE "ConfiguracionEmpresa" RENAME COLUMN "fechaCreacion" TO "dConfiguracionEmpresaFechaCreacion";
ALTER TABLE "ConfiguracionEmpresa" RENAME COLUMN "fechaActualizacion" TO "dConfiguracionEmpresaFechaActualizacion";

-- 1.8 Restaurar prefijos en tabla Contactos
ALTER TABLE "Contactos" RENAME COLUMN "id" TO "nContactosId";
ALTER TABLE "Contactos" RENAME COLUMN "empresaId" TO "nContactosEmpresaId";
ALTER TABLE "Contactos" RENAME COLUMN "aplicacionId" TO "nContactosAplicacionId";
ALTER TABLE "Contactos" RENAME COLUMN "usuarioSolicitanteId" TO "cContactosUsuarioSolicitanteId";
ALTER TABLE "Contactos" RENAME COLUMN "usuarioContactoId" TO "cContactosUsuarioContactoId";
ALTER TABLE "Contactos" RENAME COLUMN "estado" TO "cContactosEstado";
ALTER TABLE "Contactos" RENAME COLUMN "nombreContacto" TO "cContactosNombreContacto";
ALTER TABLE "Contactos" RENAME COLUMN "emailContacto" TO "cContactosEmailContacto";
ALTER TABLE "Contactos" RENAME COLUMN "telefonoContacto" TO "cContactosTelefonoContacto";
ALTER TABLE "Contactos" RENAME COLUMN "notasContacto" TO "cContactosNotasContacto";
ALTER TABLE "Contactos" RENAME COLUMN "esActivo" TO "bContactosEsActivo";
ALTER TABLE "Contactos" RENAME COLUMN "fechaSolicitud" TO "dContactosFechaSolicitud";
ALTER TABLE "Contactos" RENAME COLUMN "fechaAceptacion" TO "dContactosFechaAceptacion";
ALTER TABLE "Contactos" RENAME COLUMN "fechaCreacion" TO "dContactosFechaCreacion";
ALTER TABLE "Contactos" RENAME COLUMN "fechaModificacion" TO "dContactosFechaModificacion";

-- 1.9 Restaurar prefijos en tabla AppRegistros
ALTER TABLE "AppRegistros" RENAME COLUMN "id" TO "nAppRegistrosId";
ALTER TABLE "AppRegistros" RENAME COLUMN "codigoApp" TO "cAppRegistrosCodigoApp";
ALTER TABLE "AppRegistros" RENAME COLUMN "nombreApp" TO "cAppRegistrosNombreApp";
ALTER TABLE "AppRegistros" RENAME COLUMN "tokenAcceso" TO "cAppRegistrosTokenAcceso";
ALTER TABLE "AppRegistros" RENAME COLUMN "secretoApp" TO "cAppRegistrosSecretoApp";
ALTER TABLE "AppRegistros" RENAME COLUMN "esActivo" TO "bAppRegistrosEsActivo";
ALTER TABLE "AppRegistros" RENAME COLUMN "fechaCreacion" TO "dAppRegistrosFechaCreacion";
ALTER TABLE "AppRegistros" RENAME COLUMN "fechaExpiracion" TO "dAppRegistrosFechaExpiracion";
ALTER TABLE "AppRegistros" RENAME COLUMN "configuracionesAdicionales" TO "jAppRegistrosConfiguracionesAdicionales";

-- 1.10 Restaurar prefijos en tabla TokenRegistros
ALTER TABLE "TokenRegistros" RENAME COLUMN "id" TO "nTokenRegistrosId";
ALTER TABLE "TokenRegistros" RENAME COLUMN "codigoApp" TO "cTokenRegistrosCodigoApp";
ALTER TABLE "TokenRegistros" RENAME COLUMN "perJurCodigo" TO "cTokenRegistrosPerJurCodigo";
ALTER TABLE "TokenRegistros" RENAME COLUMN "perCodigo" TO "cTokenRegistrosPerCodigo";
ALTER TABLE "TokenRegistros" RENAME COLUMN "jwtToken" TO "cTokenRegistrosJwtToken";
ALTER TABLE "TokenRegistros" RENAME COLUMN "refreshToken" TO "cTokenRegistrosRefreshToken";
ALTER TABLE "TokenRegistros" RENAME COLUMN "usuarioId" TO "cTokenRegistrosUsuarioId";
ALTER TABLE "TokenRegistros" RENAME COLUMN "fechaExpiracion" TO "dTokenRegistrosFechaExpiracion";
ALTER TABLE "TokenRegistros" RENAME COLUMN "esActivo" TO "bTokenRegistrosEsActivo";
ALTER TABLE "TokenRegistros" RENAME COLUMN "fechaCreacion" TO "dTokenRegistrosFechaCreacion";

-- 1.11 Restaurar prefijos en tabla WebhookRegistros
ALTER TABLE "WebhookRegistros" RENAME COLUMN "id" TO "nWebhookRegistrosId";
ALTER TABLE "WebhookRegistros" RENAME COLUMN "aplicacionId" TO "nWebhookRegistrosAplicacionId";
ALTER TABLE "WebhookRegistros" RENAME COLUMN "empresaId" TO "nWebhookRegistrosEmpresaId";
ALTER TABLE "WebhookRegistros" RENAME COLUMN "url" TO "cWebhookRegistrosUrl";
ALTER TABLE "WebhookRegistros" RENAME COLUMN "evento" TO "cWebhookRegistrosEvento";
ALTER TABLE "WebhookRegistros" RENAME COLUMN "secreto" TO "cWebhookRegistrosSecreto";
ALTER TABLE "WebhookRegistros" RENAME COLUMN "esActivo" TO "bWebhookRegistrosEsActivo";
ALTER TABLE "WebhookRegistros" RENAME COLUMN "fechaCreacion" TO "dWebhookRegistrosFechaCreacion";
ALTER TABLE "WebhookRegistros" RENAME COLUMN "fechaActualizacion" TO "dWebhookRegistrosFechaActualizacion";

-- =====================================================
-- PASO 2: RECREAR FOREIGN KEYS CON NOMBRES CORRECTOS
-- =====================================================

-- Eliminar foreign keys existentes
ALTER TABLE "Mensajes" DROP CONSTRAINT IF EXISTS "fk_mensajes_conversacion";
ALTER TABLE "Mensajes" DROP CONSTRAINT IF EXISTS "fk_mensajes_remitente";
ALTER TABLE "Mensajes" DROP CONSTRAINT IF EXISTS "mensajeschat_nmensajeschatconversacionid_fkey";
ALTER TABLE "Mensajes" DROP CONSTRAINT IF EXISTS "mensajeschat_cmensajeschatremitenteid_fkey";

ALTER TABLE "Participantes" DROP CONSTRAINT IF EXISTS "fk_participantes_conversacion";
ALTER TABLE "Participantes" DROP CONSTRAINT IF EXISTS "fk_participantes_usuario";
ALTER TABLE "Participantes" DROP CONSTRAINT IF EXISTS "participanteschat_nparticipanteschatconversacionid_fkey";
ALTER TABLE "Participantes" DROP CONSTRAINT IF EXISTS "participanteschat_cparticipanteschatusuarioid_fkey";

ALTER TABLE "Empresas" DROP CONSTRAINT IF EXISTS "fk_empresas_aplicacion";
ALTER TABLE "Empresas" DROP CONSTRAINT IF EXISTS "Companies_nCompaniesApplicationId_fkey";

ALTER TABLE "ConfiguracionEmpresa" DROP CONSTRAINT IF EXISTS "fk_configuracion_empresa";
ALTER TABLE "ConfiguracionEmpresa" DROP CONSTRAINT IF EXISTS "CompanyConfiguration_nCompanyConfigurationCompanyId_fkey";

ALTER TABLE "Contactos" DROP CONSTRAINT IF EXISTS "fk_contactos_empresa";
ALTER TABLE "Contactos" DROP CONSTRAINT IF EXISTS "fk_contactos_aplicacion";

ALTER TABLE "TokenRegistros" DROP CONSTRAINT IF EXISTS "fk_token_usuario";

ALTER TABLE "WebhookRegistros" DROP CONSTRAINT IF EXISTS "fk_webhook_aplicacion";
ALTER TABLE "WebhookRegistros" DROP CONSTRAINT IF EXISTS "fk_webhook_empresa";

-- Recrear foreign keys con nombres correctos
ALTER TABLE "Mensajes" 
ADD CONSTRAINT "fk_mensajes_conversacion" 
FOREIGN KEY ("nMensajesConversacionId") REFERENCES "Conversaciones"("nConversacionesId");

ALTER TABLE "Mensajes" 
ADD CONSTRAINT "fk_mensajes_remitente" 
FOREIGN KEY ("cMensajesRemitenteId") REFERENCES "Usuarios"("nUsuariosId");

ALTER TABLE "Participantes" 
ADD CONSTRAINT "fk_participantes_conversacion" 
FOREIGN KEY ("nParticipantesConversacionId") REFERENCES "Conversaciones"("nConversacionesId");

ALTER TABLE "Participantes" 
ADD CONSTRAINT "fk_participantes_usuario" 
FOREIGN KEY ("cParticipantesUsuarioId") REFERENCES "Usuarios"("nUsuariosId");

ALTER TABLE "Empresas" 
ADD CONSTRAINT "fk_empresas_aplicacion" 
FOREIGN KEY ("nEmpresasAplicacionId") REFERENCES "Aplicaciones"("nAplicacionesId");

ALTER TABLE "ConfiguracionEmpresa" 
ADD CONSTRAINT "fk_configuracion_empresa" 
FOREIGN KEY ("nConfiguracionEmpresaEmpresaId") REFERENCES "Empresas"("nEmpresasId");

ALTER TABLE "Contactos" 
ADD CONSTRAINT "fk_contactos_empresa" 
FOREIGN KEY ("nContactosEmpresaId") REFERENCES "Empresas"("nEmpresasId");

ALTER TABLE "Contactos" 
ADD CONSTRAINT "fk_contactos_aplicacion" 
FOREIGN KEY ("nContactosAplicacionId") REFERENCES "Aplicaciones"("nAplicacionesId");

ALTER TABLE "TokenRegistros" 
ADD CONSTRAINT "fk_token_usuario" 
FOREIGN KEY ("cTokenRegistrosUsuarioId") REFERENCES "Usuarios"("nUsuariosId");

ALTER TABLE "WebhookRegistros" 
ADD CONSTRAINT "fk_webhook_aplicacion" 
FOREIGN KEY ("nWebhookRegistrosAplicacionId") REFERENCES "Aplicaciones"("nAplicacionesId");

ALTER TABLE "WebhookRegistros" 
ADD CONSTRAINT "fk_webhook_empresa" 
FOREIGN KEY ("nWebhookRegistrosEmpresaId") REFERENCES "Empresas"("nEmpresasId");

-- =====================================================
-- PASO 3: ACTUALIZAR COMENTARIOS DE TABLAS
-- =====================================================

COMMENT ON TABLE "Usuarios" IS 'Tabla de usuarios del sistema - Estándar: PascalCase/Húngaro';
COMMENT ON TABLE "Conversaciones" IS 'Tabla de conversaciones del chat - Estándar: PascalCase/Húngaro';
COMMENT ON TABLE "Mensajes" IS 'Tabla de mensajes del chat - Estándar: PascalCase/Húngaro';
COMMENT ON TABLE "Participantes" IS 'Tabla de participantes en conversaciones - Estándar: PascalCase/Húngaro';
COMMENT ON TABLE "Aplicaciones" IS 'Tabla de aplicaciones del sistema - Estándar: PascalCase/Húngaro';
COMMENT ON TABLE "Empresas" IS 'Tabla de empresas asociadas a aplicaciones - Estándar: PascalCase/Húngaro';
COMMENT ON TABLE "ConfiguracionEmpresa" IS 'Tabla de configuraciones específicas por empresa - Estándar: PascalCase/Húngaro';
COMMENT ON TABLE "Contactos" IS 'Tabla de contactos entre usuarios - Estándar: PascalCase/Húngaro';
COMMENT ON TABLE "AppRegistros" IS 'Tabla de registro de aplicaciones - Estándar: PascalCase/Húngaro';
COMMENT ON TABLE "TokenRegistros" IS 'Tabla de registro de tokens JWT - Estándar: PascalCase/Húngaro';
COMMENT ON TABLE "WebhookRegistros" IS 'Tabla de registro de webhooks - Estándar: PascalCase/Húngaro';