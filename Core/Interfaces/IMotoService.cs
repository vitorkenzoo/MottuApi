using MottuApi.DTOs.MotoDtos;
using MottuApi.DTOs.Shared;

namespace MottuApi.Core.Interfaces
{
    /// <summary>
    /// Define o contrato para o serviço de Motos.
    /// Esta interface contém a lógica de negócio e orquestra as operações
    /// relacionadas a motos, utilizando o repositório correspondente.
    /// </summary>
    public interface IMotoService
    {
        /// <summary>
        /// Busca e retorna uma moto pelo seu ID.
        /// </summary>
        /// <param name="id">ID da moto.</param>
        /// <returns>Um DTO com os dados da moto, ou nulo se não encontrada.</returns>
        Task<ReadMotoDto?> GetByIdAsync(int id);

        /// <summary>
        /// Busca e retorna uma lista paginada de todas as motos.
        /// </summary>
        /// <param name="pageNumber">Número da página.</param>
        /// <param name="pageSize">Tamanho da página.</param>
        /// <returns>Um resultado paginado contendo os DTOs das motos.</returns>
        Task<PagedResult<ReadMotoDto>> GetAllAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Valida e cria uma nova moto no sistema.
        /// </summary>
        /// <param name="createMotoDto">DTO com os dados para a criação da moto.</param>
        /// <returns>Um DTO da moto recém-criada.</returns>
        Task<ReadMotoDto> CreateAsync(CreateMotoDto createMotoDto);

        /// <summary>
        /// Valida e atualiza uma moto existente.
        /// </summary>
        /// <param name="id">ID da moto a ser atualizada.</param>
        /// <param name="updateMotoDto">DTO com os dados a serem atualizados.</param>
        Task UpdateAsync(int id, UpdateMotoDto updateMotoDto);

        /// <summary>
        /// Valida e remove uma moto do sistema.
        /// </summary>
        /// <param name="id">ID da moto a ser removida.</param>
        Task DeleteAsync(int id);
    }
}