using MottuApi.DTOs.ClienteDtos;
using MottuApi.DTOs.Shared;

namespace MottuApi.Core.Interfaces
{
    /// <summary>
    /// Define o contrato para o serviço de Clientes.
    /// Contém a lógica de negócio para gerenciar os clientes do sistema.
    /// </summary>
    public interface IClienteService
    {
        /// <summary>
        /// Busca e retorna um cliente pelo seu ID.
        /// </summary>
        /// <param name="id">ID do cliente.</param>
        /// <returns>Um DTO com os dados do cliente, ou nulo se não encontrado.</returns>
        Task<ReadClienteDto?> GetByIdAsync(int id);

        /// <summary>
        /// Busca e retorna uma lista paginada de todos os clientes.
        /// </summary>
        /// <param name="pageNumber">Número da página.</param>
        /// <param name="pageSize">Tamanho da página.</param>
        /// <returns>Um resultado paginado contendo os DTOs dos clientes.</returns>
        Task<PagedResult<ReadClienteDto>> GetAllAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Valida e cria um novo cliente no sistema. Irá verificar se o CPF e CNH já existem.
        /// </summary>
        /// <param name="createClienteDto">DTO com os dados para a criação do cliente.</param>
        /// <returns>Um DTO do cliente recém-criado.</returns>
        Task<ReadClienteDto> CreateAsync(CreateClienteDto createClienteDto);

        /// <summary>
        /// Valida e atualiza os dados de um cliente existente (exceto CNH).
        /// </summary>
        /// <param name="id">ID do cliente a ser atualizado.</param>
        /// <param name="updateClienteDto">DTO com os dados a serem atualizados.</param>
        Task UpdateAsync(int id, UpdateClienteDto updateClienteDto);

        /// <summary>
        /// Valida e remove um cliente do sistema.
        /// </summary>
        /// <param name="id">ID do cliente a ser removido.</param>
        Task DeleteAsync(int id);
    }
}