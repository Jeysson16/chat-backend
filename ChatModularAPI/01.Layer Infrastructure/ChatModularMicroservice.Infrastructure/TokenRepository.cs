using ChatModularMicroservice.Entities.Models;
using ChatModularMicroservice.Entities;
using ChatModularMicroservice.Repository;
using Utils = ChatModularMicroservice.Shared.Utils;
using ChatModularMicroservice.Shared.Configs;
using Dapper;
using System.Data;

namespace ChatModularMicroservice.Infrastructure;

public class TokenRepository : BaseRepository, ITokenRepository
{
    #region Constructor
    public TokenRepository(IConnectionFactory cn) : base(cn)
    {
    }
    #endregion

    #region Public Methods

    public async Task<int> Insert(TokenRegistro item)
    {
        int affectedRows = 0;
        var query = "USP_TokenRegistro_Insert";
        var parameters = new DynamicParameters();

        parameters.Add("@nTokenRegistroCodigo", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("@cTokenRegistroCodigoApp", item.cTokenRegistroCodigoApp, DbType.String);
        parameters.Add("@cTokenRegistroPerJurCodigo", item.cTokenRegistroPerJurCodigo, DbType.String);
        parameters.Add("@cTokenRegistroPerCodigo", item.cTokenRegistroPerCodigo, DbType.String);
        parameters.Add("@cTokenRegistroJwtToken", item.cTokenRegistroJwtToken, DbType.String);
        parameters.Add("@cTokenRegistroUsuarioId", item.cTokenRegistroUsuarioId, DbType.String);
        parameters.Add("@dTokenRegistroFechaExpiracion", item.dTokenRegistroFechaExpiracion, DbType.DateTime);
        parameters.Add("@dTokenRegistroFechaCreacion", item.dTokenRegistroFechaCreacion, DbType.DateTime);
        parameters.Add("@bTokenRegistroEsActivo", item.bTokenRegistroEsActivo, DbType.Boolean);

        affectedRows = await SqlMapper.ExecuteAsync(_connectionFactory.GetConnection(), query, parameters, commandType: CommandType.StoredProcedure);
        int generatedId = parameters.Get<int>("@nTokenRegistroCodigo");

        if (affectedRows <= 0 || generatedId <= 0)
        {
            throw new InvalidOperationException("No se pudo insertar el token o no se obtuvo un ID válido");
        }

        return generatedId;
    }

    public async Task<bool> Update(TokenRegistro item) =>
        await this.UpdateOrDelete("USP_TokenRegistro_Update", new DynamicParameters(new Dictionary<string, object>
        {
            {"@nTokenRegistroCodigo", item.nTokenRegistroId},
            {"@cTokenRegistroCodigoApp", item.cTokenRegistroCodigoApp},
            {"@cTokenRegistroPerJurCodigo", item.cTokenRegistroPerJurCodigo},
            {"@cTokenRegistroPerCodigo", item.cTokenRegistroPerCodigo},
            {"@cTokenRegistroJwtToken", item.cTokenRegistroJwtToken},
            {"@cTokenRegistroUsuarioId", item.cTokenRegistroUsuarioId},
            {"@dTokenRegistroFechaExpiracion", item.dTokenRegistroFechaExpiracion},
            {"@dTokenRegistroFechaCreacion", item.dTokenRegistroFechaCreacion},
            {"@bTokenRegistroEsActivo", item.bTokenRegistroEsActivo}
        }));

    public async Task<bool> DeleteEntero(Int32 nTokenRegistroCodigo) =>
        await this.UpdateOrDelete("USP_TokenRegistro_Delete", new DynamicParameters(new Dictionary<string, object>
        {
            {"@nTokenRegistroCodigo", nTokenRegistroCodigo}
        }));

    public async Task<TokenRegistro> GetItem(TokenRegistroFilter filter, TokenRegistroFilterItemType filterType)
    {
        TokenRegistro itemfound = null;
        switch (filterType)
        {
            case TokenRegistroFilterItemType.ById:
                itemfound = await this.GetById(filter);
                break;
            case TokenRegistroFilterItemType.ByJwtToken:
                itemfound = await this.GetByJwtToken(filter);
                break;
            case TokenRegistroFilterItemType.ByUsuarioId:
                itemfound = await this.GetByUsuarioId(filter);
                break;
            case TokenRegistroFilterItemType.ByAppCodigo:
                itemfound = await this.GetByAppCodigo(filter);
                break;
            case TokenRegistroFilterItemType.ByPerJurYPerCodigo:
                itemfound = await this.GetByPerJurYPerCodigo(filter);
                break;
        }
        return itemfound;
    }

    // Wrappers para cumplir con la interfaz usando los nuevos tipos de filtros
    public async Task<TokenRegistro> GetItem(TokenFilter filter, TokenFilterItemType filterType)
    {
        var legacyFilter = new TokenRegistroFilter
        {
            cTokenRegistroJwtToken = filter.TokenAcceso,
            cTokenRegistroCodigoApp = filter.CodigoAplicacion,
            bTokenRegistroEsActivo = filter.TokenEsActivo
        };

        switch (filterType)
        {
            case TokenFilterItemType.TokenId:
                throw new NotSupportedException("Filtro por TokenId (GUID) no soportado por repositorio legacy.");
            case TokenFilterItemType.TokenAcceso:
                return await GetItem(legacyFilter, TokenRegistroFilterItemType.ByJwtToken);
            case TokenFilterItemType.CodigoAplicacion:
                return await GetItem(legacyFilter, TokenRegistroFilterItemType.ByAppCodigo);
            case TokenFilterItemType.TokenEsActivo:
            case TokenFilterItemType.FechaExpiracion:
                throw new NotSupportedException("El filtro de item por estado/fecha no está soportado; use GetLstItem.");
            default:
                throw new NotSupportedException("Tipo de filtro de item no soportado.");
        }
    }

    public async Task<IEnumerable<TokenRegistro>> GetLstItem(TokenRegistroFilter filter, TokenRegistroFilterListType filterType, Utils.Pagination pagination)
    {
        IEnumerable<TokenRegistro> lstItemFound = new List<TokenRegistro>();
        switch (filterType)
        {
            case TokenRegistroFilterListType.ByPagination:
                lstItemFound = await this.GetByPagination(filter, pagination);
                break;
            case TokenRegistroFilterListType.ByAppCodigo:
                lstItemFound = await this.GetByAppCodigoList(filter);
                break;
            case TokenRegistroFilterListType.ByUsuarioId:
                lstItemFound = await this.GetByUsuarioIdList(filter);
                break;
            case TokenRegistroFilterListType.ByActivos:
                lstItemFound = await this.GetByActivos(filter);
                break;
            case TokenRegistroFilterListType.ByExpirados:
                lstItemFound = await this.GetByExpirados(filter);
                break;
            case TokenRegistroFilterListType.ByPerJurYPerCodigo:
                lstItemFound = await this.GetByPerJurYPerCodigoList(filter);
                break;
            case TokenRegistroFilterListType.ByTerminoBusqueda:
                lstItemFound = await this.GetByTerminoBusqueda(filter);
                break;
            case TokenRegistroFilterListType.All:
                lstItemFound = await this.GetAll(filter);
                break;
        }
        return lstItemFound;
    }

    public async Task<IEnumerable<TokenRegistro>> GetLstItem(TokenFilter filter, TokenFilterListType filterType, Utils.Pagination pagination)
    {
        var legacyFilter = new TokenRegistroFilter
        {
            cTokenRegistroCodigoApp = filter.CodigoAplicacion,
            bTokenRegistroEsActivo = filter.TokenEsActivo,
            cTokenRegistroJwtToken = filter.TokenAcceso
        };

        switch (filterType)
        {
            case TokenFilterListType.TokensActivos:
                legacyFilter.bTokenRegistroEsActivo = true;
                return await GetLstItem(legacyFilter, TokenRegistroFilterListType.ByActivos, pagination);
            case TokenFilterListType.TodosTokens:
                return await GetLstItem(legacyFilter, TokenRegistroFilterListType.All, pagination);
            case TokenFilterListType.TokensPorAplicacion:
                return await GetLstItem(legacyFilter, TokenRegistroFilterListType.ByAppCodigo, pagination);
            case TokenFilterListType.TokensExpirados:
                return await GetLstItem(legacyFilter, TokenRegistroFilterListType.ByExpirados, pagination);
            case TokenFilterListType.TokensProximosExpirar:
                throw new NotSupportedException("Listado de tokens próximos a expirar no soportado actualmente.");
            default:
                throw new NotSupportedException("Tipo de filtro de listado no soportado.");
        }
    }

    // Métodos específicos del dominio (mantenidos para compatibilidad)
    public async Task<TokenRegistro> CreateTokenAsync(string appCode, string perJurCodigo, string perCodigo, string tokenString, DateTime expiration, string usuarioId = null)
    {
        var tokenRegistro = new TokenRegistro
        {
            cTokenRegistroCodigoApp = appCode ?? "CHAT",
            cTokenRegistroPerJurCodigo = !string.IsNullOrEmpty(perJurCodigo) ? perJurCodigo : "DEFAULT",
            cTokenRegistroPerCodigo = !string.IsNullOrEmpty(perCodigo) ? perCodigo : "DEFAULT",
            cTokenRegistroJwtToken = tokenString,
            cTokenRegistroUsuarioId = usuarioId,
            dTokenRegistroFechaExpiracion = expiration,
            dTokenRegistroFechaCreacion = DateTime.UtcNow,
            bTokenRegistroEsActivo = true
        };

        int generatedId = await Insert(tokenRegistro);
        tokenRegistro.nTokenRegistroId = generatedId;
        
        return tokenRegistro;
    }

    public async Task<TokenRegistro?> GetTokenAsync(string jwtToken)
    {
        var filter = new TokenRegistroFilter { cTokenRegistroJwtToken = jwtToken };
        return await GetItem(filter, TokenRegistroFilterItemType.ByJwtToken);
    }

    public async Task<bool> IsTokenActiveAsync(string jwtToken)
    {
        string query = "USP_TokenRegistro_IsActive";
        var param = new DynamicParameters();
        param.Add("@cTokenRegistroJwtToken", jwtToken);
        
        var result = await this.LoadData<bool>(query, param);
        return result.FirstOrDefault();
    }

    public async Task<bool> RevokeTokenAsync(string jwtToken)
    {
        string query = "USP_TokenRegistro_Revoke";
        var param = new DynamicParameters();
        param.Add("@cTokenRegistroJwtToken", jwtToken);
        
        return await this.UpdateOrDelete(query, param);
    }

    public async Task<List<TokenRegistro>> GetUserTokensAsync(string appCode, string perJurCodigo, string perCodigo)
    {
        var filter = new TokenRegistroFilter 
        { 
            cTokenRegistroCodigoApp = appCode,
            cTokenRegistroPerJurCodigo = perJurCodigo,
            cTokenRegistroPerCodigo = perCodigo
        };
        
        var result = await GetLstItem(filter, TokenRegistroFilterListType.ByPerJurYPerCodigo, new Utils.Pagination());
        return result.ToList();
    }

    public async Task<bool> RevokeUserTokensAsync(string appCode, string perJurCodigo, string perCodigo)
    {
        string query = "USP_TokenRegistro_RevokeUserTokens";
        var param = new DynamicParameters();
        param.Add("@cTokenRegistroCodigoApp", appCode);
        param.Add("@cTokenRegistroPerJurCodigo", perJurCodigo);
        param.Add("@cTokenRegistroPerCodigo", perCodigo);
        
        return await this.UpdateOrDelete(query, param);
    }

    public async Task<bool> InvalidateUserTokensAsync(Guid userId)
    {
        string query = "USP_TokenRegistro_InvalidateUserTokens";
        var param = new DynamicParameters();
        param.Add("@cTokenRegistroUsuarioId", userId.ToString());
        
        return await this.UpdateOrDelete(query, param);
    }

    public async Task<TokenRegistro?> GetRefreshTokenAsync(string refreshToken)
    {
        string query = "USP_TokenRegistro_GetRefreshToken";
        var param = new DynamicParameters();
        param.Add("@cRefreshToken", refreshToken);
        
        return (await this.LoadData<TokenRegistro>(query, param)).FirstOrDefault();
    }

    public async Task<bool> UpdateRefreshTokenAsync(long tokenCodigo, string newRefreshToken, string? usuarioId = null)
    {
        string query = "USP_TokenRegistro_UpdateRefreshToken";
        var param = new DynamicParameters();
        param.Add("@nTokenCodigo", tokenCodigo);
        param.Add("@cNewRefreshToken", newRefreshToken);
        param.Add("@cUsuarioId", usuarioId);
        
        return await this.UpdateOrDelete(query, param);
    }

    public async Task<bool> TokenExistsAsync(string tokenId)
    {
        string query = "USP_TokenRegistro_Exists";
        var param = new DynamicParameters();
        param.Add("@cTokenRegistroJwtToken", tokenId);
        
        var result = await this.LoadData<bool>(query, param);
        return result.FirstOrDefault();
    }

    public async Task<bool> ValidateTokenAsync(string tokenValue)
    {
        string query = "USP_TokenRegistro_Validate";
        var param = new DynamicParameters();
        param.Add("@cTokenRegistroJwtToken", tokenValue);
        
        var result = await this.LoadData<bool>(query, param);
        return result.FirstOrDefault();
    }

    public async Task<string> GenerateTokenAsync(string userId, string appCode)
    {
        // Generar un nuevo token JWT
        var tokenValue = Guid.NewGuid().ToString();
        
        var tokenRegistro = new TokenRegistro
        {
            cTokenRegistroCodigoApp = appCode,
            cTokenRegistroUsuarioId = userId,
            cTokenRegistroJwtToken = tokenValue,
            dTokenRegistroFechaCreacion = DateTime.UtcNow,
            dTokenRegistroFechaExpiracion = DateTime.UtcNow.AddHours(24),
            bTokenRegistroEsActivo = true
        };

        var insertedId = await Insert(tokenRegistro);
        return insertedId > 0 ? tokenValue : string.Empty;
    }

    public async Task<string> RefreshTokenAsync(string refreshToken)
    {
        var existingToken = await GetRefreshTokenAsync(refreshToken);
        if (existingToken == null || !existingToken.bTokenRegistroEsActivo)
        {
            return string.Empty;
        }

        // Generar nuevo token
        var newTokenValue = Guid.NewGuid().ToString();
        var success = await UpdateRefreshTokenAsync(existingToken.nTokenRegistroId, newTokenValue, existingToken.cTokenRegistroUsuarioId);
        
        return success ? newTokenValue : string.Empty;
    }

    public async Task<TokenRegistro?> CreateTokenAsync(TokenRegistro token)
    {
        var insertedId = await Insert(token);
        if (insertedId > 0)
        {
            // Obtener el token creado
            var filter = new TokenRegistroFilter { nTokenRegistroId = insertedId };
            return await GetItem(filter, TokenRegistroFilterItemType.ById);
        }
        return null;
    }

    public async Task<bool> UpdateTokenAsync(TokenRegistro token)
    {
        // Usar el método Update(T) del repositorio genérico
        return await Update(token);
    }

    public async Task<bool> DeleteTokenAsync(string tokenId)
    {
        // Eliminar por ID entero
        if (int.TryParse(tokenId, out int intId))
        {
            return await DeleteEntero(intId);
        }

        // Fallback y compatibilidad: revocar por valor del token (JWT/refresh)
        // Si en el futuro se soporta eliminación por GUID, ajustar aquí.
        return await RevokeTokenAsync(tokenId);
    }

    #endregion

    #region Private Methods

    private async Task<TokenRegistro?> GetById(TokenRegistroFilter filter)
    {
        string query = "USP_TokenRegistro_GetById";
        var param = new DynamicParameters();
        param.Add("@nTokenRegistroCodigo", filter.nTokenRegistroId);
        return (await this.LoadData<TokenRegistro>(query, param)).FirstOrDefault();
    }

    private async Task<TokenRegistro?> GetByJwtToken(TokenRegistroFilter filter)
    {
        string query = "USP_TokenRegistro_GetByJwtToken";
        var param = new DynamicParameters();
        param.Add("@cTokenRegistroJwtToken", filter.cTokenRegistroJwtToken);
        return (await this.LoadData<TokenRegistro>(query, param)).FirstOrDefault();
    }

    private async Task<TokenRegistro?> GetByUsuarioId(TokenRegistroFilter filter)
    {
        string query = "USP_TokenRegistro_GetByUsuarioId";
        var param = new DynamicParameters();
        param.Add("@cTokenRegistroUsuarioId", filter.cTokenRegistroUsuarioId);
        return (await this.LoadData<TokenRegistro>(query, param)).FirstOrDefault();
    }

    private async Task<TokenRegistro?> GetByAppCodigo(TokenRegistroFilter filter)
    {
        string query = "USP_TokenRegistro_GetByAppCodigo";
        var param = new DynamicParameters();
        param.Add("@cTokenRegistroCodigoApp", filter.cTokenRegistroCodigoApp);
        return (await this.LoadData<TokenRegistro>(query, param)).FirstOrDefault();
    }

    private async Task<TokenRegistro?> GetByPerJurYPerCodigo(TokenRegistroFilter filter)
    {
        string query = "USP_TokenRegistro_GetByPerJurYPerCodigo";
        var param = new DynamicParameters();
        param.Add("@cTokenRegistroPerJurCodigo", filter.cTokenRegistroPerJurCodigo);
        param.Add("@cTokenRegistroPerCodigo", filter.cTokenRegistroPerCodigo);
        return (await this.LoadData<TokenRegistro>(query, param)).FirstOrDefault();
    }

    private async Task<IEnumerable<TokenRegistro>> GetByPagination(TokenRegistroFilter filter, Utils.Pagination pagination)
    {
        string query = "USP_TokenRegistro_GetByPagination";
        var param = new DynamicParameters();
        param.Add("@PageNumber", pagination.PageNumber);
        param.Add("@PageSize", pagination.PageSize);
        return await this.LoadData<TokenRegistro>(query, param);
    }

    private async Task<IEnumerable<TokenRegistro>> GetByAppCodigoList(TokenRegistroFilter filter)
    {
        string query = "USP_TokenRegistro_GetByAppCodigoList";
        var param = new DynamicParameters();
        param.Add("@cTokenRegistroCodigoApp", filter.cTokenRegistroCodigoApp);
        return await this.LoadData<TokenRegistro>(query, param);
    }

    private async Task<IEnumerable<TokenRegistro>> GetByUsuarioIdList(TokenRegistroFilter filter)
    {
        string query = "USP_TokenRegistro_GetByUsuarioIdList";
        var param = new DynamicParameters();
        param.Add("@cTokenRegistroUsuarioId", filter.cTokenRegistroUsuarioId);
        return await this.LoadData<TokenRegistro>(query, param);
    }

    private async Task<IEnumerable<TokenRegistro>> GetByActivos(TokenRegistroFilter filter)
    {
        string query = "USP_TokenRegistro_GetByActivos";
        var param = new DynamicParameters();
        param.Add("@bTokenRegistroEsActivo", filter.bTokenRegistroEsActivo);
        return await this.LoadData<TokenRegistro>(query, param);
    }

    private async Task<IEnumerable<TokenRegistro>> GetByExpirados(TokenRegistroFilter filter)
    {
        string query = "USP_TokenRegistro_GetByExpirados";
        var param = new DynamicParameters();
        param.Add("@dFechaActual", DateTime.UtcNow);
        return await this.LoadData<TokenRegistro>(query, param);
    }

    private async Task<IEnumerable<TokenRegistro>> GetByPerJurYPerCodigoList(TokenRegistroFilter filter)
    {
        string query = "USP_TokenRegistro_GetByPerJurYPerCodigoList";
        var param = new DynamicParameters();
        param.Add("@cTokenRegistroPerJurCodigo", filter.cTokenRegistroPerJurCodigo);
        param.Add("@cTokenRegistroPerCodigo", filter.cTokenRegistroPerCodigo);
        return await this.LoadData<TokenRegistro>(query, param);
    }

    private async Task<IEnumerable<TokenRegistro>> GetByTerminoBusqueda(TokenRegistroFilter filter)
    {
        string query = "USP_TokenRegistro_GetByTerminoBusqueda";
        var param = new DynamicParameters();
        param.Add("@TerminoBusqueda", filter.TerminoBusqueda);
        return await this.LoadData<TokenRegistro>(query, param);
    }

    private async Task<IEnumerable<TokenRegistro>> GetAll(TokenRegistroFilter filter)
    {
        string query = "USP_TokenRegistro_GetAll";
        var param = new DynamicParameters();
        return await this.LoadData<TokenRegistro>(query, param);
    }

    #endregion
}