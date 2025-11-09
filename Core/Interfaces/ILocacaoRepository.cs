using MottuApi.Core.Entities;

namespace MottuApi.Core.Interfaces
{
    /// <summary>
    /// Define o contrato para o repositório da entidade Locacao.
    /// Abstrai as operações de acesso a dados para as transações de locação.
    /// </summary>
    public interface ILocacaoRepository
    {
        /// <summary>
        /// Busca uma locação de forma assíncrona pelo seu ID.
        /// </summary>
        /// <param name="id">O ID da locação a ser procurada.</param>
        /// <returns>A entidade Locacao correspondente ao ID, ou nulo se não for encontrada.</returns>
        Task<Locacao?> GetByIdAsync(int id);

        /// <summary>
        /// Retorna todas as locações de forma paginada.
        /// </summary>
        /// <param name="pageNumber">O número da página a ser retornada.</param>
        /// <param name="pageSize">A quantidade de itens por página.</param>
        /// <returns>Uma coleção enumerável de locações.</returns>
        Task<IEnumerable<Locacao>> GetAllPaginatedAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Busca uma locação ativa para um cliente específico.
        /// Útil para validar se o cliente já possui uma locação em andamento.
        /// </summary>
        /// <param name="clienteId">O ID do cliente.</param>
        /// <returns>A locação ativa do cliente, ou nulo se não houver.</returns>
        Task<Locacao?> GetActiveLocacaoByClienteIdAsync(int clienteId);

        /// <summary>
        /// Adiciona uma nova locação ao repositório.
        /// </summary>
        /// <param name="locacao">A entidade Locacao a ser adicionada.</param>
        Task AddAsync(Locacao locacao);

        /// <summary>
        /// Marca uma entidade Locacao como modificada.
        /// </summary>
        /// <param name="locacao">A entidade Locacao a ser atualizada.</param>
        void Update(Locacao locacao);

        /// <summary>
        /// Marca uma entidade Locacao para remoção.
        /// </summary>
        /// <param name="locacao">A entidade Locacao a ser removida.</param>
        void Delete(Locacao locacao);

        Task<int> GetCountAsync();

    }
}