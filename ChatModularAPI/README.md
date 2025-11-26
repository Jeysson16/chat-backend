# Chat Modular API - Backend .NET 8

Sistema de chat modular multiaplicación desarrollado en .NET 8 Web API con SignalR para comunicación en tiempo real.

## Características Principales

- **Autenticación Dual**: AccessToken por aplicación + JWT por usuario
- **Comunicación en Tiempo Real**: SignalR Hub para mensajería instantánea
- **Multiaplicación**: Soporte para múltiples aplicaciones con tokens independientes
- **Webhooks Firmados**: Sistema de notificaciones seguras con validación de firma
- **Integración Supabase**: Base de datos y autenticación
- **Logging Avanzado**: Serilog con múltiples destinos
- **Arquitectura por Capas**: Controllers, Services, Repositories

## Estructura del Proyecto

```
ChatModularAPI/
├── Controllers/          # Controladores API
│   ├── AuthController.cs
│   ├── ChatController.cs
│   └── WebhookController.cs
├── Services/            # Lógica de negocio
│   ├── IChatService.cs
│   ├── ChatService.cs
│   ├── ITokenService.cs
│   └── TokenService.cs
├── Repositories/        # Acceso a datos
│   ├── IChatRepository.cs
│   ├── ChatRepository.cs
│   ├── IUsuarioRepository.cs
│   ├── UsuarioRepository.cs
│   ├── IAppRegistroRepository.cs
│   ├── AppRegistroRepository.cs
│   ├── ITokenRepository.cs
│   └── TokenRepository.cs
├── Models/             # Modelos de datos
│   ├── DTOs/          # Data Transfer Objects
│   ├── AppRegistro.cs
│   ├── ChatUsuario.cs
│   ├── ChatConversacion.cs
│   ├── ChatMensaje.cs
│   ├── ChatUsuarioConversacion.cs
│   └── TokenRegistro.cs
├── Configs/           # Configuraciones
│   ├── SupabaseConfig.cs
│   └── JwtConfig.cs
├── Middleware/        # Middleware personalizado
│   └── DualAuthMiddleware.cs
├── Hubs/             # SignalR Hubs
│   └── ChatHub.cs
├── Webhooks/         # Sistema de webhooks
│   ├── IWebhookService.cs
│   └── WebhookService.cs
└── logs/             # Archivos de log
```

## Configuración

### 1. Configurar appsettings.json

```json
{
  "Supabase": {
    "Url": "YOUR_SUPABASE_URL",
    "Key": "YOUR_SUPABASE_ANON_KEY",
    "ServiceRoleKey": "YOUR_SUPABASE_SERVICE_ROLE_KEY"
  },
  "Jwt": {
    "SecretKey": "your-super-secret-jwt-key-that-should-be-at-least-32-characters-long",
    "Issuer": "ChatModularAPI",
    "Audience": "ChatModularAPI",
    "ExpirationMinutes": 60
  },
  "AppSecurity": {
    "WebhookSecret": "your-webhook-secret-key",
    "TokenExpirationDays": 30
  }
}
```

### 2. Instalar dependencias

```bash
dotnet restore
```

### 3. Ejecutar la aplicación

```bash
dotnet run
```

## Base de Datos (Supabase)

### Tablas Principales

1. **app_registro**: Registro de aplicaciones
2. **chat_usuario**: Usuarios del sistema
3. **chat_conversacion**: Conversaciones
4. **chat_mensaje**: Mensajes
5. **chat_usuario_conversacion**: Relación usuarios-conversaciones
6. **token_registro**: Tokens JWT activos

### Convención de Nomenclatura

- `c`: Campos de texto (varchar)
- `n`: Campos numéricos (int, decimal)
- `d`: Campos de fecha (datetime)
- `b`: Campos booleanos (bit)

## Autenticación

### Flujo de Autenticación Dual

1. **AccessToken**: Validación por aplicación
2. **JWT Token**: Autenticación de usuario

### Headers Requeridos

```
X-App-Code: CODIGO_APLICACION
X-Access-Token: TOKEN_APLICACION
Authorization: Bearer JWT_TOKEN_USUARIO
```

## Endpoints API

### Auth Controller
- `POST /api/auth/login` - Login de usuario
- `POST /api/auth/logout` - Logout de usuario
- `GET /api/auth/validate` - Validar token

### Chat Controller
- `GET /api/chat/conversations` - Obtener conversaciones del usuario
- `GET /api/chat/conversations/{id}/messages` - Obtener mensajes
- `POST /api/chat/messages` - Enviar mensaje
- `POST /api/chat/conversations` - Crear conversación
- `POST /api/chat/conversations/{id}/join` - Unirse a conversación
- `DELETE /api/chat/conversations/{id}/leave` - Salir de conversación
- `GET /api/chat/conversations/{id}/participants` - Obtener participantes
- `PUT /api/chat/conversations/{id}/read` - Marcar como leído

### Webhook Controller
- `POST /api/webhook/receive/{appCode}` - Recibir webhook
- `POST /api/webhook/test` - Probar webhook

## SignalR Hub

### Endpoint
```
/chathub
```

### Métodos Disponibles
- `JoinConversation(conversationId)`
- `LeaveConversation(conversationId)`
- `SendMessage(conversationId, message)`
- `MarkMessagesAsRead(conversationId)`
- `GetOnlineUsers()`

### Eventos del Cliente
- `ReceiveMessage` - Nuevo mensaje recibido
- `UserJoined` - Usuario se unió a conversación
- `UserLeft` - Usuario salió de conversación
- `MessagesRead` - Mensajes marcados como leídos
- `OnlineUsersUpdate` - Actualización de usuarios en línea

## Logging

El sistema utiliza Serilog con múltiples destinos:
- **Console**: Para desarrollo
- **File**: Archivos rotativos diarios en `/logs`

## Despliegue

### Desarrollo
```bash
dotnet run --environment Development
```

### Producción
```bash
dotnet publish -c Release
dotnet ChatModularAPI.dll
```

## Configuración de CORS

Por defecto permite todas las conexiones. Para producción, configurar específicamente:

```csharp
options.AddPolicy("Production", policy =>
{
    policy.WithOrigins("https://yourdomain.com")
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials();
});
```

## Requisitos

- .NET 8.0 SDK
- Supabase Account
- Visual Studio 2022 o VS Code

## Contribución

1. Fork el proyecto
2. Crear rama feature (`git checkout -b feature/AmazingFeature`)
3. Commit cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abrir Pull Request

## Licencia

Este proyecto está bajo la Licencia MIT - ver el archivo [LICENSE](LICENSE) para detalles.