using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatModularMicroservice.Shared.Utils
{
    /// <summary>
    /// Respuesta genérica de servicios compatible con controladores existentes
    /// </summary>
    /// <typeparam name="T">Tipo de datos principal</typeparam>
    public class ServiceResponse<T>
    {
        public string clientName { get; set; } = string.Empty;
        public bool isSuccess { get; set; } = true;
        public List<string> lstError { get; set; } = new();
        public T? data { get; set; }
        public List<T>? lstItem { get; set; }
        public object? pagination { get; set; }
        public object? resultado { get; set; }
        public string serverName { get; set; } = Environment.MachineName;
        public string ticket { get; set; } = Guid.NewGuid().ToString();
        public string userName { get; set; } = string.Empty;
        public List<string> warnings { get; set; } = new();

        public ServiceResponse()
        {
            serverName = Environment.MachineName;
            ticket = Guid.NewGuid().ToString();
        }

        public static ServiceResponse<T> Success(T item)
            => new ServiceResponse<T> { isSuccess = true, data = item };

        public static ServiceResponse<T> SuccessList(IEnumerable<T> items, object? pagination = null)
            => new ServiceResponse<T> { isSuccess = true, lstItem = items?.ToList(), pagination = pagination };

        public static ServiceResponse<T> Success()
            => new ServiceResponse<T> { isSuccess = true };

        public static ServiceResponse<T> Error(params string[] errors)
            => new ServiceResponse<T>
            {
                isSuccess = false,
                lstError = errors?.Where(e => !string.IsNullOrWhiteSpace(e)).ToList() ?? new List<string>()
            };

        public static ServiceResponse<T> FromErrors(IEnumerable<string> errors)
            => new ServiceResponse<T>
            {
                isSuccess = false,
                lstError = errors?.Where(e => !string.IsNullOrWhiteSpace(e)).ToList() ?? new List<string>()
            };

        public ServiceResponse<T> WithWarnings(IEnumerable<string> warnings)
        {
            if (warnings != null)
            {
                this.warnings.AddRange(warnings.Where(w => !string.IsNullOrWhiteSpace(w)));
            }
            return this;
        }
    }
}