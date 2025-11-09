using System.ComponentModel.DataAnnotations;

namespace MottuApi.Core.Entities
{
    /// <summary>
    /// Enum para representar o status de uma moto de forma segura.
    /// </summary>
    public enum MotoStatus
    {
        Disponivel,
        Alugada,
        EmManutencao
    }

    /// <summary>
    /// Representa a entidade Moto, que é o principal ativo da empresa.
    /// </summary>
    public class Moto
    {
        /// <summary>
        /// Identificador único da moto no banco de dados.
        /// </summary>
        /// <example>101</example>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Ano de fabricação da moto.
        /// </summary>
        /// <example>2024</example>
        [Required]
        public int Ano { get; set; }

        /// <summary>
        /// Modelo da moto.
        /// </summary>
        /// <example>Honda Pop 110i</example>
        [Required]
        public required string Modelo { get; set; }

        /// <summary>
        /// Placa de identificação da moto (deve ser única).
        /// </summary>
        /// <example>XYZ-1234</example>
        [Required]
        public required string Placa { get; set; }

        /// <summary>
        /// Status atual da moto, indicando se está disponível para locação.
        /// </summary>
        public MotoStatus Status { get; set; }

        // Propriedade de navegação para EF Core
        public ICollection<Locacao> Locacoes { get; set; } = new List<Locacao>();
    }
}