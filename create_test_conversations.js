const { createClient } = require('@supabase/supabase-js');
require('dotenv').config();

// Configuración de Supabase
const supabaseUrl = process.env.SUPABASE_URL || 'https://your-project.supabase.co';
const supabaseKey = process.env.SUPABASE_ANON_KEY || 'your-anon-key';
const supabase = createClient(supabaseUrl, supabaseKey);

async function createTestConversationsAndMessages() {
    try {
        console.log('🚀 Iniciando creación de conversaciones y mensajes de prueba...');

        // 1. Obtener usuarios existentes
        const { data: users, error: usersError } = await supabase
            .from('Usuarios')
            .select('nUsuariosId, cUsuariosNombre')
            .limit(5);

        if (usersError) {
            console.error('❌ Error al obtener usuarios:', usersError);
            return;
        }

        if (!users || users.length === 0) {
            console.log('⚠️ No se encontraron usuarios. Creando usuarios de prueba...');
            
            // Crear usuarios de prueba
            const testUsers = [
                {
                    nUsuariosId: 'admin-user-001',
                    cUsuariosNombre: 'Administrador',
                    cUsuariosEmail: 'admin@empresa.com',
                    cUsuariosAvatar: null,
                    bUsuariosEstaEnLinea: true,
                    cUsuariosPerCodigo: 'ADMIN001',
                    cUsuariosPerJurCodigo: 'ADMIN_JUR_001',
                    cUsuariosPassword: 'admin123',
                    bUsuariosActivo: true
                },
                {
                    nUsuariosId: 'user-001',
                    cUsuariosNombre: 'María García',
                    cUsuariosEmail: 'maria.garcia@empresa.com',
                    cUsuariosAvatar: null,
                    bUsuariosEstaEnLinea: false,
                    cUsuariosPerCodigo: 'USER001',
                    cUsuariosPerJurCodigo: 'USER_JUR_001',
                    cUsuariosPassword: 'user123',
                    bUsuariosActivo: true
                },
                {
                    nUsuariosId: 'user-002',
                    cUsuariosNombre: 'Carlos López',
                    cUsuariosEmail: 'carlos.lopez@empresa.com',
                    cUsuariosAvatar: null,
                    bUsuariosEstaEnLinea: true,
                    cUsuariosPerCodigo: 'USER002',
                    cUsuariosPerJurCodigo: 'USER_JUR_002',
                    cUsuariosPassword: 'user123',
                    bUsuariosActivo: true
                },
                {
                    nUsuariosId: 'user-003',
                    cUsuariosNombre: 'Ana Martínez',
                    cUsuariosEmail: 'ana.martinez@empresa.com',
                    cUsuariosAvatar: null,
                    bUsuariosEstaEnLinea: false,
                    cUsuariosPerCodigo: 'USER003',
                    cUsuariosPerJurCodigo: 'USER_JUR_003',
                    cUsuariosPassword: 'user123',
                    bUsuariosActivo: true
                }
            ];

            const { data: createdUsers, error: createUsersError } = await supabase
                .from('Usuarios')
                .insert(testUsers)
                .select();

            if (createUsersError) {
                console.error('❌ Error al crear usuarios de prueba:', createUsersError);
                return;
            }

            console.log('✅ Usuarios de prueba creados:', createdUsers.length);
            users.push(...createdUsers);
        }

        console.log(`📋 Usuarios disponibles: ${users.length}`);

        // 2. Crear conversaciones de prueba
        const testConversations = [
            {
                cConversacionesNombre: 'Equipo de Desarrollo',
                cConversacionesTipo: 'group',
                bConversacionesEsActiva: true
            },
            {
                cConversacionesNombre: 'Soporte Técnico',
                cConversacionesTipo: 'group',
                bConversacionesEsActiva: true
            },
            {
                cConversacionesNombre: 'Chat Privado - Admin',
                cConversacionesTipo: 'individual',
                bConversacionesEsActiva: true
            },
            {
                cConversacionesNombre: 'Reunión Semanal',
                cConversacionesTipo: 'group',
                bConversacionesEsActiva: true
            }
        ];

        const { data: conversations, error: conversationsError } = await supabase
            .from('Conversaciones')
            .insert(testConversations)
            .select();

        if (conversationsError) {
            console.error('❌ Error al crear conversaciones:', conversationsError);
            return;
        }

        console.log('✅ Conversaciones creadas:', conversations.length);

        // 3. Agregar participantes a las conversaciones
        const participants = [];
        
        // Conversación 1: Equipo de Desarrollo (todos los usuarios)
        users.forEach(user => {
            participants.push({
                nParticipantesConversacionId: conversations[0].nConversacionesId,
                cParticipantesUsuarioId: user.nUsuariosId,
                cParticipantesRol: user.nUsuariosId === 'admin-user-001' ? 'admin' : 'member',
                bParticipantesEsActivo: true
            });
        });

        // Conversación 2: Soporte Técnico (admin + 2 usuarios)
        [users[0], users[1], users[2]].forEach((user, index) => {
            if (user) {
                participants.push({
                    nParticipantesConversacionId: conversations[1].nConversacionesId,
                    cParticipantesUsuarioId: user.nUsuariosId,
                    cParticipantesRol: index === 0 ? 'admin' : 'member',
                    bParticipantesEsActivo: true
                });
            }
        });

        // Conversación 3: Chat Privado (admin + 1 usuario)
        [users[0], users[1]].forEach((user, index) => {
            if (user) {
                participants.push({
                    nParticipantesConversacionId: conversations[2].nConversacionesId,
                    cParticipantesUsuarioId: user.nUsuariosId,
                    cParticipantesRol: 'member',
                    bParticipantesEsActivo: true
                });
            }
        });

        // Conversación 4: Reunión Semanal (admin + 3 usuarios)
        users.slice(0, 4).forEach((user, index) => {
            participants.push({
                nParticipantesConversacionId: conversations[3].nConversacionesId,
                cParticipantesUsuarioId: user.nUsuariosId,
                cParticipantesRol: index === 0 ? 'admin' : 'member',
                bParticipantesEsActivo: true
            });
        });

        const { data: createdParticipants, error: participantsError } = await supabase
            .from('Participantes')
            .insert(participants)
            .select();

        if (participantsError) {
            console.error('❌ Error al crear participantes:', participantsError);
            return;
        }

        console.log('✅ Participantes agregados:', createdParticipants.length);

        // 4. Crear mensajes de prueba
        const testMessages = [
            // Mensajes para Equipo de Desarrollo
            {
                nMensajesConversacionId: conversations[0].nConversacionesId,
                cMensajesRemitenteId: users[0].nUsuariosId,
                cMensajesTexto: '¡Hola equipo! Bienvenidos al chat de desarrollo. Aquí coordinaremos nuestros proyectos.',
                cMensajesTipo: 'text',
                bMensajesEsLeido: false
            },
            {
                nMensajesConversacionId: conversations[0].nConversacionesId,
                cMensajesRemitenteId: users[1].nUsuariosId,
                cMensajesTexto: 'Perfecto, gracias por crear el grupo. ¿Cuál es la prioridad para esta semana?',
                cMensajesTipo: 'text',
                bMensajesEsLeido: false
            },
            {
                nMensajesConversacionId: conversations[0].nConversacionesId,
                cMensajesRemitenteId: users[2].nUsuariosId,
                cMensajesTexto: 'Estoy trabajando en la nueva funcionalidad del chat. ¿Alguien puede revisar el código?',
                cMensajesTipo: 'text',
                bMensajesEsLeido: false
            },
            // Mensajes para Soporte Técnico
            {
                nMensajesConversacionId: conversations[1].nConversacionesId,
                cMensajesRemitenteId: users[0].nUsuariosId,
                cMensajesTexto: 'Canal de soporte técnico activo. Reporten aquí cualquier incidencia.',
                cMensajesTipo: 'text',
                bMensajesEsLeido: false
            },
            {
                nMensajesConversacionId: conversations[1].nConversacionesId,
                cMensajesRemitenteId: users[1].nUsuariosId,
                cMensajesTexto: 'Tengo un problema con la conexión a la base de datos. ¿Pueden ayudarme?',
                cMensajesTipo: 'text',
                bMensajesEsLeido: false
            },
            // Mensajes para Chat Privado
            {
                nMensajesConversacionId: conversations[2].nConversacionesId,
                cMensajesRemitenteId: users[0].nUsuariosId,
                cMensajesTexto: 'Hola, ¿cómo va todo? Quería hablar contigo sobre el proyecto.',
                cMensajesTipo: 'text',
                bMensajesEsLeido: false
            },
            {
                nMensajesConversacionId: conversations[2].nConversacionesId,
                cMensajesRemitenteId: users[1].nUsuariosId,
                cMensajesTexto: 'Todo bien, gracias. Cuéntame qué necesitas.',
                cMensajesTipo: 'text',
                bMensajesEsLeido: false
            },
            // Mensajes para Reunión Semanal
            {
                nMensajesConversacionId: conversations[3].nConversacionesId,
                cMensajesRemitenteId: users[0].nUsuariosId,
                cMensajesTexto: 'Recordatorio: Reunión semanal mañana a las 10:00 AM',
                cMensajesTipo: 'text',
                bMensajesEsLeido: false
            },
            {
                nMensajesConversacionId: conversations[3].nConversacionesId,
                cMensajesRemitenteId: users[2].nUsuariosId,
                cMensajesTexto: 'Perfecto, ahí estaré. ¿Hay alguna agenda específica?',
                cMensajesTipo: 'text',
                bMensajesEsLeido: false
            }
        ];

        const { data: createdMessages, error: messagesError } = await supabase
            .from('Mensajes')
            .insert(testMessages)
            .select();

        if (messagesError) {
            console.error('❌ Error al crear mensajes:', messagesError);
            return;
        }

        console.log('✅ Mensajes creados:', createdMessages.length);

        // 5. Resumen final
        console.log('\n🎉 ¡Datos de prueba creados exitosamente!');
        console.log('📊 Resumen:');
        console.log(`   👥 Usuarios: ${users.length}`);
        console.log(`   💬 Conversaciones: ${conversations.length}`);
        console.log(`   👤 Participantes: ${createdParticipants.length}`);
        console.log(`   📝 Mensajes: ${createdMessages.length}`);
        
        console.log('\n📋 Conversaciones creadas:');
        conversations.forEach((conv, index) => {
            console.log(`   ${index + 1}. ${conv.cConversacionesNombre} (${conv.cConversacionesTipo})`);
        });

    } catch (error) {
        console.error('❌ Error general:', error);
    }
}

// Ejecutar el script
if (require.main === module) {
    createTestConversationsAndMessages()
        .then(() => {
            console.log('\n✅ Script completado');
            process.exit(0);
        })
        .catch((error) => {
            console.error('❌ Error en el script:', error);
            process.exit(1);
        });
}

module.exports = { createTestConversationsAndMessages };