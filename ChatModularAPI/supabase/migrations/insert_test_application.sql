-- Insert test application data
INSERT INTO "Aplicaciones" (
    "cAplicacionesNombre",
    "cAplicacionesDescripcion", 
    "cAplicacionesCodigo",
    "bAplicacionesEsActiva"
) VALUES (
    'Test Application',
    'Application for testing purposes',
    'TEST_APP_001',
    true
) ON CONFLICT ("cAplicacionesCodigo") DO NOTHING;