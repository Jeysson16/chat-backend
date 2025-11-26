using ChatModularMicroservice.Entities.Models;
using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Entities;
using ChatModularMicroservice.Repository;
using ChatModularMicroservice.Shared.Configs;
using Supabase.Postgrest;
using static Supabase.Postgrest.Constants;
using Microsoft.Extensions.Logging;
using Utils = ChatModularMicroservice.Shared.Utils;

namespace ChatModularMicroservice.Infrastructure.Repositories
{
    public class EmpresaSupabaseRepository : SupabaseRepository, IEmpresaRepository
    {
        public EmpresaSupabaseRepository(Supabase.Client supabaseClient, ILogger<SupabaseRepository> logger, SupabaseConfig config)
            : base(supabaseClient, logger, config) { }

        public async Task<int> Insert(Empresa item)
        {
            await _supabaseClient.From<Empresa>().Insert(item);
            var fetch = await _supabaseClient.From<Empresa>()
                .Filter("cEmpresasCodigo", Operator.Equals, item.cEmpresasCodigo)
                .Limit(1)
                .Get();
            var created = fetch.Models?.FirstOrDefault();
            if (created == null || created.nEmpresasId <= 0) throw new InvalidOperationException("No se pudo insertar la empresa");
            return created.nEmpresasId;
        }

        public async Task<bool> Update(Empresa item)
        {
            await _supabaseClient.From<Empresa>().Update(item);
            return true;
        }

        public async Task<bool> DeleteEntero(int id)
        {
            await _supabaseClient
                .From<Empresa>()
                .Filter("nEmpresasId", Operator.Equals, id)
                .Delete();
            return true;
        }

        public async Task<Empresa> GetItem(EmpresaFilter filter, EmpresaFilterItemType filterType)
        {
            switch (filterType)
            {
                case EmpresaFilterItemType.ById:
                {
                    var res = await _supabaseClient.From<Empresa>()
                        .Filter("nEmpresasId", Operator.Equals, filter.nEmpresasId)
                        .Limit(1)
                        .Get();
                    return res.Models?.FirstOrDefault();
                }
                case EmpresaFilterItemType.ByCodigo:
                {
                    var res = await _supabaseClient.From<Empresa>()
                        .Filter("cEmpresasCodigo", Operator.Equals, filter.cEmpresasCodigo)
                        .Limit(1)
                        .Get();
                    return res.Models?.FirstOrDefault();
                }
                case EmpresaFilterItemType.ByNombre:
                {
                    var res = await _supabaseClient.From<Empresa>()
                        .Filter("cEmpresasNombre", Operator.Equals, filter.cEmpresasNombre)
                        .Limit(1)
                        .Get();
                    return res.Models?.FirstOrDefault();
                }
                default:
                {
                    var res = await _supabaseClient.From<Empresa>()
                        .Limit(1)
                        .Get();
                    return res.Models?.FirstOrDefault();
                }
            }
        }

        public async Task<IEnumerable<Empresa>> GetLstItem(EmpresaFilter filter, EmpresaFilterListType filterType, Utils.Pagination pagination)
        {
            switch (filterType)
            {
                case EmpresaFilterListType.ByPagination:
                {
                    var res = await _supabaseClient.From<Empresa>()
                        .Order("cEmpresasNombre", Ordering.Ascending)
                        .Range((pagination.PageNumber - 1) * pagination.PageSize, pagination.PageNumber * pagination.PageSize - 1)
                        .Get();
                    return res.Models ?? new List<Empresa>();
                }
                case EmpresaFilterListType.ByActivas:
                {
                    var res = await _supabaseClient.From<Empresa>()
                        .Filter("bEmpresasEsActiva", Operator.Equals, "true")
                        .Get();
                    return res.Models ?? new List<Empresa>();
                }
                case EmpresaFilterListType.ByTerminoBusqueda:
                {
                    var res = await _supabaseClient.From<Empresa>()
                        .Filter("cEmpresasNombre", Operator.ILike, $"%{filter.TerminoBusqueda}%")
                        .Get();
                    return res.Models ?? new List<Empresa>();
                }
                default:
                {
                    var res = await _supabaseClient.From<Empresa>().Get();
                    return res.Models ?? new List<Empresa>();
                }
            }
        }

        public async Task<bool> ExistsEmpresaByCodigoAsync(string codigo, int? excludeId = null)
        {
            var q = _supabaseClient.From<Empresa>().Filter("cEmpresasCodigo", Operator.Equals, codigo);
            if (excludeId.HasValue)
            {
                q = q.Filter("nEmpresasId", Operator.NotEqual, excludeId.Value);
            }
            var res = await q.Limit(1).Get();
            return res.Models != null && res.Models.Count > 0;
        }

        public async Task<bool> ExistsEmpresaByIdAsync(int id)
        {
            var res = await _supabaseClient.From<Empresa>()
                .Filter("nEmpresasId", Operator.Equals, id)
                .Limit(1)
                .Get();
            return res.Models != null && res.Models.Count > 0;
        }

        public async Task<IEnumerable<Empresa>> GetAllEmpresasAsync()
        {
            var res = await _supabaseClient.From<Empresa>()
                .Order("cEmpresasNombre", Ordering.Ascending)
                .Get();
            return res.Models ?? new List<Empresa>();
        }

        public async Task<Empresa?> GetEmpresaByIdAsync(int id)
        {
            var res = await _supabaseClient.From<Empresa>()
                .Filter("nEmpresasId", Operator.Equals, id)
                .Limit(1)
                .Get();
            return res.Models?.FirstOrDefault();
        }

        public async Task<EmpresaDto> UpdateEmpresaAsync(int id, UpdateEmpresaDto updateDto)
        {
            var existing = await GetEmpresaByIdAsync(id) ?? throw new KeyNotFoundException("Empresa no encontrada");
            if (!string.IsNullOrWhiteSpace(updateDto.cEmpresasNombre)) existing.cEmpresasNombre = updateDto.cEmpresasNombre!;
            if (!string.IsNullOrWhiteSpace(updateDto.cEmpresasCodigo)) existing.cEmpresasCodigo = updateDto.cEmpresasCodigo!;
            if (updateDto.bEmpresasActiva.HasValue) existing.bEmpresasEsActiva = updateDto.bEmpresasActiva.Value;
            var ok = await Update(existing);
            if (!ok) throw new InvalidOperationException("Error al actualizar la empresa");
            return new EmpresaDto
            {
                nEmpresasId = existing.nEmpresasId,
                nEmpresasAplicacionId = existing.nEmpresasAplicacionId,
                cEmpresasNombre = existing.cEmpresasNombre,
                cEmpresasCodigo = existing.cEmpresasCodigo,
                bEmpresasActiva = existing.bEmpresasEsActiva,
                dEmpresasFechaCreacion = existing.dEmpresasFechaCreacion
            };
        }

        public async Task<bool> DeleteEmpresaAsync(int id)
        {
            return await DeleteEntero(id);
        }

        public async Task<IEnumerable<EmpresaDto>> GetEmpresasActivasAsync()
        {
            var res = await _supabaseClient.From<Empresa>()
                .Filter("bEmpresasEsActiva", Operator.Equals, "true")
                .Order("cEmpresasNombre", Ordering.Ascending)
                .Get();
            var empresas = res.Models ?? new List<Empresa>();
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
            var res = await _supabaseClient.From<Empresa>()
                .Filter("cEmpresasNombre", Operator.ILike, $"%{searchTerm}%")
                .Order("cEmpresasNombre", Ordering.Ascending)
                .Get();
            var empresas = res.Models ?? new List<Empresa>();
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

        public async Task<Empresa?> GetEmpresaByCodigoAsync(string codigo)
        {
            var res = await _supabaseClient.From<Empresa>()
                .Filter("cEmpresasCodigo", Operator.Equals, codigo)
                .Limit(1)
                .Get();
            return res.Models?.FirstOrDefault();
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
            var newId = await Insert(empresa);
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

        public async Task<bool> ValidateEmpresaCodeAsync(string empresaCode)
        {
            if (string.IsNullOrWhiteSpace(empresaCode)) return false;
            var isFormatValid = System.Text.RegularExpressions.Regex.IsMatch(empresaCode, "^[A-Z0-9_-]+$");
            if (!isFormatValid) return false;
            var exists = await ExistsEmpresaByCodigoAsync(empresaCode);
            return !exists;
        }

        public async Task<List<object>?> GetEmpresasByApplicationAsync(string applicationId)
        {
            if (string.IsNullOrWhiteSpace(applicationId)) return new List<object>();
            if (!int.TryParse(applicationId, out var appId)) return new List<object>();
            var res = await _supabaseClient.From<Empresa>()
                .Filter("nEmpresasAplicacionId", Operator.Equals, appId)
                .Get();
            var empresas = res.Models ?? new List<Empresa>();
            return empresas.Select(e => (object)new { e.nEmpresasId, e.cEmpresasNombre, e.cEmpresasCodigo, e.nEmpresasAplicacionId }).ToList();
        }
    }
}
