using MottuApi.DTOs.LocacaoDtos;
using MottuApi.DTOs.Shared;

namespace MottuApi.Core.Interfaces
{
    /// <summary>
    /// Define o contrato para o serviço de Locações.
    /// Contém a lógica de negócio para o aluguel e devolução de motos,
    /// orquestrando os repositórios de Locacao, Moto e Cliente.
    /// </summary>
    public interface ILocacaoService
    {
        /// <summary>
        /// Busca e retorna uma locação pelo seu ID.
        /// </summary>
        /// <param name="id">ID da locação.</param>
        /// <returns>Um DTO com os dados da locação, ou nulo se não encontrada.</returns>
        Task<ReadLocacaoDto?> GetByIdAsync(int id);

        /// <summary>
        /// Busca e retorna uma lista paginada de todas as locações.
        /// </summary>
        /// <param name="pageNumber">Número da página.</param>
        /// <param name="pageSize">Tamanho da página.</param>
        /// <returns>Um resultado paginado contendo os DTOs das locações.</returns>
        Task<PagedResult<ReadLocacaoDto>> GetAllAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Cria uma nova locação para um cliente.
        /// A lógica inclui encontrar uma moto disponível, validar se o cliente pode alugar e registrar a transação.
        /// </summary>
        /// <param name="createLocacaoDto">DTO com os dados para a criação da locação.</param>
        /// <returns>Um DTO da locação recém-criada.</returns>
        Task<ReadLocacaoDto> CreateAsync(CreateLocacaoDto createLocacaoDto);

        /// <summary>
        /// Finaliza uma locação ativa.
        /// A lógica inclui o cálculo do valor total com base na data de devolução e na diária contratada.
        /// </summary>
        /// <param name="locacaoId">O ID da locação a ser finalizada.</param>
        /// <param name="dataDevolucao">A data em que a moto foi devolvida.</param>
        /// <returns>Um DTO da locação atualizada com o valor total calculado.</returns>
        Task<ReadLocacaoDto> EndLocacaoAsync(int locacaoId, DateTime dataDevolucao);

        /// <summary>
        /// Remove uma locação do sistema (operação administrativa).
        /// </summary>
        /// <param name="id">ID da locação a ser removida.</param>
        Task DeleteAsync(int id);
    }
}