namespace MottuApi.DTOs.Shared
{
    /// <summary>
    /// Representa um resultado paginado para ser retornado pela API.
    /// É uma classe genérica que pode conter qualquer tipo de DTO.
    /// </summary>
    /// <typeparam name="T">O tipo dos itens na página.</typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// A lista de itens para a página atual.
        /// </summary>
        public List<T> Items { get; }

        /// <summary>
        /// O número da página atual.
        /// </summary>
        /// <example>1</example>
        public int PageNumber { get; }

        /// <summary>
        /// A quantidade de itens por página.
        /// </summary>
        /// <example>10</example>
        public int PageSize { get; }

        /// <summary>
        /// A contagem total de itens em todas as páginas.
        /// </summary>
        /// <example>100</example>
        public int TotalCount { get; }

        /// <summary>
        /// O número total de páginas disponíveis.
        /// </summary>
        /// <example>10</example>
        public int TotalPages { get; }

        /// <summary>
        /// Construtor para inicializar o resultado paginado.
        /// </summary>
        /// <param name="items">A lista de itens da página atual.</param>
        /// <param name="totalCount">A contagem total de itens.</param>
        /// <param name="pageNumber">O número da página atual.</param>
        /// <param name="pageSize">O tamanho da página.</param>
        public PagedResult(List<T> items, int totalCount, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}