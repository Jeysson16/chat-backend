using ChatModularMicroservice.Entities;
using ChatModularMicroservice.Entities.Models;
using ChatModularMicroservice.Repository;
using Utils = ChatModularMicroservice.Shared.Utils;
using ChatModularMicroservice.Shared.Configs;
using Dapper;
using System.Data;

namespace ChatModularMicroservice.Infrastructure;

public class ApplicationRepository : BaseRepository, IApplicationRepository
{
    #region Constructor
    public ApplicationRepository(IConnectionFactory cn) : base(cn)
    {
    }
    #endregion

    #region Public Methods

    public async Task<int> Insert(Application item)
    {
        int affectedRows = 0;
        var query = "USP_Application_Insert";
        var parameters = new DynamicParameters();

        parameters.Add("@nAplicacionesId", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("@cAplicacionesNombre", item.cAplicacionesNombre, DbType.String);
        parameters.Add("@cAplicacionesCodigo", item.cAplicacionesCodigo, DbType.String);
        parameters.Add("@cAplicacionesDescripcion", item.cAplicacionesDescripcion, DbType.String);
        parameters.Add("@cAplicacionesVersion", item.cAplicacionesVersion, DbType.String);
        parameters.Add("@bAplicacionesEsActiva", item.bAplicacionesEsActiva, DbType.Boolean);

        affectedRows = await SqlMapper.ExecuteAsync(_connectionFactory.GetConnection(), query, parameters, commandType: CommandType.StoredProcedure);
        int generatedId = parameters.Get<int>("@nAplicacionesId");

        if (affectedRows <= 0 || generatedId <= 0)
        {
            throw new InvalidOperationException("No se pudo insertar la aplicación o no se obtuvo un ID válido");
        }

        return generatedId;
    }

    public async Task<bool> Update(Application item) =>
        await this.UpdateOrDelete("USP_Application_Update", new DynamicParameters(new Dictionary<string, object>
        {
            {"@nAplicacionesId", item.nAplicacionesId},
            {"@cAplicacionesNombre", item.cAplicacionesNombre},
            {"@cAplicacionesCodigo", item.cAplicacionesCodigo},
            {"@cAplicacionesDescripcion", item.cAplicacionesDescripcion},
            {"@cAplicacionesVersion", item.cAplicacionesVersion},
            {"@bAplicacionesEsActiva", item.bAplicacionesEsActiva}
        }));

    public async Task<bool> DeleteEntero(Int32 nAplicacionesId) =>
        await this.UpdateOrDelete("USP_Application_Delete", new DynamicParameters(new Dictionary<string, object>
        {
            {"@nAplicacionesId", nAplicacionesId}
        }));

    public async Task<Application> GetItem(ApplicationFilter filter, ApplicationFilterItemType filterType)
    {
        Application itemfound = null;
        switch (filterType)
        {
            case ApplicationFilterItemType.ById:
                itemfound = await this.GetById(filter);
                break;
            case ApplicationFilterItemType.ByCodigo:
                itemfound = await this.GetByCodigo(filter);
                break;
            case ApplicationFilterItemType.ByNombre:
                itemfound = await this.GetByNombre(filter);
                break;
        }
        return itemfound;
    }

    public async Task<IEnumerable<Application>> GetLstItem(ApplicationFilter filter, ApplicationFilterListType filterType, Utils.Pagination pagination)
    {
        IEnumerable<Application> lstItemFound = new List<Application>();
        switch (filterType)
        {
            case ApplicationFilterListType.ByPagination:
                lstItemFound = await this.GetByPagination(filter, pagination);
                break;
            case ApplicationFilterListType.ByActivas:
                lstItemFound = await this.GetByActivas(filter);
                break;
            case ApplicationFilterListType.ByVersion:
                lstItemFound = await this.GetByVersion(filter);
                break;
            case ApplicationFilterListType.ByTerminoBusqueda:
                lstItemFound = await this.GetByTerminoBusqueda(filter);
                break;
        }
        return lstItemFound;
    }

    // Métodos específicos del dominio (mantenidos para compatibilidad)
    public async Task<IEnumerable<Application>> GetAllApplicationsAsync()
    {
        string query = "USP_Application_GetAll";
        var param = new DynamicParameters();
        return await this.LoadData<Application>(query, param);
    }

    public async Task<Application?> GetApplicationByIdAsync(int id)
    {
        string query = "USP_Application_GetById";
        var param = new DynamicParameters();
        param.Add("@nAplicacionesId", id);
        return (await this.LoadData<Application>(query, param)).FirstOrDefault();
    }

    public async Task<Application?> GetApplicationByCodeAsync(string code)
    {
        string query = "USP_Application_GetByCode";
        var param = new DynamicParameters();
        param.Add("@cAplicacionesCodigo", code);
        return (await this.LoadData<Application>(query, param)).FirstOrDefault();
    }

    public async Task<Application> CreateApplicationAsync(Application application)
    {
        var id = await Insert(application);
        application.nAplicacionesId = id;
        return application;
    }

    public async Task<Application> UpdateApplicationAsync(Application application)
    {
        await Update(application);
        return application;
    }

    public async Task<bool> DeleteApplicationAsync(int id)
    {
        return await DeleteEntero(id);
    }

    public async Task<bool> ApplicationExistsAsync(int id)
    {
        var app = await GetApplicationByIdAsync(id);
        return app != null;
    }

    public async Task<bool> ApplicationExistsByNameAsync(string name, int? excludeId = null)
    {
        string query = "USP_Application_ExistsByName";
        var param = new DynamicParameters();
        param.Add("@cAplicacionesNombre", name);
        param.Add("@nExcludeId", excludeId);
        var result = await this.LoadData<int>(query, param);
        return result.FirstOrDefault() > 0;
    }

    public async Task<ConfiguracionAplicacion?> GetApplicationConfigurationAsync(int applicationId)
    {
        string query = "USP_ApplicationConfiguration_GetByApplicationId";
        var param = new DynamicParameters();
        param.Add("@nAplicacionesId", applicationId);
        return (await this.LoadData<ConfiguracionAplicacion>(query, param)).FirstOrDefault();
    }

    public async Task<ConfiguracionAplicacion> CreateApplicationConfigurationAsync(ConfiguracionAplicacion configuration)
    {
        string query = "USP_ApplicationConfiguration_Insert";
        var param = new DynamicParameters();
        param.Add("@nConfiguracionAplicacionId", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@nConfiguracionAplicacionAplicacionId", configuration.nConfiguracionAplicacionAplicacionId);
        param.Add("@cConfiguracionAplicacionClave", configuration.cConfiguracionAplicacionClave);
        param.Add("@cConfiguracionAplicacionValor", configuration.cConfiguracionAplicacionValor);
        param.Add("@cConfiguracionAplicacionDescripcion", configuration.cConfiguracionAplicacionDescripcion);
        param.Add("@bConfiguracionAplicacionEsActiva", configuration.bConfiguracionAplicacionEsActiva);

        await SqlMapper.ExecuteAsync(_connectionFactory.GetConnection(), query, param, commandType: CommandType.StoredProcedure);
        configuration.nConfiguracionAplicacionId = param.Get<int>("@nConfiguracionAplicacionId");
        return configuration;
    }

    public async Task<ConfiguracionAplicacion> UpdateApplicationConfigurationAsync(ConfiguracionAplicacion configuration)
    {
        string query = "USP_ApplicationConfiguration_Update";
        var param = new DynamicParameters();
        param.Add("@nConfiguracionAplicacionId", configuration.nConfiguracionAplicacionId);
        param.Add("@nConfiguracionAplicacionAplicacionId", configuration.nConfiguracionAplicacionAplicacionId);
        param.Add("@cConfiguracionAplicacionClave", configuration.cConfiguracionAplicacionClave);
        param.Add("@cConfiguracionAplicacionValor", configuration.cConfiguracionAplicacionValor);
        param.Add("@cConfiguracionAplicacionDescripcion", configuration.cConfiguracionAplicacionDescripcion);
        param.Add("@bConfiguracionAplicacionEsActiva", configuration.bConfiguracionAplicacionEsActiva);

        await SqlMapper.ExecuteAsync(_connectionFactory.GetConnection(), query, param, commandType: CommandType.StoredProcedure);
        return configuration;
    }

    public async Task<bool> DeleteApplicationConfigurationAsync(int applicationId)
    {
        return await this.UpdateOrDelete("USP_ApplicationConfiguration_DeleteByApplicationId", new DynamicParameters(new Dictionary<string, object>
        {
            {"@nAplicacionesId", applicationId}
        }));
    }

    #endregion

    #region Private Methods

    private async Task<Application> GetById(ApplicationFilter filter)
    {
        string query = "USP_Application_GetById";
        var param = new DynamicParameters();
        param.Add("@nAplicacionesId", filter.nAplicacionesId);
        return (await this.LoadData<Application>(query, param)).FirstOrDefault();
    }

    private async Task<Application> GetByCodigo(ApplicationFilter filter)
    {
        string query = "USP_Application_GetByCodigo";
        var param = new DynamicParameters();
        param.Add("@cAplicacionesCodigo", filter.cAplicacionesCodigo);
        return (await this.LoadData<Application>(query, param)).FirstOrDefault();
    }

    private async Task<Application> GetByNombre(ApplicationFilter filter)
    {
        string query = "USP_Application_GetByNombre";
        var param = new DynamicParameters();
        param.Add("@cAplicacionesNombre", filter.cAplicacionesNombre);
        return (await this.LoadData<Application>(query, param)).FirstOrDefault();
    }

    private async Task<IEnumerable<Application>> GetByPagination(ApplicationFilter filter, Utils.Pagination pagination)
    {
        string query = "USP_Application_GetByPagination";
        var param = new DynamicParameters();
        param.Add("@PageNumber", pagination.PageNumber);
        param.Add("@PageSize", pagination.PageSize);
        return await this.LoadData<Application>(query, param);
    }

    private async Task<IEnumerable<Application>> GetByActivas(ApplicationFilter filter)
    {
        string query = "USP_Application_GetByActivas";
        var param = new DynamicParameters();
        param.Add("@bAplicacionesEsActiva", filter.bAplicacionesEsActiva);
        return await this.LoadData<Application>(query, param);
    }

    private async Task<IEnumerable<Application>> GetByVersion(ApplicationFilter filter)
    {
        string query = "USP_Application_GetByVersion";
        var param = new DynamicParameters();
        param.Add("@cAplicacionesVersion", filter.cAplicacionesVersion);
        return await this.LoadData<Application>(query, param);
    }

    private async Task<IEnumerable<Application>> GetByTerminoBusqueda(ApplicationFilter filter)
    {
        string query = "USP_Application_GetByTerminoBusqueda";
        var param = new DynamicParameters();
        param.Add("@TerminoBusqueda", filter.TerminoBusqueda);
        return await this.LoadData<Application>(query, param);
    }

    #endregion
}