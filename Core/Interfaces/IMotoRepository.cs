using MottuApi.Core.Entities;

namespace MottuApi.Core.Interfaces
{
    /// <summary>
    /// Define o contrato para o repositório da entidade Moto.
    /// Esta interface abstrai as operações de acesso a dados para as motos,
    /// permitindo que a lógica de negócio não dependa diretamente do Entity Framework ou de qualquer outra tecnologia de banco de dados.
    /// </summary>
    public interface IMotoRepository
    {
        /// <summary>
        /// Busca uma moto de forma assíncrona pelo seu ID.
        /// </summary>
        /// <param name="id">O ID da moto a ser procurada.</param>
        /// <returns>A entidade Moto correspondente ao ID, ou nulo se não for encontrada.</returns>
        Task<Moto?> GetByIdAsync(int id);

        /// <summary>
        /// Retorna todas as motos de forma paginada.
        /// </summary>
        /// <param name="pageNumber">O número da página a ser retornada.</param>
        /// <param name="pageSize">A quantidade de itens por página.</param>
        /// <returns>Uma coleção enumerável de motos.</returns>
        Task<IEnumerable<Moto>> GetAllPaginatedAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Busca uma moto pela sua placa, que deve ser única.
        /// </summary>
        /// <param name="placa">A placa da moto a ser procurada.</param>
        /// <returns>A entidade Moto correspondente à placa, ou nulo se não for encontrada.</returns>
        Task<Moto?> GetByPlacaAsync(string placa);

        /// <summary>
        /// Adiciona uma nova moto ao repositório.
        /// </summary>
        /// <param name="moto">A entidade Moto a ser adicionada.</param>
        Task AddAsync(Moto moto);

        /// <summary>
        /// Marca uma entidade Moto como modificada.
        /// A alteração será persistida no banco de dados quando SaveChangesAsync for chamado.
        /// </summary>
        /// <param name="moto">A entidade Moto a ser atualizada.</param>
        void Update(Moto moto);

        /// <summary>
        /// Marca uma entidade Moto para remoção.
        /// A remoção será efetivada no banco de dados quando SaveChangesAsync for chamado.
        /// </summary>
        /// <param name="moto">A entidade Moto a ser removida.</param>
        void Delete(Moto moto);

        /// <summary>
        /// Retorna a contagem total de motos.
        /// </summary>
        Task<int> GetCountAsync();

        Task<Moto?> FindFirstAvailableAsync();

    }
}