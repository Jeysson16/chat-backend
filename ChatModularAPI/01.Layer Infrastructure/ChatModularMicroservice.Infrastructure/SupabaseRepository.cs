using Supabase;
using Supabase.Postgrest;
using Supabase.Postgrest.Models;
using ChatModularMicroservice.Entities.DTOs;
using Utils = ChatModularMicroservice.Shared.Utils;
using ChatModularMicroservice.Shared.Configs;
using Npgsql;
using System.Data;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace ChatModularMicroservice.Infrastructure.Repositories
{
    /// <summary>
    /// Repositorio base para operaciones con Supabase usando stored procedures
    /// </summary>
    public class SupabaseRepository
    {
        protected readonly Supabase.Client _supabaseClient;
        protected readonly ILogger<SupabaseRepository> _logger;
        protected readonly SupabaseConfig _config;

        public SupabaseRepository(Supabase.Client supabaseClient, ILogger<SupabaseRepository> logger, SupabaseConfig config)
        {
            _supabaseClient = supabaseClient ?? throw new ArgumentNullException(nameof(supabaseClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        /// <summary>
        /// Ejecuta un stored procedure y retorna el resultado
        /// </summary>
        protected async Task<Utils.ItemResponseDT> ExecuteStoredProcedureAsync<T>(
            string procedureName, 
            object? parameters = null,
            string clientName = "ChatModularMicroservice")
        {
            try
            {
                _logger.LogInformation("Ejecutando stored procedure: {ProcedureName}", procedureName);

                // Construir la llamada RPC
                var rpc = _supabaseClient.Rpc(procedureName, parameters);
                var result = await rpc;

                if (result?.Content != null)
                {
                    var jsonContent = result.Content;
                    _logger.LogDebug("Resultado del stored procedure: {Content}", jsonContent);

                    // Deserializar el resultado
                    var data = JsonSerializer.Deserialize<T>(jsonContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return new Utils.ItemResponseDT
                    {
                        clientName = clientName,
                        isSuccess = true,
                        lstItem = data != null ? new List<object> { data! } : new List<object>(),
                        ticket = Guid.NewGuid().ToString()
                    };
                }

                return new Utils.ItemResponseDT
                {
                    clientName = clientName,
                    isSuccess = false,
                    lstError = new List<string> { "No se recibió respuesta del stored procedure" },
                    ticket = Guid.NewGuid().ToString()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ejecutando stored procedure {ProcedureName}: {Error}", procedureName, ex.Message);
                
                return new Utils.ItemResponseDT
                {
                    clientName = clientName,
                    isSuccess = false,
                    lstError = new List<string> { $"Error ejecutando {procedureName}: {ex.Message}" },
                    ticket = Guid.NewGuid().ToString()
                };
            }
        }

        /// <summary>
        /// Ejecuta un stored procedure que retorna múltiples registros
        /// </summary>
        protected async Task<Utils.ItemResponseDT> ExecuteStoredProcedureListAsync<T>(
            string procedureName, 
            object? parameters = null,
            string clientName = "ChatModularMicroservice")
        {
            try
            {
                _logger.LogInformation("Ejecutando stored procedure (lista): {ProcedureName}", procedureName);

                var rpc = _supabaseClient.Rpc(procedureName, parameters);
                var result = await rpc;

                if (result?.Content != null)
                {
                    var jsonContent = result.Content;
                    _logger.LogDebug("Resultado del stored procedure: {Content}", jsonContent);

                    // Deserializar como lista
                    var dataList = JsonSerializer.Deserialize<List<T>>(jsonContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return new Utils.ItemResponseDT
                    {
                        clientName = clientName,
                        isSuccess = true,
                        lstItem = dataList?.Cast<object>().ToList() ?? new List<object>(),
                        ticket = Guid.NewGuid().ToString()
                    };
                }

                return new Utils.ItemResponseDT
                {
                    clientName = clientName,
                    isSuccess = false,
                    lstError = new List<string> { "No se recibió respuesta del stored procedure" },
                    ticket = Guid.NewGuid().ToString()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ejecutando stored procedure {ProcedureName}: {Error}", procedureName, ex.Message);
                
                return new Utils.ItemResponseDT
                {
                    clientName = clientName,
                    isSuccess = false,
                    lstError = new List<string> { $"Error ejecutando {procedureName}: {ex.Message}" },
                    ticket = Guid.NewGuid().ToString()
                };
            }
        }

        /// <summary>
        /// Ejecuta una consulta directa a una tabla (para casos especiales)
        /// </summary>
        protected async Task<Utils.ItemResponseDT> ExecuteQueryAsync<T>(
            string tableName,
            string? filter = null,
            string? select = "*",
            string clientName = "ChatModularMicroservice") where T : BaseModel, new()
        {
            try
            {
                _logger.LogInformation("Ejecutando consulta en tabla: {TableName}", tableName);

                var query = _supabaseClient.From<T>().Select(select);
                
                if (!string.IsNullOrEmpty(filter))
                {
                    // Aplicar filtros si se especifican
                    // Nota: Esto requiere implementación específica según el filtro
                }

                var result = await query.Get();

                return new Utils.ItemResponseDT
                {
                    clientName = clientName,
                    isSuccess = true,
                    lstItem = result?.Models?.Cast<object>().ToList() ?? new List<object>(),
                    ticket = Guid.NewGuid().ToString()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ejecutando consulta en {TableName}: {Error}", tableName, ex.Message);
                
                return new Utils.ItemResponseDT
                {
                    clientName = clientName,
                    isSuccess = false,
                    lstError = new List<string> { $"Error en consulta {tableName}: {ex.Message}" },
                    ticket = Guid.NewGuid().ToString()
                };
            }
        }

        /// <summary>
        /// Ejecuta una consulta SQL directa con parámetros
        /// </summary>
        protected async Task<List<T>> ExecuteQueryAsync<T>(string sqlQuery, object[] parameters) where T : BaseModel, new()
        {
            try
            {
                _logger.LogInformation("Ejecutando consulta SQL directa");
                
                // Para consultas SQL directas, usamos RPC con un stored procedure genérico
                // Por ahora, usaremos el método de tabla con filtros como alternativa
                throw new NotImplementedException("SQL directo no está implementado. Use stored procedures o consultas de tabla.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ejecutando consulta SQL: {Error}", ex.Message);
                return new List<T>();
            }
        }

        /// <summary>
        /// Ejecuta una consulta SQL directa sin retorno de datos
        /// </summary>
        protected async Task ExecuteQueryAsync(string sqlQuery, object[] parameters)
        {
            try
            {
                _logger.LogInformation("Ejecutando comando SQL directo");
                
                // Para comandos SQL directos, usamos RPC con un stored procedure genérico
                throw new NotImplementedException("SQL directo no está implementado. Use stored procedures.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ejecutando comando SQL: {Error}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Verifica la conectividad con Supabase
        /// </summary>
        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                _logger.LogInformation("Probando conexión con Supabase");
                
                // Intentar una consulta simple
                var result = await _supabaseClient.Rpc("version", null);
                
                _logger.LogInformation("Conexión con Supabase exitosa");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error de conexión con Supabase: {Error}", ex.Message);
                return false;
            }
        }
    }

    /// <summary>
    /// Modelos de respuesta para stored procedures
    /// </summary>
    public class StoredProcedureResult
    {
        public bool success { get; set; }
        public string message { get; set; } = string.Empty;
    }

    public class AuthLoginResult : StoredProcedureResult
    {
        public Guid? user_id { get; set; }
        public string? app_codigo { get; set; }
        public string? usuario_nombre { get; set; }
        public string? usuario_email { get; set; }
    }

    public class ConversationResult
    {
        public Guid conversation_id { get; set; }
        public long conversation_code { get; set; }
        public string? conversation_name { get; set; }
        public string? conversation_type { get; set; }
        public DateTime creation_date { get; set; }
        public bool is_active { get; set; }
        public string? last_message_text { get; set; }
        public DateTime? last_message_date { get; set; }
        public long unread_count { get; set; }
    }

    public class MessageResult
    {
        public Guid message_id { get; set; }
        public long message_code { get; set; }
        public Guid user_id { get; set; }
        public string? user_name { get; set; }
        public string? message_text { get; set; }
        public string? message_type { get; set; }
        public DateTime message_date { get; set; }
        public bool is_read { get; set; }
    }

    public class SendMessageResult
    {
        public bool success { get; set; }
        public string message { get; set; } = string.Empty;
        public Guid? message_id { get; set; }
        public object? message_data { get; set; }
    }

    public class AppRegisterResult : StoredProcedureResult
    {
        public Guid? app_id { get; set; }
        public string? access_token { get; set; }
        public string? secret_token { get; set; }
    }

    public class WebhookRegisterResult : StoredProcedureResult
    {
        public string? webhook_token { get; set; }
    }

    public class PersonaSyncResult : StoredProcedureResult
    {
        public Guid? user_id { get; set; }
        public bool was_created { get; set; }
    }

    public class CreateConversationResult : StoredProcedureResult
    {
        public Guid? conversation_id { get; set; }
        public long? conversation_code { get; set; }
    }
}