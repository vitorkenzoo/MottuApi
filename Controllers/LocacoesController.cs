using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MottuApi.Core.Interfaces;
using MottuApi.DTOs.LocacaoDtos;
using MottuApi.DTOs.Shared;
using System.ComponentModel.DataAnnotations;

namespace MottuApi.Controllers
{
    /// <summary>
    /// DTO auxiliar para receber a data de devolução.
    /// </summary>
    public class FinalizarLocacaoDto
    {
        [Required]
        public DateTime DataDevolucao { get; set; }
    }

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LocacoesController : ControllerBase
    {
        private readonly ILocacaoService _locacaoService;

        public LocacoesController(ILocacaoService locacaoService)
        {
            _locacaoService = locacaoService;
        }

        /// <summary>
        /// Retorna uma lista paginada de locações.
        /// </summary>
        /// <response code="200">Retorna a lista paginada de locações.</response>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<ReadLocacaoDto>), 200)]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _locacaoService.GetAllAsync(pageNumber, pageSize);
            result.Items.ForEach(AddHateoasLinks);
            return Ok(result);
        }

        /// <summary>
        /// Busca uma locação específica pelo seu ID.
        /// </summary>
        /// <param name="id">O ID da locação.</param>
        /// <response code="200">Retorna os dados da locação encontrada.</response>
        /// <response code="404">Se a locação não for encontrada.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReadLocacaoDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var locacaoDto = await _locacaoService.GetByIdAsync(id);
            if (locacaoDto == null) return NotFound();

            AddHateoasLinks(locacaoDto);
            return Ok(locacaoDto);
        }

        /// <summary>
        /// Inicia uma nova locação de moto para um cliente.
        /// </summary>
        /// <param name="createDto">Dados para a locação (ID do cliente e data final prevista).</param>
        /// <response code="201">Retorna os dados da locação recém-criada.</response>
        /// <response code="400">Se a locação for inválida (ex: cliente inelegível, sem motos disponíveis).</response>
        /// <response code="404">Se o cliente especificado não for encontrado.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ReadLocacaoDto), 201)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Create([FromBody] CreateLocacaoDto createDto)
        {
            try
            {
                var createdLocacao = await _locacaoService.CreateAsync(createDto);
                AddHateoasLinks(createdLocacao);
                return CreatedAtAction(nameof(GetById), new { id = createdLocacao.Id }, createdLocacao);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Finaliza uma locação ativa, calculando o custo total.
        /// </summary>
        /// <param name="id">O ID da locação a ser finalizada.</param>
        /// <param name="finalizarDto">Objeto contendo a data de devolução.</param>
        /// <response code="200">Retorna a locação atualizada com o valor total.</response>
        /// <response code="400">Se a finalização for inválida (ex: locação já finalizada).</response>
        /// <response code="404">Se a locação não for encontrada.</response>
        [HttpPut("{id}/finalizar")]
        [ProducesResponseType(typeof(ReadLocacaoDto), 200)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Finalizar(int id, [FromBody] FinalizarLocacaoDto finalizarDto)
        {
            try
            {
                var updatedLocacao = await _locacaoService.EndLocacaoAsync(id, finalizarDto.DataDevolucao);
                AddHateoasLinks(updatedLocacao);
                return Ok(updatedLocacao);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Remove uma locação (operação administrativa).
        /// </summary>
        /// <param name="id">O ID da locação a ser removida.</param>
        /// <response code="204">Se a locação foi removida com sucesso.</response>
        /// <response code="400">Se a locação estiver ativa.</response>
        /// <response code="404">Se a locação não for encontrada.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _locacaoService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // --- Métodos Privados ---
        private void AddHateoasLinks(ReadLocacaoDto dto)
        {
            dto.Links.Add(new LinkDto($"/api/v1/locacoes/{dto.Id}", "self", "GET"));

            if (dto.Status == "Ativa")
            {
                dto.Links.Add(new LinkDto($"/api/v1/locacoes/{dto.Id}/finalizar", "end-locacao", "PUT"));
            }
        }
    }
}