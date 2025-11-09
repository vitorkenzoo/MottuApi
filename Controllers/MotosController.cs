using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MottuApi.Core.Interfaces;
using MottuApi.DTOs.MotoDtos;
using MottuApi.DTOs.Shared;

namespace MottuApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MotosController : ControllerBase
    {
        private readonly IMotoService _motoService;

        public MotosController(IMotoService motoService)
        {
            _motoService = motoService;
        }

        /// <summary>
        /// Retorna uma lista paginada de motos.
        /// </summary>
        /// <param name="pageNumber">O número da página (padrão: 1).</param>
        /// <param name="pageSize">O tamanho da página (padrão: 10).</param>
        /// <response code="200">Retorna a lista paginada de motos.</response>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<ReadMotoDto>), 200)]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _motoService.GetAllAsync(pageNumber, pageSize);

            // Adiciona links HATEOAS para cada item
            result.Items.ForEach(AddHateoasLinks);

            return Ok(result);
        }

        /// <summary>
        /// Busca uma moto específica pelo seu ID.
        /// </summary>
        /// <param name="id">O ID da moto.</param>
        /// <response code="200">Retorna os dados da moto encontrada.</response>
        /// <response code="404">Se a moto com o ID especificado não for encontrada.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReadMotoDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var motoDto = await _motoService.GetByIdAsync(id);
            if (motoDto == null)
            {
                return NotFound();
            }

            AddHateoasLinks(motoDto);
            return Ok(motoDto);
        }

        /// <summary>
        /// Cadastra uma nova moto.
        /// </summary>
        /// <param name="createDto">Os dados da moto a ser criada.</param>
        /// <response code="201">Retorna os dados da moto recém-criada.</response>
        /// <response code="400">Se os dados fornecidos forem inválidos (ex: placa já existe).</response>
        [HttpPost]
        [ProducesResponseType(typeof(ReadMotoDto), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] CreateMotoDto createDto)
        {
            try
            {
                var createdMoto = await _motoService.CreateAsync(createDto);
                AddHateoasLinks(createdMoto);
                // Retorna 201 Created com o local do novo recurso e o próprio recurso.
                return CreatedAtAction(nameof(GetById), new { id = createdMoto.Id }, createdMoto);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza os dados de uma moto existente.
        /// </summary>
        /// <param name="id">O ID da moto a ser atualizada.</param>
        /// <param name="updateDto">Os novos dados da moto.</param>
        /// <response code="204">Se a moto foi atualizada com sucesso.</response>
        /// <response code="400">Se a atualização for inválida (ex: tentar alterar moto alugada).</response>
        /// <response code="404">Se a moto não for encontrada.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMotoDto updateDto)
        {
            try
            {
                await _motoService.UpdateAsync(id, updateDto);
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
        /// Remove uma moto.
        /// </summary>
        /// <param name="id">O ID da moto a ser removida.</param>
        /// <response code="204">Se a moto foi removida com sucesso.</response>
        /// <response code="400">Se a remoção for inválida (ex: tentar remover moto alugada).</response>
        /// <response code="404">Se a moto não for encontrada.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _motoService.DeleteAsync(id);
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
        private void AddHateoasLinks(ReadMotoDto dto)
        {
            dto.Links.Add(new LinkDto($"/api/v1/motos/{dto.Id}", "self", "GET"));
            dto.Links.Add(new LinkDto($"/api/v1/motos/{dto.Id}", "update-moto", "PUT"));
            dto.Links.Add(new LinkDto($"/api/v1/motos/{dto.Id}", "delete-moto", "DELETE"));
        }
    }
}