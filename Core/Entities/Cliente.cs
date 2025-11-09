using System.ComponentModel.DataAnnotations;

namespace MottuApi.Core.Entities
{
    /// <summary>
    /// Enum para os tipos de CNH (Carteira Nacional de Habilitação).
    /// Apenas CNH do tipo 'A' ou 'AB' pode alugar motos.
    /// </summary>
    public enum TipoCNH
    {
        A,
        B,
        AB
    }

    /// <summary>
    /// Representa a entidade Cliente, a pessoa que aluga as motos.
    /// </summary>
    public class Cliente
    {
        /// <summary>
        /// Identificador único do cliente.
        /// </summary>
        /// <example>1</example>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome completo do cliente.
        /// </summary>
        /// <example>João da Silva</example>
        [Required]
        public required string Nome { get; set; }

        /// <summary>
        /// CPF do cliente (Cadastro de Pessoas Físicas), deve ser único.
        /// </summary>
        /// <example>123.456.789-00</example>
        [Required]
        public required string Cpf { get; set; }

        /// <summary>
        /// Data de nascimento do cliente. Usado para verificar a maioridade.
        /// </summary>
        /// <example>1990-05-20</example>
        [Required]
        public DateOnly DataNascimento { get; set; }

        /// <summary>
        /// Número da CNH (Carteira Nacional de Habilitação) do cliente.
        /// </summary>
        /// <example>98765432100</example>
        [Required]
        public required string NumeroCNH { get; set; }

        /// <summary>
        /// Tipo da CNH do cliente (A, B ou AB).
        /// </summary>
        public TipoCNH TipoCNH { get; set; }

        // Propriedade de navegação para EF Core
        public ICollection<Locacao> Locacoes { get; set; } = new List<Locacao>();
    }
}