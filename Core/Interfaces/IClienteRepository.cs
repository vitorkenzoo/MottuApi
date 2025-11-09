using MottuApi.Core.Entities;

namespace MottuApi.Core.Interfaces
{
    /// <summary>
    /// Define o contrato para o repositório da entidade Cliente.
    /// Abstrai as operações de acesso a dados para os clientes.
    /// </summary>
    public interface IClienteRepository
    {
        /// <summary>
        /// Busca um cliente de forma assíncrona pelo seu ID.
        /// </summary>
        /// <param name="id">O ID do cliente a ser procurado.</param>
        /// <returns>A entidade Cliente correspondente ao ID, ou nulo se não for encontrada.</returns>
        Task<Cliente?> GetByIdAsync(int id);

        /// <summary>
        /// Retorna todos os clientes de forma paginada.
        /// </summary>
        /// <param name="pageNumber">O número da página a ser retornada.</param>
        /// <param name="pageSize">A quantidade de itens por página.</param>
        /// <returns>Uma coleção enumerável de clientes.</returns>
        Task<IEnumerable<Cliente>> GetAllPaginatedAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Busca um cliente pelo seu CPF, que deve ser único.
        /// </summary>
        /// <param name="cpf">O CPF do cliente a ser procurado.</param>
        /// <returns>A entidade Cliente correspondente ao CPF, ou nulo se não for encontrada.</returns>
        Task<Cliente?> GetByCpfAsync(string cpf);

        /// <summary>
        /// Busca um cliente pelo número da sua CNH, que deve ser único.
        /// </summary>
        /// <param name="cnh">O número da CNH do cliente a ser procurado.</param>
        /// <returns>A entidade Cliente correspondente à CNH, ou nulo se não for encontrada.</returns>
        Task<Cliente?> GetByCnhAsync(string cnh);

        /// <summary>
        /// Adiciona um novo cliente ao repositório.
        /// </summary>
        /// <param name="cliente">A entidade Cliente a ser adicionada.</param>
        Task AddAsync(Cliente cliente);

        /// <summary>
        /// Marca uma entidade Cliente como modificada.
        /// </summary>
        /// <param name="cliente">A entidade Cliente a ser atualizada.</param>
        void Update(Cliente cliente);

        /// <summary>
        /// Marca uma entidade Cliente para remoção.
        /// </summary>
        /// <param name="cliente">A entidade Cliente a ser removida.</param>
        void Delete(Cliente cliente);

        /// <summary>
        /// Retorna a contagem total de clientes.
        /// </summary>
        Task<int> GetCountAsync();

    }
}