using Microsoft.AspNetCore.Mvc;
using ChatModularMicroservice.Domain;
using ChatModularMicroservice.Entities.DTOs;
using Utils = ChatModularMicroservice.Shared.Utils;
using Microsoft.AspNetCore.Authorization;
using ChatModularMicroservice.Entities;

namespace ChatModularMicroservice.Api.Controllers
{
    /// <summary>
    /// Controlador para la gestión de empresas
    /// </summary>
    [ApiController]
    [Route("api/v1/empresas")]
    [Authorize]
    public class EmpresasController : BaseController
    {
        private readonly IEmpresaService _empresaService;

        public EmpresasController(
            IEmpresaService empresaService,
            ILogger<EmpresasController> logger) : base(logger)
        {
            _empresaService = empresaService;
        }

        /// <summary>
        /// Obtiene todas las empresas
        /// </summary>
        /// <returns>Lista de empresas</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllEmpresas()
        {
            try
            {
                var empresas = await _empresaService.GetAllEmpresasAsync();
                
                return Ok(CreateSuccessResponse(empresas, GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                return HandleException(ex, GetClientName(), GetUserName());
            }
        }

        /// <summary>
        /// Obtiene una empresa por ID
        /// </summary>
        /// <param name="id">ID de la empresa</param>
        /// <returns>Empresa encontrada</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetEmpresaById(int id)
        {
            try
            {
                var empresa = await _empresaService.GetEmpresaByIdAsync(id);
                if (empresa == null)
                {
                    return NotFound(CreateErrorResponse("Empresa no encontrada", GetClientName(), GetUserName()));
                }

                return Ok(CreateSuccessResponse(empresa, GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                return HandleException(ex, GetClientName(), GetUserName());
            }
        }

        /// <summary>
        /// Obtiene una empresa por código
        /// </summary>
        /// <param name="codigo">Código de la empresa</param>
        /// <returns>Empresa encontrada</returns>
        [HttpGet("codigo/{codigo}")]
        public async Task<IActionResult> GetEmpresaByCodigo(string codigo)
        {
            try
            {
                var empresa = await _empresaService.GetEmpresaByCodigoAsync(codigo);
                if (empresa == null)
                {
                    return NotFound(CreateErrorResponse("Empresa no encontrada", GetClientName(), GetUserName()));
                }

                return Ok(CreateSuccessResponse(empresa, GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                return HandleException(ex, GetClientName(), GetUserName());
            }
        }

        /// <summary>
        /// Obtiene empresas activas
        /// </summary>
        /// <returns>Lista de empresas activas</returns>
        [HttpGet("activas")]
        public async Task<IActionResult> GetEmpresasActivas()
        {
            try
            {
                var empresas = await _empresaService.GetEmpresasActivasAsync();
                
                return Ok(CreateSuccessResponse(empresas, GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                return HandleException(ex, GetClientName(), GetUserName());
            }
        }

        /// <summary>
        /// Busca empresas por término de búsqueda
        /// </summary>
        /// <param name="search">Término de búsqueda</param>
        /// <returns>Lista de empresas que coinciden con la búsqueda</returns>
        [HttpGet("buscar")]
        public async Task<IActionResult> SearchEmpresas([FromQuery] string search)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(search))
                {
                    return BadRequest(CreateErrorResponse("El término de búsqueda es requerido", GetClientName(), GetUserName()));
                }

                var empresas = await _empresaService.SearchEmpresasAsync(search);
                
                return Ok(CreateSuccessResponse(empresas, GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                return HandleException(ex, GetClientName(), GetUserName());
            }
        }

        /// <summary>
        /// Crea una nueva empresa
        /// </summary>
        /// <param name="createEmpresaDto">Datos de la empresa a crear</param>
        /// <returns>Empresa creada</returns>
        [HttpPost]
        public async Task<IActionResult> CreateEmpresa([FromBody] CreateEmpresaDto createEmpresaDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(CreateErrorResponse("Datos de entrada inválidos", GetClientName(), GetUserName()));
                }

                var empresa = await _empresaService.CreateEmpresaAsync(createEmpresaDto);

                return CreatedAtAction(nameof(GetEmpresaById), new { id = empresa.nEmpresasId }, 
                    CreateSuccessResponse(empresa, GetClientName(), GetUserName()));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(CreateErrorResponse(ex.Message, GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                return HandleException(ex, GetClientName(), GetUserName());
            }
        }

        /// <summary>
        /// Actualiza una empresa existente
        /// </summary>
        /// <param name="id">ID de la empresa</param>
        /// <param name="updateEmpresaDto">Datos actualizados de la empresa</param>
        /// <returns>Empresa actualizada</returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateEmpresa(int id, [FromBody] UpdateEmpresaDto updateEmpresaDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(CreateErrorResponse("Datos de entrada inválidos", GetClientName(), GetUserName()));
                }

                var empresa = await _empresaService.UpdateEmpresaAsync(id, updateEmpresaDto);

                return Ok(CreateSuccessResponse(empresa, GetClientName(), GetUserName()));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(CreateErrorResponse(ex.Message, GetClientName(), GetUserName()));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(CreateErrorResponse(ex.Message, GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                return HandleException(ex, GetClientName(), GetUserName());
            }
        }

        /// <summary>
        /// Elimina una empresa
        /// </summary>
        /// <param name="id">ID de la empresa</param>
        /// <returns>Resultado de la operación</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEmpresa(int id)
        {
            try
            {
                var result = await _empresaService.DeleteEmpresaAsync(id);
                if (!result)
                {
                    return NotFound(CreateErrorResponse("Empresa no encontrada", GetClientName(), GetUserName()));
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex, GetClientName(), GetUserName());
            }
        }

        /// <summary>
        /// Verifica si existe una empresa con el código especificado
        /// </summary>
        /// <param name="codigo">Código a verificar</param>
        /// <param name="excludeId">ID a excluir de la verificación (opcional)</param>
        /// <returns>True si existe</returns>
        [HttpGet("existe-codigo/{codigo}")]
        public async Task<IActionResult> ExistsEmpresaByCodigo(string codigo, [FromQuery] int? excludeId = null)
        {
            try
            {
                var exists = await _empresaService.ExistsEmpresaByCodigoAsync(codigo, excludeId);
                
                return Ok(CreateSuccessResponse(new { exists }, GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                return HandleException(ex, GetClientName(), GetUserName());
            }
        }
    }
}