using ChatModularMicroservice.Entities.Models;
using ChatModularMicroservice.Entities;
using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Repository;
using Utils = ChatModularMicroservice.Shared.Utils;
using ChatModularMicroservice.Shared.Configs;
using Dapper;
using System.Data;
using System.Text.RegularExpressions;

namespace ChatModularMicroservice.Infrastructure
{
    /// <summary>
    /// Repositorio para la gestión de empresas
    /// </summary>
    public class EmpresaRepository : BaseRepository, IEmpresaRepository
    {
        #region Constructor
        public EmpresaRepository(IConnectionFactory cn) : base(cn)
        {
        }
        #endregion

        #region Public Methods

        public async Task<int> Insert(Empresa item)
        {
            int affectedRows = 0;
            var query = "USP_Empresa_Insert";
            var parameters = new DynamicParameters();

            parameters.Add("@nEmpresasId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@cEmpresasNombre", item.cEmpresasNombre, DbType.String);
            parameters.Add("@cEmpresasCodigo", item.cEmpresasCodigo, DbType.String);
            parameters.Add("@nEmpresasAplicacionId", item.nEmpresasAplicacionId, DbType.Int32);
            parameters.Add("@bEmpresasEsActiva", item.bEmpresasEsActiva, DbType.Boolean);

            affectedRows = await SqlMapper.ExecuteAsync(_connectionFactory.GetConnection(), query, parameters, commandType: CommandType.StoredProcedure);
            int generatedId = parameters.Get<int>("@nEmpresasId");

            if (affectedRows <= 0 || generatedId <= 0)
            {
                throw new InvalidOperationException("No se pudo insertar la empresa o no se obtuvo un ID válido");
            }

            return generatedId;
        }

        public async Task<bool> Update(Empresa item) =>
            await this.UpdateOrDelete("USP_Empresa_Update", new DynamicParameters(new Dictionary<string, object>
            {
                {"@nEmpresasId", item.nEmpresasId},
                {"@cEmpresasNombre", item.cEmpresasNombre},
                {"@cEmpresasCodigo", item.cEmpresasCodigo},
                {"@nEmpresasAplicacionId", item.nEmpresasAplicacionId},
                {"@bEmpresasEsActiva", item.bEmpresasEsActiva}
            }));

        public async Task<bool> DeleteEntero(int nEmpresasId) =>
            await this.UpdateOrDelete("USP_Empresa_Delete", new DynamicParameters(new Dictionary<string, object>
            {
                {"@nEmpresasId", nEmpresasId}
            }));

        public async Task<Empresa> GetItem(EmpresaFilter filter, EmpresaFilterItemType filterType)
        {
            Empresa itemfound = null;
            switch (filterType)
            {
                case EmpresaFilterItemType.ById:
                    itemfound = await this.GetById(filter);
                    break;
                case EmpresaFilterItemType.ByCodigo:
                    itemfound = await this.GetByCodigo(filter);
                    break;
                case EmpresaFilterItemType.ByNombre:
                    itemfound = await this.GetByNombre(filter);
                    break;
            }
            return itemfound;
        }

        public async Task<IEnumerable<Empresa>> GetLstItem(EmpresaFilter filter, EmpresaFilterListType filterType, Utils.Pagination pagination)
        {
            IEnumerable<Empresa> lstItemFound = new List<Empresa>();
            switch (filterType)
            {
                case EmpresaFilterListType.ByPagination:
                    lstItemFound = await this.GetByPagination(filter, pagination);
                    break;
                case EmpresaFilterListType.ByActivas:
                    lstItemFound = await this.GetByActivas(filter);
                    break;
                case EmpresaFilterListType.ByTerminoBusqueda:
                    lstItemFound = await this.GetByTerminoBusqueda(filter);
                    break;
            }
            return lstItemFound;
        }

        public async Task<Empresa?> GetEmpresaByCodigoAsync(string codigo)
        {
            var filter = new EmpresaFilter { Codigo = codigo };
            return await this.GetItem(filter, EmpresaFilterItemType.ByCodigo);
        }

        public async Task<EmpresaDto> CreateEmpresaAsync(CreateEmpresaDto createDto)
        {
            var empresa = new Empresa
            {
                cEmpresasNombre = createDto.cEmpresasNombre,
                cEmpresasCodigo = createDto.cEmpresasCodigo,
                nEmpresasAplicacionId = createDto.nEmpresasAplicacionId,
                bEmpresasEsActiva = createDto.bEmpresasActiva
            };

            var newId = await this.Insert(empresa);
            
            return new EmpresaDto
            {
                nEmpresasId = newId,
                nEmpresasAplicacionId = empresa.nEmpresasAplicacionId,
                cEmpresasNombre = empresa.cEmpresasNombre,
                cEmpresasCodigo = empresa.cEmpresasCodigo,
                bEmpresasActiva = empresa.bEmpresasEsActiva,
                dEmpresasFechaCreacion = DateTime.UtcNow
            };
        }

        #endregion

        #region Private Methods

        private async Task<Empresa> GetById(EmpresaFilter filter)
        {
            string query = "USP_Empresa_GetById";
            var param = new DynamicParameters();
            param.Add("@nEmpresasId", filter.nEmpresasId);
            return (await this.LoadData<Empresa>(query, param)).FirstOrDefault();
        }

        private async Task<Empresa> GetByCodigo(EmpresaFilter filter)
        {
            string query = "USP_Empresa_GetByCodigo";
            var param = new DynamicParameters();
            param.Add("@cEmpresasCodigo", filter.cEmpresasCodigo);
            return (await this.LoadData<Empresa>(query, param)).FirstOrDefault();
        }

        private async Task<Empresa> GetByNombre(EmpresaFilter filter)
        {
            string query = "USP_Empresa_GetByNombre";
            var param = new DynamicParameters();
            param.Add("@cEmpresasNombre", filter.cEmpresasNombre);
            return (await this.LoadData<Empresa>(query, param)).FirstOrDefault();
        }

        private async Task<IEnumerable<Empresa>> GetByPagination(EmpresaFilter filter, Utils.Pagination pagination)
        {
            string query = "USP_Empresa_GetByPagination";
            var param = new DynamicParameters();
            param.Add("@PageNumber", pagination.PageNumber);
            param.Add("@PageSize", pagination.PageSize);
            param.Add("@nEmpresasAplicacionId", filter.nEmpresasAplicacionId);
            return await this.LoadData<Empresa>(query, param);
        }

        private async Task<IEnumerable<Empresa>> GetByActivas(EmpresaFilter filter)
        {
            string query = "USP_Empresa_GetActivas";
            var param = new DynamicParameters();
            param.Add("@nEmpresasAplicacionId", filter.nEmpresasAplicacionId);
            return await this.LoadData<Empresa>(query, param);
        }

        private async Task<IEnumerable<Empresa>> GetByTerminoBusqueda(EmpresaFilter filter)
        {
            string query = "USP_Empresa_GetByTerminoBusqueda";
            var param = new DynamicParameters();
            param.Add("@TerminoBusqueda", filter.TerminoBusqueda);
            param.Add("@nEmpresasAplicacionId", filter.nEmpresasAplicacionId);
            return await this.LoadData<Empresa>(query, param);
        }

        public async Task<bool> ExistsEmpresaByCodigoAsync(string codigo, int? excludeId = null)
        {
            string query = "USP_Empresa_ExistsByCodigo";
            var param = new DynamicParameters();
            param.Add("@cEmpresasCodigo", codigo);
            if (excludeId.HasValue)
            {
                param.Add("@nExcludeId", excludeId.Value);
            }
            var result = await this.LoadData<int>(query, param);
            return result.FirstOrDefault() > 0;
        }

        public async Task<bool> ExistsEmpresaByIdAsync(int id)
        {
            string query = "USP_Empresa_ExistsById";
            var param = new DynamicParameters();
            param.Add("@nEmpresasId", id);
            var result = await this.LoadData<int>(query, param);
            return result.FirstOrDefault() > 0;
        }

        public async Task<IEnumerable<Empresa>> GetAllEmpresasAsync()
        {
            string query = "USP_Empresa_GetAll";
            var param = new DynamicParameters();
            return await this.LoadData<Empresa>(query, param);
        }

        public async Task<Empresa?> GetEmpresaByIdAsync(int id)
        {
            string query = "USP_Empresa_GetById";
            var param = new DynamicParameters();
            param.Add("@nEmpresasId", id);
            var result = await this.LoadData<Empresa>(query, param);
            return result.FirstOrDefault();
        }

        public async Task<EmpresaDto> UpdateEmpresaAsync(int id, UpdateEmpresaDto updateDto)
        {
            string query = "USP_Empresa_Update";
            var param = new DynamicParameters();
            param.Add("@nEmpresasId", id);
            param.Add("@cEmpresasNombre", updateDto.cEmpresasNombre);
            param.Add("@cEmpresasCodigo", updateDto.cEmpresasCodigo);
            param.Add("@cEmpresasDominio", updateDto.cEmpresasDominio);
            param.Add("@cEmpresasDescripcion", updateDto.cEmpresasDescripcion);
            param.Add("@bEmpresasActiva", updateDto.bEmpresasActiva);
            
            var result = await this.LoadData<EmpresaDto>(query, param);
            return result.FirstOrDefault() ?? throw new InvalidOperationException("Error al actualizar la empresa");
        }

        public async Task<bool> DeleteEmpresaAsync(int id)
        {
            return await this.DeleteEntero(id);
        }

        public async Task<IEnumerable<EmpresaDto>> GetEmpresasActivasAsync()
        {
            var filter = new EmpresaFilter();
            var empresas = await this.GetByActivas(filter);
            return empresas.Select(e => new EmpresaDto
            {
                nEmpresasId = e.nEmpresasId,
                nEmpresasAplicacionId = e.nEmpresasAplicacionId,
                cEmpresasNombre = e.cEmpresasNombre,
                cEmpresasCodigo = e.cEmpresasCodigo,
                bEmpresasActiva = e.bEmpresasEsActiva,
                dEmpresasFechaCreacion = e.dEmpresasFechaCreacion
            });
        }

        public async Task<IEnumerable<EmpresaDto>> SearchEmpresasAsync(string searchTerm)
        {
            var filter = new EmpresaFilter { TerminoBusqueda = searchTerm };
            var empresas = await this.GetByTerminoBusqueda(filter);
            return empresas.Select(e => new EmpresaDto
            {
                nEmpresasId = e.nEmpresasId,
                nEmpresasAplicacionId = e.nEmpresasAplicacionId,
                cEmpresasNombre = e.cEmpresasNombre,
                cEmpresasCodigo = e.cEmpresasCodigo,
                bEmpresasActiva = e.bEmpresasEsActiva,
                dEmpresasFechaCreacion = e.dEmpresasFechaCreacion
            });
        }

        public async Task<bool> ValidateEmpresaCodeAsync(string empresaCode)
        {
            if (string.IsNullOrWhiteSpace(empresaCode)) return false;
            // Formato: mayúsculas, números, guion y guion bajo
            var isFormatValid = Regex.IsMatch(empresaCode, "^[A-Z0-9_-]+$");
            if (!isFormatValid) return false;
            // El código es válido si NO existe actualmente (para creación)
            var exists = await ExistsEmpresaByCodigoAsync(empresaCode);
            return !exists;
        }

        public async Task<List<object>?> GetEmpresasByApplicationAsync(string applicationId)
        {
            if (string.IsNullOrWhiteSpace(applicationId)) return new List<object>();
            if (!int.TryParse(applicationId, out var appId)) return new List<object>();

            var all = await GetAllEmpresasAsync();
            var filtered = all.Where(e => e.nEmpresasAplicacionId == appId)
                              .Select(e => (object)new { e.nEmpresasId, e.cEmpresasNombre, e.cEmpresasCodigo, e.nEmpresasAplicacionId })
                              .ToList();
            return filtered;
        }

        #endregion
    }
}