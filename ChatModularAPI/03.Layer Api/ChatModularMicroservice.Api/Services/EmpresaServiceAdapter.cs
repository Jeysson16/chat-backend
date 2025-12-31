using ChatModularMicroservice.Domain;
using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Repository;

namespace ChatModularMicroservice.Api.Services;

/// <summary>
/// Adaptador de servicio para la gestión de empresas
/// </summary>
public class EmpresaServiceAdapter : IEmpresaService
{
    private readonly IEmpresaRepository _empresaRepository;
    private readonly IConfiguracionEmpresaService _configEmpresaService;

    public EmpresaServiceAdapter(IEmpresaRepository empresaRepository, IConfiguracionEmpresaService configEmpresaService)
    {
        _empresaRepository = empresaRepository;
        _configEmpresaService = configEmpresaService;
    }

    public async Task<List<EmpresaDto>> GetAllEmpresasAsync()
    {
        var empresas = await _empresaRepository.GetAllEmpresasAsync();
        return empresas.Select(ToDto).ToList();
    }

    public async Task<EmpresaDto?> GetEmpresaByIdAsync(int id)
    {
        var empresa = await _empresaRepository.GetEmpresaByIdAsync(id);
        return empresa == null ? null : ToDto(empresa);
    }

    public async Task<EmpresaDto?> GetEmpresaByCodigoAsync(string codigo)
    {
        var empresa = await _empresaRepository.GetEmpresaByCodigoAsync(codigo);
        return empresa == null ? null : ToDto(empresa);
    }

    public async Task<List<EmpresaDto>> GetEmpresasActivasAsync()
    {
        var empresas = await _empresaRepository.GetEmpresasActivasAsync();
        return empresas.ToList();
    }

    public async Task<List<EmpresaDto>> SearchEmpresasAsync(string terminoBusqueda)
    {
        var empresas = await _empresaRepository.SearchEmpresasAsync(terminoBusqueda);
        return empresas.ToList();
    }

    public async Task<EmpresaDto> CreateEmpresaAsync(CreateEmpresaDto createEmpresaDto)
    {
        var empresaDto = await _empresaRepository.CreateEmpresaAsync(createEmpresaDto);
        if (empresaDto != null && empresaDto.nEmpresasId > 0 && createEmpresaDto.nEmpresasAplicacionId > 0)
        {
            await _configEmpresaService.CopiarConfiguracionesDeAplicacionAsync(empresaDto.nEmpresasId, createEmpresaDto.nEmpresasAplicacionId);
        }
        return empresaDto;
    }

    public async Task<EmpresaDto> UpdateEmpresaAsync(int id, UpdateEmpresaDto updateEmpresaDto)
    {
        var empresaDto = await _empresaRepository.UpdateEmpresaAsync(id, updateEmpresaDto);
        return empresaDto;
    }

    public async Task<bool> DeleteEmpresaAsync(int id)
    {
        return await _empresaRepository.DeleteEmpresaAsync(id);
    }

    public async Task<bool> ExistsEmpresaByCodigoAsync(string codigo, int? excludeId = null)
    {
        return await _empresaRepository.ExistsEmpresaByCodigoAsync(codigo, excludeId);
    }

    public async Task<List<EmpresaDto>> GetEmpresasByAplicacionAsync(int aplicacionId)
    {
        var raw = await _empresaRepository.GetEmpresasByApplicationAsync(aplicacionId.ToString()) ?? new List<object>();
        var list = new List<EmpresaDto>();
        foreach (var o in raw)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(o);
            var anon = System.Text.Json.JsonSerializer.Deserialize<AnonEmpresa>(json);
            if (anon != null)
            {
                list.Add(new EmpresaDto
                {
                    nEmpresasId = anon.nEmpresasId,
                    nEmpresasAplicacionId = anon.nEmpresasAplicacionId,
                    cEmpresasNombre = anon.cEmpresasNombre,
                    cEmpresasCodigo = anon.cEmpresasCodigo,
                    bEmpresasActiva = true
                });
            }
        }
        return list;
    }

    private class AnonEmpresa
    {
        public int nEmpresasId { get; set; }
        public string cEmpresasNombre { get; set; } = string.Empty;
        public string cEmpresasCodigo { get; set; } = string.Empty;
        public int nEmpresasAplicacionId { get; set; }
    }

    private EmpresaDto ToDto(ChatModularMicroservice.Entities.Models.Empresa empresa)
    {
        return new EmpresaDto
        {
            nEmpresasId = empresa.nEmpresasId,
            nEmpresasAplicacionId = empresa.nEmpresasAplicacionId,
            cEmpresasNombre = empresa.cEmpresasNombre,
            cEmpresasCodigo = empresa.cEmpresasCodigo,
            cEmpresasDominio = string.Empty, // Campo no existe en el modelo
            cEmpresasDescripcion = string.Empty, // Campo no existe en el modelo
            bEmpresasActiva = empresa.bEmpresasEsActiva,
            dEmpresasFechaCreacion = empresa.dEmpresasFechaCreacion
        };
    }
}
