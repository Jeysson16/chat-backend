using System.Data;

namespace ChatModularMicroservice.Shared.Configs
{
    /// <summary>
    /// Fábrica de conexiones a base de datos.
    /// </summary>
    public interface IConnectionFactory
    {
        /// <summary>
        /// Obtiene una conexión abierta a la base de datos.
        /// </summary>
        /// <returns>Instancia de IDbConnection</returns>
        IDbConnection GetConnection();
    }
}