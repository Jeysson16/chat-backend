using Dapper;
using System.Data;
using ChatModularMicroservice.Shared.Configs;

namespace ChatModularMicroservice.Infrastructure
{
    public abstract class BaseRepository
    {
        protected readonly IConnectionFactory _connectionFactory;

        protected BaseRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        protected async Task<bool> UpdateOrDelete(string storedProcedure, DynamicParameters parameters)
        {
            int affectedRows = await SqlMapper.ExecuteAsync(
                _connectionFactory.GetConnection(),
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return affectedRows > 0;
        }

        protected async Task<IEnumerable<T>> LoadData<T>(string storedProcedure, DynamicParameters parameters)
        {
            return await SqlMapper.QueryAsync<T>(
                _connectionFactory.GetConnection(),
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        protected async Task<T?> LoadSingleData<T>(string storedProcedure, DynamicParameters parameters)
        {
            var result = await LoadData<T>(storedProcedure, parameters);
            return result.FirstOrDefault();
        }
    }
}