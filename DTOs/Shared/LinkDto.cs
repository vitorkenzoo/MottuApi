namespace MottuApi.DTOs.Shared
{
    /// <summary>
    /// Representa um link HATEOAS (Hypermedia as the Engine of Application State).
    /// Usado para tornar a API auto-descritiva, informando ao cliente quais ações
    /// podem ser realizadas a partir do recurso atual.
    /// </summary>
    public class LinkDto
    {
        /// <summary>
        /// O URL do link (o endpoint da ação).
        /// </summary>
        /// <example>/api/motos/101</example>
        public string Href { get; private set; }

        /// <summary>
        /// A relação (relationship) do link com o recurso atual. Descreve o que o link faz.
        /// </summary>
        /// <example>self</example>
        public string Rel { get; private set; }

        /// <summary>
        /// O método HTTP a ser usado para acessar o link.
        /// </summary>
        /// <example>GET</example>
        public string Method { get; private set; }

        /// <summary>
        /// Construtor para o Link HATEOAS.
        /// </summary>
        /// <param name="href">O URL do link.</param>
        /// <param name="rel">A relação do link (ex: "self", "update-moto").</param>
        /// <param name="method">O método HTTP (ex: "GET", "PUT").</param>
        public LinkDto(string href, string rel, string method)
        {
            Href = href;
            Rel = rel;
            Method = method;
        }
    }
}