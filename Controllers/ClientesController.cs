using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MottuApi.Core.Interfaces;
using MottuApi.DTOs.ClienteDtos;
using MottuApi.DTOs.Shared;

namespace MottuApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        private readonly IPredictionService _predictionService;

        public ClientesController(IClienteService clienteService, IPredictionService predictionService)
        {
            _clienteService = clienteService;
            _predictionService = predictionService;
        }

        /// <summary>
        /// Retorna uma lista paginada de clientes.
        /// </summary>
        /// <param name="pageNumber">O número da página (padrão: 1).</param>
        /// <param name="pageSize">O tamanho da página (padrão: 10).</param>
        /// <response code="200">Retorna a lista paginada de clientes.</response>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<ReadClienteDto>), 200)]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _clienteService.GetAllAsync(pageNumber, pageSize);
            result.Items.ForEach(AddHateoasLinks);
            return Ok(result);
        }

        /// <summary>
        /// Busca um cliente específico pelo seu ID.
        /// </summary>
        /// <param name="id">O ID do cliente.</param>
        /// <response code="200">Retorna os dados do cliente encontrado.</response>
        /// <response code="404">Se o cliente com o ID especificado não for encontrado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReadClienteDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var clienteDto = await _clienteService.GetByIdAsync(id);
            if (clienteDto == null) return NotFound();

            AddHateoasLinks(clienteDto);
            return Ok(clienteDto);
        }

        /// <summary>
        /// Cadastra um novo cliente.
        /// </summary>
        /// <param name="createDto">Os dados do cliente a ser criado.</param>
        /// <response code="201">Retorna os dados do cliente recém-criado.</response>
        /// <response code="400">Se os dados fornecidos forem inválidos (ex: CPF/CNH duplicado, menor de idade).</response>
        [HttpPost]
        [ProducesResponseType(typeof(ReadClienteDto), 201)]
        [ProducesResponseType(typeof(object), 400)]
        public async Task<IActionResult> Create([FromBody] CreateClienteDto createDto)
        {
            try
            {
                var createdCliente = await _clienteService.CreateAsync(createDto);
                AddHateoasLinks(createdCliente);
                return CreatedAtAction(nameof(GetById), new { id = createdCliente.Id }, createdCliente);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza o nome de um cliente existente.
        /// </summary>
        /// <param name="id">O ID do cliente a ser atualizado.</param>
        /// <param name="updateDto">Os novos dados do cliente.</param>
        /// <response code="204">Se o cliente foi atualizado com sucesso.</response>
        /// <response code="400">Se a atualização for inválida (ex: cliente com locação ativa).</response>
        /// <response code="404">Se o cliente não for encontrado.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateClienteDto updateDto)
        {
            try
            {
                await _clienteService.UpdateAsync(id, updateDto);
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

        /// <summary>
        /// Remove um cliente.
        /// </summary>
        /// <param name="id">O ID do cliente a ser removido.</param>
        /// <response code="204">Se o cliente foi removido com sucesso.</response>
        /// <response code="400">Se a remoção for inválida (ex: cliente com locação ativa).</response>
        /// <response code="404">Se o cliente não for encontrado.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _clienteService.DeleteAsync(id);
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

        /// <summary>
        /// Estima o risco de um cliente baseado na idade e tipo de CNH usando Machine Learning.
        /// </summary>
        /// <param name="dto">Dados do cliente para estimativa (idade e tipo de CNH).</param>
        /// <response code="200">Retorna a estimativa de risco (Alto ou Baixo).</response>
        /// <response code="400">Se os dados fornecidos forem inválidos.</response>
        [HttpPost("estimar-risco")]
        [ProducesResponseType(typeof(RiscoEstimadoDto), 200)]
        [ProducesResponseType(typeof(object), 400)]
        public IActionResult EstimarRisco([FromBody] EstimarRiscoDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var risco = _predictionService.EstimarRisco(dto.Idade, dto.TipoCNH);

            var resultado = new RiscoEstimadoDto
            {
                Risco = risco,
                Idade = dto.Idade,
                TipoCNH = dto.TipoCNH
            };

            return Ok(resultado);
        }

        // --- Métodos Privados ---
        private void AddHateoasLinks(ReadClienteDto dto)
        {
            dto.Links.Add(new LinkDto($"/api/v1/clientes/{dto.Id}", "self", "GET"));
            dto.Links.Add(new LinkDto($"/api/v1/clientes/{dto.Id}", "update-cliente", "PUT"));
            dto.Links.Add(new LinkDto($"/api/v1/clientes/{dto.Id}", "delete-cliente", "DELETE"));
        }
    }
}