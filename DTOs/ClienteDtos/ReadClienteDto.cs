using MottuApi.DTOs.Shared;

namespace MottuApi.DTOs.ClienteDtos
{
    /// <summary>
    /// DTO para a leitura e retorno de dados de um cliente.
    /// Este é o objeto que será retornado nos endpoints GET.
    /// </summary>
    public class ReadClienteDto
    {
        /// <summary>
        /// Identificador único do cliente.
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// Nome completo do cliente.
        /// </summary>
        /// <example>Maria Souza</example>
        public required string Nome { get; set; }

        /// <summary>
        /// CPF do cliente (Cadastro de Pessoas Físicas).
        /// </summary>
        /// <example>123.456.789-00</example>
        public required string Cpf { get; set; }

        /// <summary>
        /// Data de nascimento do cliente.
        /// </summary>
        /// <example>1995-08-15</example>
        public DateOnly DataNascimento { get; set; }

        /// <summary>
        /// Número da CNH (Carteira Nacional de Habilitação) do cliente.
        /// </summary>
        /// <example>98765432100</example>
        public required string NumeroCNH { get; set; }

        /// <summary>
        /// Tipo da CNH do cliente (A, B ou AB).
        /// </summary>
        /// <example>A</example>
        public required string TipoCNH { get; set; }

        /// <summary>
        /// Lista de links HATEOAS relacionados ao cliente.
        /// </summary>
        public List<LinkDto> Links { get; set; } = new();
    }
}