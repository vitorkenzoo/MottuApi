using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MottuApi.Core.Entities
{
    /// <summary>
    /// Enum para representar o status da locação.
    /// </summary>
    public enum StatusLocacao
    {
        Ativa,
        Concluida
    }

    /// <summary>
    /// Representa a transação de locação de uma moto por um cliente.
    /// É a entidade central que conecta as demais.
    /// </summary>
    public class Locacao
    {
        /// <summary>
        /// Identificador único da locação.
        /// </summary>
        /// <example>5001</example>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Data e hora de início da locação.
        /// </summary>
        [Required]
        public DateTime DataInicio { get; set; }

        /// <summary>
        /// Data e hora prevista para o término da locação.
        /// </summary>
        [Required]
        public DateTime DataFimPrevista { get; set; }

        /// <summary>
        /// Data e hora em que a moto foi efetivamente devolvida.
        /// Permite nulo, pois uma locação ativa ainda não tem data de devolução.
        /// </summary>
        public DateTime? DataFimReal { get; set; }

        /// <summary>
        /// Valor da diária da locação no momento da contratação.
        /// </summary>
        /// <example>50.00</example>
        [Required]
        public decimal ValorDiaria { get; set; }

        /// <summary>
        /// Valor total pago. Será calculado no momento da devolução.
        /// </summary>
        /// <example>350.00</example>
        public decimal? ValorTotal { get; set; }

        /// <summary>
        /// Status atual da locação (Ativa ou Concluída).
        /// </summary>
        public StatusLocacao Status { get; set; }

        // --- Chaves Estrangeiras e Propriedades de Navegação ---

        /// <summary>
        /// ID do cliente que realizou a locação (Chave Estrangeira).
        /// </summary>
        /// <example>1</example>
        [Required]
        public int ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public required Cliente Cliente { get; set; }

        /// <summary>
        /// ID da moto que foi alugada (Chave Estrangeira).
        /// </summary>
        /// <example>101</example>
        [Required]
        public int MotoId { get; set; }

        [ForeignKey("MotoId")]
        public required Moto Moto { get; set; }
    }
}